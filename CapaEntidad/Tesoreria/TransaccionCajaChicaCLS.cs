using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Tesoreria
{
    public class TransaccionCajaChicaCLS
    {
        public long CodigoTransaccion { get; set; }
        public int CodigoReporte { get; set; }
        public short? CodigoCajaChica { get; set; }
        public string NombreCajaChica { get; set; }
        public short CodigoOperacion { get; set; }
        public string NombreOperacion { get; set; }

        public short AnioOperacion { get; set; }
        public byte SemanaOperacion { get; set; }

        public short CodigoBanco { get; set; }
        public string NumeroCheque { get; set; }
        public DateTime FechaCheque { get; set; }

        public string FechaChequeStr { get; set; }
        public Decimal Monto { get; set; }
        public string Observaciones { get; set; }

        public short CodigoEstado { get; set; }

        public string Estado { get; set; }

        public string UsuarioIng { get; set; }

        public DateTime FechaIng { get; set; }

        public string FechaIngStr { get; set; }

        public string ObservacionesRecepcion { get; set; }
    }
}
