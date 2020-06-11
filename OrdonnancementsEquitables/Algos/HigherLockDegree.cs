using OrdonnancementsEquitables.Jobs;
using OrdonnancementsEquitables.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OrdonnancementsEquitables.Algos
{
    public class HigherLockDegree : Algorithme<JobCo>
    {

        public override JobCo[] Execute(JobCo[] jobs)
        {
            int C = 0;
            GraphLock G = new GraphLock(jobs);
            List<JobCo> final = new List<JobCo>();
            JobCo higher;
            while((higher = G.GetHigherOutDegreeOnTime(C)) != null)
            {
                C += higher.Time;
                final.Add(higher);
                G.ExecuteJob(higher);
            }

            currentJobs = final.ToArray();
            return Jobs;
        }

        public override void Draw(Canvas c)
        {
            throw new NotImplementedException();
        }
    }
}