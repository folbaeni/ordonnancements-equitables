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
    public class Hogdson : Algorithme<Job>, IMultipleUsers<Job>
    {
        public string FormattedOnTime => string.Join("\n", OnTime.Select(j => j.ToString()));
        public string FormattedLate => string.Join("\n", Late.Select(j => j.ToString()));

        public User<Job>[] Users => currentUsers.ToArray();

        protected override void Init(Job[] jobs)
        {
            base.Init(jobs);
            currentUsers = null;
        }

        public override Job[] Execute(Job[] jobs)
        {
            Init(jobs);
            int C = 0;
            MaxHeap<Job> heap = new MaxHeap<Job>();

            foreach (Job job in currentJobs)
            {
                heap.Insert(job);
                onTime.Add(job);
                C += job.Time;

                if (C > job.Deadline)
                {
                    //Job biggest = onTime.OrderByDescending(j => j.Time).FirstOrDefault();
                    Job biggest = heap.RemoveMax();
                    onTime.Remove(biggest);
                    late.Add(biggest);
                    C -= biggest.Time;
                }
            }
            return Jobs;
        }

        public Job[] Execute(User<Job>[] users)
        {
            Job[] jobs = users.SelectMany(u => u.Jobs).ToArray();
            Job[] res = Execute(jobs);
            currentUsers = (User<Job>[])users.Clone();
            return res; 
        }

        public override void Draw(Canvas c)
        {
            int nbUsers = currentUsers == null ? 1 : currentUsers.Length;
            Drawer dr = new Drawer(1, c, nbUsers);

            foreach (Job j in OnTime)
            {
                int index;
                if (nbUsers == 1)
                    index = 1;
                else 
                {
                    User<Job> user = currentUsers.Where(u => u.Jobs.Contains(j)).FirstOrDefault();
                    index = Array.IndexOf(currentUsers, user);
                }
                dr.AddJob(j, false, index);
            }

            foreach (Job j in Late)
            {
                int index;
                if (nbUsers == 1)
                    index = 1;
                else
                {
                    User<Job> user = currentUsers.Where(u => u.Jobs.Contains(j)).FirstOrDefault();
                    index = Array.IndexOf(currentUsers, user);
                }
                dr.AddJob(j, true, index);
            }
        }

        public override string ToString() => base.ToString() + "Hogdson\n\nOn time:\n" + FormattedOnTime + "\nLate:\n" + FormattedLate + Separation;
    }
}
