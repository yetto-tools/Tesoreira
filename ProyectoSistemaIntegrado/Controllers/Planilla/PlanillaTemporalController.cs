using CapaEntidad.Administracion;
using CapaEntidad.Planilla;
using CapaNegocio.Planilla;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProyectoSistemaIntegrado.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Controllers.Planilla
{
    public class PlanillaTemporalController : Controller
    {
        public IActionResult ConsultaPagosDescuentos()
        {
            return View();
        }

        public IActionResult ConsultaDevolucionesBTB()
        {
            return View();
        }

        public IActionResult PagosDescuentos()
        {
            return View();
        }

        public IActionResult PagosBackToBack()
        {
            return View();
        }

        public List<PagoDescuentoCLS> GetEmpleadosBackToBackPlanilla(int codigoTipoPlanilla, int anioPlanilla, int mesPlanilla)
        {
            PagoBackToBackPlanillaBL obj = new PagoBackToBackPlanillaBL();
            return obj.GetEmpleadosBackToBackPlanilla(codigoTipoPlanilla, anioPlanilla, mesPlanilla);
        }

        public List<PagoDescuentoCLS> GetPagosBackToBackRealizadosEnPlanilla(int anio, int mes, int codigoEmpresa)
        {
            PagoBackToBackPlanillaBL obj = new PagoBackToBackPlanillaBL();
            return obj.GetPagosBackToBackRealizadosEnPlanilla(anio, mes, codigoEmpresa);
        }

        public List<PagoDescuentoCLS> GetEmpleadosBackToBackBoletaDeposito(int codigoTipoPlanilla, int anioPlanilla, int mesPlanilla)
        {
            PagoBackToBackPlanillaBL obj = new PagoBackToBackPlanillaBL();
            return obj.GetEmpleadosBackToBackBoletaDeposito(codigoTipoPlanilla, anioPlanilla, mesPlanilla);
        }

        public List<SaldoPrestamoCLS> GetEmpleadosCuentasPorCobrarPlanilla()
        {
            PagoDescuentoBL obj = new PagoDescuentoBL();
            return obj.GetEmpleadosCuentasPorCobrarPlanilla();
        }

        public string GuardarDescuentoDevolucion(int codigoEmpresa, string codigoEmpleado, int codigoOperacion, decimal monto)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            PagoDescuentoBL obj = new PagoDescuentoBL();
            return obj.GuardarDescuentoDevolucion(codigoEmpresa, codigoEmpleado, codigoOperacion, monto, objUsuario.IdUsuario);
        }

        public string GuardarDevolucionesBTB([FromBody] List<PagoDescuentoCLS> objPagoDescuento)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            PagoBackToBackPlanillaBL obj = new PagoBackToBackPlanillaBL();
            return obj.GuardarDevolucionesBTB(objPagoDescuento, objUsuario.IdUsuario);
        }

        public List<PagoDescuentoCLS> GetPagosDescuentos()
        {
            PagoDescuentoBL obj = new PagoDescuentoBL();
            return obj.GetPagosDescuentos();
        }

        public string AnularPagoDescuento(int codigoPago)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            PagoDescuentoBL obj = new PagoDescuentoBL();
            return obj.AnularPagoDescuento(codigoPago, objUsuario.IdUsuario);
        }

        public List<PagoDescuentoCLS> GetPagosDescuentosConsulta(int anio, int mes, int codigoEmpresa)
        {
            PagoDescuentoBL obj = new PagoDescuentoBL();
            return obj.GetPagosDescuentosConsulta(anio, mes, codigoEmpresa);
        }

    }

}
