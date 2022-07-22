using CapaEntidad.Ventas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Ventas
{
    public class TrasladoVentasContadoDAL : CadenaConexion
    {
        public string GuardarTraslado(TrasladoVentasContadoCLS objTraslado, string usuarioIng)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    INSERT INTO db_tesoreria.traslado_ventas_contado(codigo_traslado,fecha_operacion,monto_efectivo,monto_cheques,monto_transferencia,monto,fecha_recepcion,usuario_recepcion,
	                observaciones_generacion,observaciones_recepcion,anio_operacion,semana_operacion,dia_operacion,codigo_estado,usuario_ing,fecha_ing,usuario_act,fecha_act)
                    VALUES(NEXT VALUE FOR db_tesoreria.SQ_TRASLADO_VENTA_CONTADO,
                           @FechaOperacion,
                           @MontoEfectivo,
                           @MontoCheques,
                           @MontoTransferencia, 
                           @Monto,
                           @FechaRecepcion,
                           @UsuarioRecepcion,
                           @ObservacionesGeneracion,
                           @ObservacionesRecepcion,
                           @AnioOperacion,
                           @SemanaOperacion,
                           @DiaOperacion,
                           @CodigoEstado,
                           @UsuarioIng,
                           @FechaIng,
                           @UsuarioAct,
                           @FechaAct)";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.AddWithValue("@FechaOperacion", objTraslado.FechaOperacion);
                        cmd.Parameters.AddWithValue("@MontoEfectivo", objTraslado.MontoEfectivo);
                        cmd.Parameters.AddWithValue("@MontoCheques", objTraslado.MontoCheques);
                        cmd.Parameters.AddWithValue("@MontoTransferencia", objTraslado.MontoTransferencia);
                        cmd.Parameters.AddWithValue("@Monto", objTraslado.Monto);
                        cmd.Parameters.AddWithValue("@FechaRecepcion", DBNull.Value);
                        cmd.Parameters.AddWithValue("@UsuarioRecepcion", DBNull.Value);
                        cmd.Parameters.AddWithValue("@ObservacionesGeneracion", DBNull.Value);
                        cmd.Parameters.AddWithValue("@ObservacionesRecepcion", DBNull.Value);
                        cmd.Parameters.AddWithValue("@AnioOperacion", 0);
                        cmd.Parameters.AddWithValue("@SemanaOperacion", 0);
                        cmd.Parameters.AddWithValue("@DiaOperacion", 0);
                        cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.Ventas.EstadoTrasladoCaja.GENERADO);
                        cmd.Parameters.AddWithValue("@UsuarioIng", usuarioIng);
                        cmd.Parameters.AddWithValue("@FechaIng", DateTime.Now);
                        cmd.Parameters.AddWithValue("@UsuarioAct", DBNull.Value);
                        cmd.Parameters.AddWithValue("@FechaAct", DBNull.Value);

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

        public List<TrasladoVentasContadoCLS> GetTrasladosEnProceso()
        {
            List<TrasladoVentasContadoCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT x.codigo_traslado,
                           FORMAT(x.fecha_operacion,'dd/MM/yyyy') AS fecha_operacion_str,
	                       x.monto_efectivo,
	                       x.monto_cheques,
                           x.monto_transferencia,
	                       x.monto,
	                       x.anio_operacion,
	                       x.semana_operacion,
	                       x.dia_operacion,
                           FORMAT(x.fecha_ing, 'dd/MM/yyyy, hh:mm:ss') AS fecha_generacion_str,
	                       x.usuario_ing,
	                       x.codigo_estado,
	                       y.nombre AS estado,
                           CASE
                             WHEN x.codigo_estado = @CodigoEstadoGenerado THEN 1
                             ELSE 0
                           END AS permiso_anular, 
                           CASE
                             WHEN x.codigo_estado = @CodigoEstadoAceptado THEN 1
                             ELSE 0
                           END AS permiso_imprimir,
                           CASE
                             WHEN x.codigo_estado = @CodigoEstadoGenerado THEN 1
                             ELSE 0
                           END AS permiso_traslado,
                           ( SELECT sum(monto)
                             FROM db_tesoreria.traslado_ventas_contado
                             WHERE fecha_operacion = x.fecha_operacion
                               AND codigo_estado <> 0) AS monto_total_dia

                    FROM db_tesoreria.traslado_ventas_contado x
                    INNER JOIN db_tesoreria.estado_traslado_caja y
                    ON x.codigo_estado = y.codigo_estado
                    WHERE x.codigo_estado IN (@CodigoEstadoGenerado,@CodigoEstadoAceptado)
                    ORDER BY x.codigo_traslado DESC";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoGenerado", Constantes.Ventas.EstadoTrasladoCaja.GENERADO);
                        cmd.Parameters.AddWithValue("@CodigoEstadoAceptado", Constantes.Ventas.EstadoTrasladoCaja.ACEPTADO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            TrasladoVentasContadoCLS objTraslado;
                            lista = new List<TrasladoVentasContadoCLS>();
                            int postCodigoTraslado = dr.GetOrdinal("codigo_traslado");
                            int postFechaOperacionStr = dr.GetOrdinal("fecha_operacion_str");
                            int postMontoEfectivo = dr.GetOrdinal("monto_efectivo");
                            int postMontoCheques = dr.GetOrdinal("monto_cheques");
                            int postMontoTransferencia = dr.GetOrdinal("monto_transferencia");
                            int postMonto = dr.GetOrdinal("monto");
                            int postAnioOperacion = dr.GetOrdinal("anio_operacion");
                            int postSemanaOperacion = dr.GetOrdinal("semana_operacion");
                            int postDiaOperacion = dr.GetOrdinal("dia_operacion");
                            int postFechaGeneracionStr = dr.GetOrdinal("fecha_generacion_str");
                            int postUsuarioIng = dr.GetOrdinal("usuario_ing");
                            int postCodigoEstado = dr.GetOrdinal("codigo_estado");
                            int postEstado = dr.GetOrdinal("estado");
                            int postPermisoAnular = dr.GetOrdinal("permiso_anular");
                            int postPermisoImprimir = dr.GetOrdinal("permiso_imprimir");
                            int postPermisoTraslado = dr.GetOrdinal("permiso_traslado");
                            int postMontoTotalDia = dr.GetOrdinal("monto_total_dia");

                            while (dr.Read())
                            {
                                objTraslado = new TrasladoVentasContadoCLS();

                                objTraslado.CodigoTraslado = dr.GetInt32(postCodigoTraslado);
                                objTraslado.FechaOperacionStr = dr.GetString(postFechaOperacionStr);
                                objTraslado.MontoEfectivo = dr.GetDecimal(postMontoEfectivo);
                                objTraslado.MontoCheques = dr.GetDecimal(postMontoCheques);
                                objTraslado.MontoTransferencia = dr.GetDecimal(postMontoTransferencia);
                                objTraslado.Monto = dr.GetDecimal(postMonto);
                                objTraslado.AnioOperacion = dr.GetInt16(postAnioOperacion);
                                objTraslado.SemanaOperacion = dr.GetByte(postSemanaOperacion);
                                objTraslado.DiaOperacion = dr.GetByte(postDiaOperacion);
                                objTraslado.FechaGeneracionStr = dr.GetString(postFechaGeneracionStr);
                                objTraslado.UsuarioIng= dr.GetString(postUsuarioIng);
                                objTraslado.CodigoEstado = dr.GetInt16(postCodigoEstado);
                                objTraslado.Estado = dr.GetString(postEstado);
                                objTraslado.PermisoAnular = dr.GetInt32(postPermisoAnular);
                                objTraslado.PermisoImprimir = dr.GetInt32(postPermisoImprimir);
                                objTraslado.PermisoTraslado = dr.GetInt32(postPermisoTraslado);
                                objTraslado.MontoTotalDia = dr.GetDecimal(postMontoTotalDia);


                                lista.Add(objTraslado);
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

        public List<TrasladoVentasContadoCLS> GetTrasladosParaRecepcion(int codigoTipoTraslado)
        {
            List<TrasladoVentasContadoCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT x.codigo_traslado,
                           FORMAT(x.fecha_operacion,'dd/MM/yyyy') AS fecha_operacion_str,
	                       x.monto_efectivo,
	                       x.monto_cheques,
                           x.monto_transferencia,
	                       x.monto,
	                       x.anio_operacion,
	                       x.semana_operacion,
	                       x.dia_operacion,
                           FORMAT(x.fecha_ing, 'dd/MM/yyyy, hh:mm:ss') AS fecha_generacion_str,
	                       x.usuario_ing,
	                       x.codigo_estado,
	                       y.nombre AS estado,
                           CASE
                             WHEN x.codigo_estado = @CodigoEstadoAceptado THEN 1
                             ELSE 0
                           END AS permiso_registrar

                    FROM db_tesoreria.traslado_ventas_contado x
                    INNER JOIN db_tesoreria.estado_traslado_caja y
                    ON x.codigo_estado = y.codigo_estado
                    WHERE x.codigo_estado = @CodigoEstadoAceptado
                    ORDER BY x.codigo_traslado DESC";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoAceptado", Constantes.Ventas.EstadoTrasladoCaja.ACEPTADO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            TrasladoVentasContadoCLS objTraslado;
                            lista = new List<TrasladoVentasContadoCLS>();
                            int postCodigoTraslado = dr.GetOrdinal("codigo_traslado");
                            int postFechaOperacionStr = dr.GetOrdinal("fecha_operacion_str");
                            int postMontoEfectivo = dr.GetOrdinal("monto_efectivo");
                            int postMontoCheques = dr.GetOrdinal("monto_cheques");
                            int postMontoTransferencia = dr.GetOrdinal("monto_transferencia");
                            int postMonto = dr.GetOrdinal("monto");
                            int postAnioOperacion = dr.GetOrdinal("anio_operacion");
                            int postSemanaOperacion = dr.GetOrdinal("semana_operacion");
                            int postDiaOperacion = dr.GetOrdinal("dia_operacion");
                            int postFechaGeneracionStr = dr.GetOrdinal("fecha_generacion_str");
                            int postUsuarioIng = dr.GetOrdinal("usuario_ing");
                            int postCodigoEstado = dr.GetOrdinal("codigo_estado");
                            int postEstado = dr.GetOrdinal("estado");
                            int postPermisoRegistrar = dr.GetOrdinal("permiso_registrar");

                            while (dr.Read())
                            {
                                objTraslado = new TrasladoVentasContadoCLS();

                                objTraslado.CodigoTraslado = dr.GetInt32(postCodigoTraslado);
                                objTraslado.FechaOperacionStr = dr.GetString(postFechaOperacionStr);
                                objTraslado.MontoEfectivo = dr.GetDecimal(postMontoEfectivo);
                                objTraslado.MontoCheques = dr.GetDecimal(postMontoCheques);
                                objTraslado.MontoTransferencia = dr.GetDecimal(postMontoTransferencia);
                                objTraslado.Monto = dr.GetDecimal(postMonto);
                                objTraslado.AnioOperacion = dr.GetInt16(postAnioOperacion);
                                objTraslado.SemanaOperacion = dr.GetByte(postSemanaOperacion);
                                objTraslado.DiaOperacion = dr.GetByte(postDiaOperacion);
                                objTraslado.FechaGeneracionStr = dr.GetString(postFechaGeneracionStr);
                                objTraslado.UsuarioIng = dr.GetString(postUsuarioIng);
                                objTraslado.CodigoEstado = dr.GetInt16(postCodigoEstado);
                                objTraslado.Estado = dr.GetString(postEstado);
                                objTraslado.PermisoRegistrar = dr.GetInt32(postPermisoRegistrar);

                                lista.Add(objTraslado);
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

        public string AnularTraslado(int codigoTraslado, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    UPDATE db_tesoreria.traslado_ventas_contado
                    SET codigo_estado = @CodigoEstadoAnulado,
                        usuario_act = @UsuarioAct,
                        fecha_act = @FechaAct
                    WHERE codigo_traslado = @CodigoTraslado";
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoAnulado", Constantes.Ventas.EstadoTrasladoCaja.ANULADO);
                        cmd.Parameters.AddWithValue("@UsuarioAct", usuarioAct);
                        cmd.Parameters.AddWithValue("@FechaAct", DateTime.Now);
                        cmd.Parameters.AddWithValue("@CodigoTraslado", codigoTraslado);
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

        public string AceptarTraslado(int codigoTraslado, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    UPDATE db_tesoreria.traslado_ventas_contado
                    SET codigo_estado = @CodigoEstadoAceptado,
                        usuario_act = @UsuarioAct,
                        fecha_act = @FechaAct
                    WHERE codigo_traslado = @CodigoTraslado";
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoAceptado", Constantes.Ventas.EstadoTrasladoCaja.ACEPTADO);
                        cmd.Parameters.AddWithValue("@UsuarioAct", usuarioAct);
                        cmd.Parameters.AddWithValue("@FechaAct", DateTime.Now);
                        cmd.Parameters.AddWithValue("@CodigoTraslado", codigoTraslado);
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




    }
}
