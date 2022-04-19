using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.RRHH
{
    public class TipoBackToBackCLS
    {
        public byte CodigoTipoBackToBack { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public byte Estado { get; set; }
        public string UsuarioIng { get; set; }
        public DateTime FechaIng { get; set; }
    }
}
