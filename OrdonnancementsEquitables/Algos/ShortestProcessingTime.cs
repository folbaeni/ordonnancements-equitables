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
    public class ShortestProcessingTime : Algorithme<JobCo>, IMultipleDevices<JobCo>, IMultipleUsers<JobCo>, IMultipleDevicesAndUsers<JobCo>
    {
        public double AverageTime { get => Devices.Average(d => d.TimeReady); }
        public int ShortestTimeReady { get => Devices.OrderBy(d => d.TimeReady).FirstOrDefault().TimeReady; }
        public int LongestTimeReady { get => Devices.OrderByDescending(d => d.TimeReady).FirstOrDefault().TimeReady; }
        public User<JobCo>[] Users { get => (User<JobCo>[])currentUsers.Clone(); }
        public Device<JobCo>[] Devices { get => (Device<JobCo>[])currentDevices.Clone(); }

        public override JobCo[] Execute(JobCo[] JobCos) => Execute(JobCos, 1);

        public JobCo[] Execute(JobCo[] JobCos, int nbDevices)
        {
            currentJobs = JobCos.OrderBy(j => j.Time).ToArray();
            currentDevices = new Device<JobCo>[nbDevices];

            for (int i = 0; i < nbDevices; i++)
                currentDevices[i] = new Device<JobCo>();

            foreach (JobCo j in currentJobs)
            {
                Device<JobCo> d = currentDevices.OrderBy(d => d.TimeReady).FirstOrDefault();
                d.AddJobCo(j);

                if (d.TimeReady + j.Time < j.Deadline)
                    onTime.Add(j);
                else
                    late.Add(j);
            }

            return JobCos;
        }

        public JobCo[] Execute(User<JobCo>[] users)
        {
            currentUsers = users;
            JobCo[] JobCos = currentUsers.SelectMany(u => u.Jobs).ToArray();

            return Execute(JobCos);
        }

        public JobCo[] Execute(User<JobCo>[] users, int nbDevices)
        {
            currentUsers = users;
            JobCo[] JobCos = currentUsers.SelectMany(u => u.Jobs).ToArray();

            return Execute(JobCos, nbDevices);
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