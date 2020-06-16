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
    public class ShortestProcessingTime : Algorithme<Job>, IMultipleDevices<Job>, IMultipleUsers<Job>, IMultipleDevicesAndUsers<Job>
    {
        public double AverageTime => Devices.Average(d => d.TimeReady);
        public int ShortestTimeReady => Devices.OrderBy(d => d.TimeReady).FirstOrDefault().TimeReady;
        public int LongestTimeReady => Devices.OrderByDescending(d => d.TimeReady).FirstOrDefault().TimeReady;

        public User<Job>[] Users => currentUsers.ToArray();
        public Device<Job>[] Devices => currentDevices.ToArray();

        public override Job[] Execute(Job[] jobs) => Execute(jobs, 1);

        public Job[] Execute(Job[] jobs, int nbDevices)
        {
            currentJobs = jobs.OrderBy(j => j.Time).ToArray();
            currentDevices = new Device<Job>[nbDevices];

            for (int i = 0; i < nbDevices; i++)
                currentDevices[i] = new Device<Job>();

            foreach (Job j in currentJobs)
            {
                Device<Job> d = currentDevices.OrderBy(d => d.TimeReady).FirstOrDefault();
                d.AddJob(j);

                if (d.TimeReady + j.Time < j.Deadline)
                    onTime.Add(j);
                else
                    late.Add(j);
            }

            return Jobs;
        }

        public Job[] Execute(User<Job>[] users)
        {
            Job[] jobs = currentUsers.SelectMany(u => u.Jobs).ToArray();
            var res = Execute(jobs);
         
            currentUsers = users;
            return res;
        }

        public Job[] Execute(User<Job>[] users, int nbDevices)
        {
            currentUsers = users;
            Job[] JobCos = currentUsers.SelectMany(u => u.Jobs).ToArray();

            return Execute(JobCos, nbDevices);
        }

        public override void Draw(Canvas c)
        {
            Drawer dr = new Drawer(c, currentUsers.Length, currentDevices.Length);
            foreach (Job j in currentJobs)
            {
                bool isLate = late.Contains(j);

                int userIndex = currentUsers.Select(u => u.Contains(j)).ToList().IndexOf(true);
                int deviceIndex = currentDevices.Select(d => d.Contains(j)).ToList().IndexOf(true);

                dr.AddJob(j, isLate, userIndex, deviceIndex);
            }
        }
    }
}