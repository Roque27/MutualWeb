using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.Helpers
{
    public class SociosHelper
    {
        public static string SocioNombreApellidoORazonSocial
        {
            get { return (string)HttpContext.Current.Session["SocioNombreApellidoORazonSocial"]; }
            set { HttpContext.Current.Session["SocioNombreApellidoORazonSocial"] = value; }
        }
    }
}