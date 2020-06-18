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

        /// <summary>
        /// Parameter of type int representing the number of devices, this parameter is defined as readonly
        /// </summary>
        int NumberOfDevices { get; }

        /// <summary>
        /// Parameter of type double representing the average time of execution of an algorithme, this parameter is defined as readonly
        /// </summary>
        double AverageTime { get; }

        /// <summary>
        /// Parameter of type int representing the shortest time of execution of an algorithme, this parameter is defined as readonly
        /// </summary>
        int ShortestTimeReady { get; }

        /// <summary>
        /// Parameter of type int reoresenting the longest time of execution of an algorithme, this parameter is defined as readonly
        /// </summary>
        int LongestTimeReady { get; }

        /// <summary>
        /// Parameter of type Device<TJob>[] representing the devices used, this parameter is defined as readonly
        /// </summary>
        Device<TJob>[] Devices { get; }

        /// <summary>
        /// Method of execution of an algorithme with <paramref name="nbDevices"/> apply on TJob[] <paramref name="jobs"/>
        /// </summary>
        /// <param name="jobs"></param>
        /// <param name="nbDevices"></param>
        void Execute(TJob[] jobs, int nbDevices);
    }
}
