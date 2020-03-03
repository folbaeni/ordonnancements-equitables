#include <iostream>
#include "Job.h"

using namespace std;

int Job::cpt = 0;

Job::Job(){
    this->time = -1; 
    this->deadline = -1;
    this->id = -1;
}

Job::Job(int time, int deadline){
    this->id = cpt++;
    this->time = time; 
    this->deadline = deadline;
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


void Job::set_time(int time) {
    this->time = time;
}
void Job::set_deadline(int deadline) {
    this->deadline = deadline;

}