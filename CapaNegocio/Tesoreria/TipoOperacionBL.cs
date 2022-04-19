using CapaDatos.Tesoreria;
using CapaEntidad.Tesoreria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Tesoreria
{
    public class TipoOperacionBL
    {
        public List<TipoOperacionCLS> listarTiposOperacion()
        {
            TipoOperacionDAL obj = new TipoOperacionDAL();
            return obj.listarTiposOperacion();
        }
    }
}
