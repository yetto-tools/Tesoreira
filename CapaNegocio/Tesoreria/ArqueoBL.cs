using CapaDatos.Tesoreria;
using CapaEntidad.Tesoreria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Tesoreria
{
    public class ArqueoBL
    {
        public List<ReporteCajaCLS> GetListaArqueos(string usuarioGeneracion)
        {
            ArqueoDAL obj = new ArqueoDAL();
            return obj.GetListaArqueos(usuarioGeneracion);
        }

        public string GenerarArqueo(int anio, int numeroSemana, string usuarioIng)
        {
            ArqueoDAL obj = new ArqueoDAL();
            return obj.GenerarArqueo(anio, numeroSemana, usuarioIng);
        }
    }
}
