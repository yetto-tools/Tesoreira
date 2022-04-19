using CapaDatos.RRHH;
using CapaEntidad.RRHH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.RRHH
{
    public class MotivoSuspensionBL
    {
        public List<MotivoSuspensionCLS> GetMotivosDeSuspension()
        {
            MotivoSuspensionDAL obj = new MotivoSuspensionDAL();
            return obj.GetMotivosDeSuspension();
        }
    }


}
