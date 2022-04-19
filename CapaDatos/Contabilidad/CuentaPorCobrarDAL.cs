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
    public class CuentaPorCobrarDAL: CadenaConexion
    {
        public string CargarSaldosIniciales(int anioOperacion, int semanaOperacion, string usuarioIng)
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
                    string sentenciaInsertReporte = @"
                    INSERT INTO db_contabilidad.cxc_reporte(codigo_reporte,anio_operacion,semana_operacion,codigo_estado,usuario_ing, fecha_ing, usuario_act, fecha_act) 
                    VALUES(@CodigoReporte,@AnioOperacion,@SemanaOperacion,@EstadoReporteValido,@UsuarioIng,@FechaIng,@UsuarioAct,@FechaAct)";
                    cmd.CommandText = sentenciaInsertReporte;
                    cmd.Parameters.AddWithValue("@CodigoReporte", 100);
                    cmd.Parameters.AddWithValue("@AnioOperacion", anioOperacion);
                    cmd.Parameters.AddWithValue("@SemanaOperacion", semanaOperacion);
                    cmd.Parameters.AddWithValue("@EstadoReporteValido", Constantes.CuentaPorCobrar.EstadoReporte.VALIDO);
                    cmd.Parameters.AddWithValue("@UsuarioIng", usuarioIng);
                    cmd.Parameters.AddWithValue("@FechaIng", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UsuarioAct", DBNull.Value);
                    cmd.Parameters.AddWithValue("@FechaAct", DBNull.Value);
                    cmd.ExecuteNonQuery();

                    string sentenciaInsertReporteDetalle = @"
                    INSERT INTO db_contabilidad.cxc_reporte_detalle(codigo_reporte,anio_operacion,semana_operacion,codigo_operacion,codigo_entidad,codigo_categoria,saldo_inicial,monto_devolucion,saldo_final,observaciones,estado,usuario_ing,fecha_ing)
                    SELECT @CodigoReporte AS codigo_reporte,
                           @AnioOperacion AS anio_operacion, 
		                   @SemanaOperacion AS semana_operacion,
                           codigo_operacion,
                           codigo_entidad, 
		                   codigo_categoria,
		                   0 AS saldo_inicial,
		                   0 AS monto_devolucion,
		                   monto AS saldo_final,
		                   NULL AS observaciones,
		                   1 AS estado,
		                   @UsuarioIng AS usuario_ing,
		                   GETDATE() AS fecha_ing
                    FROM db_contabilidad.cuenta_por_cobrar
                    WHERE carga_inicial = 1 
                      AND codigo_estado <> @CodigoEstadoAnulado";

                    cmd.CommandText = sentenciaInsertReporteDetalle;
                    cmd.Parameters["@CodigoReporte"].Value = 100;
                    cmd.Parameters["@AnioOperacion"].Value = anioOperacion;
                    cmd.Parameters["@SemanaOperacion"].Value = semanaOperacion;
                    cmd.Parameters["@UsuarioIng"].Value = usuarioIng;
                    cmd.Parameters.AddWithValue("@CodigoEstadoAnulado", Constantes.CuentaPorCobrar.Estado.ANULADO);
                    cmd.ExecuteNonQuery();

                    string sentenciaUpdateCuentasPorCobrar = @"
                    UPDATE db_contabilidad.cuenta_por_cobrar 
                    SET codigo_estado = @CodigoEstadoIncluidoEnReporte 
                    WHERE codigo_estado = @CodigoEstadoTemporal 
                      AND carga_inicial = 1";
                    cmd.CommandText = sentenciaUpdateCuentasPorCobrar;
                    cmd.Parameters.AddWithValue("@CodigoEstadoIncluidoEnReporte", Constantes.CuentaPorCobrar.Estado.INCLUIDO_EN_REPORTE);
                    cmd.Parameters.AddWithValue("@CodigoEstadoTemporal", Constantes.CuentaPorCobrar.Estado.TEMPORAL);
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

        public string GuardarCuentaPorCobrar(CuentaPorCobrarCLS objCuentaPorCobrar, string usuarioIng, int cargaInicial)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaContabilidad))
            {
                conexion.Open();

                SqlCommand cmd = conexion.CreateCommand();

                try
                {
                    string sqlSequence = "SELECT NEXT VALUE FOR db_contabilidad.SQ_CUENTA_POR_COBRAR";
                    cmd.CommandText = sqlSequence;
                    long codigoCuentaPorCobrar = (long)cmd.ExecuteScalar();

                    string sentenciaSQL = @"
                    INSERT INTO db_contabilidad.cuenta_por_cobrar(codigo_cxc,codigo_tipo_cxc,codigo_categoria_entidad,codigo_categoria,codigo_entidad,nombre_entidad,fecha_prestamo,fecha_inicio_pago,anio_operacion,semana_operacion,monto,observaciones,codigo_transaccion,codigo_planilla,codigo_operacion,codigo_estado,usuario_ing,fecha_ing,usuario_act,fecha_act,carga_inicial,codigo_pago,codigo_reporte)
                    VALUES(@CodigoCuentaCobrar,
                           @CodigoTipoCuentaPorCobrar,
                           @CodigoCategoriaEntidad,
                           @CodigoCategoria,
                           @CodigoEntidad,
                           @NombreEntidad,
                           @FechaPrestamo,
                           @FechaInicioPago,
                           @AnioOperacion,
                           @SemanaOperacion,
                           @Monto,
                           @Observaciones,
                           @CodigoTransaccion,
                           @CodigoPlanilla,
                           @CodigoOperacion,
                           @CodigoEstado,
                           @UsuarioIng,
                           @FechaIng,
                           @UsuarioAct,
                           @FechaAct,
                           @CargaInicial,
                           @CodigoPago, 
                           @CodigoReporte)";

                    cmd.CommandText = sentenciaSQL;
                    cmd.Parameters.AddWithValue("@CodigoCuentaCobrar", codigoCuentaPorCobrar);
                    cmd.Parameters.AddWithValue("@CodigoTipoCuentaPorCobrar", objCuentaPorCobrar.CodigoTipoCuentaPorCobrar);
                    cmd.Parameters.AddWithValue("@CodigoCategoriaEntidad", objCuentaPorCobrar.CodigoCategoriaEntidad);
                    cmd.Parameters.AddWithValue("@CodigoCategoria", objCuentaPorCobrar.CodigoCategoria);
                    cmd.Parameters.AddWithValue("@CodigoEntidad", objCuentaPorCobrar.CodigoEntidad);
                    cmd.Parameters.AddWithValue("@NombreEntidad", objCuentaPorCobrar.NombreEntidad);
                    cmd.Parameters.AddWithValue("@FechaPrestamo", objCuentaPorCobrar.FechaPrestamo == null ? DBNull.Value : objCuentaPorCobrar.FechaPrestamo);
                    cmd.Parameters.AddWithValue("@FechaInicioPago", objCuentaPorCobrar.FechaInicioPago == null ? DBNull.Value : objCuentaPorCobrar.FechaInicioPago);
                    cmd.Parameters.AddWithValue("@AnioOperacion", objCuentaPorCobrar.AnioOperacion);
                    cmd.Parameters.AddWithValue("@SemanaOperacion", objCuentaPorCobrar.SemanaOperacion);
                    cmd.Parameters.AddWithValue("@Monto", objCuentaPorCobrar.Monto);
                    cmd.Parameters.AddWithValue("@Observaciones", objCuentaPorCobrar.Observaciones == null ? DBNull.Value : objCuentaPorCobrar.Observaciones);
                    cmd.Parameters.AddWithValue("@CodigoTransaccion", objCuentaPorCobrar.CodigoTransaccion == null ? DBNull.Value : objCuentaPorCobrar.CodigoTransaccion);
                    cmd.Parameters.AddWithValue("@CodigoPlanilla", objCuentaPorCobrar.CodigoPlanilla == null ? DBNull.Value : objCuentaPorCobrar.CodigoPlanilla);
                    cmd.Parameters.AddWithValue("@CodigoOperacion", objCuentaPorCobrar.CodigoOperacion);
                    cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.CuentaPorCobrar.Estado.TEMPORAL);
                    cmd.Parameters.AddWithValue("@UsuarioIng", usuarioIng);
                    cmd.Parameters.AddWithValue("@FechaIng", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UsuarioAct", DBNull.Value);
                    cmd.Parameters.AddWithValue("@FechaAct", DBNull.Value);
                    cmd.Parameters.AddWithValue("@CargaInicial", cargaInicial);
                    cmd.Parameters.AddWithValue("@CodigoPago", DBNull.Value);
                    cmd.Parameters.AddWithValue("@CodigoReporte", cargaInicial == 1 ? 100: 0);
                    cmd.ExecuteNonQuery();

                    resultado = codigoCuentaPorCobrar.ToString();
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    resultado = "Error [0]: " + ex.Message;
                }
            }

            return resultado;
        }

        public string ActualizarCuentaPorCobrarTemporal(CuentaPorCobrarCLS objCuentaPorCobrar, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaContabilidad))
            {
                conexion.Open();

                SqlCommand cmd = conexion.CreateCommand();

                try
                {
                    string sentenciaUpdate = @"
                    UPDATE db_contabilidad.cuenta_por_cobrar
                    SET monto = @Monto,
                        usuario_act = @UsuarioAct,
                        fecha_act = @FechaAct
                    WHERE codigo_cxc = @CodigoCuentaPorCobrar";

                    cmd.CommandText = sentenciaUpdate;
                    cmd.Parameters.AddWithValue("@CodigoCuentaPorCobrar", objCuentaPorCobrar.CodigoCuentaPorCobrar);
                    cmd.Parameters.AddWithValue("@Monto", objCuentaPorCobrar.Monto);
                    cmd.Parameters.AddWithValue("@UsuarioAct", usuarioAct);
                    cmd.Parameters.AddWithValue("@FechaAct", DateTime.Now);
                    cmd.ExecuteNonQuery();

                    conexion.Close();
                    resultado = "OK";
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    resultado = "Error [0]: " + ex.Message;
                }
            }

            return resultado;
        }

        public string AnularCuentaPorCobrarTemporal(long codigoCuentaPorCobrar, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaContabilidad))
            {
                conexion.Open();
                SqlCommand cmd = conexion.CreateCommand();

                try
                {
                    string sentenciaUpdate = @"
                    UPDATE db_contabilidad.cuenta_por_cobrar
                    SET usuario_act = @UsuarioAct,
                        fecha_act = @FechaAct,
                        codigo_estado = @CodigoEstadoAnulado
                    WHERE codigo_cxc = @CodigoCuentaPorCobrar";

                    cmd.CommandText = sentenciaUpdate;
                    cmd.Parameters.AddWithValue("@CodigoCuentaPorCobrar", codigoCuentaPorCobrar);
                    cmd.Parameters.AddWithValue("@CodigoEstadoAnulado", Constantes.CuentaPorCobrar.Estado.ANULADO);
                    cmd.Parameters.AddWithValue("@UsuarioAct", usuarioAct);
                    cmd.Parameters.AddWithValue("@FechaAct", DateTime.Now);
                    cmd.ExecuteNonQuery();

                    conexion.Close();
                    resultado = "OK";
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    resultado = "Error [0]: " + ex.Message;
                }
            }

            return resultado;
        }


        public string CargarCxCTemporal(string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    string sqlUpdate = @"
                    UPDATE db_contabilidad.cuenta_por_cobrar 
                    SET codigo_estado = @CodigoEstadoRegistrado,
                        usuario_act = @UsuarioAct,
                        fecha_act = @FechaAct
                    WHERE codigo_estado = @CodigoEstadoTemporal AND
                          carga_inicial = 0";
                    using (SqlCommand cmd = new SqlCommand(sqlUpdate, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sqlUpdate;
                        cmd.Parameters.AddWithValue("@CodigoEstadoRegistrado", Constantes.CuentaPorCobrar.Estado.REGISTRADO);
                        cmd.Parameters.AddWithValue("@CodigoEstadoTemporal", Constantes.CuentaPorCobrar.Estado.TEMPORAL);
                        cmd.Parameters.AddWithValue("@UsuarioAct", usuarioAct);
                        cmd.Parameters.AddWithValue("@FechaAct", DateTime.Now);
                        cmd.ExecuteNonQuery();

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

        public List<TipoCuentaPorCobrarCLS> GetListTiposCuentasPorCobrar()
        {
            List<TipoCuentaPorCobrarCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaContabilidad))
            {
                try
                {
                    string sql = @"
                    SELECT codigo_tipo_cxc, 
	                       nombre 
                    FROM db_contabilidad.tipo_cxc
                    WHERE codigo_tipo_cxc <> 0
                    ORDER BY nombre ASC";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        // CommandType, Gets or sets a value indicating how the CommandText property is to be interpreted.
                        // The Text CommandType.Text is used when the command is raw SQL
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            TipoCuentaPorCobrarCLS objTipoCuentaPorCobrarCLS;
                            lista = new List<TipoCuentaPorCobrarCLS>();
                            int postCodigoTipoCuentaPorCobrar = dr.GetOrdinal("codigo_tipo_cxc");
                            int postNombre = dr.GetOrdinal("nombre");
                            while (dr.Read())
                            {
                                objTipoCuentaPorCobrarCLS = new TipoCuentaPorCobrarCLS();
                                objTipoCuentaPorCobrarCLS.CodigoTipoCuentaPorCobrar = dr.GetByte(postCodigoTipoCuentaPorCobrar);
                                objTipoCuentaPorCobrarCLS.Nombre = dr.GetString(postNombre);
                                lista.Add(objTipoCuentaPorCobrarCLS);
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

        public List<CuentaPorCobrarCLS> GetCuentasPorCobrarCargaInicial()
        {
            List<CuentaPorCobrarCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaContabilidad))
            {
                try
                {
                    string sql = @"
                    SELECT x.codigo_categoria,
					       y.nombre AS categoria,
 	                       x.codigo_entidad,
						   db_tesoreria.GetNombreEntidadCompleto(x.codigo_categoria, x.codigo_entidad) AS entidad,
	                       x.monto,
                           x.codigo_operacion,
                           z.nombre_operacion 
                    FROM db_contabilidad.cuenta_por_cobrar x
					INNER JOIN db_tesoreria.entidad_categoria y
					ON x.codigo_categoria = y.codigo_categoria_entidad
                    INNER JOIN db_tesoreria.operacion z
                    ON x.codigo_operacion = z.codigo_operacion
                    WHERE x.codigo_estado = @CodigoEstadoTemporal 
                      AND x.carga_inicial = 1
					ORDER BY x.codigo_categoria, x.codigo_entidad";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        // CommandType, Gets or sets a value indicating how the CommandText property is to be interpreted.
                        // The Text CommandType.Text is used when the command is raw SQL
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoTemporal", Constantes.CuentaPorCobrar.Estado.TEMPORAL);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            CuentaPorCobrarCLS objCuentaPorCobrarCLS;
                            lista = new List<CuentaPorCobrarCLS>();
                            int postCodigoCategoria = dr.GetOrdinal("codigo_categoria");
                            int postCategoria = dr.GetOrdinal("categoria");
                            int postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            int postNombreEntidad = dr.GetOrdinal("entidad");
                            int postMonto = dr.GetOrdinal("monto");
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postOperacion = dr.GetOrdinal("nombre_operacion");
                            while (dr.Read())
                            {
                                objCuentaPorCobrarCLS = new CuentaPorCobrarCLS();
                                objCuentaPorCobrarCLS.CodigoCategoria = dr.GetInt16(postCodigoCategoria);
                                objCuentaPorCobrarCLS.Categoria = dr.GetString(postCategoria);
                                objCuentaPorCobrarCLS.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objCuentaPorCobrarCLS.NombreEntidad = dr.GetString(postNombreEntidad);
                                objCuentaPorCobrarCLS.Monto = dr.GetDecimal(postMonto);
                                objCuentaPorCobrarCLS.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objCuentaPorCobrarCLS.Operacion = dr.GetString(postOperacion);
                                lista.Add(objCuentaPorCobrarCLS);
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

        public List<CuentaPorCobrarCLS> GetCuentasPorCobrarTemporal()
        {
            List<CuentaPorCobrarCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaContabilidad))
            {
                try
                {
                    string sql = @"
                    SELECT x.codigo_cxc,
                           x.codigo_categoria,
					       y.nombre AS categoria,
 	                       x.codigo_entidad,
						   db_tesoreria.GetNombreEntidadCompleto(x.codigo_categoria, x.codigo_entidad) AS entidad,
	                       x.monto,
                           x.codigo_operacion,
                           z.nombre_operacion,
                           1 AS permiso_editar,
                           1 AS permiso_anular 
                    FROM db_contabilidad.cuenta_por_cobrar x
					INNER JOIN db_tesoreria.entidad_categoria y
					ON x.codigo_categoria = y.codigo_categoria_entidad
                    INNER JOIN db_tesoreria.operacion z
                    ON x.codigo_operacion = z.codigo_operacion
                    WHERE x.codigo_estado = @CodigoEstadoTemporal AND 
                          x.carga_inicial = 0
					ORDER BY x.codigo_categoria, x.codigo_entidad";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        // CommandType, Gets or sets a value indicating how the CommandText property is to be interpreted.
                        // The Text CommandType.Text is used when the command is raw SQL
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoTemporal", Constantes.CuentaPorCobrar.Estado.TEMPORAL);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            CuentaPorCobrarCLS objCuentaPorCobrarCLS;
                            lista = new List<CuentaPorCobrarCLS>();
                            int postCodigoCxC = dr.GetOrdinal("codigo_cxc");
                            int postCodigoCategoria = dr.GetOrdinal("codigo_categoria");
                            int postCategoria = dr.GetOrdinal("categoria");
                            int postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            int postNombreEntidad = dr.GetOrdinal("entidad");
                            int postMonto = dr.GetOrdinal("monto");
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postOperacion = dr.GetOrdinal("nombre_operacion");
                            int postPermisoEditar = dr.GetOrdinal("permiso_editar");
                            int postPermisoAnular = dr.GetOrdinal("permiso_anular");
                            while (dr.Read())
                            {
                                objCuentaPorCobrarCLS = new CuentaPorCobrarCLS();
                                objCuentaPorCobrarCLS.CodigoCuentaPorCobrar = dr.GetInt64(postCodigoCxC);
                                objCuentaPorCobrarCLS.CodigoCategoria = dr.GetInt16(postCodigoCategoria);
                                objCuentaPorCobrarCLS.Categoria = dr.GetString(postCategoria);
                                objCuentaPorCobrarCLS.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objCuentaPorCobrarCLS.NombreEntidad = dr.GetString(postNombreEntidad);
                                objCuentaPorCobrarCLS.Monto = dr.GetDecimal(postMonto);
                                objCuentaPorCobrarCLS.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objCuentaPorCobrarCLS.Operacion = dr.GetString(postOperacion);
                                objCuentaPorCobrarCLS.PermisoEditar = (byte)dr.GetInt32(postPermisoEditar);
                                objCuentaPorCobrarCLS.PermisoAnular = (byte)dr.GetInt32(postPermisoAnular);
                                lista.Add(objCuentaPorCobrarCLS);
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

    }
}
