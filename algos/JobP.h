#ifndef JOBP_H
#define JOBP_H

#include <iostream>
#include <string>

#include "Job.h"

class JobP : public Job {
    protected:
        int profit;
        int deadline;
        
    public:
        JobP();
        JobP(int profit, int deadline);
        JobP(int time, int profit, int deadline);
        int get_profit();
        void set_profit(int profit);
        int get_deadline();
        int set_deadline(int deadline);
        std::string to_string();
};

#endif