using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Tesoreria
{
    public class EntidadGenericaListCLS
    {
        public List<EntidadGenericaCLS> listaEntidadesGenericas { get; set; }

        public List<EntidadGenericaCLS> listaEntidadesEspeciales1 { get; set; }
        public List<EntidadGenericaCLS> listaEntidadesEspeciales2 { get; set; }
        public List<EntidadGenericaCLS> listaEntidadesBackToBack { get; set; }

        public List<EntidadGenericaCLS> listaEntidadesVendedores { get; set; }

        public List<EntidadGenericaCLS> listaEmpresasConcedeIVA { get; set; }
    }
}
