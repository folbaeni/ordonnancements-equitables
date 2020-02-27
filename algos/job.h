#ifndef JOB_H
#define JOB_H

#include <iostream>


using namespace std;

class job {
    private:
        int static cpt;
        int id, time, deadline;

    public:
        int getId();
        int getTime();
        int getDeadline();
        job();
        job(int time, int deadline);
        operator std::string() const;
};

#endif 