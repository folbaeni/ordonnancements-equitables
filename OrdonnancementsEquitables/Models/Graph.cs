using OrdonnancementsEquitables.Jobs;
using OrdonnancementsEquitables.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrdonnancementsEquitables.Models
{
    public abstract class Graph
    {
        /// <summary>
        /// Class containing orientes graphs
        /// </summary>
        protected JobCo[] Jobs;
        protected List<JobCo> leftJobs;
        /// <summary>
        /// Vertices-vertices matrix of the graph. <br/>
        /// If j1 -> j2 then M[j1.Id, j2.Id] = 1 and M[j1.Id, j2.Id] = -1 <br/>
        /// If also j1 <- j2 then M[j1.Id, j2.Id] = 1 and M[j1.Id, j2.Id] = 1 
        /// </summary>
        //protected int[,] M; /* matrice sommets-sommets */
        
        
        /// <summary>
        /// list of adjencies of the oriented graph
        /// </summary>
        protected List<int>[] L;


        /// <summary>
        /// initialize a new oriented graph with the given jobs
        /// </summary>
        /// <param name="jobs"></param> are the jobs used to create the oriented graph
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
        /// <param name="jobCo1"></param> 
        /// <param name="jobCo2"></param>
        protected void CreateConnection(JobCo jobCo1, JobCo jobCo2) 
        {
            //M[JobCo1.Id, JobCo2.Id] = 1;
            //if (M[JobCo2.Id, JobCo1.Id] != 1)
            //    M[JobCo2.Id, JobCo1.Id] = -1;

            L[jobCo1.Id].Add(jobCo2.Id);
        }


        /// <summary>
        /// Creates connections with all jobs that <paramref name="job"/> depends of
        /// </summary>
        /// <param name="job"></param>The job we want to create all the connection from the jobs he depends of
        protected void CreateConnections(JobCo job)
        {
            foreach (int id in job.Depend)
                CreateConnection(Jobs.FromId(id), job);
        }


        /// <summary>
        /// Deletes the edge from <paramref name="jobCo1"/> to <paramref name="jobCo2"/>
        /// </summary>
        /// <param name="jobCo1"></param> 
        /// <param name="jobCo2"></param>
        protected void DeleteConnection(JobCo jobCo1, JobCo jobCo2)
        {
            //M[JobCo1.Id, JobCo2.Id] = 0;
            //if (M[JobCo2.Id, JobCo1.Id] == 1)
            //    M[JobCo1.Id, JobCo2.Id] = -1;
            //else
            //    M[JobCo2.Id, JobCo1.Id] = 0;
            bool b = L[jobCo1.Id].Remove(jobCo2.Id);
        }


        /// <summary>
        /// Deletes connections with all jobs that <paramref name="job"/> depends of
        /// </summary>
        /// <param name="job"></param>The job we want to delete all the connection from the jobs he depends of
        protected void DeleteConnections(JobCo job)
        {
            L[job.Id].Clear();
            foreach(int id in job.Depend)
                DeleteConnection(Jobs.FromId(id), job);
        }


        /// <summary>
        /// Unexecute <paramref name="jobCo"/>, witch means puts back in the oriented graph <paramref name="jobCo"/>
        /// </summary> The job we want to unexecute
        /// <param name="jobCo"></param>
        public void UnExecuteJob(JobCo jobCo)
        {
            CreateConnections(jobCo);
            ActualiseConnectedJobs(jobCo);
        }

        /// <summary>
        /// Execute <paramref name="jobCo"/>, witch means deletes from the oriented graph <paramref name="jobCo"/>
        /// <param name="jobCo"></param> The job we want to execute
        public void ExecuteJob(JobCo jobCo)
        {
            leftJobs.Remove(jobCo);
            DeleteConnections(jobCo);
            ActualiseConnectedJobs(jobCo);
        }

        public abstract void ActualiseConnectedJobs(JobCo JobCo);

        /* Cherche un JobCo de dégré sortant *deg* qui est pas Lock, dans les temps et qui a le plus petit ExecTime; sinon renvoie null */
        /// <summary>
        /// Looks for a JobCo that has an out degree witch is worth <paramref name="deg"/>, whitch is not Lock and has the smallest ExecTime
        /// </summary>
        /// <param name="deg"></param>Degree of the JobCo we are looking for
        /// <param name="time"></param>Time reference to know if the JobCo is on time
        /// <returns>Returns thz JobCo coressponding to all these conditions or null if there is not</returns>
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
        /// <summary>
        /// Finds the JobCo with the higher out degree witch is not Late 
        /// </summary>
        /// <param name="time"></param>
        /// <returns>Retrurns the JobCo on time with higher out degree or null if there is not</returns>
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

        /// <summary>
        /// This mehod is used to make a list of all the jobs that are late in the oriented graph
        /// </summary>
        /// <returns>Returns the list of all the jobs that are late</returns>
        public List<JobCo> GetAllLeftJobs() => leftJobs.ToList();// Jobs.Where(j => L[j.Id].Count > 0 || j.IsLocked).ToList();
    }

/* GraphLock */

    public class GraphLock : Graph
    {
        ///<summary>
        /// Class extending Graph and contains in the graphs JobCo that can be locked
        /// </summary>

        /// <summary>
        /// initialize a new oriented graph with the given jobs, extends the constructot of Graph
        /// </summary>
        /// <param name="JobCos"></param>
        public GraphLock(JobCo[] JobCos)
            : base(JobCos)
        { }
        
        /// <summary>
        /// Locks the jobs that JobCoCo depends of, if needed
        /// </summary>
        /// <param name="JobCoCo"></param>
        public override void ActualiseConnectedJobs(JobCo JobCoCo)
        {
            foreach (JobCo JobCo in Jobs.Where(j => j.Depend.Contains(JobCoCo.Id)))
                JobCo.ActualiseIsLocked(L);
        }

    }


/* GraphTimeD */

    public class GraphTimeD : Graph
    {
        ///<summary>
        /// Class extending Graph and can make the jobs' ExecTime decrease 
        /// </summary>

        ///<summary>
        ///initialize a new oriented graph with the given jobs, extends the constructot of Graph
        ///</summary>
        /// <param name="JobCos"></param> jobs used to do the oriented graph
        public GraphTimeD(JobCo[] JobCos)
            : base(JobCos)
        { }

        /// <summary>
        /// Decreases the ExecTime of the jobs that JobCoCo depends of, if needed
        /// </summary>
        /// <param name="JobCoCo"></param>
        public override void ActualiseConnectedJobs(JobCo JobCoCo)
        {
            foreach (JobCo JobCo in Jobs.Where(j => j.Depend.Contains(JobCoCo.Id)))
                JobCo.ActualiseExecTime(L);
        }
    }
}
