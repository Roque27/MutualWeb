using MAASoft.HomeBankingWeb.Sitio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.Repositorios
{
    public class SucursalesRepositorio
    {
        public IEnumerable<Sucursal> ObtenerTodas()
        {
            using (var ctx = new HomeBankingContext())
            {
                return ctx.Sucursales
                    .AsNoTracking()
                    .OrderBy(e => e.Nombre)
                    .ToList();
            }
        }

        public Sucursal Obtener(byte sucursalId)
        {
            using (var ctx = new HomeBankingContext())
            {
                return ctx.Sucursales
                    .AsNoTracking()
                    .Where(e => e.Id == sucursalId)
                    .FirstOrDefault();
            }
        }
    }
}