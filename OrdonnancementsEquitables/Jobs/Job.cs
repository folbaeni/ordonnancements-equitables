using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdonnancementsEquitables.Jobs
{
    /// <summary>
    /// Class <c>JobCo</c> models a task for a devices to execute.
    /// </summary>
    public class Job
    {
        /// <summary> 
        /// Static counter to keep track of the new <c>JobCo</c>'s <see cref="Id"/>.
        /// </summary>
        static int cpt = 0;
        
        /// <value>
        /// Property <c>Time</c> represents the time required for the <c>JobCo</c> to end. 
        /// </value>
        public int Time { get; }

        /// <value>
        /// Property <c>Id</c> is a unique identifier for each <c>JobCo</c>. 
        /// </value>
        [JsonIgnore] public int Id { get; }

        /// <value>
        /// Property <c>Deadline</c> represents the time limit after what the <c>JobCo</c> is considered late. 
        /// </value>
        public int Deadline { get; }

        private Job() { }
        
        /// <summary>
        /// Constructor to initialize a new <c>JobCo</c>.
        /// </summary>
        /// <param name="time">Sets the value of <see cref="Time"/>.</param>
        /// <param name="deadline">Sets the value of <see cref="Deadline"/></param>
        public Job(int time, int deadline)
        {
            Id = cpt++;
            Time = time;
            Deadline = deadline;
        }

        /// <summary> 
        /// Static method to restart <see cref="cpt"/> to zero.
        /// </summary>
        public static void CountToZero() => cpt = 0;

        /// <summary> 
        /// <para> Virtual method in order to specify the <c>JobCo</c>'s type.</para>
        /// </summary>
        /// <returns>The <see cref="string"/> value "JobCo"</returns>
        /// <seealso cref="Prefixe"/>
        /// <seealso cref="ToString"/>
        protected virtual string JobType() => "JobCo";

        /// <summary> 
        /// Virtual method in order to specify the <c>JobCo</c>'s values
        /// </summary>
        /// <seealso cref="JobType"/> 
        /// <seealso cref="ToString"/>
        protected virtual string Prefixe() => $"{JobType()}(Id: {Id}, Time: {Time}, Deadline: {Deadline}";

        /// <summary>
        /// Formatted <see cref="string"/> representation of the <c>JobCo</c>.
        /// </summary>
        /// <returns>Returns a formatted <see cref="string"/> representation of the <c>JobCo</c>, in the form <c>JobCoType(Id, Time, Deadline, [Profit/Depend])</c> </returns>
        /// <seealso cref="Prefixe"/> 
        /// <seealso cref="JobType"/>
        public override string ToString() => Prefixe() + ")";

        /// <summary>
        /// <para> This method determines whether two JobCos are the same. </para>
        /// It is only based on each <c>JobCo</c>'s <c>Id</c>.
        /// </summary>
        /// <param name="obj"> is the object to be compared to the current object. </param>
        /// <returns><see langword="true"/> if the <c>JobCo</c>s have the same <c>Id</c>; otherwise, <see langword="false"/>.</returns>
        /// <seealso cref="operator=="/>
        /// <seealso cref="operator!="/>
        public override bool Equals(object obj) => obj is Job job && Id == job.Id;
        public override int GetHashCode() => 2108858624 + Id.GetHashCode();

        /// <summary>
        /// This operator determines whether two <c>JobCo</c>s have the same <c>Id</c>.
        /// </summary>
        /// <param name="left"> is the first <c>JobCo</c>to be compared. </param>
        /// <param name="right"> is the secend <c>JobCo</c>to be compared. </param>
        /// <returns><see langword="true"/> if the <c>JobCo</c>s have the same <c>Id</c>; otherwise, <see langword="false"/>.</returns>
        /// <seealso cref="Equals"/>
        /// <seealso cref="operator!="/>
        public static bool operator ==(Job left, Job right) => EqualityComparer<Job>.Default.Equals(left, right);

        /// <summary>
        /// This operator determines whether two <c>JobCo</c>s have the same <c>Id</c>.
        /// </summary>
        /// <param name="left"> is the first <c>JobCo</c>to be compared. </param>
        /// <param name="right"> is the secend <c>JobCo</c>to be compared. </param>
        /// <returns><see langword="true"/> if the <c>JobCo</c>s do not have the same <c>Id</c>; otherwise, <see langword="false"/>.</returns>
        /// <seealso cref="Equals"/>
        /// <seealso cref="operator=="/>
        public static bool operator !=(Job left, Job right) => !(left == right);
    }
}
