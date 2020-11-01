using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MAASoft.HomeBankingWeb.Sitio.Core.Tipos
{
    public class RangoFechas
    {
        public RangoFechas(string desde, string hasta, 
            int? cantMaxMeses = null,
            int? cantMesesDefault = null)
        {
            CantMaxMeses = cantMaxMeses;

            Hasta = 
                String.IsNullOrWhiteSpace(hasta) 
                ? DateTime.Today
                : DateTime.Parse(hasta);

            if (!String.IsNullOrWhiteSpace(desde))
            {
                Desde = DateTime.Parse(desde);
            }
            else if (desde == String.Empty)
            {
                Desde = (DateTime?)null;
            }
            else if (cantMesesDefault.HasValue)
            {
                Desde = CrearFechaDesdeDefault(Hasta, cantMesesDefault.Value);
            }
            else if (cantMaxMeses.HasValue)
            {
                Desde = CrearFechaDesdeMinima(Hasta);
            }
        }

        public RangoFechas(DateTime desde, DateTime hasta, 
            int? cantMaxMeses = null)
        {
            CantMaxMeses = cantMaxMeses;
            Desde = desde;
            Hasta = hasta;
        }

        public DateTime? Desde { get; private set; }

        public DateTime Hasta { get; private set; }

        public int? CantMaxMeses { get; private set; }

        public bool EsValido(out string mensaje)
        {
            if (Desde.HasValue && Desde.Value > Hasta)
            {
                mensaje = "El rango de fechas no es válido.";
                return false;
            }
            else if (CantMaxMeses.HasValue)
            {
                var fechaDesdeMinima = CrearFechaDesdeMinima(Hasta);
                if (!Desde.HasValue || Desde.Value < fechaDesdeMinima)
                {
                    mensaje = String.Format("El rango de fechas excede el máximo de meses permitidos ({0}).", CantMaxMeses);
                    return false;
                }
                else
                {
                    mensaje = null;
                    return true;
                }
            }
            else
            {
                mensaje = null;
                return true;
            }
        }

        private DateTime? CrearFechaDesdeMinima(DateTime hasta)
        {
            return CantMaxMeses.HasValue
                ? new MesYAño(hasta).PrimerDia.AddMonths(-CantMaxMeses.Value)
                : (DateTime?)null;
        }

        private DateTime CrearFechaDesdeDefault(DateTime hasta, int cantMesesDefault)
        {
            return new MesYAño(hasta).PrimerDia.AddMonths(-cantMesesDefault);
        }
    }
}