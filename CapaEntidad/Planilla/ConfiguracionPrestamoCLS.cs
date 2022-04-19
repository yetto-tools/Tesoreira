using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Planilla
{
    public class ConfiguracionPrestamoCLS
    {
        public short CodigoEmpresa { get; set; }
        public string NombreEmpresa { get; set; }
        public string CodigoEmpleado { get; set; }
        public string NombreCompleto { get; set; }
        public byte CodigoFrecuenciaPago { get; set; }
        public string FrecuenciaPago { get; set; }
        public decimal SaldoPrestamo { get; set; }
        public decimal MontoDescuentoPrestamo { get; set; }
        public decimal BonoDecreto372001 { get; set; }
        public decimal SalarioDiario { get; set; }

    }
}
