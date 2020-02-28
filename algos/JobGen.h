#ifndef JOBGEN_H
#define JOBGEN_H

#include <iostream>
#include <string>

#include "Job.h"

class JobGen : public Job {
    //Job génerique
    public:
        const string label, owner;

    private:
        typedef Job inherited;
};

#endif