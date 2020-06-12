using OrdonnancementsEquitables.Drawing;
using OrdonnancementsEquitables.Jobs;
using OrdonnancementsEquitables.Models;
using OrdonnancementsEquitables.Parsers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace OrdonnancementsEquitables.Algos
{
    public abstract class Algorithme<TJob> where TJob : JobCo
    {
        protected readonly static string Separation = "\n####################################\n\n";
        public string FormattedJobs => string.Join("\n", Jobs.Select(j => j.ToString()));

        public TJob[] Jobs => currentJobs.ToArray();
        public TJob[] OnTime => onTime.ToArray();
        public TJob[] Late => late.ToArray();

        protected TJob[] currentJobs;
        protected User<JobCo>[] currentUsers;
        protected Device<JobCo>[] currentDevices;
        protected List<TJob> onTime, late;

        public Algorithme()
        {
            onTime = new List<TJob>(); 
            late = new List<TJob>();
        }

        protected virtual void Init(TJob[] jobs)
        {
            onTime.Clear();
            late.Clear();
            currentJobs = (TJob[])jobs.Clone();
            currentUsers = null;
            currentDevices = null;
        }

        public TJob[] ExecuteDefault() => Execute(new Parser($@"Assets\Default Jobs\{GetType().Name}.json").ParseJobsFromJSON<TJob>());
        public abstract TJob[] Execute(TJob[] jobs);
        public override string ToString() => "Resultat de l'algorithme: ";

        public abstract void Draw(Canvas c);
    }
}
