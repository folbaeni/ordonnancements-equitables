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
    /// <summary>
    /// Abstract modeling any basic algorithm.
    /// </summary>
    /// <typeparam name="TJob">Job type accepted by the algorithm</typeparam>
    public abstract class Algorithm<TJob> where TJob : Job
    {
        /// <summary>
        /// Shallow copy of the algorithm's current jobs.
        /// </summary>
        public TJob[] Jobs => currentJobs.ToArray();
        /// <summary>
        /// Shallow copy of the algorithm's on time jobs.
        /// </summary>
        public TJob[] OnTime => onTime.ToArray();
        /// <summary>
        /// Shallow copy of the algorithm's late jobs.
        /// </summary>
        public TJob[] Late => late.ToArray();


        protected TJob[] currentJobs;
        protected User<TJob>[] currentUsers;
        protected Device<TJob>[] currentDevices;

        /// <summary>
        /// List of <see cref="TJob"/> containing all on time executed jobs.
        /// </summary>
        protected List<TJob> onTime;
        /// <summary>
        /// List of <see cref="TJob"/> containing all late executed jobs.
        /// </summary>
        protected List<TJob> late;

        /// <summary>
        /// Creates a new Algorithm.
        /// </summary>
        public Algorithm()
        {
            onTime = new List<TJob>(); 
            late = new List<TJob>();
        }

        /// <summary>
        /// Initialization method called before every execution/>.
        /// </summary>
        /// <param name="jobs">Jobs passed as the execution parameter.</param>
        protected virtual void Init(TJob[] jobs)
        {
            onTime.Clear();
            late.Clear();
            currentJobs = (TJob[])jobs.Clone();
            currentUsers = new User<TJob>[] { new User<TJob>(jobs) };
            currentDevices = new Device<TJob>[] { new Device<TJob>() };
        }

        /// <summary>
        /// Default execution for the algorithme from a .json file.
        /// </summary>
        public void ExecuteDefault() => Execute(new Parser($@"Assets\Default Jobs\{GetType().Name}.json").ParseJobsFromJSON<TJob>());
        
        /// <summary>
        /// Execute the algorithm with <paramref name="jobs"/> on one device.
        /// </summary>
        /// <param name="jobs">Array of jobs to execute.</param>
        public abstract void Execute(TJob[] jobs);

        /// <summary>
        /// Draws the algorithm execution on <paramref name="canvas"/>.
        /// </summary>
        /// <param name="canvas">The canvas on where to draw.</param>
        public virtual void Draw(Canvas canvas)
        {
            Drawer dr = new Drawer(canvas, currentUsers.Length, currentDevices.Length);

            for (int deviceIndex = 0; deviceIndex < currentDevices.Length; deviceIndex++)
            {
                Device<TJob> device = currentDevices[deviceIndex];
                foreach (TJob job in device.Jobs)
                {
                    User<TJob> user = currentUsers.Where(u => u.Contains(job)).FirstOrDefault();
                    int userIndex = Array.IndexOf(currentUsers, user);

                    bool isLate = late.Contains(job);

                    dr.AddJob(job, isLate, userIndex, deviceIndex);
                }
            }
        }
    }
}
