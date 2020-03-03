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
        this->je = this->je->get_next();
    } else {
        JobElem* tmp = this->je;
        while (tmp->get_next() != nullptr && tmp->get_next() != rm)
            tmp = tmp->get_next();
        
        if (tmp->get_next() != nullptr)
            tmp->set_next(tmp->get_next()->get_next());
    }
    rm->set_next(nullptr);

    return rm;
}

JobElem* JobList::pop_biggest() {
    JobElem* tmp = this->je;
    JobElem* biggest = this->je;
    while (tmp != nullptr) {
        if (tmp->get_time() > biggest->get_time())
            biggest = tmp;
        tmp = tmp->get_next();
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
    while (tmp->get_next() != nullptr)
        tmp = tmp->get_next();
    tmp->set_next(newje);
}

void JobList::print() {
    JobElem *tmp = this->je;
    while (tmp != nullptr) {
        (*tmp).print();
        tmp = tmp->get_next();
    }
}

void JobList::free() {
    while (this->je != nullptr) {
        JobElem *tmp = this->je->get_next();
        delete this->je;
        this->je = tmp;
    }
}