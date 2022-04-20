using CapaEntidad.Ventas;
using CapaNegocio.Ventas;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Controllers.Ventas
{
    public class ClienteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public List<ClienteCLS> GetListAllClientes()
        {
            ClienteBL obj = new ClienteBL();
            return obj.GetListAllClientes();
        }
    }


}
