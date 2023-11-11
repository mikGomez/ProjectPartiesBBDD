using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoPoblacion.Domain
{
    public class Dates
    {
        public int pobla {  get; set; }
        public int absten { get; set; }
        public int nullVotes { get; set; }
        public int votesValid { get; set; }
        public Dates() { }
        public Dates(int poblation, int absten, int nullVotes)
        {
            this.pobla = poblation;
            this.absten = absten;
            this.nullVotes = nullVotes;
        }
        public void calcularValidos(int nVotes,int votAbs,int pob)
        {
            votesValid = pob - nVotes - votAbs;
        }

        public override string? ToString()
        {
            return base.ToString();
        }

    }
}
