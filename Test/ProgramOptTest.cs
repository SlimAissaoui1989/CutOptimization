using CustomLibrary.MyCryptography;
using CutOptimization.OneD;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;


//Représente les informations technique d'une barre.
//BarInfo barInfo = new BarInfo()
//{
//    // Hauteur
//    Height = 220,
//    //Largeur
//    Weight = 60,
//};

BarInfo barInfo = new BarInfo();

// Dimension d'une barre standard utilisée si les barre den stock sont consommé.
Bar standardBar = new Bar()
{
    //Longueur de la barre.
    Length = 6500,

    //Chute initiale de la barre
    InitialFall = 25
};

//Initialise une nouvelle instance de la classe coupe responsable de la gestion des coupes et de l'optimisation. 
//5 : l'epaisseur de la lame
Cut cut = new Cut(barInfo, standardBar, 5);

//Chute minimale récupérable.
cut.MinRecoveredFall = 900;

// Chute maximale non récupérable.
// Ce paramètre indique que les chutes acceptées sont inférieures à MaxFallNonRecoverable et supérieures à MinRecoveredFall.
//cut.MaxFallNonRecoverable = 300;

//Ajout d'une barre.
//cut.Bars.Add(new Bar { Length = 4600 });

//Ajout d'une collection de 10 barres.
//cut.Bars.AddRange(Enumerable.Repeat(new Bar { Length = 6500, InitialFall = 25 }, 10));

////Ajout une resultat d'une coupe.
//cut.CutBarResults.Add(new CutBarResult(4500));



cut.CutBarResults.AddRange(Enumerable.Repeat(new CutBarResult(4500), 5));
cut.CutBarResults.AddRange(Enumerable.Repeat(new CutBarResult(1800), 5));
cut.CutBarResults.AddRange(Enumerable.Repeat(new CutBarResult(3100), 5));
cut.CutBarResults.AddRange(Enumerable.Repeat(new CutBarResult(2200), 5));
cut.CutBarResults.AddRange(Enumerable.Repeat(new CutBarResult(900), 5));
cut.CutBarResults.AddRange(Enumerable.Repeat(new CutBarResult(1200), 5));


////Ajout une collection de 230 resultats de coupe.
//cut.CutBarResults.AddRange(Enumerable.Repeat(new CutBarResult(4500), 230));

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


//public static List<decimal> LengthBarsResults_1 => Enumerable.Repeat(4500m, 5)
//                                                                .Concat(Enumerable.Repeat(1800m, 5))
//                                                                .Concat(Enumerable.Repeat(3100m, 5))
//                                                                .Concat(Enumerable.Repeat(2200m, 5))
//                                                                .Concat(Enumerable.Repeat(900m, 5))
//                                                                .Concat(Enumerable.Repeat(1200m, 5)).ToList();
//public static List<decimal> LengthBarsResults_2 => Enumerable.Repeat(2200m, 45)
//                                                            .Concat(Enumerable.Repeat(1200m, 65))
//                                                            .Concat(Enumerable.Repeat(900m, 35))
//                                                            .Concat(Enumerable.Repeat(3100m, 45))
//                                                            .Concat(Enumerable.Repeat(4500m, 45))
//                                                            .Concat(Enumerable.Repeat(1800m, 65)).ToList();
//public static List<decimal> LengthBarsResults_3 => Enumerable.Repeat(3100m, 230)
//                                                            .Concat(Enumerable.Repeat(2200m, 230))
//                                                            .Concat(Enumerable.Repeat(1800m, 325))
//                                                            .Concat(Enumerable.Repeat(1200m, 325))
//                                                            .Concat(Enumerable.Repeat(4500m, 230))
//                                                            .Concat(Enumerable.Repeat(900m, 170)).ToList();
//public static List<decimal> LengthBarsResults_4 => Enumerable.Repeat(3100m, 5)
//                                                            .Concat(Enumerable.Repeat(1800m, 5))
//                                                            .Concat(Enumerable.Repeat(1200m, 5))
//                                                            .Concat(Enumerable.Repeat(4500m, 5))
//                                                            .Concat(Enumerable.Repeat(900m, 316)).ToList();
//public static List<decimal> LengthBarsResults_5 => Enumerable.Repeat(3100m, 45)
//                                           .Concat(Enumerable.Repeat(1800m, 65))
//                                           .Concat(Enumerable.Repeat(1200m, 65))
//                                           .Concat(Enumerable.Repeat(4500m, 45))
//                                           .Concat(Enumerable.Repeat(900m, 2480)).ToList();
//public static List<decimal> LengthBarsResults_6 => Enumerable.Repeat(3100m, 230)
//                                           .Concat(Enumerable.Repeat(1800m, 325))
//                                           .Concat(Enumerable.Repeat(1200m, 325))
//                                           .Concat(Enumerable.Repeat(4500m, 230))
//                                           .Concat(Enumerable.Repeat(900m, 12320)).ToList();