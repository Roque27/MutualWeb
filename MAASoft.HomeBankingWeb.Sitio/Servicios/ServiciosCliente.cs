using MAASoft.HomeBankingWeb.Sitio.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.Servicios
{
    public class ServiciosCliente
    {
        private readonly DateTime FECHA_MINIMA = new DateTime(1990, 1, 1);
        private const string HEADER_TOKEN_ACCESO = "TokenAcceso";

        private string _urlBase;
        private string _tokenAcceso;

        public ServiciosCliente(Sucursal sucursal)
        {
            if (sucursal == null)
            {
                throw new ArgumentNullException("sucursal");
            }

            _urlBase = sucursal.WebServiceURL;
            _tokenAcceso = sucursal.WebServiceTokenAcceso;
        }

        public SaldoCajaAhorro ObtenerSaldoCajaDeAhorro(
            string nroCuenta, string tipoCuenta)
        {
            return ObtenerRespuesta<SaldoCajaAhorro>(
                servicio: ServiciosNombres.AHORROS,
                operacion: ServiciosOperacionesNombres.Ahorros.SALDO_CAJA_DE_AHORRO,
                parametros: new Dictionary<string, string> {
                    { "cuenta", nroCuenta },
                    { "tipo", tipoCuenta },
                });
        }

        public IEnumerable<ResumenCuenta> ObtenerDetalleCajaDeAhorro(
            string nroCuenta, string tipoCuenta, DateTime? desde, DateTime hasta)
        {
            return ObtenerRespuesta<IEnumerable<ResumenCuenta>>(
                servicio: ServiciosNombres.AHORROS,
                operacion: ServiciosOperacionesNombres.Ahorros.RESUMEN_CUENTAS_SOCIOS,
                parametros: new Dictionary<string, string> {
                    { "cuenta", nroCuenta },
                    { "comprobante", tipoCuenta },
                    { "desde", ObtenerFechaDesde(desde).ToString("yyyy-MM-dd") },
                    { "hasta", hasta.ToString("yyyy-MM-dd") },
                });
        }

        public IEnumerable<AhorroTerminoVigente> ObtenerAhorroATerminoEnPesos(
            string nroCuenta, DateTime? desde, DateTime hasta)
        {
            return ObtenerRespuesta<IEnumerable<AhorroTerminoVigente>>(
                servicio: ServiciosNombres.AHORROS,
                operacion: ServiciosOperacionesNombres.Ahorros.AHORROS_A_TERMINO_VIGENTES,
                parametros: new Dictionary<string, string> {
                    { "cuenta", nroCuenta },
                    { "desde", ObtenerFechaDesde(desde).ToString("yyyy-MM-dd") },
                    { "hasta", hasta.ToString("yyyy-MM-dd") },
                    { "moneda", "P" }
                });
        }

        public IEnumerable<AhorroTerminoVigente> ObtenerAhorroATerminoEnDolares(
            string nroCuenta, DateTime? desde, DateTime hasta)
        {
            return ObtenerRespuesta<IEnumerable<AhorroTerminoVigente>>(
                servicio: ServiciosNombres.AHORROS,
                operacion: ServiciosOperacionesNombres.Ahorros.AHORROS_A_TERMINO_VIGENTES,
                parametros: new Dictionary<string, string> {
                    { "cuenta", nroCuenta },
                    { "desde", ObtenerFechaDesde(desde).ToString("yyyy-MM-dd") },
                    { "hasta", hasta.ToString("yyyy-MM-dd") },
                    { "moneda", "D" }
                });
        }

        public IEnumerable<CuotaSocietaria> ObtenerCuotasSocietarias(
            string nroCuenta, DateTime? desde, DateTime hasta)
        {
            return ObtenerRespuesta<IEnumerable<CuotaSocietaria>>(
                servicio: ServiciosNombres.CUOTAS_SOCIETARIAS,
                operacion: ServiciosOperacionesNombres.CuotasSocietarias.CUOTAS_SOCIETARIAS,
                parametros: new Dictionary<string, string> {
                    { "cuenta", nroCuenta },
                    { "desde", ObtenerFechaDesde(desde).ToString("yyyy-MM-dd") },
                    { "hasta", hasta.ToString("yyyy-MM-dd") },
                });
        }

        public IEnumerable<AyudaEconomicaVigente> ObtenerAyudasEconomicas(
            string nroCuenta, DateTime? desde, DateTime hasta)
        {
            return ObtenerRespuesta<IEnumerable<AyudaEconomicaVigente>>(
                servicio: ServiciosNombres.AYUDAS_ECONOMICAS,
                operacion: ServiciosOperacionesNombres.AyudasEconomicas.AYUDAS_ECONOMICAS_VIGENTES,
                parametros: new Dictionary<string, string> {
                    { "socio", nroCuenta },
                    { "desde", ObtenerFechaDesde(desde).ToString("yyyy-MM-dd") },
                    { "hasta", hasta.ToString("yyyy-MM-dd") },
                });
        }

        public IEnumerable<DetalleCuotaAyuda> ObtenerDetalleAyudaEconomicaCuotas(
            int ayuda)
        {
            return ObtenerRespuesta<IEnumerable<DetalleCuotaAyuda>>(
                servicio: ServiciosNombres.AYUDAS_ECONOMICAS,
                operacion: ServiciosOperacionesNombres.AyudasEconomicas.DETALLE_DE_CUOTAS_AYUDAS_ECONOMICAS,
                parametros: new Dictionary<string, string> {
                    { "ayuda", ayuda.ToString() },
                });
        }

        public IEnumerable<DetalleChequeAyuda> ObtenerDetalleAyudaEconomicaCheques(
            int ayuda)
        {
            return ObtenerRespuesta<IEnumerable<DetalleChequeAyuda>>(
                servicio: ServiciosNombres.AYUDAS_ECONOMICAS,
                operacion: ServiciosOperacionesNombres.AyudasEconomicas.DETALLE_DE_CHEQUES_AYUDAS_ECONOMICAS,
                parametros: new Dictionary<string, string> {
                    { "ayuda", ayuda.ToString() },
                });
        }

        public IEnumerable<DetalleDocumentoAyuda> ObtenerDetalleAyudaEconomicaDocumentos(
            int ayuda)
        {
            return ObtenerRespuesta<IEnumerable<DetalleDocumentoAyuda>>(
                servicio: ServiciosNombres.AYUDAS_ECONOMICAS,
                operacion: ServiciosOperacionesNombres.AyudasEconomicas.DETALLE_DE_DOCUMENTOS_AYUDAS_ECONOMICAS,
                parametros: new Dictionary<string, string> {
                    { "ayuda", ayuda.ToString() },
                });
        }

        public IEnumerable<ImpuestoPendiente> ObtenerImpuestos(string nroCuenta)
        {
            return ObtenerRespuesta<IEnumerable<ImpuestoPendiente>>(
                servicio: ServiciosNombres.IMPUESTOS,
                operacion: ServiciosOperacionesNombres.Impuestos.IMPUESTOS_PENDIENTES,
                parametros: new Dictionary<string, string> {
                    { "cuenta", nroCuenta },
                });
        }

        public IEnumerable<DetalleValorCobroAcreditacion> ObtenerValoresAlCobro(string nroCuenta)
        {
            return ObtenerRespuesta<IEnumerable<DetalleValorCobroAcreditacion>>(
                servicio: ServiciosNombres.AHORROS,
                operacion: ServiciosOperacionesNombres.Ahorros.DETALLE_DE_VALORES_AL_COBRO_DE_ACREDITACION,
                parametros: new Dictionary<string, string> {
                    { "cuenta", nroCuenta },
                });
        }

        public IEnumerable<DetalleValorNegociadoAyuda> ObtenerValoresNegociados(string nroCuenta)
        {
            return ObtenerRespuesta<IEnumerable<DetalleValorNegociadoAyuda>>(
                servicio: ServiciosNombres.AYUDAS_ECONOMICAS,
                operacion: ServiciosOperacionesNombres.AyudasEconomicas.DETALLE_DE_VALORES_NEGOCIADOS_AYUDAS_ECONOMICAS,
                parametros: new Dictionary<string, string> {
                    { "cuenta", nroCuenta },
                });
        }

        public IEnumerable<ServicioCuota> ObtenerServiciosCuotas(
            string nroCuenta)
        {
            return ObtenerRespuesta<IEnumerable<ServicioCuota>>(
                servicio: ServiciosNombres.SERVICIOS,
                operacion: ServiciosOperacionesNombres.Servicios.CUOTAS_POR_SOCIO,
                parametros: new Dictionary<string, string> {
                    { "socio", nroCuenta }
                });
        }

        private DateTime ObtenerFechaDesde(DateTime? desde)
        {
            return desde ?? FECHA_MINIMA;
        }

        private TModelo ObtenerRespuesta<TModelo>(string servicio, string operacion, Dictionary<string, string> parametros)
            where TModelo : class
        {
            using (var client = CrearHttpClient())
            {
                var qs = HttpUtility.ParseQueryString("");
                foreach (var parametro in parametros)
                {
                    qs[parametro.Key] = parametro.Value;
                }

                var response = client.GetAsync(String.Format("{0}/{1}/{2}?{3}", _urlBase, servicio, operacion, qs)).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();

                var respuestaOperacion = response.Content.ReadAsAsync<RespuestaOperacion<TModelo>>().GetAwaiter().GetResult();
                if (respuestaOperacion.TieneError)
                {
                    throw new Exception(String.Format("Error en operación {0}: {1}", operacion, respuestaOperacion.MensajeError));
                }

                return respuestaOperacion.Resultado;
            }
        }

        private HttpClient CrearHttpClient()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add(HEADER_TOKEN_ACCESO, _tokenAcceso);

            return httpClient;
        }
    }
}