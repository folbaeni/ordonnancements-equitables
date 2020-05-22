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
        double AverageTime { get => Devices.Average(d => d.TimeReady); }
        int ShortestTimeReady { get => Devices.OrderBy(d => d.TimeReady).FirstOrDefault().TimeReady; }
        int LongestTimeReady { get => Devices.OrderByDescending(d => d.TimeReady).FirstOrDefault().TimeReady; }

        Device<TJob>[] Devices { get; }
        TJob[] Execute(TJob[] jobs, int nbDevices);
    }
}
