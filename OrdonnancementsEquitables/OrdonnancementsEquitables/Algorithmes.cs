using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdonnancementsEquitables
{
    class Algorithmes
    {
        protected Algorithmes(){}
        public virtual void Execute(){}
        protected String Prefixe() => "Liste des Jobs étudiés:\n";
        public override String ToString() => "Resultat de l'algorithme:";
    }
}
