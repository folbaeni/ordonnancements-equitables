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
        JobP(int profit, int deadline);
        JobP(int time, int profit, int deadline);
        int get_profit();
        virtual std::string to_string();
};

#endif