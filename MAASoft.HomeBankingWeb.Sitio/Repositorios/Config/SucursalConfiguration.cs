using MAASoft.HomeBankingWeb.Sitio.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.Repositorios.Config
{
    public class SucursalConfiguration : EntityTypeConfiguration<Sucursal>
    {
        public SucursalConfiguration()
        {
            ToTable("Sucursales");
            HasKey(e => e.Id);
        }
    }
}