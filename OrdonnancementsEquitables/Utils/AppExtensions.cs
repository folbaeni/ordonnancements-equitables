using OrdonnancementsEquitables.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OrdonnancementsEquitables.Utils
{
    public static class AppExtensions
    {
        public static string AffToSyst(this string aff)
        {
            return aff.Replace(" ", string.Empty);
        }

        public static string SystToAff(this string syst)
        {
            return Regex.Replace(syst, "([a-z])([A-Z])", "$1 $2");
        }

        public static TJob FromId<TJob>(this IEnumerable<TJob> enumerable, int id) where TJob : Job => enumerable.Where(j => j.Id == id).FirstOrDefault();

        public static void Swap<T>(this IList<T> list, int index1, int index2)
        {
            var tmp = list[index1];
            list[index1] = list[index2];
            list[index2] = tmp;
        }
    }
}
