using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.Models
{
    public class DetalleCuotaAyuda
    {
        public long Ayuda { get; set; }

        [Display(Name = "Tipo")]
        public string Moneda { get; set; }

        [Display(Name = "Vencimiento")]
        public DateTime FechaVto { get; set; }

        [Display(Name = "Nro. Cuota")]
        public long NroCuota { get; set; }

        [Display(Name = "Valor Cuota")]
        public decimal ValorCuota { get; set; }
    }
}