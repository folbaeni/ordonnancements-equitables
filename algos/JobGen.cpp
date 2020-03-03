#include <iostream>
#include <string>
#include "Job.h"
#include "JobGen.h"

using namespace std;

JobGen::JobGen() : Job() {
    this->label = to_string(inherited::get_id());
    this->owner = "def";
}

JobGen::JobGen(int time, int deadline) : Job(time,deadline) {
    this->label = to_string(inherited::get_id());
    this->owner = "def";
}

int Job::get_id(){
    return id;
}

int Job::get_time(){
    return time;
}

int Job::get_deadline(){
    return deadline;
}

Job::operator std::string() const{
    return "Job(id: " + to_string(id) + ", time: " + to_string(time) + ", dl: " + to_string(deadline) + ")\n";
}