using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Administracion
{
    public class RolCLS
    {
        public int CodigoRol { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Asignado { get; set; }

        public int PermisoAnular { get; set; }

        public int PermisoEditar { get; set; }

    }
}
