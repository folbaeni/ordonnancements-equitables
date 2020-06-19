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
        private int _execTime;
        private bool _isLocked;

        public int[] Depend { get; } /* JobCos dont il depend */
        public int ExecTime { get => _execTime; }
        public bool IsLocked { get => _isLocked; }
        
        public JobCo(int time, int deadline, int[] depend) : base(time, deadline)
        {
            Depend = depend.ToArray();
            _execTime = Time;
            _isLocked = false;
        }

        public bool ActualiseIsLocked(List<int>[] L)
        {
            foreach (var id in Depend)
            {
                if (L[id].Contains(Id))
                    return _isLocked = true;
            }

            return _isLocked = false;
        }

        public int ActualiseExecTime(List<int>[] L)
        {
            int time = Time;
            foreach (int id in Depend)
                if (!L[id].Contains(Id))
                    time--;

            return _execTime = Math.Max(time, 1);
        }


        protected override string JobType() => "JobCo";
        protected override string Prefixe() => base.Prefixe() + $", Depend: [ {string.Join(", ", Depend)} ]";
    }
}
