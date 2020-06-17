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
        /// <summary>
        /// Class containig tools in order to manipulate devices
        /// </summary>


        /// <summary>
        /// Parameter representing the list of jobs of the device
        /// </summary>
        private readonly List<TJob> _jobs;

        /// <summary>
        /// Parameter converting the list of jobs of the device into a table
        /// </summary>
        public TJob[] Jobs { get => _jobs.ToArray(); }

        /// <summary>
        /// Parameter showing when will the jobs of the devices be ready
        /// </summary>
        public int TimeReady { get => _jobs.Sum(j => j is JobCo ? (j as JobCo).ExecTime : j.Time); }


        /// <summary>
        /// Creates a devices, aka initialize a new list of TJob
        /// </summary>
        public Device()
        {
            _jobs = new List<TJob>();
        }

        /// <summary>
        /// this method allows us to add jobs in the device
        /// </summary>
        /// <param name="JobCo"></param>
        public void AddJob(TJob JobCo) => _jobs.Add(JobCo);


        /// <summary>
        /// This method allows to know if <paramref name="JobCo"/> is in the device
        /// </summary>
        /// <param name="JobCo"></param>
        /// <returns></returns>
        public bool Contains(TJob JobCo) => _jobs.Contains(JobCo);
    }
}
