using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MAASoft.HomeBankingWeb.Sitio.ViewModels
{
    public class SocioViewModel
    {
        [Required,
        EmailAddress,
        StringLength(256)]
        public string Email { get; set; }

        [Display(Name = "Nombre y Apellido o Razón Social"),
        Required,
        StringLength(150)]
        public string NombreApellidoORazonSocial { get; set; }

        [Display(Name = "Teléfono"),
        StringLength(30)]
        public string Telefono { get; set; }

        [Display(Name = "Nro. Cuenta"),
        Required,
        StringLength(30)]
        public string NroCuenta { get; set; }

        [Display(Name = "Tipo Cuenta"),
        Required,
        StringLength(4)]
        public string TipoCuenta { get; set; }

        [Display(Name = "Sucursal"),
        Required]
        public byte? IdSucursal { get; set; }

        public SelectList SucursalesSelectList { get; set; }

        public SelectList TiposCuentaSelectList { get; set; }
    }
}