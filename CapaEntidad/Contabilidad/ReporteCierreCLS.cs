using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Contabilidad
{
    public class ReporteCierreCLS
    {
        public decimal MontoReserva { get; set; }
        public decimal MontoLibre { get; set; }
        public decimal MontoFacturado { get; set; }
        public decimal MontoCompras { get; set; }
        public decimal MontoDepositos { get; set; }
        public decimal MontoSaldoAnterior { get; set; }
    }
}
