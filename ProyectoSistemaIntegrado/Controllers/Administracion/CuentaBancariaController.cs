using CapaEntidad.Administracion;
using CapaNegocio.Administracion;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Controllers.Administracion
{
    public class CuentaBancariaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public List<CuentaBancariaCLS> GetCuentasBancarias(int codigoBanco)
        {
            CuentaBancariaBL obj = new CuentaBancariaBL();
            return obj.GetCuentasBancarias(codigoBanco);
        }

        public List<CuentaBancariaCLS> GetCuentasBancariasTesoreria(int codigoBanco)
        {
            CuentaBancariaBL obj = new CuentaBancariaBL();
            return obj.GetCuentasBancariasTesoreria(codigoBanco);
        }

        public string GetComboCuentasBancarias(int codigoBanco)
        {
            CuentaBancariaBL obj = new CuentaBancariaBL();
            return obj.GetComboCuentasBancarias(codigoBanco);
        }

    }
}
