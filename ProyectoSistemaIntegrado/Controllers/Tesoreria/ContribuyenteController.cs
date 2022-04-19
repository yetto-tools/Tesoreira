using CapaDatos.Tesoreria;
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
    public class ContribuyenteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public string GuardarContribuyente(ContribuyenteCLS objContribuyente)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);
            ContribuyenteDAL obj = new ContribuyenteDAL();
            return obj.GuardarContribuyente(objContribuyente, objUsuario.IdUsuario);
        }

        public ContribuyenteCLS GetDataContribuyente(string nit)
        {
            ContribuyenteBL obj = new ContribuyenteBL();
            return obj.GetDataContribuyente(nit);
        }

    }



}
