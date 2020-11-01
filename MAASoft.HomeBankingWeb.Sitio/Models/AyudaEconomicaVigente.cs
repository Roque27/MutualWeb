using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.Models
{
    public class AyudaEconomicaVigente
    {
        public DateTime Fecha { get; set; }

        public string Tipo { get; set; }

        [Display(Name = "Detalle")]
        public string Comprobantes { get; set; }

        public long Ayuda { get; set; }

        public int Plazo { get; set; }

        [Display(Name = "Vencimiento")]
        public DateTime FechaVto { get; set; }

        [Display(Name = "Total Adeudado")]
        public decimal Total { get; set; }

        public int Cuotas { get; set; }
    }
}