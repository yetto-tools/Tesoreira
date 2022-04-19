using CapaDatos.Planilla;
using CapaEntidad.Planilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Planilla
{
    public class PagoBackToBackPlanillaBL
    {
        public List<PagoDescuentoCLS> GetEmpleadosBackToBackPlanilla(int codigoTipoPlanilla, int anioPlanilla, int mesPlanilla)
        {
            PagoBackToBackPlanillaDAL obj = new PagoBackToBackPlanillaDAL();
            return obj.GetEmpleadosBackToBackPlanilla(codigoTipoPlanilla, anioPlanilla, mesPlanilla);
        }

        //public string GuardarDevolucionesBTB(List<PagoDescuentoCLS> objPagoDescuento, int anioOperacion, int semanaOperacion, string usuarioIng)
        //{
        //    PagoBackToBackPlanillaDAL obj = new PagoBackToBackPlanillaDAL();
        //    return obj.GuardarDevolucionesBTB(objPagoDescuento, anioOperacion, semanaOperacion, usuarioIng);
        //}

        public string GuardarDevolucionesBTB(List<PagoDescuentoCLS> objPagoDescuento, string usuarioIng)
        {
            PagoBackToBackPlanillaDAL obj = new PagoBackToBackPlanillaDAL();
            return obj.GuardarDevolucionesBTB(objPagoDescuento, usuarioIng);
        }

        public List<PagoDescuentoCLS> GetEmpleadosBackToBackBoletaDeposito(int codigoTipoPlanilla, int anioPlanilla, int mesPlanilla)
        {
            PagoBackToBackPlanillaDAL obj = new PagoBackToBackPlanillaDAL();
            return obj.GetEmpleadosBackToBackBoletaDeposito(codigoTipoPlanilla, anioPlanilla, mesPlanilla);
        }

    }

}
