using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Tesoreria
{
    public class ReporteCajaDetalleCLS
    {
        public long CodigoDetalleReporte { get; set; }
        public int CodigoReporte { get; set; }
        public short CodigoConcepto { get; set; }
        public string Concepto { get; set; }
        public short CodigoOperacion { get; set; }
        public short CodigoCategoriaEntidad { get; set; }
        public string CodigoEntidad { get; set; }
        public string NombreEntidad { get; set; }
        public long? CodigoTransaccion { get; set; }
        public decimal SaldoAnterior { get; set; }
        public decimal MontoLunes { get; set; }
        public decimal MontoMartes { get; set; }
        public decimal MontoMiercoles { get; set; }
        public decimal MontoJueves { get; set; }
        public decimal MontoViernes { get; set; }
        public decimal MontoSabado { get; set; }
        public decimal MontoDomingo { get; set; }
        public byte Estado { get; set; }
        public decimal TotalSemana { get; set; }
        public decimal Devoluciones { get; set; }
        public decimal Acumulado { get; set; }
        public string Observaciones { get; set; }

    }
}
