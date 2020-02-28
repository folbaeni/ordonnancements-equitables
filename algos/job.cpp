#include <iostream>
#include "job.h"

using namespace std;

int job::cpt = 0;

job::job(){
    this->time = -1; 
    this->deadline = -1;
    this->id = -1;
}

job::job(int time, int deadline){
    this->id = cpt++;
    this->time = time; 
    this->deadline = deadline;
}

int job::getId(){
    return id;
}

int job::getTime(){
    return time;
}

int job::getDeadline(){
    return deadline;
}

job::operator std::string() const{
    return "Job(id: " + to_string(id) + ", time: " + to_string(time) + ", dl: " + to_string(deadline) + ")\n";
}