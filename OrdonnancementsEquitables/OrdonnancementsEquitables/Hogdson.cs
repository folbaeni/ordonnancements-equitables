using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdonnancementsEquitables
{
    class Hogdson : Algorithmes
    {
        private List<Job> Ontime { get; }
        private List<Job> Late { get; }
        private int C;

        private Job[] Jobs { set; get; }

        public Hogdson()
        {
            C = 0;
            Ontime = new List<Job>();
            Late = new List<Job>();
            Jobs = new Job[6]
            {
                new Job(6, 8),
                new Job(4, 9),
                new Job(7, 15),
                new Job(8, 20),
                new Job(3, 21),
                new Job(5, 22)
            };
        }

        public Hogdson(Job[] jobs)
        {
            C = 0;
            Ontime = new List<Job>();
            Late = new List<Job>();
            Jobs = jobs;
        }
        public override void Execute()
        {
            base.Execute();
            foreach (Job job in Jobs)
            {
                Ontime.Add(job);
                C += job.Time;

                if (C > job.Deadline)
                {
                    Job biggest = Ontime.OrderByDescending(j => j.Time).FirstOrDefault();
                    Ontime.Remove(biggest);
                    Late.Add(biggest);
                    C -= biggest.Time;
                }
            }
        }

        public override string ToString()
        {
            String res = "";
            foreach(Job j in Jobs)res += j.ToString();
            res += "####################################\n On time:\n";
            foreach (Job j in Ontime) res += j.ToString();
            res += "####################################\n Late:\n";
            foreach (Job j in Late) res += j.ToString();
            return base.ToString()  + " Hogdson\n " + base.Prefixe() + res;
        }


    }
}
