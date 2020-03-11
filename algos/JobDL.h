#ifndef JOBDL_H
#define JOBDL_H

#include <iostream>
#include <string>

#include "Job.h"

class JobDL : public Job {
    protected:
        int deadline;
        
    public:
        JobDL();
        JobDL(int time, int deadline);
        int get_deadline();
        void set_deadline(int deadline);
        std::string to_string();
};

#endif