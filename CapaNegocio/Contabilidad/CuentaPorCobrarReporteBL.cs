using CapaDatos.Contabilidad;
using CapaEntidad.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Contabilidad
{
    public class CuentaPorCobrarReporteBL
    {
        public List<CuentaPorCobrarReporteCLS> GetReportesCuentasPorCobrarParaGeneracion()
        {
            CuentaPorCobrarReporteDAL obj = new CuentaPorCobrarReporteDAL();
            return obj.GetReportesCuentasPorCobrarParaGeneracion();
        }

        public List<CuentaPorCobrarReporteCLS> GetReportesCuentasPorCobrarParaConsulta()
        {
            CuentaPorCobrarReporteDAL obj = new CuentaPorCobrarReporteDAL();
            return obj.GetReportesCuentasPorCobrarParaConsulta();
        }
        public string GenerarReporteCuentaPorCobrar(int codigoReporte, int anioOperacion, int semanaOperacion, string idUsuario)
        {
            CuentaPorCobrarReporteDAL obj = new CuentaPorCobrarReporteDAL();
            return obj.GenerarReporteCuentaPorCobrar(codigoReporte, anioOperacion, semanaOperacion, idUsuario);
        }
        public List<CuentaPorCobrarReporteDetalleCLS> GetDetalleReporteCuentasPorCobrar(int codigoReporte, int anioOperacion, int semanaOperacion)
        {
            CuentaPorCobrarReporteDAL obj = new CuentaPorCobrarReporteDAL();
            return obj.GetDetalleReporteCuentasPorCobrar(codigoReporte, anioOperacion, semanaOperacion);
        }

        public List<CuentaPorCobrarReporteDetalleCLS> GetDetalleReporteCuentasPorCobrarGeneracion(int codigoReporte, int anioOperacion, int semanaOperacion)
        {
            CuentaPorCobrarReporteDAL obj = new CuentaPorCobrarReporteDAL();
            return obj.GetDetalleReporteCuentasPorCobrarGeneracion(codigoReporte, anioOperacion, semanaOperacion);
        }

        public string AceptarReporteComoValido(int codigoReporte, int anioOperacion, int semanaOperacion, string usuarioAct)
        {
            CuentaPorCobrarReporteDAL obj = new CuentaPorCobrarReporteDAL();
            return obj.AceptarReporteComoValido(codigoReporte, anioOperacion, semanaOperacion, usuarioAct);
        }

        public string EliminarReporteGenerado(int codigoReporte, int anioOperacion, int semanaOperacion, string usuarioAct)
        {
            CuentaPorCobrarReporteDAL obj = new CuentaPorCobrarReporteDAL();
            return obj.EliminarReporteGenerado(codigoReporte, anioOperacion, semanaOperacion, usuarioAct);
        }

        public ReporteOperacionesCajaListCLS GetDetalleCorteCuentasPorCobrar(int anioOperacion, int semanaOperacion, int codigoReporte, int arqueo, int excluirCeros)
        {
            CuentaPorCobrarReporteDAL obj = new CuentaPorCobrarReporteDAL();
            return obj.GetDetalleCorteCuentasPorCobrar(anioOperacion, semanaOperacion, codigoReporte, arqueo, excluirCeros);
        }

    }
}
