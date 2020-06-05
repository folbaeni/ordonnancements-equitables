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

        public int[] Depend { get; } /* jobs dont il depend */
        public int ExecTime { get => execTime; }
        public bool IsLocked { get => isLocked; }
        
        public JobCo(int time, int deadline, int[] depend) : base(time, deadline)
        {
            Depend = (int[])depend.Clone();
            execTime = Time;
            isLocked = false;
        }

        public JobCo(int time, int execTime)
            : base(time, 40)
        {
            this.execTime = execTime;
            isLocked = false;
        }

        public bool ActualiseIsLocked(int[,] M)
        {
            foreach (int i in Depend)
                if (M[Id, i] == -1)
                    return isLocked = true;

            return isLocked = false;
        }

        public int ActualiseExecTime(int[,] M)
        {
            int time = Time;
            foreach (int i in Depend)
                if (M[i, Id] == 0)
                    time--;

            return execTime = Math.Max(time, 1);
        }


        protected override string JobType() => "JobCo";
        protected override string Prefixe() => base.Prefixe() + $", Depend: {Depend}";

    }
}
