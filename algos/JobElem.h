#ifndef JOBELEM_H
#define JOBELEM_H

#include <iostream>
#include "Job.h"


using namespace std;

class JobElem {
    private:
        Job *j;
        JobElem *next;

    public:
        JobElem();
        JobElem(Job j);
        Job *get_job();
        int get_time();
        void set_job(Job j);
        JobElem* get_next();
        void set_next(JobElem *next);

        void print();
};

#endif 