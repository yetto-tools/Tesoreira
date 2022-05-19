using CapaDatos.Contabilidad;
using CapaEntidad.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Contabilidad
{
    public class DepositoBancarioBL
    {
        public List<DepositoBancarioCLS> GetDepositos(int anioReporte, int semanaReporte, int codigoReporte)
        {
            DepositoBancarioDAL obj = new DepositoBancarioDAL();
            return obj.GetDepositos(anioReporte, semanaReporte, codigoReporte);
        }

    }
}
