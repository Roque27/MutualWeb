using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MAASoft.HomeBankingWeb.Sitio.Core.Mail
{
    public class ServidorCorreos
    {
        public void EnviarCorreo(string destinatarioEmail, string asunto, string nombrePlantilla, 
            Dictionary<string,string> campos = null,
            IEnumerable<Adjunto> adjuntos = null)
        {
            if (String.IsNullOrWhiteSpace(nombrePlantilla))
            {
                throw new ArgumentNullException("nombrePlantilla");
            }

            var contenido = File.ReadAllText(Path.Combine(ConfigurationManager.AppSettings["CorreosPath"], nombrePlantilla + ".html"));
            if (campos != null)
            {
                foreach (var campo in campos)
                {
                    contenido = contenido.Replace("{{Campo" + campo.Key + "}}", campo.Value);
                }
            }
            
            var cuerpo = File.ReadAllText(Path.Combine(ConfigurationManager.AppSettings["CorreosPath"], "Base.html"));
            cuerpo = cuerpo
                .Replace("{{CampoLogoURL}}", Configuracion.Configuracion.LogoURL)
                .Replace("{{CampoColorPrincipal}}", Configuracion.Configuracion.ColorPrincipal)
                .Replace("{{CampoCuerpoContenido}}", contenido)
                .Replace("{{CampoMutualNombre}}", Configuracion.Configuracion.MutualNombre);

            EnviarCorreo(destinatarioEmail, asunto, cuerpo, adjuntos);
        }

        public void EnviarCorreo(string destinatarioEmail, string asunto, string cuerpo, 
            IEnumerable<Adjunto> adjuntos = null)
        {
            using (var smtp = new SmtpClient())
            using (var correo = new MailMessage())
            {
                correo.To.Add(destinatarioEmail);
                correo.SubjectEncoding = correo.BodyEncoding = Encoding.UTF8;
                correo.Subject = asunto;
                correo.IsBodyHtml = true;
                correo.Body = cuerpo;

                if (adjuntos != null)
                {
                    foreach (var adjunto in adjuntos)
                    {
                        correo.Attachments.Add(new Attachment(new MemoryStream(adjunto.Contenido), adjunto.Nombre));
                    }
                }

                smtp.Send(correo);
            }
        }
    }
}
