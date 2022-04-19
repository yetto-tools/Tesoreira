using CapaDatos.RRHH;
using CapaEntidad.RRHH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.RRHH
{
    public class MotivoBajaBL
    {
        public List<MotivoBajaCLS> GetMotivosDeBaja()
        {
            MotivoBajaDAL obj = new MotivoBajaDAL();
            return obj.GetMotivosDeBaja();
        }
    }


}
