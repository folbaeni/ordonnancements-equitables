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
            Init(jobs);

            int C = 0;
            GraphLock G = new GraphLock(jobs);
            JobCo higher;
            while((higher = G.GetHigherOutDegreeOnTime(C)) != null)
            {
                C += higher.Time;
                onTime.Add(higher);
                G.ExecuteJob(higher);
            }

            late = G.GetAllLeftJobs();
            return Jobs;
        }

        public override void Draw(Canvas c)
        {
            throw new NotImplementedException();
        }
    }
}