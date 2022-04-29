using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapaEntidad.Administracion;
using CapaEntidad.Contabilidad;
using CapaEntidad.Tesoreria;
using CapaNegocio.Contabilidad;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProyectoSistemaIntegrado.Models;
using ProyectoSistemaIntegrado.Rotativa;
using Rotativa.AspNetCore.Options;

namespace ProyectoSistemaIntegrado.Controllers.Contabilidad
{
    public class CompromisoFiscalController : Controller
    {
        public IActionResult RegistroFacturasAlContado()
        {
            return View();
        }

        public IActionResult ConsultaFacturasAlContado()
        {
            return View();
        }

        public IActionResult EdicionFacturasAlContado()
        {
            return View();
        }

        public IActionResult DetalleFacturasAlContado()
        {
            return View();
        }

        public IActionResult EdicionDetalleFacturasAlContado()
        {
            return View();
        }

        public List<CompromisoFiscalCLS> GetCompromisosFiscales(int codigoEmpresa, int anioOperacion, int semanaOperacion)
        {
            CompromisoFiscalBL obj = new CompromisoFiscalBL();
            return obj.GetCompromisosFiscales(codigoEmpresa, anioOperacion,  semanaOperacion);
        }

        public List<CompromisoFiscalDetalleCLS> GetDetalleCompromisoFiscal(int codigoEmpresa, int anioOperacion, int semanaOperacion)
        {
            CompromisoFiscalBL obj = new CompromisoFiscalBL();
            return obj.GetDetalleCompromisoFiscal(codigoEmpresa, anioOperacion, semanaOperacion);
        }

        public string CargarCompromisoFiscal([FromBody] List<TransaccionCLS> listaTransacciones)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            CompromisoFiscalBL obj = new CompromisoFiscalBL();
            return obj.CargarCompromisoFiscal(listaTransacciones, objUsuario.IdUsuario);
        }

        public CompromisoFiscalCLS GetMontoCompromisosFiscal(int anioOperacion, int semanaOperacion)
        {
            CompromisoFiscalBL obj = new CompromisoFiscalBL();
            return obj.GetMontoCompromisosFiscal(anioOperacion, semanaOperacion);
        }

    }


}
