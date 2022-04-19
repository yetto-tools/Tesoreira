using CapaEntidad.Administracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Tesoreria
{
    public class CajaChicaComboCLS
    {
        public List<ConfiguracionCajaChicaCLS> listaCajasChicas { get; set; }
        public List<OperacionCLS> listaOperaciones { get; set; }

        public List<ProgramacionSemanalCLS> listaProgramacionSemanal { get; set; }

        public byte NumeroSemana { get; set; }

        public short AnioOperacion { get; set; }
    }
}
