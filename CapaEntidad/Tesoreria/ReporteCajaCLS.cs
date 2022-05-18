using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Tesoreria
{
    public class ReporteCajaCLS
    {
        public int CodigoReporte { get; set; }

        public string NombreReporte { get; set; }
        public short Anio { get; set; }
        public byte NumeroSemana { get; set; }
        public string Semana { get; set; }
        public string Observaciones { get; set; }
        public byte CodigoEstado { get; set; }
        public string Estado { get; set; }
        public string UsuarioIng { get; set; }
        public DateTime FechaIng { get; set; }
        public string FechaIngStr { get; set; }
        public string UsuarioAct { get; set; }
        public DateTime? FechaAct { get; set; }
        public byte bloqueado { get; set; }
        public byte PermisoEditar { get; set; }
        public byte PermisoAnular { get; set; }
        public byte PermisoArqueo { get; set; }
        public string FechaCorteStr { get; set; }
        public byte Arqueo { get; set; }
        public string Tipo { get; set; }
        public int CantidadTransacciones { get; set; }
        public int CodigoTipoReporte { get; set; }
        public string NombreControlador { get; set; }
        public string NombreAccion { get; set; }
        public byte Pdf { get; set; }
        public byte Excel { get; set; }
        public byte Web { get; set; }
        public byte DeshabilitarCheck{ get; set; }


    }
}
