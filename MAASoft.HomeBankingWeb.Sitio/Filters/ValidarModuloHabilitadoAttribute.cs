using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MAASoft.HomeBankingWeb.Sitio.Filters
{
    public class ValidarModuloHabilitadoAttribute : ActionFilterAttribute
    {
        public ValidarModuloHabilitadoAttribute(string moduloNombre)
        {
            if (String.IsNullOrWhiteSpace(moduloNombre))
            {
                throw new ArgumentNullException("moduloNombre");
            }

            ModuloNombre = moduloNombre;
        }

        public string ModuloNombre { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (!Configuracion.Configuracion.ElModuloEstaHabilitado(ModuloNombre))
            {
                filterContext.Result = new RedirectResult("~/");
            }
        }
    }
}