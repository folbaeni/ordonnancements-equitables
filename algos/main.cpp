#include <iostream>
#include <new>

#include "job.h"
#include "jobelem.h"
#include "joblist.h"

using namespace std;

void Hogdson(job jobs[], int n) {
    joblist ontime = joblist();
    joblist late = joblist();
    int C = 0;

    for (int k = 0; k < n; k++) {
        cout << "--------- " + to_string(k + 1) + " ---------\n";
        ontime.add_job(jobs[k]);
        C += jobs[k].getTime();

        if (C > jobs[k].getDeadline()) {
            jobelem *je = ontime.pop_biggest();
            C -= je->getTime();
            late.add_job(je);
        }

        ontime.print();
        late.print();
    }
}

int main() {

    int n = 6;
    job jobs[] = {
        job(6, 8),
        job(4, 9),
        job(7, 15),
        job(8, 20),
        job(3, 21),
        job(5, 22)
    };

    for (int i = 0; i < n; i++) {
        cout << string(jobs[i]);
    }

    cout << endl << endl;
    Hogdson(jobs, n);
    

    return 0;
}