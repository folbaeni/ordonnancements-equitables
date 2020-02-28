#include <iostream>
#include "Job.h"
#include "JobElem.h"
#include "JobList.h"


JobList::JobList() {
    this->je = nullptr;
}

JobList::JobList(JobElem *je) {
    this->je = je;
}

JobElem* JobList::remove(JobElem *rm) {
    if (rm == this->je) {
        this->je = this->je->getNext();
    } else {
        JobElem* tmp = this->je;
        while (tmp->getNext() != nullptr && tmp->getNext() != rm)
            tmp = tmp->getNext();
        
        if (tmp->getNext() != nullptr)
            tmp->setNext(tmp->getNext()->getNext());
    }
    rm->setNext(nullptr);
    return rm;
}

JobElem* JobList::pop_biggest() {
    JobElem* tmp = this->je;
    JobElem* biggest = this->je;
    while (tmp != nullptr) {
        if (tmp->getTime() > biggest->getTime())
            biggest = tmp;
        tmp = tmp->getNext();
    }
    return remove(biggest);
}

void JobList::add_job(Job j) {
    this->add_job(new JobElem(j));
}

void JobList::add_job(JobElem *newje) {
    if (this->je == nullptr) {
        this->je = newje;
        return;
    }
    JobElem *tmp = this->je;
    while (tmp->getNext() != nullptr)
        tmp = tmp->getNext();
    tmp->setNext(newje);
}

void JobList::print() {
    JobElem *tmp = this->je;
    while (tmp != nullptr) {
        (*tmp).print();
        tmp = tmp->getNext();
    }
    //cout << endl;
}

void JobList::free() {
    while (this->je != nullptr) {
        JobElem *tmp = this->je->getNext();
        delete this->je;
        this->je = tmp;
    }
}