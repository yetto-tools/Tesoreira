using CapaDatos.Administracion;
using CapaEntidad.Administracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Administracion
{
    public class SistemaBL
    {
        public List<SistemaCLS> GetAllSistemas()
        {
            SistemaDAL obj = new SistemaDAL();
            return obj.GetAllSistemas();
        }


    }
}
