#ifndef JOBELEM_H
#define JOBELEM_H

#include <iostream>
#include "Job.h"


using namespace std;

class JobElem {
    private:
        Job j;
        JobElem *next;

    public:
        JobElem();
        JobElem(Job j);
        Job getJob();
        int getTime();
        void setJob(Job j);
        JobElem* getNext();
        void setNext(JobElem *next);
        void print();
};

#endif 