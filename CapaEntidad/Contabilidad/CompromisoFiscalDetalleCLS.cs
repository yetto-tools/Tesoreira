using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Contabilidad
{
    public class CompromisoFiscalDetalleCLS
    {
        public long CodigoTransaccion { get; set; }
        public short CodigoEmpresa { get; set; }
        public string NombreEmpresa { get; set; }
        public short AnioOperacion { get; set; }
        public byte SemanaOperacion { get; set; }
        public byte DiaOperacion { get; set; }
        public string NombreDia { get; set; }
        public decimal Monto { get; set; }
        public short CodigoOperacion { get; set; }
        public string Operacion { get; set; }
        public string FechaIngresoStr { get; set; }
        public string UsuarioIng { get; set; }
        public byte PermisoAnular { get; set; }
        public int CodigoReporte { get; set; }
    }
}
