using MAASoft.HomeBankingWeb.Sitio.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.Models
{
    public class SocioModel
    {
        public string Id { get; set; }

        public string Email { get; set; }

        [Display(Name = "Nombre y Apellido o Razón Social")]
        public string NombreApellidoORazonSocial { get; set; }

        [Display(Name = "Sucursal")]
        public string SucursalNombre { get; set; }

        public DateTime? LockoutEndDateUtc { get; set; }

        [Display(Name = "Está bloqueado")]
        public bool EstaBloqueado
        {
            get { return LockoutEndDateUtc.HasValue && LockoutEndDateUtc >= DateTime.UtcNow; }
        }

        [Display(Name = "Está bloqueado")]
        public string EstaBloqueadoFormateado
        {
            get { return Formato.FormatoSiNo(EstaBloqueado); }
        }
    }
}