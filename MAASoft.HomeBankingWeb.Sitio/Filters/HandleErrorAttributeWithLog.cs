using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MAASoft.HomeBankingWeb.Sitio.Filters
{
    public class HandleErrorAttributeWithLog : HandleErrorAttribute
    {
        private readonly log4net.ILog logger;

        public HandleErrorAttributeWithLog()
        {
            logger = log4net.LogManager.GetLogger("HandleError");
        }

        public override void OnException(ExceptionContext filterContext)
        {
            logger.Error(filterContext.Exception.Message, filterContext.Exception);

            base.OnException(filterContext);
        }
    }
}