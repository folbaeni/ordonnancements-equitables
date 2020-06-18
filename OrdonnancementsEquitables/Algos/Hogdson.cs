using OrdonnancementsEquitables.Drawing;
using OrdonnancementsEquitables.Jobs;
using OrdonnancementsEquitables.Models;
using OrdonnancementsEquitables.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OrdonnancementsEquitables.Algos
{
    public class Hogdson : Algorithme<Job>, IMultipleUsers<Job>
    {

        /// <summary>
        /// Parameter of type Device<Job> represening the main device, aka the first device of currentDevices from Algorithme
        /// </summary>
        private Device<Job> MainDevice => currentDevices[0];

        /// <summary>
        /// Parameter of type User<Job>[] witch is the conversion of currentUsers to an Array
        /// </summary>
        public User<Job>[] Users => currentUsers.ToArray();

        /// <summary>
        /// Parameter of type int corresponding to the number of users
        /// </summary>
        public int NumberOfUsers => currentUsers.Length;


        /// <summary>
        /// Execute the algorithme Hogdson with <paramref name="jobs"/>
        /// </summary>
        /// <param name="jobs"></param>
        public override void Execute(Job[] jobs)
        {
            Init(jobs);
            int C = 0;
            MaxHeap<Job> heap = new MaxHeap<Job>();

            foreach (Job job in currentJobs)
            {
                heap.Insert(job);
                onTime.Add(job);
                C += job.Time;

                if (C > job.Deadline)
                {
                    //JobCo biggest = onTime.OrderByDescending(j => j.Time).FirstOrDefault();
                    Job biggest = heap.RemoveMax();
                    onTime.Remove(biggest);
                    late.Add(biggest);
                    C -= biggest.Time;
                }
            }

            foreach (Job j in onTime)
                MainDevice.AddJob(j);
            foreach (Job j in late)
                MainDevice.AddJob(j);
        }

        public void Execute(User<Job>[] users)
        {
            Job[] jobs = users.SelectMany(u => u.Jobs).ToArray();
            Execute(jobs);
            
            currentUsers = users.ToArray();
        }

        //public override void Draw(Canvas c)
        //{
        //    Drawer dr = new Drawer(c, currentUsers.Length, currentDevices.Length);

        //    for (int deviceIndex = 0; deviceIndex < currentDevices.Length; deviceIndex++)
        //    {
        //        Device<Job> device = currentDevices[deviceIndex];
        //        foreach (Job job in device.Jobs)
        //        {
        //            User<Job> user = currentUsers.Where(u => u.Jobs.Contains(job)).FirstOrDefault();
        //            int userIndex = Array.IndexOf(currentUsers, user);

        //            bool isLate = late.Contains(job);

        //            dr.AddJob(job, isLate, userIndex, deviceIndex);
        //        }
        //    }
        //}
    }
}
