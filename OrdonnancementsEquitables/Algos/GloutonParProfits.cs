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
        private int Time; /* temps total */
        private int Profit;

        public User<JobP>[] Users => currentUsers.ToArray();

        public GloutonParProfits()
        {
            Time = 0;
            Profit = 0;
        }

        //public override JobP[] ExecuteDefault() => Execute(Parser.ParseFromContent<JobP>(Properties.Resources.GloutonParProfits));

        public override JobP[] Execute(JobP[] jobs)
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

                if (currentJobs[i].Deadline > Time)
                {
                    Profit += currentJobs[i].Profit;
                    onTime.Add(currentJobs[i]);
                }
                else
                    late.Add(currentJobs[i]);
                Time += currentJobs[i].Time;
            }
            return Jobs;
        }

        public JobP[] Execute(User<JobP>[] users)
        {
            JobP[] jobs = users.SelectMany(u => u.Jobs).ToArray();
            var res = Execute(jobs);
            currentUsers = users;
            return res;
        }

        public override void Draw(Canvas c)
        {
            Drawer dr = new Drawer(c, currentUsers.Length);

            foreach (JobP jobP in currentJobs)
            {
                User<JobP> user = currentUsers.Where(u => u.Jobs.Contains(jobP)).FirstOrDefault();
                int index = Array.IndexOf(currentUsers, user);
                bool isLate = late.Contains(jobP);

                dr.AddJob(jobP, isLate, index);
            }
        }

        public override string ToString() => base.ToString() + "Glouton Par Profit\nListe triée:\n" + FormattedJobs + "\nProfit obtenu = " + Profit + Separation;

    }
}
