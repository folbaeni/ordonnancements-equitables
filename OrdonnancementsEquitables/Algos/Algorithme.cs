using OrdonnancementsEquitables.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdonnancementsEquitables.Algos
{
    public abstract class Algorithme<TJob> where TJob : Job
    {
        protected readonly static string Separation = "\n####################################\n\n";

        public string FormattedJobs => string.Join("\n", Jobs.Select(j => j.ToString()));
        public TJob[] Jobs => (TJob[])currentJobs.Clone();
        protected TJob[] currentJobs;

        public abstract TJob[] ExecuteDefault();
        public abstract TJob[] Execute(TJob[] jobs);

        public override string ToString() => "Resultat de l'algorithme: ";
    }
}
