using MAASoft.HomeBankingWeb.Sitio.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.Repositorios
{
    public class SociosRepositorio
    {
        public IEnumerable<Socio> ObtenerTodos(
            int p, int tamañoPagina,
            out int cantTotalItems,
            string email = null,
            string nombreApellidoORazonSocial = null,
            byte? idSucursal = null,
            bool incluirSucursal = false, 
            bool incluirUsuario = false)
        {
            using (var ctx = new HomeBankingContext())
            {
                var query = ctx.Socios as IQueryable<Socio>;

                if (incluirSucursal)
                {
                    query = query.Include(e => e.Sucursal);
                }

                if (incluirUsuario)
                {
                    query = query.Include(e => e.Usuario);
                }

                if (!String.IsNullOrEmpty(email))
                {
                    query = query.Where(e => e.Usuario.Email.Contains(email));
                }

                if (!String.IsNullOrEmpty(nombreApellidoORazonSocial))
                {
                    query = query.Where(e => e.NombreApellidoORazonSocial.Contains(nombreApellidoORazonSocial));
                }

                if (idSucursal.HasValue)
                {
                    query = query.Where(e => e.IdSucursal == idSucursal.Value);
                }

                cantTotalItems = query.Count();

                return query
                    .OrderBy(e => e.NombreApellidoORazonSocial)
                    .Skip(tamañoPagina * (p - 1))
                    .Take(tamañoPagina)
                    .ToList();
            }
        }

        public Socio Obtener(string id,
            bool incluirSucursal = false)
        {
            using (var ctx = new HomeBankingContext())
            {
                return ctx.Socios
                    .Include(e => e.Sucursal)
                    .Where(e => e.Id == id)
                    .FirstOrDefault();
            }
        }

        public void Agregar(Socio socio)
        {
            using (var ctx = new HomeBankingContext())
            {
                ctx.Socios.Add(socio);
                ctx.SaveChanges();
            }
        }

        public void Actualizar(Socio socio)
        {
            using (var ctx = new HomeBankingContext())
            {
                ctx.Socios.Attach(socio);
                ctx.Entry(socio).State = EntityState.Modified;

                ctx.SaveChanges();
            }
        }

        public void Eliminar(string id)
        {
            using (var ctx = new HomeBankingContext())
            {
                var entidad = new Socio { Id = id };
                ctx.Socios.Attach(entidad);
                ctx.Entry(entidad).State = EntityState.Deleted;
                
                ctx.SaveChanges();
            }
        }
    }
}