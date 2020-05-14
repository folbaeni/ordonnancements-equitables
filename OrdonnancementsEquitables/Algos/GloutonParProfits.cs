﻿using OrdonnancementsEquitables.Jobs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdonnancementsEquitables.Algos
{

    public class GloutonParProfits : Algorithme<JobP>
    {
        private int Time;
        private int Profit;

        public GloutonParProfits() : base()
        {
            Time = 0;
            Profit = 0;
        }

        /*public override JobP[] ExecuteDefault() => Execute(new JobP[] {
            new JobP(1, 12, 4),
            new JobP(1, 10, 3),
            new JobP(1, 8, 1),
            new JobP(1, 7, 6),
            new JobP(1, 6, 1),
            new JobP(1, 5, 6),
            new JobP(1, 4, 6),
            new JobP(1, 3, 5)
        });*/

        public override JobP[] Execute(JobP[] jobs)
        {
            currentJobs = (JobP[])jobs.Clone();
            JobP tmp;
            for (int i = 0; i < currentJobs.Length; i++) /*boucle sur le temps*/
            {
                for (int j = i; j < currentJobs.Length; j++) /* parcours du tableau */
                {
                    if (currentJobs[j].Deadline < currentJobs[i].Deadline)
                    {
                        tmp = Jobs[i];
                        currentJobs[i] = Jobs[j];
                        currentJobs[j] = tmp;
                    }
                } /* On a ordonnancé selon le principe glouton par profits*/

                if (currentJobs[i].Deadline > Time)
                {
                    Profit += currentJobs[i].Profit;
                }
                Time++;
            }
            return Jobs;
        }

        public override string ToString() => base.ToString() + "Glouton Par Profit\nListe triée:\n" + FormattedJobs + "\nProfit obtenu = " + Profit + Separation;
    }
}
