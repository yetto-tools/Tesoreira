using CapaEntidad.Planilla;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Planilla
{
    public class FechaReporteCxCDAL: CadenaConexion
    {
        //public FechaReporteCxCCLS GetFechaReporteCxCPagoBTBYDescuento()
        //{
        //    FechaReporteCxCCLS objFechaReporte = null;
        //    using (SqlConnection conexion = new SqlConnection(cadenaContabilidad))
        //    {
        //        try
        //        {
        //            conexion.Open();
        //            string sql = @"
        //            SELECT y.anio_operacion, 
	       //                y.semana_operacion,
	       //                y.semana_maxima,
	       //                CASE
		      //               WHEN y.semana_operacion = y.semana_maxima THEN y.anio_operacion + 1
		      //               ELSE y.anio_operacion
	       //                END anio_operacion_reporte_cxc,
	       //                CASE
		      //               WHEN y.semana_operacion = y.semana_maxima THEN 1
		      //               ELSE y.semana_operacion + 1
	       //                END semana_operacion_reporte_cxc
        //            FROM ( SELECT TOP (1)
	       //                       x.anio_operacion, 
	       //                       x.semana_operacion,
	       //                       (SELECT max(numero_semana) FROM db_admon.programacion_semanal where anio = x.anio_operacion) AS semana_maxima
        //                          FROM db_contabilidad.cxc_reporte x
        //                   ORDER BY x.anio_operacion DESC, x.semana_operacion DESC
        //                ) y";

        //            using (SqlCommand cmd = new SqlCommand(sql, conexion))
        //            {
        //                cmd.CommandType = CommandType.Text;
        //                SqlDataReader dr = cmd.ExecuteReader();
        //                if (dr != null)
        //                {
        //                    int postAnioOperacion = dr.GetOrdinal("anio_operacion_reporte_cxc");
        //                    int postSemanaOperacion = dr.GetOrdinal("semana_operacion_reporte_cxc");
        //                    while (dr.Read())
        //                    {
        //                        objFechaReporte = new FechaReporteCxCCLS();
        //                        objFechaReporte.AnioOperacion = (short)dr.GetInt32(postAnioOperacion);
        //                        objFechaReporte.SemanaOperacion = (byte)dr.GetInt32(postSemanaOperacion);
        //                    }
        //                }
        //            }
        //            conexion.Close();
        //        }
        //        catch (Exception)
        //        {
        //            objFechaReporte = null;
        //            conexion.Close();
        //        }

        //        return objFechaReporte;
        //    }
        //}
    }
}
