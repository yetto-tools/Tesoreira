using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Administracion
{
    public class MunicipioCLS
    {
        public short CodigoDepartamento { get; set; }
        public short CodigoMunicipio { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public byte Estado { get; set; }
        public string UsuarioIng { get; set; }
        public DateTime FechaIng { get; set; }

    }
}
