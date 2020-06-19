using OrdonnancementsEquitables.Jobs;
using OrdonnancementsEquitables.Models;
using OrdonnancementsEquitables.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OrdonnancementsEquitables.Algos
{
    public class HigherTimeDecrease : Algorithm<JobCo>, IMultipleUsers<JobCo>, IMultipleDevices<JobCo>, IMultipleDevicesAndUsers<JobCo>
    {
        public int NumberOfUsers => currentUsers.Length;
        public int NumberOfDevices => currentDevices.Length;
        public User<JobCo>[] Users => currentUsers.ToArray();
        public Device<JobCo>[] Devices => currentDevices.ToArray();
        public double AverageTime => Devices.Average(d => d.TimeReady);
        public int ShortestTimeReady => Devices.OrderBy(d => d.TimeReady).FirstOrDefault().TimeReady;
        public int LongestTimeReady => Devices.OrderByDescending(d => d.TimeReady).FirstOrDefault().TimeReady;

        public override void Execute(JobCo[] jobs) => Execute(jobs, 1);

        public void Execute(User<JobCo>[] users) => Execute(users, 1);

        public void Execute(User<JobCo>[] users, int nbDevices)
        {
            JobCo[] jobs = users.SelectMany(u => u.Jobs).ToArray();
            Execute(jobs, nbDevices);
            
            currentUsers = users.ToArray();
        }

        public void Execute(JobCo[] jobs, int nbDevices)
        {
            Init(jobs);
            currentDevices = new Device<JobCo>[nbDevices];


            for (int i = 0; i < nbDevices; i++)
                currentDevices[i] = new Device<JobCo>();

            Device<JobCo>[] trOrder = currentDevices.OrderBy(j => j.TimeReady).ToArray();

            GraphTimeD G = new GraphTimeD(jobs);
            JobCo higher;

            while ((higher = G.GetHigherOutDegreeOnTime(trOrder[0].TimeReady)) != null)
            {
                trOrder[0].AddJob(higher);
                onTime.Add(higher);
                G.ExecuteJob(higher);

                int i = 0;
                while (i < trOrder.Length && trOrder[i].TimeReady > trOrder[i + 1].TimeReady)
                {
                    trOrder.Swap(i, i + 1);
                    i++;
                }
            }

            late = G.GetAllLeftJobs();
            foreach (var jobCo in late)
            {
                trOrder[0].AddJob(jobCo);

                int i = 0;
                while (i < trOrder.Length && trOrder[i].TimeReady > trOrder[i + 1].TimeReady)
                {
                    trOrder.Swap(i, i + 1);
                    i++;
                }
            }
        }
    }
}
