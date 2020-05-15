using OrdonnancementsEquitables.Models;
using OrdonnancementsEquitables.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdonnancementsEquitables.Algos
{
    public class ShortestProcessingTime : Algorithme<Job>, IMultipleDevices<Job>
    {
        private Device<Job>[] Devices;

        public override Job[] ExecuteDefault() => null;

        public override Job[] Execute(Job[] jobs) => Execute(jobs, 1);

        public Job[] Execute(Job[] jobs, int nbDevices)
        {
            currentJobs = jobs.OrderBy(j => j.Time).ToArray();
            Devices = new Device<Job>[nbDevices];

            for(int i = 0; i < currentJobs.Length; i++)
            {
                Devices[i % nbDevices].AddJob(currentJobs[i]);
            }

            return Jobs;
        }

        /*public Job[] Execute(User[] users);
        public Job[] Execute(User[] users, int nbDevices);*/
    }
}