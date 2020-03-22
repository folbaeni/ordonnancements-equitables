#include <iostream>
#include "Job.h"


int Job::cpt = 0;

Job::Job() : time(-1), id(-1) {
}

Job::Job(int time) : id(cpt++), time(time), deadline(0) {
}

Job::Job(int time, int deadline) : id(cpt++), time(time), deadline(deadline) {
}

int Job::get_id(){
    return id;
}

int Job::get_time(){
    return time;
}
int Job::get_deadline() {
    return deadline;
}

void Job::set_time(int time) {
    this->time = time;
}

std::string Job::to_string(){
    std::string res = "Job(id: " + std::to_string(id);
    if (time != 1)
        res += ", time: " + std::to_string(time);
    if (deadline != 0)
        res += ", dl:" + std::to_string(deadline);
    return res + ")";
}