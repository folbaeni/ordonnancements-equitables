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
        int NumberOfUsers { get; }
        public User<TJob>[] Users { get; }

        void Execute(User<TJob>[] users);
    }
}
