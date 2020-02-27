#include <iostream>
#include <queue>

#include "Job.h"

using namespace std;

void Hogdson(Job jobs[], int n) {
    Job onTime[n];
    Job late[n];
    int C = 0, i = 0, j = 0;

    for (int k = 0; k < n; k++) {
        C += jobs[k].getTime();
        onTime[i++] = jobs[k];

        if (C > jobs[i].getDeadline()) {
            int l = 1, imax = 0, timemax = onTime[imax].getTime();
            while (l < i) {
                if (onTime[i].getTime() > timemax) {
                    imax = l;
                    timemax = onTime[imax].getTime();
                } 
                l++;
            }
            late[j++] = onTime[imax];
            onTime[imax] = onTime[i--];
        }
    }


    cout << "On time Jobs:\n";
    for (int k = 0; k < i; k++) {
        onTime[k].print();
    }
    cout << "Late Jobs:\n";
    for (int k = 0; k < i; k++) {
        late[k].print();
    }
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
        jobs[i].print();
    }

    Hogdson(jobs, n);
    

    return 0;
}