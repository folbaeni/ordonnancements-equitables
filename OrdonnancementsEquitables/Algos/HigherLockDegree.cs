using OrdonnancementsEquitables.Drawing;
using OrdonnancementsEquitables.Jobs;
using OrdonnancementsEquitables.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OrdonnancementsEquitables.Algos
{
    public class HigherLockDegree : Algorithme<JobCo>, IMultipleUsers<JobCo>, IMultipleDevices<JobCo>
    {
        public User<JobCo>[] Users => currentUsers.ToArray();
        public Device<JobCo>[] Devices => currentDevices.ToArray();

        public double AverageTime => throw new NotImplementedException();
        public int ShortestTimeReady => throw new NotImplementedException();
        public int LongestTimeReady => throw new NotImplementedException();

        public override JobCo[] Execute(JobCo[] jobs)
        {
            Init(jobs);

            int C = 0;
            GraphLock G = new GraphLock(jobs);
            JobCo higher;
            while((higher = G.GetHigherOutDegreeOnTime(C)) != null)
            {
                C += higher.Time;
                onTime.Add(higher);
                G.ExecuteJob(higher);
            }

            late = G.GetAllLeftJobs();
            return Jobs;
        }

        public JobCo[] Execute(User<JobCo>[] users)
        {
            JobCo[] jobs = users.SelectMany(u => u.Jobs).ToArray();
            JobCo[] res = Execute(jobs);
            currentUsers = users.ToArray();
            return res;
        }

        public JobCo[] Execute(JobCo[] jobs, int nbDevices)
        {
            Init(jobs);
            //currentJobs = currentJobs.OrderByDescending(j => j.Depend.Length).ToArray();
            currentDevices = new Device<JobCo>[nbDevices];
            
            int i;
            for (i = 0; i < nbDevices; i++)
                currentDevices[i] = new Device<JobCo>();

            GraphLock G = new GraphLock(jobs);
            JobCo higher;
            i = 0;
            while((higher = G.GetHigherOutDegreeOnTime(currentDevices[i].TimeReady)) != null)
            { 
                currentDevices[i].AddJob(higher);
                onTime.Add(higher);
                G.ExecuteJob(higher);
                i = (i + 1) % nbDevices;
            }
            late = G.GetAllLeftJobs();
            return jobs;
        }

        public override void Draw(Canvas c)
        {
            DrawerCo dr = new DrawerCo(c, currentUsers == null ? 1 : currentUsers.Length, currentDevices == null ? 1 : currentDevices.Length);
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