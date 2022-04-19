using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Tesoreria
{
    public class SolicitudCorreccionCLS
    {
        public long CodigoTransaccion { get; set; }

        public long? CodigoTransaccionCorrecta { get; set; }

        public string ObservacionesSolicitud { get; set; }
        public string ObservacionesAprobacion { get; set; }
        public byte codigoResultado { get; set; }
        public string Resultado { get; set; }
        public string UsuarioAprobacion { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public string FechaAprobacionStr { get; set; }
        public string UsuarioIng { get; set; }
        public DateTime FechaIng { get; set; }
        public string FechaIngStr { get; set; }
        public string UsuarioAct { get; set; }
        public DateTime? FechaAct { get; set; }
        public byte CodigoTipoCorreccion { get; set; }
        public string TipoCorreccion { get; set; }

    }
}
