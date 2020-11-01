using MAASoft.HomeBankingWeb.Sitio.Repositorios;
using MAASoft.HomeBankingWeb.Sitio.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MAASoft.HomeBankingWeb.Sitio.Helpers
{
    public class ListasHelper
    {
        public static SelectList CrearSucursalesSelectList()
        {
            var sucursalesRepositorio = new SucursalesRepositorio();
            return new SelectList(sucursalesRepositorio.ObtenerTodas(), "Id", "Nombre");
        }

        public static SelectList CrearTiposCuentaSelectList()
        {
            var lista = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(ServiciosTiposCuentaCodigos.COMUN, ServiciosTiposCuentaCodigos.COMUN_NOMBRE),
                new KeyValuePair<string, string>(ServiciosTiposCuentaCodigos.DIFERENCIAL, ServiciosTiposCuentaCodigos.DIFERENCIAL_NOMBRE),
                new KeyValuePair<string, string>(ServiciosTiposCuentaCodigos.ESPECIAL, ServiciosTiposCuentaCodigos.ESPECIAL_NOMBRE)
            };
            return new SelectList(lista, "Key", "Value", ServiciosTiposCuentaCodigos.COMUN);
        }
    }
}