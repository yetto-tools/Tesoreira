using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Planilla
{
    public class PagoDescuentoCLS
    {
        public int CodigoPago { get; set; }
        public short CodigoTipoPlanilla { get; set; }
        public string TipoPlanilla { get; set; }
        public short CodigoEmpresa { get; set; }
        public string NombreEmpresa { get; set; }
        public string CodigoEmpleado { get; set; }
        public string NombreCompleto { get; set; }
        public byte CodigoFrecuenciaPago { get; set; }
        public string FrecuenciaPago { get; set; }
        public short CodigoOperacion { get; set; }
        public string Operacion { get; set; }
        public short Anio { get; set; }
        public byte Mes { get; set; }
        public byte NumeroQuincena { get; set; }
        public byte NumeroSemana { get; set; }
        public string NombreMes { get; set; }
        public decimal BonoDecreto372001 { get; set; }
        public decimal SalarioDiario { get; set; }
        public byte CodigoTipoBTB { get; set; }
        public string TipoBTB { get; set; }
        public decimal MontoDevolucionBTB { get; set; }
        public decimal MontoDescuentoPrestamo { get; set; }
        public decimal Monto { get; set; }
        public byte CodigoEstado { get; set; }
        public string UsuarioIng { get; set; }
        public DateTime FechaIng { get; set; }
        public byte PermisoAnular { get; set; }
        public string Periodo { get; set; }
        public string NumeroBoleta { get; set; }
        public decimal MontoDescuento { get; set; }
        public decimal MontoCalculado { get; set; }
        public decimal MontoPlanillaExcel { get; set; }
        public byte ExistePagoBTB { get; set; }
       
    }
}
