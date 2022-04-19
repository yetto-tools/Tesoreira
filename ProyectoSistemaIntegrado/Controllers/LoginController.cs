using CapaEntidad.Administracion;
using CapaNegocio.Administracion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public List<SiteMapCLS> GetMenus(string idUsuario, int esSuperAdmin)
        {
            SiteMapBL obj = new SiteMapBL();
            return obj.GetMenus(idUsuario, esSuperAdmin);
        }

        public UsuarioCLS login(string usuario, string contrasenia)
        {
            UsuarioBL obj = new UsuarioBL();
            UsuarioCLS objUsuario = obj.Login(usuario, contrasenia);
            if (objUsuario.IdUsuario != null)
            {
                string objCadena = JsonConvert.SerializeObject(objUsuario);
                HttpContext.Session.SetString("usuario", objCadena);
                string idUsuario = objUsuario.IdUsuario;
                List<SiteMapCLS> listaItems = GetMenus(idUsuario, objUsuario.SuperAdmin);
                string objSiteMap = JsonConvert.SerializeObject(listaItems);
                HttpContext.Session.SetString("menus", objSiteMap);

                ProgramacionSemanalBL objProgramacionSemanal = new ProgramacionSemanalBL();
                ProgramacionSemanalCLS objSemanaActual = objProgramacionSemanal.GetSemanaActual();
                string objCadenaSemana = JsonConvert.SerializeObject(objSemanaActual);
                HttpContext.Session.SetString("numeroSemanaActual", objCadenaSemana);
                HttpContext.Session.SetString("tituloPrincipal", "");

            }
            else {
                HttpContext.Session.Remove("usuario");
            }

            return objUsuario;
        }

        public IActionResult CerrarSesion() 
        {
            HttpContext.Session.Remove("usuario");
            return RedirectToAction("Index");
        }
        

    }
}
