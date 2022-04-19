using CapaEntidad.Administracion;
using CapaNegocio.Administracion;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Controllers.Administracion
{
    public class ProgramacionQuincenalController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public List<ProgramacionQuincenalCLS> GetListaQuincenas(int anio, int numeroMes)
        {
            ProgramacionQuincenalBL obj = new ProgramacionQuincenalBL();
            return obj.GetListaQuincenas(anio, numeroMes);
        }

    }


}
