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
    public class HigherTimeDecrease : Algorithme<JobCo>, IMultipleUsers<JobCo>, IMultipleDevices<JobCo>, IMultipleDevicesAndUsers<JobCo>
    {
        /// <summary>
        /// Parameter of type int telling the number of users used to do the algorithme HigherLockDegree
        /// </summary>
        public int NumberOfUsers => currentUsers.Length;

        /// <summary>
        /// Parameter of type int telling the number of devices used to do the algorithme HigherLockDegree
        /// </summary>
        public int NumberOfDevices => currentDevices.Length;

        /// <summary>
        /// Parameter of type User<JobCo>[] used to stock the current users of the algorithme HigherLockDegree
        /// </summary>
        public User<JobCo>[] Users => currentUsers.ToArray();

        /// <summary>
        /// Parameter of type User<JobCo>[] used to stock the current devices of the algorithme HigherLockDegree
        /// </summary>
        public Device<JobCo>[] Devices => currentDevices.ToArray();

        /// <summary>
        /// Parameter of type double used to know the average time of execution of the jobs with HigherLockDegree
        /// </summary>
        public double AverageTime => Devices.Average(d => d.TimeReady);

        /// <summary>
        /// Parameter of type int used to know the shortest time when a job is ready
        /// </summary>
        public int ShortestTimeReady => Devices.OrderBy(d => d.TimeReady).FirstOrDefault().TimeReady;

        /// <summary>
        /// Parameter of type int used to know the longest time when a job is ready
        /// </summary>
        public int LongestTimeReady => Devices.OrderByDescending(d => d.TimeReady).FirstOrDefault().TimeReady;

        /// <summary>
        ///  Execute the algorithme HigherTimeDecrease apply on <paramref name="jobs"/> with one device 
        /// </summary>
        /// <param name="jobs"></param>
        public override void Execute(JobCo[] jobs) => Execute(jobs, 1);
        /*{
            Init(jobs);

            int C = 0;
            GraphTimeD G = new GraphTimeD(jobs);
            JobCo higher;
            while ((higher = G.GetHigherOutDegreeOnTime(C)) != null)
            {
                C += higher.ExecTime;
                onTime.Add(higher);
                G.ExecuteJob(higher);
            }

            late = G.GetAllLeftJobs();
            return Jobs;
        }*/

        /// <summary>
        /// Execute the algorithme HigherTimeDecrease with one device and <paramref name="users"/>
        /// </summary>
        /// <param name="users"></param>  used to execute HigherTimeDecrease
        public void Execute(User<JobCo>[] users) => Execute(users, 1);
        /*{
            JobCo[] jobs = users.SelectMany(u => u.Jobs).ToArray();
            JobCo[] res = Execute(jobs);
            currentUsers = users.ToArray();
            return res;
        }*/

        /// <summary>
        ///  Execute the algorithme HigherTimeDecrease <paramref name="users"/> of type User<JobCo>[] and <paramref name="nbDevices"/> of type int
        /// </summary>
        /// <param name="users"></param> used to know the users we use to execute HigherTimeDecrease
        /// <param name="nbDevices"></param> used to know with how lany devices we apply HigherTimeDecrease
        public void Execute(User<JobCo>[] users, int nbDevices)
        {
            JobCo[] jobs = users.SelectMany(u => u.Jobs).ToArray();
            Execute(jobs, nbDevices);
            
            currentUsers = users.ToArray();
        }

        /// <summary>
        ///  Execute the algorithme HiggherTimeDecrease apply on <paramref name="jobs"/> of type JobCo[] and with <paramref name="nbDevices"/> of type int
        /// </summary>
        /// <param name="jobs"></param> used to know the jobs we apply on the algorithme HigherLockDegree
        /// <param name="nbDevices"></param> used to know how many devices are used for the algorithme HigherLockDegree
        public void Execute(JobCo[] jobs, int nbDevices)
        {

            Init(jobs);
            //currentJobCos = currentJobCos.OrderByDescending(j => j.Depend.Length).ToArray();
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
