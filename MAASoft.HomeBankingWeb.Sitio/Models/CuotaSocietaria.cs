using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.Models
{
    public class CuotaSocietaria
    {
        public DateTime Fecha { get; set; }

        public decimal Importe { get; set; }

        [Display(Name = "Fecha pago")]
        public DateTime FechaDePago { get; set; }

        public string Estado { get; set; }
    }
}