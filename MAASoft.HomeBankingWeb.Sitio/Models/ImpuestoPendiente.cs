using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.Models
{
    public class ImpuestoPendiente
    {
        [Display(Name = "Vencimiento")]
        public DateTime FechaVto { get; set; }

        [Display(Name = "N° Boleta")]
        public string NroBol { get; set; }

        [Display(Name = "Concepto")]
        public string Nombre { get; set; }

        public decimal Importe { get; set; }
    }
}