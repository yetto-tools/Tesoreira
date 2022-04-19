using CapaDatos.Administracion;
using CapaEntidad.Administracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Administracion
{
    public class EmpresaBL
    {
        public List<EmpresaCLS> GetAllEmpresas() 
        {
            EmpresaDAL obj = new EmpresaDAL();
            return obj.GetAllEmpresas();
        }

        public List<EmpresaCLS> GetEmpresasTesoreriaFacturas()
        {
            EmpresaDAL obj = new EmpresaDAL();
            return obj.GetEmpresasTesoreriaFacturas();
        }

    }
}
