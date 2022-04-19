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
    public class ConfiguracionSueldoIndirectoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public string GuardarConfiguracion(ConfiguracionSueldoIndirectoCLS objConfiguracion)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            ConfiguracionSueldoIndirectoBL objConfig = new ConfiguracionSueldoIndirectoBL();
            return objConfig.GuardarConfiguracion(objConfiguracion, objUsuario.IdUsuario);
        }

        public string ActualizarConfiguracion(ConfiguracionSueldoIndirectoCLS objConfiguracion)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            ConfiguracionSueldoIndirectoBL obj = new ConfiguracionSueldoIndirectoBL();
            return obj.ActualizarConfiguracion(objConfiguracion, objUsuario.IdUsuario);
        }

        public List<ConfiguracionSueldoIndirectoCLS> GetConfiguracionSueldoIndirecto(int anio)
        {
            ConfiguracionSueldoIndirectoBL obj = new ConfiguracionSueldoIndirectoBL();
            return obj.GetConfiguracionSueldoIndirecto(anio);
        }



    }
}
