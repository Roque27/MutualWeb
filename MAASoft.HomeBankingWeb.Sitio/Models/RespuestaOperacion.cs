using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.Models
{
    public class RespuestaOperacion<T>
        where T : class
    {
        public T Resultado { get; set; }
        public bool TieneError { get; set; }
        public string MensajeError { get; set; }
    }
}