using CapaEntidad.Administracion;
using CapaEntidad.Tesoreria;
using CapaNegocio.Tesoreria;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Controllers.Tesoreria
{
    public class CajaChicaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult New()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult SolicitudesCorreccion()
        {
            return View();
        }

        public IActionResult EditCorreccion()
        {
            return View();
        }

        public IActionResult Configuracion()
        {
            return View();
        }

        public IActionResult ConfiguracionAdmin()
        {
            return View();
        }

        public IActionResult Movimientos()
        {
            return View();
        }

        public List<ProgramacionSemanalCLS> GetAniosTransacciones()
        {
            CajaChicaBL obj = new CajaChicaBL();
            return obj.GetAniosTransacciones();
        }

        public List<ConfiguracionCajaChicaCLS> GetCajasChicas()
        {
            CajaChicaBL obj = new CajaChicaBL();
            return obj.GetCajasChicas();
        }

        public List<CajaChicaCLS> GetMovimientosEnConfiguracionCajasChicas()
        {
            CajaChicaBL obj = new CajaChicaBL();
            return obj.GetMovimientosEnConfiguracionCajasChicas();
        }

        public List<CajaChicaCLS> GetAllMovimientosEnCajasChicas(int codigoCajaChica, int anioOperacion, int codigoOperacion)
        {
            CajaChicaBL obj = new CajaChicaBL();
            return obj.GetAllMovimientosEnCajasChicas(codigoCajaChica, anioOperacion, codigoOperacion);
        }

        public CajaChicaComboCLS fillCombosNewCajaChica()
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            CajaChicaBL obj = new CajaChicaBL();
            return obj.fillCombosNewCajaChica(objUsuario.IdUsuario, objUsuario.SuperAdmin);
        }

        public CajaChicaComboCLS FillCombosEditCajaChica(int anioOperacion, int semanaOperacion)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            CajaChicaBL obj = new CajaChicaBL();
            return obj.FillCombosEditCajaChica(anioOperacion, semanaOperacion, objUsuario.IdUsuario, objUsuario.SuperAdmin);
        }

        public string GuardarDatos(CajaChicaCLS objTransaccion)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            CajaChicaBL obj = new CajaChicaBL();
            if (!obj.ExisteFactura(objTransaccion.NitProveedor, objTransaccion.SerieFactura, objTransaccion.NumeroDocumento))
            {
                //decimal montoSaldo = obj.GetSaldoActual(objTransaccion.CodigoEmpresa);
                //if (montoSaldo != -1)
                //{
                    //if ((montoSaldo - objTransaccion.Monto) >= 0)
                    //{
                        objTransaccion.FechaDocumento = Util.Conversion.ConvertDateSpanishToEnglish(objTransaccion.FechaDocumentoStr);
                        return obj.GuardarTransaccion(objTransaccion, objUsuario.IdUsuario);
                    //}
                    //else {
                        //return "Monto insuficente en caja Chica";
                    //}
                //}
                //else
                //{
                    //return "Error al obtener el saldo de caja chica";
                //}
            }
            else {
                return "Factura ya ha sido registrado previamente";            
            }
        }

        public decimal GetSaldoActual(int codigoCajaChica)
        {
            CajaChicaBL obj = new CajaChicaBL();
            return obj.GetSaldoActual(codigoCajaChica); 
        }

        public CajaChicaCLS GetDataTransaccion(long codigoTransaccion)
        {
            CajaChicaBL obj = new CajaChicaBL();

            return obj.GetDataTransaccion(codigoTransaccion);
        }

        public string RegistrarSolicitudDeCorreccion(long codigoTransaccion, int codigoTipoCorreccion, string observaciones)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            CajaChicaBL obj = new CajaChicaBL();
            return obj.RegistrarSolicitudDeCorreccion(codigoTransaccion, codigoTipoCorreccion, observaciones, objUsuario.IdUsuario);
        }

        public List<CajaChicaCLS> GetSolicitudesDeCorreccion(int anioOperacion, int semanaOperacion, int codigoReporte)
        {
            CajaChicaBL obj = new CajaChicaBL();
            return obj.GetSolicitudesDeCorreccion(anioOperacion, semanaOperacion, codigoReporte);
        }

        public SolicitudCorreccionCLS GetDataCorreccion(long codigoTransaccion)
        {
            CajaChicaBL obj = new CajaChicaBL();
            return obj.GetDataCorreccion(codigoTransaccion);
        }

        public string AutorizarCorreccion(long codigoTransaccion, string observaciones, int codigoResultado)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            CajaChicaBL obj = new CajaChicaBL();
            return obj.AutorizarCorreccion(codigoTransaccion, observaciones, codigoResultado, objUsuario.IdUsuario);
        }

        public string ActualizarDatos(CajaChicaCLS objTransaccion, int codigoTipoActualizacion)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            CajaChicaBL obj = new CajaChicaBL();
            if (objTransaccion.NitProveedor == objTransaccion.NitProveedorOld && objTransaccion.SerieFactura == objTransaccion.SerieFacturaOld && objTransaccion.NumeroDocumento == objTransaccion.NumeroDocumentoOld)
            {
                // No cambiaron los datos de nit, serie y numero, por lo no es necesario validar si ya existe la factura
                objTransaccion.FechaDocumento = Util.Conversion.ConvertDateSpanishToEnglish(objTransaccion.FechaDocumentoStr);
                return obj.ActualizarTransaccion(objTransaccion, codigoTipoActualizacion, objUsuario.IdUsuario);
            }
            else
            {
                // Algun dato como nit, serie, numero ha cambiaado, por lo que hay que validar si la nueva factura ya se encuentra registrada previamente.
                if (!obj.ExisteFactura(objTransaccion.NitProveedor, objTransaccion.SerieFactura, objTransaccion.NumeroDocumento))
                {
                    objTransaccion.FechaDocumento = Util.Conversion.ConvertDateSpanishToEnglish(objTransaccion.FechaDocumentoStr);
                    return obj.ActualizarTransaccion(objTransaccion, codigoTipoActualizacion, objUsuario.IdUsuario);
                }
                else
                {
                    return "Factura ya ha sido registrado previamente";
                }
            }
        }

        public List<CajaChicaCLS> GetTransaccionesCajaChica(int codigoCajaChica)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            CajaChicaBL obj = new CajaChicaBL();
            return obj.GetTransaccionesCajaChica(codigoCajaChica, objUsuario.IdUsuario, objUsuario.SuperAdmin, objUsuario.SetSemanaAnterior);
        }

        public List<CajaChicaCLS> ListarTransaccionesCajaChicaParaRevision(int codigoReporte)
        {
            CajaChicaBL obj = new CajaChicaBL();
            return obj.ListarTransaccionesCajaChica(codigoReporte);
        }

        public List<CajaChicaCLS> GetTransaccionesCajaChicaConsulta(int codigoReporte, int codigoCajaChica, int anioOperacion, int semanaOperacion)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            CajaChicaBL obj = new CajaChicaBL();
            if (codigoReporte != 0)
            {
                return obj.ListarTransaccionesCajaChica(codigoReporte);
            }
            else {
                return obj.GetTransaccionesCajaChicaConsulta(codigoCajaChica, anioOperacion, semanaOperacion, objUsuario.IdUsuario);
            }
        }

        public string ExcluirFacturaDeCajaChica(long codigoTransaccion, int excluirFactura, int codigoMotivoExclusion, string observaciones)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            CajaChicaBL obj = new CajaChicaBL();
            return obj.ExcluirFacturaDeCajaChica(codigoTransaccion, excluirFactura, codigoMotivoExclusion, objUsuario.IdUsuario, observaciones);
        }

        public string AnularTransaccionCajaChica(long codigoTransaccion, string observaciones)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            CajaChicaBL obj = new CajaChicaBL();
            return obj.AnularTransaccionCajaChica(codigoTransaccion, observaciones, objUsuario.IdUsuario);
        }

        public string AnularMovimientoCajaChica(long codigoTransaccion, string observaciones)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            CajaChicaBL obj = new CajaChicaBL();
            return obj.AnularMovimientoCajaChica(codigoTransaccion, observaciones, objUsuario.IdUsuario);
        }

        public List<MotivoExclusionCLS> GetMotivosExclusionDeFacturas()
        {
            CajaChicaBL obj = new CajaChicaBL();
            return obj.GetMotivosExclusionDeFacturas();
        }

        public string ActualizarTransaccionCajaChica(long codigoTransaccion, int codigoOperacion, string descripcion)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            CajaChicaBL obj = new CajaChicaBL();
            return obj.ActualizarTransaccionCajaChica(codigoTransaccion, codigoOperacion, descripcion, objUsuario.IdUsuario);
        }

        public List<ConfiguracionCajaChicaCLS> GetConfiguracionCajasChicas()
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            CajaChicaBL obj = new CajaChicaBL();
            return obj.GetConfiguracionCajasChicas(objUsuario.IdUsuario, objUsuario.SuperAdmin);
        }

        public string ActualizarMontoDisponibleCajaChica(int codigoCajaChica, int codigoOperacion, int anioOperacion, int semanaOperacion, decimal monto, string observaciones)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            CajaChicaBL obj = new CajaChicaBL();
            return obj.ActualizarMontoDisponibleCajaChica(codigoCajaChica, codigoOperacion, anioOperacion, semanaOperacion, monto, observaciones, objUsuario.IdUsuario);
        }

        public string ActualizarMontosCajaChicaAdmin(int codigoCajaChica, decimal montoLimite, decimal montoDisponible, string observaciones)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            CajaChicaBL obj = new CajaChicaBL();
            return obj.ActualizarMontosCajaChicaAdmin(codigoCajaChica, montoLimite, montoDisponible, observaciones, objUsuario.IdUsuario);
        }

        public List<CajaChicaCLS> GetTransaccionesPorRecepcionarEnTesoreria()
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            CajaChicaBL obj = new CajaChicaBL();
            return obj.GetTransaccionesPorRecepcionarEnTesoreria(objUsuario.IdUsuario, objUsuario.SuperAdmin);

        }

        public string RecepcionarTransaccion(TransaccionCajaChicaCLS objTransaccion, string observaciones)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            CajaChicaBL obj = new CajaChicaBL();
            return obj.RecepcionarTransaccion(objTransaccion, objUsuario.IdUsuario);
        }


    }
}
