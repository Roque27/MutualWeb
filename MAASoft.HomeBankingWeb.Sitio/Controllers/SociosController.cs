using MAASoft.HomeBankingWeb.Sitio.Core.Mail;
using MAASoft.HomeBankingWeb.Sitio.Helpers;
using MAASoft.HomeBankingWeb.Sitio.Models;
using MAASoft.HomeBankingWeb.Sitio.Repositorios;
using MAASoft.HomeBankingWeb.Sitio.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MAASoft.HomeBankingWeb.Sitio.Controllers
{
    [Authorize(Roles = RolesNombres.ADMINISTRADOR)]
    public class SociosController : Controller
    {
        private const int CANT_ITEMS_POR_PAGINA = 20;

        private ControllerHelper _controllerHelper;

        public SociosController()
        {
            _controllerHelper = new ControllerHelper(this);
        }

        #region Properties

        private UsuariosRepositorio _usuariosRepositorio;
        private UsuariosRepositorio UsuariosRepositorio
        {
            get { return _usuariosRepositorio ?? (_usuariosRepositorio = new UsuariosRepositorio(this.GetUserManager(), this.GetRoleManager())); }
        }

        private SociosRepositorio _sociosRepositorio;
        private SociosRepositorio SociosRepositorio
        {
            get { return _sociosRepositorio ?? (_sociosRepositorio = new SociosRepositorio()); }
        }

        #endregion
        
        #region Actions

        public ActionResult Index(
           string nombreApellidoORazonSocial = null,
           string email = null,
           byte? idSucursal = null,
           int p = 1)
        {
            var viewModel = CrearSociosViewModel(nombreApellidoORazonSocial, email, idSucursal, p);
            return View(viewModel);
        }

        [HttpPost,
        ValidateAntiForgeryToken]
        public ActionResult Index(
            string operacion,
            string idSocio,
            string nombreApellidoORazonSocial = null,
            string email = null,
            byte? idSucursal = null,
            int p = 1)
        {
            switch (operacion)
            {
                case "Bloquear":
                    UsuariosRepositorio.Bloquear(idSocio);

                    break;
                case "Desbloquear":
                    UsuariosRepositorio.Desbloquear(idSocio);

                    break;
            }

            var viewModel = CrearSociosViewModel(nombreApellidoORazonSocial, email, idSucursal, p);
            return View(viewModel);
        }

        public ActionResult Agregar()
        {
            var viewModel = new SocioViewModel();
            CargarListasSocioViewModel(viewModel);

            return View(viewModel);
        }

        [HttpPost,
        ValidateAntiForgeryToken]
        public ActionResult Agregar(SocioViewModel model)
        {
            if (ModelState.IsValid)
            {
                string idUsuario;
                var identityResultado = UsuariosRepositorio.Agregar(model.Email, out idUsuario);
                if (identityResultado.Succeeded)
                {
                    var socio = new Socio
                    {
                        Id = idUsuario,
                        NombreApellidoORazonSocial = model.NombreApellidoORazonSocial,
                        Telefono = model.Telefono,
                        NroCuenta = model.NroCuenta,
                        TipoCuenta = model.TipoCuenta,
                        IdSucursal = model.IdSucursal.Value
                    };

                    string tokenActivacion = UsuariosRepositorio.GenerarTokenActivacionCuenta(idUsuario);
                    string urlActivarCuenta = String.Format("{0}{1}", 
                        Request.Url.GetLeftPart(UriPartial.Authority),
                        Url.Action("ActivarCuenta", "Acceso", new { u = socio.Id, c = tokenActivacion }));

                    try
                    {
                        SociosRepositorio.Agregar(socio);
                        UsuariosRepositorio.AsignarRol(idUsuario, RolesNombres.SOCIO);

                        var correos = new ServidorCorreos();
                        correos.EnviarCorreo(model.Email, "Activación de Cuenta", "ActivarCuentaSocio",
                            campos: new Dictionary<string, string>
                            {
                                { "ActivarCuentaURL", urlActivarCuenta }
                            });

                        _controllerHelper.CargarResultadoOk(String.Format("El Socio {0} fue creado correctamente!", model.Email));

                        ModelState.Clear();
                        model = new SocioViewModel();
                    }
                    catch (Exception)
                    {
                        SociosRepositorio.Eliminar(idUsuario);
                        UsuariosRepositorio.Eliminar(idUsuario);

                        throw;
                    }
                }
                else
                {
                    _controllerHelper.CargarErrores(identityResultado.Errors);
                }
            }

            CargarListasSocioViewModel(model);
            return View(model);
        }

        public ActionResult Editar(string id)
        {
            var socio = SociosRepositorio.Obtener(id);
            var usuario = UsuariosRepositorio.ObtenerPorId(id);
            if (socio == null || usuario == null)
            {
                return RedirectToAction("Index");
            }

            var viewModel = new SocioViewModel
            {
                Email = usuario.Email,
                NombreApellidoORazonSocial = socio.NombreApellidoORazonSocial,
                Telefono = socio.Telefono,
                NroCuenta = socio.NroCuenta,
                TipoCuenta = socio.TipoCuenta,
                IdSucursal = socio.IdSucursal
            };
            CargarListasSocioViewModel(viewModel);

            return View(viewModel);
        }

        [HttpPost,
        ValidateAntiForgeryToken]
        public ActionResult Editar(string id, SocioViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var socio = new Socio
                    {
                        Id = id,
                        NombreApellidoORazonSocial = model.NombreApellidoORazonSocial,
                        Telefono = model.Telefono,
                        NroCuenta = model.NroCuenta,
                        TipoCuenta = model.TipoCuenta,
                        IdSucursal = model.IdSucursal.Value
                    };
                    SociosRepositorio.Actualizar(socio);

                    _controllerHelper.CargarResultadoOk("El Socio fue editado correctamente!");
                }
                catch (Exception)
                {
                    _controllerHelper.CargarError("No se pudo editar el Socio.");
                }
            }

            CargarListasSocioViewModel(model);
            return View(model);
        }

        #endregion

        #region Methods

        public SociosViewModel CrearSociosViewModel(
            string nombreApellidoORazonSocial = null,
            string email = null,
            byte? idSucursal = null,
            int p = 1)
        {
            int cantTotalItems = 0;

            var socios = SociosRepositorio.ObtenerTodos(p, CANT_ITEMS_POR_PAGINA,
                out cantTotalItems,
                email, nombreApellidoORazonSocial, idSucursal,
                incluirSucursal: true, incluirUsuario: true)
                .Select(e => new SocioModel
                {
                    Id = e.Id,
                    Email = e.Usuario.Email,
                    NombreApellidoORazonSocial = e.NombreApellidoORazonSocial,
                    SucursalNombre = e.Sucursal.Nombre,
                    LockoutEndDateUtc = e.Usuario.LockoutEndDateUtc
                })
                .ToList();

            var viewModel = new SociosViewModel
            {
                NombreApellidoORazonSocial = nombreApellidoORazonSocial,
                Email = email,
                IdSucursal = idSucursal
            };
            viewModel.CargarItemsYPaginacion(socios, p, cantTotalItems, CANT_ITEMS_POR_PAGINA);
            viewModel.SucursalesSelectList = ListasHelper.CrearSucursalesSelectList();

            return viewModel;
        }

        public void CargarListasSocioViewModel(SocioViewModel viewModel)
        {
            viewModel.SucursalesSelectList = ListasHelper.CrearSucursalesSelectList();
            viewModel.TiposCuentaSelectList = ListasHelper.CrearTiposCuentaSelectList();
        }

        #endregion
    }
}