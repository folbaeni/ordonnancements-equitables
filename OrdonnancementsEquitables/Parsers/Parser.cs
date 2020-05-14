using OrdonnancementsEquitables.Jobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace OrdonnancementsEquitables.Parsers
{
    public class Parser
    {
        public static TJob[] Parse<TJob>(string filePath) where TJob : Job
        {
            string content = File.ReadAllText(filePath);

            return null;
        }
    }
}
