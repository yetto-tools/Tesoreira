using CapaDatos.Tesoreria;
using CapaEntidad.Contabilidad;
using CapaEntidad.Tesoreria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Tesoreria
{
    public class ReporteCajaChicaBL
    {
        public List<ReporteCajaChicaCLS> ListarReportesCajaChica(string usuarioIng, int esSuperAdmin)
        {
            ReporteCajaChicaDAL obj = new ReporteCajaChicaDAL();
            return obj.ListarReportesCajaChica(usuarioIng, esSuperAdmin);
        }

        public List<ReporteCajaChicaCLS> ListarReportesCajaChicaConsulta(int codigoCajaChica, int anioOperacion, string usuarioIng, int esSuperAdmin)
        {
            ReporteCajaChicaDAL obj = new ReporteCajaChicaDAL();
            return obj.ListarReportesCajaChicaConsulta(codigoCajaChica, anioOperacion, usuarioIng, esSuperAdmin);
        }

        public string GenerarReporteSemanal(int codigoCajaChica, int anioOperacion, int semanaOperacion, string observaciones, string idUsuario)
        {
            ReporteCajaChicaDAL obj = new ReporteCajaChicaDAL();
            return obj.GenerarReporteSemanal(codigoCajaChica, anioOperacion, semanaOperacion, observaciones, idUsuario);
        }

        public string ActualizarEstadoReporte(int codigoReporte, int codigoEstadoReporte, string usuarioAct)
        {

            ReporteCajaChicaDAL obj = new ReporteCajaChicaDAL();
            return obj.ActualizarEstadoReporte(codigoReporte, codigoEstadoReporte, usuarioAct);
        }

        public string FinalizarRevision(int codigoReporte, decimal montoReintegroCalculado, decimal montoReintegro, string usuarioAct)
        {
            ReporteCajaChicaDAL obj = new ReporteCajaChicaDAL();
            return obj.FinalizarRevision(codigoReporte, montoReintegroCalculado, montoReintegro, usuarioAct);
        }

        public List<ReporteCajaChicaCLS> ListarReportesCajaChicaParaRevision(string usuarioIng, int esSuperAdmin)
        {
            ReporteCajaChicaDAL obj = new ReporteCajaChicaDAL();
            return obj.ListarReportesCajaChicaParaRevision(usuarioIng, esSuperAdmin);
        }

        public string AnularReporteGenerado(int codigoReporte, string usuarioAct)
        {
            ReporteCajaChicaDAL obj = new ReporteCajaChicaDAL();
            return obj.AnularReporteGenerado(codigoReporte, usuarioAct);
        }

        public string TrasladarReporteParaRevision(int codigoReporte, int codigoCajaChica, decimal montoGastosFiscales, string usuarioAct)
        {
            ReporteCajaChicaDAL obj = new ReporteCajaChicaDAL();
            return obj.TrasladarReporteParaRevision(codigoReporte, codigoCajaChica, montoGastosFiscales, usuarioAct);
        }

        public ReporteOperacionesCajaListCLS GetReporteReintegroCajaChica(int codigoReporte, int anioOperacion, int semanaOperacion, int codigoCajaChica)
        {
            ReporteCajaChicaDAL obj = new ReporteCajaChicaDAL();
            return obj.GetReporteReintegroCajaChica(codigoReporte, anioOperacion, semanaOperacion, codigoCajaChica);
        }

        public ReporteCorteCajaChicaCLS GetTransaccionesReporteChica(int codigoReporte, int anioOperacion, int semanaOperacion)
        {
            ReporteCajaChicaDAL obj = new ReporteCajaChicaDAL();
            return obj.GetTransaccionesReporteChica(codigoReporte, anioOperacion, semanaOperacion);
        }

    }
}
