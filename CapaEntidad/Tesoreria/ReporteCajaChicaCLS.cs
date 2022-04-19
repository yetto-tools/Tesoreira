using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Tesoreria
{
    public class ReporteCajaChicaCLS
    {
        public int CodigoReporte { get; set; }
        public short CodigoCajaChica { get; set; }
        public string NombreCajaChica { get; set; }
        public short AnioOperacion { get; set; }
        public byte SemanaOperacion { get; set; }
        public string Periodo { get; set; }

        public decimal Monto { get; set; }
        public decimal MontoFiscal { get; set; }
        public decimal MontoNoFiscal { get; set; }
        public decimal MontoDisponible { get; set; }
        public decimal MontoEgreso { get; set; }
        public decimal MontoSaldo { get; set; }
        public string Observaciones { get; set; }
        public byte CodigoEstado { get; set; }
        public string Estado { get; set; }
        public byte Bloqueado { get; set; }
        public byte PermisoAnular { get; set; }
        public byte PermisoEditar { get; set; }

        public decimal MontoReembolsoCalculado { get; set; }
        public decimal MontoReembolso { get; set; }

        public byte PermisoCorregir { get; set; }

        public string FechaCorteStr { get; set; }
        public string NombreControlador { get; set; }
        public string NombreAccion { get; set; }
        public byte Pdf { get; set; }
        public byte Excel { get; set; }
        public byte Web { get; set; }

        public byte Arqueo { get; set; }
        public string Tipo { get; set; }

        public int CodigoTipoReporte { get; set; }
        public string UsuarioIng { get; set; }

        public DateTime FechaIng { get; set; }

        public string FechaIngStr { get; set; }

    }
}
