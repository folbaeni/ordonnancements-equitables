using OrdonnancementsEquitables.Jobs;
using OrdonnancementsEquitables.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrdonnancementsEquitables.Models
{

    /// <summary>
    /// Abstract class modeling oriented graphs.
    /// </summary>
    public abstract class Graph
    {
        protected JobCo[] Jobs;
        protected List<JobCo> leftJobs;
        
        
        /// <summary>
        /// List of adjencies of the oriented graph.
        /// </summary>
        protected List<int>[] L;


        /// <summary>
        /// Initialize a new oriented graph with the given jobs.
        /// </summary>
        /// <param name="jobs">Array of jobs used to create the oriented graph.</param>
        public Graph(JobCo[] jobs)
        {
            Jobs = jobs;
            leftJobs = jobs.ToList();
            L = new List<int>[Jobs.Length];
            for (int i = 0; i < L.Length; i++)
                L[i] = new List<int>();

            foreach(JobCo j in Jobs)
                CreateConnections(j);
        }

        /// <summary>
        /// Creates the arc from <paramref name="jobCo1"/> to <paramref name="jobCo2"/>.
        /// </summary>
        /// <param name="jobCo1">Where the arc is coming from.</param> 
        /// <param name="jobCo2">Where the arc is ending.</param>
        protected void CreateConnection(JobCo jobCo1, JobCo jobCo2) 
        {
            //M[JobCo1.Id, JobCo2.Id] = 1;
            //if (M[JobCo2.Id, JobCo1.Id] != 1)
            //    M[JobCo2.Id, JobCo1.Id] = -1;

            L[jobCo1.Id].Add(jobCo2.Id);
        }


        /// <summary>
        /// Creates connections with the jobs <paramref name="job"/> is depending on.
        /// </summary>
        /// <param name="job">Job where all arcs will ending.</param>.
        protected void CreateConnections(JobCo job)
        {
            foreach (int id in job.Depend)
                CreateConnection(Jobs.FromId(id), job);
        }


        /// <summary>
        /// Deletes the arc from <paramref name="jobCo1"/> to <paramref name="jobCo2"/>
        /// </summary>
        /// <param name="jobCo1">Where the arc is coming from.</param> 
        /// <param name="jobCo2">Where the arc is ending.</param>
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
        /// Deletes connections with the jobs <paramref name="job"/> is depending on.
        /// </summary>
        /// <param name="job">Job where all arcs are ending.</param>
        protected void DeleteConnections(JobCo job)
        {
            L[job.Id].Clear();
            foreach(int id in job.Depend)
                DeleteConnection(Jobs.FromId(id), job);
        }


        /// <summary>
        /// Unexecutes <paramref name="jobCo"/>. Puts back all arcs containing <paramref name="jobCo"/> in the oriented graph.
        /// </summary>
        /// <param name="jobCo">The job to unexecute.</param>
        public void UnExecuteJob(JobCo jobCo)
        {
            leftJobs.Add(jobCo);
            CreateConnections(jobCo);
            ActualiseConnectedJobs(jobCo);
        }

        /// <summary>
        /// Executes <paramref name="jobCo"/>. Deletes all arcs containing <paramref name="jobCo"/> in the oriented graph.
        /// <param name="jobCo">The job to execute.</param>
        public void ExecuteJob(JobCo jobCo)
        {
            leftJobs.Remove(jobCo);
            DeleteConnections(jobCo);
            ActualiseConnectedJobs(jobCo);
        }

        public abstract void ActualiseConnectedJobs(JobCo JobCo);

        /// <summary>
        /// Looks for a JobCo that has an out degree equals to <paramref name="deg"/>, whitch is not Lock and has the smallest ExecTime
        /// </summary>
        /// <param name="deg">Wanted degree for the JobCo.</param>
        /// <param name="time">Time reference to know if the JobCo is on time or not.</param>
        /// <returns>Returns a JobCo if one is found; <see langword="null"/> otherwise.</returns>
        protected JobCo GetHigherOutDegreeOnTime(int deg, int time)
        {
            List<JobCo> jobCoDeg = new List<JobCo>();
            for (int id = 0; id < L.Length; id++)
            {
                if (L[id].Count == deg)
                    jobCoDeg.Add(Jobs.FromId(id));
            }

            jobCoDeg = jobCoDeg.Where(j => j.IsLocked == false).OrderBy(j => j.ExecTime).ToList();

            foreach (JobCo onTime in jobCoDeg)
            {
                if (time + onTime.ExecTime < onTime.Deadline)
                    return onTime;
            }

            return null;
        }

        /// <summary>
        /// Finds the JobCo with the higher out degree.
        /// </summary>
        /// <param name="time">Time reference to know if the JobCo is late or not.</param>
        /// <returns>Returns a JobCo if one is found; <see langword="null"/> otherwise.</returns>
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
        /// This mehod is used to make a list of all the left jobs that are late or locked in the oriented graph.
        /// </summary>
        /// <returns>Returns the list of all the jobs that are late.</returns>
        public List<JobCo> GetAllLeftJobs() => leftJobs.ToList();
    }

    /// <summary>
    /// Class extending Graph and contains in the graphs JobCo that can be locked.
    /// </summary>
    public class GraphLock : Graph
    {
        /// <summary>
        /// initialize a new oriented graph with the given jobs, extends the constructot of Graph.
        /// </summary>
        /// <param name="JobCos"></param>
        public GraphLock(JobCo[] JobCos)
            : base(JobCos)
        { }
        
        /// <summary>
        /// Locks the jobs that JobCoCo depends of, if needed.
        /// </summary>
        /// <param name="JobCoCo"></param>
        public override void ActualiseConnectedJobs(JobCo JobCoCo)
        {
            foreach (JobCo JobCo in Jobs.Where(j => j.Depend.Contains(JobCoCo.Id)))
                JobCo.ActualiseIsLocked(L);
        }
    }

    /// <summary>
    /// Class extending Graph and can make the jobs' ExecTime decrease.
    /// </summary>
    public class GraphTimeD : Graph
    {
        /// <summary>
        /// initialize a new oriented graph with the given jobs, extends the constructot of Graph.
        /// </summary>
        /// <param name="JobCos">Jobs used to do the oriented graph.</param>
        public GraphTimeD(JobCo[] JobCos)
            : base(JobCos)
        { }

        /// <summary>
        /// Decreases the ExecTime of the jobs that JobCoCo depends of, if needed.
        /// </summary>
        /// <param name="JobCoCo"></param>
        public override void ActualiseConnectedJobs(JobCo JobCoCo)
        {
            foreach (JobCo JobCo in Jobs.Where(j => j.Depend.Contains(JobCoCo.Id)))
                JobCo.ActualiseExecTime(L);
        }
    }
}
