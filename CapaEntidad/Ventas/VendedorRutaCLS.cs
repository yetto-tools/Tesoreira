using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Ventas
{
    public class VendedorRutaCLS
    {
        public string CodigoVendedor { get; set; }
        public string NombreVendedor { get; set; }
        public short CodigoCanalVenta { get; set; }
        public string CanalVenta { get; set; }
        public short Ruta { get; set; }
        public short CodigoCategoriaEntidad { get; set; }
        public byte CodigoEstadoRutaVendedor { get; set; }
        public string EstadoRutaVendedor { get; set; }
        public byte PermisoAnular { get; set; }
        public byte PermisoEditar { get; set; }
    }
}
