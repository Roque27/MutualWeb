using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.Models
{
    public class Sucursal
    {
        public byte Id { get; set; }

        public string Nombre { get; set; }

        public string WebServiceURL { get; set; }

        public string WebServiceTokenAcceso { get; set; }
    }
}