using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Administracion
{
    public class TipoReporteCLS
    {
        public int CodigoTipoReporte { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string NombreControlador { get; set; }
        public string NombreAccion { get; set; }
        public byte CodigoEstado { get; set; }
        public string Estado { get; set; }
        public byte Pdf { get; set; }
        public byte Excel { get; set; }
        public byte Web { get; set; }
        public int PermisoEditar { get; set; }
        public int PermisoAnular { get; set; }
        public string UsuarioIng { get; set; }
        public DateTime FechaIng { get; set; }
        public string UsuarioAct { get; set; }
        public DateTime? FechaAct { get; set; }
        public int Asignado { get; set; }


    }
}
