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

        public override JobCo[] Execute(JobCo[] JobCos)
        {
            Init(JobCos);

            int C = 0;
            GraphLock G = new GraphLock(JobCos);
            JobCo higher;
            while((higher = G.GetHigherOutDegreeOnTime(C)) != null)
            {
                C += higher.Time;
                onTime.Add(higher);
                G.ExecuteJobCo(higher);
            }

            late = G.GetAllLeftJobCos();
            return JobCos;
        }

        public JobCo[] Execute(User<JobCo>[] users)
        {
            JobCo[] JobCos = users.SelectMany(u => u.Jobs).ToArray();
            JobCo[] res = Execute(JobCos);
            currentUsers = users.ToArray();
            return res;
        }

        public JobCo[] Execute(JobCo[] JobCos, int nbDevices)
        {
            Init(JobCos);
            //currentJobCos = currentJobCos.OrderByDescending(j => j.Depend.Length).ToArray();
            currentDevices = new Device<JobCo>[nbDevices];
            
            int i;
            for (i = 0; i < nbDevices; i++)
                currentDevices[i] = new Device<JobCo>();

            GraphLock G = new GraphLock(JobCos);
            JobCo higher;
            i = 0;
            while((higher = G.GetHigherOutDegreeOnTime(currentDevices[i].TimeReady)) != null)
            { 
                currentDevices[i].AddJobCo(higher);
                onTime.Add(higher);
                G.ExecuteJobCo(higher);
                i = (i + 1) % nbDevices;
            }
            late = G.GetAllLeftJobCos();
            return JobCos;
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

                dr.AddJobCo(j, isLate, userIndex, deviceIndex);
            }
        }
    }
}