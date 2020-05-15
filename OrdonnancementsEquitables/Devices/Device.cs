using OrdonnancementsEquitables.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdonnancementsEquitables.Devices
{
    public class Device<TJob> where TJob : Job
    {
        private List<TJob> _jobs;

        public TJob[] Jobs { get => _jobs.ToArray(); }
        public int TimeReady { get => _jobs.Sum(j => j.Time);  }

        public Device()
        {
            _jobs = new List<TJob>();
        }

        public void AddJob(TJob job)
        {
            _jobs.Add(job);
        }
    }
}
