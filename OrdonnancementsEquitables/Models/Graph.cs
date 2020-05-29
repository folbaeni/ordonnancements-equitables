using OrdonnancementsEquitables.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace OrdonnancementsEquitables.Models
{
    public abstract class Graph
    {
        protected JobCo[] Jobs;
        protected int[,] M; /* matrice sommets-sommets du graph orienté*/

        public Graph(JobCo[] jobs)
        {
            Jobs = jobs;
            M = new int[jobs.Length, jobs.Length];
            for(int i = 0; i < jobs.Length; i++)
                for(int j = 0; j < jobs.Length; j++)
                    M[i, j] = 0;

            foreach(JobCo j in jobs)
                CreateConnection(j);

        }

        protected void CreateConnection(JobCo job)
        {
            foreach (int i in job.Depend)
            {
                M[job.Id, i] = 1;
                if (M[i, job.Id] != 1)
                    M[i, job.Id] = -1;
            }
        }

        protected void DeleteConnection(JobCo job)
        {
            foreach (int i in job.Depend)
            {
                M[job.Id, i] = 0;
                M[i, job.Id] = 0;
            }
        }

        public void UnExecuteJob(JobCo job)
        {
            CreateConnection(job);
            ActualiseConnectedJobs(job);
        }

        public void ExecuteJob(JobCo job)
        {
            DeleteConnection(job);
            ActualiseConnectedJobs(job);
        }

        public abstract void ActualiseConnectedJobs(JobCo job);
    }

    public class GraphLock : Graph
    {
        public GraphLock(JobCo[] jobs)
            : base(jobs)
        { }
        
        public override void ActualiseConnectedJobs(JobCo jobCo)
        {
            foreach (int id in jobCo.Depend)
            {
                JobCo job = Jobs.Where(j => j.Id == id).FirstOrDefault();
                job.IsLocked = IsLocked(job);
            }
        }

        public bool IsLocked(JobCo job)
        {
            foreach (int i in job.Depend)
            {
                if (M[job.Id, i] != 0)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class GraphTimeD : Graph
    {
        public GraphTimeD(JobCo[] jobs)
            : base(jobs)
        { }

        public override void ActualiseConnectedJobs(JobCo jobCo)
        {
            foreach (int id in jobCo.Depend)
            {
                JobCo job = Jobs.Where(j => j.Id == id).FirstOrDefault();
                job.ExecTime = TimeDecrease(job);
            }
        }

        public int TimeDecrease(JobCo job)
        {
            int time = job.Time;
            foreach (int i in job.Depend)
            {
                if (M[i, job.Id] == 0)
                {
                    time--;
                }
            }
            return (int)Math.Max(time, 1);
        }
    }
}
