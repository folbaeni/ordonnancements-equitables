#ifndef JOBLIST_H
#define JOBLIST_H

#include <iostream>
#include "Job.h"
#include "JobElem.h"


using namespace std;

class JobList {
    private:
        JobElem *je;

    public:
        JobList();
        JobList(JobElem *je);
        ~JobList();
        JobElem* remove(JobElem* je);
        JobElem* pop_biggest();
        JobElem* get_elem();
        void add_job(JobElem *je);
        void add_job(Job *j);
        void print();
};

#endif 