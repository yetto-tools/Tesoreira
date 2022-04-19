using CapaEntidad.Administracion;
using CapaEntidad.RRHH;
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
    public class EmpleadoController : Controller
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

        public List<EmpleadoCLS> BuscarEmpleados(int codigoEmpresa, int codigoArea, int codigoPuesto, int codigoEstado, int btb, int saldoPrestamo)
        {
            EmpleadoBL obj = new EmpleadoBL();
            return obj.GetListaEmpleados(codigoEmpresa, codigoArea, codigoPuesto, codigoEstado, btb, saldoPrestamo);
        }

        public EmpleadoComboCLS FillCombosNewEmpleado()
        {
            EmpleadoBL obj = new EmpleadoBL();
            return obj.FillCombosNewEmpleado();
        }

        public EmpleadoComboCLS GetListFillCombosConsulta()
        {
            EmpleadoBL obj = new EmpleadoBL();
            return obj.GetListFillCombosConsulta();
        }

        public string GuardarEmpleado(EmpleadoCLS objEmpleado)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            EmpleadoBL obj = new EmpleadoBL();
            return obj.GuardarEmpleado(objEmpleado, objUsuario.IdUsuario);
        }

        public string ActualizarEmpleado(EmpleadoCLS objEmpleado)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            EmpleadoBL obj = new EmpleadoBL();
            return obj.ActualizarEmpleado(objEmpleado, objUsuario.IdUsuario);
        }

        public string ActualizarEmpleadoPlanilla(EmpleadoCLS objEmpleado)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            EmpleadoBL obj = new EmpleadoBL();
            objEmpleado.FechaIngreso = Util.Conversion.ConvertDateSpanishToEnglish(objEmpleado.FechaIngresoStr);
            return obj.ActualizarEmpleadoPlanilla(objEmpleado, objUsuario.IdUsuario);
        }

        public EmpleadoCLS GetDataEmpleado(string codigoEmpleado)
        {
            EmpleadoBL obj = new EmpleadoBL();
            return obj.GetDataEmpleado(codigoEmpleado);
        }

        public List<MotivoBajaCLS> GetMotivosDeBaja()
        {
            MotivoBajaBL obj = new MotivoBajaBL();
            return obj.GetMotivosDeBaja();
        }

        public List<MotivoSuspensionCLS> GetMotivosDeSuspension()
        {
            MotivoSuspensionBL obj = new MotivoSuspensionBL();
            return obj.GetMotivosDeSuspension();
        }

        public string DarDeBajaEmpleado(SuspensionCLS objSuspension)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            EmpleadoBL obj = new EmpleadoBL();
            if (objSuspension.CodigoMotivoBaja == Constantes.Empleado.MotivoDeBaja.SUSPENDIDO)
            {
                return obj.DarDeBajaEmpleadoPorSuspension(objSuspension, objUsuario.IdUsuario);
            }
            else {
                return obj.DarDeBajaEmpleado(objSuspension, objUsuario.IdUsuario);
            }
        }



    }


}
