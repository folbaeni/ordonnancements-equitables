using OrdonnancementsEquitables.Drawing;
using OrdonnancementsEquitables.Jobs;
using OrdonnancementsEquitables.Utils;
using OrdonnancementsEquitables.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OrdonnancementsEquitables.Algos
{
    public class HigherLockDegree : Algorithme<JobCo>, IMultipleUsers<JobCo>, IMultipleDevices<JobCo>, IMultipleDevicesAndUsers<JobCo>
    {
        public int NumberOfUsers => currentUsers.Length;
        public int NumberOfDevices => currentDevices.Length;
        public User<JobCo>[] Users => currentUsers.ToArray();
        public Device<JobCo>[] Devices => currentDevices.ToArray();
        public double AverageTime => Devices.Average(d => d.TimeReady);
        public int ShortestTimeReady => Devices.OrderBy(d => d.TimeReady).FirstOrDefault().TimeReady;
        public int LongestTimeReady => Devices.OrderByDescending(d => d.TimeReady).FirstOrDefault().TimeReady;

        public override void Execute(JobCo[] jobs) => Execute(jobs, 1);
        //{
        //    Init(jobs);

        //    int C = 0;
        //    GraphLock G = new GraphLock(jobs);
        //    JobCo higher;
        //    while((higher = G.GetHigherOutDegreeOnTime(C)) != null)
        //    {
        //        C += higher.Time;
        //        onTime.Add(higher);
        //        G.ExecuteJob(higher);
        //    }

        //    late = G.GetAllLeftJobs();
        //    return jobs;
        //}

        public void Execute(User<JobCo>[] users) => Execute(users, 1);
        //{
        //    JobCo[] jobs = users.SelectMany(u => u.Jobs).ToArray();
        //    JobCo[] res = Execute(jobs );
        //    currentUsers = users.ToArray();
        //    return res;
        //}

        public void Execute(User<JobCo>[] users, int nbDevices)
        {
            JobCo[] jobs = users.SelectMany(u => u.Jobs).ToArray();
            Execute(jobs, nbDevices);
            
            currentUsers = users.ToArray();
        }

        public void Execute(JobCo[] jobs, int nbDevices)
        {
            Init(jobs);
            //currentJobCos = currentJobCos.OrderByDescending(j => j.Depend.Length).ToArray();
            currentDevices = new Device<JobCo>[nbDevices];


            for (int i = 0; i < nbDevices; i++)
                currentDevices[i] = new Device<JobCo>();
            
            Device<JobCo>[] trOrder = currentDevices.OrderBy(j => j.TimeReady).ToArray();

            GraphLock G = new GraphLock(jobs);
            JobCo higher;

            while((higher = G.GetHigherOutDegreeOnTime(trOrder[0].TimeReady)) != null)
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
            foreach(var jobCo in late)
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

        //public override void Draw(Canvas c)
        //{
        //    DrawerCo dr = new DrawerCo(c, currentUsers == null ? 1 : currentUsers.Length, currentDevices == null ? 1 : currentDevices.Length);
        //    foreach (JobCo j in currentJobs)
        //    {
        //        int userIndex = 0, deviceIndex = 0;
        //        bool isLate = late.Contains(j);

        //        if (currentUsers != null)
        //            userIndex = currentUsers.Select(u => u.Contains(j)).ToList().IndexOf(true);
        //        if (currentDevices != null)
        //            deviceIndex = currentDevices.Select(d => d.Contains(j)).ToList().IndexOf(true);

        //        dr.AddJobCo(j, isLate, userIndex, deviceIndex);
        //    }
        //}
    }
}