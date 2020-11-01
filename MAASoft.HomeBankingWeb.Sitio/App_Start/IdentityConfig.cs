using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using MAASoft.HomeBankingWeb.Sitio.Models;
using System.Web.Mvc;

namespace MAASoft.HomeBankingWeb.Sitio
{
    public class RolesNombres
    {
        public const string ADMINISTRADOR = "Administrador";
        public const string SOCIO = "Socio";
    }

    public static class IdentityExtensiones
    {
        public static ApplicationUserManager GetUserManager(this Controller controller)
        {
            return controller.Request.GetUserManager();
        }

        public static ApplicationRoleManager GetRoleManager(this Controller controller)
        {
            return controller.Request.GetRolesManager();
        }

        public static ApplicationSignInManager GetSignInManager(this Controller controller)
        {
            return controller.Request.GetSignInManager();
        }

        public static ApplicationUserManager GetUserManager(this HttpRequestBase request)
        {
            return request.GetOwinContext().GetUserManager();
        }

        public static ApplicationRoleManager GetRolesManager(this HttpRequestBase request)
        {
            return request.GetOwinContext().GetRolesManager();
        }

        public static ApplicationSignInManager GetSignInManager(this HttpRequestBase request)
        {
            return request.GetOwinContext().GetSignInManager();
        }

        public static ApplicationUserManager GetUserManager(this IOwinContext owinContext)
        {
            return owinContext.Get<ApplicationUserManager>();
        }

        public static ApplicationRoleManager GetRolesManager(this IOwinContext owinContext)
        {
            return owinContext.Get<ApplicationRoleManager>();
        }

        public static ApplicationSignInManager GetSignInManager(this IOwinContext owinContext)
        {
            return owinContext.Get<ApplicationSignInManager>();
        }
    }

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(15);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }

            return manager;
        }
    }

    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(IRoleStore<IdentityRole, string> store)
            : base(store)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(new RoleStore<IdentityRole>(context.Get<ApplicationDbContext>()));
        }
    }

    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }

    public class UsuariosRepositorio
    {
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public UsuariosRepositorio(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public ApplicationUser ObtenerPorId(string id)
        {
            return _userManager.FindById(id);
        }

        public string ObtenerRolDeUsuario(string usuarioId)
        {
            return _userManager
                .GetRoles(usuarioId)
                .FirstOrDefault();
        }

        public IEnumerable<string> ObtenerEmailsDeUsuariosConRol(string rol)
        {
            var usuariosIds = ObtenerIdsDeUsuariosConRol(rol);

            return _userManager.Users
                .Where(e => usuariosIds.Contains(e.Id))
                .OrderBy(e => e.Email)
                .Select(e => e.Email)
                .ToList();
        }

        private IEnumerable<string> ObtenerIdsDeUsuariosConRol(string rol)
        {
            return _roleManager.FindByName(rol)
                .Users
                .Select(e => e.UserId)
                .ToList();
        }

        public IdentityResult Agregar(string email, out string id)
        {
            var user = new ApplicationUser { UserName = email, Email = email };
            var result = _userManager.Create(user);
            id = user.Id;

            return result;
        }

        public string GenerarTokenActivacionCuenta(string id)
        {
            return _userManager.GenerateEmailConfirmationToken(id);
        }

        public string GenerarTokenRestablecerContraseña(string id)
        {
            return _userManager.GeneratePasswordResetToken(id);
        }

        public IdentityResult ResetearContraseña(string id, string token, string contraseñaNueva)
        {
            return _userManager.ResetPassword(id, token, contraseñaNueva);
        }

        public IdentityResult CambiarContraseña(string id, string contraseñaActual, string contraseñaNueva)
        {
            return _userManager.ChangePassword(id, contraseñaActual, contraseñaNueva);
        }

        public void AsignarRol(string usuarioId, string rol)
        {
            _userManager.AddToRole(usuarioId, rol);
        }

        public IdentityResult Actualizar(ApplicationUser usuario,
            string nuevaContraseña = null)
        {
            var errores = new List<string>();

            var resultadoActualizar = _userManager.Update(usuario);
            if (!resultadoActualizar.Succeeded)
            {
                errores.AddRange(resultadoActualizar.Errors);
            }
            else
            {
                if (!String.IsNullOrWhiteSpace(nuevaContraseña))
                {
                    var resultadoContraseña = _userManager.RemovePassword(usuario.Id);
                    if (resultadoContraseña.Succeeded)
                    {
                        resultadoContraseña = _userManager.AddPassword(usuario.Id, nuevaContraseña);
                        if (!resultadoContraseña.Succeeded)
                        {
                            errores.AddRange(resultadoContraseña.Errors);
                        }
                    }
                    else
                    {
                        errores.Add("No se pudo cambiar la contraseña.");
                    }
                }
            }

            return
                errores.Any()
                ? IdentityResult.Failed(errores.ToArray())
                : IdentityResult.Success;
        }

        public IdentityResult Eliminar(string id)
        {
            var user = _userManager.FindById(id);
            return _userManager.Delete(user);
        }

        public void Bloquear(string usuarioId)
        {
            var resultado = _userManager.SetLockoutEndDate(usuarioId, new DateTimeOffset(new DateTime(2050, 1, 1)));
            if (!resultado.Succeeded)
            {
                throw new Exception(String.Join(Environment.NewLine, resultado.Errors));
            }
        }

        public void Desbloquear(string usuarioId)
        {
            var resultado = _userManager.SetLockoutEndDate(usuarioId, new DateTimeOffset(new DateTime(1950, 1, 1)));
            if (!resultado.Succeeded)
            {
                throw new Exception(String.Join(Environment.NewLine, resultado.Errors));
            }
        }
    }
}
