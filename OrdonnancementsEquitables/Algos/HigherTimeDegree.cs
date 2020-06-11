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
    public class HigherTimeDegree : Algorithme<JobCo>
    {
        protected override void Init(JobCo[] jobs)
        {
            base.Init(jobs);
        }

        public override JobCo[] Execute(JobCo[] jobs)
        {
            int C = 0;
            GraphTimeD G = new GraphTimeD(jobs);
            List<JobCo> final = new List<JobCo>();
            JobCo higher;
            while ((higher = G.GetHigherOutDegreeOnTime(C)) != null)
            {
                C += higher.ExecTime;
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
