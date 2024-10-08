// Importation des espaces de noms nécessaires
//using CustomLibrary.MyCryptography;
using CutOptimization.Verification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

// Espace de noms pour les fonctionnalités d'optimisation de coupe à une dimension
namespace CutOptimization.OneD
{
    /// <summary>
    /// Représente une barre utilisée dans l'optimisation de coupe.
    /// </summary>
    public class Bar
    {
        // Clé statique utilisée pour la génération d'identifiants internes
        private static string SystemIdKeyString = "454d5e018eb08ee22d7f4fa0a403552fdc9acd2e5b7501baec5f3de292bdf0b9";

        // Champs privés représentant les propriétés de la barre
        private decimal _length = 0;
        private decimal _initialFall = 0;

        /// <summary>
        /// Obtient l'identifiant de la barre.
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        /// Obtient ou définit la longueur de la barre.
        /// </summary>
        public decimal Length
        {
            get => _length;
            set
            {
                Verify.ThrowIfLowerThanNumber(value, nameof(Length), 0);
                _length = value;
            }
        }

        /// <summary>
        /// Obtient ou définit la chute initiale de la barre.
        /// </summary>
        public decimal InitialFall
        {
            get => _initialFall;
            set
            {
                Verify.ThrowIfNotBetweenNumber(value, nameof(InitialFall), 0, Length);
                _initialFall = value;
            }
        }

        /// <summary>
        /// Obtient la longueur effective de la barre après la chute initiale.
        /// </summary>
        public decimal EffectiveLength => Length - InitialFall;

        /// <summary>
        /// Obtient ou définit l'identifiant de la barre provenant de la base de données.
        /// </summary>
        public string? DataBaseId { get; set; }

        ///// <summary>
        ///// Obtient l'identifiant interne généré à partir de la longueur effective de la barre.
        ///// </summary>
        //public string InternalId => HashFeatures.GetHashFromString($"${EffectiveLength}$", SystemIdKeyString);
    }
}
