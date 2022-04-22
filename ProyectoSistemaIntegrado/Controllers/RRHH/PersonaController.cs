using CapaEntidad.Administracion;
using CapaEntidad.RRHH;
using CapaNegocio.Administracion;
using CapaNegocio.RRHH;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProyectoSistemaIntegrado.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Controllers.RRHH
{
    public class PersonaController : Controller
    {
        [ServiceFilter(typeof(Seguridad))]
        public IActionResult Index()
        {
            return View();
        }

        public List<PersonaCLS> GetAllPersonas(int noIncluidoEnPlanilla)
        {
            PersonaBL obj = new PersonaBL();
            return obj.GetAllPersonas(noIncluidoEnPlanilla);
        }

        public PersonaCLS BusquedaPersona(string cui)
        {
            PersonaBL obj = new PersonaBL();
            return obj.GetDataPersona(cui);
        }

        public string GuardarPersona(PersonaCLS objPersona)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            PersonaBL obj = new PersonaBL();
            objPersona.FechaNacimiento = Util.Conversion.ConvertDateSpanishToEnglish(objPersona.FechaNacimientoStr);
            return obj.GuardarPersona(objPersona, objUsuario.IdUsuario);
        }


        public string ActualizarPersona(PersonaCLS objPersona)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            PersonaBL obj = new PersonaBL();
            objPersona.FechaNacimiento = Util.Conversion.ConvertDateSpanishToEnglish(objPersona.FechaNacimientoStr);
            return obj.ActualizarPersona(objPersona, objUsuario.IdUsuario);
        }


    }




}
