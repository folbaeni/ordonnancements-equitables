using OrdonnancementsEquitables.Jobs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        /// <summary>
        /// Vertices-vertices matrix of the graph. <br/>
        /// If j1 -> j2 then M[j1.Id, j2.Id] = 1 and M[j1.Id, j2.Id] = -1 <br/>
        /// If also j1 <- j2 then M[j1.Id, j2.Id] = 1 and M[j1.Id, j2.Id] = 1 
        /// </summary>
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

        /// <summary>
        /// Creates the edge from <paramref name="job1"/> to <paramref name="job2"/>
        /// </summary>
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


        /* Cherche un job de dégré sortant *deg* qui est pas Lock et qui a le plus petit ExecTime; sinon renvoie null */
        protected JobCo GetHigherOutDegree(int deg)
        {
            int k;
            List<JobCo> jobDeg = new List<JobCo>();
            for (int i = 0; i < M.GetLength(0); i++)
            {
                k = 0;
                for (int j = 0; j < M.GetLength(1); j++)
                {
                    if (M[i, j] == 1)
                        k++;
                }

                if (k == deg)
                    jobDeg.Add(Jobs.Where(j => j.Id == i).FirstOrDefault());
            }

            if (jobDeg.Count == 0 && jobDeg.Where(j => j.IsLocked == false).Count() == 0)
                return null;

            return jobDeg.Where(j => j.IsLocked == false).OrderBy(j => j.ExecTime).FirstOrDefault();
        }

        public JobCo GetHigherOutDegree() /* retourne le job avec le plus haut degree sortant */
        {
            JobCo higher;
            for(int i = M.GetLength(0); i > 0; i--)
            {
                if((higher = GetHigherOutDegree(i)) != null)               
                    return higher;               
            }
            return null;
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
