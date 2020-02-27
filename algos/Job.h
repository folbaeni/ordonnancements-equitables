#ifndef JOB_H
#define JOB_H

#include <iostream>


using namespace std;

class Job {
    private:
        int time, deadline;

    public:
        int getTime();
        int getDeadline();
        Job();
        Job(int time, int deadline);
        void print(); 
};

#endif 