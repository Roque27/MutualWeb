using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.Models
{
    public class SocioWeb
    {
        public string Id { get; set; }

        public string NombreApellidoORazonSocial { get; set; }

        public string Telefono { get; set; }

        public string NroCuenta { get; set; }

        public string TipoCuenta { get; set; }

        public string TipoCuentaAbreviado
        {
            get
            {
                return
                    String.IsNullOrWhiteSpace(TipoCuenta)
                    ? String.Empty
                    : TipoCuenta.Last().ToString(); 
            }
        }

        public byte IdSucursal { get; set; }

        public ApplicationUser Usuario { get; set; }

        public Sucursal Sucursal { get; set; }
    }
}