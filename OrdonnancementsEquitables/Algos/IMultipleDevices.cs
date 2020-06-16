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
        int NumberOfDevices { get; }
        double AverageTime { get; }
        int ShortestTimeReady { get; }
        int LongestTimeReady { get; }

        Device<TJob>[] Devices { get; }
        TJob[] Execute(TJob[] jobs, int nbDevices);
    }
}
