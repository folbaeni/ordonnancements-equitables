#ifndef JOBGEN_H
#define JOBGEN_H

#include <iostream>
#include <string>

#include "Job.h"

class JobGen : public Job {
    //Job génerique
    public:
        string label, owner;
        JobGen();
        JobGen(int time, int deadline);

    private:
        typedef Job inherited;
};

#endif