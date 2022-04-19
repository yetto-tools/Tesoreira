using CapaEntidad.Contabilidad;
using CapaEntidad.Tesoreria;
using CapaNegocio.Contabilidad;
using CapaNegocio.Tesoreria;
using Microsoft.AspNetCore.Mvc;
using ProyectoSistemaIntegrado.Models;
using ProyectoSistemaIntegrado.Rotativa;
using Rotativa.AspNetCore.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Controllers.Contabilidad
{
    public class ReportesTesoreriaController : Controller
    {
        /*public IActionResult Index()
        {
            return View();
        }*/

        public IActionResult DesglocePagoPlanillas()
        {
            return View();
        }

        public IActionResult VistoBuenoReporteCaja()
        {
            return View();
        }

        public List<ReporteCajaCLS> GetReportesSemanalesCajaParaVistoBueno()
        {
            ReporteCajaBL obj = new ReporteCajaBL();
            return obj.GetReportesSemanalesCajaParaVistoBueno();
        }

    }
}
