using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.Helpers
{
    public class PaginadorHelper
    {
        public static string GenerarUrlPagina(string urlPaginaBase, int pagina)
        {
            return UrlHelper.GenerarUrl(urlPaginaBase, new Dictionary<string, string> { { "p", pagina.ToString() } });
        }
    }
}