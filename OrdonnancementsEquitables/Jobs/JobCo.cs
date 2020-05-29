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
        public int[] Depend { get; }
        public int ExecTime { get; set; }
        public bool IsLocked { get; set; }
        
        public JobCo(int time, int deadline, int[] depend) : base(time, deadline)
        {
            Depend = depend;
        }
    }
}
