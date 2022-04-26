using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Contabilidad
{
    public class ReporteResumenOperacionesCajaCLS
    {
        public string IdTipoOperacion { get; set; }
        public string TipoOperacion { get; set; }
        public short CodigoOperacion { get; set; }
        public string Operacion { get; set; }
        public decimal MontoLunes { get; set; }
        public decimal MontoMartes { get; set; }
        public decimal MontoMiercoles { get; set; }
        public decimal MontoJueves { get; set; }
        public decimal MontoViernes { get; set; }
        public decimal MontoSabado { get; set; }
        public decimal MontoDomingo { get; set; }
        public decimal MontoSemana { get; set; }
    }
}
