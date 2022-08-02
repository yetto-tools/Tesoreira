using CapaDatos.Planilla;
using CapaEntidad.Planilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Planilla
{
    public class PagoDescuentoBL
    {

        public List<SaldoPrestamoCLS> GetEmpleadosCuentasPorCobrarPlanilla()
        {
            PagoDescuentoDAL obj = new PagoDescuentoDAL();
            return obj.GetEmpleadosCuentasPorCobrarPlanilla();
        }

        public string GuardarDescuentoDevolucion(int codigoEmpresa, int codigoCategoria, string codigoEmpleado, int codigoOperacion, decimal monto, string usuarioIng)
        {
            PagoDescuentoDAL obj = new PagoDescuentoDAL();
            return obj.GuardarDescuentoDevolucion(codigoEmpresa, codigoCategoria, codigoEmpleado, codigoOperacion, monto, usuarioIng);
        }

        public List<PagoDescuentoCLS> GetPagosDescuentos()
        {
            PagoDescuentoDAL obj = new PagoDescuentoDAL();
            return obj.GetPagosDescuentos();
        }

        public string AnularPagoDescuento(int codigoPago, string usuarioAct)
        {
            PagoDescuentoDAL obj = new PagoDescuentoDAL();
            return obj.AnularPagoDescuento(codigoPago, usuarioAct);
        }

        public List<PagoDescuentoCLS> GetPagosDescuentosConsulta(int anio, int mes, int codigoEmpresa)
        {
            PagoDescuentoDAL obj = new PagoDescuentoDAL();
            return obj.GetPagosDescuentosConsulta(anio, mes, codigoEmpresa);
        }

    }


}
