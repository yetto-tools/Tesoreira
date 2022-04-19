using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Administracion
{
    public class NotificacionCLS
    {
        public string Cui { get; set; }

        public string NombreCompleto { get; set; }
        public string CorreoElectronico { get; set; }
        public short CodigoTipoNotificacion { get; set; }
        public string TipoNotificacion { get; set; }
        public byte CodigoEstado { get; set; }

        public string Estado { get; set; }

        public byte PermisoAnular { get; set; }
    }
}
