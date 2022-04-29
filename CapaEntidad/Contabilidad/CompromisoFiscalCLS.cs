using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Contabilidad
{
    public class CompromisoFiscalCLS
    {
        public short CodigoEmpresa { get; set; }
        public string NombreEmpresa { get; set; }
        public int CodigoReporte { get; set; }
        public short AnioOperacion { get; set; }
        public byte SemanaOperacion { get; set; }
        public string Periodo { get; set; }
        public decimal MontoTotal { get; set; }
        public decimal MontoLunes { get; set; }
        public decimal MontoMartes { get; set; }
        public decimal MontoMiercoles { get; set; }
        public decimal MontoJueves { get; set; }
        public decimal MontoViernes { get; set; }
        public decimal MontoSabado { get; set; }
        public decimal MontoDomingo { get; set; }
        public string MontoTotalStr { get; set; }

        public string NombreControlador { get; set; }
        public string NombreAccion { get; set; }
    }
}
