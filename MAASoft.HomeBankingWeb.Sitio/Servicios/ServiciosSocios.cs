using MAASoft.HomeBankingWeb.Sitio.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.Servicios
{
    public class ServiciosSocios
    {
        private const string HEADER_TOKEN_ACCESO = "TokenAcceso";

        private string _urlBase;
        private string _tokenAcceso;

        public ServiciosSocios(Sucursal sucursal)
        {
            if (sucursal == null)
            {
                throw new ArgumentNullException("sucursal");
            }

            _urlBase = sucursal.WebServiceURL;
            _tokenAcceso = sucursal.WebServiceTokenAcceso;
        }

        public Socio ObtenerDatosDelSocio(
            string nombre, string email)
        {
            return ObtenerRespuesta<Socio>(
                servicio: ServiciosNombres.SOCIOS,
                operacion: ServiciosOperacionesNombres.Socio.SOCIO_POR_NOMBRE_Y_EMAIL,
                parametros: new Dictionary<string, string> {
                    { "nombre", nombre },
                    { "email", email }
                });
        }

        public Socio ObtenerDatosDelSocio(
            string nrocuentasocio)
        {
            return ObtenerRespuesta<Socio>(
                servicio: ServiciosNombres.SOCIOS,
                operacion: ServiciosOperacionesNombres.Socio.SOCIO_POR_NRO_SOCIO,
                parametros: new Dictionary<string, string> {
                    { "nrosocio", nrocuentasocio }
                });
        }

        public Socio ActualizarDatosDelSocio(
            Socio socio)
        {
            return ObtenerRespuesta<Socio>(
                servicio: ServiciosNombres.SOCIOS,
                operacion: ServiciosOperacionesNombres.Socio.ACTUALIZAR_SOCIO,
                parametros: socio);
        }

        private TModelo ObtenerRespuesta<TModelo>(string servicio, string operacion, Dictionary<string, string> parametros)
           where TModelo : class
        {
            using (var client = CrearHttpClient())
            {
                var qs = HttpUtility.ParseQueryString("");
                foreach (var parametro in parametros)
                    qs[parametro.Key] = parametro.Value;

                var response = client.GetAsync(String.Format("{0}/{1}/{2}?{3}", _urlBase, servicio, operacion, qs)).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();

                var respuestaOperacion = response.Content.ReadAsAsync<RespuestaOperacion<TModelo>>().GetAwaiter().GetResult();
                if (respuestaOperacion.TieneError)
                    throw new Exception(String.Format("Error en operación {0}: {1}", operacion, respuestaOperacion.MensajeError));
                
                return respuestaOperacion.Resultado;
            }
        }

        private TModelo ObtenerRespuesta<TModelo>(string servicio, string operacion, TModelo parametros)
           where TModelo : class
        {
            using (var client = CrearHttpClient())
            {
                var json = JsonConvert.SerializeObject(parametros);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var response = client.PostAsync(String.Format("{0}/{1}/{2}", _urlBase, servicio, operacion), data).GetAwaiter().GetResult();
                
                response.EnsureSuccessStatusCode();

                var respuestaOperacion = response.Content.ReadAsAsync<RespuestaOperacion<TModelo>>().GetAwaiter().GetResult();
                if (respuestaOperacion.TieneError)
                    throw new Exception(String.Format("Error en operación {0}: {1}", operacion, respuestaOperacion.MensajeError));

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