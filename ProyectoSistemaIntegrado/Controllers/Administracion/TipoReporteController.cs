using CapaEntidad.Administracion;
using CapaNegocio.Administracion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Controllers.Administracion
{
    public class TipoReporteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public List<TipoReporteCLS> GetAllTiposReportes()
        {
            TipoReporteBL obj = new TipoReporteBL();
            return obj.GetAllTiposReportes();
        }

        public string GuardarTipoReporte(TipoReporteCLS objTipoReporte)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            TipoReporteBL obj = new TipoReporteBL();
            return obj.GuardarTipoReporte(objTipoReporte, objUsuario.IdUsuario);
        }

        public string ActualizarTipoReporte(TipoReporteCLS objTipoReporte)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            TipoReporteBL obj = new TipoReporteBL();
            return obj.ActualizarTipoReporte(objTipoReporte, objUsuario.IdUsuario);
        }


    }
}
