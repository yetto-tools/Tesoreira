using CapaDatos.Tesoreria;
using CapaEntidad.Tesoreria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Tesoreria
{
    public class ReporteCajaDetalleBL
    {
        public ReporteCajaDetalleListCLS GetDetalleReporte(int codigoReporte)
        {
            ReporteCajaDetalleDAL obj = new ReporteCajaDetalleDAL();
            return obj.GetDetalleReporte(codigoReporte);
        }

        public ReporteCajaDetalleListCLS GetDetalleReporteCaja(int anioOperacion, int semanaOperacion, int codigoReporte)
        {
            ReporteCajaDetalleDAL obj = new ReporteCajaDetalleDAL();
            return obj.GetDetalleReporteCaja(anioOperacion, semanaOperacion, codigoReporte);
        }

    }



}
