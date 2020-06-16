using OrdonnancementsEquitables.Jobs;
using OrdonnancementsEquitables.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrdonnancementsEquitables.Models
{
    public abstract class Graph
    {
        protected JobCo[] Jobs;
        protected List<JobCo> leftJobs;
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
            leftJobs = jobs.ToList();
            L = new List<int>[Jobs.Length];
            for (int i = 0; i < L.Length; i++)
                L[i] = new List<int>();
            //M = new int[JobCos.Length, JobCos.Length];
            //for(int i = 0; i < JobCos.Length; i++)
            //    for(int j = 0; j < JobCos.Length; j++)
            //        M[i, j] = 0;

            foreach(JobCo j in Jobs)
                CreateConnections(j);
        }

        /// <summary>
        /// Creates the edge from <paramref name="jobCo1"/> to <paramref name="jobCo2"/>
        /// </summary>
        protected void CreateConnection(JobCo jobCo1, JobCo jobCo2) 
        {
            //M[JobCo1.Id, JobCo2.Id] = 1;
            //if (M[JobCo2.Id, JobCo1.Id] != 1)
            //    M[JobCo2.Id, JobCo1.Id] = -1;

            L[jobCo1.Id].Add(jobCo2.Id);
        }

        protected void CreateConnections(JobCo job)
        {
            foreach (int id in job.Depend)
                CreateConnection(Jobs.FromId(id), job);
        }

        protected void DeleteConnection(JobCo jobCo1, JobCo jobCo2)
        {
            //M[JobCo1.Id, JobCo2.Id] = 0;
            //if (M[JobCo2.Id, JobCo1.Id] == 1)
            //    M[JobCo1.Id, JobCo2.Id] = -1;
            //else
            //    M[JobCo2.Id, JobCo1.Id] = 0;
            bool b = L[jobCo1.Id].Remove(jobCo2.Id);
        }

        protected void DeleteConnections(JobCo job)
        {
            L[job.Id].Clear();
            foreach(int id in job.Depend)
                DeleteConnection(Jobs.FromId(id), job);
        }

        public void UnExecuteJob(JobCo jobCo)
        {
            CreateConnections(jobCo);
            ActualiseConnectedJobs(jobCo);
        }

        public void ExecuteJob(JobCo jobCo)
        {
            leftJobs.Remove(jobCo);
            DeleteConnections(jobCo);
            ActualiseConnectedJobs(jobCo);
        }

        public abstract void ActualiseConnectedJobs(JobCo JobCo);

        /* Cherche un JobCo de dégré sortant *deg* qui est pas Lock, dans les temps et qui a le plus petit ExecTime; sinon renvoie null */
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
            //    JobCodeg.add(JobCos.where(j => j.id == i).firstordefault());
            //}

            List<JobCo> jobCoDeg = new List<JobCo>();
            for (int id = 0; id < L.Length; id++)
            {
                if (L[id].Count == deg)
                    jobCoDeg.Add(Jobs.FromId(id));
            }

            /*if (JobCoDeg.Count == 0 && JobCoDeg.Where(j => j.IsLocked == false).Count() == 0)
                return null;*/

            JobCo onTime;
            jobCoDeg = jobCoDeg.Where(j => j.IsLocked == false).OrderBy(j => j.ExecTime).ToList();

            while (jobCoDeg.Count != 0)
            {
                onTime = jobCoDeg.FirstOrDefault();
                if (time + onTime.ExecTime < onTime.Deadline)
                    return onTime;

                jobCoDeg.RemoveAt(0);
            }

            return null;
        }

        /* retourne le JobCo avec le plus haut degree sortant, dans les temps et le plus petit ExecTime*/
        public JobCo GetHigherOutDegreeOnTime(int time)
        {
            JobCo higher;
            for (int degree = L.Length; degree >= 0; degree--)
            {
                if ((higher = GetHigherOutDegreeOnTime(degree, time)) != null)
                    return higher;
            }
            return null;
        }

        public List<JobCo> GetAllLeftJobs() => leftJobs.ToList();// Jobs.Where(j => L[j.Id].Count > 0 || j.IsLocked).ToList();
    }

/* GraphLock */

    public class GraphLock : Graph
    {
        public GraphLock(JobCo[] JobCos)
            : base(JobCos)
        { }
        
        public override void ActualiseConnectedJobs(JobCo JobCoCo)
        {
            foreach (JobCo JobCo in Jobs.Where(j => j.Depend.Contains(JobCoCo.Id)))
                JobCo.ActualiseIsLocked(L);
        }

    }


/* GraphTimeD */

    public class GraphTimeD : Graph
    {
        public GraphTimeD(JobCo[] JobCos)
            : base(JobCos)
        { }

        public override void ActualiseConnectedJobs(JobCo JobCoCo)
        {
            foreach (JobCo JobCo in Jobs.Where(j => j.Depend.Contains(JobCoCo.Id)))
                JobCo.ActualiseExecTime(L);
        }
    }
}
