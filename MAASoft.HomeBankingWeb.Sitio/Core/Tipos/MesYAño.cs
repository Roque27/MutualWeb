using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAASoft.HomeBankingWeb.Sitio.Core.Tipos
{
    public class MesYAño : IComparable<MesYAño>
    {
        public MesYAño(string añoYMes)
        {
            if (String.IsNullOrWhiteSpace(añoYMes))
            {
                throw new ArgumentNullException("añoYMes");
            }

            int año, mes;
            try
            {
                año = int.Parse(añoYMes.Substring(0, 4));
                mes = int.Parse(añoYMes.Substring(4, 2));
            }
            catch (Exception)
            {
                throw new FormatException(String.Format("Formato de año y mes inválido. Formato esperado: aaaamm, Valor: {0}.", añoYMes));
            }

            Inicializar(mes, año);
        }

        public MesYAño(DateTime fecha)
            : this(fecha.Month, fecha.Year)
        {
        }

        public MesYAño(int mes, int año)
        {
            Inicializar(mes, año);
        }

        public int Mes { get; private set; }
        public int Año { get; private set; }

        protected CultureInfo Culture
        {
            get { return CultureInfo.CurrentUICulture; }
        }

        protected Calendar Calendar
        {
            get { return Culture.Calendar; }
        }

        public string TextoMesYAño
        {
            get { return String.Format("{0} {1}", Culture.DateTimeFormat.GetMonthName(Mes), Año); }
        }

        public string TextoAñoYMes
        {
            get { return String.Format("{0:D4}{1:D2}", Año, Mes); }
        }

        public MesYAño MesYAñoSiguiente
        {
            get
            {
                int sigAño = Año,
                    sigMes = Mes + 1;

                if (sigMes == 13)
                {
                    sigAño++;
                    sigMes = 1;
                }

                return new MesYAño(sigMes, sigAño);
            }
        }

        public MesYAño MesYAñoAnterior
        {
            get
            {
                int antAño = Año,
                    antMes = Mes - 1;

                if (antMes == 0)
                {
                    antAño--;
                    antMes = 12;
                }

                return new MesYAño(antMes, antAño);
            }
        }

        public DateTime PrimerDia
        {
            get { return new DateTime(Año, Mes, 1); }
        }

        public DateTime UltimoDia
        {
            get { return MesYAñoSiguiente.PrimerDia.AddDays(-1); }
        }

        private void Inicializar(int mes, int año)
        {
            if (mes < 1 || mes > 12)
            {
                throw new ArgumentOutOfRangeException("mes", mes, "Mes inválido.");
            }
            if (año < 1)
            {
                throw new ArgumentOutOfRangeException("año", año, "Año inválido.");
            }

            Mes = mes;
            Año = año;
        }

        public IEnumerable<Semana> ObtenerSemanas()
        {
            DateTime fechaDesde = this.PrimerDia,
                fechaHasta = this.UltimoDia;
            if (fechaDesde.DayOfWeek != DayOfWeek.Monday)
            {
                fechaDesde = fechaDesde.AddDays(-(int)fechaDesde.DayOfWeek + 1);
            }
            if (fechaHasta.DayOfWeek != DayOfWeek.Sunday)
            {
                fechaHasta = fechaHasta.AddDays(7 - (int)fechaHasta.DayOfWeek);
            }

            DateTime fecha = fechaDesde;
            var semanas = new List<Semana>();
            int numeroSemanaEnMes = 1;
            int numeroSemanaEnAño = this.Calendar.GetWeekOfYear(fecha, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            var dias = new List<DateTime>();
            while (fecha <= fechaHasta)
            {
                dias.Add(fecha);

                if (fecha.DayOfWeek == DayOfWeek.Sunday)
                {
                    semanas.Add(new Semana(numeroSemanaEnMes, numeroSemanaEnAño, dias));

                    numeroSemanaEnMes++;
                    numeroSemanaEnAño++;
                    dias = new List<DateTime>();
                }

                fecha = fecha.AddDays(1);
            }

            return semanas;
        }

        public override string ToString()
        {
            return TextoMesYAño;
        }

        public int CompareTo(MesYAño other)
        {
            if (other == null)
            {
                return -1;
            }

            int añoCompare = this.Año.CompareTo(other.Año);
            if (añoCompare != 0)
            {
                return añoCompare;
            }

            return this.Mes.CompareTo(other.Mes);
        }
    }
}
