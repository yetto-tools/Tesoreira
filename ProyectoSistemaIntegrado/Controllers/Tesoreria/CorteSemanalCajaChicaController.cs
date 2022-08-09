using CapaEntidad.Administracion;
using CapaEntidad.Tesoreria;
using CapaNegocio.Tesoreria;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using ProyectoSistemaIntegrado.Models;
using ProyectoSistemaIntegrado.Rotativa;
using Rotativa.AspNetCore.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Controllers.Tesoreria
{
    public class CorteSemanalCajaChicaController : BaseController
    {
        public static List<CajaChicaCLS> lista;

        public IActionResult MostrarCortesSemanalesGeneracion()
        {
            return View();
        }

        public IActionResult MostrarCortesSemanalesRevision()
        {
            return View();
        }

        public IActionResult EditCorreccion()
        {
            return View();
        }

        public IActionResult MostrarCortesSemanalesConsulta()
        {
            return View();
        }

        public IActionResult MostrarDetalle()
        {
            return View();
        }

        public IActionResult MostrarDetalleConsulta()
        {
            return View();
        }

        public IActionResult MostrarDetalleRevision()
        {
            return View();
        }

        public IActionResult RecepcionReembolso()
        {
            return View();
        }

        public IActionResult RecepcionReembolsoContabilidad()
        {
            return View();
        }

        public IActionResult ViewReporteReintegroCajaChica(int codigoReporte, int anioOperacion, int semanaOperacion, int codigoCajaChica)
        {
            ReportContabilidadViewModel obj = new ReportContabilidadViewModel();
            obj.CodigoReporte = codigoReporte;
            obj.AnioOperacion = anioOperacion;
            obj.SemanaOperacion = semanaOperacion;
            obj.CodigoCajaChica = codigoCajaChica;
            var demoViewPortrait = new ViewAsPdf("ViewReporteReintegroCajaChica", String.Empty, obj);
            demoViewPortrait.PageMargins = new Margins { Bottom = 5, Left = 5, Right = 5, Top = 5 };
            demoViewPortrait.PageSize = Size.Letter;
            return demoViewPortrait;
        }

        public IActionResult ViewReporteCorteCajaChica(int anioOperacion, int semanaOperacion, int codigoReporte, int arqueo)
        {
            ReportContabilidadViewModel obj = new ReportContabilidadViewModel();
            obj.AnioOperacion = anioOperacion;
            obj.SemanaOperacion = semanaOperacion;
            obj.CodigoReporte = codigoReporte;
            obj.Arqueo = arqueo;
            var demoViewPortrait = new ViewAsPdf("ViewReporteCorteCajaChica", String.Empty, obj);
            demoViewPortrait.PageMargins = new Margins { Bottom = 5, Left = 5, Right = 5, Top = 5 };
            demoViewPortrait.PageSize = Size.Letter;
            return demoViewPortrait;
        }

        public FileResult ExportarExcel(int codigoReporte,int codigoCajaChica,int anioOperacion,int semanaOperacion)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            CajaChicaBL obj = new CajaChicaBL();
            if (codigoReporte != 0)
            {
                lista = obj.ListarTransaccionesCajaChica(codigoReporte);
            }
            else
            {
                lista = obj.GetTransaccionesCajaChicaConsulta(codigoCajaChica, anioOperacion, semanaOperacion, objUsuario.IdUsuario);
            }

            string[] cabeceras = { "Código Transacción", "Caja Chica", "Nit Proveedor","Nombre Proveedor","Fecha Factura","Serie Factura","Numero Factura","Monto","Descripción" };
            string[] nombrePropiedades = { "CodigoTransaccion", "NombreCajaChica", "NitProveedor", "NombreProveedor", "FechaDocumento", "SerieFactura", "NumeroDocumento", "Monto", "Descripcion" };
            byte[] buffer = ExportarExcelDatos(cabeceras, nombrePropiedades, lista);
            return File(buffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public List<ReporteCajaChicaCLS> ListarReportesCajaChica()
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            ReporteCajaChicaBL obj = new ReporteCajaChicaBL();
            return obj.ListarReportesCajaChica(objUsuario.IdUsuario, objUsuario.SuperAdmin);
        }

        public List<ReporteCajaChicaCLS> ListarReportesCajaChicaConsulta(int codigoCajaChica, int anioOperacion)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            ReporteCajaChicaBL obj = new ReporteCajaChicaBL();
            return obj.ListarReportesCajaChicaConsulta(codigoCajaChica, anioOperacion, objUsuario.IdUsuario, objUsuario.SuperAdmin);
        }

        public string GenerarReporteSemanal(int codigoCajaChica, int anioOperacion, int semanaOperacion)
        {
            ReporteCajaChicaBL obj = new ReporteCajaChicaBL();
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            return obj.GenerarReporteSemanal(codigoCajaChica, anioOperacion, semanaOperacion, "", objUsuario.IdUsuario);
        }

        public string FinalizarRevision(int codigoReporte, decimal montoReintegroCalculado, decimal montoReintegro)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            ReporteCajaChicaBL obj = new ReporteCajaChicaBL();
            return obj.FinalizarRevision(codigoReporte, montoReintegroCalculado, montoReintegro,  objUsuario.IdUsuario);
        }

        public string TrasladarReporteParaRevision(int codigoReporte, int codigoCajaChica, decimal montoGastosFiscales)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            ReporteCajaChicaBL obj = new ReporteCajaChicaBL();
            return obj.TrasladarReporteParaRevision(codigoReporte, codigoCajaChica, montoGastosFiscales, objUsuario.IdUsuario);
        }

        public string AnularReporteGenerado(int codigoReporte)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            ReporteCajaChicaBL obj = new ReporteCajaChicaBL();
            return obj.AnularReporteGenerado(codigoReporte, objUsuario.IdUsuario);
        }

        public List<ReporteCajaChicaCLS> ListarReportesCajaChicaParaRevision()
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            ReporteCajaChicaBL obj = new ReporteCajaChicaBL();
            return obj.ListarReportesCajaChicaParaRevision(objUsuario.IdUsuario, objUsuario.SuperAdmin);
        }



    }
}
