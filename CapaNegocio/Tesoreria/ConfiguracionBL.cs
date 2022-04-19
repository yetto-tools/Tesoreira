using CapaDatos.Tesoreria;
using CapaEntidad.Tesoreria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Tesoreria
{
    public class ConfiguracionBL
    {
        public List<OtroIngresoCLS> GetListaOtrosIngresos()
        {
            OtroIngresoDAL obj = new OtroIngresoDAL();
            return obj.GetListaOtrosIngresos();
        }
    }
}
