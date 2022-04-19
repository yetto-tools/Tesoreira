using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Contabilidad
{
    public class ReportesTesoreriaCLS
    {
        public int CodigoTipoReporte { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string NombreControlador { get; set; }
        public string NombreAccion { get; set; }
    }
}
