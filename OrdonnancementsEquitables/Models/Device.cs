using OrdonnancementsEquitables.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdonnancementsEquitables.Models
{
    /// <summary>
    /// Class representing a device where typed jobs are executed.
    /// </summary>
    public class Device<TJob> where TJob : Job
    {
        /// <value>
        /// Private field representing the list of jobs of the device.
        /// </value>
        private readonly List<TJob> _jobs;

        /// <value>
        /// A shallow copy of the jobs in the device.
        /// </value>
        public TJob[] Jobs { get => _jobs.ToArray(); }

        /// <value>
        /// Property showing when will the jobs of the devices be ready.
        /// </value>
        public int TimeReady { get => _jobs.Sum(j => j is JobCo jobCo ? jobCo.ExecTime : j.Time); }


        /// <summary>
        /// Creates a new instance of an empty device.
        /// </summary>
        public Device()
        {
            _jobs = new List<TJob>();
        }

        /// <summary>
        /// Method to add a new executed job in the device's list.
        /// </summary>
        /// <param name="JobCo">Job to be added.</param>
        public void AddJob(TJob JobCo) => _jobs.Add(JobCo);


        /// <summary>
        /// This method allows to know if <paramref name="JobCo"/> is in the device.
        /// </summary>
        /// <param name="JobCo">Job to check if it is in the device.</param>
        /// <returns>returns <see langword="true"/> if <paramref name="JobCo"/> is in the device; otherwise <see langword="false"/>.</returns>
        public bool Contains(TJob JobCo) => _jobs.Contains(JobCo);
    }
}
