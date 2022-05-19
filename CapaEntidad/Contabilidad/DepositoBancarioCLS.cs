using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Contabilidad
{
    public class DepositoBancarioCLS
    {
        public short CodigoBancoDeposito { get; set; }
        public string NombreBanco { get; set; }
        public string NumeroCuenta { get; set; }
        public string NumeroBoleta { get; set; }
        public decimal Monto { get; set; }
        public byte DiaOperacion { get; set; }
        public string NombreDiaOperacion { get; set; }
        public byte CodigoOrigenDeposito { get; set; }
        public string OrigenDeposito { get; set; }
    }

}
