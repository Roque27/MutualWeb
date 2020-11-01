using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.ViewModels
{
    public class DetalleViewModelBase<TModelo> : ListadoConPaginacionViewModel<TModelo>
        where TModelo : class
    {
        public DateTime? Desde { get; set; }

        public DateTime? Hasta { get; set; }
    }
}