using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.Models
{
    public class Socio
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string Domicilio { get; set; }
        public string Localidad { get; set; }
        public string CodPostal { get; set; }
        public string Telefono { get; set; }
        public string Fax { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public char Socade { get; set; }
        public decimal Cuota { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string TipoDocumento { get; set; }
        public long NroDocumento { get; set; }
        public long CUIT { get; set; }
        public string SituacionIva { get; set; }
    }
}