using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.Servicios
{
    public class ServiciosTiposCuentaCodigos
    {
        public const string COMUN = "AMVC";
        public const string ESPECIAL = "AMVE";
        public const string DIFERENCIAL = "AMVD";

        public const string COMUN_NOMBRE = "Común";
        public const string ESPECIAL_NOMBRE = "Especial";
        public const string DIFERENCIAL_NOMBRE = "Diferencial";

        public static string ObtenerNombreDesdeCodigo(string codigo)
        {
            if (String.IsNullOrWhiteSpace(codigo))
            {
                throw new ArgumentNullException("codigo");
            }

            if (codigo.Length == 1)
            {
                codigo = "AMV" + codigo;
            }

            switch (codigo)
            {
                case COMUN:
                    return COMUN_NOMBRE;
                case ESPECIAL:
                    return ESPECIAL_NOMBRE;
                case DIFERENCIAL:
                    return DIFERENCIAL_NOMBRE;
                default:
                    throw new Exception(String.Format("Tipo de Cuenta no contemplado ({0}).", codigo));
            }
        }
    }
}