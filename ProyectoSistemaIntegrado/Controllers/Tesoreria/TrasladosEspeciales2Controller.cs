using CapaDatos.Tesoreria;
using CapaEntidad.Administracion;
using CapaEntidad.Tesoreria;
using CapaNegocio.Tesoreria;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Controllers.Tesoreria
{
    public class TrasladosEspeciales2Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public List<TrasladoEspeciales2DetalleCLS> GetTrasladosParaDepuracion(int codigoTraslado)
        {
            Especiales2DAL obj = new Especiales2DAL();
            return obj.GetTrasladosParaDepuracion(codigoTraslado);
        }

        public string ActualizarDetalleEspeciales2([FromBody] List<TrasladoEspeciales2DetalleCLS> listDetalle, int codigoTraslado)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            Especiales2DAL obj = new Especiales2DAL();
            return obj.ActualizarDetalleEspeciales2(listDetalle, codigoTraslado, objUsuario.IdUsuario);
        }

        public List<TrasladoEspeciales2DetalleCLS> GetDetalleEspeciales2(int codigoTraslado)
        {
            Especiales2BL obj = new Especiales2BL();
            return obj.GetDetalleEspeciales2(codigoTraslado);
        }

        public string RegistrarEspeciales2(List<TrasladoEspeciales2DetalleCLS> listDetalle, int codigoTraslado, DateTime fechaOperacion, int semanaOperacion, int anioOperacion)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            Especiales2DAL obj = new Especiales2DAL();
            return obj.RegistrarEspeciales2(listDetalle, codigoTraslado, fechaOperacion, semanaOperacion, anioOperacion, objUsuario.IdUsuario);
        }


    }
}
