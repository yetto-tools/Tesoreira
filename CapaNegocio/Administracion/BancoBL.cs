using CapaDatos.Administracion;
using CapaEntidad.Administracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Administracion
{
    public class BancoBL
    {
        public List<BancoCLS> GetAllBancos()
        {
            BancoDAL obj = new BancoDAL();
            return obj.GetAllBancos();
        }
    }
}
