using OrdonnancementsEquitables.Drawing;
using OrdonnancementsEquitables.Jobs;
using OrdonnancementsEquitables.Models;
using OrdonnancementsEquitables.Parsers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OrdonnancementsEquitables.Algos
{
    public class GloutonParProfits : Algorithm<JobP>, IMultipleUsers<JobP>
    {
        /// <summary>
        /// The only device used for this algorithm.
        /// </summary>
        /// <value>
        /// Alias for the first element of <see cref="Algorithm{TJob}.currentDevices"/>.
        /// </value>
        private Device<JobP> MainDevice => currentDevices[0];
        /// <summary>
        /// Parameter of type int used to know the final profit of Glouton Par Profit.
        /// </summary>
        public int Profit { get; private set; }

        public int NumberOfUsers => currentUsers.Length;
        public User<JobP>[] Users => currentUsers.ToArray();

        /// <summary>
        /// Creates a new GloutonParProfits algorithm
        /// </summary>
        public GloutonParProfits() { }

        protected override void Init(JobP[] jobs)
        {
            base.Init(jobs);
            Profit = 0;
        }

        public override void Execute(JobP[] jobs)
        {
            Init(jobs);
            JobP tmp;
            for (int i = 0; i < currentJobs.Length; i++) /*boucle sur le temps*/
            {
                for (int j = i; j < currentJobs.Length; j++) /* parcours du tableau */
                {
                    if (currentJobs[j].Deadline < currentJobs[i].Deadline)
                    {
                        tmp = currentJobs[i];
                        currentJobs[i] = currentJobs[j];
                        currentJobs[j] = tmp;
                    }
                } /* On a ordonnancé selon le principe glouton par profits*/

                if (currentJobs[i].Deadline > MainDevice.TimeReady)
                {
                    Profit += currentJobs[i].Profit;
                    onTime.Add(currentJobs[i]);
                }
                else
                    late.Add(currentJobs[i]);
                MainDevice.AddJob(currentJobs[i]);
            }
        }

        public void Execute(User<JobP>[] users)
        {
            JobP[] jobs = users.SelectMany(u => u.Jobs).ToArray();
            Execute(jobs);

            currentUsers = users;
        }
    }
}
