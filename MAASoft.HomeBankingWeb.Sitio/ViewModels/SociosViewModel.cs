using MAASoft.HomeBankingWeb.Sitio.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MAASoft.HomeBankingWeb.Sitio.ViewModels
{
    public class SociosViewModel : ListadoConPaginacionViewModel<SocioModel>
    {
        public string Email { get; set; }

        [Display(Name = "Nombre y Apellido o Razón Social")]
        public string NombreApellidoORazonSocial { get; set; }

        [Display(Name = "Sucursal")]
        public byte? IdSucursal { get; set; }

        public SelectList SucursalesSelectList { get; set; }
    }
}