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

JobList tri_par_profit(JobList* l){
    JobList lr = JobList();
    while (l->get_elem()->get_job() != nullptr)
    {
        lr.add_job(l->pop_biggest());
    }
    return lr;

}

JobList tri_malin(JobList* l){
    JobList lr = JobList();
    JobList* bis = l;
    while (l->get_elem()->get_job() != nullptr)
    {
        bis = l;
        Job *k = bis->get_elem()->get_job();
        JobElem* pop;
        while (bis->get_elem()->get_job() != nullptr)
        {
           Job *j = bis->get_elem()->get_job(); 
           if(j->get_profit()/j->get_deadline() >= k->get_profit()/k->get_deadline()){
               k = j;
               pop = bis->get_elem();
           } 
           bis->get_elem() = bis->get_elem()->get_next();
        }
        lr.add_job(pop->get_job());
        l->remove(pop);
    }

    return lr;
}

int gloutonParProfits(JobList* jl){ //mettre en parametre la fonction de tri serait cool
    int tps = 0;
    int profit = 0;

    while (jl->get_elem()->get_job() != nullptr)
    {
        tps += jl->get_elem()->get_time();
        if(tps <= jl->get_elem()->get_job()->get_deadline()){
            profit += jl->get_elem()->get_job()->get_profit();
        }
        jl->get_elem() = jl->get_elem()->get_next();
    }
    return profit;
}

int main() {
    //POUR ALI





    return 0;
}