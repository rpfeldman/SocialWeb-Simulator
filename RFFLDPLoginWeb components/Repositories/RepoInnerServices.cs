using System;
using System.Collections.Generic;
using System.Text;

namespace USRepositories
{
    static internal class RepoInnerServices
    {
        /// <summary>
        /// Returns a predicate that validates a string's length against a provided limit.
        /// Use this to prevent inputting more characters than the corresponding database column accepts.
        /// </summary>
        public static Predicate<string> CharController(int Limit, bool Greather)
        {
            return t => Greather ? t.Length >= Limit : t.Length <= Limit;
        }
    }
}
