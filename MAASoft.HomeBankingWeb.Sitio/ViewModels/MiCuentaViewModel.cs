using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.ViewModels
{
    public class MiCuentaViewModel : IValidatableObject
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

        [DataType(DataType.Password),
        Display(Name = "Contraseña actual")]
        public string ContraseñaActual { get; set; }

        [DataType(DataType.Password),
        Display(Name = "Contraseña nueva")]
        public string ContraseñaNueva { get; set; }

        [DataType(DataType.Password),
        Display(Name = "Confirmar contraseña nueva")]
        public string ConfirmarContraseñaNueva { get; set; }

        public bool SeDebeActualizarContraseña
        {
            get
            {
                return !String.IsNullOrWhiteSpace(ContraseñaActual)
                    || !String.IsNullOrWhiteSpace(ContraseñaNueva) 
                    || !String.IsNullOrWhiteSpace(ConfirmarContraseñaNueva);
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SeDebeActualizarContraseña)
            {
                if (String.IsNullOrWhiteSpace(ContraseñaActual))
                {
                    yield return new ValidationResult("La contraseña actual es requerida.", new string[] { "ContraseñaActual" });
                }

                if (String.IsNullOrWhiteSpace(ContraseñaNueva))
                {
                    yield return new ValidationResult("La contraseña nueva es requerida.", new string[] { "ContraseñaNueva" });
                }

                if (String.IsNullOrWhiteSpace(ConfirmarContraseñaNueva))
                {
                    yield return new ValidationResult("Debe confirmar la contraseña nueva.", new string[] { "ConfirmarContraseñaNueva" });
                }
                else
                {
                    if (ContraseñaNueva != ConfirmarContraseñaNueva)
                    {
                        yield return new ValidationResult("Las contraseñas no coinciden.", new string[] { "ConfirmarContraseñaNueva" });
                    }
                }
            }
        }
    }
}