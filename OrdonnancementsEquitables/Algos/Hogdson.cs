using OrdonnancementsEquitables.Drawing;
using OrdonnancementsEquitables.Jobs;
using OrdonnancementsEquitables.Models;
using OrdonnancementsEquitables.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OrdonnancementsEquitables.Algos
{
    public class Hogdson : Algorithme<JobCo>, IMultipleUsers<JobCo>
    {
        public string FormattedOnTime => string.Join("\n", OnTime.Select(j => j.ToString()));
        public string FormattedLate => string.Join("\n", Late.Select(j => j.ToString()));

        public User<JobCo>[] Users => currentUsers.ToArray();

        public override JobCo[] Execute(JobCo[] jobs)
        {
            Init(jobs);
            int C = 0;
            MaxHeap<JobCo> heap = new MaxHeap<JobCo>();

            foreach (JobCo job in currentJobs)
            {
                heap.Insert(job);
                onTime.Add(job);
                C += job.Time;

                if (C > job.Deadline)
                {
                    //Job biggest = onTime.OrderByDescending(j => j.Time).FirstOrDefault();
                    JobCo biggest = heap.RemoveMax();
                    onTime.Remove(biggest);
                    late.Add(biggest);
                    C -= biggest.Time;
                }
            }
            return Jobs;
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
            int nbUsers = currentUsers == null ? 1 : currentUsers.Length;
            Drawer dr = new Drawer(c, nbUsers);

            foreach (JobCo j in OnTime)
            {
                int index;
                if (nbUsers == 1)
                    index = 1;
                else 
                {
                    User<JobCo> user = currentUsers.Where(u => u.Jobs.Contains(j)).FirstOrDefault();
                    index = Array.IndexOf(currentUsers, user);
                }
                dr.AddJob(j, false, index);
            }

            foreach (JobCo j in Late)
            {
                int index;
                if (nbUsers == 1)
                    index = 1;
                else
                {
                    User<JobCo> user = currentUsers.Where(u => u.Jobs.Contains(j)).FirstOrDefault();
                    index = Array.IndexOf(currentUsers, user);
                }
                dr.AddJob(j, true, index);
            }
        }

        public override string ToString() => base.ToString() + "Hogdson\n\nOn time:\n" + FormattedOnTime + "\nLate:\n" + FormattedLate + Separation;
    }
}
