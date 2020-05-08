﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdonnancementsEquitables.Algos
{
    public class Hogdson : Algorithmes<Job>
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

        public override Job[] ExecuteDefault() => Execute(new Job[] { 
            new Job(6, 8),
            new Job(4, 9),
            new Job(7, 15),
            new Job(8, 20),
            new Job(3, 21),
            new Job(5, 22)
        });

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

        public override string ToString() => base.ToString() + "Hogdson\n" + Prefixe + FormattedJobs + Separation + "On time:\n" + FormattedOnTime + Separation + "Late:\n" + FormattedLate + End;
    }
}
