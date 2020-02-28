#ifndef JOB_H
#define JOB_H

#include <iostream>


using namespace std;

class Job {
    private:
        int static cpt;
        int id, time, deadline;

    public:
        int getId();
        int getTime();
        int getDeadline();
        Job();
        Job(int time, int deadline);
        operator std::string() const;
};

#endif 