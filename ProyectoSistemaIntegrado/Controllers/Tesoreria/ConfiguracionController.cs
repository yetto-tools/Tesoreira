using CapaEntidad.Tesoreria;
using CapaNegocio.Tesoreria;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Controllers.Tesoreria
{
    public class ConfiguracionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public List<OtroIngresoCLS> GetListaOtrosIngresos()
        {
            ConfiguracionBL obj = new ConfiguracionBL();
            return obj.GetListaOtrosIngresos();
        }

    }
}
