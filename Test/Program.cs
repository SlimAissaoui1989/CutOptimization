
//ParameterExpression x = Expression.Parameter(typeof(int), "x");
//LambdaExpression e = DynamicExpressionParser.ParseLambda(new ParameterExpression[] { x }, typeof(bool), "x>0&&x<10");
//var y = new List<int> { 1, 2, 3, 4, 11, 12, 1223, 22256 }.Where(w=> (bool)e.Compile().DynamicInvoke(w)!);
//.Dynamic.DynamicExpression.ParseLambda(typeof(int), typeof(bool), "x=>x>0||x<10", new List<int> { 1, 2, 3, 4, 11, 12, 1223, 22256 });
//Représente les informations technique d'une barre.
using CutOptimization.OneD;

BarInfo barInfo = new BarInfo()
    {
        // Hauteur
        Height = 220,
        //Largeur
        Weight = 60,
    };

    // Dimension d'une barre standard utilisée si les barre den stock sont consommé.
    Bar standardBar = new Bar()
    {
        //Longueur de la barre.
        Length = 6.5m,

        //Chute initiale de la barre
        InitialFall = 0.025m
    };

    //Initialise une nouvelle instance de la classe coupe responsable de la gestion des coupes et de l'optimisation. 
    //5 : l'epaisseur de la lame
    Cut cut = new Cut(barInfo, standardBar, 0.005m);

    //Chute minimale récupérable.
    //cut.MinRecoveredFall = 0.8m;

    // Chute maximale non récupérable.
    // Ce paramètre indique que les chutes acceptées sont inférieures à MaxFallNonRecoverable et supérieures à MinRecoveredFall.
    //cut.MaxFallNonRecoverable = 0.5m;

    //Ajout d'une barre.
    cut.Bars.Add(new Bar { Length = 3.175m, InitialFall = 0.025m });
    cut.Bars.Add(new Bar { Length = 1.395m, InitialFall = 0.025m });
    cut.Bars.Add(new Bar { Length = 0.775m, InitialFall = 0.025m });
    cut.Bars.Add(new Bar { Length = 1.805m, InitialFall = 0.025m });
    cut.Bars.Add(new Bar { Length = 1.905m, InitialFall = 0.025m });
    cut.Bars.Add(new Bar { Length = 4.425m, InitialFall = 0.025m });
    cut.Bars.Add(new Bar { Length = 3.5m });
    cut.Bars.Add(new Bar { Length = 4m });
    cut.Bars.Add(new Bar { Length = 4m });
    cut.Bars.Add(new Bar { Length = 4.5m });
    cut.Bars.Add(new Bar { Length = 4.6m });
    cut.Bars.Add(new Bar { Length = 6m });

    //Ajout d'une collection de 10 barres.
    //cut.Bars.AddRange(Enumerable.Repeat(new Bar { Length = 6500, InitialFall = 25 }, 10));

    //Ajout une resultat d'une coupe.
    //cut.CutBarResults.Add(new CutBarResult(1.2m));

    //Ajout une collection de 230 resultats de coupe.
    cut.CutBarResults.AddRange(Enumerable.Repeat(new CutBarResult(1.265m), 29));
    cut.CutBarResults.AddRange(Enumerable.Repeat(new CutBarResult(1.260m), 28));
    cut.CutBarResults.AddRange(Enumerable.Repeat(new CutBarResult(0.903m), 29));

    cut.CutBarResults.AddRange(new List<CutBarResult>
        {
            new CutBarResult{ Length = 1.335m},
            new CutBarResult{ Length = 1.330m},
            new CutBarResult{ Length = 0.973m},
            new CutBarResult{ Length = 1.735m},
            new CutBarResult{ Length = 1.735m},
            new CutBarResult{ Length = 1.795m},
            new CutBarResult{ Length = 2.070m},
            new CutBarResult{ Length = 2.070m},
        });

    //Executer l'opptimisation
    var cuts = cut.OptimizeCut();

    var cutGrouped = cuts.GroupBy(g => g.CutBarResults.ToString());

    foreach (var cg in cutGrouped)
    {
        var cutGroupedByBarLength = cg.GroupBy(g => g.BarLength);
        foreach (var cgl in cutGroupedByBarLength)
            Console.WriteLine($"{cgl.Count()} x bare({cgl.First().BarLength}) : ({cgl.First().CutBarResults}) chut({cgl.First().Fall})[{cgl.First().FallPercent}%]");
    }



    Console.WriteLine($"\n\nlongueur des coupes: {cuts.TotalBarResultLength}");
    Console.WriteLine($"\nnombre des coupes : {cuts.TotalBarResultCount}");

    Console.WriteLine($"\nLongueur des barres utilise: {cuts.TotalBarLength}");
    Console.WriteLine($"\n\nnombre des barres utilise: {cuts.TotalBarCount}");
    Console.WriteLine($"\nnombre du barre utilisé mais non stocker {cuts.BarUsedButNotStocked}");

    Console.WriteLine($"\nchut totale : longueure ({cuts.TotalFallLength} mm), en %({cuts.TotalFallByPercent} %)");

    Console.WriteLine($"\nchut non recuperable :  longueur ({cuts.TotalNonRecoveredLength} mm), en % ({cuts.TotalNonRecoveredByPercent} %)");
    Console.WriteLine($"\nchut recuperable : longueur ({cuts.TotalRecoveredFallLength} mm), nombre de barre ({cuts.TotalRecoveredFallBar})");



    //Console.WriteLine($"\n*********************");
    //Console.WriteLine($"\n*********************");
    //Console.WriteLine($"\nnombre d'itteration: ({cut.TotalIterationsNumberForAllPossibleCuts})");
