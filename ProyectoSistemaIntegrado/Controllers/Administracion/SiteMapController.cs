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
    public class SiteMapController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Roles()
        {
            return View();
        }

        public IActionResult ConfigRoles()
        {
            return View();
        }

        public List<SiteMapCLS> GetConfiguracion(int codigoSistema, int nivel)
        {
            SiteMapBL obj = new SiteMapBL();
            return obj.GetConfiguracion(codigoSistema, nivel);
        }

        public List<SiteMapCLS> GetSiteMapsPadre(int codigoSistema, int nivel)
        {
            SiteMapBL obj = new SiteMapBL();
            return obj.GetSiteMapsPadre(codigoSistema, nivel);
        }

        public string GuardarItemMenu(SiteMapCLS objMenu)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            SiteMapBL obj = new SiteMapBL();
            return obj.GuardarItemMenu(objMenu, objUsuario.IdUsuario);
        }

        public List<RolCLS> GetAllRoles()
        {
            RolBL obj = new RolBL();
            return obj.GetAllRoles();
        }

        public SiteMapCLS GetDataItemMenu(int codigoSiteMap)
        {
            SiteMapBL obj = new SiteMapBL();
            return obj.GetDataItemMenu(codigoSiteMap);
        }

        public string ActualizarItemMenu(SiteMapCLS objMenu)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            SiteMapBL obj = new SiteMapBL();
            return obj.ActualizarItemMenu(objMenu, objUsuario.IdUsuario);
        }

        public string AnularItemMenu(int codigoSitemap)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            SiteMapBL obj = new SiteMapBL();
            return obj.AnularItemMenu(codigoSitemap, objUsuario.IdUsuario);
        }

        public RolCLS GetDataRol(int codigoRol)
        {
            RolBL obj = new RolBL();
            return obj.GetDataRol(codigoRol);
        }

        public List<SiteMapCLS> GetConfiguracionRol(int codigoRol)
        {
            RolBL obj = new RolBL();
            return obj.GetConfiguracionRol(codigoRol);
        }


    }
}
