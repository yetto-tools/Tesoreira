using CapaDatos.Administracion;
using CapaEntidad.Administracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Administracion
{
    public class NotificacionBL
    {
        public string GuardarConfiguracion(NotificacionCLS objNotificacion, string usuarioIng)
        {
            NotificacionDAL obj = new NotificacionDAL();
            return obj.GuardarConfiguracion(objNotificacion, usuarioIng);
        }

        public List<NotificacionCLS> GetAllConfiguraciones()
        {
            NotificacionDAL obj = new NotificacionDAL();
            return obj.GetAllConfiguraciones();
        }

        public string EliminarConfiguracion(string cui, int codigoTipoNotificacion)
        {
            NotificacionDAL obj = new NotificacionDAL();
            return obj.EliminarConfiguracion(cui, codigoTipoNotificacion);
        }
    }
}
