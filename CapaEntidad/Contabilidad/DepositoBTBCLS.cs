using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Contabilidad
{
    public class DepositoBTBCLS
    {
        public long CodigoDepositoBTB { get; set; }
        public short CodigoTipoPlanilla { get; set; }
        public string TipoPlanilla { get; set; }
        public short AnioPlanilla { get; set; }
        public byte MesPlanilla { get; set; }
        public int CodigoReporte { get; set; }
        public string NombreMesPlanilla { get; set; }
        public short CodigoEmpresa { get; set; }
        public string NombreEmpresa { get; set; }
        public string CodigoEmpleado { get; set; }
        public string NombreEmpleado { get; set; }
        public byte CodigoFrecuenciaPago { get; set; }
        public string FrecuenciaPago { get; set; }
        public short CodigoOperacion { get; set; }
        public short AnioOperacion { get; set; }
        public byte SemanaOperacion { get; set; }
        public string Periodo { get; set; }
        public short CodigoBancoDeposito { get; set; }
        public string BancoDeposito { get; set; }
        public string NumeroCuenta { get; set; }
        public string NumeroBoleta { get; set; }
        public decimal Monto { get; set; }
        public string Observaciones { get; set; }
        public string UsuarioIng { get; set; }
        public DateTime FechaIng { get; set; }
        public string FechaIngStr { get; set; }
        public byte PermisoEditar { get; set; }
        public byte PermisoAnular { get; set; }
        public byte DiaOperacion { get; set; }

    }
}
