using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.Models
{
    public class AhorroTerminoVigente
    {
        public DateTime Fecha { get; set; }

        [Display(Name = "Vencimiento")]
        public DateTime FechaVto { get; set; }

        [Display(Name = "Número")]
        public int Numero { get; set; }

        public int Plazo { get; set; }

        public decimal TEM { get; set; }

        public decimal TNA { get; set; }

        [Display(Name = "Depósito")]
        public decimal Deposito { get; set; }

        [Display(Name = "Estímulo")]
        public decimal Estimu { get; set; }

        public decimal Sello { get; set; }

        public decimal Total { get; set; }
    }
}