using MAASoft.HomeBankingWeb.Sitio.Filters;
using System.Web;
using System.Web.Mvc;

namespace MAASoft.HomeBankingWeb.Sitio
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttributeWithLog());
        }
    }
}
