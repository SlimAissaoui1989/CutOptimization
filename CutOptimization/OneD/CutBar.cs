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
    /// Représente une barre coupée dans le cadre de l'optimisation de coupe.
    /// </summary>
    public class CutBar
    {
        // Champs privés représentant les propriétés de la barre coupée
        private Bar? _bar = null;
        private decimal _barLength = 0;
        private decimal _bladeThickness = 0;
        private decimal _initFall = 0;

        // Constructeur privé utilisé pour initialiser la barre coupée à partir d'informations de barre et d'épaisseur de lame
        private CutBar(BarInfo barInfo, decimal bladeThickness)
        {
            this.BarInfo = barInfo;
            CutBarResults = new CutBarResults(barInfo);
            BladeThickness = bladeThickness;
        }
        
        // Constructeur public utilisé pour créer une barre coupée à partir d'une barre existante
        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="CutBar"/>.
        /// </summary>
        /// <param name="bar">La barre a coupée.</param>
        /// <param name="bladeThickness">L'épaisseur de la lame de coupe.</param>
        /// <param name="barInfo">Les informations sur la barre a coupée.</param>
        public CutBar(Bar bar, decimal bladeThickness, BarInfo barInfo)
            : this(barInfo, bladeThickness)
        {
            Bar = bar;
        }

        // Constructeur public utilisé pour créer une barre coupée à partir de longueur, chute initiale, épaisseur de lame et informations de barre
        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="CutBar"/>.
        /// </summary>
        /// <param name="barLength">La longueur totale de la barre a coupée.</param>
        /// <param name="initFall">La chute initiale de la barre a coupée.</param>
        /// <param name="bladeThickness">L'épaisseur de la lame de coupe.</param>
        /// <param name="barInfo">Les informations sur la barre a coupée.</param>
        public CutBar(decimal barLength, decimal initFall, decimal bladeThickness, BarInfo barInfo)
            : this(barInfo, bladeThickness)
        {
            BarLength = barLength;
            InitFall = initFall;
        }

        /// <summary>
        /// Obtient ou définit la longueur totale de la barre a coupée.
        /// </summary>
        public decimal BarLength
        {
            get => _barLength;
            set
            {
                Verify.ThrowIfLowerThanNumber(value, nameof(BarLength), 0);
                _barLength = value;
            }
        }

        /// <summary>
        /// Obtient ou définit la chute initiale de la barre a coupée.
        /// </summary>
        public decimal InitFall
        {
            get => _initFall;
            set
            {
                Verify.ThrowIfLowerThanNumber(value, nameof(InitFall), 0);
                _initFall = value;
            }
        }

        /// <summary>
        /// Obtient la longueur effective de la barre a coupée après la chute initiale.
        /// </summary>
        public decimal BarEffectiveLength => BarLength - InitFall;

        /// <summary>
        /// Obtient ou définit la barre a coupée.
        /// </summary>
        public Bar? Bar
        {
            get => _bar;
            set
            {
                _bar = value;
                if (value != null)
                {
                    BarLength = value.Length;
                    InitFall = value.InitialFall;
                }
            }
        }

        /// <summary>
        /// Obtient ou définit les informations sur la barre a coupée.
        /// </summary>
        public BarInfo BarInfo { get; set; }

        /// <summary>
        /// Obtient ou définit les résultats de la coupe.
        /// </summary>
        public CutBarResults CutBarResults { get; set; }

        /// <summary>
        /// Obtient ou définit l'épaisseur de la lame de coupe avec.
        /// </summary>
        public decimal BladeThickness
        {
            get => _bladeThickness;
            set
            {
                Verify.ThrowIfLowerThanNumber(value, nameof(BladeThickness), 0);
                _bladeThickness = value;
            }
        }

        /// <summary>
        /// Obtient la chute totale de la barre coupée.
        /// </summary>
        public decimal TotalFall => BarLength - CutBarResults.Sum(s => s.Length);

        /// <summary>
        /// Obtient le pourcentage de chute totale de la barre coupée.
        /// </summary>
        public decimal FallPercent => Math.Round((1 - (CutBarResults.Sum(s => s.Length) / BarLength)) * 100, 3);

        /// <summary>
        /// Obtient la chute due à l'épaisseur de la lame de coupe.
        /// </summary>
        public decimal BladeFall => CutBarResults.Count * BladeThickness;

        /// <summary>
        /// Obtient chute restant aprés le coupe.
        /// </summary>
        public decimal Fall => BarEffectiveLength - CutBarResults.Sum(s => s.Length) - BladeFall;

        /// <summary>
        /// Crée une copie de la barre coupée.
        /// </summary>
        /// <returns>Une copie de la barre coupée.</returns>
        internal CutBar Clone()
        {
            CutBar cutBar = new CutBar(BarInfo, BladeThickness)
            {
                BarLength = BarLength,
                InitFall = InitFall,
                Bar = Bar,
                CutBarResults = new CutBarResults(BarInfo, CutBarResults),
            };
            return cutBar;
        }

        /// <summary>
        /// Retourne une représentation textuelle de la barre coupée.
        /// </summary>
        /// <returns>Une chaîne représentant la barre coupée.</returns>
        public override string ToString()
        {
            return $"bar longueur({BarLength}) : ({string.Join(", ", CutBarResults.Select(s => s.Length))}) chuts({Fall})";
        }
    }
}
