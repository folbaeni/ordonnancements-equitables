#include <iostream>
#include <string>
#include "Job.h"
#include "JobP.h"

JobP::JobP() : Job() {
}

JobP::JobP(int profit) : Job(1), profit(profit) {
}


JobP::JobP(int time, int profit) : Job(time), profit(profit) {
}

int JobP::get_profit(){
    return profit;
}

std::string JobP::to_string() {
    std::string res = "JobP(id: " + std::to_string(id);
    if (time != 1)
        res += ", time: " + std::to_string(time);
    res += ", profit: " + std::to_string(this->profit) + ")";
    
    return res;
}