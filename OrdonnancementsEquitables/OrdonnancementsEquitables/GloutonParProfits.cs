using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdonnancementsEquitables
{
    class GloutonParProfits : Algorithmes
    {
        private int Time;
        private int Profit { set; get; }
        private JobP[] Jobs { set; get; }

        public GloutonParProfits()
        {
            Time = 0;
            Profit = 0;
            Jobs = new JobP[]
            {
                new JobP(1, 12, 4),
                new JobP(1, 10, 3),
                new JobP(1, 8, 1),
                new JobP(1, 7, 6),
                new JobP(1, 6, 1),
                new JobP(1, 5, 6),
                new JobP(1, 4, 6),
                new JobP(1, 3, 5),
            };
        }

        public GloutonParProfits(JobP[] jobs)
        {
            Time = 0;
            Profit = 0;
            Jobs = jobs;
        }
        public override void Execute()
        {
            JobP tmp;
            for (int i = 0; i < Jobs.Length; i++) /*boucle sur le temps*/
            {
                for (int j = i; j < Jobs.Length; j++) /* parcours du tableau */
                {
                    if (Jobs[j].Deadline < Jobs[i].Deadline)
                    {
                        tmp = Jobs[i];
                        Jobs[i] = Jobs[j];
                        Jobs[j] = tmp;
                    }
                } /* On a ordonnancé selon le principe glouton par profits*/

                Console.WriteLine(Jobs[i] + "\n");

                if (Jobs[i].Deadline > Time)
                {
                    Profit += Jobs[i].Profit;
                }
                Time++;
            }
        }

        public override string ToString()
        {
            String res = "";
            foreach (JobP j in Jobs) res += j.ToString();
            String separation = "####################################";
            return base.ToString() + "GloutonPar Profit:\n" + base.Prefixe() + separation + "Liste triée:`\n" + res + separation + "Profit obtenu = " + Profit;
        }
    }
}
