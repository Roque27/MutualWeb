using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.Models
{
    public class ResumenCuenta
    {
        public string Comprobante { get; set; } 

        public DateTime Fecha { get; set; }

        public long Numero { get; set; }

        public long Cuenta { get; set; }

        public int CodMovimiento { get; set; }

        [Display(Name = "Concepto")]
        public string Nombre { get; set; }

        public decimal Importe { get; set; }

        public decimal Saldo { get; set; }

        public int CodImp { get; set; }

        [Display(Name = "Detalle")]
        public string Observaciones { get; set; }

        public string Operador { get; set; }
    }
}