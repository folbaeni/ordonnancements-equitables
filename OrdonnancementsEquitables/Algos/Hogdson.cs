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

        public override Job[] Execute(Job[] jobs)
        {
            Init(jobs);
            int C = 0;
            MaxHeap<Job> heap = new MaxHeap<Job>();

            foreach (Job JobCo in currentJobs)
            {
                heap.Insert(JobCo);
                onTime.Add(JobCo);
                C += JobCo.Time;

                if (C > JobCo.Deadline)
                {
                    //JobCo biggest = onTime.OrderByDescending(j => j.Time).FirstOrDefault();
                    Job biggest = heap.RemoveMax();
                    onTime.Remove(biggest);
                    late.Add(biggest);
                    C -= biggest.Time;
                }
            }
            return jobs;
        }

        public Job[] Execute(User<Job>[] users)
        {
            Job[] jobs = users.SelectMany(u => u.Jobs).ToArray();
            Job[] res = Execute(jobs);
            currentUsers = users.ToArray();
            return res; 
        }

        public override void Draw(Canvas c)
        {
            Drawer dr = new Drawer(c, currentUsers.Length, currentDevices.Length);

            for (int deviceIndex = 0; deviceIndex < currentDevices.Length; deviceIndex++)
            {
                Device<Job> device = currentDevices[deviceIndex];
                foreach (Job job in device.Jobs)
                {
                    User<Job> user = currentUsers.Where(u => u.Jobs.Contains(job)).FirstOrDefault();
                    int userIndex = Array.IndexOf(currentUsers, user);

                    bool isLate = late.Contains(job);

                    dr.AddJob(job, isLate, userIndex, deviceIndex);
                }
            }

            //foreach (Job j in onTime)
            //{
            //    User<Job> user = currentUsers.Where(u => u.Jobs.Contains(j)).FirstOrDefault();
            //    int index = Array.IndexOf(currentUsers, user);

            //    int deviceIndex = currentDevices.Select(d => d.Contains(j)).ToList().IndexOf(true);

            //    dr.AddJob(j, false, index);
            //}

            //foreach (Job j in late)
            //{
            //    User<Job> user = currentUsers.Where(u => u.Jobs.Contains(j)).FirstOrDefault();
            //    int index = Array.IndexOf(currentUsers, user);
            //    dr.AddJob(j, true, index);
            //}
        }

        public override string ToString() => base.ToString() + "Hogdson\n\nOn time:\n" + FormattedOnTime + "\nLate:\n" + FormattedLate + Separation;
    }
}
