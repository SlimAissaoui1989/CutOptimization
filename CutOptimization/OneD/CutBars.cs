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
    /// Représente une liste de barres coupées.
    /// </summary>
    public class CutBars : List<CutBar>
    {
        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="CutBars"/> avec une chute minimale récupérée spécifiée.
        /// </summary>
        /// <param name="minRecoveredFall">La chute minimale récupérée spécifiée.</param>
        public CutBars(decimal minRecoveredFall) : this(minRecoveredFall, null)
        {
        }

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="CutBars"/> avec une chute minimale récupérée spécifiée et une collection existante de barres coupées.
        /// </summary>
        /// <param name="minRecoveredFall">La chute minimale récupérée spécifiée.</param>
        /// <param name="cutBars">La collection existante de barres coupées à ajouter à la liste.</param>
        public CutBars(decimal minRecoveredFall, IEnumerable<CutBar>? cutBars) :
            base(new Func<IEnumerable<CutBar>>(() => {
                if (cutBars == null)
                    return new List<CutBar>();
                return cutBars;
            }).Invoke())
        {
            MinRecoveredFall = minRecoveredFall;
        }

        /// <summary>
        /// Obtient ou définit la chute minimale a récupérée.
        /// </summary>
        private decimal MinRecoveredFall { get; set; }

        /// <summary>
        /// Obtient la longueur totale des résultats de coupe sur toutes les barres coupées.
        /// </summary>
        public decimal TotalBarResultLength => this.Sum(s => s.CutBarResults.Sum(s1 => s1.Length));

        /// <summary>
        /// Obtient le nombre total des coupes sur toutes les barres coupées.
        /// </summary>
        public int TotalBarResultCount => this.Sum(s => s.CutBarResults.Count);

        /// <summary>
        /// Obtient la longueur totale de toutes les barres coupées.
        /// </summary>
        public decimal TotalBarLength => this.Sum(s => s.BarLength);

        /// <summary>
        /// Obtient le nombre total de barres coupées.
        /// </summary>
        public int TotalBarCount => this.Count;

        /// <summary>
        /// Obtient le nombre de barres utilisées mais non stockées.
        /// </summary>
        public decimal BarUsedButNotStocked => this.Count(c => c.Bar == null);

        /// <summary>
        /// Obtient la longueur totale de toutes les chutes a récupérées.
        /// </summary>
        public decimal TotalFallLength => this.Sum(s => s.TotalFall);

        /// <summary>
        /// Obtient la longueur totale des chutes en pourcentage.
        /// </summary>
        public decimal TotalFallByPercent => Math.Round((TotalFallLength / TotalBarLength) * 100, 3);

        /// <summary>
        /// Obtient les barres coupées qui ont une chute récupérée supérieure ou égale à la chute minimale a récupérée.
        /// </summary>
        private IEnumerable<CutBar> CutBarsWithRecoveredFall => this.Where(c => c.Fall >= MinRecoveredFall);

        /// <summary>
        /// Obtient la longueur totale des chutes a récupérées.
        /// </summary>
        public decimal TotalRecoveredFallLength => CutBarsWithRecoveredFall.Sum(s => s.Fall);

        /// <summary>
        /// Obtient le nombre total de barres coupées avec une chute récupérée supérieure ou égale à la chute minimale a récupérée.
        /// </summary>
        public decimal TotalRecoveredFallBar => CutBarsWithRecoveredFall.Count();

        /// <summary>
        /// Obtient la longueur totale des chutes non récupérées.
        /// </summary>
        public decimal TotalNonRecoveredLength => this.Sum(s => s.BarLength - s.CutBarResults.Sum(s1 => s1.Length) - (s.Fall < MinRecoveredFall ? 0 : s.Fall));

        /// <summary>
        /// Obtient la longueur totale des chutes non récupérées en pourcentage.
        /// </summary>
        public decimal TotalNonRecoveredByPercent => Math.Round((TotalNonRecoveredLength / TotalBarLength) * 100, 3);
    }
}
