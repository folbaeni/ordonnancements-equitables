using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdonnancementsEquitables.Jobs
{
    public class Job
    {
        static int cpt = 0;

        public int Time { get; }
        public int Id { get; }
        public int Deadline { get; }

        private Job() { }

        public Job(int time, int deadline)
        {
            Id = cpt++;
            Time = time;
            Deadline = deadline;
        }

        protected void CountToZero() => cpt = 0;

        protected virtual string JobType() => "Job";
        protected virtual string Prefixe() => $"{JobType()}(Id: {Id}, Time: {Time}, Deadline: {Deadline}";
        public override string ToString() => Prefixe() + ")";
    }
}
