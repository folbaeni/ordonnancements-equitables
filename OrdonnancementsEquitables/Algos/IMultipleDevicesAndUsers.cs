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
        TJob[] Execute(User[] users, int nbDevices);
    }
}
