using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.Helpers
{
    public class UrlHelper
    {
        public static string GenerarUrl(string urlBase, IDictionary<string,string> parametros)
        {
            string[] urlPartes = urlBase.Split('?');

            string urlPath = urlPartes[0];
            string urlParametros =
                urlPartes.Length == 2
                ? urlPartes[1]
                : "";

            var queryString = HttpUtility.ParseQueryString(urlParametros);
            foreach (var parametro in parametros)
            {
                queryString[parametro.Key] = parametro.Value;
            }

            return String.Format("{0}?{1}", urlPath, queryString.ToString());
        }
    }
}