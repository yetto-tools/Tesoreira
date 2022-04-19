using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Administracion
{
    public class CuentaBancariaCLS
    {
        public short CodigoBanco { get; set; }
        public string NumeroCuenta { get; set; }
        public string NumeroCuentaDescriptivo { get; set; }
        public byte CodigoTipoCuenta { get; set; }
        public string NombreCuenta { get; set; }
        public string Descripcion { get; set; }
        public byte Tesoreria { get; set; }
        public byte Estado { get; set; }
        public string UsuarioIng { get; set; }
        public DateTime FechaIng { get; set; }
    }
}
