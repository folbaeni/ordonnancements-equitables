#include <iostream>
#include <new>

#include "Job.h"
#include "JobDL.h"
#include "JobElem.h"
#include "JobList.h"

using namespace std;

void Hogdson(JobDL jobs[], int n) {

    JobList ontime = JobList();
    JobList late = JobList();
    int C = 0;

    for (int k = 0; k < n; k++) {
        cout << "--------- " + to_string(k + 1) + " ---------\n";
        ontime.add_job(jobs + k);
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
}

void hogdson_helper() {
    int n = 6;
    JobDL jobs[] = {
        JobDL(6, 8),
        JobDL(4, 9),
        JobDL(7, 15),
        JobDL(8, 20),
        JobDL(3, 21),
        JobDL(5, 22)
    };

    for (int i = 0; i < n; i++) {
        cout << jobs[i].to_string() << endl;
    }

    cout << endl << endl;
    Hogdson(jobs, n);
}

int main() {
    //POUR ALI





    return 0;
}