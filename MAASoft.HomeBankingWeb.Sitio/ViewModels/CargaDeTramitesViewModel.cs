using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.ViewModels
{
    public class CargaDeTramitesViewModel
    {
        public string UserName { get; set; }
        public string Nombre { get; set; }
        public string IdUsuario { get; set; }
        public List<string> Archivos { get; set; }
        
        public CargaDeTramitesViewModel()
        {
            this.Archivos = new List<string>();
        }
    }
}