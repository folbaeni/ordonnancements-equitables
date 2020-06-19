using OrdonnancementsEquitables.Jobs;
using OrdonnancementsEquitables.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdonnancementsEquitables.Algos
{
    public interface IMultipleDevices<TJob> where TJob : Job
    {
        /// <value>
        /// Number of devices on the current execution.
        /// </value>
        int NumberOfDevices { get; }

        /// <summary>
        /// Average end of execution time on every devices.
        /// </summary>
        double AverageTime { get; }

        /// <summary>
        /// Shortest end of execution time.
        /// </summary>
        int ShortestTimeReady { get; }

        /// <summary>
        /// Longest end of execution time.
        /// </summary>
        int LongestTimeReady { get; }

        /// <summary>
        /// Shallow copy of all devices.
        /// </summary>
        Device<TJob>[] Devices { get; }

        /// <summary>
        /// Execute the algorithm with <paramref name="jobs"/> on <paramref name="nbDevices"/> devices.
        /// </summary>
        /// <param name="jobs">Array of jobs to execute.</param>
        /// <param name="nbDevices">Number of devices.</param>
        void Execute(TJob[] jobs, int nbDevices);
    }
}
