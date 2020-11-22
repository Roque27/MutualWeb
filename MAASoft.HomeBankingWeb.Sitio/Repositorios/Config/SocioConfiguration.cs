using MAASoft.HomeBankingWeb.Sitio.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.Repositorios.Config
{
    public class SocioConfiguration : EntityTypeConfiguration<SocioWeb>
    {
        public SocioConfiguration()
        {
            ToTable("Socios");
            HasKey(e => e.Id);

            HasRequired(e => e.Usuario)
                .WithRequiredDependent();

            HasRequired(e => e.Sucursal)
                .WithMany()
                .HasForeignKey(e => e.IdSucursal);
        }
    }
}