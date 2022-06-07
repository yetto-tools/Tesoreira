using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapaEntidad.Tesoreria;
using CapaNegocio.Tesoreria;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoSistemaIntegrado.Controllers.Tesoreria
{
    public class EntidadProveedoresController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public List<EntidadProveedorCLS> GetProveedores(int codigoEntidad)
        {
            EntidadProveedorBL obj = new EntidadProveedorBL();
            return obj.GetProveedores(codigoEntidad);
        }


    }
}
