using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CutOptimization.Verification
{
    internal static class Verify
    {
        internal static void ThrowIfLowerThanNumber(decimal value, string valueName, decimal limitNumber)
        {
            if (value < limitNumber)
                throw new ArgumentException($"'{valueName}' must be great than '{limitNumber}'");
        }
        internal static void ThrowIfNotBetweenNumber(decimal value, string valueName, decimal limitNumberLess, decimal limitNumberGreat)
        {
            if (value < limitNumberLess || value > limitNumberGreat)
                throw new ArgumentException($"'{valueName}' must be in ['{limitNumberLess}'..'{limitNumberGreat}']");
        }
    }
}
