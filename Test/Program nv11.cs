//using Framework.Payout;

//NV11Connector nV11Connector = new NV11Connector();

//while (true)
//{
//    Console.WriteLine("*****");
//    string r = Console.ReadLine();
//    switch (r.Trim().ToLower())
//    {
//        case "open":
//            nV11Connector.OpenConnection(msg => Console.WriteLine("==> " + msg));
//            break;
//        case "close":
//            nV11Connector.CloseConnection();
//            break;
//        case "exit":
//            return;

//    }
//}

using Test.NewFolder;

Console.WriteLine("1");
Console.WriteLine("2");
await Class1.RunTaskAsync("3");
Console.WriteLine("4");
Task t = Class1.RunTaskAsync("5");
Console.WriteLine("6");
Console.WriteLine("7");
Task t1 = Class1.RunTaskAsync("8");
Task t2 = Class1.RunTaskAsync("9");
Task t3 = Class1.RunTaskAsync("10");
Task t4 = Class1.RunTaskAsync("11");

while (!t.IsCompleted 
    && !t1.IsCompleted
    && !t2.IsCompleted
    && !t3.IsCompleted
    && !t4.IsCompleted);
Console.WriteLine("12");
