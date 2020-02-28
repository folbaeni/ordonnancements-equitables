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

Job JobElem::getJob() {
    return this->j;
}

int JobElem::getTime() {
    return this->j.getTime();
}

void JobElem::setJob(Job j) {
    this->j = j;
}

JobElem* JobElem::getNext() {
    return this->next;
}

void JobElem::setNext(JobElem* jl) {
    this->next = jl;
}

void JobElem::print() {
    cout << string(j);
}