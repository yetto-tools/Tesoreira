using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapaEntidad.Administracion;
using CapaEntidad.Ventas;
using CapaNegocio.Ventas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ProyectoSistemaIntegrado.Controllers.Ventas
{
    public class RutasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public string GuardarRuta(RutaCLS objRuta)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            RutaBL obj = new RutaBL();
            return obj.GuardarRuta(objRuta, objUsuario.IdUsuario);
        }

        public List<RutaCLS> GetListaRutas()
        {
            RutaBL obj = new RutaBL();
            return obj.GetListaRutas();
        }

        public List<RutaCLS> GetRutas()
        {
            RutaBL obj = new RutaBL();
            return obj.GetRutas();
        }

        public string ActualizarRuta(RutaCLS objRuta)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            RutaBL obj = new RutaBL();
            return obj.ActualizarRuta(objRuta, objUsuario.IdUsuario);
        }

    }


}
