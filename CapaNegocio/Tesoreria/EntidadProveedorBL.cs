using CapaDatos.Tesoreria;
using CapaEntidad.Tesoreria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Tesoreria
{
    public class EntidadProveedorBL
    {
        public List<EntidadProveedorCLS> GetProveedores(int codigoEntidad)
        {
            EntidadProveedorDAL obj = new EntidadProveedorDAL();
            return obj.GetProveedores(codigoEntidad);

        }
    }
}
