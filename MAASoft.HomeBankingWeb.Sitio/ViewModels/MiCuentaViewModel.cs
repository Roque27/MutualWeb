using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [Display(Name = "Condición frente a IVA")]
        public string CondicionIva { get; set; }

        [Display(Name = "Número de Socio")]
        public string NumeroSocio { get; set; }

        [Display(Name = "Tipo de Documento"),
        RegularExpression("CUIT|CUIL|DNI"),
        StringLength(5)]
        public string TipoDocumento { get; set; }

        [Display(Name = "Número de Documento"),
        DefaultValue(0),
        StringLength(50)]
        public string NumeroDocumento { get; set; }

        [Display(Name = "Domicilio"),
        StringLength(200)]
        public string Domicilio { get; set; }

        [Display(Name = "Localidad"),
        StringLength(200)]
        public string Localidad { get; set; }

        [Display(Name = "Código Postal"),
        StringLength(10)]
        public string CodPostal { get; set; }

        //[Display(Name = "Provincia"),
        //ReadOnly(true),
        //StringLength(200)]
        //public string Provincia { get; set; }

        [Display(Name = "Número de Celular"),
        StringLength(50)]
        public string Celular { get; set; }

        [Display(Name = "Número de Fax"),
        StringLength(50)]
        public string Fax { get; set; }

        [DataType(DataType.Password),
        Display(Name = "Contraseña actual")]
        public string ContraseñaActual { get; set; }

        [DataType(DataType.Password),
        Display(Name = "Contraseña nueva")]
        public string ContraseñaNueva { get; set; }

        [DataType(DataType.Password),
        Display(Name = "Confirmar contraseña nueva")]
        public string ConfirmarContraseñaNueva { get; set; }

        public bool DatosCompletos { get; set; }

        public bool SeDebeActualizarContraseña
        {
            get
            {
                return !String.IsNullOrWhiteSpace(ContraseñaActual)
                    && (!String.IsNullOrWhiteSpace(ContraseñaNueva) 
                    && !String.IsNullOrWhiteSpace(ConfirmarContraseñaNueva));
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

                if (String.IsNullOrWhiteSpace(ContraseñaNueva) && !String.IsNullOrWhiteSpace(ConfirmarContraseñaNueva))
                {
                    yield return new ValidationResult("La contraseña nueva es requerida.", new string[] { "ContraseñaNueva" });
                }

                if (String.IsNullOrWhiteSpace(ConfirmarContraseñaNueva) && !String.IsNullOrWhiteSpace(ContraseñaNueva))
                {
                    yield return new ValidationResult("Debe confirmar la contraseña nueva.", new string[] { "ConfirmarContraseñaNueva" });
                }
                
                if(!String.IsNullOrWhiteSpace(ConfirmarContraseñaNueva) && !String.IsNullOrWhiteSpace(ContraseñaNueva))
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