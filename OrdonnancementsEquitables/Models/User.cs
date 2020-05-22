﻿using OrdonnancementsEquitables.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdonnancementsEquitables.Models
{
    public class User<TJob> where TJob : Job
    {

        public TJob[] Jobs { get; set; }

        public User(TJob[] jobs)
        {
            Jobs = jobs;
        }
    }
}