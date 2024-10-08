using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.NewFolder
{
    internal static class Class1
    {
        public static async Task RunTaskAsync(string msg)
        {
            await Task.Delay(3000);
            Console.WriteLine(msg);
        }
    }
}
