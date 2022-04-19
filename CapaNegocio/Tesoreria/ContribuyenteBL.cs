using CapaDatos.Tesoreria;
using CapaEntidad.Tesoreria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Tesoreria
{
    public class ContribuyenteBL
    {
        public string GuardarContribuyente(ContribuyenteCLS objContribuyente, string usuarioIng)
        {
            ContribuyenteDAL obj = new ContribuyenteDAL();
            return obj.GuardarContribuyente(objContribuyente, usuarioIng);
        }

        public ContribuyenteCLS GetDataContribuyente(string nit)
        {
            ContribuyenteDAL obj = new ContribuyenteDAL();
            return obj.GetDataContribuyente(nit);
        }


    }
}
