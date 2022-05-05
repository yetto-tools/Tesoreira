using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Contabilidad
{
    public class CuentaPorCobrarReporteDetalleCLS
    {
        public int CodigoReporte { get; set; }
        public short AnioOperacion { get; set; }
        public byte SemanaOperacion { get; set; }
        public short CodigoOperacion { get; set; }
        public string Operacion { get; set; }
        public string CodigoEntidad { get; set; }
        public string NombreEntidad { get; set; }

        public string NombreEmpresa { get; set; }
        public short CodigoCategoria { get; set; }
        public string Categoria { get; set; }
        public decimal SaldoInicial { get; set; }
        public decimal MontoSolicitado { get; set; }
        public decimal MontoDevolucion { get; set; }
        public decimal SaldoFinal { get; set; }
    }
}
