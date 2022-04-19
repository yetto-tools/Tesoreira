using CapaDatos.Administracion;
using CapaEntidad.Administracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Administracion
{
    public class MunicipioBL
    {
        public List<MunicipioCLS> GetAllMunicipios(Int32 codigoDepartamento)
        {
            MunicipioDAL obj = new MunicipioDAL();
            return obj.GetAllMunicipios(codigoDepartamento);
        }


    }
}
