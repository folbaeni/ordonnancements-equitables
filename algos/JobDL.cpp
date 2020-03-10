#include <iostream>
#include <string>
#include "Job.h"
#include "JobDL.h"

using namespace std;

JobDL::JobDL() : Job() {
}

JobDL::JobDL(int time, int deadline) : Job(time), deadline(deadline) {
}

int JobDL::get_deadline(){
    return deadline;
}

std::string JobDL::to_string() {
    return "JobDL(id: " + std::to_string(id) + ", time: " + std::to_string(time) + ", dl: " + std::to_string(this->deadline) + ")\n";
}