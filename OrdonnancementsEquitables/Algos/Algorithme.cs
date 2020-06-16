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
        public TJob[] Jobs => currentJobs.ToArray();
        public TJob[] OnTime => onTime.ToArray();
        public TJob[] Late => late.ToArray();

        protected TJob[] currentJobs;
        protected User<TJob>[] currentUsers;
        protected Device<TJob>[] currentDevices;
        protected List<TJob> onTime, late;

        public Algorithme()
        {
            onTime = new List<TJob>(); 
            late = new List<TJob>();
        }

        protected virtual void Init(TJob[] jobs)
        {
            onTime.Clear();
            late.Clear();
            currentJobs = (TJob[])jobs.Clone();
            currentUsers = new User<TJob>[] { new User<TJob>(jobs) };
            currentDevices = new Device<TJob>[] { new Device<TJob>() };
        }

        public void ExecuteDefault() => Execute(new Parser($@"Assets\Default Jobs\{GetType().Name}.json").ParseJobsFromJSON<TJob>());
        public abstract void Execute(TJob[] jobs);

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
