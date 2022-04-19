using CapaEntidad.Administracion;
using CapaEntidad.Tesoreria;
using CapaNegocio.Administracion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProyectoSistemaIntegrado.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Controllers.Administracion
{
    public class UsuarioController : Controller
    {
        [ServiceFilter(typeof(Seguridad))]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult New()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult CambioPassword()
        {
            return View();
        }

        public IActionResult CambioPasswordAdmin()
        {
            return View();
        }

        public string GuardarUsuario(UsuarioCLS objUsuarioLogin)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            objUsuarioLogin.UsuarioIng = objUsuario.IdUsuario;
            UsuarioBL obj = new UsuarioBL();
            return obj.GuardarUsuario(objUsuarioLogin);
        }

        public string ActualizarContrasenia(UsuarioCLS objUsuario)
        {
            UsuarioBL obj = new UsuarioBL();
            return obj.ActualizarContrasenia(objUsuario);
        }

        public List<UsuarioCLS> ListarUsuarios()
        {
            UsuarioBL obj = new UsuarioBL();
            return obj.GetListaUsuarios();
        }

        public UsuarioCLS GetDataUsuario(string idUsuario)
        {
            UsuarioBL obj = new UsuarioBL();
            return obj.GetDataUsuario(idUsuario);
        }

        public List<RolCLS> GetPermisoRoles(string idUsuario)
        {
            UsuarioBL obj = new UsuarioBL();
            return obj.GetPermisoRoles(idUsuario);
        }

        public List<ConfiguracionCajaChicaCLS> GetPermisoCajasChicas(string idUsuario)
        {
            UsuarioBL obj = new UsuarioBL();
            return obj.GetPermisoCajasChicas(idUsuario);
        }

        public List<EmpresaCLS> GetPermisoEmpresas(string idUsuario)
        {
            UsuarioBL obj = new UsuarioBL();
            return obj.GetPermisoEmpresas(idUsuario);
        }

        public List<TipoReporteCLS> GetPermisoReportes(string idUsuario)
        {
            UsuarioBL obj = new UsuarioBL();
            return obj.GetPermisoReportes(idUsuario);
        }

        public string GuardarPermisos([FromBody] List<PermisoCLS> objPermisos, string idUsuario)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            UsuarioBL obj = new UsuarioBL();
            return obj.GuardarPermisos(objPermisos, idUsuario, objUsuario.IdUsuario);
        }


    }
}
