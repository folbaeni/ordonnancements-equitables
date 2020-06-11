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
            while(table[table.IndexOf(item) / 2].Time < item.Time)
            {
                int index = table.IndexOf(item);
                Swap(index, index / 2);
            }
        }

        public void SuppressionMax()
        {
            if (table.Count == 0) { return; }
            Swap(0, table.Count);
            TJob item = table[0];
            table.RemoveAt(table.Count - 1);
            while (item.Time < table[table.IndexOf(item) * 2].Time)
            {
                int index = table.IndexOf(item);
                Swap(index, index * 2);
            }
        }
        
        private void Swap(int A, int B)
        {
            var tmp = table[A];
            table[A] = table[B];
            table[B] = tmp;
        }
    }
}
