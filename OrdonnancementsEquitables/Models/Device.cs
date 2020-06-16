using OrdonnancementsEquitables.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdonnancementsEquitables.Models
{
    public class Device<TJob> where TJob : Job
    {
        private readonly List<TJob> _jobs;

        public TJob[] Jobs { get => _jobs.ToArray(); }
        public int TimeReady { get => _jobs.Sum(j => j is JobCo ? (j as JobCo).ExecTime : j.Time); }

        public Device()
        {
            _jobs = new List<TJob>();
        }

        public void AddJob(TJob JobCo) => _jobs.Add(JobCo);

        public bool Contains(TJob JobCo) => _jobs.Contains(JobCo);
    }
}
