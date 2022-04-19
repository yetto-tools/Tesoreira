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
    public class DepositosBTBController : Controller
    {
        public IActionResult RegistroDepositosBTB()
        {
            return View();
        }

        public IActionResult ConsultaDepositosBTB()
        {
            return View();
        }

        public IActionResult EdicionDepositosBTB()
        {
            return View();
        }

        public string GuardarDepositosBTB([FromBody] List<DepositoBTBCLS> listaDepositos)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            DepositoBTBBL obj = new DepositoBTBBL();
            return obj.GuardarDepositosBTB(listaDepositos, objUsuario.IdUsuario);
        }

        public string ActualizarDeposito(DepositoBTBCLS objDeposito)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            DepositoBTBBL obj = new DepositoBTBBL();
            return obj.ActualizarDeposito(objDeposito, objUsuario.IdUsuario);
        }

        public string AnularDeposito(int codigoDepositoBTB)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            DepositoBTBBL obj = new DepositoBTBBL();
            return obj.AnularDeposito(codigoDepositoBTB, objUsuario.IdUsuario);
        }

        public List<DepositoBTBCLS> GetDepositosBTB(int anioPlanilla, int anioOperacion, int semanaOperacion, int codigoReporte)
        {
            DepositoBTBBL obj = new DepositoBTBBL();
            return obj.GetDepositosBTB(anioPlanilla, anioOperacion, semanaOperacion, codigoReporte);
        }

    }


}
