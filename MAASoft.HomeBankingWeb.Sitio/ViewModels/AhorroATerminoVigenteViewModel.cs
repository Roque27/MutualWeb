using MAASoft.HomeBankingWeb.Sitio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.ViewModels
{
    public class AhorroATerminoVigenteViewModel : DetalleViewModelBase<AhorroTerminoVigente>
    {
        public AhorroATerminoMoneda Moneda { get; set; }
    }
}