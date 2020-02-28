#ifndef JOBLIST_H
#define JOBLIST_H

#include <iostream>
#include "job.h"
#include "jobelem.h"


using namespace std;

class joblist {
    private:
        jobelem *je;

    public:
        joblist();
        joblist(jobelem *je);
        jobelem* remove(jobelem* je);
        jobelem* pop_biggest();
        void add_job(jobelem *je);
        void add_job(job j);
        void print();
};

#endif 