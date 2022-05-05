using CapaEntidad.Administracion;
using CapaEntidad.Contabilidad;
using CapaEntidad.Tesoreria;
using CapaNegocio.Administracion;
using CapaNegocio.Contabilidad;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProyectoSistemaIntegrado.Models;
using ProyectoSistemaIntegrado.Rotativa;
using Rotativa.AspNetCore.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Controllers.Reportes
{
    public class ReportesController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MostrarReporte()
        {
            return View();
        }

        public IActionResult MostrarReporteCorteCajaChica()
        {
            return View();
        }

        public IActionResult MostrarReporteCompromisoFiscal()
        {
            return View();
        }

        public List<TipoReporteCLS> GetTiposDeReportesAsignados()
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            TipoReporteBL obj = new TipoReporteBL();
            return obj.GetTiposDeReportesAsignados(objUsuario.IdUsuario, objUsuario.SuperAdmin);
        }

        public List<ReporteCajaCLS> GetReportes(int codigoTipoReporte)
        {
            ReportesTesoreriaBL obj = new ReportesTesoreriaBL();
            return obj.GetReportes(codigoTipoReporte);
        }

        public List<ReporteCajaChicaCLS> GetCortesCajaChica(int anioOperacion, int codigoCajaChica)
        {
            ReportesTesoreriaBL obj = new ReportesTesoreriaBL();
            return obj.GetCortesCajaChica(anioOperacion, codigoCajaChica);
        }

        public List<CompromisoFiscalCLS> GetReportesCompromisoFiscal(int anioOperacion)
        {
            CompromisoFiscalBL obj = new CompromisoFiscalBL();
            return obj.GetReportesCompromisoFiscal(anioOperacion);
        }

        /*public List<CompromisoFiscalDetalleCLS> GetReportesCompromisoFiscal()
        {
            ReportesTesoreriaBL obj = new ReportesTesoreriaBL();
            public List<CompromisoFiscalDetalleCLS> GetDetalleReporteCompromisoFiscalSemanal(int anioOperacion, int semanaOperacion)
            return obj.GetDetalleReporteCompromisoFiscalSemanal();
        }*/

        public IActionResult ViewReporteResumenOperacionesCaja(int anioOperacion, int semanaOperacion, int codigoReporte, int arqueo)
        {

            ReportContabilidadViewModel obj = new ReportContabilidadViewModel();
            obj.AnioOperacion = anioOperacion;
            obj.SemanaOperacion = semanaOperacion;
            obj.CodigoReporte = codigoReporte;
            obj.Arqueo = arqueo;
            var demoViewPortrait = new ViewAsPdf("ViewReporteResumenOperacionesCaja", String.Empty, obj);
            demoViewPortrait.PageMargins = new Margins { Bottom = 5, Left = 5, Right = 5, Top = 5 };
            demoViewPortrait.PageSize = Size.Letter;

            return demoViewPortrait;
        }

        public IActionResult ViewReporteResumenOperacionesCajaDetallado(int anioOperacion, int semanaOperacion, int codigoReporte, int arqueo)
        {

            ReportContabilidadViewModel obj = new ReportContabilidadViewModel();
            obj.AnioOperacion = anioOperacion;
            obj.SemanaOperacion = semanaOperacion;
            obj.CodigoReporte = codigoReporte;
            obj.Arqueo = arqueo;
            var demoViewPortrait = new ViewAsPdf("ViewReporteResumenOperacionesCajaDetallado", String.Empty, obj);
            demoViewPortrait.PageMargins = new Margins { Bottom = 5, Left = 5, Right = 5, Top = 5 };
            demoViewPortrait.PageSize = Size.Letter;

            return demoViewPortrait;
        }


        public IActionResult ViewReporteOperacionesCaja(int anioOperacion, int semanaOperacion, int codigoReporte, int arqueo)
        {
            ReportContabilidadViewModel obj = new ReportContabilidadViewModel();
            obj.AnioOperacion = anioOperacion;
            obj.SemanaOperacion = semanaOperacion;
            obj.CodigoReporte = codigoReporte;
            obj.Arqueo = arqueo;
            var demoViewPortrait = new ViewAsPdf("ViewReporteOperacionesCaja", String.Empty, obj);
            demoViewPortrait.PageMargins = new Margins { Bottom = 15, Left = 5, Right = 5, Top = 5 };
            demoViewPortrait.PageSize = Size.Letter;
            //demoViewPortrait.CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8";
            //demoViewPortrait.CustomSwitches = "--page-offset 0 --footer-center Página:[page]/[toPage] --footer-line --footer-font-size 10";
            demoViewPortrait.CustomSwitches = "--footer-center Página:[page]/[toPage] --footer-font-size 10 --footer-spacing 2";
            /*demoViewPortrait.CustomSwitches =
                    "--footer-center \"Name: " + "XYZ" + "  DOS: " +
                    DateTime.Now.Date.ToString("MM/dd/yyyy") + "  
                    Page:[page]/[toPage]\"" +
                   " --footer-line --footer-font-size \"9\" 
                   --footer - spacing 6--footer - font - name \"calibri light\""*/

            return demoViewPortrait;
        }

        public IActionResult ViewReporteDepositosBancarios(int anioOperacion, int semanaOperacion, int codigoReporte, int arqueo)
        {
            ReportContabilidadViewModel obj = new ReportContabilidadViewModel();
            obj.AnioOperacion = anioOperacion;
            obj.SemanaOperacion = semanaOperacion;
            obj.CodigoReporte = codigoReporte;
            obj.Arqueo = arqueo;
            var demoViewPortrait = new ViewAsPdf("ViewReporteDepositosBancarios", String.Empty, obj);
            demoViewPortrait.PageMargins = new Margins { Bottom = 5, Left = 5, Right = 5, Top = 5 };
            demoViewPortrait.PageSize = Size.Letter;
            return demoViewPortrait;
        }


        public IActionResult ViewReporteOperacionesDeSocios(int anioOperacion, int semanaOperacion, int codigoReporte, int arqueo)
        {
            ReportContabilidadViewModel obj = new ReportContabilidadViewModel();
            obj.AnioOperacion = anioOperacion;
            obj.SemanaOperacion = semanaOperacion;
            obj.CodigoReporte = codigoReporte;
            obj.Arqueo = arqueo;
            var demoViewPortrait = new ViewAsPdf("ViewReporteOperacionesDeSocios", String.Empty, obj);
            demoViewPortrait.PageMargins = new Margins { Bottom = 5, Left = 5, Right = 5, Top = 5 };
            demoViewPortrait.PageSize = Size.Letter;
            return demoViewPortrait;
        }


        public IActionResult ViewReporteReservasYCajas(int anioOperacion, int semanaOperacion, int codigoReporte, int arqueo)
        {
            ReportContabilidadViewModel obj = new ReportContabilidadViewModel();
            obj.AnioOperacion = anioOperacion;
            obj.SemanaOperacion = semanaOperacion;
            obj.CodigoReporte = codigoReporte;
            obj.Arqueo = arqueo;
            var demoViewPortrait = new ViewAsPdf("ViewReporteReservasYCajas", String.Empty, obj);
            demoViewPortrait.PageMargins = new Margins { Bottom = 5, Left = 5, Right = 5, Top = 5 };
            demoViewPortrait.PageSize = Size.Letter;
            return demoViewPortrait;
        }


        public IActionResult ViewReporteCierre(int anioOperacion, int semanaOperacion, int codigoReporte, int arqueo)
        {
            ReportContabilidadViewModel obj = new ReportContabilidadViewModel();
            obj.AnioOperacion = anioOperacion;
            obj.SemanaOperacion = semanaOperacion;
            obj.CodigoReporte = codigoReporte;
            obj.Arqueo = arqueo;
            var demoViewPortrait = new ViewAsPdf("ViewReporteCierre", String.Empty, obj);
            demoViewPortrait.PageMargins = new Margins { Bottom = 5, Left = 5, Right = 5, Top = 5 };
            demoViewPortrait.PageSize = Size.Letter;
            return demoViewPortrait;
        }

        public IActionResult ViewReporteCuentasPorCobrar(int anioOperacion, int semanaOperacion, int codigoReporte, int arqueo)
        {
            ReportContabilidadViewModel obj = new ReportContabilidadViewModel();
            obj.AnioOperacion = anioOperacion;
            obj.SemanaOperacion = semanaOperacion;
            obj.CodigoReporte = codigoReporte;
            obj.Arqueo = arqueo;
            var demoViewPortrait = new ViewAsPdf("ViewReporteCuentasPorCobrar", String.Empty, obj);
            demoViewPortrait.PageMargins = new Margins { Bottom = 5, Left = 5, Right = 5, Top = 5 };
            demoViewPortrait.PageSize = Size.Letter;
            return demoViewPortrait;
        }

        //public IActionResult ViewReporteCorteCajaChica(int anioOperacion, int semanaOperacion, int codigoReporte, int arqueo)
        //{
        //    ReportContabilidadViewModel obj = new ReportContabilidadViewModel();
        //    obj.AnioOperacion = anioOperacion;
        //    obj.SemanaOperacion = semanaOperacion;
        //    obj.CodigoReporte = codigoReporte;
        //    obj.Arqueo = arqueo;
        //    var demoViewPortrait = new ViewAsPdf("ViewReporteCuentasPorCobrar", String.Empty, obj);
        //    demoViewPortrait.PageMargins = new Margins { Bottom = 5, Left = 5, Right = 5, Top = 5 };
        //    demoViewPortrait.PageSize = Size.Letter;
        //    return demoViewPortrait;
        //}


        public FileResult ExportarExcel(int codigoTipoReporte, int anioReporte, int semanaReporte, int codigoReporte, int arqueo)
        {
            string[] cabeceras = new string[] { };
            string[] nombrePropiedades = new string[] { };
            List<CuentaPorCobrarReporteDetalleCLS> lista = null;
            ReporteOperacionesCajaListCLS reporte = null;
            switch (codigoTipoReporte)
            {
                case Constantes.Reporte.CUENTAS_POR_COBRAR:
                    CuentaPorCobrarReporteBL obj = new CuentaPorCobrarReporteBL();
                    reporte = obj.GetDetalleCorteCuentasPorCobrar(anioReporte, semanaReporte, codigoReporte, arqueo);
                    lista = reporte.listaDetalleCuentasPorCobrar;
                    cabeceras = new string[10]{ "Código Entidad", "Entidad", "Código Operación", "Operación", "Código Categoría", "Categoría", "Saldo Inicial","Monto Solicitado", "Monto Devoluciones", "Saldo Final" };
                    nombrePropiedades = new string[10]{ "CodigoEntidad", "NombreEntidad", "CodigoOperacion", "Operacion", "CodigoCategoria", "Categoria", "SaldoInicial","MontoSolicitado", "MontoDevolucion", "SaldoFinal" };
                    break;
                default:
                    
                    break;

            }//fin switch

            byte[] buffer = ExportarExcelDatos(cabeceras, nombrePropiedades, lista);
            return File(buffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }


        public IActionResult ViewConstanciaIngreso(long codigoTransaccion)
        {
            TransaccionViewModel obj = new TransaccionViewModel();
            obj.CodigoTransaccion = codigoTransaccion;
            var demoViewPortrait = new ViewAsPdf("ViewConstanciaIngreso", String.Empty, obj);
            demoViewPortrait.PageMargins = new Margins { Left = 2, Right = 2};
            demoViewPortrait.PageHeight = 297;
            demoViewPortrait.PageWidth = 76;
            //demoViewPortrait.PageMargins = new Margins { Bottom = 5, Left = 5, Right = 5, Top = 5 };
            //demoViewPortrait.PageSize = Size.A5;
            //demoViewPortrait.PageOrientation = Orientation.Portrait;

            return demoViewPortrait;
        }

        public IActionResult ViewConstanciaEgreso(long codigoTransaccion)
        {
            TransaccionViewModel obj = new TransaccionViewModel();
            obj.CodigoTransaccion = codigoTransaccion;
            var demoViewPortrait = new ViewAsPdf("ViewConstanciaEgreso", String.Empty, obj);
            demoViewPortrait.PageHeight = 297;
            demoViewPortrait.PageWidth = 76;
            //demoViewPortrait.PageMargins = new Margins { Bottom = 5, Left = 5, Right = 5, Top = 5 };
            //demoViewPortrait.PageSize = Size.Letter;

            return demoViewPortrait;
        }

        public IActionResult ViewReporteCompromisoFiscal(int anioOperacion, int semanaOperacion)
        {
            ReportContabilidadViewModel obj = new ReportContabilidadViewModel();
            obj.AnioOperacion = anioOperacion;
            obj.SemanaOperacion = semanaOperacion;
            var demoViewPortrait = new ViewAsPdf("ViewReporteCompromisoFiscal", String.Empty, obj);
            demoViewPortrait.PageMargins = new Margins { Bottom = 5, Left = 5, Right = 5, Top = 5 };
            demoViewPortrait.PageSize = Size.Letter;

            return demoViewPortrait;
        }


    }

}
