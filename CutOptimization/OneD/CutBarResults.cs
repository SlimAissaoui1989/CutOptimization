// Importation des espaces de noms nécessaires
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Espace de noms pour les fonctionnalités d'optimisation de coupe à une dimension
namespace CutOptimization.OneD
{
    /// <summary>
    /// Représente une liste de résultats de coupe pour une barre spécifique.
    /// </summary>
    public class CutBarResults : List<CutBarResult>
    {
        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="CutBarResults"/> avec des informations sur la barre associée.
        /// </summary>
        /// <param name="barInfo">Les informations sur la barre associée.</param>
        public CutBarResults(BarInfo barInfo) : base()
        {
            BarInfo = barInfo;
        }

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="CutBarResults"/> avec des informations sur la barre associée et une collection existante de résultats de coupe.
        /// </summary>
        /// <param name="barInfo">Les informations sur la barre associée.</param>
        /// <param name="cutBarResults">La collection existante de résultats de coupe à ajouter à la liste.</param>
        public CutBarResults(BarInfo barInfo, IEnumerable<CutBarResult> cutBarResults)
            : this(barInfo)
        {
            AddRange(cutBarResults);
        }

        /// <summary>
        /// Obtient ou définit les informations sur la barre associée à ces résultats de coupe.
        /// </summary>
        public BarInfo BarInfo { get; set; }

        /// <summary>
        /// Ajoute un résultat de coupe à la liste.
        /// </summary>
        /// <param name="cutBarResult">Le résultat de coupe à ajouter à la liste.</param>
        public new void Add(CutBarResult cutBarResult)
        {
            // Vérifie si le résultat de coupe n'est pas nul
            if (cutBarResult == null)
                throw new NullReferenceException("cutBarResult cannot be null");

            // Attribue un identifiant unique au résultat de coupe basé sur la position dans la liste
            cutBarResult.Id = Count + 1;

            // Définit la hauteur du résultat de coupe en fonction des informations sur la barre associée
            cutBarResult.Height = BarInfo.Height;

            // Ajoute le résultat de coupe à la liste de base
            base.Add(cutBarResult);
        }

        /// <summary>
        /// Ajoute une collection de résultats de coupe à la liste.
        /// </summary>
        /// <param name="cutBarResults">La collection de résultats de coupe à ajouter à la liste.</param>
        public new void AddRange(IEnumerable<CutBarResult> cutBarResults)
        {
            // Ajoute chaque résultat de coupe de la collection à la liste
            for (int i = 0; i < cutBarResults.Count(); i++) Add(cutBarResults.ElementAt(i));
        }

        /// <summary>
        /// Retourne une représentation textuelle des longueurs des résultats de coupe séparées par des virgules.
        /// </summary>
        /// <returns>Une chaîne représentant les longueurs des résultats de coupe.</returns>
        public override string ToString()
        {
            return string.Join(", ", this.Select(s => s.Length));
        }
    }
}
