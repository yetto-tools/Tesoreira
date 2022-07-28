using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Planilla
{
    public class SaldoPrestamoCLS
    {
        public short CodigoEmpresa { get; set; }
        public string NombreEmpresa { get; set; }
        public string CodigoEmpleado { get; set; }
        public string NombreCompleto { get; set; }
        public short CodigoOperacion { get; set; }
        public string Operacion { get; set; }
        public short CodigoCategoria { get; set; }
        public string NombreCategoria { get; set; }
        public short CodigoOperacionDescuento { get; set; }
        public byte CodigoFrecuenciaPago { get; set; }
        public string FrecuenciaPago { get; set; }
        public decimal SaldoPendiente { get; set; }
        public decimal MontoDescuento { get; set; }
        public decimal MontoDevolucion { get; set; }

    }
}
