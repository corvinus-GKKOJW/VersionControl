using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mikroszimulacio.Entities
{
    public class DeathProb
    {
        public int Age { get; set; }
        public int NumberOfChildren { get; set; }
        public double DeathProbability { get; set; }
    }
}
