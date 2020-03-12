#include <iostream>
#include <string>
#include "Job.h"
#include "JobDL.h"

JobDL::JobDL() : Job() {
}

JobDL::JobDL(int time, int deadline) : Job(time), deadline(deadline) {
}

int JobDL::get_deadline(){
    return deadline;
}

std::string JobDL::to_string() {
    std::string res = "JobDL(id: " + std::to_string(id);
    if (time != 1)
        res += ", time: " + std::to_string(time);
    res += ", dl: " + std::to_string(this->deadline) + ")";

    return res;
}