
// Importation des espaces de noms nécessaires
using CutOptimization.Verification;

// Espace de noms pour les fonctionnalités d'optimisation de coupe à une dimension
namespace CutOptimization.OneD
{
    /// <summary>
    /// Classe responsable de la gestion des coupes et de l'optimisation.
    /// </summary>
    public class Cut
    {
        // Champs privés pour les propriétés de la classe
        private decimal _bladeThickness = 0;
        private decimal _minRecovredFall = 0;
        private decimal _maxFallNonRecoverable = 0;

        /// <summary>
        /// Initialise une nouvelle instance de la classe Cut.
        /// </summary>
        /// <param name="barInfo">Informations sur la barre.</param>
        public Cut(BarInfo barInfo)
        {
            // Initialisation des champs et propriétés
            Bars = new Bars();
            BarInfo = barInfo;
            CutBarResults = new CutBarResults(barInfo);
        }

        /// <summary>
        /// Initialise une nouvelle instance de la classe Cut avec une barre standard et une épaisseur de lame spécifiées.
        /// </summary>
        /// <param name="barInfo">Informations sur la barre.</param>
        /// <param name="standardBar">Barre standard.</param>
        /// <param name="bladeThickness">Épaisseur de la lame.</param>
        public Cut(BarInfo barInfo, Bar standardBar, decimal bladeThickness) :
            this(barInfo)
        {
            StandardBar = standardBar;
            BladeThickness = bladeThickness;
        }

        /// <summary>
        /// Barre standard utilisée si les barre de stock sont consommé.
        /// </summary>
        public Bar? StandardBar { get; set; }

        /// <summary>
        /// Épaisseur de la lame utilisée pour les coupes.
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
        /// Chute minimale récupérable.
        /// </summary>
        public decimal MinRecoveredFall
        {
            get => _minRecovredFall;
            set
            {
                Verify.ThrowIfLowerThanNumber(value, nameof(MinRecoveredFall), 0);
                _minRecovredFall = value;
            }
        }

        /// <summary>
        /// Chute maximale non récupérable.
        /// Ce paramètre indique que les chutes acceptées sont inférieures à MaxFallNonRecoverable et supérieures à MinRecoveredFall
        /// </summary>
        public decimal MaxFallNonRecoverable
        {
            get => _maxFallNonRecoverable;
            set
            {
                Verify.ThrowIfLowerThanNumber(value, nameof(MaxFallNonRecoverable), 0);
                _maxFallNonRecoverable = value;
            }
        }

        /// <summary>
        /// Informations sur la barre utilisées.
        /// </summary>
        public BarInfo BarInfo { get; }

        /// <summary>
        /// Liste des barres utilisées pour les coupes.
        /// </summary>
        public Bars Bars { get; }

        /// <summary>
        /// Résultats des coupes a acquérir.
        /// </summary>
        public CutBarResults CutBarResults { get; }

        /// <summary>
        /// Méthode pour minimiser les valeurs de combinaison.
        /// </summary>
        private CutBarResults MinimiseCombinationValues(Bar bar, CutBarResults cutBarResults)
        {
            IEnumerable<CutBarResult> cutBarResultsDistincted = cutBarResults.DistinctBy(d => d.Length);
            CutBarResults result = new CutBarResults(BarInfo);
            foreach (CutBarResult cutBarResult in cutBarResultsDistincted)
            {
                IEnumerable<CutBarResult> LengthBarsResultsPossible = cutBarResults.Where(c => c.Length == cutBarResult.Length);
                decimal sum = 0;
                foreach (CutBarResult cut in LengthBarsResultsPossible)
                {
                    sum += cut.Length;
                    if (sum <= bar.EffectiveLength)
                    {
                        result.Add(cut);
                        sum += BladeThickness;
                    }
                    else break;
                }
            }
            return result;
        }

        /// <summary>
        /// Méthode pour obtenir les combinaisons dans une seule barre.
        /// </summary>
        private List<CutBar> GetCombinationInOneBar(Bars bars, CutBarResults cutBarResults)
        {
            List<CutBar> result = new List<CutBar>();
            IEnumerable<Bar> myBars = bars.DistinctBy(d => d.EffectiveLength);
            if (myBars.Count() == 0 && StandardBar != null)
                myBars = myBars.Concat(new List<Bar> { StandardBar });
            foreach (Bar bar in myBars)
            {
                CutBarResults LengthBarsResultsMinimized = MinimiseCombinationValues(bar, cutBarResults);
                CutBar cutbar = new CutBar(bar.Length, bar.InitialFall, BladeThickness, BarInfo);
                GetCombinationInOneBarHelper(result, cutbar, LengthBarsResultsMinimized);
            }
            return result.Where(c => c.CutBarResults.Count > 0).DistinctBy(d => d.ToString()).OrderBy(o => o.TotalFall).ToList();
        }

        /// <summary>
        /// Méthode auxiliaire pour obtenir les combinaisons dans une seule barre.
        /// </summary>
        private void GetCombinationInOneBarHelper(List<CutBar> result,
                                                    CutBar cutbar,
                                                    IEnumerable<CutBarResult> cutBarResults)
        {

            bool @continue = true;

            decimal lastFall = cutbar.Fall;
            if (!result.Any(c => c.ToString() == cutbar.ToString()))
            {
                if (lastFall >= -BladeThickness)
                {
                    result.Add(cutbar);
                }
                else @continue = false;

                if (@continue)
                    for (int i = 0; i < cutBarResults.Count(); i++)
                    {
                        CutBarResult cut = cutBarResults.ElementAt(i);
                        CutBar newCutBar = cutbar.Clone();
                        newCutBar.CutBarResults.Add(cut);
                        newCutBar.CutBarResults = new CutBarResults(newCutBar.BarInfo, newCutBar.CutBarResults.OrderByDescending(o => o.Length));

                        IEnumerable<CutBarResult> cutBarResultsCloned = new List<CutBarResult>(cutBarResults.Skip(i + 1));

                        GetCombinationInOneBarHelper(result, newCutBar, cutBarResultsCloned);
                    }
            }
            cutbar.CutBarResults = new CutBarResults(BarInfo, cutbar.CutBarResults.OrderByDescending(o => o.Length));

        }

        /// <summary>
        /// Méthode pour obtenir les barres coupées pour une combinaison.
        /// </summary>
        private CutBars GetCutBarsForCombination(CutBar cutBar, CutBarResults cutBarResults, Bars bars)
        {
            CutBars result = new CutBars(MinRecoveredFall);
            List<Bar> myBars = bars.Where(c => c.EffectiveLength == cutBar.BarEffectiveLength).ToList();

            while ((myBars.Count > 0 || cutBar.BarLength == StandardBar?.Length) && cutBarResults.Count >= cutBar.CutBarResults.Count)
            {
                List<CutBarResult> cutBarResultsRemoved = new List<CutBarResult>();
                foreach (CutBarResult cutBarResult in cutBar.CutBarResults)
                {
                    CutBarResult? cbrInCutcbr = cutBarResults.FirstOrDefault(c => cutBarResult.Length == c.Length);
                    if (cbrInCutcbr != null)
                    {
                        cutBarResults.Remove(cbrInCutcbr);
                        cutBarResultsRemoved.Add(cbrInCutcbr);
                    }
                    else
                    {
                        cutBarResults.AddRange(cutBarResultsRemoved);
                        cutBarResultsRemoved.Clear();
                        break;
                    }
                }
                if (cutBarResultsRemoved.Count == 0)
                    break;
                else
                {
                    Bar? bar = bars.FirstOrDefault(c => c.Length == cutBar.BarLength);
                    if (bar != null)
                    {
                        myBars.Remove(bar);
                        bars.Remove(bar);
                    }
                    result.Add(cutBar);
                }

            }
            return result;
        }

        /// <summary>
        /// Méthode auxiliaire pour obtenir les combinaisons pour toutes les coupes.
        /// </summary>
        private void GetCombinationForAllCutsHelper(List<CutBars> result, IEnumerable<CutBar> combinationsInOneBar, CutBarResults cutBarResults, Bars bars, CutBars? currentCutBar = null)
        {
            currentCutBar = currentCutBar ?? new CutBars(MinRecoveredFall);

            if (result.Count > 0)
            {
                decimal minFall = result.Min(m => m.TotalFallLength);
                if (minFall < currentCutBar.TotalFallLength)
                    return;
            }

            if (cutBarResults.Count == 0 || combinationsInOneBar.Count() == 0)
            {
                result.Add(currentCutBar);
                return;
            }

            for (int i = 0; i < combinationsInOneBar.Count(); i++)
            {
                CutBar cutBar = combinationsInOneBar.ElementAt(i);
                CutBarResults myCutBarResults = new CutBarResults(BarInfo, cutBarResults);
                Bars myBars = new Bars(bars);
                CutBars myCutBars = new CutBars(MinRecoveredFall, currentCutBar);
                myCutBars.AddRange(GetCutBarsForCombination(cutBar, myCutBarResults, myBars));
                IEnumerable<CutBar> myCombinationsInOneBarToNextStep = combinationsInOneBar.Where(c => c != cutBar); //GetCombinationInOneBar(myBars, myCutBarResults);
                GetCombinationForAllCutsHelper(result, myCombinationsInOneBarToNextStep, myCutBarResults, myBars, myCutBars);
            }

        }

        /// <summary>
        /// Méthode pour obtenir les combinaisons pour toutes les coupes.
        /// </summary>
        private List<CutBars> GetCombinationForAllCuts(IEnumerable<CutBar> combinationsInOneBar, CutBarResults cutBarResults, Bars bars)
        {
            List<CutBars> result = new List<CutBars>();
            GetCombinationForAllCutsHelper(result, combinationsInOneBar, cutBarResults, bars);
            return result;
        }

        /// <summary>
        /// Méthode pour attribuer une barre aux barres coupées.
        /// </summary>
        private void AssignBarToCutBars(CutBars cutBars)
        {
            foreach (CutBar cutBar in cutBars)
            {
                Bar? bar = Bars.FirstOrDefault(c => c.EffectiveLength == cutBar.BarEffectiveLength);
                if (bar != null)
                    cutBar.Bar = bar;
            }
        }

        /// <summary>
        /// Optimise les coupes en recherchant les combinaisons les plus efficaces.
        /// </summary>
        /// <returns>Les résultats optimisés des coupes.</returns>
        public CutBars OptimizeCut()
        {
            CutBars result = new CutBars(MinRecoveredFall);

            CutBarResults myCutBarResults = new CutBarResults(BarInfo, CutBarResults);
            Bars myBars = new Bars(Bars);

            while (myCutBarResults.Count > 0)
            {
                IEnumerable<CutBar> combinationsInOneBar = GetCombinationInOneBar(myBars, myCutBarResults);
                IEnumerable<decimal> cutResultsLengths = myCutBarResults.Select(s => s.Length).Distinct();
                List<CutBar> cutBarsSelection = new List<CutBar>();
                for (int i = 0; i < cutResultsLengths.Count(); i++)
                {
                    decimal cut = cutResultsLengths.ElementAt(i);

                    if (MaxFallNonRecoverable != 0 && MinRecoveredFall != 0)
                    {
                        IEnumerable<CutBar> myCombinationsInOneBar = combinationsInOneBar.Where(c => c.Fall < MaxFallNonRecoverable || c.Fall >= MinRecoveredFall);
                        if (myCombinationsInOneBar.Count() > 0)
                            combinationsInOneBar = myCombinationsInOneBar;
                    }

                    IEnumerable<CutBar> cutBarsContainCut = combinationsInOneBar.Where(c => c.CutBarResults.Select(s => s.Length).Contains(cut));
                    CutBar cutBarContainCut = cutBarsContainCut.OrderBy(o => o.TotalFall).First();
                    cutBarsSelection.Add(cutBarContainCut);
                }

                List<CutBars> listCombination = GetCombinationForAllCuts(cutBarsSelection.Distinct(), myCutBarResults, myBars);

                CutBars cutBars = listCombination.MinBy(m => m.TotalFallLength / m.TotalBarCount) ?? throw new Exception();
                result.AddRange(cutBars);

                foreach (CutBar cutBar in cutBars)
                {
                    foreach (CutBarResult cutBarResult in cutBar.CutBarResults)
                    {
                        CutBarResult toRemove = myCutBarResults.First(c => c.Length == cutBarResult.Length);
                        myCutBarResults.Remove(toRemove);
                    }
                    Bar? bar = myBars.FirstOrDefault(c => c.Length == cutBar.BarLength);
                    if (bar != null)
                        myBars.Remove(bar);
                }
            }
            AssignBarToCutBars(result);
            return result;
        }
    }
}
