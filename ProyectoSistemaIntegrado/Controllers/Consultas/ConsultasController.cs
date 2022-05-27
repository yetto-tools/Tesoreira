using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Controllers.Consultas
{
    public class ConsultasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ConsultaTransacciones()
        {
            return View();
        }
        public IActionResult ConsultaTransaccionesVendedores()
        {
            return View();
        }

    }
}
