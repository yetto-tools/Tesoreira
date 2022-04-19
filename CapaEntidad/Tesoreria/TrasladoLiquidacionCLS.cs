using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Tesoreria
{
    public class TrasladoLiquidacionCLS
    {
        public long CodigoTraslado { get; set; }
        public DateTime FechaGeneracion { get; set; }

        public string FechaGeneracionStr { get; set; }

        public string UsuarioGeneracion { get; set; }

        public short AnioOperacion { get; set; }
        public byte SemanaOperacion { get; set; }

        public string  Periodo { get; set; }

        public int Cantidad { get; set; }

        public decimal MontoTotal { get; set; }

        public byte CodigoEstadoTraslado { get; set; }

        public string EstadoTraslado { get; set; }

        public byte bloqueado { get; set; }
        public byte PermisoAnular { get; set; }
    }
}
