using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAASoft.HomeBankingWeb.Sitio.Core.Mail
{
    public class Adjunto
    {
        public Adjunto(string nombre, byte[] contenido)
        {
            if (String.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentNullException("nombre");
            }
            if (contenido == null || contenido.Length == 0)
            {
                throw new ArgumentNullException("contenido");
            }

            Nombre = nombre;
            Contenido = contenido;
        }

        public string Nombre { get; private set; }

        public byte[] Contenido { get; private set; }
    }
}
