using CapaDatos.Ventas;
using CapaEntidad.Administracion;
using CapaEntidad.Ventas;
using CapaNegocio.Ventas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Controllers.Ventas
{
    public class VendedoresController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ConsultaVendedoresRuta()
        {
            return View();
        }

        public string GuardarVendedor(VendedorRutaCLS objVendedor)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            VendedorBL obj = new VendedorBL();
            return obj.GuardarVendedor(objVendedor, objUsuario.IdUsuario);
        }

        public List<VendedorCLS> GetVendedores()
        {
            VendedorBL obj = new VendedorBL();
            return obj.GetVendedores();
        }

        public List<VendedorRutaCLS> GetListaVendedores(int codigoCanalVenta)
        {
            VendedorRutaBL obj = new VendedorRutaBL();
            return obj.GetListaVendedores(codigoCanalVenta);
        }

        public string AnularConfiguracionVendedorRuta(int codigoConfiguracion)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            VendedorRutaBL obj = new VendedorRutaBL();
            return obj.AnularConfiguracionVendedorRuta(codigoConfiguracion, objUsuario.IdUsuario);
        }

        //public string BloquearVendedorRuta(string codigoVendedor, int codigoCanalVenta, int ruta)
        //{
        //    VendedorRutaDAL obj = new VendedorRutaDAL();
        //    return obj.BloquearVendedorRuta(codigoVendedor, codigoCanalVenta, ruta);
        //}

        //public string DesbloquearVendedorRuta(string codigoVendedor, int codigoCanalVenta, int ruta)
        //{
        //    VendedorRutaBL obj = new VendedorRutaBL();
        //    return obj.DesbloquearVendedorRuta(codigoVendedor, codigoCanalVenta, ruta);
        //}

        public string GuardarVendedorRuta(VendedorRutaCLS objVendedorRuta)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            VendedorRutaBL obj = new VendedorRutaBL();
            return obj.GuardarVendedorRuta(objVendedorRuta, objUsuario.IdUsuario);
        }

        public string ActualizarConfiguracionVendedorRuta(VendedorRutaCLS objVendedorRuta)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            VendedorRutaBL obj = new VendedorRutaBL();
            return obj.ActualizarConfiguracionVendedorRuta(objVendedorRuta, objUsuario.IdUsuario);
        }

        public List<VendedorRutaCLS> GetRutasDelVendedor(int codigoCategoriaEntidad, string codigoVendedor)
        {
            VendedorRutaBL obj = new VendedorRutaBL();
            return obj.GetRutasDelVendedor(codigoCategoriaEntidad, codigoVendedor);
        }

        public int ExisteConfiguracionVendedorRuta(string codigoVendedor, int codigoCanalVenta, int ruta)
        {
            VendedorRutaBL obj = new VendedorRutaBL();
            return obj.ExisteConfiguracionVendedorRuta(codigoVendedor, codigoCanalVenta, ruta);
        }



    }


}
