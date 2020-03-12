#ifndef JOBP_H
#define JOBP_H

#include <iostream>
#include <string>

#include "Job.h"

class JobP : public Job {
    protected:
        int profit;
        
    public:
        JobP();
        JobP(int profit);
        JobP(int time, int profit);
        int get_profit();
        void set_profit(int profit);
        std::string to_string();
};

#endif