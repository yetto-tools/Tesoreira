using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Tesoreria
{
    public class TrasladoLiquidacionDetalleCLS
    {
        public long CodigoTransaccion { get; set; }
        public long CodigoTransaccionAnt { get; set; }

        public long CodigoTraslado { get; set; }
        public short? CodigoEmpresa { get; set; }
        public string NombreEmpresa { get; set; }

        public DateTime FechaOperacion { get; set; }
        public string FechaOperacionStr { get; set; }

        public byte DiaOperacion { get; set; }

        public string NombreDia { get; set; }

        public string CodigoVendedor { get; set; }

        public string NombreVendedor { get; set; }

        public short Ruta { get; set; }

        public short CodigoCanalVenta { get; set; }
        public string CanalVenta { get; set; }
        public decimal Monto { get; set; }

        public byte CodigoTipoTraslado { get; set; }
        public string TipoTraslado { get; set; }

        public byte CodigoEstado { get; set; }
        public string Estado { get; set; }
        public string UsuarioIng { get; set; }
        public DateTime FechaIng { get; set; }
        public string FechaIngStr { get; set; }
        public string UsuarioAct { get; set; }

    }
}
