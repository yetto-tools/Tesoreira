using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapaEntidad.Administracion;
using CapaNegocio.Administracion;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoSistemaIntegrado.Controllers.Administracion
{
    public class TipoNotificacionController : Controller
    {
        public List<TipoNotificacionCLS> GetAllTipoNotificacion()
        {
            TipoNotificacionBL obj = new TipoNotificacionBL();
            return obj.GetAllTipoNotificacion();
        }
    }
}
