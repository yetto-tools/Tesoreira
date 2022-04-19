using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapaEntidad.Administracion;
using CapaEntidad.Planilla;
using CapaNegocio.Planilla;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ProyectoSistemaIntegrado.Controllers.Planilla
{
    public class PlanillaController : Controller
    {
        public IActionResult ConfigDescuentosDevoluciones()
        {
            return View();
        }

        public IActionResult ListEmpleados()
        {
            return View();
        }

        public IActionResult EditEmpleado()
        {
            return View();
        }

        public List<ConfiguracionPrestamoCLS> GetEmpleadosCuentasPorCobrar(int codigoFrecuenciaPago)
        {
            ConfiguracionDescuentoDevolucionBL obj = new ConfiguracionDescuentoDevolucionBL();
            return obj.GetEmpleadosCuentasPorCobrar(codigoFrecuenciaPago);
        }

        public List<ConfiguracionPrestamoCLS> GetEmpleadosBackToBack()
        {
            ConfiguracionDescuentoDevolucionBL obj = new ConfiguracionDescuentoDevolucionBL();
            return obj.GetEmpleadosBackToBack();
        }

        public string RegistrarConfiguracionDevolucionBTB(int codigoEmpresa, string codigoEmpleado, decimal montoSalarioDiario, decimal montoBonoDecreto372001)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            ConfiguracionDescuentoDevolucionBL obj = new ConfiguracionDescuentoDevolucionBL();
            return obj.RegistrarConfiguracionDevolucionBTB(codigoEmpresa, codigoEmpleado, montoSalarioDiario, montoBonoDecreto372001, objUsuario.IdUsuario);
        }

        public string RegistrarConfiguracionPrestamo(int codigoEmpresa, string codigoEmpleado, decimal montoDescuentoPrestamo)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            ConfiguracionDescuentoDevolucionBL obj = new ConfiguracionDescuentoDevolucionBL();
            return obj.RegistrarConfiguracionPrestamo(codigoEmpresa, codigoEmpleado, montoDescuentoPrestamo, objUsuario.IdUsuario);
        }

    }


}
