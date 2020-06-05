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
        public User<TJob>[] Users { get; }

        TJob[] Execute(User<TJob>[] users);
    }
}
