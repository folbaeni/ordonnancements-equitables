using OrdonnancementsEquitables.Jobs;
using OrdonnancementsEquitables.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OrdonnancementsEquitables.Algos
{
    public class HigherLockDegree : Algorithme<JobCo>
    {
        public override void Draw(Canvas c)
        {
            throw new NotImplementedException();
        }


        public override JobCo[] Execute(JobCo[] jobs)
        {
            GraphLock G = new GraphLock(jobs);
            JobCo[] trie = new JobCo[jobs.Length];
            JobCo tmp;
            int parcouru = jobs.Length;
            int cpt;
            if (parcouru == 1)
                return jobs;

            while(parcouru > 0) 
            {
                tmp = G.GetHigherDegree();
                if (tmp.IsLocked)
                {

                    JobCo[] depend = new JobCo[tmp.Depend.Length];
                    foreach(int id in tmp.Depend)
                    {
                        depend.Append((JobCo)Jobs.Where(jobs => jobs.Id == id));
                    }
                    cpt = 0;
                    /* foreach(JobCo j in depend) /* risque de boucle infini en cas d'interblocage ( arrive en cas de forte connexité) */
                   /* {
                        if (j.Depend.Length != depend.Length)
                            break;
                        else
                        {
                            if (depend.Contains(j.Id))
                                cpt++;                            
                        }
                    }

                    if(cpt == depend.Length)
                    {
                        Console.WriteLine("Erreur: cas d'interblocage");
                        return null;
                    } */
                }
                G.ExecuteJob(tmp);
                trie.Append(tmp);
                parcouru--;
            }
            return trie;
        }
    }
    }