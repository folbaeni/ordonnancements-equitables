using OrdonnancementsEquitables.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdonnancementsEquitables.Models
{
    public class User<TJob> where TJob : Job
    {
        private readonly TJob[] _jobs;

        public TJob[] Jobs => _jobs.ToArray();

        public User(TJob[] jobs)
        {
            _jobs = jobs.ToArray();
        }

        public bool Contains(TJob job) => _jobs.Contains(job);
    }
}
