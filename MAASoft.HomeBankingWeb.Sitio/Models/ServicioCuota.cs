using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.Models
{
    public class ServicioCuota
    {
        public DateTime Fecha { get; set; }

        [Display(Name = "Servicio")]
        public string Nombre { get; set; }

        public int Numero { get; set; }

        public int Periodo { get; set; }

        public decimal Importe { get; set; }

        public decimal Gastos { get; set; }

        public decimal Total { get; set; }

        [Display(Name = "Fecha Pago")]
        public DateTime? FechaPago { get; set; }

        public int Cuota { get; set; }

        public int Plan { get; set; }
    }
}