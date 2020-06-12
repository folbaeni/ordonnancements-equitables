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
    public class LongestProcessingTime : Algorithme<JobCo>, IMultipleDevices<JobCo>, IMultipleUsers<JobCo>
    {
        public double AverageTime => Devices.Average(d => d.TimeReady);
        public int ShortestTimeReady => Devices.OrderBy(d => d.TimeReady).FirstOrDefault().TimeReady;
        public int LongestTimeReady => Devices.OrderByDescending(d => d.TimeReady).FirstOrDefault().TimeReady;

        public User<JobCo>[] Users => currentUsers.ToArray();
        public Device<JobCo>[] Devices => currentDevices.ToArray();


        public override JobCo[] Execute(JobCo[] jobs) => Execute(jobs, 1);
        
        public JobCo[] Execute(JobCo[] jobs, int nbDevices)
        {
            Init(jobs);
            currentJobs = currentJobs.OrderByDescending(j => j.Time).ToArray();
            currentDevices = new Device<JobCo>[nbDevices];

            for (int i = 0; i < nbDevices; i++)
                currentDevices[i] = new Device<JobCo>();

            foreach (JobCo j in currentJobs)
            {
                Device<JobCo> d = currentDevices.OrderBy(d => d.TimeReady).FirstOrDefault();
                d.AddJob(j);

                if (d.TimeReady + j.Time < j.Deadline)
                    onTime.Add(j);
                else
                    late.Add(j);
            }
            return Jobs;
        }

        public JobCo[] Execute(User<JobCo>[] users) => Execute(users, 1);

        public JobCo[] Execute(User<JobCo>[] users, int nbDevices)
        {
            currentUsers = users;
            JobCo[] jobs = currentUsers.SelectMany(u => u.Jobs).ToArray();

            return Execute(jobs, nbDevices);
        }

        public override void Draw(Canvas c)
        {
            Drawer dr = new Drawer(c, currentUsers == null ? 1 : currentUsers.Length, currentDevices == null ? 1 : currentDevices.Length);
            foreach (JobCo j in currentJobs)
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