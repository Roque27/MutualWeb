using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.ViewModels
{
    public class CargaDeTramitesViewModel
    {
        public List<string> Archivos { get; set; }
        
        public CargaDeTramitesViewModel()
        {
            this.Archivos = new List<string>();
        }
    }
}