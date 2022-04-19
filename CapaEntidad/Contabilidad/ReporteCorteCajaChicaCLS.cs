using CapaEntidad.Administracion;
using CapaEntidad.Tesoreria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Contabilidad
{
    public class ReporteCorteCajaChicaCLS
    {
        public List<ProgramacionSemanalCLS> periodoOperacion { get; set; }

        public ReporteCajaChicaCLS encabezado { get; set; }

        public List<CajaChicaCLS> listaTransacciones { get; set; }

    }
}
