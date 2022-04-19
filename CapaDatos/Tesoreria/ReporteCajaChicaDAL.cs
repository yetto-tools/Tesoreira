using CapaEntidad.Administracion;
using CapaEntidad.Contabilidad;
using CapaEntidad.Tesoreria;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Tesoreria
{
    public class ReporteCajaChicaDAL: CadenaConexion
    {
        public List<ReporteCajaChicaCLS> ListarReportesCajaChica(string usuarioIng, int esSuperAdmin)
        {
            List<ReporteCajaChicaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string filterCajasChicasAsignadas = String.Empty;
                    if (esSuperAdmin != 1)
                    {
                        filterCajasChicasAsignadas = " AND y.codigo_caja_chica IN (select codigo_caja_chica from db_admon.usuario_caja_chica WHERE id_usuario = '" + usuarioIng + "')";
                    }
                  

                    string sql = @"
                    SELECT 0 AS codigo_reporte, 
	                       y.codigo_caja_chica,
                           a.nombre_caja_chica, 
	                       y.anio_operacion,
	                       y.semana_operacion,
                           (SELECT monto_disponible FROM db_admon.caja_chica WHERE codigo_caja_chica = y.codigo_caja_chica) AS monto_saldo_inicial,
                           COALESCE(z.monto_fiscal,0) As monto_fiscal,
                           COALESCE(w.monto_no_fiscal,0) As monto_no_fiscal,
	                       COALESCE(z.monto_fiscal,0) AS monto_egreso,
                           ((SELECT monto_disponible FROM db_admon.caja_chica WHERE codigo_caja_chica = y.codigo_caja_chica) - COALESCE(z.monto_fiscal,0)) AS monto_saldo, 
	                       1 AS codigo_estado, 
	                       'GENERAR REPORTE' estado,
                           '' AS observaciones,
                           0 AS bloqueado, 
                           0 AS permiso_anular,
                           0 AS permiso_editar

                    FROM ( SELECT codigo_caja_chica, anio_operacion, semana_operacion
                           FROM db_tesoreria.transaccion_caja_chica 
                           WHERE codigo_estado = 1 
                            AND codigo_operacion NOT IN (6,46,69,70)
                           GROUP BY codigo_caja_chica, anio_operacion, semana_operacion
                         ) y
                    LEFT JOIN ( SELECT codigo_caja_chica, anio_operacion, semana_operacion, sum(monto) AS monto_fiscal
                                FROM db_tesoreria.transaccion_caja_chica 
                                WHERE codigo_estado = 1 AND codigo_tipo_transaccion = 'F' 
                                  AND codigo_operacion NOT IN (6,46,69,70)
                                GROUP BY codigo_caja_chica, anio_operacion, semana_operacion
                              ) z
                    ON y.codigo_caja_chica = z.codigo_caja_chica AND y.anio_operacion = z.anio_operacion AND y.semana_operacion = z.semana_operacion
                    LEFT JOIN ( SELECT codigo_caja_chica, anio_operacion, semana_operacion,  sum(monto) AS monto_no_fiscal
                                FROM db_tesoreria.transaccion_caja_chica 
                                WHERE codigo_estado = 1 AND codigo_tipo_transaccion = 'NF' 
                                  AND codigo_operacion NOT IN (6,46,69,70)
                                GROUP BY codigo_caja_chica, anio_operacion, semana_operacion
                              ) w
                    ON y.codigo_caja_chica = w.codigo_caja_chica AND y.anio_operacion = w.anio_operacion AND y.semana_operacion = w.semana_operacion
                    INNER JOIN db_admon.caja_chica a
                    ON y.codigo_caja_chica = a.codigo_caja_chica
                    WHERE 1 = 1
                    " + filterCajasChicasAsignadas + @"

                    UNION

                    SELECT y.codigo_reporte,
	                       y.codigo_caja_chica, 
                           a.nombre_caja_chica, 
	                       y.anio_operacion, 
	                       y.semana_operacion,
                           y.monto_saldo_inicial, 
                           COALESCE(z.monto_fiscal,0) As monto_fiscal,
                           COALESCE(w.monto_no_fiscal,0) As monto_no_fiscal,
                           COALESCE(z.monto_fiscal,0) As monto_egreso,
	                       (y.monto_saldo_inicial - COALESCE(( SELECT sum(monto) AS monto  
                                                               FROM db_tesoreria.transaccion_caja_chica 
	                                                           WHERE codigo_estado <> 0 
                                                                 AND codigo_reporte = y.codigo_reporte
                                                                 AND codigo_tipo_transaccion = 'F'
                                                                 AND codigo_operacion NOT IN (6,46,69,70)
		                                                       GROUP BY codigo_reporte
	                                                         ),0)) AS monto_saldo,
	                       y.codigo_estado,
	                       x.nombre AS estado,
                           y.observaciones,
                           1 AS bloqueado,
                           1 AS permiso_anular,
                           0 AS permiso_editar

                    FROM db_tesoreria.reporte_caja_chica y
                    INNER JOIN db_tesoreria.estado_reporte_caja_chica x
                    ON y.codigo_estado = x.codigo_estado
                    INNER JOIN db_admon.caja_chica a
                    ON y.codigo_caja_chica = a.codigo_caja_chica
                    INNER JOIN db_admon.caja_chica m
                    ON y.codigo_caja_chica = m.codigo_caja_chica
                    LEFT JOIN ( SELECT codigo_reporte, codigo_caja_chica, anio_operacion, semana_operacion, sum(monto) AS monto_fiscal
                                FROM db_tesoreria.transaccion_caja_chica 
                                WHERE codigo_estado <> 0 AND codigo_tipo_transaccion = 'F' AND codigo_operacion NOT IN (6,46,69,70)
                                GROUP BY codigo_reporte, codigo_caja_chica, anio_operacion, semana_operacion

                              ) z
                    ON y.codigo_reporte = z.codigo_reporte AND y.codigo_caja_chica = z.codigo_caja_chica AND y.anio_operacion = z.anio_operacion AND y.semana_operacion = z.semana_operacion
                    LEFT JOIN ( SELECT codigo_reporte, codigo_caja_chica, anio_operacion, semana_operacion, sum(monto) AS monto_no_fiscal
                                FROM db_tesoreria.transaccion_caja_chica 
                                WHERE codigo_estado <> 0 AND codigo_tipo_transaccion = 'NF' AND codigo_operacion NOT IN (6,46,69,70)
                                GROUP BY codigo_reporte, codigo_caja_chica, anio_operacion, semana_operacion
                              ) w
                    ON y.codigo_caja_chica = w.codigo_caja_chica AND y.anio_operacion = w.anio_operacion AND y.semana_operacion = w.semana_operacion
                    WHERE y.codigo_estado = @EstadoReporteGenerado 
                    " + filterCajasChicasAsignadas;

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@EstadoReporteGenerado", Constantes.CajaChica.EstadoReporte.REPORTE_GENERADO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            ReporteCajaChicaCLS objReporte;
                            lista = new List<ReporteCajaChicaCLS>();
                            int postCodigoReporte = dr.GetOrdinal("codigo_reporte");
                            int postCodigoCajaChica = dr.GetOrdinal("codigo_caja_chica");
                            int postNombreCajaChica = dr.GetOrdinal("nombre_caja_chica");
                            int postAnioOperacion = dr.GetOrdinal("anio_operacion");
                            int postSemanaOperacion = dr.GetOrdinal("semana_operacion");
                            int postMontoFiscal = dr.GetOrdinal("monto_fiscal");
                            int postMontoNoFiscal = dr.GetOrdinal("monto_no_fiscal");
                            int postMontoSaldoInicial = dr.GetOrdinal("monto_saldo_inicial");
                            int postMontoEgreso = dr.GetOrdinal("monto_egreso");
                            int postMontoSaldo = dr.GetOrdinal("monto_saldo");
                            int postCodigoEstado = dr.GetOrdinal("codigo_estado");
                            int postEstado = dr.GetOrdinal("estado");
                            int postObservaciones = dr.GetOrdinal("observaciones");
                            int postBloqueado = dr.GetOrdinal("bloqueado");
                            int postPermisoAnular = dr.GetOrdinal("permiso_anular");
                            int postPermisoEditar = dr.GetOrdinal("permiso_editar");
                            while (dr.Read())
                            {
                                objReporte = new ReporteCajaChicaCLS();
                                objReporte.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objReporte.CodigoCajaChica = dr.GetInt16(postCodigoCajaChica);
                                objReporte.NombreCajaChica = dr.GetString(postNombreCajaChica);
                                objReporte.AnioOperacion = dr.GetInt16(postAnioOperacion);
                                objReporte.SemanaOperacion = dr.GetByte(postSemanaOperacion);
                                objReporte.MontoFiscal = dr.GetDecimal(postMontoFiscal);
                                objReporte.MontoNoFiscal = dr.GetDecimal(postMontoNoFiscal);
                                objReporte.MontoDisponible = dr.GetDecimal(postMontoSaldoInicial);
                                objReporte.MontoEgreso = dr.GetDecimal(postMontoEgreso);
                                objReporte.MontoSaldo = dr.GetDecimal(postMontoSaldo);
                                objReporte.CodigoEstado = (byte)dr.GetInt32(postCodigoEstado);
                                objReporte.Estado = dr.GetString(postEstado);
                                objReporte.Observaciones = dr.IsDBNull(postObservaciones) ? null : dr.GetString(postObservaciones);
                                objReporte.Bloqueado = (byte)dr.GetInt32(postBloqueado);
                                objReporte.PermisoAnular = (byte)dr.GetInt32(postPermisoAnular);
                                objReporte.PermisoEditar = (byte)dr.GetInt32(postPermisoEditar);
                                lista.Add(objReporte);
                            }
                            
                        }
                    }
                    conexion.Close();
                }
                catch (Exception e)
                {
                    conexion.Close();
                    lista = null;
                }

                return lista;
            }
        }

        public List<ReporteCajaChicaCLS> ListarReportesCajaChicaConsulta(int codigoCajaChica, int anioOperacion, string usuarioIng, int esSuperAdmin)
        {
            List<ReporteCajaChicaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string filterCajasChicasAsignadas = String.Empty;
                    string filterCajaChica = String.Empty;
                    string filterAnioOperacion = String.Empty;
                    if (codigoCajaChica != 0 && codigoCajaChica != -1)
                    {
                        filterCajaChica = " AND y.codigo_caja_chica = " + codigoCajaChica.ToString();
                    }
                    if (anioOperacion != 0) 
                    {
                        filterAnioOperacion = " AND y.anio_operacion = " + anioOperacion.ToString();
                    }
                    if (esSuperAdmin != 1)
                    {
                        filterCajasChicasAsignadas = " AND y.codigo_caja_chica IN (select codigo_caja_chica from db_admon.usuario_caja_chica WHERE id_usuario = '" + usuarioIng + "')";
                    }

                    string sql = @"
                    SELECT 0 AS codigo_reporte, 
	                       y.codigo_caja_chica,
                           a.nombre_caja_chica, 
	                       y.anio_operacion,
	                       y.semana_operacion,
	                       y.monto,
                           COALESCE(z.monto_fiscal,0) As monto_fiscal,
                           COALESCE(w.monto_no_fiscal,0) As monto_no_fiscal,
                           a.monto_limite - COALESCE(z.monto_fiscal,0) - COALESCE(w.monto_no_fiscal,0) AS monto_disponible,
	                       1 AS codigo_estado, 
	                       'GENERAR REPORTE' estado,
                           '' AS observaciones,
                           0 AS bloqueado, 
                           0 AS permiso_anular,
                           0 AS permiso_editar,
                           0 AS monto_reembolso_calculado,
                           0 AS monto_reembolso,
                           FORMAT(GETDATE(), 'dd/MM/yyyy, hh:mm:ss') AS fecha_corte_str

                    FROM ( SELECT codigo_caja_chica, anio_operacion, semana_operacion, sum(monto) AS monto 
                           FROM db_tesoreria.transaccion_caja_chica 
                           WHERE codigo_estado = @CodigoEstadoTransaccionRegistrado AND codigo_operacion NOT IN (6,46,69,70)
                           GROUP BY codigo_caja_chica, anio_operacion, semana_operacion
                         ) y
                    LEFT JOIN ( SELECT codigo_caja_chica, anio_operacion, semana_operacion, sum(monto) AS monto_fiscal
                                FROM db_tesoreria.transaccion_caja_chica 
                                WHERE codigo_estado = 1 AND codigo_tipo_transaccion = 'F' AND codigo_operacion NOT IN (6,46,69,70)
                                GROUP BY codigo_caja_chica, anio_operacion, semana_operacion
                              ) z
                    ON y.codigo_caja_chica = z.codigo_caja_chica AND y.anio_operacion = z.anio_operacion AND y.semana_operacion = z.semana_operacion
                    LEFT JOIN ( SELECT codigo_caja_chica, anio_operacion, semana_operacion, sum(monto) AS monto_no_fiscal
                                FROM db_tesoreria.transaccion_caja_chica 
                                WHERE codigo_estado = 1 AND codigo_tipo_transaccion = 'NF' AND codigo_operacion NOT IN (6,46,69,70)
                                GROUP BY codigo_caja_chica, anio_operacion, semana_operacion
                              ) w
                    ON y.codigo_caja_chica = w.codigo_caja_chica AND y.anio_operacion = w.anio_operacion AND y.semana_operacion = w.semana_operacion
                    INNER JOIN db_admon.caja_chica a
                    ON y.codigo_caja_chica = a.codigo_caja_chica
                    WHERE 1 = 1
                    " + filterCajaChica + @"
                    " + filterAnioOperacion + @"
                    " + filterCajasChicasAsignadas + @"

                    UNION

                    SELECT y.codigo_reporte,
	                       y.codigo_caja_chica, 
                           a.nombre_caja_chica, 
	                       y.anio_operacion, 
	                       y.semana_operacion,
	                       COALESCE(( SELECT sum(monto) AS monto  
                                      FROM db_tesoreria.transaccion_caja_chica 
	                                  WHERE codigo_estado <> 0 
                                        AND codigo_reporte = y.codigo_reporte
                                        AND codigo_operacion NOT IN (6,46,69,70)    
		                              GROUP BY codigo_reporte
	                                ),0) AS monto,
                           COALESCE(z.monto_fiscal,0) As monto_fiscal,
                           COALESCE(w.monto_no_fiscal,0) As monto_no_fiscal,
                           a.monto_limite - COALESCE(z.monto_fiscal,0) - COALESCE(w.monto_no_fiscal,0) AS monto_disponible,
	                       y.codigo_estado,
	                       x.nombre AS estado,
                           y.observaciones,
                           1 AS bloqueado,
                           1 AS permiso_anular,
                           0 AS permiso_editar,
                           y.monto_reembolso_calculado,
                           y.monto_reembolso,
                           FORMAT(y.fecha_generacion, 'dd/MM/yyyy, hh:mm:ss') AS fecha_corte_str

                    FROM db_tesoreria.reporte_caja_chica y
                    INNER JOIN db_tesoreria.estado_reporte_caja_chica x
                    ON y.codigo_estado = x.codigo_estado
                    INNER JOIN db_admon.caja_chica a
                    ON y.codigo_caja_chica = a.codigo_caja_chica
                    LEFT JOIN ( SELECT codigo_reporte, codigo_caja_chica, anio_operacion, semana_operacion, sum(monto) AS monto_fiscal
                                FROM db_tesoreria.transaccion_caja_chica 
                                WHERE codigo_estado <> 0 AND codigo_tipo_transaccion = 'F' AND codigo_operacion NOT IN (6,46,69,70)
                                GROUP BY codigo_reporte, codigo_caja_chica, anio_operacion, semana_operacion
                              ) z
                    ON y.codigo_reporte = z.codigo_reporte AND y.codigo_caja_chica = z.codigo_caja_chica AND y.anio_operacion = z.anio_operacion AND y.semana_operacion = z.semana_operacion
                    LEFT JOIN ( SELECT codigo_reporte, codigo_caja_chica, anio_operacion, semana_operacion, sum(monto) AS monto_no_fiscal
                                FROM db_tesoreria.transaccion_caja_chica 
                                WHERE codigo_estado <> 0 AND codigo_tipo_transaccion = 'NF' AND codigo_operacion NOT IN (6,46,69,70)
                                GROUP BY codigo_reporte, codigo_caja_chica, anio_operacion, semana_operacion
                              ) w
                    ON y.codigo_caja_chica = w.codigo_caja_chica AND y.anio_operacion = w.anio_operacion AND y.semana_operacion = w.semana_operacion
                    WHERE y.codigo_estado <> @CodigoEstadoReporteAnulado
                    " + filterCajaChica + @"
                    " + filterAnioOperacion + @"
                    " + filterCajasChicasAsignadas;

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoTransaccionRegistrado", Constantes.CajaChica.EstadoTransaccion.REGISTRADO);
                        cmd.Parameters.AddWithValue("@CodigoEstadoReporteAnulado", Constantes.CajaChica.EstadoReporte.ANULADO);
                        cmd.Parameters.AddWithValue("@CodigoEstadoReporteRevisado", Constantes.CajaChica.EstadoReporte.REVISADO);
                        
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            ReporteCajaChicaCLS objReporte;
                            lista = new List<ReporteCajaChicaCLS>();
                            int postCodigoReporte = dr.GetOrdinal("codigo_reporte");
                            int postCodigoCajaChica = dr.GetOrdinal("codigo_caja_chica");
                            int postNombreCajaChica = dr.GetOrdinal("nombre_caja_chica");
                            int postAnioOperacion = dr.GetOrdinal("anio_operacion");
                            int postSemanaOperacion = dr.GetOrdinal("semana_operacion");
                            int postMonto = dr.GetOrdinal("monto");
                            int postMontoFiscal = dr.GetOrdinal("monto_fiscal");
                            int postMontoNoFiscal = dr.GetOrdinal("monto_no_fiscal");
                            int postMontoDisponible = dr.GetOrdinal("monto_disponible");
                            int postCodigoEstado = dr.GetOrdinal("codigo_estado");
                            int postEstado = dr.GetOrdinal("estado");
                            int postObservaciones = dr.GetOrdinal("observaciones");
                            int postBloqueado = dr.GetOrdinal("bloqueado");
                            int postPermisoAnular = dr.GetOrdinal("permiso_anular");
                            int postPermisoEditar = dr.GetOrdinal("permiso_editar");
                            int postMontoReembolsoCalculado = dr.GetOrdinal("monto_reembolso_calculado");
                            int postMontoReembolso = dr.GetOrdinal("monto_reembolso");
                            int postFechaCorteStr = dr.GetOrdinal("fecha_corte_str");
                            while (dr.Read())
                            {
                                objReporte = new ReporteCajaChicaCLS();
                                objReporte.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objReporte.CodigoCajaChica = dr.GetInt16(postCodigoCajaChica);
                                objReporte.NombreCajaChica = dr.GetString(postNombreCajaChica);
                                objReporte.AnioOperacion = dr.GetInt16(postAnioOperacion);
                                objReporte.SemanaOperacion = dr.GetByte(postSemanaOperacion);
                                objReporte.Monto = dr.GetDecimal(postMonto);
                                objReporte.MontoFiscal = dr.GetDecimal(postMontoFiscal);
                                objReporte.MontoNoFiscal = dr.GetDecimal(postMontoNoFiscal);
                                objReporte.MontoDisponible = dr.GetDecimal(postMontoDisponible);
                                objReporte.CodigoEstado = (byte)dr.GetInt32(postCodigoEstado);
                                objReporte.Estado = dr.GetString(postEstado);
                                objReporte.Observaciones = dr.IsDBNull(postObservaciones) ? null : dr.GetString(postObservaciones);
                                objReporte.Bloqueado = (byte)dr.GetInt32(postBloqueado);
                                objReporte.PermisoAnular = (byte)dr.GetInt32(postPermisoAnular);
                                objReporte.PermisoEditar = (byte)dr.GetInt32(postPermisoEditar);
                                objReporte.MontoReembolsoCalculado = dr.GetDecimal(postMontoReembolsoCalculado);
                                objReporte.MontoReembolso = dr.GetDecimal(postMontoReembolso);
                                objReporte.FechaCorteStr = dr.GetString(postFechaCorteStr);

                                lista.Add(objReporte);
                            }
                            
                        }
                    }
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

        public List<ReporteCajaChicaCLS> ListarReportesCajaChicaParaRevision(string usuarioIng, int esSuperAdmin)
        {
            List<ReporteCajaChicaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string filterCajasChicasAsignadas = String.Empty;
                    if (esSuperAdmin != 1) {
                        filterCajasChicasAsignadas = " AND x.codigo_caja_chica IN (select codigo_caja_chica from db_admon.usuario_caja_chica WHERE id_usuario = '" + usuarioIng + "')";
                    }

                    string sql = @"
                    SELECT x.codigo_reporte,
	                       x.codigo_caja_chica, 
                           a.nombre_caja_chica, 
	                       x.anio_operacion, 
	                       x.semana_operacion,
	                       COALESCE(( SELECT sum(monto) AS monto  FROM db_tesoreria.transaccion_caja_chica 
	                         WHERE codigo_estado <> 0 AND codigo_reporte = x.codigo_reporte
		                     GROUP BY codigo_reporte
	                       ),0) AS monto,
                           COALESCE(z.monto_fiscal,0) As monto_fiscal,
                           COALESCE(w.monto_no_fiscal,0) As monto_no_fiscal,
	                       x.codigo_estado,
	                       y.nombre AS estado,
                           x.observaciones,
                           0 AS bloqueado,
                           1 AS permiso_corregir,
                           FORMAT(x.fecha_ing, 'dd/MM/yyyy, hh:mm:ss') AS fecha_corte_str,
                           ( SELECT CONCAT(FORMAT(MIN(fecha_operacion), 'dd/MM/yyyy, hh:mm:ss'),' a ',FORMAT(MAX(fecha_operacion), 'dd/MM/yyyy, hh:mm:ss'))
                             FROM db_tesoreria.transaccion_caja_chica 
                             WHERE codigo_estado <> 0
                             AND codigo_reporte = x.codigo_reporte) AS periodo

                    FROM db_tesoreria.reporte_caja_chica x
                    INNER JOIN db_tesoreria.estado_reporte_caja_chica y
                    ON x.codigo_estado = y.codigo_estado
                    INNER JOIN db_admon.caja_chica a
                    ON x.codigo_caja_chica = a.codigo_caja_chica
                    LEFT JOIN ( SELECT codigo_reporte, codigo_caja_chica, anio_operacion, semana_operacion, sum(monto) AS monto_fiscal
                                FROM db_tesoreria.transaccion_caja_chica 
                                WHERE codigo_estado <> 0 AND codigo_tipo_transaccion = 'F' AND codigo_operacion NOT IN (6,46,69,70)
                                GROUP BY codigo_reporte, codigo_caja_chica, anio_operacion, semana_operacion
                              ) z
                    ON x.codigo_reporte = z.codigo_reporte AND x.codigo_caja_chica = z.codigo_caja_chica AND x.anio_operacion = z.anio_operacion AND x.semana_operacion = z.semana_operacion
                    LEFT JOIN ( SELECT codigo_reporte, codigo_caja_chica, anio_operacion, semana_operacion, sum(monto) AS monto_no_fiscal
                                FROM db_tesoreria.transaccion_caja_chica 
                                WHERE codigo_estado <> 0 AND codigo_tipo_transaccion = 'NF' AND codigo_operacion NOT IN (6,46,69,70)
                                GROUP BY codigo_reporte, codigo_caja_chica, anio_operacion, semana_operacion
                              ) w
                    ON x.codigo_caja_chica = w.codigo_caja_chica AND x.anio_operacion = w.anio_operacion AND x.semana_operacion = w.semana_operacion
                    WHERE x.codigo_estado = @EstadoReporteEnRevision
                    " + filterCajasChicasAsignadas;

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@EstadoReporteEnRevision", Constantes.CajaChica.EstadoReporte.EN_REVISION);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            ReporteCajaChicaCLS objReporte;
                            lista = new List<ReporteCajaChicaCLS>();
                            int postCodigoReporte = dr.GetOrdinal("codigo_reporte");
                            int postCodigoCajaChica = dr.GetOrdinal("codigo_caja_chica");
                            int postNombreCajaChica = dr.GetOrdinal("nombre_caja_chica");
                            int postAnioOperacion = dr.GetOrdinal("anio_operacion");
                            int postSemanaOperacion = dr.GetOrdinal("semana_operacion");
                            int postMonto = dr.GetOrdinal("monto");
                            int postMontoFiscal = dr.GetOrdinal("monto_fiscal");
                            int postMontoNoFiscal = dr.GetOrdinal("monto_no_fiscal");
                            int postCodigoEstado = dr.GetOrdinal("codigo_estado");
                            int postEstado = dr.GetOrdinal("estado");
                            int postObservaciones = dr.GetOrdinal("observaciones");
                            int postBloqueado = dr.GetOrdinal("bloqueado");
                            int postPermisoCorregir = dr.GetOrdinal("permiso_corregir");
                            int postFechaCorteStr = dr.GetOrdinal("fecha_corte_str");
                            int postPeriodo = dr.GetOrdinal("periodo");
                            while (dr.Read())
                            {
                                objReporte = new ReporteCajaChicaCLS();
                                objReporte.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objReporte.CodigoCajaChica = dr.GetInt16(postCodigoCajaChica);
                                objReporte.NombreCajaChica = dr.GetString(postNombreCajaChica);
                                objReporte.AnioOperacion = dr.GetInt16(postAnioOperacion);
                                objReporte.SemanaOperacion = dr.GetByte(postSemanaOperacion);
                                objReporte.Monto = dr.GetDecimal(postMonto);
                                objReporte.MontoFiscal = dr.GetDecimal(postMontoFiscal);
                                objReporte.MontoNoFiscal = dr.GetDecimal(postMontoNoFiscal);
                                objReporte.CodigoEstado = (byte)dr.GetByte(postCodigoEstado);
                                objReporte.Estado = dr.GetString(postEstado);
                                objReporte.Observaciones = dr.IsDBNull(postObservaciones) ? null : dr.GetString(postObservaciones);
                                objReporte.Bloqueado = (byte)dr.GetInt32(postBloqueado);
                                objReporte.PermisoCorregir = (byte)dr.GetInt32(postPermisoCorregir);
                                objReporte.Periodo = dr.GetString(postPeriodo);
                                objReporte.FechaCorteStr = dr.GetString(postFechaCorteStr);
                                lista.Add(objReporte);
                            }
                         
                        }
                    }
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

        public string GenerarReporteSemanal(int codigoCajaChica, int anioOperacion, int semanaOperacion, string observaciones, string idUsuario)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_tesoreria.uspGenerarReporteCajaChica", conexion))
                    {
                        // le indico que es un procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CodigoCajaChica", codigoCajaChica);
                        cmd.Parameters.AddWithValue("@AnioOperacion", anioOperacion);
                        cmd.Parameters.AddWithValue("@SemanaOperacion", semanaOperacion);
                        cmd.Parameters.AddWithValue("@Observaciones", observaciones);
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
                        resultado = outParameter.Value.ToString();
                        

                    }// fin using
                    conexion.Close();
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    resultado = "Error [0]: " + ex.Message;
                }
                return resultado;
            }
        }

        public string ActualizarEstadoReporte(int codigoReporte, int codigoEstadoReporte, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    UPDATE db_tesoreria.reporte_caja_chica 
                    SET codigo_estado = @CodigoEstado,
                        usuario_act = @UsuarioActualizacion,
                        fecha_act = @FechaActualizacion
                    WHERE codigo_reporte = @CodigoReporte";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstado", codigoEstadoReporte);
                        cmd.Parameters.AddWithValue("@UsuarioActualizacion", usuarioAct);
                        cmd.Parameters.AddWithValue("@FechaActualizacion", DateTime.Now);
                        cmd.Parameters.AddWithValue("@CodigoReporte", codigoReporte);

                        // rows sotred the number of rows affected
                        int rows = cmd.ExecuteNonQuery();
                        conexion.Close();
                    }
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

        public string FinalizarRevision(int codigoReporte, decimal montoReintegroCalculado, decimal montoReintegro, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_tesoreria.uspVistoBuenoReporteCajaChica", conexion))
                    {
                        // le indico que es un procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CodigoReporte", codigoReporte);
                        cmd.Parameters.AddWithValue("@MontoReintegroCalculado", montoReintegroCalculado);
                        cmd.Parameters.AddWithValue("@MontoReintegro", montoReintegro);
                        cmd.Parameters.AddWithValue("@UsuarioIng", usuarioAct);

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
                        resultado = outParameter.Value.ToString();
                    }
                    conexion.Close();
                }
                catch (Exception ex)
                {
                    resultado = "Error [0]: " + ex.Message;
                    conexion.Close();
                }

                return resultado;
            }
        }

        public string AnularReporteGenerado(int codigoReporte, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
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
                    string sqlUpdateReporte = @"
                    UPDATE db_tesoreria.reporte_caja_chica 
                    SET codigo_estado = @CodigoEstado,
                        usuario_act = @UsuarioActualizacion,
                        fecha_act = @FechaActualizacion
                    WHERE codigo_reporte = @CodigoReporte";

                    cmd.CommandText = sqlUpdateReporte;
                    cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.CajaChica.EstadoReporte.ANULADO);
                    cmd.Parameters.AddWithValue("@UsuarioActualizacion", usuarioAct);
                    cmd.Parameters.AddWithValue("@FechaActualizacion", DateTime.Now);
                    cmd.Parameters.AddWithValue("@CodigoReporte", codigoReporte);
                    // rows sotred the number of rows affected
                    int rowsReportes = cmd.ExecuteNonQuery();

                    string sqlUpdateDetalleReporte = @"
                    UPDATE db_tesoreria.transaccion_caja_chica 
                    SET codigo_estado = @EstadoTransaccion, 
                        codigo_reporte = null 
                    WHERE codigo_reporte = @CodigoReporte";
                    cmd.CommandText = sqlUpdateDetalleReporte;
                    cmd.Parameters.AddWithValue("@EstadoTransaccion", Constantes.CajaChica.EstadoTransaccion.REGISTRADO);
                    cmd.Parameters["@CodigoReporte"].Value = codigoReporte;
                    int rowsTransaccion =  cmd.ExecuteNonQuery();

                    if (rowsReportes > 0 && rowsTransaccion > 0)
                    {
                        // Attempt to commit the transaction.
                        transaction.Commit();
                        conexion.Close();
                        resultado = "OK";
                    }
                    else {
                        transaction.Rollback();
                        conexion.Close();
                        resultado = "ERROR";
                    }
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

        public string TrasladarReporteParaRevision(int codigoReporte, int codigoCajaChica, decimal montoGastosFiscales, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
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

                    // Actualiza el saldo de la caja chica
                    string sentenciaUpdateSaldo = @"
                    UPDATE db_admon.caja_chica 
                    SET monto_disponible = monto_disponible - @MontoGastos,
                        usuario_act = @UsuarioAct,
                        fecha_act = @FechaAct
                    WHERE codigo_caja_chica = @CodigoCajaChica";

                    cmd.CommandText = sentenciaUpdateSaldo;
                    cmd.Parameters.AddWithValue("@MontoGastos", montoGastosFiscales);
                    cmd.Parameters.AddWithValue("@CodigoCajaChica", codigoCajaChica);
                    cmd.Parameters.AddWithValue("@UsuarioAct", usuarioAct);
                    cmd.Parameters.AddWithValue("@FechaAct", DateTime.Now);
                    int rowsSaldo = cmd.ExecuteNonQuery();

                    // Cambia el estado de transacciones de caja chica del reporte asignado
                    string sentenciaUpdateTransaccion = @"
                    UPDATE db_tesoreria.transaccion_caja_chica 
                    SET codigo_estado = @CodigoEstadoDesembolsado
                    WHERE codigo_reporte = @CodigoReporte";

                    cmd.CommandText = sentenciaUpdateTransaccion;
                    cmd.Parameters.AddWithValue("@CodigoEstadoDesembolsado", Constantes.CajaChica.EstadoTransaccion.DESEMBOLSADO);
                    cmd.Parameters.AddWithValue("@CodigoReporte", codigoReporte);
                    int rowsTransaccion = cmd.ExecuteNonQuery();

                    // Actualiza el estado del reporte de caja chica
                    string sentenciaUpdateReporte = @"
                    UPDATE db_tesoreria.reporte_caja_chica 
                    SET codigo_estado = @CodigoEstadoReporte,
                        usuario_act = @UsuarioAct,
                        fecha_act = @FechaAct
                    WHERE codigo_reporte = @CodigoReporte";

                    cmd.CommandText = sentenciaUpdateReporte;
                    cmd.Parameters.AddWithValue("@CodigoEstadoReporte", Constantes.CajaChica.EstadoReporte.EN_REVISION);
                    cmd.Parameters["@CodigoReporte"].Value = codigoReporte;
                    cmd.Parameters["@UsuarioAct"].Value = usuarioAct;
                    cmd.Parameters["@FechaAct"].Value = DateTime.Now;
                    int rowsReporte = cmd.ExecuteNonQuery();

                    if (rowsSaldo > 0 && rowsTransaccion > 0 && rowsReporte > 0)
                    {
                        // Attempt to commit the transaction.
                        transaction.Commit();
                        conexion.Close();
                        resultado = "OK";
                    }
                    else
                    {
                        transaction.Rollback();
                        conexion.Close();
                        resultado = "Error [0]: Transacción incompleta";
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    conexion.Close();
                    resultado = "Error [0]: " + ex.Message;
                }
            }

            return resultado;
        }

        public ReporteOperacionesCajaListCLS GetReporteReintegroCajaChica(int codigoReporte, int anioOperacion, int semanaOperacion, int codigoCajaChica)
        {
            int postMontoAsignado = 0;
            int postMontoReembolso = 0;
            int postFechaGeneracionStr = 0;

            int postCodigoTipoOperacion = 0;
            int postTipoOperacion = 0;
            int postIdTipoOperacion = 0;
            int postSigno = 0;
            int postCodigoCategoriaOperacion = 0;
            int postCategoriaOperacion = 0;
            int postCodigoOperacion = 0;
            int postOperacion = 0;
            int postCodigoCategoriaEntidad = 0;
            int postCategoriaEntidad = 0;
            int postCodigoEntidad = 0;
            int postNombreEntidad = 0;
            int postNombreEntidadCompleto = 0;
            int postDescripcion = 0;

            int postMontoLunes = 0;
            int postMontoMartes = 0;
            int postMontoMiercoles = 0;
            int postMontoJueves = 0;
            int postMontoViernes = 0;
            int postMontoSabado = 0;
            int postMontoDomingo = 0;

            int postMontoTotalLunes = 0;
            int postMontoTotalMartes = 0;
            int postMontoTotalMiercoles = 0;
            int postMontoTotalJueves = 0;
            int postMontoTotalViernes = 0;
            int postMontoTotalSabado = 0;
            int postMontoTotalDomingo = 0;
            int postMontoTotalSemana = 0;

            ReporteOperacionesCajaListCLS objReporte = new ReporteOperacionesCajaListCLS();
            using (SqlConnection conexion = new SqlConnection(cadenaContabilidad))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_contabilidad.uspGetReporteCajaChica", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CodigoReporte", codigoReporte);
                        cmd.Parameters.AddWithValue("@AnioReporte", anioOperacion);
                        cmd.Parameters.AddWithValue("@NumeroSemanaReporte", semanaOperacion);
                        cmd.Parameters.AddWithValue("@CodigoCajaChica", codigoCajaChica);
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr != null)
                        {// Monto Asignado a Caja Chica
                            postMontoAsignado = dr.GetOrdinal("monto_disponible");
                            postMontoReembolso = dr.GetOrdinal("monto_reembolso");
                            postFechaGeneracionStr = dr.GetOrdinal("fecha_generacion_str");
                            while (dr.Read())
                            {
                                objReporte.MontoAsignado = dr.GetDecimal(postMontoAsignado);
                                objReporte.MontoReembolso = dr.GetDecimal(postMontoReembolso);
                                objReporte.FechaGeneracionStr = dr.GetString(postFechaGeneracionStr);
                            }
                        }

                        if (dr.NextResult())
                        {// Encabezado de la fecha de inicio y fin del reporte
                            int postFechaInicioStr = dr.GetOrdinal("fecha_operacion_minima");
                            int postFechaFinStr = dr.GetOrdinal("fecha_operacion_maxima");
                            List<ProgramacionSemanalCLS> listaEncabezado = new List<ProgramacionSemanalCLS>();
                            ProgramacionSemanalCLS objEncabezado;
                            while (dr.Read())
                            {
                                objEncabezado = new ProgramacionSemanalCLS();
                                objEncabezado.FechaInicioSemana = dr.GetString(postFechaInicioStr);
                                objEncabezado.FechaFinSemana = dr.GetString(postFechaFinStr);
                                listaEncabezado.Add(objEncabezado);
                            }
                            objReporte.listaEncabezado = listaEncabezado;
                        }

                        if (dr.NextResult())
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

                        if (dr.NextResult())
                        {// Operacion: Todas las operaciones
                            postCodigoTipoOperacion = dr.GetOrdinal("codigo_tipo_operacion");
                            postTipoOperacion = dr.GetOrdinal("tipo_operacion");
                            postIdTipoOperacion = dr.GetOrdinal("id_tipo_operacion");
                            postSigno = dr.GetOrdinal("signo");
                            postCodigoCategoriaOperacion = dr.GetOrdinal("codigo_categoria_operacion");
                            postCategoriaOperacion = dr.GetOrdinal("categoria_operacion");
                            postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            postOperacion = dr.GetOrdinal("operacion");
                            postCodigoCategoriaEntidad = dr.GetOrdinal("codigo_categoria_entidad");
                            postCategoriaEntidad = dr.GetOrdinal("categoria_entidad");
                            postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            postNombreEntidad = dr.GetOrdinal("nombre_entidad");
                            postNombreEntidadCompleto = dr.GetOrdinal("nombre_entidad_completo");
                            postDescripcion = dr.GetOrdinal("descripcion");
                            postMontoLunes = dr.GetOrdinal("monto_lunes");
                            postMontoMartes = dr.GetOrdinal("monto_martes");
                            postMontoMiercoles = dr.GetOrdinal("monto_miercoles");
                            postMontoJueves = dr.GetOrdinal("monto_jueves");
                            postMontoViernes = dr.GetOrdinal("monto_viernes");
                            postMontoSabado = dr.GetOrdinal("monto_sabado");
                            postMontoDomingo = dr.GetOrdinal("monto_domingo");
                            //postMontoSemana = dr.GetOrdinal("monto_semana");
                            List<ReporteOperacionesCajaCLS> listaTransacciones = new List<ReporteOperacionesCajaCLS>();
                            ReporteOperacionesCajaCLS objReporteDetalle;
                            while (dr.Read())
                            {
                                objReporteDetalle = new ReporteOperacionesCajaCLS();
                                objReporteDetalle.CodigoTipoOperacion = dr.GetInt16(postCodigoTipoOperacion);
                                objReporteDetalle.TipoOperacion = dr.GetString(postTipoOperacion);
                                objReporteDetalle.IdTipoOperacion = dr.GetString(postIdTipoOperacion);
                                objReporteDetalle.Signo = dr.GetInt16(postSigno);
                                objReporteDetalle.CodigoCategoriaOperacion = dr.GetInt16(postCodigoCategoriaOperacion);
                                objReporteDetalle.CategoriaOperacion = dr.GetString(postCategoriaOperacion);
                                objReporteDetalle.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objReporteDetalle.Operacion = dr.GetString(postOperacion);
                                objReporteDetalle.CodigoCategoriaEntidad = (short)dr.GetInt32(postCodigoCategoriaEntidad);
                                objReporteDetalle.CategoriaEntidad = dr.GetString(postCategoriaEntidad);
                                objReporteDetalle.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objReporteDetalle.NombreEntidad = dr.GetString(postNombreEntidad);
                                objReporteDetalle.NombreEntidadCompleto = dr.GetString(postNombreEntidadCompleto);
                                objReporteDetalle.Descripcion = dr.IsDBNull(postDescripcion) ? "" : dr.GetString(postDescripcion);
                                objReporteDetalle.MontoLunes = dr.GetDecimal(postMontoLunes);
                                objReporteDetalle.MontoMartes = dr.GetDecimal(postMontoMartes);
                                objReporteDetalle.MontoMiercoles = dr.GetDecimal(postMontoMiercoles);
                                objReporteDetalle.MontoJueves = dr.GetDecimal(postMontoJueves);
                                objReporteDetalle.MontoViernes = dr.GetDecimal(postMontoViernes);
                                objReporteDetalle.MontoSabado = dr.GetDecimal(postMontoSabado);
                                objReporteDetalle.MontoDomingo = dr.GetDecimal(postMontoDomingo);
                                //objReporteDetalle.MontoSemana = dr.GetDecimal(postMontoSemana);
                                listaTransacciones.Add(objReporteDetalle);
                            }// fin while
                            objReporte.listaTransaccciones = listaTransacciones;
                        }// fin if


                        if (dr.NextResult())
                        {// Operacion: Montos total del los tipos de operacion, del ID del tipo de operacion
                            postIdTipoOperacion = dr.GetOrdinal("id");
                            postMontoTotalLunes = dr.GetOrdinal("monto_total_lunes");
                            postMontoTotalMartes = dr.GetOrdinal("monto_total_martes");
                            postMontoTotalMiercoles = dr.GetOrdinal("monto_total_miercoles");
                            postMontoTotalJueves = dr.GetOrdinal("monto_total_jueves");
                            postMontoTotalViernes = dr.GetOrdinal("monto_total_viernes");
                            postMontoTotalSabado = dr.GetOrdinal("monto_total_sabado");
                            postMontoTotalDomingo = dr.GetOrdinal("monto_total_domingo");
                            postMontoTotalSemana = dr.GetOrdinal("monto_total_semana");
                            List<TipoOperacionCLS> listaMontosTipoOperacion = new List<TipoOperacionCLS>();
                            TipoOperacionCLS objMontoTipoOperacion;
                            while (dr.Read())
                            {
                                objMontoTipoOperacion = new TipoOperacionCLS();
                                objMontoTipoOperacion.IdTipoOperacion = dr.GetString(postIdTipoOperacion);
                                objMontoTipoOperacion.MontoTotalLunes = dr.GetDecimal(postMontoTotalLunes);
                                objMontoTipoOperacion.MontoTotalMartes = dr.GetDecimal(postMontoTotalMartes);
                                objMontoTipoOperacion.MontoTotalMiercoles = dr.GetDecimal(postMontoTotalMiercoles);
                                objMontoTipoOperacion.MontoTotalJueves = dr.GetDecimal(postMontoTotalJueves);
                                objMontoTipoOperacion.MontoTotalViernes = dr.GetDecimal(postMontoTotalViernes);
                                objMontoTipoOperacion.MontoTotalSabado = dr.GetDecimal(postMontoTotalSabado);
                                objMontoTipoOperacion.MontoTotalDomingo = dr.GetDecimal(postMontoTotalDomingo);
                                objMontoTipoOperacion.MontoTotalSemana = dr.GetDecimal(postMontoTotalSemana);

                                listaMontosTipoOperacion.Add(objMontoTipoOperacion);
                            }// fin while
                            objReporte.listaMontosTiposDeOperacion = listaMontosTipoOperacion;
                        }// fin if
                    }
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

        public ReporteCorteCajaChicaCLS GetTransaccionesReporteChica(int codigoReporte, int anioOperacion, int semanaOperacion)
        {
            ReporteCorteCajaChicaCLS objReporte = new ReporteCorteCajaChicaCLS();
            using (SqlConnection conexion = new SqlConnection(cadenaContabilidad))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_contabilidad.uspGetReporteCorteCajaChica", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CodigoReporte", codigoReporte);
                        cmd.Parameters.AddWithValue("@AnioReporte", anioOperacion);
                        cmd.Parameters.AddWithValue("@SemanaReporte", semanaOperacion);
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr != null)
                        {// Periodo de las transacciones
                            int postFechaInicioStr = dr.GetOrdinal("fecha_operacion_minima");
                            int postFechaFinStr = dr.GetOrdinal("fecha_operacion_maxima");
                            List<ProgramacionSemanalCLS> periodo = new List<ProgramacionSemanalCLS>();
                            ProgramacionSemanalCLS objPeriodo;
                            while (dr.Read())
                            {
                                objPeriodo = new ProgramacionSemanalCLS();
                                objPeriodo.FechaInicioSemana = dr.GetString(postFechaInicioStr);
                                objPeriodo.FechaFinSemana = dr.GetString(postFechaFinStr);
                                periodo.Add(objPeriodo);
                            }
                            objReporte.periodoOperacion = periodo;
                        }

                        if (dr.NextResult())
                        {// Encabezado del reporte
                            int postFechaGeneracionStr = dr.GetOrdinal("fecha_generacion_str");
                            int postNombreCajaChica = dr.GetOrdinal("nombre_caja_chica");
                            ReporteCajaChicaCLS encabezado = new ReporteCajaChicaCLS();
                            while (dr.Read())
                            {
                                encabezado = new ReporteCajaChicaCLS();
                                encabezado.FechaCorteStr = dr.GetString(postFechaGeneracionStr);
                                encabezado.NombreCajaChica = dr.GetString(postNombreCajaChica);
                                objReporte.encabezado = encabezado;
                            }
                        }

                        if (dr.NextResult())
                        {// Detalle del reporte
                            
                            int postCodigoTransaccion = dr.GetOrdinal("codigo_transaccion");
                            int postCodigoCajaChica = dr.GetOrdinal("codigo_caja_chica");
                            int postNombreCajaChica = dr.GetOrdinal("nombre_caja_chica");
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postNombreOperacion = dr.GetOrdinal("nombre_operacion");
                            int postCodigoTipoDocumento = dr.GetOrdinal("codigo_tipo_documento");
                            int postTipoDocumento = dr.GetOrdinal("tipo_documento");
                            int postNitProveedor = dr.GetOrdinal("nit_proveedor");
                            int postNombreProveedor = dr.GetOrdinal("nombre_proveedor");
                            int postSerieFactura = dr.GetOrdinal("serie_factura");
                            int postNumeroDocumento = dr.GetOrdinal("numero_documento");
                            int postFechaDocumento = dr.GetOrdinal("fecha_documento");
                            int postFechaDocumentoStr = dr.GetOrdinal("fecha_documento_str");
                            int postMonto = dr.GetOrdinal("monto");
                            int postDescripcion = dr.GetOrdinal("descripcion");

                            List<CajaChicaCLS> listaTransacciones = new List<CajaChicaCLS>();
                            CajaChicaCLS objTransaccion;
                            while (dr.Read())
                            {
                                objTransaccion = new CajaChicaCLS();
                                objTransaccion.CodigoTransaccion = dr.GetInt64(postCodigoTransaccion);
                                objTransaccion.CodigoCajaChica = dr.GetInt16(postCodigoCajaChica);
                                objTransaccion.NombreCajaChica = dr.GetString(postNombreCajaChica);
                                objTransaccion.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objTransaccion.Operacion = dr.GetString(postNombreOperacion);
                                objTransaccion.CodigoTipoDocumento = dr.GetByte(postCodigoTipoDocumento);
                                objTransaccion.TipoDocumento = dr.GetString(postTipoDocumento);
                                objTransaccion.NitProveedor = dr.GetString(postNitProveedor);
                                objTransaccion.NombreProveedor = dr.GetString(postNombreProveedor);
                                objTransaccion.SerieFactura = dr.GetString(postSerieFactura);
                                objTransaccion.NumeroDocumento = dr.GetInt64(postNumeroDocumento);
                                objTransaccion.FechaDocumento = dr.GetDateTime(postFechaDocumento);
                                objTransaccion.FechaDocumentoStr = dr.GetString(postFechaDocumentoStr);
                                objTransaccion.Monto = dr.GetDecimal(postMonto);
                                objTransaccion.Descripcion = dr.IsDBNull(postDescripcion) ? "" : dr.GetString(postDescripcion);
                                listaTransacciones.Add(objTransaccion);
                            }// fin while
                            objReporte.listaTransacciones = listaTransacciones;
                        }// fin if

                    }
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


    }
}
