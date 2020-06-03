using OrdonnancementsEquitables.Drawing;
using OrdonnancementsEquitables.Jobs;
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

    public class GloutonParProfits : Algorithme<JobP>
    {
        private int Time;
        private int Profit;

        public GloutonParProfits() : base()
        {
            Time = 0;
            Profit = 0;
        }

        //public override JobP[] ExecuteDefault() => Execute(Parser.ParseFromContent<JobP>(Properties.Resources.GloutonParProfits));

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

        public override void Draw(Canvas c)
        {

        }

        public override string ToString() => base.ToString() + "Glouton Par Profit\nListe triée:\n" + FormattedJobs + "\nProfit obtenu = " + Profit + Separation;
    }
}
