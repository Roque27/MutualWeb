using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.Servicios
{
    public class ServiciosOperacionesNombres
    {
        public class Ahorros
        {
            public const string SALDO_CAJA_DE_AHORRO = "ObtenerSaldoCajaDeAhorro";
            public const string SALDOS_CAJAS_DE_AHORROS = "ObtenerSaldosCajaDeAhorroPorSocio";
            public const string RESUMEN_CUENTAS_SOCIOS = "ResumenCuentasSocios";
            public const string AHORROS_A_TERMINO_VIGENTES = "ObtenerAhorrosATerminoVigentes";
            public const string DETALLE_DE_VALORES_AL_COBRO_DE_ACREDITACION = "DetalleDeValoresAlCobroDeAcreditacion";
        }

        public class AyudasEconomicas
        {
            public const string AYUDAS_ECONOMICAS_VIGENTES = "ObtenerAyudasEconomicasVigentes";
            public const string DETALLE_DE_CUOTAS_AYUDAS_ECONOMICAS = "DetalleDeCuotasAyudasEconomicas";
            public const string DETALLE_DE_DOCUMENTOS_AYUDAS_ECONOMICAS = "DetalleDeDocumentosAyudasEconomicas";
            public const string DETALLE_DE_CHEQUES_AYUDAS_ECONOMICAS = "DetalleDeChequesAyudasEconomicas";
            public const string DETALLE_DE_VALORES_NEGOCIADOS_AYUDAS_ECONOMICAS = "DetalleDeValoresNegociadosAyudasEconomicas";
        }

        public class CuotasSocietarias
        {
            public const string CUOTAS_SOCIETARIAS = "ObtenerCuotasSocietarias";
        }

        public class Facturacion
        {
            public const string FACTURAS_SERVICIO_DE_INTERNET = "ObtenerFacturasServicioDeInternet";
        }

        public class Impuestos
        {
            public const string IMPUESTOS_PENDIENTES = "ObtenerImpuestosPendientes";
        }

        public class Socio
        {
            public const string SOCIO_POR_DNI = "ObtenerSocioPorDNI";
            public const string SOCIO_POR_CUIT = "ObtenerSocioPorCUIT";
            public const string SOCIO_POR_NOMBRE = "ObtenerSocioPorNombre";
            public const string SOCIO_POR_NOMBRE_Y_EMAIL = "ObtenerSocioPorNombreYEmail";
            public const string ACTUALIZAR_SOCIO = "ActualizarSocio";
        }

        public class Servicios
        {
            public const string CUOTAS_POR_SOCIO = "ObtenerCuotasPorSocio";
        }
    }
}