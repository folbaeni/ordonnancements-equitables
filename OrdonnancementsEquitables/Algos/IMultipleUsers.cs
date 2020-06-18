using OrdonnancementsEquitables.Jobs;
using OrdonnancementsEquitables.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdonnancementsEquitables.Algos
{
    public interface IMultipleUsers<TJob> where TJob : Job
    {

        /// <summary>
        /// Parameter of type in showing the number of users, this parmaeter is defined as readonly
        /// </summary>
        int NumberOfUsers { get; }

        /// <summary>
        /// Parameter of type User<TJob>[] representing the users for an algorithme, this parameter is defined as readonly
        /// </summary>
        public User<TJob>[] Users { get; }


        /// <summary>
        /// Execution function of an algorithme with <paramref name="users"/>
        /// </summary>
        /// <param name="users"></param>
        void Execute(User<TJob>[] users);
    }
}
