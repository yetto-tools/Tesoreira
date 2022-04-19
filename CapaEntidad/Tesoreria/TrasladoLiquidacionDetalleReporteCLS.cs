using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Tesoreria
{
    public class TrasladoLiquidacionDetalleReporteCLS
    {
        public string CodigoVendedor { get; set; }

        public string NombreVendedor { get; set; }

        public short Ruta { get; set; }

        public short CodigoCanalVenta { get; set; }
        public string CanalVenta { get; set; }
        public decimal MontoLunes { get; set; }
        public decimal MontoMartes { get; set; }
        public decimal MontoMiercoles { get; set; }
        public decimal MontoJueves { get; set; }
        public decimal MontoViernes { get; set; }
        public decimal MontoSabado { get; set; }
        public decimal MontoDomingo { get; set; }
        public decimal MontoTotal { get; set; }
    }
}
