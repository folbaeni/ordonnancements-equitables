using OrdonnancementsEquitables.Models;
using OrdonnancementsEquitables.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrdonnancementsEquitables.Drawing;
using System.Windows.Controls;

namespace OrdonnancementsEquitables.Algos
{
    public class LongestProcessingTime : Algorithme<Job>, IMultipleDevices<Job>, IMultipleUsers<Job>
    {
        public double AverageTime => Devices.Average(d => d.TimeReady);
        public int ShortestTimeReady => Devices.OrderBy(d => d.TimeReady).FirstOrDefault().TimeReady;
        public int LongestTimeReady => Devices.OrderByDescending(d => d.TimeReady).FirstOrDefault().TimeReady;
        public User<Job>[] Users => (User<Job>[])currentUsers.Clone();
        public Device<Job>[] Devices => (Device<Job>[])currentDevices.Clone();

        private Device<Job>[] currentDevices;
        private User<Job>[] currentUsers;

        public override Job[] Execute(Job[] jobs) => Execute(jobs, 1);
        
        public Job[] Execute(Job[] jobs, int nbDevices)
        {
            currentJobs = jobs.OrderByDescending(j => j.Time).ToArray();
            currentDevices = new Device<Job>[nbDevices];

            for (int i = 0; i < nbDevices; i++)
                currentDevices[i] = new Device<Job>();

            foreach (Job j in currentJobs)
            {
                Device<Job> d = currentDevices.OrderBy(d => d.TimeReady).FirstOrDefault();
                d.AddJob(j);
            }
            return Jobs;
        }

        public Job[] Execute(User<Job>[] users) => Execute(users, 1);

        public Job[] Execute(User<Job>[] users, int nbDevices)
        {
            currentUsers = users;
            Job[] jobs = currentUsers.SelectMany(u => u.Jobs).ToArray();

            return Execute(jobs, nbDevices);
        }

        public override void Draw(Canvas c)
        {
            Drawer dr = new Drawer(currentDevices.Length, c, currentUsers.Length);
            foreach (Job j in currentJobs)
            {
            }
        }
    }
}
