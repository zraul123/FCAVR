using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FCA.Repository
{
    class WeekComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            int intInX = int.Parse(Regex.Match(x, @"\d+").Value);
            int intInY = int.Parse(Regex.Match(y, @"\d+").Value);
            return intInX - intInY;
        }
    }
}
