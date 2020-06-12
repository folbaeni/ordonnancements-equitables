using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OrdonnancementsEquitables.Jobs
{
    public class JobCo : Job
    {
        private int execTime;
        private bool isLocked;

        public int[] Depend { get; } /* JobCos dont il depend */
        public int ExecTime { get => execTime; }
        public bool IsLocked { get => isLocked; }
        
        public JobCo(int time, int deadline, int[] depend) : base(time, deadline)
        {
            Depend = depend.ToArray();
            execTime = Time;
            isLocked = false;
        }

        public JobCo(int time, int execTime)
            : base(time, 40)
        {
            this.execTime = execTime;
            isLocked = false;
        }

        public bool ActualiseIsLocked(List<int>[] L)
        {
            foreach (var id in Depend)
            {
                if (L[id].Contains(Id))
                    return isLocked = true;
            }

            return isLocked = false;
        }

        public int ActualiseExecTime(List<int>[] L)
        {
            int time = Time;
            foreach (int id in Depend)
                if (!L[id].Contains(Id))
                    time--;

            return execTime = Math.Max(time, 1);
        }


        protected override string JobType() => "JobCoCo";
        protected override string Prefixe() => base.Prefixe() + $", Depend: {Depend}";
    }
}
