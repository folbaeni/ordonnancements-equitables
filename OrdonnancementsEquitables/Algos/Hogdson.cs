using OrdonnancementsEquitables.Drawing;
using OrdonnancementsEquitables.Jobs;
using OrdonnancementsEquitables.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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

        //public override Job[] ExecuteDefault() => Execute(Parser.ParseFromContent<Job>(Properties.Resources.Hogdson));

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

        public override void Draw(Canvas c)
        {
            Drawer g = new Drawer(1, c);
            foreach (Job j in OnTime)
            {
                g.AddJob(j, false);
            }
            foreach (Job j in Late)
            {
                g.AddJob(j, true);
            }
        }


        public override string ToString() => base.ToString() + "Hogdson\n\nOn time:\n" + FormattedOnTime + "\nLate:\n" + FormattedLate + Separation;
    }
}
