using MAASoft.HomeBankingWeb.Sitio.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MAASoft.HomeBankingWeb.Sitio.ViewModels
{
    public class DetalleCajaDeAhorrosViewModel : DetalleViewModelBase<ResumenCuenta>
    {
        [Display(Name = "Cuenta")]
        public string TipoComprobante { get; set; }
        
        public SelectList TiposComprobanteSelectList { get; set; }
    }
}