using CapaDatos.Tesoreria;
using CapaEntidad.Tesoreria;
using System.Collections.Generic;

namespace CapaNegocio.Tesoreria
{
    public class ReporteCajaBL
    {
        public List<ReporteCajaCLS> GetReportesSemanalesCajaGeneracion(string usuarioGeneracion)
        {
            ReporteCajaDAL obj = new ReporteCajaDAL();
            return obj.GetReportesSemanalesCajaGeneracion(usuarioGeneracion);
        }

        public List<ReporteCajaCLS> GetReportesSemanalesCajaGeneracionTemporal(string usuarioGeneracion, int semanaOculta)
        {
            ReporteCajaDAL obj = new ReporteCajaDAL();
            return obj.GetReportesSemanalesCajaGeneracionTemporal(usuarioGeneracion, semanaOculta);
        }

        public List<ReporteCajaCLS> GetReportesSemanalesEnProcesoDeGeneracion(string usuarioGeneracion, int anioOperacion, int semanaOperacion)
        {
            ReporteCajaDAL obj = new ReporteCajaDAL();
            return obj.GetReportesSemanalesEnProcesoDeGeneracion(usuarioGeneracion, anioOperacion, semanaOperacion);
        }

        public List<ReporteCajaCLS> GetReportesSemanalesCajaParaVistoBueno()
        {
            ReporteCajaDAL obj = new ReporteCajaDAL();
            return obj.GetReportesSemanalesCajaParaVistoBueno();
        }

        public List<ReporteCajaCLS> GetReportesSemanalesCaja(int anio)
        {
            ReporteCajaDAL obj = new ReporteCajaDAL();
            return obj.GetReportesSemanalesCaja(anio);
        }

        public string GenerarReporteSemanal(int anio, int numeroSemana, string idUsuario)
        {
            ReporteCajaDAL obj = new ReporteCajaDAL();
            return obj.GenerarReporteSemanal(anio, numeroSemana, idUsuario);
        }

        public string EliminarReporteSemanal(int codigoReporte, int anioOperacion, int semanaOperacion, string usuarioAct)
        {
            ReporteCajaDAL obj = new ReporteCajaDAL();
            return obj.EliminarReporteSemanal(codigoReporte, anioOperacion, semanaOperacion, usuarioAct);
        }

        public string ExisteReporte(int anio, int numeroSemana)
        {
            ReporteCajaDAL obj = new ReporteCajaDAL();
            return obj.ExisteReporte(anio, numeroSemana);
        }

        public string AceptarReporteGenerado(int codigoReporte, int anioOperacion, int semanaOperacion, string usuarioAct)
        {
            ReporteCajaDAL obj = new ReporteCajaDAL();
            return obj.AceptarReporteGenerado(codigoReporte, anioOperacion, semanaOperacion, usuarioAct);
        }

        public string AceptarReportePorContabilidad(int codigoReporte, int anioOperacion, int semanaOperacion, string usuarioAct)
        {
            ReporteCajaDAL obj = new ReporteCajaDAL();
            return obj.AceptarReportePorContabilidad(codigoReporte, anioOperacion, semanaOperacion, usuarioAct);
        }

        public List<ReporteCajaCLS> GetReportesCajaEnProceso(int anioOperacion, int semanaOperacion, int esSuperAdmin)
        {
            ReporteCajaDAL obj = new ReporteCajaDAL();
            return obj.GetReportesCajaEnProceso(anioOperacion, semanaOperacion, esSuperAdmin);
        }

        public List<ReporteCajaCLS> GetReportesCajaConsulta(int anioOperacion, int semanaOperacion)
        {
            ReporteCajaDAL obj = new ReporteCajaDAL();
            return obj.GetReportesCajaConsulta(anioOperacion, semanaOperacion);
        }

        public List<ReporteCajaCLS> GetReportesCaja(int anioOperacion, int semanaOperacion)
        {
            ReporteCajaDAL obj = new ReporteCajaDAL();
            return obj.GetReportesCaja(anioOperacion, semanaOperacion);
        }

    }

}
