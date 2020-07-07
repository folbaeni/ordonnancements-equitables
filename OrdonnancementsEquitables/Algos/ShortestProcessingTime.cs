using OrdonnancementsEquitables.Models;
using OrdonnancementsEquitables.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using OrdonnancementsEquitables.Drawing;

namespace OrdonnancementsEquitables.Algos
{
    public class ShortestProcessingTime : Algorithm<Job>, IMultipleDevices<Job>, IMultipleUsers<Job>, IMultipleDevicesAndUsers<Job>
    {
        public int NumberOfUsers => currentUsers.Length;
        public int NumberOfDevices => currentDevices.Length;
        public double AverageTime => Devices.Average(d => d.TimeReady);
        public int ShortestTimeReady => Devices.OrderBy(d => d.TimeReady).FirstOrDefault().TimeReady;
        public int LongestTimeReady => Devices.OrderByDescending(d => d.TimeReady).FirstOrDefault().TimeReady;
        
        public User<Job>[] Users => currentUsers.ToArray();
        public Device<Job>[] Devices => currentDevices.ToArray();

        public override void Execute(Job[] jobs) => Execute(jobs, 1);

        public void Execute(Job[] jobs, int nbDevices)
        {
            Init(jobs);
            currentJobs = jobs.OrderBy(j => j.Time).ToArray();
            currentDevices = new Device<Job>[nbDevices];

            for (int i = 0; i < nbDevices; i++)
                currentDevices[i] = new Device<Job>();

            foreach (Job j in currentJobs)
            {
                Device<Job> d = currentDevices.OrderBy(d => d.TimeReady).FirstOrDefault();
                d.AddJob(j);

                if (d.TimeReady < j.Deadline)
                    onTime.Add(j);
                else
                    late.Add(j);
            }
        }

        public void Execute(User<Job>[] users) => Execute(users, 1); 

        public void Execute(User<Job>[] users, int nbDevices)
        {
            Job[] jobs = users.SelectMany(u => u.Jobs).ToArray();
            
            Execute(jobs, nbDevices);
            currentUsers = users;
        }
    }
}