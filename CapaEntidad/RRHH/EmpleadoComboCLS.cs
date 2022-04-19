using CapaEntidad.Administracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.RRHH
{
    public class EmpleadoComboCLS
    {
        public List<EmpresaCLS> listaEmpresas { get; set; }

        public List<AreaCLS> listaAreas { get; set; }

        public List<SeccionCLS> listaSecciones { get; set; }

        public List<PuestoCLS> listaPuestos { get; set; }

        public List<EstadoEmpleadoCLS> listaEstadosEmpleado { get; set; }

        public List<TipoBackToBackCLS> listaTiposBackToBack { get; set; }



    }
}
