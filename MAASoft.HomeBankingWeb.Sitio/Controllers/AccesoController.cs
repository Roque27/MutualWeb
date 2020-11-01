using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MAASoft.HomeBankingWeb.Sitio.Models;
using MAASoft.HomeBankingWeb.Sitio.ViewModels;
using MAASoft.HomeBankingWeb.Sitio.Repositorios;
using MAASoft.HomeBankingWeb.Sitio.Helpers;
using MAASoft.HomeBankingWeb.Sitio.Core.Mail;
using System.Collections.Generic;

namespace MAASoft.HomeBankingWeb.Sitio.Controllers
{
    [Authorize]
    public class AccesoController : Controller
    {
        private ControllerHelper _controllerHelper;

        public AccesoController()
        {
            _controllerHelper = new ControllerHelper(this);
        }

        #region Properties

        private ApplicationSignInManager _signInManager;
        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? (_signInManager = this.GetSignInManager()); }
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? (_userManager ?? this.GetUserManager()); }
        }

        private UsuariosRepositorio _usuariosRepositorio;
        private UsuariosRepositorio UsuariosRepositorio
        {
            get { return _usuariosRepositorio ?? (_usuariosRepositorio = new UsuariosRepositorio(UserManager, this.GetRoleManager())); }
        }

        #endregion

        #region Actions

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost,
        AllowAnonymous,
        ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    Session.Clear();

                    var user = UserManager.FindByName(model.Email);
                    if (UserManager.IsInRole(user.Id, RolesNombres.SOCIO))
                    {
                        var sociosRepositorio = new SociosRepositorio();
                        SociosHelper.SocioNombreApellidoORazonSocial = sociosRepositorio.Obtener(user.Id).NombreApellidoORazonSocial;
                    }
                    
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    ModelState.AddModelError("", "Su cuenta se encuentra bloqueada. Por favor, pongase en contacto con nosotros.");
                    return View(model);
                case SignInStatus.RequiresVerification:
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Usuario y/o contraseña incorrectos.");
                    return View(model);
            }
        }

        [AllowAnonymous]
        public ActionResult ActivarCuenta(string u, string c)
        {
            ApplicationUser usuario = null;
            if (u == null || c == null
                || (usuario = UserManager.FindById(u)) == null
                || usuario.EmailConfirmed)
            {
                return Redirect("Login");
            }

            var viewModel = new ActivarCuentaViewModel { Email = usuario.Email };
            return View(viewModel);
        }

        [AllowAnonymous,
        HttpPost,
        ValidateAntiForgeryToken]
        public ActionResult ActivarCuenta(string u, string c,
            ActivarCuentaViewModel modelo)
        {
            ApplicationUser usuario = null;
            if (u == null || c == null
                || (usuario = UserManager.FindById(u)) == null
                || usuario.EmailConfirmed)
            {
                return Redirect("Login");
            }

            if (ModelState.IsValid)
            {
                var result = UserManager.ConfirmEmail(u, c);
                if (result.Succeeded)
                {
                    result = UserManager.AddPassword(u, modelo.Password);
                    if (result.Succeeded)
                    {
                        return View("ActivarCuentaConfirmacion");
                    }
                    else
                    {
                        _controllerHelper.CargarErrores(result.Errors);
                    }
                }
                else
                {
                    _controllerHelper.CargarErrores(result.Errors);
                }
            }

            return View(modelo);
        }

        [AllowAnonymous]
        public ActionResult RestablecerSolicitud()
        {
            return View();
        }

        [HttpPost,
        AllowAnonymous,
        ValidateAntiForgeryToken]
        public ActionResult RestablecerSolicitud(RestablecerContraseñaSolicitudViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usuario = UserManager.FindByName(model.Email);
            if (usuario != null)
            {
                var token = UsuariosRepositorio.GenerarTokenRestablecerContraseña(usuario.Id);
                string urlRestablecerContraseña = String.Format("{0}{1}",
                    Request.Url.GetLeftPart(UriPartial.Authority),
                    Url.Action("Restablecer", "Acceso", new { u = usuario.Id, c = token }));

                var correos = new ServidorCorreos();
                correos.EnviarCorreo(usuario.Email, "Restablecer Contraseña", "RestablecerContraseña",
                    campos: new Dictionary<string, string>
                    {
                        { "RestablecerContraseñaURL", urlRestablecerContraseña }
                    });
            }

            return View("RestablecerSolicitudConfirmacion");
        }

        [AllowAnonymous]
        public ActionResult Restablecer(string u, string c)
        {
            ApplicationUser usuario = null;
            if (u == null || c == null
                || (usuario = UserManager.FindById(u)) == null)
            {
                return Redirect("Login");
            }

            var viewModel = new RestablecerContraseñaViewModel { Email = usuario.Email };
            return View(viewModel);
        }

        [HttpPost,
        AllowAnonymous,
        ValidateAntiForgeryToken]
        public ActionResult Restablecer(string u, string c,
            RestablecerContraseñaViewModel modelo)
        {
            ApplicationUser usuario = null;
            if (u == null || c == null
                || (usuario = UserManager.FindById(u)) == null)
            {
                return Redirect("Login");
            }

            if (ModelState.IsValid)
            {
                var result = UsuariosRepositorio.ResetearContraseña(u, c, modelo.Password);
                if (result.Succeeded)
                {
                    return View("RestablecerConfirmacion");
                }
                else
                {
                    _controllerHelper.CargarErrores(result.Errors);
                }
            }

            return View(modelo);
        }

        [HttpPost,
        ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Methods

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion

        #endregion
    }
}