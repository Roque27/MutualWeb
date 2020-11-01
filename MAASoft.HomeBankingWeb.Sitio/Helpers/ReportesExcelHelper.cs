using MAASoft.HomeBankingWeb.Sitio.Core;
using MAASoft.HomeBankingWeb.Sitio.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;

namespace MAASoft.HomeBankingWeb.Sitio.Helpers
{
    public class ReportesExcelHelper
    {
        public static ActionResult GenerarActionResultExcelResumen(SaldoCajaAhorro saldo)
        {
            return ReportesExcelHelper.GenerarReporteExcelFileResult(
                new SaldoCajaAhorro[] { saldo }, "Resúmen General",
                encabezados: new string[] { "Tipo Cuenta", "Nro. Cuenta", "Saldo" },
                mapeoItemValores:
                    (item) => new object[]
                    {
                        item.Tipo,
                        item.Cuenta,
                        item.Saldo,
                    });
        }

        public static ActionResult GenerarActionResultExcelCajaDeAhorros(IEnumerable<ResumenCuenta> items)
        {
            return ReportesExcelHelper.GenerarReporteExcelFileResult(
                items, Configuracion.Configuracion.CajaAhorrosTitulo,
                encabezados: new string[] { "Fecha", "Concepto", "Importe", "Saldo", "Detalle" },
                mapeoItemValores:
                    (item) => new object[]
                    {
                        Formato.FormatoFecha(item.Fecha),
                        item.Nombre,
                        item.Importe,
                        item.Saldo,
                        item.Observaciones
                    });
        }

        public static ActionResult GenerarActionResultExcelAhorroATerminoEnPesos(IEnumerable<AhorroTerminoVigente> items)
        {
            return GenerarActionResultExcelAhorroATermino(Configuracion.Configuracion.AhorroATerminoPesosTitulo, items);
        }

        public static ActionResult GenerarActionResultExcelAhorroATerminoEnDolares(IEnumerable<AhorroTerminoVigente> items)
        {
            return GenerarActionResultExcelAhorroATermino(Configuracion.Configuracion.AhorroATerminoDolaresTitulo, items);
        }

        private static ActionResult GenerarActionResultExcelAhorroATermino(string nombre, IEnumerable<AhorroTerminoVigente> items)
        {
            return ReportesExcelHelper.GenerarReporteExcelFileResult(
                items, nombre,
                encabezados: new string[] { "Fecha", "Vencimiento", "Número", "Plazo", "TEM", "TNA", "Depósito", "Sello", "Estímulo", "Total" },
                mapeoItemValores:
                    (item) => new object[]
                    {
                        Formato.FormatoFecha(item.Fecha),
                        Formato.FormatoFecha(item.FechaVto),
                        item.Numero,
                        item.Plazo,
                        Formato.FormatoPorcentaje(item.TEM),
                        Formato.FormatoPorcentaje(item.TNA),
                        item.Deposito,
                        item.Sello,
                        item.Estimu,
                        item.Total,
                    });
        }

        public static ActionResult GenerarActionResultExcelCuotasSocietarias(IEnumerable<CuotaSocietaria> items)
        {
            return ReportesExcelHelper.GenerarReporteExcelFileResult(
                items, Configuracion.Configuracion.CuotasSocietariasTitulo,
                encabezados: new string[] { "Fecha", "Fecha pago", "Estado", "Importe" },
                mapeoItemValores:
                    (item) => new object[]
                    {
                        Formato.FormatoFecha(item.Fecha),
                        Formato.FormatoFecha(item.FechaDePago),
                        item.Estado,
                        item.Importe,
                    });
        }

        public static ActionResult GenerarActionResultExcelAyudasEconomicas(IEnumerable<AyudaEconomicaVigente> items)
        {
            return ReportesExcelHelper.GenerarReporteExcelFileResult(
                items, Configuracion.Configuracion.AyudasEconomicasTitulo,
                encabezados: new string[] { "Fecha", "Tipo", "Detalle", "Ayuda", "Plazo", "Vencimiento", "Total", "Cuotas" },
                mapeoItemValores:
                    (item) => new object[]
                    {
                        Formato.FormatoFecha(item.Fecha),
                        item.Tipo,
                        item.Comprobantes,
                        item.Ayuda,
                        item.Plazo,
                        Formato.FormatoFecha(item.FechaVto),
                        item.Total,
                        item.Cuotas,
                    });
        }

        public static ActionResult GenerarActionResultExcelDetalleAyudaEconomicaCuotas(IEnumerable<DetalleCuotaAyuda> items)
        {
            return ReportesExcelHelper.GenerarReporteExcelFileResult(
                items, Configuracion.Configuracion.AyudasEconomicasTitulo + "-Cuotas",
                encabezados: new string[] { "Ayuda", "Tipo", "Nro. Cuota", "Vencimiento", "Valor Cuota" },
                mapeoItemValores:
                    (item) => new object[]
                    {
                        item.Ayuda,
                        item.Moneda,
                        item.NroCuota,
                        Formato.FormatoFecha(item.FechaVto),
                        item.ValorCuota,
                    });
        }

        public static ActionResult GenerarActionResultExcelDetalleAyudaEconomicaCheques(IEnumerable<DetalleChequeAyuda> items)
        {
            return ReportesExcelHelper.GenerarReporteExcelFileResult(
                items, Configuracion.Configuracion.AyudasEconomicasTitulo + "-Cheques",
                encabezados: new string[] { "Acreditación", "Banco", "Localidad", "Cheque", "Importe" },
                mapeoItemValores:
                    (item) => new object[]
                    {
                        Formato.FormatoFecha(item.FechaAcr),
                        item.Banco,
                        item.Localidad,
                        item.Cheque,
                        item.Importe,
                    });
        }

        public static ActionResult GenerarActionResultExcelDetalleAyudaEconomicaDocumentos(IEnumerable<DetalleDocumentoAyuda> items)
        {
            return ReportesExcelHelper.GenerarReporteExcelFileResult(
                items, Configuracion.Configuracion.AyudasEconomicasTitulo + "-Documentos",
                encabezados: new string[] { "Acreditación", "Banco", "Importe" },
                mapeoItemValores:
                    (item) => new object[]
                    {
                        Formato.FormatoFecha(item.FechaAcr),
                        item.Banco,
                        item.Importe,
                    });
        }

        public static ActionResult GenerarActionResultExcelImpuestos(IEnumerable<ImpuestoPendiente> items)
        {
            return ReportesExcelHelper.GenerarReporteExcelFileResult(
                items, Configuracion.Configuracion.ImpuestosTitulo,
                encabezados: new string[] { "Vencimiento", "N° Boleta", "Concepto", "Importe" },
                mapeoItemValores:
                    (item) => new object[]
                    {
                        Formato.FormatoFecha(item.FechaVto),
                        item.NroBol,
                        item.Nombre,
                        item.Importe,
                    });
        }

        public static ActionResult GenerarActionResultExcelValoresAlCobro(IEnumerable<DetalleValorCobroAcreditacion> items)
        {
            return ReportesExcelHelper.GenerarReporteExcelFileResult(
                items, Configuracion.Configuracion.ValoresAlCobroTitulo,
                encabezados: new string[] { "Fecha Depósito", "Fecha Acreditación", "Banco", "Localidad", "Cheque", "Importe" },
                mapeoItemValores:
                    (item) => new object[]
                    {
                        Formato.FormatoFecha(item.FecDep),
                        Formato.FormatoFecha(item.FechaAcr),
                        item.Banco,
                        item.Localidad,
                        item.Cheque,
                        item.Importe,
                    });
        }

        public static ActionResult GenerarActionResultExcelValoresNegociados(IEnumerable<DetalleValorNegociadoAyuda> items)
        {
            return ReportesExcelHelper.GenerarReporteExcelFileResult(
                items, Configuracion.Configuracion.ValoresNegociadosTitulo,
                encabezados: new string[] { "Ayuda", "Fecha Depósito", "Fecha Acreditación", "Banco", "Localidad", "Cheque", "Importe" },
                mapeoItemValores:
                    (item) => new object[]
                    {
                        item.Ayuda,
                        Formato.FormatoFecha(item.FecDep),
                        Formato.FormatoFecha(item.FechaAcr),
                        item.Banco,
                        item.Localidad,
                        item.Cheque,
                        item.Importe,
                    });
        }

        public static ActionResult GenerarActionResultExcelServiciosCuotas(IEnumerable<ServicioCuota> items,
            int cuotasPendientes, int cuotasImpagas, int cuotasPagas, decimal totalPagado)
        {
            return ReportesExcelHelper.GenerarReporteExcelFileResult(
                items, Configuracion.Configuracion.ServiciosCuotasTitulo,
                encabezados: new string[] { "Fecha", "Servicio", "Importe", "Gastos", "Total", "Fecha Pago", "Cuota" },
                mapeoItemValores:
                    (item) => new object[]
                    {
                        Formato.FormatoFecha(item.Fecha),
                        item.Nombre,
                        item.Importe,
                        item.Gastos,
                        item.Total,
                        Formato.FormatoFecha(item.FechaPago),
                        item.Cuota,
                    },
                valoresAdicionales: new Dictionary<string, object>
                {
                    { "Cuotas pendientes", cuotasPendientes },
                    { "Cuotas impagas", cuotasImpagas },
                    { "Cuotas pagas", cuotasPagas },
                    { "Total pagado", totalPagado },
                });
        }

        public static FileResult GenerarReporteExcelFileResult<TModelo>(
            IEnumerable<TModelo> items,
            string titulo, string[] encabezados, Func<TModelo, object[]> mapeoItemValores,
            Dictionary<string,object> valoresAdicionales = null)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            if (String.IsNullOrWhiteSpace(titulo))
            {
                throw new ArgumentNullException("nombre");
            }
            if (encabezados == null || !encabezados.Any())
            {
                throw new ArgumentNullException("titulos");
            }
            if (mapeoItemValores == null)
            {
                throw new ArgumentNullException("mapeoItemValores");
            }

            using (var pkg = new ExcelPackage())
            {
                var hoja = pkg.Workbook.Worksheets.Add(titulo);

                int tituloCol = 1;
                foreach (var encabezado in encabezados)
                {
                    hoja.Cells[1, tituloCol].Value = encabezado;
                    tituloCol++;
                }

                var titulosEstilos = hoja.Cells[1, 1, 1, encabezados.Length].Style;
                titulosEstilos.Font.Bold = true;
                titulosEstilos.Fill.PatternType = ExcelFillStyle.Solid;
                titulosEstilos.Fill.BackgroundColor.SetColor(Color.LightGray);

                int fila = 2,
                    columna = 1;

                object[] valores = null;

                foreach (var item in items)
                {
                    valores = mapeoItemValores(item);
                    columna = 1;

                    foreach(var valor in valores)
                    {
                        hoja.Cells[fila, columna].Value = valor;
                        columna++;
                    }

                    fila++;
                }

                if (valoresAdicionales != null)
                {
                    foreach (var valor in valoresAdicionales)
                    {
                        hoja.Cells[fila, 1].Value = valor.Key + ":";
                        hoja.Cells[fila, 1].Style.Font.Bold = true;

                        hoja.Cells[fila, 2].Value = valor.Value;

                        fila++;
                    }
                }

                hoja.Cells[1, 1, fila, encabezados.Length].AutoFitColumns();

                var resultado = new FileContentResult(pkg.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                resultado.FileDownloadName = String.Format("{0}-{1:ddMMyyyyHHmm}.xlsx", Formato.FormatoSlug(titulo), DateTime.Now);
                return resultado;
            }
        }
    }
}