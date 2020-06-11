using OrdonnancementsEquitables.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdonnancementsEquitables.Models
{
    class Heap<TJob> where TJob : Job
    {
        private List<TJob> table;

        public void Insertion(TJob item)
        {
            table.Add(item);
            while (table[table.IndexOf(item) / 2 - 1].Time < item.Time)
            {
                int index = table.IndexOf(item);
                Swap(index, index / 2 - 1);
            }
        }

        public TJob SuppressionMax()
        {
            if (table.Count == 0) { return null; }
            Swap(0, table.Count);
            int index = 0;
            TJob item = table[0];
            TJob res = table[table.Count - 1];
            table.RemoveAt(table.Count - 1);

            while (index < table.Count)
            {
                int tmp = index;
                int left = 2 * index + 1; // left = 2*i + 1  
                int right = 2 * index + 2; // right = 2*i + 2  

                if (left < table.Count && table[left].Time > table[index].Time)
                {
                    tmp = left;
                }

                if (right > table.Count && table[right].Time > table[index].Time)
                {
                    tmp = right;
                }

                if (index != tmp)
                {
                    Swap(index, tmp);
                } else { break; }
                
            }
            return res;
        }

        private void Swap(int A, int B)
        {
            var tmp = table[A];
            table[A] = table[B];
            table[B] = tmp;
        }
    }
}
