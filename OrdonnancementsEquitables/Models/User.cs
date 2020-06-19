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
        /// Private field containing the user's jobs.
        /// </summary>
        private readonly TJob[] _jobs;


        /// <summary>
        /// Array containing the user's jobs.
        /// </summary>
        /// <value>
        /// Shallow copy of the user's jobs.
        /// </value>
        public TJob[] Jobs => _jobs.ToArray();


        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="jobs">The user's jobs.</param>
        public User(TJob[] jobs)
        {
            _jobs = jobs.ToArray();
        }

        /// <summary>
        /// Method telling if <paramref name="job"/> belongs to this user.
        /// </summary>
        /// <param name="job">The job to check if it belongs to the user or not.</param>
        /// <returns>Returns <see langword="true"/> if <paramref name="job"/> belongs to the user; otherwise <see langword="false"/></returns>
        public bool Contains(TJob job) => _jobs.Contains(job);
    }
}
