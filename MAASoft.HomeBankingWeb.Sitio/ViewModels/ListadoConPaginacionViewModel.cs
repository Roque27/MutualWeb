using MAASoft.HomeBankingWeb.Sitio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.ViewModels
{
    public class ListadoConPaginacionViewModel<TItem>
        where TItem : class
    {
        public ListadoConPaginacionViewModel()
        {
            Items = new List<TItem>();
        }

        public IList<TItem> Items { get; set; }

        public bool TieneItems
        {
            get { return Items != null && Items.Any(); }
        }

        public Paginacion Paginacion { get; set; }

        public void CargarItemsYPaginacion(IList<TItem> items, int nroPagina, 
            int cantTotalItems, int cantItemsPorPagina)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            Items = items;
            Paginacion = new Paginacion(nroPagina, items.Count, cantTotalItems, cantItemsPorPagina);
        }

        public void CargarItemsYPaginacionDesdeListaCompletaDeItems(IList<TItem> items, int nroPagina, 
            int cantItemsPorPagina)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            Items = items
                .Skip(cantItemsPorPagina * (nroPagina - 1))
                .Take(cantItemsPorPagina)
                .ToList();

            Paginacion = new Paginacion(nroPagina, Items.Count, items.Count, cantItemsPorPagina);
        }
    }
}