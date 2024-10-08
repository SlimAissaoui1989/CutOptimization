// Importation des espaces de noms nécessaires
using CutOptimization.Verification;
using System;

// Espace de noms pour les fonctionnalités d'optimisation de coupe à une dimension
namespace CutOptimization.OneD
{
    /// <summary>
    /// Représente les informations sur une barre utilisée dans l'optimisation de coupe.
    /// </summary>
    public class BarInfo
    {
        // Champs privés représentant les dimensions de la barre
        private decimal _height = 0;
        private decimal _width = 0;
        private decimal _weight = 0;
        private decimal _volume = 0;

        /// <summary>
        /// Obtient ou définit l'identifiant de la barre provenant de la base de données.
        /// </summary>
        public object? DataBaseId { get; set; }

        /// <summary>
        /// Obtient ou définit la hauteur de la barre par unité.
        /// </summary>
        public decimal Height
        {
            get => _height;
            set
            {
                // Vérification si la hauteur est inférieure à zéro
                Verify.ThrowIfLowerThanNumber(value, nameof(Height), 0);
                _height = value;
            }
        }

        /// <summary>
        /// Obtient ou définit la largeur de la barre par unité.
        /// </summary>
        public decimal Width
        {
            get => _width;
            set
            {
                // Vérification si la largeur est inférieure à zéro
                Verify.ThrowIfLowerThanNumber(value, nameof(Width), 0);
                _width = value;
            }
        }

        /// <summary>
        /// Obtient ou définit le poids de la barre par unité.
        /// </summary>
        public decimal Weight
        {
            get => _weight;
            set
            {
                // Vérification si le poids est inférieur à zéro
                Verify.ThrowIfLowerThanNumber(value, nameof(Weight), 0);
                _weight = value;
            }
        }

        /// <summary>
        /// Obtient ou définit le volume de la barre par unité.
        /// </summary>
        public decimal Volume
        {
            get => _volume;
            set
            {
                // Vérification si le volume est inférieur à zéro
                Verify.ThrowIfLowerThanNumber(value, nameof(Volume), 0);
                _volume = value;
            }
        }
    }
}
