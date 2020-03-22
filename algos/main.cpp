#include <iostream>
#include <new>

#include "Job.h"
#include "JobP.h"
#include "JobElem.h"
#include "JobList.h"

using namespace std;

void Hogdson(JobP jobs[], int n) {

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
    JobP jobs[] = {
        JobP(6, 0, 8),
        JobP(4, 0, 9),
        JobP(7, 0, 15),
        JobP(8, 0, 20),
        JobP(3, 0, 21),
        JobP(5, 0, 22)
    };

    for (int i = 0; i < n; i++) {
        cout << jobs[i].to_string() << endl;
    }

    cout << endl << endl;
    Hogdson(jobs, n);
}


int gloutonParProfits(JobP* job, int taille){ 
    int time = 0;
    int profit = 0;
    JobP tmp;

    for(int i(0); i < taille; i++){ /* boucle pour faire avancer le tps */
        time++;
        for(int j(i); j < taille; j++){ /* parcours de tableau */
            if( job[j].get_deadline() < time && job[j].get_deadline() < job[i].get_deadline()){
                tmp = job[i];
                job[i] = job[j];
                job[j] = tmp;
            }
        }/* on a ordonnancé selon le principe glouton par profit */

        if(job[i].get_deadline() < time){
            profit += job[i].get_profit();
        }
    }
    return profit;
}

int main() {
    JobP jobs[] = {
        JobP(1, 12, 4),
        JobP(1, 10, 3),
        JobP(1, 8, 1),
        JobP(1, 7, 6),
        JobP(1, 6, 1),
        JobP(1, 5, 6),
        JobP(1, 4, 6),
        JobP(1, 3, 5),
    };

    for (int i = 0; i < 8; i++) {
        cout << jobs[i].to_string() << endl;
    }
    
    int resultat = gloutonParProfits(jobs,8);
    cout << "Profit : ";
    cout << resultat ;
    cout << "\n" ;
    return 0;
}