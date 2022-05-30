using CapaDatos.Administracion;
using CapaEntidad.Administracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Administracion
{
    public class ParametroImpresionBL
    {
        public List<ParametroImpresionCLS> GetAllConfiguracionesImpresion()
        {
            ParametroImpresionDAL obj = new ParametroImpresionDAL();
            return obj.GetAllConfiguracionesImpresion();
        }
    }
}
