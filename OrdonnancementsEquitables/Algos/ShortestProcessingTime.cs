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
        public double AverageTime { get => Devices.Average(d => d.TimeReady); }
        public int ShortestTimeReady { get => Devices.OrderBy(d => d.TimeReady).FirstOrDefault().TimeReady; }
        public int LongestTimeReady { get => Devices.OrderByDescending(d => d.TimeReady).FirstOrDefault().TimeReady; }
        public User<Job>[] Users { get => (User<Job>[])currentUsers.Clone(); }
        public Device<Job>[] Devices { get => (Device<Job>[])currentDevices.Clone(); }

        private User<Job>[] currentUsers;
        private Device<Job>[] currentDevices;

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
            }

            return Jobs;
        }

        public Job[] Execute(User<Job>[] users)
        {
            currentUsers = users;
            Job[] jobs = currentUsers.SelectMany(u => u.Jobs).ToArray();

            return Execute(jobs);
        }

        public Job[] Execute(User<Job>[] users, int nbDevices)
        {
            currentUsers = users;
            Job[] jobs = currentUsers.SelectMany(u => u.Jobs).ToArray();

            return Execute(jobs, nbDevices);
        }

        public override void Draw(Canvas c)
        {
            Drawer dr = new Drawer(c, currentUsers == null ? 1 : currentUsers.Length, currentDevices == null ? 1 : currentDevices.Length);
            foreach (Job j in currentJobs)
            {
                int userIndex = 0, deviceIndex = 0;
                bool isLate = late.Contains(j);

                if (currentUsers != null)
                    userIndex = currentUsers.Select(u => u.Contains(j)).ToList().IndexOf(true);
                if (currentDevices != null)
                    deviceIndex = currentDevices.Select(d => d.Contains(j)).ToList().IndexOf(true);

                dr.AddJob(j, isLate, userIndex, deviceIndex);
            }
        }
    }
}