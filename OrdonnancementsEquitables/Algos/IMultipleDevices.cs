using OrdonnancementsEquitables.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdonnancementsEquitables.Algos
{
    public interface IMultipleDevices<TJob> where TJob : Job
    {
        TJob[] Execute(TJob[] jobs, int nbDevices);
    }
}
