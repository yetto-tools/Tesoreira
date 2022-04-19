using CapaDatos.Administracion;
using CapaEntidad.Administracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Administracion
{
    public class ProgramacionQuincenalBL
    {
        public List<ProgramacionQuincenalCLS> GetListaQuincenas(int anio, int numeroMes)
        {
            ProgramacionQuincenalDAL obj = new ProgramacionQuincenalDAL();
            return obj.GetListaQuincenas(anio, numeroMes);
        }


    }
}
