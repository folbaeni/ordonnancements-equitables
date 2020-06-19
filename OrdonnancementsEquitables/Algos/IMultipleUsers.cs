using OrdonnancementsEquitables.Jobs;
using OrdonnancementsEquitables.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdonnancementsEquitables.Algos
{
    public interface IMultipleUsers<TJob> where TJob : Job
    {

        /// <value>
        /// Number of user in the current execution.
        /// </value>
        int NumberOfUsers { get; }

        /// <value>
        /// Shallow copy of the current users.
        /// </value>
        public User<TJob>[] Users { get; }

        /// <summary>
        /// Execution function of an algorithme with <paramref name="users"/> on one device.
        /// </summary>
        /// <param name="users">Array of the users containing the jobs to execute.</param>
        void Execute(User<TJob>[] users);
    }
}
