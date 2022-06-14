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

        public List<PagoDescuentoCLS> GetPagosBackToBackRealizadosEnPlanilla(int anio, int mes, int codigoEmpresa)
        {
            PagoBackToBackPlanillaDAL obj = new PagoBackToBackPlanillaDAL();
            return obj.GetPagosBackToBackRealizadosEnPlanilla(anio, mes, codigoEmpresa);
        }

    }

}
