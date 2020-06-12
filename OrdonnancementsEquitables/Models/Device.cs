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
        private readonly List<TJob> _JobCos;

        public TJob[] JobCos { get => _JobCos.ToArray(); }
        public int TimeReady { get => _JobCos.Sum(j => j is JobCo ? (j as JobCo).ExecTime : j.Time); }

        public Device()
        {
            _JobCos = new List<TJob>();
        }

        public void AddJobCo(TJob JobCo) => _JobCos.Add(JobCo);

        public bool Contains(TJob JobCo) => _JobCos.Contains(JobCo);
    }
}
