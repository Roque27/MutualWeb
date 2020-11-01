using MAASoft.HomeBankingWeb.Sitio.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.Repositorios
{
    public class HomeBankingContext : ApplicationDbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Socio> Socios { get; set; }

        public DbSet<Sucursal> Sucursales { get; set; }
    }
}