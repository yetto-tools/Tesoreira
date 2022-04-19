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

namespace ProyectoSistemaIntegrado.Controllers.Tesoreria
{
    public class ArqueoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public List<ReporteCajaCLS> GetListaArqueos(string usuarioGeneracion)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            ArqueoBL obj = new ArqueoBL();
            return obj.GetListaArqueos(objUsuario.IdUsuario);
        }

        public string GenerarArqueo(int anio, int numeroSemana)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            ArqueoBL obj = new ArqueoBL();
            return obj.GenerarArqueo(anio, numeroSemana, objUsuario.IdUsuario);
        }

        

    }
}
