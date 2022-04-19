using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapaEntidad.Administracion;
using CapaNegocio.Administracion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ProyectoSistemaIntegrado.Controllers.Administracion
{
    public class NotificacionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public string GuardarConfiguracion(NotificacionCLS objNotificacion)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            NotificacionBL obj = new NotificacionBL();
            return obj.GuardarConfiguracion(objNotificacion, objUsuario.IdUsuario);
        }

        public List<NotificacionCLS> GetAllConfiguraciones()
        {
            NotificacionBL obj = new NotificacionBL();
            return obj.GetAllConfiguraciones();
        }

        public string EliminarConfiguracion(string cui, int codigoTipoNotificacion)
        {
            NotificacionBL obj = new NotificacionBL();
            return obj.EliminarConfiguracion(cui, codigoTipoNotificacion);
        }




    }
}
