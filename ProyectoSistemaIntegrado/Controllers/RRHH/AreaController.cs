using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapaEntidad.RRHH;
using CapaNegocio.RRHH;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoSistemaIntegrado.Controllers.RRHH
{
    public class AreaController : Controller
    {

        public List<AreaCLS> GetAllAreas()
        {
            AreaBL obj = new AreaBL();
            return obj.GetAllAreas();
        }


    }
}
