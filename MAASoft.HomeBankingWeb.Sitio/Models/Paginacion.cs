using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.Models
{
    public class Paginacion
    {
        public Paginacion(int paginaActual, int cantItemsPaginaActual, int cantTotalItems, int cantItemsPorPagina)
        {
            PaginaActual = paginaActual;
            CantItemsPaginaActual = cantItemsPaginaActual;
            CantTotalItems = cantTotalItems;
            CantTotalPaginas = 
                cantItemsPorPagina == 0 
                ? 0
                : (int)Math.Ceiling((double)cantTotalItems / cantItemsPorPagina);
        }

        public int PaginaActual { get; private set; }

        public int CantItemsPaginaActual { get; private set; }

        public int CantTotalItems { get; private set; }

        public int CantTotalPaginas { get; private set; }

        public bool TieneUnicaPagina
        {
            get { return CantTotalPaginas == 1; }
        }

        public bool TienePaginaAnterior
        {
            get { return PaginaActual > 1; }
        }

        public bool TienePaginaSiguiente
        {
            get { return PaginaActual < CantTotalPaginas; }
        }

        public int PaginaAnterior
        {
            get
            {
                return TienePaginaAnterior
                    ? PaginaActual - 1
                    : 1;
            }
        }

        public int PaginaSiguiente
        {
            get
            {
                return TienePaginaSiguiente
                    ? PaginaActual + 1
                    : CantTotalPaginas;
            }
        }
    }
}