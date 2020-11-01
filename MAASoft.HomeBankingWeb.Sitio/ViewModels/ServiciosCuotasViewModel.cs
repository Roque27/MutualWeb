using MAASoft.HomeBankingWeb.Sitio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.ViewModels
{
    public class ServiciosCuotasViewModel : DetalleViewModelBase<ServicioCuota>
    {
        public int CuotasPendientes { get; set; }

        public int CuotasImpagas { get; set; }

        public int CuotasPagas { get; set; }

        public decimal TotalPagado { get; set; }
    }
}