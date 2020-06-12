using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdonnancementsEquitables.Jobs
{
    public class JobP : Job
    {
        public int Profit { get; }
        public JobP(int time, int deadline, int profit) : base(time, deadline)
        {
            Profit = profit;
        }

        protected override string JobType() => "JobP";
        protected override string Prefixe() => base.Prefixe() + $", Profit: {Profit}";
    }
}
