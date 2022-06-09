using CapaDatos.QSystems;
using CapaEntidad.QSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.QSystems
{
    public class ReportesBL
    {
        public List<FacturaVentasCLS> GetListaVentaPorRangoFechaDetallado(string codigoEmpresa, string fechaInicio, string fechaFin)
        {
            ReportesDAL obj = new ReportesDAL();
            return obj.GetListaVentaPorRangoFechaDetallado(codigoEmpresa, fechaInicio, fechaFin);
        }

        public List<ValeSalidaCLS> GetListaValesDeSalida(string codigoEmpresa, string fechaInicio, string fechaFin)
        {
            ReportesDAL obj = new ReportesDAL();
            return obj.GetListaValesDeSalida(codigoEmpresa, fechaInicio, fechaFin);
        }


    }
}
