using CapaEntidad.Administracion;
using CapaEntidad.Tesoreria;
using CapaEntidad.Ventas;
using CapaNegocio.Tesoreria;
using CapaNegocio.Ventas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProyectoSistemaIntegrado.Filter;
using ProyectoSistemaIntegrado.Models;
using ProyectoSistemaIntegrado.Rotativa;
using Rotativa.AspNetCore.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Controllers.Tesoreria
{
    public class TransaccionController : BaseController
    {
        [ServiceFilter(typeof(Seguridad))]
        
        public IActionResult Index()
        {
            return View();
        }

        public static List<TransaccionCLS> lista;

        public IActionResult ConsultaTransacciones()
        {
            return View();
        }

        public IActionResult New()
        {
            return View();
        }

        public IActionResult TrasladoSemanaOperacion()
        {
            return View();
        }

        public IActionResult Revision()
        {
            return View();
        }


        public IActionResult SolicitudesCorreccion()
        {
            return View();
        }

        public IActionResult Edit(Int64 slug)
        {
            return View();
        }

        public IActionResult ComplementoDepositosBancarios()
        {
            return View();
        }

        public IActionResult ComplementoEmpresaGastos()
        {
            return View();
        }

        public IActionResult EditRevision(Int64 slug)
        {
            return View();
        }

        public IActionResult ConsultaCorrecciones()
        {
            return View();
        }

        //public IActionResult ViewConstanciaTransaccion(int codigoTransaccion)
        //{
        //    TransaccionViewModel obj = new TransaccionViewModel();
        //    obj.CodigoTransaccion = codigoTransaccion;

        //    //var demoViewPortrait = new ViewAsPdf("DemoViewAsPDF")
        //    var demoViewPortrait = new ViewAsPdf("ViewConstanciaTransaccion", String.Empty, obj);
        //    //demoViewPortrait.PageSize = Rotativa.Options.Size.A6;
        //    demoViewPortrait.PageSize = Size.A5;
        //    //demoViewPortrait.IsGrayScale = true;
        //    //demoViewPortrait.PageMargins = new Margins { Bottom = 5, Left = 10, Right = 10, Top = 5 };
        //    //demoViewPortrait.PageOrientation = Orientation.Portrait;
        //    demoViewPortrait.PageOrientation = Orientation.Landscape;

        //    return demoViewPortrait;
        //    //return new ViewAsPdf("ViewConstanciaTransaccion", String.Empty, obj);
        //}


        public decimal GetMontoPlanillaParaDesglosar(int anioOperacion, int semanaOperacion, int codigoReporte)
        {
            TransaccionBL obj = new TransaccionBL();
            return obj.GetMontoPlanillaParaDesglosar(anioOperacion, semanaOperacion, codigoReporte);
        }

        public List<TransaccionCLS> BuscarTransacciones(int codigoOperacion, int codigoCategoriaEntidad, int diaOperacion)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            TransaccionBL obj = new TransaccionBL();
            return obj.BuscarTransacciones(objUsuario.IdUsuario, codigoOperacion, codigoCategoriaEntidad, diaOperacion, objUsuario.SuperAdmin, objUsuario.SetSemanaAnterior);
        }


        //public List<TransaccionCLS> BuscarTransaccionesEdicion(string idUsuario, int codigoOperacion, int codigoCategoriaEntidad)
        //{
        //    ViewBag.Message = HttpContext.Session.GetString("usuario");
        //    UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);
        //    TransaccionBL obj = new TransaccionBL();
        //    return obj.BuscarTransaccionesEdicion(objUsuario.IdUsuario, codigoOperacion, codigoCategoriaEntidad);
        //}


        public List<TransaccionCLS> BuscarTransaccionesConsulta(int anioOperacion, int semanaOperacion, int codigoReporte, int codigoOperacion, int codigoCategoriaEntidad, int diaOperacion)
        {
            TransaccionBL obj = new TransaccionBL();
            return obj.BuscarTransaccionesConsulta(anioOperacion, semanaOperacion, codigoReporte, codigoOperacion, codigoCategoriaEntidad, diaOperacion);
        }

        public List<TransaccionCLS> BuscarTransaccionesConsultaContabilidad(int anioOperacion, int semanaOperacion, int codigoTipoOperacion, int codigoOperacion, int codigoCategoriaEntidad, string nombreEntidad, string fechaInicioStr, string fechaFinStr)
        {
            TransaccionBL obj = new TransaccionBL();
            return obj.BuscarTransaccionesConsultaContabilidad(anioOperacion, semanaOperacion, codigoTipoOperacion, codigoOperacion, codigoCategoriaEntidad, nombreEntidad, fechaInicioStr, fechaFinStr);
        }

        public List<TransaccionCLS> BuscarTransaccionesParaCorreccion(int anioOperacion, int semanaOperacion, int codigoReporte, int codigoOperacion, int codigoCategoriaEntidad)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            TransaccionBL obj = new TransaccionBL();
            return obj.BuscarTransaccionesParaCorreccion(anioOperacion, semanaOperacion, codigoReporte, codigoOperacion, codigoCategoriaEntidad, objUsuario.SuperAdmin);
        }

        public List<TransaccionCLS> BuscarTransaccionesDepositosBancarios(int anioOperacion, int semanaOperacion, int codigoReporte)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            TransaccionBL obj = new TransaccionBL();
            return obj.BuscarTransaccionesDepositosBancarios(anioOperacion, semanaOperacion, codigoReporte, objUsuario.SuperAdmin);
        }

        public List<TransaccionCLS> BuscarTransaccionesGasto(int anioOperacion, int semanaOperacion, int codigoReporte, int esSuperAdmin)
        {
            TransaccionBL obj = new TransaccionBL();
            return obj.BuscarTransaccionesGasto(anioOperacion, semanaOperacion, codigoReporte, esSuperAdmin);
        }

        public List<TransaccionCLS> GetSolicitudesDeCorreccion(int anioOperacion, int semanaOperacion, int codigoReporte, int codigoOperacion, int codigoCategoriaEntidad)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            TransaccionBL obj = new TransaccionBL();
            return obj.GetSolicitudesDeCorreccion(anioOperacion, semanaOperacion, codigoReporte, codigoOperacion, codigoCategoriaEntidad, objUsuario.SuperAdmin);
        }

        public string AutorizarCorreccion(long codigoTransaccion, string observaciones, int codigoResultado)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            TransaccionBL obj = new TransaccionBL();
            return obj.AutorizarCorreccion(codigoTransaccion, observaciones, codigoResultado, objUsuario.IdUsuario);
        }


        public List<TransaccionCLS> BuscarTransaccionesDesglocePagoPlanillas()
        {
            TransaccionBL obj = new TransaccionBL();
            return obj.BuscarTransaccionesDesglocePagoPlanillas();
        }

        public List<TransaccionCLS> BuscarTransaccionesReporteFacturadoAlContado()
        {
            TransaccionBL obj = new TransaccionBL();
            return obj.BuscarTransaccionesReporteFacturadoAlContado();
        }

        public TransaccionComboCLS FillCombosNuevaTransaccion(int codigoTipoOperacion)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            TransaccionBL obj = new TransaccionBL();
            return obj.FillCombosNuevaTransaccion(codigoTipoOperacion, objUsuario.IdUsuario);
        }

        public TransaccionComboCLS FillCombosEditTransaccion(int codigoTipoOperacion, int semanaOperacion, int anioOperacion)
        {
            TransaccionBL obj = new TransaccionBL();
            return obj.FillCombosEditTransaccion(codigoTipoOperacion, semanaOperacion, anioOperacion);
        }

        public TransaccionComboCLS FillCombosConsultaTransacciones()
        {
            TransaccionBL obj = new TransaccionBL();
            return obj.FillCombosConsultaTransacciones();
        }

        public TransaccionComboCLS FillComboSemana(int habilitarSemanaAnterior)
        {
            TransaccionBL obj = new TransaccionBL();
            return obj.FillComboSemana(habilitarSemanaAnterior);
        }

        public string GuardarDatos(TransaccionCLS objTransaccion, int complemento)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            if (objUsuario.SetSemanaAnterior == 0) {
                objTransaccion.NumeroReciboReferencia = -1;
            }
            TransaccionBL obj = new TransaccionBL();
            return obj.GuardarTransaccion(objTransaccion, objUsuario.IdUsuario, complemento);
        }

        public string ActualizarTransaccion(TransaccionCLS objTransaccion)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            TransaccionBL obj = new TransaccionBL();
            return obj.ActualizarTransaccion(objTransaccion, objUsuario.IdUsuario);
        }

        public string RegistrarSolicitudAprobacionDeCorreccion(long codigoTransaccion, int codigoTipoCorreccion, string observaciones)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            TransaccionBL obj = new TransaccionBL();
            return obj.RegistrarSolicitudAprobacionDeCorreccion(codigoTransaccion, codigoTipoCorreccion, observaciones, objUsuario.IdUsuario);
        }

        public string RegistrarCorreccion(TransaccionCLS objTransaccion)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            TransaccionBL obj = new TransaccionBL();
            return obj.RegistrarCorreccion(objTransaccion, objUsuario.IdUsuario);
        }

        public TransaccionCLS GetDataTransaccion(long codigoTransaccion)
        {
            TransaccionBL obj = new TransaccionBL();
            return obj.GetDataTransaccion(codigoTransaccion);
        }

        public List<EntidadGenericaCLS> ListarEntidadesGenericasEmpty(int codigoEmpresa, string nombreEntidad)
        {
            List<EntidadGenericaCLS> lista = new List<EntidadGenericaCLS>();
            return lista;
        }

        public List<EntidadGenericaCLS> ListarEntidadesGenericas()
        {
            EntidadBL obj = new EntidadBL();
            return obj.ListarEntidadesGenericas();
        }

        public List<ContribuyenteCLS> FillEmpresasConcecionIVA()
        {
            TransaccionBL obj = new TransaccionBL();
            return obj.GetEmpresasConcecionIVA();
        }

        public List<VendedorRutaCLS> GetListaVendedores()
        {
            VendedorRutaBL obj = new VendedorRutaBL();
            return obj.GetListaVendedores();
        }

        public string AnularTransaccion(long codigoTransaccion, int codigoOperacion, long codigoCuentaPorCobrar, string observaciones)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            TransaccionBL obj = new TransaccionBL();
            return obj.AnularTransaccion(codigoTransaccion, codigoOperacion, codigoCuentaPorCobrar, observaciones, objUsuario.IdUsuario);
        }

        public string AnularTransaccionComplementoContabilidad(long codigoTransaccion)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            TransaccionBL obj = new TransaccionBL();
            return obj.AnularTransaccionComplementoContabilidad(codigoTransaccion, objUsuario.IdUsuario);
        }

        public string AceptarTransaccionComplementoContabilidad(long codigoTransaccion)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            TransaccionBL obj = new TransaccionBL();
            return obj.AceptarTransaccionComplementoContabilidad(codigoTransaccion, objUsuario.IdUsuario);
        }

        public List<ReporteCajaCLS> GetTransaccionParaCambioDeSemanaOperacion()
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            TransaccionBL obj = new TransaccionBL();

            return obj.GetTransaccionParaCambioDeSemanaOperacion(objUsuario.IdUsuario);
        }

        public string CambiarSemanaOperacionTransacciones(int anioOperacion, int semanaOperacion)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            TransaccionBL obj = new TransaccionBL();
            return obj.CambiarSemanaOperacionTransacciones(anioOperacion, semanaOperacion, objUsuario.IdUsuario);
        }

        public string AceptarRevision(long codigoTransaccion, int revisado)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            TransaccionBL obj = new TransaccionBL();
            return obj.AceptarRevision(codigoTransaccion, revisado, objUsuario.IdUsuario);
        }

        public SolicitudCorreccionCLS GetDataCorreccion(long codigoTransaccion)
        {
            TransaccionBL obj = new TransaccionBL();
            return obj.GetDataCorreccion(codigoTransaccion);
        }
        public string ActualizarNumeroBoletaDeposito(long codigoTransaccion, string numeroBoletaDeposito)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            TransaccionBL obj = new TransaccionBL();
            return obj.ActualizarNumeroBoletaDeposito(codigoTransaccion, numeroBoletaDeposito, objUsuario.IdUsuario);
        }

        public FileResult ExportarExcelTransaccionesEnProceso(int codigoOperacion, int codigoCategoriaEntidad, int diaOperacion)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            TransaccionBL obj = new TransaccionBL();
            lista = obj.BuscarTransacciones(objUsuario.IdUsuario, codigoOperacion, codigoCategoriaEntidad, diaOperacion, objUsuario.SuperAdmin, objUsuario.SetSemanaAnterior);

            string[] cabeceras = { "CodigoTransaccion","CodigoTipoTransaccion","NumeroRecibo","FechaRecibo","CodigoEntidad","NombreEntidad","CodigoCategoriaEntidad",
            "CategoriaEntidad","CodigoOperacion","Operacion","TipoCuentaPorCobrar","CodigoArea","Area","FechaOperacion","FechaStr","DiaOperacion","NombreDiaOperacion","Monto","CodigoEstado","Estado","FechaIng",
            "UsuarioIng","Ruta","ComplementoConta","Revisado","CodigoTransaccionAnt","Correccion"};
            string[] nombrePropiedades = { "CodigoTransaccion","CodigoTipoTransaccion","NumeroRecibo","FechaRecibo","CodigoEntidad","NombreEntidad","CodigoCategoriaEntidad",
            "CategoriaEntidad","CodigoOperacion","Operacion","TipoCuentaPorCobrar","CodigoArea","Area","FechaOperacion","FechaStr","DiaOperacion","NombreDiaOperacion","Monto","CodigoEstado","Estado","FechaIng",
            "UsuarioIng","Ruta","ComplementoConta","Revisado","CodigoTransaccionAnt","Correccion"};

            byte[] buffer = ExportarExcelDatos(cabeceras, nombrePropiedades, lista);
            return File(buffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }


        public FileResult ExportarExcelTransaccionRevision(int anioOperacion, int semanaOperacion, int codigoReporte, int codigoOperacion, int codigoCategoriaEntidad)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            TransaccionBL obj = new TransaccionBL();
            lista = obj.BuscarTransaccionesParaCorreccion(anioOperacion, semanaOperacion, codigoReporte, codigoOperacion, codigoCategoriaEntidad, objUsuario.SuperAdmin);

            string[] cabeceras = { "CodigoTransaccion","CodigoTipoTransaccion","NumeroRecibo","FechaRecibo","CodigoEntidad","NombreEntidad","CodigoCategoriaEntidad",
            "CategoriaEntidad","CodigoOperacion","Operacion","TipoCuentaPorCobrar","CodigoArea","Area","FechaOperacion","FechaStr","DiaOperacion","NombreDiaOperacion","Monto","CodigoEstado","Estado","FechaIng",
            "UsuarioIng","Ruta","ComplementoConta","Revisado","CodigoTransaccionAnt","Correccion"};
            string[] nombrePropiedades = { "CodigoTransaccion","CodigoTipoTransaccion","NumeroRecibo","FechaRecibo","CodigoEntidad","NombreEntidad","CodigoCategoriaEntidad",
            "CategoriaEntidad","CodigoOperacion","Operacion","TipoCuentaPorCobrar","CodigoArea","Area","FechaOperacion","FechaStr","DiaOperacion","NombreDiaOperacion","Monto","CodigoEstado","Estado","FechaIng",
            "UsuarioIng","Ruta","ComplementoConta","Revisado","CodigoTransaccionAnt","Correccion"};

            byte[] buffer = ExportarExcelDatos(cabeceras, nombrePropiedades, lista);
            return File(buffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public FileResult ExportarExcelTransaccionConsulta(int anioOperacion, int semanaOperacion, int codigoReporte, int codigoOperacion, int codigoCategoriaEntidad, int diaOperacion)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            TransaccionBL obj = new TransaccionBL();
            lista = obj.BuscarTransaccionesConsulta(anioOperacion, semanaOperacion, codigoReporte, codigoOperacion, codigoCategoriaEntidad, diaOperacion);

            string[] cabeceras = { "CodigoTransaccion","CodigoTipoTransaccion","NumeroRecibo","FechaRecibo","CodigoEntidad","NombreEntidad","CodigoCategoriaEntidad",
            "CategoriaEntidad","CodigoOperacion","Operacion","TipoCuentaPorCobrar","CodigoArea","Area","FechaOperacion","FechaStr","DiaOperacion","NombreDiaOperacion","Monto","CodigoEstado","Estado","FechaIng",
            "UsuarioIng","Ruta","ComplementoConta","Revisado","CodigoTransaccionAnt","Correccion"};
            string[] nombrePropiedades = { "CodigoTransaccion","CodigoTipoTransaccion","NumeroRecibo","FechaRecibo","CodigoEntidad","NombreEntidad","CodigoCategoriaEntidad",
            "CategoriaEntidad","CodigoOperacion","Operacion","TipoCuentaPorCobrar","CodigoArea","Area","FechaOperacion","FechaStr","DiaOperacion","NombreDiaOperacion","Monto","CodigoEstado","Estado","FechaIng",
            "UsuarioIng","Ruta","ComplementoConta","Revisado","CodigoTransaccionAnt","Correccion"};

            byte[] buffer = ExportarExcelDatos(cabeceras, nombrePropiedades, lista);
            return File(buffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public FileResult ExportarExcelTransaccionConsultaContabilidad(int anioOperacion, int semanaOperacion, int codigoTipoOperacion, int codigoOperacion, int codigoCategoriaEntidad, string nombreEntidad, string fechaInicioStr, string fechaFinStr)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            TransaccionBL obj = new TransaccionBL();
            lista = obj.BuscarTransaccionesConsultaContabilidad(anioOperacion, semanaOperacion, codigoTipoOperacion, codigoOperacion, codigoCategoriaEntidad, nombreEntidad, fechaInicioStr, fechaFinStr);

            string[] cabeceras = { "CodigoTransaccion","CodigoTipoTransaccion","NumeroRecibo","FechaRecibo","CodigoEntidad","NombreEntidad","CodigoCategoriaEntidad",
            "CategoriaEntidad","CodigoOperacion","Operacion","TipoCuentaPorCobrar","CodigoArea","Area","FechaOperacion","FechaStr","DiaOperacion","NombreDiaOperacion","Monto","CodigoEstado","Estado","FechaIng",
            "UsuarioIng","Ruta","ComplementoConta","Revisado","CodigoTransaccionAnt","Correccion"};
            string[] nombrePropiedades = { "CodigoTransaccion","CodigoTipoTransaccion","NumeroRecibo","FechaRecibo","CodigoEntidad","NombreEntidad","CodigoCategoriaEntidad",
            "CategoriaEntidad","CodigoOperacion","Operacion","TipoCuentaPorCobrar","CodigoArea","Area","FechaOperacion","FechaStr","DiaOperacion","NombreDiaOperacion","Monto","CodigoEstado","Estado","FechaIng",
            "UsuarioIng","Ruta","ComplementoConta","Revisado","CodigoTransaccionAnt","Correccion"};

            byte[] buffer = ExportarExcelDatos(cabeceras, nombrePropiedades, lista);
            return File(buffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }


    }
}
