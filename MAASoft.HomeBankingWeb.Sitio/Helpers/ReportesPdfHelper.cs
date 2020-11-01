using MAASoft.HomeBankingWeb.Sitio.Core;
using MAASoft.HomeBankingWeb.Sitio.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MAASoft.HomeBankingWeb.Sitio.Helpers
{
    public class ReportesPdfHelper
    {
        public static FileResult GenerarReporteAhorrosATerminoEnPesosPdfFileResult(
            IEnumerable<AhorroTerminoVigente> datos)
        {
            return GenerarReportePdfFileResult("AhorrosATermino",
                Configuracion.Configuracion.AhorroATerminoPesosTitulo, datos);
        }

        public static FileResult GenerarReporteAhorrosATerminoEnDolaresPdfFileResult(
            IEnumerable<AhorroTerminoVigente> datos)
        {
            return GenerarReportePdfFileResult("AhorrosATermino",
                Configuracion.Configuracion.AhorroATerminoDolaresTitulo, datos);
        }

        public static FileResult GenerarReporteAyudasEconomicasPdfFileResult(
            IEnumerable<AyudaEconomicaVigente> datos)
        {
            return GenerarReportePdfFileResult("AyudasEconomicas",
                Configuracion.Configuracion.AyudasEconomicasTitulo, datos);
        }

        public static FileResult GenerarReporteDetalleAyudaEconomicaChequesPdfFileResult(
            IEnumerable<DetalleChequeAyuda> datos)
        {
            return GenerarReportePdfFileResult("DetalleAyudaEconomicaCheques",
                Configuracion.Configuracion.AyudasEconomicasTitulo + " - Cheques", datos);
        }

        public static FileResult GenerarReporteDetalleAyudaEconomicaCuotasPdfFileResult(
            IEnumerable<DetalleCuotaAyuda> datos)
        {
            return GenerarReportePdfFileResult("DetalleAyudaEconomicaCuotas",
                Configuracion.Configuracion.AyudasEconomicasTitulo + " - Cuotas", datos);
        }

        public static FileResult GenerarReporteDetalleAyudaEconomicaDocumentosPdfFileResult(
            IEnumerable<DetalleDocumentoAyuda> datos)
        {
            return GenerarReportePdfFileResult("DetalleAyudaEconomicaDocumentos",
                Configuracion.Configuracion.AyudasEconomicasTitulo + " - Documentos", datos);
        }

        public static FileResult GenerarReporteCajaDeAhorrosPdfFileResult(
            IEnumerable<ResumenCuenta> datos)
        {
            return GenerarReportePdfFileResult("CajaDeAhorros",
                Configuracion.Configuracion.CajaAhorrosTitulo, datos);
        }

        public static FileResult GenerarReporteCuotasSocietariasPdfFileResult(
            IEnumerable<CuotaSocietaria> datos)
        {
            return GenerarReportePdfFileResult("CuotasSocietarias",
                Configuracion.Configuracion.CuotasSocietariasTitulo, datos);
        }

        public static FileResult GenerarReporteImpuestosPdfFileResult(
           IEnumerable<ImpuestoPendiente> datos)
        {
            return GenerarReportePdfFileResult("Impuestos",
                Configuracion.Configuracion.ImpuestosTitulo, datos);
        }

        public static FileResult GenerarReporteResumenPdfFileResult(
           SaldoCajaAhorro datos)
        {
            return GenerarReportePdfFileResult("Resumen",
                "Resúmen General", new List<SaldoCajaAhorro> { datos });
        }

        public static FileResult GenerarReporteValoresAlCobroPdfFileResult(
           IEnumerable<DetalleValorCobroAcreditacion> datos)
        {
            return GenerarReportePdfFileResult("ValoresAlCobro",
                Configuracion.Configuracion.ValoresAlCobroTitulo, datos);
        }

        public static FileResult GenerarReporteValoresNegociadosPdfFileResult(
           IEnumerable<DetalleValorNegociadoAyuda> datos)
        {
            return GenerarReportePdfFileResult("ValoresNegociados",
                Configuracion.Configuracion.ValoresNegociadosTitulo, datos);
        }

        public static FileResult GenerarReporteServiciosCuotasPdfFileResult(
            IEnumerable<ServicioCuota> datos,
            int cuotasPendientes, int cuotasImpagas, int cuotasPagas, decimal totalPagado)
        {
            return GenerarReportePdfFileResult("ServiciosCuotas",
                Configuracion.Configuracion.ServiciosCuotasTitulo, datos,
                parametrosAdicionales: new Dictionary<string, string>
                {
                    { "CuotasPendientes", cuotasPendientes.ToString() },
                    { "CuotasImpagas", cuotasImpagas.ToString() },
                    { "CuotasPagas", cuotasPagas.ToString() },
                    { "TotalPagado", totalPagado.ToString() },
                });
        }

        private static FileResult GenerarReportePdfFileResult(
            string nombreReporte,
            string titulo,
            IEnumerable datos,
            Dictionary<string, string> parametrosAdicionales = null)
        {
            var reportViewer = new ReportViewer();
            reportViewer.LocalReport.ReportPath = ObtenerPathReporte(nombreReporte);
            reportViewer.LocalReport.EnableExternalImages = true;
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("Datos", datos));

            var parametros = new List<ReportParameter>
            {
                new ReportParameter("Titulo", titulo),
                new ReportParameter("ColorPrincipal", Configuracion.Configuracion.ColorPrincipal),
                new ReportParameter("LogoURL", Configuracion.Configuracion.LogoURL),
                new ReportParameter("MutualNombre", Configuracion.Configuracion.MutualNombre),
                new ReportParameter("MutualDireccion", Configuracion.Configuracion.MutualDireccion.Replace(@"\n", Environment.NewLine))
            };

            if (parametrosAdicionales != null)
            {
                foreach (var parametro in parametrosAdicionales)
                {
                    parametros.Add(new ReportParameter(parametro.Key, parametro.Value));
                }
            }

            reportViewer.LocalReport.SetParameters(parametros);

            var resultado = new FileContentResult(reportViewer.LocalReport.Render("PDF"), "application/pdf");
            resultado.FileDownloadName = String.Format("{0}-{1:ddMMyyyyHHmm}.pdf", Formato.FormatoSlug(titulo), DateTime.Now);
            return resultado;
        }

        private static string ObtenerPathReporte(string nombre)
        {
            return Path.Combine(
                ConfigurationManager.AppSettings["ReportesPath"],
                String.Format("{0}.rdlc", nombre)
            );
        }
    }
}