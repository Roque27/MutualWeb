using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAASoft.HomeBankingWeb.Sitio.Core.Net
{
    public class GeneradorHtml
    {
        private const string TABLE_STYLE_DEFAULT = "border: 0; border-collapse: collapsed;";
        private const string CELDA_STYLE_DEFAULT = "margin: 0; padding: 5px; border: 1px solid #ccc;";
        private const string FILA_HEADER_STYLE_DEFAULT = "font-weight: bold; background-color: #eee;";
        private const string FORMATO_ENCABEZADO_DEFAULT = "<span style='font-weight: bold;'>{0}</span>";

        public string GenerarTable<TItem>(IEnumerable<string> encabezados, IEnumerable<TItem> items, Func<TItem, IEnumerable<object>> mapeoItem,
            string tableStyle = null,
            string celdaStyle = null, 
            string filaHeaderStyle = null)
            where TItem : class
        {
            tableStyle = tableStyle ?? TABLE_STYLE_DEFAULT;
            celdaStyle = celdaStyle ?? CELDA_STYLE_DEFAULT;
            filaHeaderStyle = filaHeaderStyle ?? FILA_HEADER_STYLE_DEFAULT;

            var sbItems = new StringBuilder();
            foreach(var item in items)
            {
                sbItems.AppendLine(String.Format("<tr>{0}</tr>", String.Join("", mapeoItem(item).Select(v => String.Format("<td style='{0}'>{1}</td>", celdaStyle, v)))));
            }

            string filaEncabezados = String.Format("<tr style='{0}'>{1}</tr>",
                filaHeaderStyle, String.Join("", encabezados.Select(e => String.Format("<td style='{0}'>{1}</td>", celdaStyle, e))));

            return String.Format("<table style='{0}' cellpadding='0' cellspacing='0'>{1}{2}</table>",
                tableStyle, filaEncabezados, sbItems.ToString());
        }

        public string GenerarListaEncabezadosValor(Dictionary<string,object> encabezadosYValores,
            string formatoEncabezado = null)
        {
            formatoEncabezado = formatoEncabezado ?? FORMATO_ENCABEZADO_DEFAULT;

            return Formato.FormatoListaEncabezadoYValor(encabezadosYValores,
                formatoEncabezado: formatoEncabezado,
                separadorItems: "<br />"
            );
        }
    }
}
