using CapaEntidad.Administracion;
using CapaEntidad.Contabilidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Contabilidad
{
    public class CuentaPorCobrarReporteDAL: CadenaConexion
    {


        public List<CuentaPorCobrarReporteCLS> GetReportesCuentasPorCobrarParaGeneracion()
        {
            List<CuentaPorCobrarReporteCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaContabilidad))
            {
                try
                {
                    string sql = @"
                    SELECT COALESCE(x.codigo_reporte, 0) AS codigo_reporte,
                           x.anio_operacion,
	                       x.semana_operacion, 
	                       COALESCE(y.anticipo_liquidable,0) AS anticipo_liquidable, 
	                       COALESCE(z.devolucion_anticipo_liquidable,0) AS devolucion_anticipo_liquidable,
	                       COALESCE(w.anticipo_salario,0) AS anticipo_salario,
	                       COALESCE(p.descuento_anticipo_salario,0) AS descuento_anticipo_salario,
	                       COALESCE(a.prestamo,0) AS prestamo,
	                       COALESCE(b.abono_prestamo,0) AS abono_prestamo,
	                       COALESCE(c.back_to_back,0) AS back_to_back,
                           COALESCE(f.btb_pago_en_planilla,0) AS btb_pago_en_planilla,
	                       COALESCE(d.retiro_socios,0) AS retiro_socios,
	                       COALESCE(e.devolucion_socios,0) AS devolucion_socios,
                           0 AS bloqueado,
                           1 AS codigo_estado,
                           'Por Generar'  AS estado,
                           0 AS permiso_anular
                    FROM ( SELECT m.anio_operacion, m.semana_operacion, m.codigo_reporte
                           FROM (
                                  SELECT anio_operacion, semana_operacion, codigo_reporte
                                  FROM db_contabilidad.cuenta_por_cobrar 
                                  WHERE codigo_estado = @CodigoEstadoParaIncluirEnReporte 
                                    AND carga_inicial = 0
                                    AND anio_operacion  <> 0
                                    AND semana_operacion <> 0
                                    AND codigo_reporte <> 0

                                  UNION

                                  SELECT y.anio_operacion, y.semana_operacion, y.codigo_reporte
                                  FROM ( SELECT  0 AS codigo 
	                                     FROM db_contabilidad.cuenta_por_cobrar
	                                     WHERE codigo_estado = @CodigoEstadoParaIncluirEnReporte 
		                                   AND carga_inicial = 0
		                                   AND anio_operacion = 0 
		                                   AND semana_operacion = 0
		                                   AND codigo_reporte = 0
                                       ) x 
                                  INNER JOIN ( SELECT TOP(1) 
                                                      0 AS codigo, 
                                                      anio AS anio_operacion, 
                                                      numero_semana as semana_operacion, 
                                                      codigo_reporte  
                                               FROM db_tesoreria.reporte_caja 
                                               WHERE codigo_estado = @CodigoEstadoReporteDeCajaPorRevisarEnContabilidad
                                               ORDER BY codigo_reporte ASC
                                             ) y
                                  ON x.codigo = y.codigo
                           ) m                            
                           GROUP BY m.anio_operacion, m.semana_operacion, m.codigo_reporte
                    ) x
                    LEFT JOIN ( SELECT anio_operacion, semana_operacion, codigo_reporte, sum(monto) AS anticipo_liquidable
                                FROM db_contabilidad.cuenta_por_cobrar
                                WHERE codigo_operacion = @CodigoAnticipoLiquidable
                                GROUP BY anio_operacion, semana_operacion, codigo_reporte
                              ) y
                    ON x.anio_operacion = y.anio_operacion AND x.semana_operacion = y.semana_operacion AND x.codigo_reporte = y.codigo_reporte
                    LEFT JOIN ( SELECT anio_operacion, semana_operacion, codigo_reporte, sum(monto) AS devolucion_anticipo_liquidable
                                FROM db_contabilidad.cuenta_por_cobrar
                                WHERE codigo_operacion = @CodigoDevolucionAnticipoLiquidable
                                GROUP BY anio_operacion, semana_operacion, codigo_reporte
                              ) z
                    ON x.anio_operacion = z.anio_operacion AND x.semana_operacion = z.semana_operacion  AND x.codigo_reporte = z.codigo_reporte
                    LEFT JOIN ( SELECT anio_operacion, semana_operacion, codigo_reporte, sum(monto) AS anticipo_salario
                                FROM db_contabilidad.cuenta_por_cobrar
                                WHERE codigo_operacion = @CodigoAnticipoSalario
                                GROUP BY anio_operacion, semana_operacion, codigo_reporte
                              ) w
                    ON x.anio_operacion = w.anio_operacion AND x.semana_operacion = w.semana_operacion  AND x.codigo_reporte = w.codigo_reporte
                    LEFT JOIN ( SELECT anio_operacion, semana_operacion, codigo_reporte, sum(monto) AS descuento_anticipo_salario
                                FROM db_contabilidad.cuenta_por_cobrar
                                WHERE codigo_operacion = @CodigoDescuentoAnticipoSalario
                                GROUP BY anio_operacion, semana_operacion, codigo_reporte
                              ) p
                    ON x.anio_operacion = p.anio_operacion AND x.semana_operacion = p.semana_operacion  AND x.codigo_reporte = p.codigo_reporte
                    LEFT JOIN ( SELECT anio_operacion, semana_operacion, codigo_reporte, sum(monto) AS prestamo
                                FROM db_contabilidad.cuenta_por_cobrar
                                WHERE codigo_operacion = @CodigoPrestamo
                                GROUP BY anio_operacion, semana_operacion, codigo_reporte
                              ) a
                    ON x.anio_operacion = a.anio_operacion AND x.semana_operacion = a.semana_operacion  AND x.codigo_reporte = a.codigo_reporte
                    LEFT JOIN ( 
                                SELECT m.anio_operacion, m.semana_operacion, m.codigo_reporte, sum(m.monto) AS abono_prestamo
                                FROM ( SELECT anio_operacion, semana_operacion, codigo_reporte, monto
                                       FROM db_contabilidad.cuenta_por_cobrar 
                                       WHERE codigo_estado = @CodigoEstadoParaIncluirEnReporte 
                                         AND codigo_operacion = @CodigoAbonoPrestamo
                                         AND carga_inicial = 0
                                         AND anio_operacion  <> 0
                                         AND semana_operacion <> 0
                                         AND codigo_reporte <> 0

                                       UNION

                                       SELECT y.anio_operacion, y.semana_operacion, y.codigo_reporte, x.monto
                                       FROM ( SELECT  0 AS codigo, 
                                                      monto
	                                          FROM db_contabilidad.cuenta_por_cobrar
	                                          WHERE codigo_estado = @CodigoEstadoParaIncluirEnReporte 
                                                AND codigo_operacion = @CodigoAbonoPrestamo
		                                        AND carga_inicial = 0
		                                        AND anio_operacion = 0 
		                                        AND semana_operacion = 0
		                                        AND codigo_reporte = 0
                                            ) x 
                                       INNER JOIN ( SELECT TOP(1) 
                                                           0 AS codigo, 
                                                           anio AS anio_operacion, 
                                                           numero_semana as semana_operacion, 
                                                           codigo_reporte  
                                                    FROM db_tesoreria.reporte_caja 
                                                    WHERE codigo_estado = @CodigoEstadoReporteDeCajaPorRevisarEnContabilidad
                                                    ORDER BY codigo_reporte ASC
                                                   ) y
                                        ON x.codigo = y.codigo
                                     ) m                            
                                GROUP BY m.anio_operacion, m.semana_operacion, m.codigo_reporte
                              ) b
                    ON x.anio_operacion = b.anio_operacion AND x.semana_operacion = b.semana_operacion  AND x.codigo_reporte = b.codigo_reporte
                    LEFT JOIN ( SELECT anio_operacion, semana_operacion, codigo_reporte, sum(monto) AS back_to_back
                                FROM db_contabilidad.cuenta_por_cobrar
                                WHERE codigo_operacion = @CodigoBackToBack
                                GROUP BY anio_operacion, semana_operacion, codigo_reporte
                              ) c
                    ON x.anio_operacion = c.anio_operacion AND x.semana_operacion = c.semana_operacion  AND x.codigo_reporte = c.codigo_reporte
                    LEFT JOIN ( SELECT anio_operacion, semana_operacion, codigo_reporte, sum(monto) AS retiro_socios
                                FROM db_contabilidad.cuenta_por_cobrar
                                WHERE codigo_operacion = @CodigoRetiroSocios
                                GROUP BY anio_operacion, semana_operacion, codigo_reporte
                              ) d
                    ON x.anio_operacion = d.anio_operacion AND x.semana_operacion = d.semana_operacion  AND x.codigo_reporte = d.codigo_reporte
                    LEFT JOIN ( SELECT anio_operacion, semana_operacion, codigo_reporte, sum(monto) AS devolucion_socios
                                FROM db_contabilidad.cuenta_por_cobrar
                                WHERE codigo_operacion = @CodigoDevolucionSocios
                                GROUP BY anio_operacion, semana_operacion, codigo_reporte
                              ) e
                    ON x.anio_operacion = e.anio_operacion AND x.semana_operacion = e.semana_operacion  AND x.codigo_reporte = e.codigo_reporte
                    LEFT JOIN ( SELECT m.codigo_reporte, m.anio_operacion, m.semana_operacion, sum(m.monto) AS btb_pago_en_planilla
                                FROM ( SELECT y.anio_operacion, y.semana_operacion, y.codigo_reporte, x.monto
                                       FROM ( SELECT  0 AS codigo, 
                                                      monto
	                                          FROM db_contabilidad.cuenta_por_cobrar
	                                          WHERE codigo_estado = @CodigoEstadoParaIncluirEnReporte
                                                AND codigo_operacion = @CodigoBackToBackPago
		                                        AND carga_inicial = 0 
		                                        AND anio_operacion = 0 
		                                        AND semana_operacion = 0
		                                        AND codigo_reporte = 0
                                            ) x 
                                       INNER JOIN ( SELECT TOP(1) 
                                                           0 AS codigo, 
                                                           anio AS anio_operacion, 
                                                           numero_semana as semana_operacion, 
                                                           codigo_reporte  
                                                    FROM db_tesoreria.reporte_caja 
                                                    WHERE codigo_estado = @CodigoEstadoReporteDeCajaPorRevisarEnContabilidad
                                                    ORDER BY codigo_reporte asc 
                                                  ) y
                                       ON x.codigo = y.codigo
                                    ) m                                
                                GROUP BY m.codigo_reporte, m.anio_operacion, m.semana_operacion
                              ) f
                    ON x.anio_operacion = f.anio_operacion AND x.semana_operacion = f.semana_operacion  AND x.codigo_reporte = f.codigo_reporte

                    UNION 

                    SELECT x.codigo_reporte,
                           x.anio_operacion,
	                       x.semana_operacion, 
	                       COALESCE(y.anticipo_liquidable,0) AS anticipo_liquidable, 
	                       COALESCE(z.devolucion_anticipo_liquidable,0) AS devolucion_anticipo_liquidable,
	                       COALESCE(w.anticipo_salario,0) AS anticipo_salario,
	                       COALESCE(p.descuento_anticipo_salario,0) AS descuento_anticipo_salario,
	                       COALESCE(a.prestamo,0) AS prestamo,
	                       COALESCE(b.abono_prestamo,0) AS abono_prestamo,
	                       COALESCE(c.back_to_back,0) AS back_to_back,
                           COALESCE(f.btb_pago_en_planilla,0) AS btb_pago_en_planilla,
	                       COALESCE(d.retiro_socios,0) AS retiro_socios,
	                       COALESCE(e.devolucion_socios,0) AS devolucion_socios,
                           1 AS bloqueado,
                           x.codigo_estado,
                           x.estado,
                           CASE 
                             WHEN x.codigo_estado = 2 THEN 1
                             ELSE 0
                           END AS permiso_anular 
                    FROM ( SELECT r.codigo_reporte, r.anio_operacion, r.semana_operacion, r.codigo_estado, s.nombre AS estado
	                       FROM db_contabilidad.cxc_reporte r
                           INNER JOIN db_contabilidad.estado_reporte_cxc s
                           ON r.codigo_estado = s.codigo_estado_reporte 
	                       WHERE r.codigo_estado = 2
                    ) x
                    LEFT JOIN ( SELECT codigo_reporte, anio_operacion, semana_operacion, sum(monto) AS anticipo_liquidable
                                FROM db_contabilidad.cuenta_por_cobrar
                                WHERE codigo_operacion = @CodigoAnticipoLiquidable
                                GROUP BY codigo_reporte, anio_operacion, semana_operacion
                              ) y
                    ON x.anio_operacion = y.anio_operacion AND x.semana_operacion = y.semana_operacion AND x.codigo_reporte = y.codigo_reporte
                    LEFT JOIN ( SELECT codigo_reporte, anio_operacion, semana_operacion, sum(monto) AS devolucion_anticipo_liquidable
                                FROM db_contabilidad.cuenta_por_cobrar
                                WHERE codigo_operacion = @CodigoDevolucionAnticipoLiquidable
                                GROUP BY codigo_reporte, anio_operacion, semana_operacion
                              ) z
                    ON x.anio_operacion = z.anio_operacion AND x.semana_operacion = z.semana_operacion  AND x.codigo_reporte = z.codigo_reporte
                    LEFT JOIN ( SELECT codigo_reporte, anio_operacion, semana_operacion, sum(monto) AS anticipo_salario
                                FROM db_contabilidad.cuenta_por_cobrar
                                WHERE codigo_operacion = @CodigoAnticipoSalario
                                GROUP BY codigo_reporte, anio_operacion, semana_operacion
                              ) w
                    ON x.anio_operacion = w.anio_operacion AND x.semana_operacion = w.semana_operacion  AND x.codigo_reporte = w.codigo_reporte
                    LEFT JOIN ( SELECT codigo_reporte, anio_operacion, semana_operacion, sum(monto) AS descuento_anticipo_salario
                                FROM db_contabilidad.cuenta_por_cobrar
                                WHERE codigo_operacion = @CodigoDescuentoAnticipoSalario
                                GROUP BY codigo_reporte, anio_operacion, semana_operacion
                              ) p
                    ON x.anio_operacion = p.anio_operacion AND x.semana_operacion = p.semana_operacion  AND x.codigo_reporte = p.codigo_reporte
                    LEFT JOIN ( SELECT codigo_reporte, anio_operacion, semana_operacion, sum(monto) AS prestamo
                                FROM db_contabilidad.cuenta_por_cobrar
                                WHERE codigo_operacion = @CodigoPrestamo
                                GROUP BY codigo_reporte, anio_operacion, semana_operacion
                              ) a
                    ON x.anio_operacion = a.anio_operacion AND x.semana_operacion = a.semana_operacion  AND x.codigo_reporte = a.codigo_reporte
                    LEFT JOIN ( SELECT codigo_reporte, anio_operacion, semana_operacion, sum(monto) AS abono_prestamo
                                FROM db_contabilidad.cuenta_por_cobrar
                                WHERE codigo_operacion = @CodigoAbonoPrestamo
                                GROUP BY codigo_reporte, anio_operacion, semana_operacion
                              ) b
                    ON x.anio_operacion = b.anio_operacion AND x.semana_operacion = b.semana_operacion  AND x.codigo_reporte = b.codigo_reporte
                    LEFT JOIN ( SELECT codigo_reporte, anio_operacion, semana_operacion, sum(monto) AS back_to_back
                                FROM db_contabilidad.cuenta_por_cobrar
                                WHERE codigo_operacion = @CodigoBackToBack
                                GROUP BY codigo_reporte, anio_operacion, semana_operacion
                              ) c
                    ON x.anio_operacion = c.anio_operacion AND x.semana_operacion = c.semana_operacion  AND x.codigo_reporte = c.codigo_reporte
                    LEFT JOIN ( SELECT codigo_reporte, anio_operacion, semana_operacion, sum(monto) AS retiro_socios
                                FROM db_contabilidad.cuenta_por_cobrar
                                WHERE codigo_operacion = @CodigoRetiroSocios
                                GROUP BY codigo_reporte, anio_operacion, semana_operacion
                              ) d
                    ON x.anio_operacion = d.anio_operacion AND x.semana_operacion = d.semana_operacion  AND x.codigo_reporte = d.codigo_reporte
                    LEFT JOIN ( SELECT codigo_reporte, anio_operacion, semana_operacion, sum(monto) AS devolucion_socios
                                FROM db_contabilidad.cuenta_por_cobrar
                                WHERE codigo_operacion = @CodigoDevolucionSocios
                                GROUP BY codigo_reporte, anio_operacion, semana_operacion
                              ) e
                    ON x.anio_operacion = e.anio_operacion AND x.semana_operacion = e.semana_operacion  AND x.codigo_reporte = e.codigo_reporte
                    LEFT JOIN ( SELECT codigo_reporte, anio_operacion, semana_operacion, sum(monto) AS btb_pago_en_planilla
                                FROM db_contabilidad.cuenta_por_cobrar
                                WHERE codigo_operacion = @CodigoBackToBackPago
                                GROUP BY codigo_reporte, anio_operacion, semana_operacion
                              ) f
                    ON x.anio_operacion = f.anio_operacion AND x.semana_operacion = f.semana_operacion  AND x.codigo_reporte = f.codigo_reporte";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        // CommandType, Gets or sets a value indicating how the CommandText property is to be interpreted.
                        // The Text CommandType.Text is used when the command is raw SQL
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoAnticipoLiquidable", Constantes.CuentaPorCobrar.Operacion.ANTICIPO_LIQUIDABLE);
                        cmd.Parameters.AddWithValue("@CodigoDevolucionAnticipoLiquidable", Constantes.CuentaPorCobrar.Operacion.DEVOLUCION_ANTICIPO_LIQUIDABLE);
                        cmd.Parameters.AddWithValue("@CodigoAnticipoSalario", Constantes.CuentaPorCobrar.Operacion.ANTICIPO_SALARIO);
                        cmd.Parameters.AddWithValue("@CodigoDescuentoAnticipoSalario", Constantes.CuentaPorCobrar.Operacion.DESCUENTO_PLANILLA_POR_ANTICIPO_SALARIO);
                        cmd.Parameters.AddWithValue("@CodigoPrestamo", Constantes.CuentaPorCobrar.Operacion.PRESTAMO);
                        cmd.Parameters.AddWithValue("@CodigoAbonoPrestamo", Constantes.CuentaPorCobrar.Operacion.ABONO_PRESTAMO);
                        cmd.Parameters.AddWithValue("@CodigoBackToBack", Constantes.CuentaPorCobrar.Operacion.BACK_TO_BACK);
                        cmd.Parameters.AddWithValue("@CodigoBackToBackPago", Constantes.CuentaPorCobrar.Operacion.BACK_TO_BACK_PAGO_PLANILLA);
                        cmd.Parameters.AddWithValue("@CodigoRetiroSocios", Constantes.CuentaPorCobrar.Operacion.RETIRO_SOCIOS);
                        cmd.Parameters.AddWithValue("@CodigoDevolucionSocios", Constantes.CuentaPorCobrar.Operacion.DEVOLUCION_SOCIOS);
                        cmd.Parameters.AddWithValue("@CodigoEstadoParaIncluirEnReporte", Constantes.CuentaPorCobrar.Estado.PARA_INCLUIR_EN_REPORTE);
                        cmd.Parameters.AddWithValue("@CodigoEstadoReporteDeCajaPorRevisarEnContabilidad", Constantes.ReporteCaja.Estado.POR_REVISAR);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            CuentaPorCobrarReporteCLS objCuentaPorCobrarReporteCLS;
                            lista = new List<CuentaPorCobrarReporteCLS>();
                            int postCodigoReporte = dr.GetOrdinal("codigo_reporte");
                            int postAnioOperacion = dr.GetOrdinal("anio_operacion");
                            int postSemanaOperacion = dr.GetOrdinal("semana_operacion");
                            int postAnticipoLiquidable = dr.GetOrdinal("anticipo_liquidable");
                            int postDevolucionAnticipoLiquidable = dr.GetOrdinal("devolucion_anticipo_liquidable");
                            int postAnticipoSalario = dr.GetOrdinal("anticipo_salario");
                            int postDescuentoAnticipoSalario = dr.GetOrdinal("descuento_anticipo_salario");
                            int postPrestamo = dr.GetOrdinal("prestamo");
                            int postAbonoPrestamo = dr.GetOrdinal("abono_prestamo");
                            int postBackToBackDevolucion = dr.GetOrdinal("back_to_back");
                            int postBackToBackPagoPlanilla = dr.GetOrdinal("btb_pago_en_planilla");
                            int postRetiroSocios = dr.GetOrdinal("retiro_socios");
                            int postDevolucionSocios = dr.GetOrdinal("devolucion_socios");
                            int postBloqueado = dr.GetOrdinal("bloqueado");
                            int postCodigoEstadoReporte = dr.GetOrdinal("codigo_estado");
                            int postEstadoReporte = dr.GetOrdinal("estado");
                            int postPermisoAnular = dr.GetOrdinal("permiso_anular");
                            while (dr.Read())
                            {
                                objCuentaPorCobrarReporteCLS = new CuentaPorCobrarReporteCLS();
                                objCuentaPorCobrarReporteCLS.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objCuentaPorCobrarReporteCLS.AnioOperacion = dr.GetInt16(postAnioOperacion);
                                objCuentaPorCobrarReporteCLS.SemanaOperacion = dr.GetByte(postSemanaOperacion);
                                objCuentaPorCobrarReporteCLS.AnticipoLiquidable = dr.GetDecimal(postAnticipoLiquidable);
                                objCuentaPorCobrarReporteCLS.DevolucionAnticipoLiquidable = dr.GetDecimal(postDevolucionAnticipoLiquidable);
                                objCuentaPorCobrarReporteCLS.AnticipoSalario = dr.GetDecimal(postAnticipoSalario);
                                objCuentaPorCobrarReporteCLS.DescuentoAnticipoSalario = dr.GetDecimal(postDescuentoAnticipoSalario);
                                objCuentaPorCobrarReporteCLS.Prestamo = dr.GetDecimal(postPrestamo);
                                objCuentaPorCobrarReporteCLS.AbonoPrestamo = dr.GetDecimal(postAbonoPrestamo);
                                objCuentaPorCobrarReporteCLS.BackToBack = dr.GetDecimal(postBackToBackDevolucion);
                                objCuentaPorCobrarReporteCLS.BackToBackPagoPlanilla = dr.GetDecimal(postBackToBackPagoPlanilla);
                                objCuentaPorCobrarReporteCLS.RetiroSocios = dr.GetDecimal(postRetiroSocios);
                                objCuentaPorCobrarReporteCLS.DevolucionSocios = dr.GetDecimal(postDevolucionSocios);
                                objCuentaPorCobrarReporteCLS.Bloqueado = (byte)dr.GetInt32(postBloqueado);
                                objCuentaPorCobrarReporteCLS.CodigoEstado = (byte)dr.GetInt32(postCodigoEstadoReporte);
                                objCuentaPorCobrarReporteCLS.Estado = dr.GetString(postEstadoReporte);
                                objCuentaPorCobrarReporteCLS.PermisoAnular = (byte)dr.GetInt32(postPermisoAnular);

                                lista.Add(objCuentaPorCobrarReporteCLS);
                            }//fin while
                        }// fin if
                    }// fin using
                    conexion.Close();

                }
                catch (Exception)
                {
                    conexion.Close();
                    lista = null;

                }
                return lista;
            }
        }

        // Revisado
        public string GenerarReporteCuentaPorCobrar(int codigoReporte, int anioOperacion, int semanaOperacion, string idUsuario)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaContabilidad))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_contabilidad.uspGenerarReporteCuentaPorCobrar", conexion))
                    {
                        // le indico que es un procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CodigoReporte", codigoReporte);
                        cmd.Parameters.AddWithValue("@Anio", anioOperacion);
                        cmd.Parameters.AddWithValue("@NumeroSemana", semanaOperacion);
                        cmd.Parameters.AddWithValue("@UsuarioIng", idUsuario);

                        //Set SqlParameter
                        SqlParameter outParameter = new SqlParameter
                        {
                            ParameterName = "@Resultado", //Parameter name defined in stored procedure
                            SqlDbType = SqlDbType.Int, //Data Type of Parameter
                            Direction = ParameterDirection.Output //Specify the parameter as ouput
                        };

                        //add the parameter to the SqlCommand object
                        cmd.Parameters.Add(outParameter);
                        cmd.ExecuteReader();
                        resultado = outParameter.Value.ToString().Trim();
                        conexion.Close();
                    }// fin using
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    resultado = "Error [0]: " + ex.Message;
                }
                return resultado;
            }
        }

        // Revisado
        public List<CuentaPorCobrarReporteCLS> GetReportesCuentasPorCobrarParaConsulta()
        {
            List<CuentaPorCobrarReporteCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaContabilidad))
            {
                try
                {
                    string sql = @"
                    SELECT x.codigo_reporte,
                           x.anio_operacion,
	                       x.semana_operacion, 
	                       COALESCE(y.anticipo_liquidable,0) AS anticipo_liquidable, 
	                       COALESCE(z.devolucion_anticipo_liquidable,0) AS devolucion_anticipo_liquidable,
	                       COALESCE(w.anticipo_salario,0) AS anticipo_salario,
	                       COALESCE(p.descuento_anticipo_salario,0) AS descuento_anticipo_salario,
	                       COALESCE(a.prestamo,0) AS prestamo,
	                       COALESCE(b.abono_prestamo,0) AS abono_prestamo,
	                       COALESCE(c.back_to_back,0) AS back_to_back,
	                       COALESCE(d.retiro_socios,0) AS retiro_socios,
	                       COALESCE(e.devolucion_socios,0) AS devolucion_socios,
                           x.codigo_estado,
                           x.estado
                    FROM ( SELECT r.codigo_reporte, r.anio_operacion, r.semana_operacion, r.codigo_estado, s.nombre AS estado
	                       FROM db_contabilidad.cxc_reporte r
                           INNER JOIN db_contabilidad.estado_reporte_cxc s
                           ON r.codigo_estado = s.codigo_estado_reporte 
	                       WHERE r.codigo_estado = 3
                    ) x
                    LEFT JOIN ( SELECT codigo_reporte, anio_operacion, semana_operacion, sum(monto) AS anticipo_liquidable
                                FROM db_contabilidad.cuenta_por_cobrar
                                WHERE codigo_operacion = @CodigoAnticipoLiquidable
                                GROUP BY codigo_reporte, anio_operacion, semana_operacion
                              ) y
                    ON x.anio_operacion = y.anio_operacion AND x.semana_operacion = y.semana_operacion AND x.codigo_reporte = y.codigo_reporte
                    LEFT JOIN ( SELECT codigo_reporte, anio_operacion, semana_operacion, sum(monto) AS devolucion_anticipo_liquidable
                                FROM db_contabilidad.cuenta_por_cobrar
                                WHERE codigo_operacion = @CodigoDevolucionAnticipoLiquidable
                                GROUP BY codigo_reporte, anio_operacion, semana_operacion
                              ) z
                    ON x.anio_operacion = z.anio_operacion AND x.semana_operacion = z.semana_operacion AND x.codigo_reporte = z.codigo_reporte
                    LEFT JOIN ( SELECT codigo_reporte, anio_operacion, semana_operacion, sum(monto) AS anticipo_salario
                                FROM db_contabilidad.cuenta_por_cobrar
                                WHERE codigo_operacion = @CodigoAnticipoSalario
                                GROUP BY codigo_reporte, anio_operacion, semana_operacion
                              ) w
                    ON x.anio_operacion = w.anio_operacion AND x.semana_operacion = w.semana_operacion AND x.codigo_reporte = w.codigo_reporte
                    LEFT JOIN ( SELECT codigo_reporte, anio_operacion, semana_operacion, sum(monto) AS descuento_anticipo_salario
                                FROM db_contabilidad.cuenta_por_cobrar
                                WHERE codigo_operacion = @CodigoDescuentoAnticipoSalario
                                GROUP BY codigo_reporte, anio_operacion, semana_operacion
                              ) p
                    ON x.anio_operacion = p.anio_operacion AND x.semana_operacion = p.semana_operacion AND x.codigo_reporte = p.codigo_reporte
                    LEFT JOIN ( SELECT codigo_reporte, anio_operacion, semana_operacion, sum(monto) AS prestamo
                                FROM db_contabilidad.cuenta_por_cobrar
                                WHERE codigo_operacion = @CodigoPrestamo
                                GROUP BY codigo_reporte, anio_operacion, semana_operacion
                              ) a
                    ON x.anio_operacion = a.anio_operacion AND x.semana_operacion = a.semana_operacion AND x.codigo_reporte = a.codigo_reporte
                    LEFT JOIN ( SELECT codigo_reporte, anio_operacion, semana_operacion, sum(monto) AS abono_prestamo
                                FROM db_contabilidad.cuenta_por_cobrar
                                WHERE codigo_operacion = @CodigoAbonoPrestamo
                                GROUP BY codigo_reporte, anio_operacion, semana_operacion
                              ) b
                    ON x.anio_operacion = b.anio_operacion AND x.semana_operacion = b.semana_operacion AND x.codigo_reporte = b.codigo_reporte
                    LEFT JOIN ( SELECT codigo_reporte, anio_operacion, semana_operacion, sum(monto) AS back_to_back
                                FROM db_contabilidad.cuenta_por_cobrar
                                WHERE codigo_operacion = @CodigoBackToBack
                                GROUP BY codigo_reporte, anio_operacion, semana_operacion
                              ) c
                    ON x.anio_operacion = c.anio_operacion AND x.semana_operacion = c.semana_operacion AND x.codigo_reporte = c.codigo_reporte
                    LEFT JOIN ( SELECT codigo_reporte, anio_operacion, semana_operacion, sum(monto) AS retiro_socios
                                FROM db_contabilidad.cuenta_por_cobrar
                                WHERE codigo_operacion = @CodigoRetiroSocios
                                GROUP BY codigo_reporte, anio_operacion, semana_operacion
                              ) d
                    ON x.anio_operacion = d.anio_operacion AND x.semana_operacion = d.semana_operacion AND x.codigo_reporte = d.codigo_reporte
                    LEFT JOIN ( SELECT codigo_reporte, anio_operacion, semana_operacion, sum(monto) AS devolucion_socios
                                FROM db_contabilidad.cuenta_por_cobrar
                                WHERE codigo_operacion = @CodigoDevolucionSocios
                                GROUP BY codigo_reporte, anio_operacion, semana_operacion
                              ) e
                    ON x.anio_operacion = e.anio_operacion AND x.semana_operacion = e.semana_operacion AND x.codigo_reporte = e.codigo_reporte";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        // CommandType, Gets or sets a value indicating how the CommandText property is to be interpreted.
                        // The Text CommandType.Text is used when the command is raw SQL
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoAnticipoLiquidable", Constantes.CuentaPorCobrar.Operacion.ANTICIPO_LIQUIDABLE);
                        cmd.Parameters.AddWithValue("@CodigoDevolucionAnticipoLiquidable", Constantes.CuentaPorCobrar.Operacion.DEVOLUCION_ANTICIPO_LIQUIDABLE);
                        cmd.Parameters.AddWithValue("@CodigoAnticipoSalario", Constantes.CuentaPorCobrar.Operacion.ANTICIPO_SALARIO);
                        cmd.Parameters.AddWithValue("@CodigoDescuentoAnticipoSalario", Constantes.CuentaPorCobrar.Operacion.DESCUENTO_PLANILLA_POR_ANTICIPO_SALARIO);
                        cmd.Parameters.AddWithValue("@CodigoPrestamo", Constantes.CuentaPorCobrar.Operacion.PRESTAMO);
                        cmd.Parameters.AddWithValue("@CodigoAbonoPrestamo", Constantes.CuentaPorCobrar.Operacion.ABONO_PRESTAMO);
                        cmd.Parameters.AddWithValue("@CodigoBackToBack", Constantes.CuentaPorCobrar.Operacion.BACK_TO_BACK);
                        cmd.Parameters.AddWithValue("@CodigoRetiroSocios", Constantes.CuentaPorCobrar.Operacion.RETIRO_SOCIOS);
                        cmd.Parameters.AddWithValue("@CodigoDevolucionSocios", Constantes.CuentaPorCobrar.Operacion.DEVOLUCION_SOCIOS);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            CuentaPorCobrarReporteCLS objCuentaPorCobrarReporteCLS;
                            lista = new List<CuentaPorCobrarReporteCLS>();
                            int postCodigoReporte = dr.GetOrdinal("codigo_reporte");
                            int postAnioOperacion = dr.GetOrdinal("anio_operacion");
                            int postSemanaOperacion = dr.GetOrdinal("semana_operacion");
                            int postAnticipoLiquidable = dr.GetOrdinal("anticipo_liquidable");
                            int postDevolucionAnticipoLiquidable = dr.GetOrdinal("devolucion_anticipo_liquidable");
                            int postAnticipoSalario = dr.GetOrdinal("anticipo_salario");
                            int postDescuentoAnticipoSalario = dr.GetOrdinal("descuento_anticipo_salario");
                            int postPrestamo = dr.GetOrdinal("prestamo");
                            int postAbonoPrestamo = dr.GetOrdinal("abono_prestamo");
                            int postBackToBack = dr.GetOrdinal("back_to_back");
                            int postRetiroSocios = dr.GetOrdinal("retiro_socios");
                            int postDevolucionSocios = dr.GetOrdinal("devolucion_socios");
                            int postCodigoEstadoReporte = dr.GetOrdinal("codigo_estado");
                            int postEstadoReporte = dr.GetOrdinal("estado");
                            while (dr.Read())
                            {
                                objCuentaPorCobrarReporteCLS = new CuentaPorCobrarReporteCLS();
                                objCuentaPorCobrarReporteCLS.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objCuentaPorCobrarReporteCLS.AnioOperacion = dr.GetInt16(postAnioOperacion);
                                objCuentaPorCobrarReporteCLS.SemanaOperacion = dr.GetByte(postSemanaOperacion);
                                objCuentaPorCobrarReporteCLS.AnticipoLiquidable = dr.GetDecimal(postAnticipoLiquidable);
                                objCuentaPorCobrarReporteCLS.DevolucionAnticipoLiquidable = dr.GetDecimal(postDevolucionAnticipoLiquidable);
                                objCuentaPorCobrarReporteCLS.AnticipoSalario = dr.GetDecimal(postAnticipoSalario);
                                objCuentaPorCobrarReporteCLS.DescuentoAnticipoSalario = dr.GetDecimal(postDescuentoAnticipoSalario);
                                objCuentaPorCobrarReporteCLS.Prestamo = dr.GetDecimal(postPrestamo);
                                objCuentaPorCobrarReporteCLS.AbonoPrestamo = dr.GetDecimal(postAbonoPrestamo);
                                objCuentaPorCobrarReporteCLS.BackToBack = dr.GetDecimal(postBackToBack);
                                objCuentaPorCobrarReporteCLS.RetiroSocios = dr.GetDecimal(postRetiroSocios);
                                objCuentaPorCobrarReporteCLS.DevolucionSocios = dr.GetDecimal(postDevolucionSocios);
                                objCuentaPorCobrarReporteCLS.CodigoEstado = dr.GetByte(postCodigoEstadoReporte);
                                objCuentaPorCobrarReporteCLS.Estado = dr.GetString(postEstadoReporte);
                                lista.Add(objCuentaPorCobrarReporteCLS);
                            }//fin while
                        }// fin if
                    }// fin using
                    conexion.Close();

                }
                catch (Exception)
                {
                    conexion.Close();
                    lista = null;

                }
                return lista;
            }
        }

        // Revisado
        public List<CuentaPorCobrarReporteDetalleCLS> GetDetalleReporteCuentasPorCobrar(int codigoReporte, int anioOperacion, int semanaOperacion)
        {
            List<CuentaPorCobrarReporteDetalleCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaContabilidad))
            {
                try
                {
                    string sql = @"
                    SELECT x.codigo_entidad, 
		                    db_tesoreria.GetNombreEntidadCompleto(x.codigo_categoria, x.codigo_entidad) AS entidad,
                            db_tesoreria.GetNombreEmpresa(x.codigo_categoria,x.codigo_entidad) AS nombre_empresa,
                            x.codigo_reporte,
	                        x.anio_operacion,
		                    x.semana_operacion,
		                    x.codigo_operacion,
		                    y.nombre_operacion,
		                    x.codigo_categoria,
		                    z.nombre AS categoria,
		                    x.saldo_inicial,
                            X.monto_solicitado,
		                    x.monto_devolucion,
		                    x.saldo_final
                    FROM db_contabilidad.cxc_reporte_detalle x
                    INNER JOIN db_tesoreria.operacion y
                    ON x.codigo_operacion = y.codigo_operacion
                    INNER JOIN db_tesoreria.entidad_categoria z
                    ON x.codigo_categoria = z.codigo_categoria_entidad
                    WHERE x.codigo_reporte = @CodigoReporte
                      AND x.anio_operacion = @AnioOperacion 
                      AND x.semana_operacion = @SemanaOperacion";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        // CommandType, Gets or sets a value indicating how the CommandText property is to be interpreted.
                        // The Text CommandType.Text is used when the command is raw SQL
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoReporte", codigoReporte);
                        cmd.Parameters.AddWithValue("@AnioOperacion", anioOperacion);
                        cmd.Parameters.AddWithValue("@SemanaOperacion", semanaOperacion);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            CuentaPorCobrarReporteDetalleCLS objCuentaPorCobrarReporteCLS;
                            lista = new List<CuentaPorCobrarReporteDetalleCLS>();
                            int postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            int postEntidad = dr.GetOrdinal("entidad");
                            int postNombreEmpresa = dr.GetOrdinal("nombre_empresa");
                            int postCodigoReporte = dr.GetOrdinal("codigo_reporte");
                            int postAnioOperacion = dr.GetOrdinal("anio_operacion");
                            int postSemanaOperacion = dr.GetOrdinal("semana_operacion");
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postOperacion = dr.GetOrdinal("nombre_operacion");
                            int postCodigoCategoria = dr.GetOrdinal("codigo_categoria");
                            int postCategoria = dr.GetOrdinal("categoria");
                            int postSaldoInicial = dr.GetOrdinal("saldo_inicial");
                            int postMontoSolicitado = dr.GetOrdinal("monto_solicitado");
                            int postMontoDevolucion = dr.GetOrdinal("monto_devolucion");
                            int postSaldoFinal = dr.GetOrdinal("saldo_final");

                            while (dr.Read())
                            {
                                objCuentaPorCobrarReporteCLS = new CuentaPorCobrarReporteDetalleCLS();
                                objCuentaPorCobrarReporteCLS.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objCuentaPorCobrarReporteCLS.NombreEntidad = dr.GetString(postEntidad);
                                objCuentaPorCobrarReporteCLS.NombreEmpresa = dr.IsDBNull(postNombreEmpresa) ? "" : dr.GetString(postNombreEmpresa);
                                objCuentaPorCobrarReporteCLS.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objCuentaPorCobrarReporteCLS.AnioOperacion = dr.GetInt16(postAnioOperacion);
                                objCuentaPorCobrarReporteCLS.SemanaOperacion = dr.GetByte(postSemanaOperacion);
                                objCuentaPorCobrarReporteCLS.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objCuentaPorCobrarReporteCLS.Operacion = dr.GetString(postOperacion);
                                objCuentaPorCobrarReporteCLS.CodigoCategoria = dr.GetInt16(postCodigoCategoria);
                                objCuentaPorCobrarReporteCLS.Categoria = dr.GetString(postCategoria);
                                objCuentaPorCobrarReporteCLS.SaldoInicial = dr.GetDecimal(postSaldoInicial);
                                objCuentaPorCobrarReporteCLS.MontoSolicitado = dr.GetDecimal(postMontoSolicitado);
                                objCuentaPorCobrarReporteCLS.MontoDevolucion = dr.GetDecimal(postMontoDevolucion);
                                objCuentaPorCobrarReporteCLS.SaldoFinal = dr.GetDecimal(postSaldoFinal);
                                
                                lista.Add(objCuentaPorCobrarReporteCLS);
                            }//fin while
                        }// fin if
                    }// fin using
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                    lista = null;

                }
                return lista;
            }
        }

        // Revisado
        public List<CuentaPorCobrarReporteDetalleCLS> GetDetalleReporteCuentasPorCobrarGeneracion(int codigoReporte, int anioOperacion, int semanaOperacion)
        {
            List<CuentaPorCobrarReporteDetalleCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaContabilidad))
            {
                try
                {
                    string sql = @"
                    SELECT m.codigo_entidad,
                           db_tesoreria.GetNombreEntidadCompleto(m.codigo_categoria, m.codigo_entidad) AS entidad,
                           @CodigoReporte AS codigo_reporte, 
                           @AnioOperacion AS anio_operacion,
		                   @SemanaOperacion AS semana_operacion,
		                   m.codigo_operacion,
		                   n.nombre_operacion, 
		                   m.codigo_categoria,
                           o.nombre AS categoria, 
		                   m.saldo_inicial,
		                   m.monto_devolucion_semana,
		                   m.saldo_final
	                FROM ( SELECT x.codigo_operacion,
				                  x.codigo_entidad,
				                  x.codigo_categoria,	
				                  (COALESCE(x.saldo_inicial,0) -  COALESCE(y.monto_devolucion,0)) AS saldo_inicial,
				                  COALESCE(y.monto_devolucion,0) AS monto_devolucion,
                                  (COALESCE(x.saldo_inicial,0) - COALESCE(y.monto_devolucion,0) - COALESCE(z.monto_devolucion_semana,0)) AS saldo_final,
				                  COALESCE(z.monto_devolucion_semana,0) AS monto_devolucion_semana,
				                  CASE
					                WHEN ((COALESCE(x.saldo_inicial,0) - COALESCE(y.monto_devolucion,0)) = 0 AND COALESCE(z.monto_devolucion_semana,0) <> 0) THEN 'NO'
					                ELSE 'SI'
				                  END AS saldo_pendiente
		                   FROM ( SELECT codigo_categoria, codigo_entidad, codigo_operacion, sum(monto) AS saldo_inicial
				                  FROM db_contabilidad.cuenta_por_cobrar
				                  WHERE codigo_estado IN (2,4) AND codigo_operacion IN (38,62,41,65,26)
				                  GROUP BY codigo_categoria, codigo_entidad, codigo_operacion 
				                ) x 
                           LEFT JOIN ( SELECT codigo_categoria, 
									          codigo_entidad, 
									          CASE
										         WHEN codigo_operacion = 39 THEN 38
										         WHEN codigo_operacion = 4 THEN 62
										         WHEN codigo_operacion = 42 THEN 41
										         WHEN codigo_operacion = 17 THEN 26
										         WHEN codigo_operacion = 50 THEN 65
										         ELSE 0
									          END codigo_operacion,
									          sum(monto) AS monto_devolucion
								       FROM db_contabilidad.cuenta_por_cobrar
								       WHERE codigo_estado = 2 AND codigo_operacion IN (39,4,42,17,50)
								       GROUP BY codigo_categoria, codigo_entidad, codigo_operacion 
							         ) y
				           ON x.codigo_categoria = y.codigo_categoria AND x.codigo_entidad = y.codigo_entidad AND x.codigo_operacion = y.codigo_operacion
				           LEFT JOIN ( SELECT codigo_categoria, 
								   codigo_entidad, 
								   CASE
									 WHEN codigo_operacion = 39 THEN 38
									 WHEN codigo_operacion = 4 THEN 62
									 WHEN codigo_operacion = 42 THEN 41
									 WHEN codigo_operacion = 17 THEN 26
									 WHEN codigo_operacion = 50 THEN 65
								     ELSE 0
								   END codigo_operacion,
								   sum(monto) AS monto_devolucion_semana
							FROM db_contabilidad.cuenta_por_cobrar
							WHERE codigo_estado = 4 
                              AND codigo_operacion IN (39,4,42,17,50)
                              AND codigo_reporte = @CodigoReporte  
                              AND anio_operacion = @AnioOperacion 
                              AND semana_operacion = @SemanaOperacion
							GROUP BY codigo_categoria, codigo_entidad, codigo_operacion 
						  ) z
				          ON x.codigo_categoria = z.codigo_categoria AND x.codigo_entidad = z.codigo_entidad AND x.codigo_operacion = z.codigo_operacion	
		            ) m 
                    INNER JOIN db_tesoreria.operacion n
                    ON m.codigo_operacion = n.codigo_operacion
                    INNER JOIN db_tesoreria.entidad_categoria o
                    ON m.codigo_categoria = o.codigo_categoria_entidad
                    WHERE m.saldo_pendiente = 'SI' OR m.monto_devolucion_semana > 0";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        // CommandType, Gets or sets a value indicating how the CommandText property is to be interpreted.
                        // The Text CommandType.Text is used when the command is raw SQL
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoReporte", codigoReporte);
                        cmd.Parameters.AddWithValue("@AnioOperacion", anioOperacion);
                        cmd.Parameters.AddWithValue("@SemanaOperacion", semanaOperacion);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            CuentaPorCobrarReporteDetalleCLS objCuentaPorCobrarReporteCLS;
                            lista = new List<CuentaPorCobrarReporteDetalleCLS>();
                            int postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            int postEntidad = dr.GetOrdinal("entidad");
                            int postCodigoReporte = dr.GetOrdinal("codigo_reporte");
                            int postAnioOperacion = dr.GetOrdinal("anio_operacion");
                            int postSemanaOperacion = dr.GetOrdinal("semana_operacion");
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postOperacion = dr.GetOrdinal("nombre_operacion");
                            int postCodigoCategoria = dr.GetOrdinal("codigo_categoria");
                            int postCategoria = dr.GetOrdinal("categoria");
                            int postSaldoInicial = dr.GetOrdinal("saldo_inicial");
                            int postMontoDevolucion = dr.GetOrdinal("monto_devolucion_semana");
                            int postSaldoFinal = dr.GetOrdinal("saldo_final");

                            while (dr.Read())
                            {
                                objCuentaPorCobrarReporteCLS = new CuentaPorCobrarReporteDetalleCLS();
                                objCuentaPorCobrarReporteCLS.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objCuentaPorCobrarReporteCLS.NombreEntidad = dr.GetString(postEntidad);
                                objCuentaPorCobrarReporteCLS.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objCuentaPorCobrarReporteCLS.AnioOperacion = (short)dr.GetInt32(postAnioOperacion);
                                objCuentaPorCobrarReporteCLS.SemanaOperacion = (byte)dr.GetInt32(postSemanaOperacion);
                                objCuentaPorCobrarReporteCLS.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objCuentaPorCobrarReporteCLS.Operacion = dr.GetString(postOperacion);
                                objCuentaPorCobrarReporteCLS.CodigoCategoria = dr.GetInt16(postCodigoCategoria);
                                objCuentaPorCobrarReporteCLS.Categoria = dr.GetString(postCategoria);
                                objCuentaPorCobrarReporteCLS.SaldoInicial = dr.GetDecimal(postSaldoInicial);
                                objCuentaPorCobrarReporteCLS.MontoDevolucion = dr.GetDecimal(postMontoDevolucion);
                                objCuentaPorCobrarReporteCLS.SaldoFinal = dr.GetDecimal(postSaldoFinal);
                                lista.Add(objCuentaPorCobrarReporteCLS);
                            }//fin while
                        }// fin if
                    }// fin using
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                    lista = null;

                }
                return lista;
            }
        }

        // Revisado
        public ReporteOperacionesCajaListCLS GetDetalleCorteCuentasPorCobrar(int anioOperacion, int semanaOperacion, int codigoReporte, int arqueo)
        {
            ReporteOperacionesCajaListCLS objReporte = new ReporteOperacionesCajaListCLS();
            using (SqlConnection conexion = new SqlConnection(cadenaContabilidad))
            {
                try
                {
                    string tableReporte = string.Empty;
                    if (arqueo == 1)
                    {
                        tableReporte = "db_contabilidad.cxc_reporte_detalle_arqueo";
                    }
                    else {
                        tableReporte = "db_contabilidad.cxc_reporte_detalle";
                    }

                    string sqlEncabezado = @"
                    SELECT convert(varchar, fecha, 103) as fecha_str
                    FROM db_admon.programacion_semanal
                    WHERE anio = @AnioOperacion
                      AND numero_semana = @SemanaOperacion
                    ORDER BY numero_dia ASC";

                    string sqlDetalle = @"
                    SELECT m.codigo_entidad,
                           m.entidad,
                           m.nombre_empresa,
                           m.codigo_operacion,
                           m.nombre_operacion,
                           m.codigo_categoria,
                           m.categoria,
                           m.saldo_inicial,
                           m.monto_solicitado,
                           m.monto_devolucion_semana,
                           m.saldo_final 
                    FROM (  SELECT x.codigo_entidad,
	                               db_tesoreria.GetNombreEntidadCompleto(x.codigo_categoria,x.codigo_entidad) AS entidad,
                                   db_tesoreria.GetNombreEmpresa(x.codigo_categoria,x.codigo_entidad) AS nombre_empresa,
                                   x.codigo_operacion,
	                               y.nombre_operacion,
	                               x.codigo_categoria,
	                               z.nombre AS categoria,
	                               x.saldo_inicial,
                                   x.monto_solicitado, 
	                               x.monto_devolucion AS monto_devolucion_semana,
	                               x.saldo_final
                            FROM " + tableReporte + @" x
                            INNER JOIN db_tesoreria.operacion y
                            ON x.codigo_operacion = y.codigo_operacion
                            INNER JOIN db_tesoreria.entidad_categoria z
                            ON x.codigo_categoria = z.codigo_categoria_entidad
                            WHERE x.estado <> 0 
                              AND x.anio_operacion = @AnioOperacion
                              AND x.semana_operacion = @SemanaOperacion
                              AND x.codigo_reporte = @CodigoReporte
                         ) m
                    ORDER BY m.nombre_empresa, m.entidad, m.nombre_operacion";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlDetalle, conexion))
                    {
                        // CommandType, Gets or sets a value indicating how the CommandText property is to be interpreted.
                        // The Text CommandType.Text is used when the command is raw SQL
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@AnioOperacion", anioOperacion);
                        cmd.Parameters.AddWithValue("@SemanaOperacion", semanaOperacion);
                        cmd.Parameters.AddWithValue("@CodigoReporte", codigoReporte);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            List<CuentaPorCobrarReporteDetalleCLS> lista = new List<CuentaPorCobrarReporteDetalleCLS>();
                            CuentaPorCobrarReporteDetalleCLS objCuentaPorCobrarReporteCLS;
                            int postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            int postEntidad = dr.GetOrdinal("entidad");
                            int postNombreEmpresa = dr.GetOrdinal("nombre_empresa");
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postOperacion = dr.GetOrdinal("nombre_operacion");
                            int postCodigoCategoria = dr.GetOrdinal("codigo_categoria");
                            int postCategoria = dr.GetOrdinal("categoria");
                            int postSaldoInicial = dr.GetOrdinal("saldo_inicial");
                            int postMontoSolicitado = dr.GetOrdinal("monto_solicitado");
                            int postMontoDevolucion = dr.GetOrdinal("monto_devolucion_semana");
                            int postSaldoFinal = dr.GetOrdinal("saldo_final");

                            while (dr.Read())
                            {
                                objCuentaPorCobrarReporteCLS = new CuentaPorCobrarReporteDetalleCLS();
                                objCuentaPorCobrarReporteCLS.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objCuentaPorCobrarReporteCLS.NombreEntidad = dr.GetString(postEntidad);
                                objCuentaPorCobrarReporteCLS.NombreEmpresa = dr.IsDBNull(postNombreEmpresa) ? "" : dr.GetString(postNombreEmpresa);
                                objCuentaPorCobrarReporteCLS.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objCuentaPorCobrarReporteCLS.Operacion = dr.GetString(postOperacion);
                                objCuentaPorCobrarReporteCLS.CodigoCategoria = dr.GetInt16(postCodigoCategoria);
                                objCuentaPorCobrarReporteCLS.Categoria = dr.GetString(postCategoria);
                                objCuentaPorCobrarReporteCLS.SaldoInicial = dr.GetDecimal(postSaldoInicial);
                                objCuentaPorCobrarReporteCLS.MontoSolicitado = dr.GetDecimal(postMontoSolicitado);
                                objCuentaPorCobrarReporteCLS.MontoDevolucion = dr.GetDecimal(postMontoDevolucion);
                                objCuentaPorCobrarReporteCLS.SaldoFinal = dr.GetDecimal(postSaldoFinal);
                                lista.Add(objCuentaPorCobrarReporteCLS);
                            }//fin while
                            objReporte.listaDetalleCuentasPorCobrar = lista;
                        }// fin if
                    }// fin using

                    using (SqlCommand cmd = new SqlCommand(sqlEncabezado, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@AnioOperacion", anioOperacion);
                        cmd.Parameters.AddWithValue("@SemanaOperacion", semanaOperacion);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {// Encabezado de las fechas de la semana indicada
                            int postFechaStr = dr.GetOrdinal("fecha_str");
                            List<ProgramacionSemanalCLS> listaEncabezadoFechas = new List<ProgramacionSemanalCLS>();
                            ProgramacionSemanalCLS objEncabezadoFechas;
                            while (dr.Read())
                            {
                                objEncabezadoFechas = new ProgramacionSemanalCLS();
                                objEncabezadoFechas.FechaStr = dr.GetString(postFechaStr);
                                listaEncabezadoFechas.Add(objEncabezadoFechas);
                            }
                            objReporte.listaEncabezadoFechas = listaEncabezadoFechas;
                        }
                    }// fin using


                    conexion.Close();
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    objReporte = null;

                }

                return objReporte;
            }
        }

        // Revisado
        public string AceptarReporteComoValido(int codigoReporte, int anioOperacion, int semanaOperacion, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaContabilidad))
            {
                try
                {
                    string sql = @"
                    UPDATE db_contabilidad.cxc_reporte 
                    SET codigo_estado = @CodigoEstadoReporteValido,
                        usuario_act = @UsuarioActualizacion,
                        fecha_act = @FechaActualizacion
                    WHERE anio_operacion = @AnioOperacion 
                      AND semana_operacion = @SemanaOperacion
                      AND codigo_reporte = @CodigoReporte";
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoReporteValido", Constantes.CuentaPorCobrar.EstadoReporte.VALIDO);
                        cmd.Parameters.AddWithValue("@UsuarioActualizacion", usuarioAct);
                        cmd.Parameters.AddWithValue("@FechaActualizacion", DateTime.Now);
                        cmd.Parameters.AddWithValue("@AnioOperacion", anioOperacion);
                        cmd.Parameters.AddWithValue("@SemanaOperacion", semanaOperacion);
                        cmd.Parameters.AddWithValue("@CodigoReporte", codigoReporte);

                        // rows sotred the number of rows affected
                        int rows = cmd.ExecuteNonQuery();
                    }
                    conexion.Close();
                    resultado = "OK";
                }
                catch (Exception ex)
                {
                    resultado = "Error [0]: " + ex.Message;
                    conexion.Close();
                }

                return resultado;
            }
        }

        // Revisado
        public string EliminarReporteGenerado(int codigoReporte, int anioOperacion, int semanaOperacion, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaContabilidad))
            {
                conexion.Open();

                SqlCommand cmd = conexion.CreateCommand();
                SqlTransaction transaction;

                // Start a local transaction.
                transaction = conexion.BeginTransaction("SampleTransaction");

                // Must assign both transaction object and connection
                // to Command object for a pending local transaction
                cmd.Connection = conexion;
                cmd.Transaction = transaction;

                try
                {
                    string sqlDeleteDetalleReporte = @"
                    DELETE FROM db_contabilidad.cxc_reporte_detalle 
                    WHERE anio_operacion = @AnioOperacion 
                      AND semana_operacion = @SemanaOperacion
                      AND codigo_reporte = @CodigoReporte";
                    cmd.CommandText = sqlDeleteDetalleReporte;
                    cmd.Parameters.AddWithValue("@AnioOperacion", anioOperacion);
                    cmd.Parameters.AddWithValue("@SemanaOperacion", semanaOperacion);
                    cmd.Parameters.AddWithValue("@CodigoReporte", codigoReporte);
                    cmd.ExecuteNonQuery();

                    string sqlDeleteReporte = @"
                    DELETE FROM db_contabilidad.cxc_reporte 
                    WHERE anio_operacion = @AnioOperacion 
                      AND semana_operacion = @SemanaOperacion
                      AND codigo_reporte = @CodigoReporte";
                    cmd.CommandText = sqlDeleteReporte;
                    cmd.ExecuteNonQuery();

                    string sqlUpdateTransaccionCxC = @"
                    UPDATE db_contabilidad.cuenta_por_cobrar 
                    SET codigo_estado = @CodigoEstadoRegistrado
                    WHERE anio_operacion = @AnioOperacion 
                      AND semana_operacion = @SemanaOperacion
                      AND codigo_reporte = @CodigoReporte";
                    cmd.CommandText = sqlUpdateTransaccionCxC;
                    cmd.Parameters.AddWithValue("@CodigoEstadoRegistrado", Constantes.CuentaPorCobrar.Estado.PARA_INCLUIR_EN_REPORTE);
                    cmd.ExecuteNonQuery();

                    // Attempt to commit the transaction.
                    transaction.Commit();
                    conexion.Close();

                    resultado = "OK";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    resultado = "Error [0]: " + ex.Message;
                    conexion.Close();
                }

                return resultado;
            }
        }


    }
}

