using CapaDatos.RRHH;
using CapaEntidad.RRHH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.RRHH
{
    public class AreaBL
    {
        public List<AreaCLS> GetAllAreas()
        {
            AreaDAL obj = new AreaDAL();
            return obj.GetAllAreas();
        }
    }
}
