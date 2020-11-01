using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.Models
{
    public class DetalleValorCobroAcreditacion
    {
        public string Tipo { get; set; }

        [Display(Name = "Fecha Depósito")]
        public DateTime FecDep { get; set; }

        [Display(Name = "Fecha Acreditación")]
        public DateTime FechaAcr { get; set; }

        public string Banco { get; set; }

        public string Localidad { get; set; }

        public long Cheque { get; set; }

        public decimal Importe { get; set; }
    }
}