using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OrdonnancementsEquitables.Outils
{
    public static class StringConverter
    {
        public static string AffToSyst(this string aff)
        {
            return aff.Replace(" ", String.Empty);
        }

        public static string SystToAff(this string syst)
        {
            return Regex.Replace(syst, "([a-z])([A-Z])", "$1 $2");
        }
    }
}
