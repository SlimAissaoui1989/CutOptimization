// Importation des espaces de noms nécessaires
using CutOptimization.Verification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Espace de noms pour les fonctionnalités d'optimisation de coupe à une dimension
namespace CutOptimization.OneD
{
    /// <summary>
    /// Représente le résultat d'une coupe sur une barre.
    /// </summary>
    public class CutBarResult
    {
        // Champs privés représentant les propriétés du résultat de coupe
        private decimal _cutDegreeLeft = 90;
        private decimal _cutDegreeRight = 90;
        private decimal _height = 0;
        private decimal _length = 0;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="CutBarResult"/>.
        /// </summary>
        public CutBarResult()
        {
        }

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="CutBarResult"/>.
        /// </summary>
        /// <param name="length">La longueur du résultat de la coupe.</param>
        public CutBarResult(decimal length)
            : this()
        {
            Length = length;
        }

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="CutBarResult"/> avec une longueur et des degrés de coupe spécifiés.
        /// </summary>
        /// <param name="length">La longueur du résultat de la coupe.</param>
        /// <param name="cutDegreeLeft">Les degrés de coupe à gauche.</param>
        /// <param name="cutDegreeRight">Les degrés de coupe à droite.</param>
        public CutBarResult(decimal length, decimal cutDegreeLeft, decimal cutDegreeRight)
            : this(length)
        {
            CutLeftDegree = cutDegreeLeft;
            CutRightDegree = cutDegreeRight;
        }

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="CutBarResult"/>.
        /// </summary>
        /// <param name="length">La longueur du résultat de la coupe.</param>
        /// <param name="cutDegreeLeft">Les degrés de coupe à gauche.</param>
        /// <param name="cutDegreeRight">Les degrés de coupe à droite.</param>
        /// <param name="height">La hauteur du barre.</param>
        public CutBarResult(decimal length, decimal cutDegreeLeft, decimal cutDegreeRight, decimal height)
            : this(length, cutDegreeLeft, cutDegreeRight)
        {
            Height = height;
        }

        /// <summary>
        /// Obtient ou définit l'identifiant du résultat de la coupe.
        /// </summary>
        public int Id { get; internal set; }

        // Méthode privée pour calculer la chute en fonction du degré de coupe
        private decimal GetFallByDegree(decimal degree)
        {
            return Math.Round(Height / (decimal)Math.Tan(double.Parse(degree.ToString())), 3);
        }

        /// <summary>
        /// Obtient la hauteur du barre.
        /// </summary>
        internal decimal Height
        {
            get => _height;
            set
            {
                Verify.ThrowIfLowerThanNumber(value, nameof(Height), 0);
                _height = value;
            }
        }

        /// <summary>
        /// Obtient la chute à gauche en fonction du degré de coupe.
        /// </summary>
        private decimal LeftFallByDegree => GetFallByDegree(CutLeftDegree);

        /// <summary>
        /// Obtient la chute à droite en fonction du degré de coupe.
        /// </summary>
        private decimal RightFallByDegree => GetFallByDegree(CutRightDegree);

        /// <summary>
        /// Obtient la longueur interne du résultat de la coupe après les chutes à gauche et à droite.
        /// </summary>
        public decimal InternalLength => Length - LeftFallByDegree - RightFallByDegree;

        /// <summary>
        /// Obtient ou définit la longueur du résultat de la coupe.
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
        /// Obtient ou définit les degrés de coupe à gauche.
        /// </summary>
        public decimal CutLeftDegree
        {
            get => _cutDegreeLeft;
            private set
            {
                Verify.ThrowIfNotBetweenNumber(value, nameof(CutLeftDegree), 0, 180);
                _cutDegreeLeft = value;
            }
        }

        /// <summary>
        /// Obtient ou définit les degrés de coupe à droite.
        /// </summary>
        public decimal CutRightDegree
        {
            get => _cutDegreeRight;
            private set
            {
                Verify.ThrowIfNotBetweenNumber(value, nameof(CutRightDegree), 0, 180);
                _cutDegreeRight = value;
            }
        }
    }
}
