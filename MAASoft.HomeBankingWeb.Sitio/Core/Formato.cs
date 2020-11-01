using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MAASoft.HomeBankingWeb.Sitio.Core
{
    public class Formato
    {
        public const string ARCHIVO_FORMATO_EXCEL = "excel";
        public const string ARCHIVO_FORMATO_PDF = "pdf";

        public static string FormatoSlug(string texto)
        {
            if (String.IsNullOrWhiteSpace(texto))
            {
                return texto;
            }

            texto = Encoding.ASCII.GetString(Encoding.GetEncoding("Cyrillic").GetBytes(texto.ToLower()));
            texto = Regex.Replace(texto, @"[^a-z0-9\s-]", "");
            texto = Regex.Replace(texto, @"\s+", " ").Trim();
            texto = Regex.Replace(texto, @"\s", "-");
            texto = Regex.Replace(texto, @"\-+", "-");

            return texto;
        }

        public static string FormatoSiNo(bool valor)
        {
            return valor ? "Si" : "No";
        }

        public static string FormatoMoneda(decimal? valor)
        {
            return valor.HasValue ? FormatoMoneda(valor.Value) : String.Empty;
        }

        public static string FormatoMoneda(decimal valor)
        {
            return valor.ToString("C2");
        }

        public static string FormatoFecha(DateTime? fecha)
        {
            return fecha.HasValue ? FormatoFecha(fecha.Value) : String.Empty;
        }

        public static string FormatoFecha(DateTime fecha)
        {
            return fecha.ToString("dd/MM/yyyy");
        }

        public static string FormatoPrecio(decimal precio,
            bool incluirSignosYSeparadorMiles = true)
        {
            string formato =
                incluirSignosYSeparadorMiles
                ? "C2"
                : "F2";

            return precio.ToString(formato);
        }

        public static string FormatoPorcentaje(decimal numero,
            int cantDecimales = 1)
        {
            return String.Format("{0:N" + cantDecimales + "} %", numero);
        }

        public static string FormatoDecimal(decimal numero,
            int cantDecimales = 1)
        {
            return numero.ToString("N" + cantDecimales);
        }

        public static string FormatoListaEncabezadoYValor(Dictionary<string,object> valores,
            string formatoEncabezado = null, string formatoValor = null, 
            string separadorEncabezadoYValor = ": ", string separadorItems = ", ")
        {
            if (String.IsNullOrWhiteSpace(formatoEncabezado))
            {
                formatoEncabezado = "{0}";
            }
            if (String.IsNullOrWhiteSpace(formatoValor))
            {
                formatoValor = "{0}";
            }

            return String.Join(separadorItems, valores.Select(it => String.Format(formatoEncabezado, it.Key) + separadorEncabezadoYValor + String.Format(formatoValor, it.Value)));
        }

        public static string FormatoVendedorNroRemito(int idVendedor, int nroRemito)
        {
            return String.Format("{0:D4}-{1:D8}", idVendedor, nroRemito);
        }
    }
}
