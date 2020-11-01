using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.Models
{
    public class DetalleDocumentoAyuda
    {
        public long Ayuda { get; set; }

        public string Tipo { get; set; }

        [Display(Name = "Acreditación")]
        public DateTime FechaAcr { get; set; }

        public string Banco { get; set; }

        public decimal Importe { get; set; }
    }
}