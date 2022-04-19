using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Ventas
{
    public class RutaCLS
    {
        public short Ruta { get; set; }
        public string Nombre { get; set; }
        public string NombreCorto { get; set; }
        public byte CodigoTipoRuta { get; set; }
        public string TipoRuta { get; set; }
        public short CodigoCanalVenta { get; set; }
        public string CanalVenta { get; set; }
        public byte MigracionCompleta { get; set; }
        public string Descripcion { get; set; }
        public string DescripcionMigracion { get; set; }
        public byte CodigoEstado { get; set; }
        public string Estado { get; set; }
        public int PermisoAnular { get; set; }
        public int PermisoEditar { get; set; }

    }
}
