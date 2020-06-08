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
    class HigherTimeDegree : Algorithme<JobCo>
    {

        public override JobCo[] Execute(JobCo[] jobs)
        {
            GraphTimeD G = new GraphTimeD(jobs);
            List<JobCo> final = new List<JobCo>();
            JobCo higher;
            while ((higher = G.GetHigherOutDegree()) != null)
            {
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
