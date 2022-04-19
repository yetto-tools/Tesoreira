using CapaEntidad.Administracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Tesoreria
{
    public class TransaccionComboCLS
    {
        public List<EmpresaCLS> listaEmpresasTesoreria { get; set; }
        public List<OperacionCLS> listaOperaciones { get; set; }

        public List<ProgramacionSemanalCLS> listaProgramacionSemanal { get; set; }

        public List<EntidadCategoriaCLS> listaCategoriasEntidades { get; set; }

        public List<AnioCLS> listaAnios { get; set; }

        public byte NumeroSemana { get; set; }

        public short AnioOperacion { get; set; }

    }
}
