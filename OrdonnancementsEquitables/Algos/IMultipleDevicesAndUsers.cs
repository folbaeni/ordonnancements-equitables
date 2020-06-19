using OrdonnancementsEquitables.Jobs;
using OrdonnancementsEquitables.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdonnancementsEquitables.Algos
{
    public interface IMultipleDevicesAndUsers<TJob> : IMultipleDevices<TJob>, IMultipleUsers<TJob> where TJob : Job
    {

        /// <summary>
        /// Execution function of an algorithme with <paramref name="users"/> on on <paramref name="nbDevices"/> devices.
        /// </summary>
        /// <param name="users">Array of the users containing the jobs to execute.</param>
        /// <param name="nbDevices">Number of devices.</param>
        void Execute(User<TJob>[] users, int nbDevices);
    }
}
