#include <iostream>
#include "Job.h"

using namespace std;

Job::Job(){
    this->time = 0; 
    this->deadline = 0;
}


Job::Job(int time, int deadline){
    this->time = time; 
    this->deadline = deadline;
}

int Job::getTime(){
    return time;
}

int Job::getDeadline(){
    return deadline;
}

void Job::print(){
    cout << "Job :\ttemps: " << time << "\tdeadline: " << deadline << endl;
}