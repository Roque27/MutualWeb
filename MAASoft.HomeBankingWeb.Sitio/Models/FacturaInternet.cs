using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.Models
{
    public class FacturaInternet
    {
        public DateTime Fecha { get; set; }

        public string Compro { get; set; }

        public string Letra { get; set; }

        public long Numero { get; set; }

        public string Periodo { get; set; }

        public DateTime Vencimiento { get; set; }

        public decimal Importe { get; set; }

        public decimal Pago { get; set; }

        public DateTime FechaPago { get; set; }

        public decimal Interes { get; set; }
    }
}