using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapaEntidad.Administracion;
using CapaNegocio.Administracion;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoSistemaIntegrado.Controllers.Administracion
{
    public class BancoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public List<BancoCLS> GetAllBancos()
        {
            BancoBL obj = new BancoBL();
            return obj.GetAllBancos();
        }

    }


}
