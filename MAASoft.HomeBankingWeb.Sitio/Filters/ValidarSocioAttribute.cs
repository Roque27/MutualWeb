using MAASoft.HomeBankingWeb.Sitio.Helpers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MAASoft.HomeBankingWeb.Sitio.Filters
{
    public class ValidarSocioAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (filterContext.HttpContext.User.Identity.IsAuthenticated 
                && SociosHelper.SocioNombreApellidoORazonSocial == null)
            {
                filterContext.HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

                filterContext.Result = new RedirectResult("~/Acceso/Login");
            }
        }
    }
}