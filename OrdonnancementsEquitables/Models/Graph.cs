using OrdonnancementsEquitables.Jobs;
using OrdonnancementsEquitables.Utils;
using System.Collections.Generic;
using System.Linq;

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
        //protected int[,] M; /* matrice sommets-sommets */
        
        protected List<int>[] L;

        public Graph(JobCo[] jobs)
        {
            Jobs = jobs;
            L = new List<int>[jobs.Length];
            L.Initialize();             
            //M = new int[jobs.Length, jobs.Length];
            //for(int i = 0; i < jobs.Length; i++)
            //    for(int j = 0; j < jobs.Length; j++)
            //        M[i, j] = 0;

            foreach(JobCo j in jobs)
                CreateConnections(j);
        }

        protected void CreateConnections(JobCo job)
        {
            foreach (int id in job.Depend)
                CreateConnection(Jobs.FromId(id), job);
        }

        /// <summary>
        /// Creates the edge from <paramref name="job1"/> to <paramref name="job2"/>
        /// </summary>
        protected void CreateConnection(JobCo job1, JobCo job2) 
        {
            //M[job1.Id, job2.Id] = 1;
            //if (M[job2.Id, job1.Id] != 1)
            //    M[job2.Id, job1.Id] = -1;

            L[job1.Id].Add(job2.Id);
        }

        protected void DeleteConnections(JobCo job)
        {
            foreach(int id in job.Depend)
                DeleteConnection(Jobs.FromId(id), job);
        }

        protected void DeleteConnection(JobCo job1, JobCo job2)
        {
            //M[job1.Id, job2.Id] = 0;
            //if (M[job2.Id, job1.Id] == 1)
            //    M[job1.Id, job2.Id] = -1;
            //else
            //    M[job2.Id, job1.Id] = 0;
            L[job1.Id].Remove(job2.Id);
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

        /* Cherche un job de dégré sortant *deg* qui est pas Lock, dans les temps et qui a le plus petit ExecTime; sinon renvoie null */
        protected JobCo GetHigherOutDegreeOnTime(int deg, int time)
        {
            //for (int i = 0; i < M.GetLength(0); i++)
            //{
            //    k = 0;
            //    for (int j = 0; j < M.GetLength(1); j++)
            //    {
            //        if (M[i, j] == 1)
            //            k++;
            //    }
            //if (k == deg)
            //    jobdeg.add(jobs.where(j => j.id == i).firstordefault());
            //}

            List<JobCo> jobDeg = new List<JobCo>();
            for (int id = 0; id < L.Length; id++)
            {
                if (L[id].Count == deg)
                    jobDeg.Add(Jobs.FromId(id));
            }

            /*if (jobDeg.Count == 0 && jobDeg.Where(j => j.IsLocked == false).Count() == 0)
                return null;*/

            JobCo onTime;
            jobDeg = jobDeg.Where(j => j.IsLocked == false).OrderBy(j => j.ExecTime).ToList();

            while (jobDeg.Count != 0)
            {
                onTime = jobDeg.FirstOrDefault();
                if (time + onTime.ExecTime < onTime.Deadline)
                    return onTime;

                jobDeg.RemoveAt(0);
            }

            return null;
        }

        /* retourne le job avec le plus haut degree sortant, dans les temps et le plus petit ExecTime*/
        public JobCo GetHigherOutDegreeOnTime(int time)
        {
            JobCo higher;
            for (int degree = L.Length; degree > 0; degree--)
            {
                if ((higher = GetHigherOutDegreeOnTime(degree, time)) != null)
                    return higher;
            }
            return null;
        }

        public List<JobCo> GetAllLeftJobs() => Jobs.Where(j => L[j.Id].Count > 0).ToList();
    }

/* GraphLock */

    public class GraphLock : Graph
    {
        public GraphLock(JobCo[] jobs)
            : base(jobs)
        { }
        
        public override void ActualiseConnectedJobs(JobCo jobCo)
        {
            foreach (JobCo job in Jobs.Where(j => j.Depend.Contains(jobCo.Id)))
                job.ActualiseIsLocked(L);
        }

    }


/* GraphTimeD */

    public class GraphTimeD : Graph
    {
        public GraphTimeD(JobCo[] jobs)
            : base(jobs)
        { }

        public override void ActualiseConnectedJobs(JobCo jobCo)
        {
            foreach (JobCo job in Jobs.Where(j => j.Depend.Contains(jobCo.Id)))
                job.ActualiseExecTime(L);
        }
    }
}
