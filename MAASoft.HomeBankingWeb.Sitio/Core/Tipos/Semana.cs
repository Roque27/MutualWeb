using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAASoft.HomeBankingWeb.Sitio.Core.Tipos
{
    public class Semana
    {
        public Semana(int numeroEnMes, int numeroEnAño, IEnumerable<DateTime> dias)
        {
            NumeroEnMes = numeroEnMes;
            NumeroEnAño = numeroEnAño;
            Dias = dias;
        }

        public int NumeroEnMes { get; private set; }

        public int NumeroEnAño { get; private set; }

        public IEnumerable<DateTime> Dias { get; set; }
    }
}
