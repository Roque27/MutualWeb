using MAASoft.HomeBankingWeb.Sitio.Repositorios;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.Repositorios.Config
{
    public class DatabaseConfig
    {
        public static void Config()
        {
            Database.SetInitializer<HomeBankingContext>(null);
        }
    }
}