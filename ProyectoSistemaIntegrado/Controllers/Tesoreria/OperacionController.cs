using CapaEntidad.Tesoreria;
using CapaNegocio.Tesoreria;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Controllers.Tesoreria
{
    public class OperacionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public OperacionComboCLS FillComboOperacion(int codigoTipoOperacion)
        {
            OperacionBL obj = new OperacionBL();
            return obj.FillComboOperacion(codigoTipoOperacion);
        }

        public List<OperacionCLS> GetOperacionesParaAsignacionAEntidadesGenericas()
        {
            OperacionBL obj = new OperacionBL();
            return obj.GetOperacionesParaAsignacionAEntidadesGenericas();
        }

        public List<OperacionCLS> GetListOperacionesCajaChica()
        {
            OperacionBL obj = new OperacionBL();
            return obj.GetListOperacionesCajaChica();
        }

    }
}
