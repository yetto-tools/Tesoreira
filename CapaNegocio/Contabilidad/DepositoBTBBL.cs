using CapaDatos.Contabilidad;
using CapaEntidad.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Contabilidad
{
    public class DepositoBTBBL
    {
        public string GuardarDepositosBTB(List<DepositoBTBCLS> listaDepositos, string usuarioIng)
        {
            DepositoBTBDAL obj = new DepositoBTBDAL();
            return obj.GuardarDepositosBTB(listaDepositos, usuarioIng);
        }

        public string ActualizarDeposito(DepositoBTBCLS objDeposito, string usuarioAct)
        {
            DepositoBTBDAL obj = new DepositoBTBDAL();
            return obj.ActualizarDeposito(objDeposito, usuarioAct);
        }

        public string AnularDeposito(int codigoDepositoBTB, string usuarioAct)
        {
            DepositoBTBDAL obj = new DepositoBTBDAL();
            return obj.AnularDeposito(codigoDepositoBTB, usuarioAct);
        }

        public List<DepositoBTBCLS> GetDepositosBTB(int anioPlanilla, int anioOperacion, int semanaOperacion, int codigoReporte)
        {
            DepositoBTBDAL obj = new DepositoBTBDAL();
            return obj.GetDepositosBTB(anioPlanilla, anioOperacion, semanaOperacion, codigoReporte);
        }

    }
}
