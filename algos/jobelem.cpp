#include <iostream>
#include "job.h"
#include "jobelem.h"


jobelem::jobelem() {
    this->j = job();
    this->next = nullptr;
}

jobelem::jobelem(job j) {
    this->j = j;
    this->next = nullptr;
}

job jobelem::getJob() {
    return this->j;
}

int jobelem::getTime() {
    return this->j.getTime();
}

void jobelem::setJob(job j) {
    this->j = j;
}

jobelem* jobelem::getNext() {
    return this->next;
}

void jobelem::setNext(jobelem* jl) {
    this->next = jl;
}

void jobelem::print() {
    cout << string(j);
}