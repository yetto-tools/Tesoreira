using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapaEntidad.Administracion;
using CapaEntidad.Tesoreria;
using CapaNegocio.Tesoreria;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProyectoSistemaIntegrado.Models;
using ProyectoSistemaIntegrado.Rotativa;
using Rotativa.AspNetCore.Options;

namespace ProyectoSistemaIntegrado.Controllers.Tesoreria
{
    public class LiquidacionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Traslado()
        {
            return View();
        }

        public IActionResult Detalle()
        {
            return View();
        }

        public IActionResult DetalleConsulta()
        {
            return View();
        }

        public IActionResult ViewReporteTrasladoLiquidacion(long codigoTraslado, int anioOperacion, int semanaOperacion)
        {
            ReportViewModel obj = new ReportViewModel();
            obj.CodigoTraslado = codigoTraslado;
            obj.AnioOperacion = anioOperacion;
            obj.SemanaOperacion = semanaOperacion;
            var demoViewPortrait = new ViewAsPdf("ViewReporteTrasladoLiquidacion", String.Empty, obj);
            demoViewPortrait.PageMargins = new Margins { Bottom = 5, Left = 5, Right = 5, Top = 5 };
            demoViewPortrait.PageSize = Size.Letter;

            return demoViewPortrait;
        }

        public List<TrasladoLiquidacionCLS> GetTrasladosParaLiquidacion()
        {
            LiquidacionBL obj = new LiquidacionBL();
            return obj.GetTrasladosParaLiquidacion();
        }


        public List<TrasladoLiquidacionDetalleCLS> GetDetalleTrasladoLiquidacion(long codigoTraslado, int anioOperacion, int semanaOperacion)
        {
            LiquidacionBL obj = new LiquidacionBL();
            if (codigoTraslado == 0)
            {
                return obj.GetDetalleTrasladoLiquidacionPorGenerar(anioOperacion, semanaOperacion);
            }
            else {
                return obj.GetDetalleTrasladoLiquidacion(codigoTraslado);
            }
        }

        public List<TrasladoLiquidacionCLS> GetTrasladosLiquidacionConsulta(int anioOperacion)
        {
            LiquidacionBL obj = new LiquidacionBL();
            return obj.GetTrasladosLiquidacionConsulta(anioOperacion);
        }
        public string GenerarTraslado(int anioOperacion, int semanaOperacion)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            LiquidacionBL obj = new LiquidacionBL();
            return obj.GenerarTraslado(anioOperacion, semanaOperacion, objUsuario.IdUsuario);
        }

        public string AnularTraslado(long codigoTraslado)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            LiquidacionBL obj = new LiquidacionBL();
            return obj.AnularTraslado(codigoTraslado, objUsuario.IdUsuario);
        }

        public string TrasladarParaLiquidacion(long codigoTraslado)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            LiquidacionBL obj = new LiquidacionBL();
            return obj.TrasladarParaLiquidacion(codigoTraslado, objUsuario.IdUsuario);
        }

    }
}
