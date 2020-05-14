using OrdonnancementsEquitables.Jobs;
using OrdonnancementsEquitables.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdonnancementsEquitables.Algos
{
    public class Hogdson : Algorithme<Job>
    {
        public string FormattedOnTime => string.Join("\n", OnTime.Select(j => j.ToString()));
        public string FormattedLate => string.Join("\n", Late.Select(j => j.ToString()));

        private readonly List<Job> OnTime;
        private readonly List<Job> Late;
        private int C;

        public Hogdson()
        {
            C = 0;
            OnTime = new List<Job>();
            Late = new List<Job>();
        }

        public override Job[] ExecuteDefault() => Execute(Parser<Job>.ParseFromContent(Properties.Resources.Hogdson));

        public override Job[] Execute(Job[] jobs)
        {
            currentJobs = (Job[])jobs.Clone();
            OnTime.Clear();
            Late.Clear();
            foreach (Job job in currentJobs)
            {
                OnTime.Add(job);
                C += job.Time;

                if (C > job.Deadline)
                {
                    Job biggest = OnTime.OrderByDescending(j => j.Time).FirstOrDefault();
                    OnTime.Remove(biggest);
                    Late.Add(biggest);
                    C -= biggest.Time;
                }
            }
            return Jobs;
        }

        public override string ToString() => base.ToString() + "Hogdson\n\nOn time:\n" + FormattedOnTime + "\nLate:\n" + FormattedLate + Separation;
    }
}
