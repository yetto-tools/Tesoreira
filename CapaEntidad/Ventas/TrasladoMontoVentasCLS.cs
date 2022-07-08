using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Ventas
{
    public class TrasladoMontoVentasCLS
    {
        public int CodigoTraslado { get; set; }
        public byte CodigoTipoTraslado { get; set; }
        public string TipoTraslado { get; set; }
        public DateTime FechaOperacion { get; set; }

        public string FechaOperacionStr { get; set; }
        public decimal MontoEfectivo { get; set; }
        public decimal MontoCheques { get; set; }
        public decimal Monto { get; set; }
        public string FechaRecepcion { get; set; }
        public string UsuarioRecepcion { get; set; }
        public string ObservacionesGeneracion { get; set; }
        public string ObservacionesRecepcion { get; set; }
        public short AnioOperacion { get; set; }
        public byte SemanaOperacion { get; set; }
        public byte DiaOperacion { get; set; }
        public short CodigoEstado { get; set; }
        public string Estado { get; set; }
        public string FechaGeneracionStr { get; set; }
        public string UsuarioIng { get; set; }
        public int PermisoAnular { get; set; }
        public int PermisoImprimir { get; set; }
        public int PermisoTraslado { get; set; }
        public int PermisoRecepcionar { get; set; }
    }
}


