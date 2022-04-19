using CapaDatos.Administracion;
using CapaEntidad.Administracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Administracion
{
    public class DepartamentoBL
    {
        public List<DepartamentoCLS> GetAllDepartamentos()
        {
            DepartamentoDAL obj = new DepartamentoDAL();
            return obj.GetAllDepartamentos();
        }



    }
}
