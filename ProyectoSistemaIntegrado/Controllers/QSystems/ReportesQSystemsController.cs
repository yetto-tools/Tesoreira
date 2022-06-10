using CapaEntidad.QSystems;
using CapaNegocio.QSystems;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Controllers.QSystems
{
    public class ReportesQSystemsController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public List<FacturaVentasCLS> GetListaVentaPorRangoFechaDetallado(string codigoEmpresa, string fechaInicio, string fechaFin)
        {
            ReportesBL obj = new ReportesBL();
            return obj.GetListaVentaPorRangoFechaDetallado(codigoEmpresa, fechaInicio, fechaFin);
        }

        public FileResult ExportarExcelVentaPorRangoFechaDetallado(string codigoEmpresa, string fechaInicio, string fechaFin)
        {
            string[] cabeceras = new string[] { };
            string[] nombrePropiedades = new string[] { };
            List<FacturaVentasCLS> lista = null;

            ReportesBL obj = new ReportesBL();
            lista = obj.GetListaVentaPorRangoFechaDetallado(codigoEmpresa, fechaInicio, fechaFin);
            cabeceras = new string[24] { "veh_fecha","DiaSemana","Tienda","Caja","Serie","NumFactura","Fel_Serie","Fel_Numero","CodCliente","ClienteNombre","ClienteNit","FacturadoA","CodVendedor","NombreVendedor","FormaDePago","BIEN_SERVICIO","TotalSinIVA","TotalIVA","TOTALCONIVA","NUMLINEA","CODINVENTARIO","CANTIDAD","PRECIOUNITARIO", "TOTXARTICULO" };
            nombrePropiedades = new string[24] { "FechaFactura", "DiaSemana", "CodigoTienda", "CodigoCaja", "SerieFactura", "NumeroFactura", "SerieFacturaFEL", "NumeroFacturaFEL", "CodigoCliente", "NombreCliente", "NitCliente", "FacturadoA", "CodigoVendedor", "NombreVendedor", "FormaPago", "Clasificacion", "TotalSinIVA", "TotalIVA", "TotalConIVA", "NumeroLinea", "CodigoSKU", "Cantidad", "PrecioUnitario", "TotalPorArticulo" };

            byte[] buffer = ExportarExcelDatos(cabeceras, nombrePropiedades, lista);
            return File(buffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public List<ValeSalidaCLS> GetListaValesDeSalida(string codigoEmpresa, string fechaInicio, string fechaFin)
        {
            ReportesBL obj = new ReportesBL();
            return obj.GetListaValesDeSalida(codigoEmpresa, fechaInicio, fechaFin);
        }

        public FileResult ExportarExcelValesSalidaPorRangoFechaDetallado(string codigoEmpresa, string fechaInicio, string fechaFin)
        {
            string[] cabeceras = new string[] { };
            string[] nombrePropiedades = new string[] { };
            List<ValeSalidaCLS> lista = null;

            ReportesBL obj = new ReportesBL();
            lista = obj.GetListaValesDeSalida(codigoEmpresa, fechaInicio, fechaFin);

            cabeceras = new string[10] { "FechaEmision", "DiaSemana","Vendedor", "Nombre","ValeNumero","Linea","Codigo","Descripcion","PrecioUnit", "Cantidad" };
            nombrePropiedades = new string[10] { "FechaEmisionStr", "DiaSemana", "Ruta", "NombreVendedor", "NumeroVale", "NumeroLinea", "CodigoInventario", "Descripcion", "PrecioUnitario", "Cantidad"};

            byte[] buffer = ExportarExcelDatos(cabeceras, nombrePropiedades, lista);
            return File(buffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }



    }
}
