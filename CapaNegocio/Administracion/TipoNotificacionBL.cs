using CapaDatos.Administracion;
using CapaEntidad.Administracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Administracion
{
    public class TipoNotificacionBL
    {
        public List<TipoNotificacionCLS> GetAllTipoNotificacion()
        {
            TipoNotificacionDAL obj = new TipoNotificacionDAL();
            return obj.GetAllTipoNotificacion();
        }
    }
}
