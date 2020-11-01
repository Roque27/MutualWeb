using MAASoft.HomeBankingWeb.Sitio.Helpers;
using MAASoft.HomeBankingWeb.Sitio.Repositorios.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MAASoft.HomeBankingWeb.Sitio
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly log4net.ILog logger;

        public MvcApplication()
        {
            logger = log4net.LogManager.GetLogger("Application");
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DatabaseConfig.Config();
            Configuracion.Configuracion.Inicializar();

            log4net.Config.XmlConfigurator.Configure();
        }

        protected void Application_Error()
        {
            try
            {
                var ex = Server.GetLastError();
                logger.Error(ex.Message, ex);
            }
            catch (Exception)
            {
                // nada mas que hacer
            }
        }
    }
}
