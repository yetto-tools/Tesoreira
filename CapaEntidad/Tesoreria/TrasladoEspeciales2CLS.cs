using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Tesoreria
{
    public class TrasladoEspeciales2CLS
    {
        public int CodigoTraslado { get; set; }
        public DateTime FechaOperacion { get; set; }
        public string FechaOperacionStr { get; set; }
        public DateTime FechaRecepcion { get; set; }
        public string FechaRecepcionStr { get; set; }
        public string ObservacionesRecepcion { get; set; }
        public int CodigoEstado { get; set; }
        public string Estado { get; set; }
        public string UsuarioIngreso { get; set; }
    }
}
