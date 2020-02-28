#ifndef JOB_H
#define JOB_H

#include <iostream>


using namespace std;

class Job {
    private:
        int static cpt;
        int id;
        void set_time(int time);
        void set_deadline(int deadline);
        
    protected:
        int time, deadline;

    public:
        int get_id();
        int get_time();
        int get_deadline();
        Job();
        Job(int time, int deadline);
        operator std::string() const;
};

#endif 