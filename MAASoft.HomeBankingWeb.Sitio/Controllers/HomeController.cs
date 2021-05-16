using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;

using MAASoft.HomeBankingWeb.Sitio.Core;
using MAASoft.HomeBankingWeb.Sitio.Core.Tipos;
using MAASoft.HomeBankingWeb.Sitio.Helpers;
using MAASoft.HomeBankingWeb.Sitio.Filters;
using MAASoft.HomeBankingWeb.Sitio.Models;
using MAASoft.HomeBankingWeb.Sitio.Repositorios;
using MAASoft.HomeBankingWeb.Sitio.Servicios;
using MAASoft.HomeBankingWeb.Sitio.ViewModels;
using MAASoft.HomeBankingWeb.Sitio.Configuracion;
using System.Web;
using System.IO;

namespace MAASoft.HomeBankingWeb.Sitio.Controllers
{
    public class HomeController : Controller
    {
        #region Propiedades

        private const int CANT_ITEMS_POR_PAGINA = 20;
        private int CantMaxMesesDetalleCajaAhorros
        {
            get { return int.Parse(ConfigurationManager.AppSettings["CantMaxMesesDetalleCajaAhorros"]); }
        }

        #endregion

        #region Servicios

        private ControllerHelper _controllerHelper;
        private ControllerHelper ControllerHelper
        {
            get { return _controllerHelper ?? (_controllerHelper = new ControllerHelper(this)); }
        }

        private SociosRepositorio _sociosRepositorio;
        private SociosRepositorio SociosRepositorio
        {
            get { return _sociosRepositorio ?? (_sociosRepositorio = new SociosRepositorio()); }
        }

        private UsuariosRepositorio _usuariosRepositorio;
        private UsuariosRepositorio UsuariosRepositorio
        {
            get { return _usuariosRepositorio ?? (_usuariosRepositorio = new UsuariosRepositorio(this.GetUserManager(), this.GetRoleManager())); }
        }

        private SocioWeb _socio;
        private SocioWeb Socio
        {
            get
            {
                return
                    this.User.Identity.IsAuthenticated
                    ? _socio ?? (_socio = SociosRepositorio.Obtener(this.User.Identity.GetUserId(), incluirSucursal: true))
                    : null;
            }
        }

        private ServiciosCliente _servicioCliente;
        private ServiciosCliente ServicioCliente
        {
            get { return _servicioCliente ?? (_servicioCliente = new ServiciosCliente(Socio.Sucursal)); }
        }

        private ServiciosSocios _servicioSocios;
        private ServiciosSocios ServicioSocio
        {
            get { return _servicioSocios ?? (_servicioSocios = new ServiciosSocios(Socio.Sucursal)); }
        }

        #endregion

        #region Actions

        public ActionResult Index()
        {
            if (User.IsInRole(RolesNombres.SOCIO))
            {
                return RedirectToAction("Resumen");
            }
            else if (User.IsInRole(RolesNombres.ADMINISTRADOR))
            {
                return RedirectToAction("Index", "Socios");
            }
            else
            {
                return RedirectToAction("Login", "Acceso");
            }
        }

        [Authorize(Roles = RolesNombres.SOCIO),
        ValidarSocio]
        public ActionResult MiCuenta()
        {
            var modelo = new MiCuentaViewModel
            {
                Email = User.Identity.GetUserName(),
                NombreApellidoORazonSocial = Socio.NombreApellidoORazonSocial,
                Telefono = Socio.Telefono,
                DatosCompletos = false,
            };

            Socio _socio = ServicioSocio.ObtenerDatosDelSocio(Socio.NroCuenta);
            if (_socio != null)
            {
                modelo.NumeroSocio = _socio.Codigo.ToString();
                modelo.Telefono = _socio.Telefono;
                modelo.TipoDocumento = _socio.TipoDocumento;
                modelo.NumeroDocumento = _socio.NroDocumento.ToString();
                modelo.Domicilio = _socio.Domicilio;
                modelo.Localidad = _socio.Localidad;
                modelo.CodPostal = _socio.CodPostal;
                modelo.Telefono = (_socio.Telefono.Trim().Equals(String.Empty) ? modelo.Telefono : _socio.Telefono);
                modelo.Celular = _socio.Celular;
            }

            return View(modelo);
        }

        [HttpPost,
        ValidateAntiForgeryToken,
        Authorize(Roles = RolesNombres.SOCIO),
        ValidarSocio]
        public ActionResult MiCuenta(MiCuentaViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                bool hayError = false;
                if (modelo.SeDebeActualizarContraseña)
                {
                    var resultado = UsuariosRepositorio.CambiarContraseña(Socio.Id, modelo.ContraseñaActual, modelo.ContraseñaNueva);
                    if (!resultado.Succeeded)
                    {
                        ControllerHelper.CargarErrores(resultado.Errors);
                        hayError = true;
                    }
                }

                if (!hayError)
                {
                    // En la web
                    var socio = Socio;
                    socio.NombreApellidoORazonSocial = modelo.NombreApellidoORazonSocial;
                    socio.Telefono = modelo.Telefono;

                    SociosRepositorio.Actualizar(socio);

                    SociosHelper.SocioNombreApellidoORazonSocial = socio.NombreApellidoORazonSocial;

                    // En Sucursal
                    var _socio = new Socio()
                    {
                        Codigo = Convert.ToInt32(modelo.NumeroSocio),
                        Nombre = modelo.NombreApellidoORazonSocial,
                        Domicilio = modelo.Domicilio,
                        Localidad = modelo.Localidad,
                        CodPostal = modelo.CodPostal,
                        Telefono = modelo.Telefono,
                        Fax = modelo.Fax,
                        Celular = modelo.Celular,
                        Email = modelo.Email,
                        TipoDocumento = modelo.TipoDocumento,
                        NroDocumento = Convert.ToInt64(modelo.NumeroDocumento),
                    };
                    var respuesta = ServicioSocio.ActualizarDatosDelSocio(_socio);

                    ControllerHelper.CargarResultadoOk("Sus datos" + (modelo.SeDebeActualizarContraseña ? " y su contraseña" : String.Empty) + " fueron actualizados correctamente!");
                }
            }

            return View(modelo);
        }

        [Authorize(Roles = RolesNombres.SOCIO),
        ValidarSocio]
        public ActionResult Resumen(
            int p = 1,
            bool exp = false,
            string f = null)
        {
            List<SaldoCajaAhorro> saldos = new List<SaldoCajaAhorro>();

            try
            {
                //saldo = ServicioCliente.ObtenerSaldoCajaDeAhorro(Socio.NroCuenta, Socio.TipoCuentaAbreviado);
                saldos = ServicioCliente.ObtenerSaldosCajaDeAhorro(Socio.NroCuenta).ToList();
            }
            catch (Exception)
            {
                // NOTA: si no pude obtener el saldo asumimos que el socio no posee una Caja de ahorros
            }

            if (saldos != null && saldos.Count > 0)
            {
                saldos.ForEach(s => s.TipoDesc = String.Format("{0} $ ({1})", Configuracion.Configuracion.CajaAhorrosTitulo, ServiciosTiposCuentaCodigos.ObtenerNombreDesdeCodigo(s.Tipo), s.TipoAMV = "AMV" + s.Tipo));

                if (exp)
                {
                    switch (f)
                    {
                        case Formato.ARCHIVO_FORMATO_PDF:
                            return ReportesPdfHelper.GenerarReporteResumenPdfFileResult(saldos);

                        case Formato.ARCHIVO_FORMATO_EXCEL:
                        default:
                            return ReportesExcelHelper.GenerarActionResultExcelResumen(saldos);
                    }
                }
            }

            return View(saldos);
        }

        [Authorize(Roles = RolesNombres.SOCIO),
        ValidarSocio,
        ValidarModuloHabilitado(ModuloNombres.CAJA_AHORROS)]
        public ActionResult CajaDeAhorros(
            string desde = null,
            string hasta = null,
            string tipoComprobante = null,
            int p = 1,
            bool exp = false,
            string f = null)
        {
            var rangoFechas = new RangoFechas(desde, hasta,
                cantMaxMeses: CantMaxMesesDetalleCajaAhorros);

            var viewModel = new DetalleCajaDeAhorrosViewModel
            {
                Desde = rangoFechas.Desde,
                Hasta = rangoFechas.Hasta,
                TipoComprobante = tipoComprobante ?? Socio.TipoCuenta,
                TiposComprobanteSelectList = ListasHelper.CrearTiposCuentaSelectList()
            };

            if (ValidarRangoFechas(rangoFechas))
            {
                CargarItemsCajaDeAhorrosViewModel(viewModel, p, exp);
            }

            if (exp)
            {
                switch (f)
                {
                    case Formato.ARCHIVO_FORMATO_PDF:
                        return ReportesPdfHelper.GenerarReporteCajaDeAhorrosPdfFileResult(viewModel.Items);

                    case Formato.ARCHIVO_FORMATO_EXCEL:
                    default:
                        return ReportesExcelHelper.GenerarActionResultExcelCajaDeAhorros(viewModel.Items);
                }
            }
            else
            {
                return View(viewModel);
            }
        }

        [Authorize(Roles = RolesNombres.SOCIO),
        ValidarSocio,
        ValidarModuloHabilitado(ModuloNombres.AHORRO_A_TERMINO_PESOS)]
        public ActionResult AhorroATerminoEnPesos(
            string desde = null,
            string hasta = null,
            int p = 1,
            bool exp = false,
            string f = null)
        {
            var rangoFechas = new RangoFechas(desde, hasta);

            var viewModel = new DetalleViewModelBase<AhorroTerminoVigente>
            {
                Desde = rangoFechas.Desde,
                Hasta = rangoFechas.Hasta,
            };

            if (ValidarRangoFechas(rangoFechas))
            {
                CargarItemsAhorroATerminoEnPesosViewModel(viewModel, p, exp);
            }

            if (exp)
            {
                switch (f)
                {
                    case Formato.ARCHIVO_FORMATO_PDF:
                        return ReportesPdfHelper.GenerarReporteAhorrosATerminoEnPesosPdfFileResult(viewModel.Items);

                    case Formato.ARCHIVO_FORMATO_EXCEL:
                    default:
                        return ReportesExcelHelper.GenerarActionResultExcelAhorroATerminoEnPesos(viewModel.Items);
                }
            }
            else
            {
                return View(viewModel);
            }
        }

        [Authorize(Roles = RolesNombres.SOCIO),
        ValidarSocio,
        ValidarModuloHabilitado(ModuloNombres.AHORRO_A_TERMINO_DOLARES)]
        public ActionResult AhorroATerminoEnDolares(
            string desde = null,
            string hasta = null,
            int p = 1,
            bool exp = false,
            string f = null)
        {
            var rangoFechas = new RangoFechas(desde, hasta);

            var viewModel = new DetalleViewModelBase<AhorroTerminoVigente>
            {
                Desde = rangoFechas.Desde,
                Hasta = rangoFechas.Hasta,
            };

            if (ValidarRangoFechas(rangoFechas))
            {
                CargarItemsAhorroATerminoEnDolaresViewModel(viewModel, p, exp);
            }

            if (exp)
            {
                switch (f)
                {
                    case Formato.ARCHIVO_FORMATO_PDF:
                        return ReportesPdfHelper.GenerarReporteAhorrosATerminoEnDolaresPdfFileResult(viewModel.Items);

                    case Formato.ARCHIVO_FORMATO_EXCEL:
                    default:
                        return ReportesExcelHelper.GenerarActionResultExcelAhorroATerminoEnDolares(viewModel.Items);
                }
            }
            else
            {
                return View(viewModel);
            }
        }

        [Authorize(Roles = RolesNombres.SOCIO),
        ValidarSocio,
        ValidarModuloHabilitado(ModuloNombres.CUOTAS_SOCIETARIAS)]
        public ActionResult CuotasSocietarias(
            string desde = null,
            string hasta = null,
            int p = 1,
            bool exp = false,
            string f = null)
        {
            var rangoFechas = new RangoFechas(desde, hasta,
                cantMesesDefault: 12);

            var viewModel = new DetalleViewModelBase<CuotaSocietaria>
            {
                Desde = rangoFechas.Desde,
                Hasta = rangoFechas.Hasta,
            };

            if (ValidarRangoFechas(rangoFechas))
            {
                CargarItemsCuotasSocietariasViewModel(viewModel, p, exp);
            }

            if (exp)
            {
                switch (f)
                {
                    case Formato.ARCHIVO_FORMATO_PDF:
                        return ReportesPdfHelper.GenerarReporteCuotasSocietariasPdfFileResult(viewModel.Items);

                    case Formato.ARCHIVO_FORMATO_EXCEL:
                    default:
                        return ReportesExcelHelper.GenerarActionResultExcelCuotasSocietarias(viewModel.Items);
                }
            }
            else
            {
                return View(viewModel);
            }
        }

        [Authorize(Roles = RolesNombres.SOCIO),
        ValidarSocio,
        ValidarModuloHabilitado(ModuloNombres.AYUDAS_ECONOMICAS)]
        public ActionResult AyudasEconomicas(
            string desde = null,
            string hasta = null,
            int p = 1,
            bool exp = false,
            string f = null)
        {
            var rangoFechas = new RangoFechas(desde, hasta);

            var viewModel = new DetalleViewModelBase<AyudaEconomicaVigente>
            {
                Desde = rangoFechas.Desde,
                Hasta = rangoFechas.Hasta,
            };

            if (ValidarRangoFechas(rangoFechas))
            {
                CargarItemsAyudasEconomicasViewModel(viewModel, p, exp);
            }

            if (exp)
            {
                switch (f)
                {
                    case Formato.ARCHIVO_FORMATO_PDF:
                        return ReportesPdfHelper.GenerarReporteAyudasEconomicasPdfFileResult(viewModel.Items);

                    case Formato.ARCHIVO_FORMATO_EXCEL:
                    default:
                        return ReportesExcelHelper.GenerarActionResultExcelAyudasEconomicas(viewModel.Items);
                }
            }
            else
            {
                return View(viewModel);
            }
        }

        [Authorize(Roles = RolesNombres.SOCIO),
        ValidarSocio,
        ValidarModuloHabilitado(ModuloNombres.AYUDAS_ECONOMICAS)]
        public ActionResult DetalleAyudaEconomicaCuotas(
            int id,
            bool exp = false,
            string f = null)
        {
            var items = ServicioCliente.ObtenerDetalleAyudaEconomicaCuotas(id).ToList();

            if (exp)
            {
                switch (f)
                {
                    case Formato.ARCHIVO_FORMATO_PDF:
                        return ReportesPdfHelper.GenerarReporteDetalleAyudaEconomicaCuotasPdfFileResult(items);

                    case Formato.ARCHIVO_FORMATO_EXCEL:
                    default:
                        return ReportesExcelHelper.GenerarActionResultExcelDetalleAyudaEconomicaCuotas(items);
                }
            }
            else
            {
                return View(items);
            }
        }

        [Authorize(Roles = RolesNombres.SOCIO),
        ValidarSocio,
        ValidarModuloHabilitado(ModuloNombres.AYUDAS_ECONOMICAS)]
        public ActionResult DetalleAyudaEconomicaCheques(
            int id,
            bool exp = false,
            string f = null)
        {
            var items = ServicioCliente.ObtenerDetalleAyudaEconomicaCheques(id).ToList();

            if (exp)
            {
                switch (f)
                {
                    case Formato.ARCHIVO_FORMATO_PDF:
                        return ReportesPdfHelper.GenerarReporteDetalleAyudaEconomicaChequesPdfFileResult(items);

                    case Formato.ARCHIVO_FORMATO_EXCEL:
                    default:
                        return ReportesExcelHelper.GenerarActionResultExcelDetalleAyudaEconomicaCheques(items);
                }
            }
            else
            {
                return View(items);
            }
        }


        [Authorize(Roles = RolesNombres.SOCIO),
        ValidarSocio,
        ValidarModuloHabilitado(ModuloNombres.AYUDAS_ECONOMICAS)]
        public ActionResult DetalleAyudaEconomicaDocumentos(
            int id,
            bool exp = false,
            string f = null)
        {
            var items = ServicioCliente.ObtenerDetalleAyudaEconomicaDocumentos(id).ToList();

            if (exp)
            {
                switch (f)
                {
                    case Formato.ARCHIVO_FORMATO_PDF:
                        return ReportesPdfHelper.GenerarReporteDetalleAyudaEconomicaDocumentosPdfFileResult(items);

                    case Formato.ARCHIVO_FORMATO_EXCEL:
                    default:
                        return ReportesExcelHelper.GenerarActionResultExcelDetalleAyudaEconomicaDocumentos(items);
                }
            }
            else
            {
                return View(items);
            }
        }

        [Authorize(Roles = RolesNombres.SOCIO),
        ValidarSocio,
        ValidarModuloHabilitado(ModuloNombres.IMPUESTOS)]
        public ActionResult Impuestos(
            string desde = null,
            string hasta = null,
            int p = 1,
            bool exp = false,
            string f = null)
        {
            var rangoFechas = new RangoFechas(desde, hasta);

            var viewModel = new DetalleViewModelBase<ImpuestoPendiente>
            {
                Desde = rangoFechas.Desde,
                Hasta = rangoFechas.Hasta,
            };

            if (ValidarRangoFechas(rangoFechas))
            {
                CargarItemsImpuestosViewModel(viewModel, p, exp);
            }

            if (exp)
            {
                switch (f)
                {
                    case Formato.ARCHIVO_FORMATO_PDF:
                        return ReportesPdfHelper.GenerarReporteImpuestosPdfFileResult(viewModel.Items);

                    case Formato.ARCHIVO_FORMATO_EXCEL:
                    default:
                        return ReportesExcelHelper.GenerarActionResultExcelImpuestos(viewModel.Items);
                }
            }
            else
            {
                return View(viewModel);
            }
        }

        [Authorize(Roles = RolesNombres.SOCIO),
        ValidarSocio,
        ValidarModuloHabilitado(ModuloNombres.VALORES_AL_COBRO)]
        public ActionResult ValoresAlCobro(
            string desde = null,
            string hasta = null,
            int p = 1,
            bool exp = false,
            string f = null)
        {
            var rangoFechas = new RangoFechas(desde, hasta);

            var viewModel = new DetalleViewModelBase<DetalleValorCobroAcreditacion>
            {
                Desde = rangoFechas.Desde,
                Hasta = rangoFechas.Hasta,
            };

            if (ValidarRangoFechas(rangoFechas))
            {
                CargarItemsValoresAlCobroViewModel(viewModel, p, exp);
            }

            if (exp)
            {
                switch (f)
                {
                    case Formato.ARCHIVO_FORMATO_PDF:
                        return ReportesPdfHelper.GenerarReporteValoresAlCobroPdfFileResult(viewModel.Items);

                    case Formato.ARCHIVO_FORMATO_EXCEL:
                    default:
                        return ReportesExcelHelper.GenerarActionResultExcelValoresAlCobro(viewModel.Items);
                }
            }
            else
            {
                return View(viewModel);
            }
        }

        [Authorize(Roles = RolesNombres.SOCIO),
        ValidarSocio,
        ValidarModuloHabilitado(ModuloNombres.VALORES_NEGOCIADOS)]
        public ActionResult ValoresNegociados(
            string desde = null,
            string hasta = null,
            int p = 1,
            bool exp = false,
            string f = null)
        {
            var rangoFechas = new RangoFechas(desde, hasta);

            var viewModel = new DetalleViewModelBase<DetalleValorNegociadoAyuda>
            {
                Desde = rangoFechas.Desde,
                Hasta = rangoFechas.Hasta,
            };

            if (ValidarRangoFechas(rangoFechas))
            {
                CargarItemsValoresNegociadosViewModel(viewModel, p, exp);
            }

            if (exp)
            {
                switch (f)
                {
                    case Formato.ARCHIVO_FORMATO_PDF:
                        return ReportesPdfHelper.GenerarReporteValoresNegociadosPdfFileResult(viewModel.Items);

                    case Formato.ARCHIVO_FORMATO_EXCEL:
                    default:
                        return ReportesExcelHelper.GenerarActionResultExcelValoresNegociados(viewModel.Items);
                }
            }
            else
            {
                return View(viewModel);
            }
        }

        [Authorize(Roles = RolesNombres.SOCIO),
        ValidarSocio,
        ValidarModuloHabilitado(ModuloNombres.SERVICIOS_CUOTAS)]
        public ActionResult ServiciosCuotas(
            string desde = null,
            string hasta = null,
            int p = 1,
            bool exp = false,
            string f = null)
        {
            var rangoFechas = new RangoFechas(desde, hasta);

            var viewModel = new ServiciosCuotasViewModel
            {
                Desde = rangoFechas.Desde,
                Hasta = rangoFechas.Hasta,
            };

            if (ValidarRangoFechas(rangoFechas))
            {
                CargarServiciosCuotasViewModel(viewModel, p, exp);
            }

            if (exp)
            {
                switch (f)
                {
                    case Formato.ARCHIVO_FORMATO_PDF:
                        return ReportesPdfHelper.GenerarReporteServiciosCuotasPdfFileResult(viewModel.Items,
                            viewModel.CuotasPendientes, viewModel.CuotasImpagas, viewModel.CuotasPagas, viewModel.TotalPagado);

                    case Formato.ARCHIVO_FORMATO_EXCEL:
                    default:
                        return ReportesExcelHelper.GenerarActionResultExcelServiciosCuotas(viewModel.Items,
                            viewModel.CuotasPendientes, viewModel.CuotasImpagas, viewModel.CuotasPagas, viewModel.TotalPagado);
                }
            }
            else
            {
                return View(viewModel);
            }
        }

        [Authorize(Roles = RolesNombres.SOCIO),
        ValidarSocio,
        ValidarModuloHabilitado(ModuloNombres.CARGA_TRAMITES)]
        public ActionResult CargaDeTramites()
        {
            var viewModel = new CargaDeTramitesViewModel
            {
                UserName = User.Identity.Name,
                Archivos = FilesHelper.ObtenerTramitesSubidos(User.Identity.Name)
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult CargaDeTramites(IEnumerable<HttpPostedFileBase> files)
        {
            int count = 0;
            foreach (var file in files)
            {
                try
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        string path = Server.MapPath("~/DocumentosSubidos/" + User.Identity.Name);
                        string pathFile = Path.Combine(path, Path.GetFileName(file.FileName));
                        file.SaveAs(pathFile);
                        count++;
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "El archivo número " + (count + 1) + " presenta un error y no se pudo subir.";
                    //ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    break;
                }
            }

            if (count == 3)
                ViewBag.Message = "Arhivos subidos exitosamente.";
            else if (count == 0)
                ViewBag.Message = "No ha especificado ningun archivo.";

            var viewModel = new CargaDeTramitesViewModel
            {
                UserName = User.Identity.Name,
                Archivos = FilesHelper.ObtenerTramitesSubidos(User.Identity.Name)
            };

            return View(viewModel);
        }

        [Authorize(Roles = RolesNombres.SOCIO),
        ValidarSocio,
        ValidarModuloHabilitado(ModuloNombres.TRANSFERENCIAS)]
        public ActionResult Transferencias()
        {
            var viewModel = new TransferenciaViewModel
            {
            };

            return View();
        }

        #endregion

        #region Methods

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _servicioCliente = null;
            }

            base.Dispose(disposing);
        }

        private bool ValidarRangoFechas(RangoFechas rangoFechas)
        {
            string mensajeError = null;
            if (rangoFechas == null)
            {
                mensajeError = "Debe especificar el rango de fechas";
            }
            else
            {
                rangoFechas.EsValido(out mensajeError);
            }

            if (!String.IsNullOrWhiteSpace(mensajeError))
            {
                if (ViewBag.Errores == null)
                {
                    ViewBag.Errores = new string[] { mensajeError };
                }
                else
                {
                    var errores = new List<string>((IEnumerable<string>)ViewBag.Errores);
                    errores.Add(mensajeError);

                    ViewBag.Errores = errores;
                }

                return false;
            }

            return true;
        }

        private void CargarItemsCajaDeAhorrosViewModel(
            DetalleCajaDeAhorrosViewModel viewModel,
            int p = 1,
            bool exp = false)
        {
            var items = ServicioCliente.ObtenerDetalleCajaDeAhorro(Socio.NroCuenta, viewModel.TipoComprobante, viewModel.Desde, viewModel.Hasta.Value);

            if (exp)
            {
                viewModel.Items = items.ToList();
            }
            else
            {
                viewModel.CargarItemsYPaginacionDesdeListaCompletaDeItems(items.ToList(), p, CANT_ITEMS_POR_PAGINA);
            }
        }

        private void CargarItemsAhorroATerminoEnPesosViewModel(
            DetalleViewModelBase<AhorroTerminoVigente> viewModel,
            int p = 1,
            bool exp = false)
        {
            var items = ServicioCliente.ObtenerAhorroATerminoEnPesos(Socio.NroCuenta, viewModel.Desde, viewModel.Hasta.Value);

            if (exp)
            {
                viewModel.Items = items.ToList();
            }
            else
            {
                viewModel.CargarItemsYPaginacionDesdeListaCompletaDeItems(items.ToList(), p, CANT_ITEMS_POR_PAGINA);
            }
        }

        private void CargarItemsAhorroATerminoEnDolaresViewModel(
            DetalleViewModelBase<AhorroTerminoVigente> viewModel,
            int p = 1,
            bool exp = false)
        {
            var items = ServicioCliente.ObtenerAhorroATerminoEnDolares(Socio.NroCuenta, viewModel.Desde, viewModel.Hasta.Value);

            if (exp)
            {
                viewModel.Items = items.ToList();
            }
            else
            {
                viewModel.CargarItemsYPaginacionDesdeListaCompletaDeItems(items.ToList(), p, CANT_ITEMS_POR_PAGINA);
            }
        }

        private void CargarItemsCuotasSocietariasViewModel(
            DetalleViewModelBase<CuotaSocietaria> viewModel,
            int p = 1,
            bool exp = false)
        {
            var items = ServicioCliente.ObtenerCuotasSocietarias(Socio.NroCuenta, viewModel.Desde, viewModel.Hasta.Value);

            if (exp)
            {
                viewModel.Items = items.ToList();
            }
            else
            {
                viewModel.CargarItemsYPaginacionDesdeListaCompletaDeItems(items.ToList(), p, CANT_ITEMS_POR_PAGINA);
            }
        }

        private void CargarItemsAyudasEconomicasViewModel(
            DetalleViewModelBase<AyudaEconomicaVigente> viewModel,
            int p = 1,
            bool exp = false)
        {
            var items = ServicioCliente.ObtenerAyudasEconomicas(Socio.NroCuenta, viewModel.Desde, viewModel.Hasta.Value);

            if (exp)
            {
                viewModel.Items = items.ToList();
            }
            else
            {
                viewModel.CargarItemsYPaginacionDesdeListaCompletaDeItems(items.ToList(), p, CANT_ITEMS_POR_PAGINA);
            }
        }

        private void CargarItemsImpuestosViewModel(
            DetalleViewModelBase<ImpuestoPendiente> viewModel,
            int p = 1,
            bool exp = false)
        {
            var items = ServicioCliente.ObtenerImpuestos(Socio.NroCuenta);

            if (viewModel.Desde.HasValue)
            {
                items = items.Where(e => e.FechaVto.Date >= viewModel.Desde.Value.Date);
            }

            if (viewModel.Hasta.HasValue)
            {
                items = items.Where(e => e.FechaVto.Date <= viewModel.Hasta.Value.Date);
            }

            if (exp)
            {
                viewModel.Items = items.ToList();
            }
            else
            {
                viewModel.CargarItemsYPaginacionDesdeListaCompletaDeItems(items.ToList(), p, CANT_ITEMS_POR_PAGINA);
            }
        }

        private void CargarItemsValoresAlCobroViewModel(
            DetalleViewModelBase<DetalleValorCobroAcreditacion> viewModel,
            int p = 1,
            bool exp = false)
        {
            var items = ServicioCliente.ObtenerValoresAlCobro(Socio.NroCuenta);

            if (viewModel.Desde.HasValue)
            {
                items = items.Where(e => e.FecDep.Date >= viewModel.Desde.Value.Date);
            }

            if (viewModel.Hasta.HasValue)
            {
                items = items.Where(e => e.FecDep.Date <= viewModel.Hasta.Value.Date);
            }

            if (exp)
            {
                viewModel.Items = items.ToList();
            }
            else
            {
                viewModel.CargarItemsYPaginacionDesdeListaCompletaDeItems(items.ToList(), p, CANT_ITEMS_POR_PAGINA);
            }
        }

        private void CargarItemsValoresNegociadosViewModel(
            DetalleViewModelBase<DetalleValorNegociadoAyuda> viewModel,
            int p = 1,
            bool exp = false)
        {
            var items = ServicioCliente.ObtenerValoresNegociados(Socio.NroCuenta);

            if (viewModel.Desde.HasValue)
            {
                items = items.Where(e => e.FecDep.Date >= viewModel.Desde.Value.Date);
            }

            if (viewModel.Hasta.HasValue)
            {
                items = items.Where(e => e.FecDep.Date <= viewModel.Hasta.Value.Date);
            }

            if (exp)
            {
                viewModel.Items = items.ToList();
            }
            else
            {
                viewModel.CargarItemsYPaginacionDesdeListaCompletaDeItems(items.ToList(), p, CANT_ITEMS_POR_PAGINA);
            }
        }

        private void CargarServiciosCuotasViewModel(
            ServiciosCuotasViewModel viewModel,
            int p = 1,
            bool exp = false)
        {
            var items = ServicioCliente.ObtenerServiciosCuotas(Socio.NroCuenta);

            viewModel.CuotasPendientes = viewModel.CuotasImpagas = viewModel.CuotasPagas = 0;
            viewModel.TotalPagado = 0;

            if (items.Any())
            {
                foreach (var item in items)
                {
                    if (item.FechaPago.HasValue)
                    {
                        viewModel.CuotasPagas++;
                        viewModel.TotalPagado += item.Total;
                    }
                    else
                    {
                        viewModel.CuotasImpagas++;
                    }
                }

                var ultimaCuota = items.Last();
                viewModel.CuotasPendientes = ultimaCuota.Plan - ultimaCuota.Cuota;
            }

            if (viewModel.Desde.HasValue)
            {
                items = items.Where(e => e.Fecha.Date >= viewModel.Desde.Value.Date);
            }

            if (viewModel.Hasta.HasValue)
            {
                items = items.Where(e => e.Fecha.Date <= viewModel.Hasta.Value.Date);
            }

            if (exp)
            {
                viewModel.Items = items.ToList();
            }
            else
            {
                viewModel.CargarItemsYPaginacionDesdeListaCompletaDeItems(items.ToList(), p, CANT_ITEMS_POR_PAGINA);
            }
        }

        public FileResult Descargar(string FileName)
        {
            var FileVirtualPath = "~/DocumentosSubidos/" + User.Identity.Name + "/" + FileName;
            return File(FileVirtualPath, "application/force- download", Path.GetFileName(FileVirtualPath));
        }

        public ActionResult Borrar(string FileName)
        {
            FilesHelper.BorrarTramiteSubido(User.Identity.Name, FileName);
            return RedirectToAction("CargaDeTramites", "Home");
        }

        #endregion
    }
}