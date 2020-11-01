using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.Models
{
    public class SaldoCajaAhorro
    {
        [Display(Name = "Tipo de Cuenta")]
        public string Tipo { get; set; }

        [Display(Name = "Nro. de Cuenta")]
        public long Cuenta { get; set; }

        public decimal Saldo { get; set; }
    }
}