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
    public class HigherTimeDegree : Algorithme<JobCo>, IMultipleUsers<JobCo>
    {
        public User<JobCo>[] Users => currentUsers.ToArray();

        public override JobCo[] Execute(JobCo[] JobCos)
        {
            Init(JobCos);

            int C = 0;
            GraphTimeD G = new GraphTimeD(JobCos);
            JobCo higher;
            while ((higher = G.GetHigherOutDegreeOnTime(C)) != null)
            {
                C += higher.ExecTime;
                onTime.Add(higher);
                G.ExecuteJobCo(higher);
            }

            late = G.GetAllLeftJobCos();
            return JobCos;
        }

        public JobCo[] Execute(User<JobCo>[] users)
        {
            JobCo[] jobs = users.SelectMany(u => u.Jobs).ToArray();
            JobCo[] res = Execute(jobs);
            currentUsers = users.ToArray();
            return res;
        }

        public override void Draw(Canvas c)
        {
            throw new NotImplementedException();
        }
    }
}
