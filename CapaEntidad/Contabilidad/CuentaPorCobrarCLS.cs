using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Contabilidad
{
    public class CuentaPorCobrarCLS
    {
        public long CodigoCuentaPorCobrar { get; set; }
        public byte CodigoTipoCuentaPorCobrar { get; set; }
        public short CodigoCategoriaEntidad { get; set; }
        public short CodigoCategoria { get; set; }
        public string Categoria { get; set; }
        public string CodigoEntidad { get; set; }
        public string NombreEntidad { get; set; }
        public DateTime? FechaPrestamo { get; set; }
        public DateTime? FechaInicioPago { get; set; }
        public short AnioOperacion { get; set; }
        public byte SemanaOperacion { get; set; }
        public decimal Monto { get; set; }
        public string Observaciones { get; set; }
        public long? CodigoTransaccion { get; set; }
        public int? CodigoPlanilla { get; set; }
        public short CodigoOperacion { get; set; }
        public string Operacion { get; set; }
        public byte CodigoEstado { get; set; }
        public string UsuarioIng { get; set; }
        public DateTime FechaIng { get; set; }
        public byte CargaInicial { get; set; }

        public int CodigoPago { get; set; }
        public byte PermisoEditar { get; set; }
        public byte PermisoAnular { get; set; }
    }


}
