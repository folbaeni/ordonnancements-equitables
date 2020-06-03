using OrdonnancementsEquitables.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace OrdonnancementsEquitables.Models
{
    public abstract class Graph
    {
        protected JobCo[] Jobs;
        protected int[,] M; /* matrice sommets-sommets */

        public Graph(JobCo[] jobs)
        {
            Jobs = jobs;
            M = new int[jobs.Length, jobs.Length];
            for(int i = 0; i < jobs.Length; i++)
                for(int j = 0; j < jobs.Length; j++)
                    M[i, j] = 0;

            foreach(JobCo j in jobs)
                CreateConnections(j);
        }

        protected void CreateConnection(JobCo job1, JobCo job2) 
        {
            M[job1.Id, job2.Id] = 1;
            if (M[job2.Id, job1.Id] != 1)
                M[job2.Id, job1.Id] = -1;
        }

        protected void CreateConnections(JobCo job)
        {
            foreach (int i in job.Depend)
                CreateConnection(Jobs.Where(j => j.Id == i).FirstOrDefault(), job);
        }

        protected void DeleteConnection(JobCo job1, JobCo job2)
        {
            M[job1.Id, job2.Id] = 0;
            if (M[job2.Id, job1.Id] == 1)
                M[job1.Id, job2.Id] = -1;
            else
                M[job2.Id, job1.Id] = 0;
        }

        protected void DeleteConnections(JobCo job)
        {
            foreach(int i in job.Depend)
                DeleteConnection(Jobs.Where(j => j.Id == i).FirstOrDefault(), job);
        }

        public void UnExecuteJob(JobCo job)
        {
            CreateConnections(job);
            ActualiseConnectedJobs(job);
        }

        public void ExecuteJob(JobCo job)
        {
            DeleteConnections(job);
            ActualiseConnectedJobs(job);
        }

        public abstract void ActualiseConnectedJobs(JobCo job);

        public JobCo GetHigherDegree()
        {
            return Jobs.OrderByDescending(j => j.Depend.Length).FirstOrDefault();
        }
    }

    public class GraphLock : Graph
    {
        public GraphLock(JobCo[] jobs)
            : base(jobs)
        { }
        
        public override void ActualiseConnectedJobs(JobCo jobCo)
        {
            foreach (JobCo job in Jobs.Where(j => j.Depend.Contains(jobCo.Id)))
                job.ActualiseIsLocked(M);
        }
    }

    public class GraphTimeD : Graph
    {
        public GraphTimeD(JobCo[] jobs)
            : base(jobs)
        { }

        public override void ActualiseConnectedJobs(JobCo jobCo)
        {
            foreach (JobCo job in Jobs.Where(j => j.Depend.Contains(jobCo.Id)))
                job.ActualiseExecTime(M);
        }
    }
}
