using CapaEntidad.Administracion;
using CapaEntidad.Tesoreria;
using CapaNegocio.Tesoreria;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProyectoSistemaIntegrado.Models;
using ProyectoSistemaIntegrado.Rotativa;
using Rotativa;
using Rotativa.AspNetCore.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Controllers.Tesoreria
{
    public class CorteCajaSemanalController : Controller
    {
        public static ReporteCajaDetalleListCLS lista;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GeneracionReporte()
        {
            return View();
        }

        public IActionResult ConsultaReportes()
        {
            return View();
        }

        public IActionResult ViewReporteSemanalCajaPDF(int anioOperacion, int semanaOperacion, int codigoReporte)
        {
            ReportViewModel obj = new ReportViewModel();
            obj.CodigoReporte = codigoReporte;
            obj.AnioOperacion = anioOperacion;
            obj.SemanaOperacion = semanaOperacion;
            var demoViewPortrait = new ViewAsPdf("ViewReporteSemanalCajaPDF", String.Empty, obj);
            demoViewPortrait.PageMargins = new Margins { Bottom = 5, Left = 5, Right = 5, Top = 5 };
            demoViewPortrait.PageSize = Size.Letter;
            return demoViewPortrait;
        }

        public string GenerarReporteSemanal(int anio, int numeroSemana)
        {
            ReporteCajaBL obj = new ReporteCajaBL();
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            return obj.GenerarReporteSemanal(anio, numeroSemana, objUsuario.IdUsuario);
        }

        public List<ReporteCajaCLS> GetReportesSemanalesCajaGeneracion()
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            ReporteCajaBL obj = new ReporteCajaBL();
            return obj.GetReportesSemanalesCajaGeneracion(objUsuario.IdUsuario);
        }

        public List<ReporteCajaCLS> GetReportesSemanalesCaja(int anio)
        {
            ReporteCajaBL obj = new ReporteCajaBL();
            return obj.GetReportesSemanalesCaja(anio);
        }

        /// <summary>
        /// Quedará obsoleto, por que ahora se consulta directamente en la tabla de transacciones
        /// </summary>
        /// <param name="codigoReporte"></param>
        /// <returns></returns>
        /*public ReporteCajaDetalleListCLS GetDetalleReporteCajaSemanal(int codigoReporte) 
        {
            ReporteCajaDetalleBL obj = new ReporteCajaDetalleBL();
            return obj.GetDetalleReporte(codigoReporte);
        }*/

        /*public ReporteCajaDetalleListCLS GetDetalleReporteCaja(int anioOperacion, int semanaOperacion, int codigoReporte)
        {
            ReporteCajaDetalleBL obj = new ReporteCajaDetalleBL();
            return obj.GetDetalleReporteCaja(anioOperacion, semanaOperacion, codigoReporte);
        }*/


        public string EliminarReporteSemanal(int codigoReporte, int anioOperacion, int semanaOperacion)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            ReporteCajaBL obj = new ReporteCajaBL();
            return obj.EliminarReporteSemanal(codigoReporte, anioOperacion, semanaOperacion, objUsuario.IdUsuario);
        }

        public string ExisteReporte(int anio, int numeroSemana)
        {
            ReporteCajaBL obj = new ReporteCajaBL();
            return obj.ExisteReporte(anio, numeroSemana);
        }

        public string AceptarReporteGenerado(int codigoReporte, int anioOperacion, int semanaOperacion)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            ReporteCajaBL obj = new ReporteCajaBL();
            return obj.AceptarReporteGenerado(codigoReporte, anioOperacion, semanaOperacion, objUsuario.IdUsuario);
        }

        public string AceptarReportePorContabilidad(int codigoReporte, int anioOperacion, int semanaOperacion)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            ReporteCajaBL obj = new ReporteCajaBL();
            return obj.AceptarReportePorContabilidad(codigoReporte, anioOperacion, semanaOperacion, objUsuario.IdUsuario);
        }

        public List<ReporteCajaCLS> GetReportesCajaEnProceso(int anioOperacion, int semanaOperacion)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            ReporteCajaBL obj = new ReporteCajaBL();
            return obj.GetReportesCajaEnProceso(anioOperacion, semanaOperacion, objUsuario.SuperAdmin);
        }

        public List<ReporteCajaCLS> GetReportesCajaConsulta(int anioOperacion, int semanaOperacion)
        {
            ReporteCajaBL obj = new ReporteCajaBL();
            return obj.GetReportesCajaConsulta(anioOperacion, semanaOperacion);
        }

        public List<ReporteCajaCLS> GetReportesCaja(int anioOperacion, int semanaOperacion)
        {
            ReporteCajaBL obj = new ReporteCajaBL();
            return obj.GetReportesCaja(anioOperacion, semanaOperacion);
        }

    }
}
