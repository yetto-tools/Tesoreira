using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Tesoreria
{
    public class TipoOperacionCLS
    {
        public short CodigoTipoOperacion { get; set; }
        public byte CodigoOrigen { get; set; }
        public string Nombre { get; set; }
        public string IdTipoOperacion { get; set; }
        public short Signo { get; set; }
        public string Descripcion { get; set; }
        public byte Estado { get; set; }
        public string UsuarioIng { get; set; }
        public DateTime FechaIng { get; set; }
        public decimal MontoTotalSemana { get; set; }
        public decimal MontoTotalLunes { get; set; }
        public decimal MontoTotalMartes { get; set; }
        public decimal MontoTotalMiercoles { get; set; }
        public decimal MontoTotalJueves { get; set; }
        public decimal MontoTotalViernes { get; set; }
        public decimal MontoTotalSabado { get; set; }
        public decimal MontoTotalDomingo { get; set; }
        public short CodigoBancoDeposito { get; set; }
        public string NumeroCuenta { get; set; }
        public short CodigoEmpresa { get; set; }

    }
}
