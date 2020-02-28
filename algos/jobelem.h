#ifndef JOBELEM_H
#define JOBELEM_H

#include <iostream>
#include "job.h"


using namespace std;

class jobelem {
    private:
        job j;
        jobelem *next;

    public:
        jobelem();
        jobelem(job j);
        job getJob();
        int getTime();
        void setJob(job j);
        jobelem* getNext();
        void setNext(jobelem *next);
        void print();
};

#endif 