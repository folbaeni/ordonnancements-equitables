#ifndef JOB_H
#define JOB_H

#include <iostream>


class Job {
    private:
        int static cpt;
        void set_time(int time);
        
    protected:
        int time;
        int id;

    public:
        int get_id();
        int get_time();
        int get_deadline();

        Job();
        Job(int time);
        virtual std::string to_string();
};

#endif 