using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.RRHH
{
    public class SuspensionCLS
    {
        public int CodigoSuspension { get; set; }
        public short CodigoEmpresa { get; set; }
        public string CodigoEmpleado { get; set; }
        public DateTime FechaInicioSuspension { get; set; }
        public string FechaInicioSuspensionStr { get; set; }
        public DateTime FechaFinSuspension { get; set; }
        public string FechaFinSuspensionStr { get; set; }
        public short CodigoMotivoSuspension { get; set; }
        public string Observaciones { get; set; }
        public DateTime? FechaAlta { get; set; }
        public byte Estado { get; set; }
        public string UsuarioIng { get; set; }
        public DateTime FechaIng { get; set; }
        public string UsuarioAct { get; set; }
        public DateTime? FechaAct { get; set; }

        public short CodigoMotivoBaja { get; set; }
        public DateTime FechaEgreso { get; set; }
        public string FechaEgresoStr { get; set; }

    }
}
