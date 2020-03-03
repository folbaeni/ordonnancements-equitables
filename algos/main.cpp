#include <iostream>
#include <new>

#include "Job.h"
#include "JobElem.h"
#include "JobList.h"

using namespace std;

void Hogdson(Job jobs[], int n) {

    JobList ontime = JobList();
    JobList late = JobList();
    int C = 0;

    for (int k = 0; k < n; k++) {
        cout << "--------- " + to_string(k + 1) + " ---------\n";
        ontime.add_job(jobs[k]);
        C += jobs[k].get_time();

        if (C > jobs[k].get_deadline()) {
            JobElem *je = ontime.pop_biggest();
            C -= je->get_time();
            late.add_job(je);
        }

    }
    cout << "\nOn Time:\n";
    ontime.print();
    cout << "\nLate:\n";
    late.print();

    ontime.free();
    late.free();
}

int main() {

    int n = 6;
    Job jobs[] = {
        Job(6, 8),
        Job(4, 9),
        Job(7, 15),
        Job(8, 20),
        Job(3, 21),
        Job(5, 22)
    };

    for (int i = 0; i < n; i++) {
        cout << string(Jobs[i]);
    }

    cout << endl << endl;
    Hogdson(jobs, n);

    return 0;
}