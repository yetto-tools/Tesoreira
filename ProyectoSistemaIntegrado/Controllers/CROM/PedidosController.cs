using CapaEntidad.Administracion;
using CapaEntidad.CROM;
using CapaNegocio.CROM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProyectoSistemaIntegrado.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Controllers.CROM
{
    public class PedidosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
