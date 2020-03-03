#include <iostream>
#include "Job.h"
#include "JobElem.h"

JobElem::JobElem() {
    this->j = Job();
    this->next = nullptr;
}

JobElem::JobElem(Job j) {
    this->j = j;
    this->next = nullptr;
}


Job JobElem::get_job() {
    return this->j;
}

int JobElem::get_time() {
    return this->j.get_time();
}

void JobElem::set_job(Job j) {
    this->j = j;
}

JobElem* JobElem::get_next() {
    return this->next;
}

void JobElem::set_next(JobElem* jl) {

    this->next = jl;
}

void JobElem::print() {
    cout << string(j);
}