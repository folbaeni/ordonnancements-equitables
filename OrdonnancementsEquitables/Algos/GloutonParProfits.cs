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

    public class GloutonParProfits : Algorithme<JobP>, IMultipleUsers<JobP>
    {

        /// <summary>
        /// Parameter of type Device<JobP> used to stock the jobs for the algorithme witch uses only one device here 
        /// </summary>
        private Device<JobP> MainDevice => currentDevices[0];
        /// <summary>
        /// Parameter of type int used to know the final profit of Glouton Par Profit
        /// </summary>
        private int Profit;

        /// <summary>
        /// Parameter of type int used to know how many users are using GloutonParrofit
        /// </summary>
        public int NumberOfUsers => currentUsers.Length;

        /// <summary>
        /// Parameter of type User<JobP>[] used to stock the current users in an Array
        /// </summary>
        public User<JobP>[] Users => currentUsers.ToArray();

        /// <summary>
        /// Constructor of GloutonParProfit, sets Profit at 0
        /// </summary>
        public GloutonParProfits()
        {
            Profit = 0;
        }

        //public override JobP[] ExecuteDefault() => Execute(Parser.ParseFromContent<JobP>(Properties.Resources.GloutonParProfits));

        /// <summary>
        /// Execute the algorithme GloutonParProfit with <paramref name="jobs"/> and one device
        /// </summary>
        /// <param name="jobs"></param> used to execute GloutonParProfit
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

        /// <summary>
        /// Method used to execute GloutonParProfit with many users
        /// </summary>
        /// <param name="users"></param> is the table of users used to execute GloutonParPrrofit
        public void Execute(User<JobP>[] users)
        {
            JobP[] jobs = users.SelectMany(u => u.Jobs).ToArray();
            Execute(jobs);

            currentUsers = users;
        }

        //public override void Draw(Canvas c)
        //{
        //    Drawer dr = new Drawer(c, currentUsers.Length);

        //    foreach (JobP jobP in currentJobs)
        //    {
        //        User<JobP> user = currentUsers.Where(u => u.Jobs.Contains(jobP)).FirstOrDefault();
        //        int index = Array.IndexOf(currentUsers, user);
        //        bool isLate = late.Contains(jobP);

        //        dr.AddJob(jobP, isLate, index);
        //    }
        //}

        //public override string ToString() => base.ToString() + "Glouton Par Profit\nListe triée:\n" + FormattedJobs + "\nProfit obtenu = " + Profit + Separation;
    }
}
