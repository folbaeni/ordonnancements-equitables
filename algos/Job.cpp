#include <iostream>
#include "Job.h"

using namespace std;

int Job::cpt = 0;

Job::Job() : time(-1), id(-1) {
}

Job::Job(int time) : id(cpt++), time(time) {
}

int Job::get_id(){
    return id;
}

int Job::get_time(){
    return time;
}

void Job::set_time(int time) {
    this->time = time;
}

std::string Job::to_string(){
    return "Job(id: " + std::to_string(id) + ", time: " + std::to_string(time) + ")\n";
}