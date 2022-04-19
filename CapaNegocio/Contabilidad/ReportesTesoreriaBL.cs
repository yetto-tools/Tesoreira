using CapaDatos.Contabilidad;
using CapaEntidad.Contabilidad;
using CapaEntidad.Tesoreria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Contabilidad
{
    public class ReportesTesoreriaBL
    {

        public List<ReporteCajaChicaCLS> GetCortesCajaChica(int anioOperacion, int codigoCajaChica)
        {
            ReportesTesoreriaDAL obj = new ReportesTesoreriaDAL();
            return obj.GetCortesCajaChica(anioOperacion, codigoCajaChica);
        }

        public List<ReporteCajaCLS> GetReportes(int codigoTipoReporte)
        {
            ReportesTesoreriaDAL obj = new ReportesTesoreriaDAL();
            return obj.GetReportes(codigoTipoReporte);
        }

        public ReporteOperacionesCajaListCLS GetReporteResumenOperacionCaja(int anioOperacion, int semanaOperacion, int codigoReporte, int arqueo)
        {
            ReportesTesoreriaDAL obj = new ReportesTesoreriaDAL();
            return obj.GetReporteResumenOperacionCaja(anioOperacion, semanaOperacion, codigoReporte, arqueo);
        }

        public ReporteOperacionesCajaListCLS GetReporteOperacionCaja(int anioOperacion, int semanaOperacion, int codigoReporte, int arqueo)
        {
            ReportesTesoreriaDAL obj = new ReportesTesoreriaDAL();
            return obj.GetReporteOperacionCaja(anioOperacion, semanaOperacion, codigoReporte, arqueo);
        }

        public ReporteOperacionesCajaListCLS GetReporteDepositosBancarios(int anioOperacion, int semanaOperacion, int codigoReporte, int arqueo)
        {
            ReportesTesoreriaDAL obj = new ReportesTesoreriaDAL();
            return obj.GetReporteDepositosBancarios(anioOperacion, semanaOperacion, codigoReporte, arqueo);
        }

        public ReporteOperacionesCajaListCLS GetReporteOperacionesDeSocios(int anioOperacion, int semanaOperacion, int codigoReporte, int arqueo)
        {
            ReportesTesoreriaDAL obj = new ReportesTesoreriaDAL();
            return obj.GetReporteOperacionesDeSocios(anioOperacion, semanaOperacion, codigoReporte, arqueo);
        }

        public ReporteOperacionesCajaListCLS GetReporteReservasYCajas(int anioOperacion, int semanaOperacion, int codigoReporte, int arqueo)
        {
            ReportesTesoreriaDAL obj = new ReportesTesoreriaDAL();
            return obj.GetReporteReservasYCajas(anioOperacion, semanaOperacion, codigoReporte, arqueo);
        }

        public ReporteOperacionesCajaListCLS GetReporteCierre(int anioOperacion, int semanaOperacion, int codigoReporte, int arqueo)
        {
            ReportesTesoreriaDAL obj = new ReportesTesoreriaDAL();
            return obj.GetReporteCierre(anioOperacion, semanaOperacion, codigoReporte);
        }

    }
}
