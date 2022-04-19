using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Tesoreria
{
    public class EntidadCLS
    {
        public int CodigoEntidad { get; set; }
        public short CodigoCategoriaEntidad { get; set; }
        public string NombreCategoriaEntidad { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public byte Estado { get; set; }
        public string UsuarioIng { get; set; }
        public DateTime FechaIng { get; set; }

        public short CodigoOperacion { get; set; }
        public string Operacion { get; set; }

        public byte PermisoEditar { get; set; }

    }
}
