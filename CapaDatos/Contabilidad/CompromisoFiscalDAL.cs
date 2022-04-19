using CapaEntidad.Contabilidad;
using CapaEntidad.Tesoreria;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Contabilidad
{
    public class CompromisoFiscalDAL: CadenaConexion
    {
        public List<CompromisoFiscalCLS> GetCompromisosFiscales(int codigoEmpresa, int anioOperacion, int semanaOperacion)
        {
            List<CompromisoFiscalCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string filterEmpresa = String.Empty;
                    string filterSemanaOperacion = String.Empty;
                    string filterReporte = String.Empty;
                    if (codigoEmpresa != -1) {
                        filterSemanaOperacion = " AND a.codigo_empresa = " + codigoEmpresa.ToString();
                    }

                    if (semanaOperacion != -1)
                    {
                        filterSemanaOperacion = " AND a.semana_operacion = " + semanaOperacion.ToString();
                    }

                    string sql = @" 
                    SELECT a.codigo_empresa,
                           x.nombre_comercial AS nombre_empresa,	
                           a.anio_operacion,
                           a.semana_operacion,
                           a.codigo_reporte,
                           a.monto_total,
                           COALESCE(b.monto_lunes,0) AS monto_lunes,
                           COALESCE(c.monto_martes,0) AS monto_martes,
                           COALESCE(d.monto_miercoles,0) AS monto_miercoles,
                           COALESCE(e.monto_jueves,0) AS monto_jueves,
                           COALESCE(f.monto_viernes,0) AS monto_viernes,
                           COALESCE(g.monto_sabado,0) AS monto_sabado,
                           COALESCE(h.monto_domingo,0) AS monto_domingo
                    FROM ( SELECT codigo_empresa,
                                  anio_operacion, 
                                  semana_operacion,
                                  codigo_reporte,
                                  sum(monto) AS monto_total
                           FROM db_tesoreria.transaccion 
                           WHERE codigo_operacion = 43 AND codigo_estado <> 0
                           GROUP BY codigo_empresa, anio_operacion, semana_operacion, codigo_reporte
                        ) a
                    LEFT JOIN ( SELECT codigo_empresa,
                                       anio_operacion, 
                                       semana_operacion, 
                                       codigo_reporte,
                                       sum(monto) AS monto_lunes
                                FROM db_tesoreria.transaccion 
                                WHERE codigo_operacion = 43 AND codigo_estado <> 0 AND dia_operacion = 1
                                GROUP BY codigo_empresa, anio_operacion, semana_operacion, codigo_reporte
                              ) b
                    ON a.codigo_empresa = b.codigo_empresa AND a.anio_operacion = b.anio_operacion AND a.semana_operacion = b.semana_operacion AND a.codigo_reporte = b.codigo_reporte
                    LEFT JOIN ( SELECT codigo_empresa,
                                       anio_operacion, 
                                       semana_operacion, 
                                       codigo_reporte,
                                       sum(monto) AS monto_martes
                                FROM db_tesoreria.transaccion 
                                WHERE codigo_operacion = 43 AND codigo_estado <> 0 AND dia_operacion = 2
                                GROUP BY codigo_empresa, anio_operacion, semana_operacion, codigo_reporte
                              ) c
                    ON a.codigo_empresa = c.codigo_empresa AND a.anio_operacion = c.anio_operacion AND a.semana_operacion = c.semana_operacion AND a.codigo_reporte = c.codigo_reporte
                    LEFT JOIN ( SELECT codigo_empresa,
                                       anio_operacion, 
                                       semana_operacion, 
                                       codigo_reporte,
                                       sum(monto) AS monto_miercoles
                                FROM db_tesoreria.transaccion 
                                WHERE codigo_operacion = 43 AND codigo_estado <> 0 AND dia_operacion = 3
                                GROUP BY codigo_empresa, anio_operacion, semana_operacion, codigo_reporte
                              ) d
                    ON a.codigo_empresa = d.codigo_empresa AND a.anio_operacion = d.anio_operacion AND a.semana_operacion = d.semana_operacion AND a.codigo_reporte = d.codigo_reporte
                    LEFT JOIN ( SELECT codigo_empresa,
                                       anio_operacion, 
                                       semana_operacion, 
                                       codigo_reporte,
                                       sum(monto) AS monto_jueves
                                FROM db_tesoreria.transaccion 
                                WHERE codigo_operacion = 43 AND codigo_estado <> 0 AND dia_operacion = 4
                                GROUP BY codigo_empresa, anio_operacion, semana_operacion, codigo_reporte
                              ) e
                    ON a.codigo_empresa = e.codigo_empresa AND a.anio_operacion = e.anio_operacion AND a.semana_operacion = e.semana_operacion AND a.codigo_reporte = e.codigo_reporte
                    LEFT JOIN ( SELECT codigo_empresa,
                                       anio_operacion, 
                                       semana_operacion, 
                                       codigo_reporte,
                                       sum(monto) AS monto_viernes
                                FROM db_tesoreria.transaccion 
                                WHERE codigo_operacion = 43 AND codigo_estado <> 0 AND dia_operacion = 5
                                GROUP BY codigo_empresa, anio_operacion, semana_operacion, codigo_reporte
                              ) f
                    ON a.codigo_empresa = f.codigo_empresa AND a.anio_operacion = f.anio_operacion AND a.semana_operacion = f.semana_operacion AND a.codigo_reporte = f.codigo_reporte
                    LEFT JOIN ( SELECT codigo_empresa,
                                       anio_operacion, 
                                       semana_operacion, 
                                       codigo_reporte,
                                       sum(monto) AS monto_sabado
                                FROM db_tesoreria.transaccion 
                                WHERE codigo_operacion = 43 AND codigo_estado <> 0 AND dia_operacion = 6
                                GROUP BY codigo_empresa, anio_operacion, semana_operacion, codigo_reporte
                              ) g
                    ON a.codigo_empresa = g.codigo_empresa AND a.anio_operacion = g.anio_operacion AND a.semana_operacion = g.semana_operacion AND a.codigo_reporte = g.codigo_reporte
                    LEFT JOIN ( SELECT codigo_empresa,
                                       anio_operacion, 
                                       semana_operacion, 
                                       codigo_reporte,
                                       sum(monto) AS monto_domingo
                                FROM db_tesoreria.transaccion 
                                WHERE codigo_operacion = 43 AND codigo_estado <> 0 AND dia_operacion = 6
                                GROUP BY codigo_empresa, anio_operacion, semana_operacion, codigo_reporte
                              ) h
                    ON a.codigo_empresa = h.codigo_empresa AND a.anio_operacion = h.anio_operacion AND a.semana_operacion = h.semana_operacion AND a.codigo_reporte = h.codigo_reporte
                    INNER JOIN db_admon.empresa x
                    ON a.codigo_empresa = x.codigo_empresa
                    WHERE a.anio_operacion = @AnioOperacion 
                      " + filterEmpresa + @"
                      " + filterSemanaOperacion;

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        // CommandType, Gets or sets a value indicating how the CommandText property is to be interpreted.
                        // The Text CommandType.Text is used when the command is raw SQL
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@AnioOperacion", anioOperacion);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            CompromisoFiscalCLS objCompromisoFiscalCLS;
                            lista = new List<CompromisoFiscalCLS>();
                            int postCodigoEmpresa = dr.GetOrdinal("codigo_empresa");
                            int postNombreEmpresa = dr.GetOrdinal("nombre_empresa");
                            int postCodigoReporte = dr.GetOrdinal("codigo_reporte");
                            int postAnioOperacion = dr.GetOrdinal("anio_operacion");
                            int postSemanaOperacion = dr.GetOrdinal("semana_operacion");
                            int postMontoTotal = dr.GetOrdinal("monto_total");
                            int postMontoLunes = dr.GetOrdinal("monto_lunes");
                            int postMontoMartes = dr.GetOrdinal("monto_martes");
                            int postMontoMiercoles = dr.GetOrdinal("monto_miercoles");
                            int postMontoJueves = dr.GetOrdinal("monto_jueves");
                            int postMontoViernes = dr.GetOrdinal("monto_viernes");
                            int postMontoSabado = dr.GetOrdinal("monto_sabado");
                            int postMontoDomingo = dr.GetOrdinal("monto_domingo");
                            while (dr.Read())
                            {
                                objCompromisoFiscalCLS = new CompromisoFiscalCLS();
                                objCompromisoFiscalCLS.CodigoEmpresa = dr.GetInt16(postCodigoEmpresa);
                                objCompromisoFiscalCLS.NombreEmpresa = dr.GetString(postNombreEmpresa);
                                objCompromisoFiscalCLS.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objCompromisoFiscalCLS.AnioOperacion = dr.GetInt16(postAnioOperacion);
                                objCompromisoFiscalCLS.SemanaOperacion = dr.GetByte(postSemanaOperacion);
                                objCompromisoFiscalCLS.MontoTotal = dr.GetDecimal(postMontoTotal);
                                objCompromisoFiscalCLS.MontoLunes = dr.GetDecimal(postMontoLunes);
                                objCompromisoFiscalCLS.MontoMartes = dr.GetDecimal(postMontoMartes);
                                objCompromisoFiscalCLS.MontoMiercoles = dr.GetDecimal(postMontoMiercoles);
                                objCompromisoFiscalCLS.MontoJueves = dr.GetDecimal(postMontoJueves);
                                objCompromisoFiscalCLS.MontoViernes = dr.GetDecimal(postMontoViernes);
                                objCompromisoFiscalCLS.MontoSabado = dr.GetDecimal(postMontoSabado);
                                objCompromisoFiscalCLS.MontoDomingo = dr.GetDecimal(postMontoDomingo);
                                lista.Add(objCompromisoFiscalCLS);
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

        public List<CompromisoFiscalDetalleCLS> GetDetalleCompromisoFiscal(int codigoEmpresa, int anioOperacion, int semanaOperacion)
        {
            List<CompromisoFiscalDetalleCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @" 
                    SELECT x.codigo_transaccion,
                           x.codigo_empresa,
	                       y.nombre_comercial AS nombre_empresa, 
	                       x.anio_operacion, 
	                       x.semana_operacion, 
                           x.codigo_reporte, 
	                       x.dia_operacion,
	                       CASE
		                     WHEN x.dia_operacion = 1 THEN 'Lunes'
		                     WHEN x.dia_operacion = 2 THEN 'Martes'
		                     WHEN x.dia_operacion = 3 THEN 'Miercoles'
		                     WHEN x.dia_operacion = 4 THEN 'Jueves'
		                     WHEN x.dia_operacion = 5 THEN 'Viernes'
		                     WHEN x.dia_operacion = 6 THEN 'Sábado'
		                     WHEN x.dia_operacion = 7 THEN 'Domingo'
		                     ELSE 'No definido'
	                       END AS nombre_dia,
                           x.codigo_operacion,
                           z.nombre_operacion, 
	                       x.monto,
	                       FORMAT(x.fecha_ing, 'dd/MM/yyyy, hh:mm:ss') AS fecha_ing_str,
	                       x.usuario_ing,
                           1 AS permiso_anular 
                    FROM db_tesoreria.transaccion x
                    INNER JOIN db_admon.empresa y
                    ON x.codigo_empresa = y.codigo_empresa
                    INNER JOIN db_tesoreria.operacion z
                    ON x.codigo_operacion = z.codigo_operacion
                    WHERE x.codigo_operacion = 43 
                      AND x.codigo_estado <> 0
                      AND x.codigo_empresa = @CodigoEmpresa 
                      AND x.anio_operacion = @AnioOperacion
                      AND x.semana_operacion = @SemanaOperacion";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        // CommandType, Gets or sets a value indicating how the CommandText property is to be interpreted.
                        // The Text CommandType.Text is used when the command is raw SQL
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEmpresa", codigoEmpresa);
                        cmd.Parameters.AddWithValue("@AnioOperacion", anioOperacion);
                        cmd.Parameters.AddWithValue("@SemanaOperacion", semanaOperacion);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            CompromisoFiscalDetalleCLS objCompromisoFiscalDetalleCLS;
                            lista = new List<CompromisoFiscalDetalleCLS>();
                            int postCodigoTransaccion = dr.GetOrdinal("codigo_transaccion");
                            int postCodigoEmpresa = dr.GetOrdinal("codigo_empresa");
                            int postNombreEmpresa = dr.GetOrdinal("nombre_empresa");
                            int postCodigoReporte = dr.GetOrdinal("codigo_reporte");
                            int postAnioOperacion = dr.GetOrdinal("anio_operacion");
                            int postSemanaOperacion = dr.GetOrdinal("semana_operacion");
                            int postDiaOperacion = dr.GetOrdinal("dia_operacion");
                            int postNombreDia = dr.GetOrdinal("nombre_dia");
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postOperacion = dr.GetOrdinal("nombre_operacion");
                            int postFechaIngStr = dr.GetOrdinal("fecha_ing_str");
                            int postUsuarioIng = dr.GetOrdinal("usuario_ing");
                            int postMonto = dr.GetOrdinal("monto");
                            int postPermisoAnular = dr.GetOrdinal("permiso_anular");
                            while (dr.Read())
                            {
                                objCompromisoFiscalDetalleCLS = new CompromisoFiscalDetalleCLS();
                                objCompromisoFiscalDetalleCLS.CodigoTransaccion = dr.GetInt64(postCodigoTransaccion);
                                objCompromisoFiscalDetalleCLS.CodigoEmpresa = dr.GetInt16(postCodigoEmpresa);
                                objCompromisoFiscalDetalleCLS.NombreEmpresa = dr.GetString(postNombreEmpresa);
                                objCompromisoFiscalDetalleCLS.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objCompromisoFiscalDetalleCLS.AnioOperacion = dr.GetInt16(postAnioOperacion);
                                objCompromisoFiscalDetalleCLS.SemanaOperacion = dr.GetByte(postSemanaOperacion);
                                objCompromisoFiscalDetalleCLS.DiaOperacion = dr.GetByte(postDiaOperacion);
                                objCompromisoFiscalDetalleCLS.NombreDia = dr.GetString(postNombreDia);
                                objCompromisoFiscalDetalleCLS.FechaIngresoStr = dr.GetString(postFechaIngStr);
                                objCompromisoFiscalDetalleCLS.UsuarioIng = dr.GetString(postUsuarioIng);
                                objCompromisoFiscalDetalleCLS.Monto = dr.GetDecimal(postMonto);
                                objCompromisoFiscalDetalleCLS.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objCompromisoFiscalDetalleCLS.Operacion = dr.GetString(postOperacion);
                                objCompromisoFiscalDetalleCLS.PermisoAnular = (byte)dr.GetInt32(postPermisoAnular);
                                lista.Add(objCompromisoFiscalDetalleCLS);

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

        public string CargarCompromisoFiscal(List<TransaccionCLS> listaTransacciones, string usuarioAct)
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
                    StringBuilder cadena = new StringBuilder();
                    int contador = 0;
                    foreach (TransaccionCLS obj in listaTransacciones)
                    {
                        cadena.Append(obj.CodigoTransaccion.ToString());
                        cadena.Append(',');
                        contador++;
                    }

                    string listadoTransacciones = cadena.ToString().TrimEnd(',');
                    string sentenciaSQL = @"
                    UPDATE db_tesoreria.transaccion 
                    SET codigo_estado = @CodigoEstadoTransaccion,
                        usuario_act = @UsuarioAct,
                        fecha_act = @FechaAct    
                    WHERE codigo_transaccion IN (" + listadoTransacciones.ToString() + ")"; 

                    cmd.CommandText = sentenciaSQL;
                    cmd.Parameters.AddWithValue("@CodigoEstadoTransaccion", Constantes.EstadoTransacccion.INCLUIDO_EN_REPORTE);
                    cmd.Parameters.AddWithValue("@UsuarioAct", usuarioAct);
                    cmd.Parameters.AddWithValue("@FechaAct", DateTime.Now);
                    cmd.ExecuteNonQuery();

                    transaction.Commit();
                    conexion.Close();

                    resultado = "OK";
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

        public CompromisoFiscalCLS GetMontoCompromisosFiscal(int anioOperacion, int semanaOperacion)
        {
            CompromisoFiscalCLS objCompromisoFiscal = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_tesoreria.uspGetCompromisoFiscal", conexion))
                    {
                        // le indico que es un procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@AnioOperacion", anioOperacion);
                        cmd.Parameters.AddWithValue("@SemanaOperacion", semanaOperacion);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            int postMonto = dr.GetOrdinal("monto");
                            while (dr.Read())
                            {
                                objCompromisoFiscal = new CompromisoFiscalCLS();
                                objCompromisoFiscal.MontoTotal = dr.GetDecimal(postMonto);
                                objCompromisoFiscal.MontoTotalStr = dr.GetDecimal(postMonto).ToString("N2");
                            }
                        }
                    }
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                    objCompromisoFiscal = null;

                }
                return objCompromisoFiscal;
            }
        }

    }
}
