using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.Configuracion
{
    public class Configuracion
    {
        private const string COLOR_PRINCIPAL_KEY = "ColorPrincipal";

        private const string MUTUAL_NOMBRE_KEY = "MutualNombre";
        private const string MUTUAL_DIRECCION_KEY = "MutualDireccion";

        public static string LogoURL { get { return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Content/img/logo.png"; } }

        public static string ColorPrincipal { get { return (string)Application[COLOR_PRINCIPAL_KEY]; } }
        public static string MutualNombre { get { return (string)Application[MUTUAL_NOMBRE_KEY]; } }
        public static string MutualDireccion { get { return (string)Application[MUTUAL_DIRECCION_KEY]; } }

        public static bool CajaAhorrosHabilitado { get { return (bool)Application[GenerarApplicationKeyModuloHabilitado(ModuloNombres.CAJA_AHORROS)]; } }
        public static bool AhorroATerminoPesosHabilitado { get { return (bool)Application[GenerarApplicationKeyModuloHabilitado(ModuloNombres.AHORRO_A_TERMINO_PESOS)]; } }
        public static bool AhorroATerminoDolaresHabilitado { get { return (bool)Application[GenerarApplicationKeyModuloHabilitado(ModuloNombres.AHORRO_A_TERMINO_DOLARES)]; } }
        public static bool CuotasSocietariasHabilitado { get { return (bool)Application[GenerarApplicationKeyModuloHabilitado(ModuloNombres.CUOTAS_SOCIETARIAS)]; } }
        public static bool AyudasEconomicasHabilitado { get { return (bool)Application[GenerarApplicationKeyModuloHabilitado(ModuloNombres.AYUDAS_ECONOMICAS)]; } }
        public static bool ImpuestosHabilitado { get { return (bool)Application[GenerarApplicationKeyModuloHabilitado(ModuloNombres.IMPUESTOS)]; } }
        public static bool ValoresAlCobroHabilitado { get { return (bool)Application[GenerarApplicationKeyModuloHabilitado(ModuloNombres.VALORES_AL_COBRO)]; } }
        public static bool ValoresNegociadosHabilitado { get { return (bool)Application[GenerarApplicationKeyModuloHabilitado(ModuloNombres.VALORES_NEGOCIADOS)]; } }
        public static bool ServiciosCuotasHabilitado { get { return (bool)Application[GenerarApplicationKeyModuloHabilitado(ModuloNombres.SERVICIOS_CUOTAS)]; } }

        public static string CajaAhorrosTitulo { get { return (string)Application[GenerarApplicationKeyTitulo(ModuloNombres.CAJA_AHORROS)]; } }
        public static string AhorroATerminoPesosTitulo { get { return (string)Application[GenerarApplicationKeyTitulo(ModuloNombres.AHORRO_A_TERMINO_PESOS)]; } }
        public static string AhorroATerminoDolaresTitulo { get { return (string)Application[GenerarApplicationKeyTitulo(ModuloNombres.AHORRO_A_TERMINO_DOLARES)]; } }
        public static string CuotasSocietariasTitulo { get { return (string)Application[GenerarApplicationKeyTitulo(ModuloNombres.CUOTAS_SOCIETARIAS)]; } }
        public static string AyudasEconomicasTitulo { get { return (string)Application[GenerarApplicationKeyTitulo(ModuloNombres.AYUDAS_ECONOMICAS)]; } }
        public static string ImpuestosTitulo { get { return (string)Application[GenerarApplicationKeyTitulo(ModuloNombres.IMPUESTOS)]; } }
        public static string ValoresAlCobroTitulo { get { return (string)Application[GenerarApplicationKeyTitulo(ModuloNombres.VALORES_AL_COBRO)]; } }
        public static string ValoresNegociadosTitulo { get { return (string)Application[GenerarApplicationKeyTitulo(ModuloNombres.VALORES_NEGOCIADOS)]; } }
        public static string ServiciosCuotasTitulo { get { return (string)Application[GenerarApplicationKeyTitulo(ModuloNombres.SERVICIOS_CUOTAS)]; } }

        private static HttpApplicationState Application { get { return HttpContext.Current.Application; } }

        public static void Inicializar()
        {
            InicializarConfiguracion(COLOR_PRINCIPAL_KEY);
            InicializarConfiguracion(MUTUAL_NOMBRE_KEY);
            InicializarConfiguracion(MUTUAL_DIRECCION_KEY);

            InicializarConfiguracion(GenerarApplicationKeyTitulo(ModuloNombres.CAJA_AHORROS));
            InicializarConfiguracion(GenerarApplicationKeyTitulo(ModuloNombres.AHORRO_A_TERMINO_PESOS));
            InicializarConfiguracion(GenerarApplicationKeyTitulo(ModuloNombres.AHORRO_A_TERMINO_DOLARES));
            InicializarConfiguracion(GenerarApplicationKeyTitulo(ModuloNombres.CUOTAS_SOCIETARIAS));
            InicializarConfiguracion(GenerarApplicationKeyTitulo(ModuloNombres.AYUDAS_ECONOMICAS));
            InicializarConfiguracion(GenerarApplicationKeyTitulo(ModuloNombres.IMPUESTOS));
            InicializarConfiguracion(GenerarApplicationKeyTitulo(ModuloNombres.VALORES_AL_COBRO));
            InicializarConfiguracion(GenerarApplicationKeyTitulo(ModuloNombres.VALORES_NEGOCIADOS));
            InicializarConfiguracion(GenerarApplicationKeyTitulo(ModuloNombres.SERVICIOS_CUOTAS));

            InicializarConfiguracion(GenerarApplicationKeyModuloHabilitado(ModuloNombres.CAJA_AHORROS), bool.Parse);
            InicializarConfiguracion(GenerarApplicationKeyModuloHabilitado(ModuloNombres.AHORRO_A_TERMINO_PESOS), bool.Parse);
            InicializarConfiguracion(GenerarApplicationKeyModuloHabilitado(ModuloNombres.AHORRO_A_TERMINO_DOLARES), bool.Parse);
            InicializarConfiguracion(GenerarApplicationKeyModuloHabilitado(ModuloNombres.CUOTAS_SOCIETARIAS), bool.Parse);
            InicializarConfiguracion(GenerarApplicationKeyModuloHabilitado(ModuloNombres.AYUDAS_ECONOMICAS), bool.Parse);
            InicializarConfiguracion(GenerarApplicationKeyModuloHabilitado(ModuloNombres.IMPUESTOS), bool.Parse);
            InicializarConfiguracion(GenerarApplicationKeyModuloHabilitado(ModuloNombres.VALORES_AL_COBRO), bool.Parse);
            InicializarConfiguracion(GenerarApplicationKeyModuloHabilitado(ModuloNombres.VALORES_NEGOCIADOS), bool.Parse);
            InicializarConfiguracion(GenerarApplicationKeyModuloHabilitado(ModuloNombres.SERVICIOS_CUOTAS), bool.Parse);
        }

        public static bool ElModuloEstaHabilitado(string nombre)
        {
            try
            {
                return (bool)Application[GenerarApplicationKeyModuloHabilitado(nombre)];
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static string GenerarApplicationKeyModuloHabilitado(string nombre)
        {
            return nombre + "Habilitado";
        }

        private static string GenerarApplicationKeyTitulo(string nombre)
        {
            return nombre + "Titulo";
        }

        private static void InicializarConfiguracion(string nombre)
        {
            InicializarConfiguracion<string>(nombre, null);
        }

        private static void InicializarConfiguracion<T>(string nombre, Func<string, T> mapeo)
        {
            if (mapeo == null)
            {
                Application[nombre] = ConfigurationManager.AppSettings[nombre];
            }
            else
            {
                Application[nombre] = mapeo(ConfigurationManager.AppSettings[nombre]);
            }
        }
    }
}