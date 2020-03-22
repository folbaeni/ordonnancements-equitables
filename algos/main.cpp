#include <iostream>
#include <new>

#include "Job.h"
#include "JobP.h"
#include "JobElem.h"
#include "JobList.h"

using namespace std;

void Hogdson(Job jobs[], int n) {

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
    Job jobs[] = {
        Job(6, 8),
        Job(4, 9),
        Job(7, 15),
        Job(8, 20),
        Job(3, 21),
        Job(5, 22)
    };

    for (int i = 0; i < n; i++) {
        cout << jobs[i].to_string() << endl;
    }

    cout << endl << endl;
    Hogdson(jobs, n);
}


int gloutonParProfits(JobP* jobs, int taille){ 
    int time = 0;
    int profit = 0;
    JobP tmp;
    for(int i(0); i < taille; i++){ /* boucle pour faire avancer le tps */
        for(int j(i); j < taille; j++){ /* parcours de tableau */
            if(jobs[j].get_deadline() < jobs[i].get_deadline() && jobs[j].get_deadline() < jobs[i].get_deadline()){
                tmp = jobs[i];
                jobs[i] = jobs[j];
                jobs[j] = tmp;
            }
        }/* on a ordonnancé selon le principe glouton par profit */
        cout << jobs[i].to_string() ;
        cout << "\n";

        if(jobs[i].get_deadline() > time){
            profit += jobs[i].get_profit();
        }
        time++;
    }
    return profit;
}

void gpp_helper() {
    int n = 8;
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

    for (int i = 0; i < n; i++) {
        cout << jobs[i].to_string() << endl;
    }

    cout << "\n\n";
    
    int resultat = gloutonParProfits(jobs, n);
    cout << "Profit : " << resultat << endl;
}

int main() {
    gpp_helper();
    return 0;
}