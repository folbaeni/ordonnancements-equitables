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

        /// <summary>
        /// Parameter of type double representing the average time of execution for the algorithme ShortestProcessingTime
        /// </summary>
        public double AverageTime => Devices.Average(d => d.TimeReady);

        /// <summary>
        /// Parameter of type int representing the shortest time when a job is ready for the execution of teh algorithme ShortestProcessingTime
        /// </summary>
        public int ShortestTimeReady => Devices.OrderBy(d => d.TimeReady).FirstOrDefault().TimeReady;

        /// <summary>
        /// Parameter of type int representing the longest time when a job is ready for the execution of teh algorithme ShortestProcessingTime
        /// </summary>
        public int LongestTimeReady => Devices.OrderByDescending(d => d.TimeReady).FirstOrDefault().TimeReady;


        /// <summary>
        /// Parameter of type User<Job>[] witch is a conversion of currentUsers to an Array
        /// </summary>
        public User<Job>[] Users => currentUsers.ToArray();

        /// <summary>
        /// Parameter of type Device<Job>[] witch is a conversion of currentDevices to an Array
        /// </summary>
        public Device<Job>[] Devices => currentDevices.ToArray();

        /// <summary>
        /// Parameter of type int representing the number of devices used in the algorithme ShortestProcessingTime
        /// </summary>
        public int NumberOfDevices => currentDevices.Length;

        /// <summary>
        /// Parameter of type int representing the number of users used in the algorithme LongestProcessingTime
        /// </summary>
        public int NumberOfUsers => currentUsers.Length;

        /// <summary>
        /// Execution of the algorithme ShortestProcessingTime on <paramref name="jobs"/> with one device
        /// </summary>
        /// <param name="jobs"></param> jobs we execute on the algorithme ShortestProcessingTime
        public override void Execute(Job[] jobs) => Execute(jobs, 1);


        /// <summary>
        /// Execution of the algorithme LongestProcessingTime on <paramref name="jobs"/> with <paramref name="nbDevices"/>
        /// </summary>
        /// <param name="jobs"></param> jobs we execute on the algorithme ShortestProcessingTime
        /// <param name="nbDevices"></param> Number of devices used to execute the algorithme ShortestProcessingTime
        public void Execute(Job[] jobs, int nbDevices)
        {
            Init(jobs);
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
        }

        /// <summary>
        /// Execution of the algorithme SortestProcessingTime with <paramref name="users"/> and one device
        /// </summary>
        /// <param name="users"></param> users used to execute the algorithme ShortestProcessingTime
        public void Execute(User<Job>[] users) => Execute(users, 1); 
        //{
        //    Job[] jobs = currentUsers.SelectMany(u => u.Jobs).ToArray();
        //    Execute(jobs);
         
        //    currentUsers = users;
        //}

        /// <summary>
        /// <summary>
        /// Execution of the algorithme ShortestProcessingTime with <paramref name="users"/> and<paramref name="nbDevices"/> devices
        /// </summary>
        /// <param name="users"></param> Users we use to execute the algorithme ShortestProcessingTime
        /// <param name="nbDevices"></param> Number of devices used to execute the algorithme
        public void Execute(User<Job>[] users, int nbDevices)
        {
            Job[] jobs = currentUsers.SelectMany(u => u.Jobs).ToArray();
            
            Execute(jobs, nbDevices);
            currentUsers = users;
        }

        //public override void Draw(Canvas c)
        //{
        //    Drawer dr = new Drawer(c, currentUsers.Length, currentDevices.Length);
        //    foreach (Job j in currentJobs)
        //    {
        //        bool isLate = late.Contains(j);

        //        int userIndex = currentUsers.Select(u => u.Contains(j)).ToList().IndexOf(true);
        //        int deviceIndex = currentDevices.Select(d => d.Contains(j)).ToList().IndexOf(true);

        //        dr.AddJob(j, isLate, userIndex, deviceIndex);
        //    }
        //}
    }
}