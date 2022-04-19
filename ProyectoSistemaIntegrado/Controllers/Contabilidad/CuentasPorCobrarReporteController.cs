using CapaEntidad.Administracion;
using CapaEntidad.Contabilidad;
using CapaNegocio.Contabilidad;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Controllers.Contabilidad
{
    public class CuentasPorCobrarReporteController : BaseController
    {
        public static List<CuentaPorCobrarReporteDetalleCLS> lista;

        public IActionResult GenerarCuentasPorCobrar()
        {
            return View();
        }

        public IActionResult MostrarDetalleReporteCuentasPorCobrar()
        {
            return View();
        }

        public IActionResult MostrarReportesCuentasPorCobrarConsulta()
        {
            return View();
        }

        public IActionResult MostrarDetalleReporteCuentasPorCobrarConsulta() 
        {
            return View();
        }

        public List<CuentaPorCobrarReporteCLS> GetReportesCuentasPorCobrarParaGeneracion()
        {
            CuentaPorCobrarReporteBL obj = new CuentaPorCobrarReporteBL();
            return obj.GetReportesCuentasPorCobrarParaGeneracion();
        }

        public List<CuentaPorCobrarReporteCLS> GetReportesCuentasPorCobrarParaConsulta()
        {
            CuentaPorCobrarReporteBL obj = new CuentaPorCobrarReporteBL();
            return obj.GetReportesCuentasPorCobrarParaConsulta();
        }

        public string GenerarReporteCuentaPorCobrar(int codigoReporte, int anioOperacion, int semanaOperacion)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            CuentaPorCobrarReporteBL obj = new CuentaPorCobrarReporteBL();
            return obj.GenerarReporteCuentaPorCobrar(codigoReporte, anioOperacion, semanaOperacion, objUsuario.IdUsuario);
        }

        public List<CuentaPorCobrarReporteDetalleCLS> GetDetalleReporteCuentasPorCobrar(int codigoReporte, int anioOperacion, int semanaOperacion, int codigoEstado)
        {
            CuentaPorCobrarReporteBL obj = new CuentaPorCobrarReporteBL();
            if (codigoEstado == Constantes.CuentaPorCobrar.EstadoReporte.POR_GENERAR)
            {
                return obj.GetDetalleReporteCuentasPorCobrarGeneracion(codigoReporte, anioOperacion, semanaOperacion);
            }
            else
            {
                return obj.GetDetalleReporteCuentasPorCobrar(codigoReporte, anioOperacion, semanaOperacion);
            }
        }

        public FileResult ExportarExcel(int codigoReporte, int anioOperacion, int semanaOperacion)
        {
            CuentaPorCobrarReporteBL obj = new CuentaPorCobrarReporteBL();
            lista =  obj.GetDetalleReporteCuentasPorCobrar(codigoReporte, anioOperacion, semanaOperacion);

            string[] cabeceras = { "Código Entidad", "Nombre Entidad", "Código Categoría", "Categoría", "Saldo Inicial","Monto Solicitado", "Monto Devoluciones", "Saldo Final", "Operacion" };
            string[] nombrePropiedades = { "CodigoEntidad", "NombreEntidad", "CodigoCategoria", "Categoria", "SaldoInicial","montoSolicitado", "MontoDevolucion", "SaldoFinal", "Operacion" };
            byte[] buffer = ExportarExcelDatos(cabeceras, nombrePropiedades, lista);
            return File(buffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public string AceptarReporteComoValido(int codigoReporte, int anioOperacion, int semanaOperacion, string usuarioAct)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            CuentaPorCobrarReporteBL obj = new CuentaPorCobrarReporteBL();
            return obj.AceptarReporteComoValido(codigoReporte, anioOperacion, semanaOperacion, objUsuario.IdUsuario);
        }

        public string EliminarReporteGenerado(int codigoReporte, int anioOperacion, int semanaOperacion)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            CuentaPorCobrarReporteBL obj = new CuentaPorCobrarReporteBL();
            return obj.EliminarReporteGenerado(codigoReporte, anioOperacion, semanaOperacion, objUsuario.IdUsuario);
        }

    }


}
