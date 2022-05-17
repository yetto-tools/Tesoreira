using CapaEntidad.Administracion;
using CapaNegocio.Administracion;
using Microsoft.AspNetCore.Mvc;
using ProyectoSistemaIntegrado.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Controllers.Administracion
{
    public class EmpresaController : Controller
    {
        [ServiceFilter(typeof(Seguridad))]
        public IActionResult Index()
        {
            return View();
        }

        public List<EmpresaCLS> GetAllEmpresas()
        {
            EmpresaBL obj = new EmpresaBL();
            return obj.GetAllEmpresas();
        }

        public List<EmpresaCLS> GetAllEmpresasComercializadoras()
        {
            EmpresaBL obj = new EmpresaBL();
            return obj.GetAllEmpresasComercializadoras();
        }

        public List<EmpresaCLS> GetEmpresasTesoreriaFacturas()
        {
            EmpresaBL obj = new EmpresaBL();
            return obj.GetEmpresasTesoreriaFacturas();
        }

    }

}
