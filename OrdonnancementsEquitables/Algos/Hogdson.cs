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

        private User<Job>[] currentUsers;
        private readonly List<Job> OnTime;
        private readonly List<Job> Late;
        private int C;

        public User<Job>[] Users => (User<Job>[])currentUsers.Clone();

        public Hogdson()
        {
            C = 0;
            OnTime = new List<Job>();
            Late = new List<Job>();
        }

        public override Job[] Execute(Job[] jobs)
        {
            C = 0;
            OnTime.Clear();
            Late.Clear();
            currentUsers = null;

            currentJobs = (Job[])jobs.Clone();
            foreach (Job job in currentJobs)
            {
                OnTime.Add(job);
                C += job.Time;

                if (C > job.Deadline)
                {
                    Job biggest = OnTime.OrderByDescending(j => j.Time).FirstOrDefault();
                    OnTime.Remove(biggest);
                    Late.Add(biggest);
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
            int nbUsers = currentUsers == null ? 0 : currentUsers.Length;
            Drawer dr = new Drawer(1, c, nbUsers);

            foreach (Job j in OnTime)
            {
                User<Job> user = currentUsers.Where(u => u.Jobs.Contains(j)).FirstOrDefault();
                int index = Array.IndexOf(currentUsers, user);
                dr.AddJob(j, false, index);
            }

            foreach (Job j in Late)
            {
                User<Job> user = currentUsers.Where(u => u.Jobs.Contains(j)).FirstOrDefault();
                int index = Array.IndexOf(currentUsers, user);
                dr.AddJob(j, true, index);
            }
        }

        public override string ToString() => base.ToString() + "Hogdson\n\nOn time:\n" + FormattedOnTime + "\nLate:\n" + FormattedLate + Separation;
    }
}
