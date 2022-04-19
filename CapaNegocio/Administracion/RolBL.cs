using CapaDatos.Administracion;
using CapaEntidad.Administracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Administracion
{
    public class RolBL
    {
        public List<RolCLS> GetAllRoles()
        {
            RolDAL obj = new RolDAL();
            return obj.GetAllRoles();
        }

        public RolCLS GetDataRol(int codigoRol)
        {
            RolDAL obj = new RolDAL();
            return obj.GetDataRol(codigoRol);
        }

        public List<SiteMapCLS> GetConfiguracionRol(int codigoRol)
        {
            RolDAL obj = new RolDAL();
            return obj.GetConfiguracionRol(codigoRol);
        }



    }
}
