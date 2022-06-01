using CapaEntidad.Administracion;
using CapaEntidad.Contabilidad;
using CapaNegocio.Contabilidad;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Controllers.Contabilidad
{
    public class CuentasPorCobrarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult New()
        {
            return View();
        }

        public IActionResult NewCargaInicial()
        {
            return View();
        }

        public IActionResult MostrarCuentasPorCobrarCargaInicial()
        {
            return View();
        }

        public IActionResult RegistroCxC()
        {
            return View();
        }

        public string CargarSaldosIniciales(int anioOperacion, int semanaOperacion)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            CuentaPorCobrarBL obj = new CuentaPorCobrarBL();
            return obj.CargarSaldosIniciales(anioOperacion, semanaOperacion, objUsuario.IdUsuario);
        }

        public string CargarCxCTemporal()
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            CuentaPorCobrarBL obj = new CuentaPorCobrarBL();
            return obj.CargarCxCTemporal(objUsuario.IdUsuario);
        }

        public string GuardarCuentaPorCobrar(CuentaPorCobrarCLS objCuentaPorCobrar, int cargaInicial)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            CuentaPorCobrarBL obj = new CuentaPorCobrarBL();
            return obj.GuardarCuentaPorCobrar(objCuentaPorCobrar, objUsuario.IdUsuario, cargaInicial);
        }

        public List<TipoCuentaPorCobrarCLS> GetListTiposCuentasPorCobrar()
        {
            CuentaPorCobrarBL obj = new CuentaPorCobrarBL();
            return obj.GetListTiposCuentasPorCobrar();
        }

        public List<CuentaPorCobrarCLS> GetCuentasPorCobrarCargaInicial()
        {
            CuentaPorCobrarBL obj = new CuentaPorCobrarBL();
            return obj.GetCuentasPorCobrarCargaInicial();
        }

        public List<CuentaPorCobrarCLS> GetCuentasPorCobrarTemporal()
        {
            CuentaPorCobrarBL obj = new CuentaPorCobrarBL();
            return obj.GetCuentasPorCobrarTemporal();
        }

        public string ActualizarCuentaPorCobrarTemporal(CuentaPorCobrarCLS objCuentaPorCobrar)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            CuentaPorCobrarBL obj = new CuentaPorCobrarBL();
            return obj.ActualizarCuentaPorCobrarTemporal(objCuentaPorCobrar, objUsuario.IdUsuario);
        }

        public string AnularCuentaPorCobrarTemporal(long codigoCuentaPorCobrar)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            CuentaPorCobrarBL obj = new CuentaPorCobrarBL();
            return obj.AnularCuentaPorCobrarTemporal(codigoCuentaPorCobrar, objUsuario.IdUsuario);
        }

        public decimal GetMontoCuentaPorCobrar(int codigoTipoOperacion, int codigoOperacion, string codigoEntidad, int codigoCategoriaEntidad)
        {
            CuentaPorCobrarBL obj = new CuentaPorCobrarBL();
            return obj.GetMontoCuentaPorCobrar(codigoTipoOperacion, codigoOperacion, codigoEntidad, codigoCategoriaEntidad);
        }


    }


}
