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
        /// Method of execution of an algorithme with many users with <paramref name="nbDevices"/>
        /// </summary>
        /// <param name="users"></param>
        /// <param name="nbDevices"></param>
        void Execute(User<TJob>[] users, int nbDevices);
    }
}
