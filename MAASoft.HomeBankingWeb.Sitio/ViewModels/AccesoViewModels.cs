using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MAASoft.HomeBankingWeb.Sitio.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }
    }

    public class ActivarCuentaViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required,
        DataType(DataType.Password),
        Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required,
        DataType(DataType.Password),
        Display(Name = "Confirmar Contraseña"),
        Compare("Password", ErrorMessage = "Las contraseñas deben coincidir.")]
        public string ConfirmarPassword { get; set; }
    }

    public class RestablecerContraseñaSolicitudViewModel
    {
        [Required,
        EmailAddress,
        Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class RestablecerContraseñaViewModel
    {
        [Required,
        EmailAddress,
        Display(Name = "Email")]
        public string Email { get; set; }

        [Required,
        DataType(DataType.Password),
        Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required,
        DataType(DataType.Password),
        Display(Name = "Confirmar Contraseña"),
        Compare("Password", ErrorMessage = "Las contraseñas deben coincidir.")]
        public string ConfirmarPassword { get; set; }
    }
}
