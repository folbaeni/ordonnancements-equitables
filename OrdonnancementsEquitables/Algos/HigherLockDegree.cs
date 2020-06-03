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
        public override void Draw(Canvas c)
        {
            throw new NotImplementedException();
        }

        public override JobCo[] Execute(JobCo[] jobs)
        {
            throw new NotImplementedException();
        }

        /*public override JobCo[] Execute(JobCo[] jobs)
        {
            GraphLock G = new GraphLock(jobs);
            JobCo[] trie = new JobCo[];
            JobCo tmp;
            int parcouru = jobs.Length;
            if (parcouru == 1)
                return jobs[0];

            while(parcouru > 0)
            {
                tmp = G.GetHigherDegree();
                if (tmp.IsLocked)
                {
                    JobCo[] depend = new JobCo[];
                    foreach(int id in tmp.Depend)
                    {
                        depend.Append(jobs.Where(j => j.Id = id));
                    }
                    G.ExecuteJob(tmp);
                }
            }            
        }
    }*/
    }