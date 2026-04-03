using System;
using System.Collections.Generic;
using System.Text;

namespace USRepositories
{
    static internal class RepoInnerServices
    {
        public static Predicate<string> CharController(int Limit, bool Greather)
        {
            return t => Greather ? t.Length >= Limit : t.Length <= Limit;
        }
    }
}
