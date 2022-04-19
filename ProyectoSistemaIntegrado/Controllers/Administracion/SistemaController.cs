using CapaEntidad.Administracion;
using CapaNegocio.Administracion;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Controllers.Administracion
{
    public class SistemaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public List<SistemaCLS> GetAllSistemas()
        {
            SistemaBL obj = new SistemaBL();
            return obj.GetAllSistemas();
        }




    }
}
