#include <iostream>
#include "job.h"
#include "jobelem.h"
#include "joblist.h"


joblist::joblist() {
    this->je = nullptr;
}

joblist::joblist(jobelem *je) {
    this->je = je;
}

jobelem* joblist::remove(jobelem *rm) {
    if (rm == this->je) {
        this->je = this->je->getNext();
    } else {
        jobelem* tmp = this->je;
        while (tmp->getNext() != nullptr && tmp->getNext() != rm)
            tmp = tmp->getNext();
        
        if (tmp->getNext() != nullptr)
            tmp->setNext(tmp->getNext()->getNext());
    }
    rm->setNext(nullptr);
    return rm;
}

jobelem* joblist::pop_biggest() {
    jobelem* tmp = this->je;
    jobelem* biggest = this->je;
    while (tmp != nullptr) {
        if (tmp->getTime() > biggest->getTime())
            biggest = tmp;
        tmp = tmp->getNext();
    }
    return remove(biggest);
}

void joblist::add_job(job j) {
    this->add_job(new jobelem(j));
}

void joblist::add_job(jobelem *newje) {
    if (this->je == nullptr) {
        this->je = newje;
        return;
    }
    jobelem *tmp = this->je;
    while (tmp->getNext() != nullptr)
        tmp = tmp->getNext();
    tmp->setNext(newje);
}

void joblist::print() {
    jobelem *tmp = this->je;
    while (tmp != nullptr) {
        (*tmp).print();
        tmp = tmp->getNext();
    }
    cout << endl;
}