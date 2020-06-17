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
        /// <summary>
        /// Parameter of type Tjob[] containing the user's jobs
        /// </summary>
        private readonly TJob[] _jobs;


        /// <summary>
        /// Parameter of type Tjob[] containing the user's jobs
        /// </summary>
        public TJob[] Jobs => _jobs.ToArray();


        /// <summary>
        /// Creates the user's table of jobs 
        /// </summary>
        /// <param name="jobs"></param>
        public User(TJob[] jobs)
        {
            _jobs = jobs.ToArray();
        }

        /// <summary>
        /// method telling if the user has <paramref name="job"/>
        /// </summary>
        /// <param name="job"></param> The job we want to know if the user has or not
        /// <returns>Returns true if the user has <paramref name="job"/>, fale in the other case</returns>
        public bool Contains(TJob job) => _jobs.Contains(job);
    }
}
