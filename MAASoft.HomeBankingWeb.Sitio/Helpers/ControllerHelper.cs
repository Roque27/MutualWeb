using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MAASoft.HomeBankingWeb.Sitio.Helpers
{
    public class ControllerHelper
    {
        private Controller _controller;

        public ControllerHelper(Controller controller)
        {
            if (controller == null)
            {
                throw new ArgumentNullException("controller");
            }

            _controller = controller;
        }

        public void CargarError(string mensaje)
        {
            CargarErrores(new string[] { mensaje });
        }

        public void CargarErrores(IEnumerable<string> mensajes)
        {
            _controller.ViewBag.Errores = mensajes;
        }

        public void CargarResultadoOk(string mensaje)
        {
            _controller.ViewBag.ResultadoOk = mensaje;
        }
    }
}