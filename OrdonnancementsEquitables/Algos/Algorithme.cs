using OrdonnancementsEquitables.Drawing;
using OrdonnancementsEquitables.Jobs;
using OrdonnancementsEquitables.Models;
using OrdonnancementsEquitables.Parsers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace OrdonnancementsEquitables.Algos
{
    public abstract class Algorithme<TJob> where TJob : Job
    {
        /// <summary>
        /// Public parameter of type TJob[] containing the currentJobs of the algorithme
        /// </summary>
        public TJob[] Jobs => currentJobs.ToArray();
        /// <summary>
        /// Public parameter of type TJob[] containing the jobs witch are on time
        /// </summary>
        public TJob[] OnTime => onTime.ToArray();
        /// <summary>
        /// Public parameter of type TJob[] containing the late jobs 
        /// </summary>
        public TJob[] Late => late.ToArray();

        /// <summary>
        /// Protected parameter of type TJob containing the jobs of the algorithme
        /// </summary>
        protected TJob[] currentJobs;

        /// <summary>
        /// Protected parameter of type User<TJob>[] containing the users
        /// </summary>
        protected User<TJob>[] currentUsers;

        /// <summary>
        /// Protected parameter of type Device<TJob>[] containing the devices
        /// </summary>
        protected Device<TJob>[] currentDevices;

        /// <summary>
        /// Parameters of type List<TJob> containing on time and late jobs
        /// </summary>
        protected List<TJob> onTime, late;

        /// <summary>
        /// Initialize new lists of TJobs OnTime and Late
        /// </summary>
        public Algorithme()
        {
            onTime = new List<TJob>(); 
            late = new List<TJob>();
        }

        /// <summary>
        /// Initialize the lists of jobs of the algorithme with <paramref name="jobs"/>
        /// </summary>
        /// <param name="jobs"></param> jobs to initialize
        protected virtual void Init(TJob[] jobs)
        {
            onTime.Clear();
            late.Clear();
            currentJobs = (TJob[])jobs.Clone();
            currentUsers = new User<TJob>[] { new User<TJob>(jobs) };
            currentDevices = new Device<TJob>[] { new Device<TJob>() };
        }

        /// <summary>
        /// Execute the jobs of a .json file
        /// </summary>
        public void ExecuteDefault() => Execute(new Parser($@"Assets\Default Jobs\{GetType().Name}.json").ParseJobsFromJSON<TJob>());
        
        /// <summary>
        /// Execute the algorithme with the TJob[] <paramref name="jobs"/>
        /// </summary>
        /// <param name="jobs"></param>
        public abstract void Execute(TJob[] jobs);

        /// <summary>
        /// Creates the image of the execution of the algorithme
        /// </summary>
        /// <param name="c"></param>
        public virtual void Draw(Canvas c)
        {
            Drawer dr = new Drawer(c, currentUsers.Length, currentDevices.Length);

            for (int deviceIndex = 0; deviceIndex < currentDevices.Length; deviceIndex++)
            {
                Device<TJob> device = currentDevices[deviceIndex];
                foreach (TJob job in device.Jobs)
                {
                    User<TJob> user = currentUsers.Where(u => u.Jobs.Contains(job)).FirstOrDefault();
                    int userIndex = Array.IndexOf(currentUsers, user);

                    bool isLate = late.Contains(job);

                    dr.AddJob(job, isLate, userIndex, deviceIndex);
                }
            }
        }
    }
}
