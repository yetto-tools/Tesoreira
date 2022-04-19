using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Tesoreria
{
    public class OperacionCLS
    {
        public short CodigoOperacion { get; set; }
        public short CodigoCategoriaOperacion { get; set; }
        public short CodigoTipoOperacion { get; set; }
        public string Nombre { get; set; }
        public string NombreReporteCaja { get; set; }
        public string Descripcion { get; set; }
        public byte Estado { get; set; }
        public string UsuarioIng { get; set; }
        public DateTime FechaIng { get; set; }
    }
}
