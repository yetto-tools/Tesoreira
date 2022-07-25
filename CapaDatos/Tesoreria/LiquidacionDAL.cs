using CapaEntidad.Administracion;
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
    public class LiquidacionDAL: CadenaConexion
    {
        public List<TrasladoLiquidacionCLS> GetTrasladosParaLiquidacion()
        {
            List<TrasladoLiquidacionCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sentenciaTraslados = @"
                    SELECT m.* 
                    FROM ( SELECT CAST(0 AS BIGINT) AS codigo_traslado, 
                                  anio_operacion, 
                                  semana_operacion, 
                                  db_admon.GetPeriodoSemana(anio_operacion,semana_operacion)  AS periodo,
                                  count(*) AS cantidad, 
                                  SUM(monto) AS monto_total,
                                  1 AS codigo_estado,
                                  'POR GENERAR'  AS estado,
                                  0 AS bloqueado,
                                  0 AS permiso_anular 

                           FROM db_tesoreria.transaccion
                           WHERE codigo_estado <> @CodigoEstadoTransaccion
                             AND liquidacion = 0
                             AND complemento_conta = 0
                             AND codigo_categoria_entidad <> @CodigoCategoriaEntidad
                             AND codigo_operacion = @CodigoOperacion  
                           GROUP BY anio_operacion, semana_operacion

                           UNION

                           SELECT x.codigo_traslado, 
                                  x.anio_operacion, 
                                  x.semana_operacion, 
                                  db_admon.GetPeriodoSemana(x.anio_operacion,x.semana_operacion)  AS periodo,
                                  (SELECT COUNT(*) FROM db_tesoreria.traslado_liquidacion_detalle WHERE codigo_traslado = x.codigo_traslado) AS cantidad,
                                  (SELECT SUM(monto) FROM db_tesoreria.traslado_liquidacion_detalle WHERE codigo_traslado = x.codigo_traslado) AS monto_total,
                                  x.codigo_estado,
                                  y.nombre AS estado, 
                                  1 AS bloqueado,
                                  1 AS permiso_anular 

                           FROM db_tesoreria.traslado_liquidacion x
                           INNER JOIN db_tesoreria.estado_traslado_liquidacion y
                           ON x.codigo_estado = y.codigo_estado_traslado
                           WHERE codigo_estado = @CodigoEstadoTraslado
                    ) m
                    ORDER BY m.anio_operacion DESC, m.semana_operacion DESC
                    ";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sentenciaTraslados, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoTransaccion", Constantes.EstadoTransacccion.ANULADO);
                        cmd.Parameters.AddWithValue("@CodigoCategoriaEntidad", Constantes.Entidad.Categoria.CLIENTES_ESPECIALES_2);
                        cmd.Parameters.AddWithValue("@CodigoOperacion", Constantes.Operacion.Ingreso.VENTAS_EN_RUTA);
                        cmd.Parameters.AddWithValue("@CodigoEstadoTraslado", Constantes.Liquidacion.EstadoTraslado.GENERADO);
                        cmd.ExecuteNonQuery();

                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            TrasladoLiquidacionCLS objTraslado;
                            lista = new List<TrasladoLiquidacionCLS>();
                            int postCodigoTraslado = dr.GetOrdinal("codigo_traslado");
                            int postAnioOperacion = dr.GetOrdinal("anio_operacion");
                            int postSemanaOperacion = dr.GetOrdinal("semana_operacion");
                            int postPeriodo = dr.GetOrdinal("periodo");
                            int postCantidad = dr.GetOrdinal("cantidad");
                            int postMontoTotal = dr.GetOrdinal("monto_total");
                            int postCodigoEstado = dr.GetOrdinal("codigo_estado");
                            int postEstado = dr.GetOrdinal("estado");
                            int postBloqueado = dr.GetOrdinal("bloqueado");
                            int postPermisoAnular = dr.GetOrdinal("permiso_anular");

                            while (dr.Read())
                            {
                                objTraslado = new TrasladoLiquidacionCLS();
                                objTraslado.CodigoTraslado = dr.GetInt64(postCodigoTraslado);
                                objTraslado.AnioOperacion = dr.GetInt16(postAnioOperacion);
                                objTraslado.SemanaOperacion = dr.GetByte(postSemanaOperacion);
                                objTraslado.Periodo = dr.GetString(postPeriodo);
                                objTraslado.Cantidad = dr.GetInt32(postCantidad);
                                objTraslado.MontoTotal = dr.GetDecimal(postMontoTotal);
                                objTraslado.CodigoEstadoTraslado = (byte) dr.GetInt32(postCodigoEstado);
                                objTraslado.EstadoTraslado = dr.GetString(postEstado);
                                objTraslado.bloqueado = (byte) dr.GetInt32(postBloqueado);
                                objTraslado.PermisoAnular = (byte)dr.GetInt32(postPermisoAnular);
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

        public List<TrasladoLiquidacionDetalleCLS> GetDetalleTrasladoLiquidacionPorGenerar(int anioOperacion, int semanaOperacion)
        {
            List<TrasladoLiquidacionDetalleCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sqlDetalleTraslados = @"
                    SELECT x.codigo_transaccion, 
                           CAST(0 AS BIGINT) AS codigo_transaccion_ant,
	                       CAST(0 AS BIGINT) AS codigo_traslado,
	                       COALESCE(x.codigo_empresa,0) AS codigo_empresa,
                           z.nombre_comercial AS nombre_empresa, 
                           x.fecha_operacion, 
                           FORMAT(x.fecha_operacion, 'dd/MM/yyyy') AS fecha_operacion_str,
                           x.dia_operacion,
                           CASE 
                             WHEN x.dia_operacion = 1 THEN 'LUNES'
                             WHEN x.dia_operacion = 2 THEN 'MARTES'
                             WHEN x.dia_operacion = 3 THEN 'MIERCOLES'
                             WHEN x.dia_operacion = 4 THEN 'JUEVES'
                             WHEN x.dia_operacion = 5 THEN 'VIERNES'
                             WHEN x.dia_operacion = 6 THEN 'SABADO'
                             WHEN x.dia_operacion = 7 THEN 'DOMINGO'
                             ELSE 'NO DEFINIDO'
                           END AS nombre_dia,
                           x.codigo_entidad AS codigo_vendedor,
                           db_tesoreria.GetNombreEntidad(x.codigo_categoria_entidad,x.codigo_entidad) AS nombre_vendedor,
	                       x.ruta,
                           x.codigo_canal_venta,
                           y.nombre AS canal_venta, 
                           x.monto,
	                       x.codigo_tipo_traslado,
                           CASE
                              WHEN x.codigo_tipo_traslado = 1 THEN 'NORMAL'
                              WHEN x.codigo_tipo_traslado = 2 THEN 'AJUSTE'
                              ELSE 'NO DEFINIDO'
                           END AS tipo_traslado,
	                       1 AS codigo_estado,
                           'ACTIVO' AS estado,
	                       x.usuario_ing,
	                       FORMAT(x.fecha_ing, 'dd/MM/yyyy, hh:mm:ss') AS fecha_ing_str

                    FROM db_tesoreria.transaccion x
                    INNER JOIN db_ventas.canal_venta y
                    ON x.codigo_canal_venta = y.codigo_canal_venta
                    LEFT JOIN db_admon.empresa z
                    ON x.codigo_empresa = z.codigo_empresa 
                    WHERE x.codigo_estado <> @CodigoEstadoTransaccion
                      AND x.liquidacion = 0
                      AND x.complemento_conta = 0
                      AND x.codigo_categoria_entidad <> @CodigoCategoriaEntidad
                      AND x.codigo_operacion = @CodigoOperacion
                      AND x.anio_operacion = @AnioOperacion
                      AND x.semana_operacion = @SemanaOperacion";  

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlDetalleTraslados, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoTransaccion", Constantes.EstadoTransacccion.ANULADO);
                        cmd.Parameters.AddWithValue("@CodigoCategoriaEntidad", Constantes.Entidad.Categoria.CLIENTES_ESPECIALES_2);
                        cmd.Parameters.AddWithValue("@CodigoOperacion", Constantes.Operacion.Ingreso.VENTAS_EN_RUTA);
                        cmd.Parameters.AddWithValue("@AnioOperacion", anioOperacion);
                        cmd.Parameters.AddWithValue("@SemanaOperacion", semanaOperacion);
                        cmd.ExecuteNonQuery();

                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            TrasladoLiquidacionDetalleCLS objTrasladoDetalle;
                            lista = new List<TrasladoLiquidacionDetalleCLS>();
                            int postCodigoTransaccion = dr.GetOrdinal("codigo_transaccion");
                            int postCodigoTransaccionAnt = dr.GetOrdinal("codigo_transaccion_ant");
                            int postCodigoTraslado = dr.GetOrdinal("codigo_traslado");
                            int postCodigoEmpresa = dr.GetOrdinal("codigo_empresa");
                            int postNombreEmpresa = dr.GetOrdinal("nombre_empresa");
                            int postFechaOperacion = dr.GetOrdinal("fecha_operacion");
                            int postFechaOperacionStr = dr.GetOrdinal("fecha_operacion_str");
                            int postDiaOperacion = dr.GetOrdinal("dia_operacion");
                            int postNombreDia = dr.GetOrdinal("nombre_dia");
                            int postCodigoVendedor = dr.GetOrdinal("codigo_vendedor");
                            int postNombreVendedor = dr.GetOrdinal("nombre_vendedor");
                            int postRuta = dr.GetOrdinal("ruta");
                            int postCodigoCanalVenta = dr.GetOrdinal("codigo_canal_venta");
                            int postCanalVenta = dr.GetOrdinal("canal_venta");
                            int postMonto = dr.GetOrdinal("monto");
                            int postCodigoTipoTraslado = dr.GetOrdinal("codigo_tipo_traslado");
                            int postTipoTraslado = dr.GetOrdinal("tipo_traslado");
                            int postCodigoEstado = dr.GetOrdinal("codigo_estado");
                            int postEstado = dr.GetOrdinal("estado");
                            int postUsuarioIng = dr.GetOrdinal("usuario_ing");
                            int postFechaIngStr = dr.GetOrdinal("fecha_ing_str");


                            while (dr.Read())
                            {
                                objTrasladoDetalle = new TrasladoLiquidacionDetalleCLS();
                                objTrasladoDetalle.CodigoTransaccion = dr.GetInt64(postCodigoTransaccion);
                                objTrasladoDetalle.CodigoTransaccionAnt = dr.GetInt64(postCodigoTransaccionAnt);
                                objTrasladoDetalle.CodigoTraslado = dr.GetInt64(postCodigoTraslado);
                                objTrasladoDetalle.CodigoEmpresa = (short)dr.GetInt32(postCodigoEmpresa);
                                objTrasladoDetalle.NombreEmpresa = dr.IsDBNull(postNombreEmpresa) ? "" : dr.GetString(postNombreEmpresa);
                                objTrasladoDetalle.FechaOperacion = dr.GetDateTime(postFechaOperacion);
                                objTrasladoDetalle.FechaOperacionStr = dr.GetString(postFechaOperacionStr);
                                objTrasladoDetalle.DiaOperacion = dr.GetByte(postDiaOperacion);
                                objTrasladoDetalle.NombreDia = dr.GetString(postNombreDia);
                                objTrasladoDetalle.CodigoVendedor = dr.GetString(postCodigoVendedor);
                                objTrasladoDetalle.NombreVendedor = dr.IsDBNull(postNombreVendedor) ? "" : dr.GetString(postNombreVendedor);
                                objTrasladoDetalle.Ruta = dr.GetInt16(postRuta);
                                objTrasladoDetalle.CodigoCanalVenta = dr.GetInt16(postCodigoCanalVenta);
                                objTrasladoDetalle.CanalVenta = dr.GetString(postCanalVenta);
                                objTrasladoDetalle.Monto = dr.GetDecimal(postMonto);
                                objTrasladoDetalle.CodigoTipoTraslado = dr.GetByte(postCodigoTipoTraslado);
                                objTrasladoDetalle.TipoTraslado = dr.GetString(postTipoTraslado);
                                objTrasladoDetalle.CodigoEstado = (byte)dr.GetInt32(postCodigoEstado);
                                objTrasladoDetalle.Estado = dr.GetString(postEstado);
                                objTrasladoDetalle.UsuarioIng = dr.GetString(postUsuarioIng);
                                objTrasladoDetalle.FechaIngStr = dr.GetString(postFechaIngStr);
                                lista.Add(objTrasladoDetalle);
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

        public List<TrasladoLiquidacionDetalleCLS> GetDetalleTrasladoLiquidacion(long codigoTraslado)
        {
            List<TrasladoLiquidacionDetalleCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sqlDetalleTraslados = @"
                    SELECT x.codigo_transaccion, 
                           CAST(COALESCE(codigo_transaccion_ant,0) AS BIGINT) AS codigo_transaccion_ant,
	                       codigo_traslado,
	                       COALESCE(x.codigo_empresa,0) AS codigo_empresa,
                           z.nombre_comercial AS nombre_empresa, 
                           x.fecha_operacion, 
                           FORMAT(x.fecha_operacion, 'dd/MM/yyyy') AS fecha_operacion_str,
                           x.dia_operacion,
                           CASE 
                             WHEN x.dia_operacion = 1 THEN 'LUNES'
                             WHEN x.dia_operacion = 2 THEN 'MARTES'
                             WHEN x.dia_operacion = 3 THEN 'MIERCOLES'
                             WHEN x.dia_operacion = 4 THEN 'JUEVES'
                             WHEN x.dia_operacion = 5 THEN 'VIERNES'
                             WHEN x.dia_operacion = 6 THEN 'SABADO'
                             WHEN x.dia_operacion = 7 THEN 'DOMINGO'
                             ELSE 'NO DEFINIDO'
                           END AS nombre_dia,
                           x.codigo_vendedor,
                           w.nombre_completo AS nombre_vendedor, 
	                       x.ruta,
                           x.codigo_canal_venta, 
                           y.nombre AS canal_venta, 
	                       x.monto,
	                       x.codigo_tipo_traslado,
                           CASE
                              WHEN x.codigo_tipo_traslado = 1 THEN 'NORMAL'
                              WHEN x.codigo_tipo_traslado = 2 THEN 'AJUSTE'
                              ELSE 'NO DEFINIDO'
                           END AS tipo_traslado,
	                       x.estado AS codigo_estado,
                           CASE 
                             WHEN x.estado  = 1 THEN 'ACTIVO'
                             WHEN x.estado  = 2 THEN 'BLOQUEADO'
                             ELSE 'NO DEFINIDO'  
                           END AS estado, 
	                       x.usuario_ing,
	                       FORMAT(x.fecha_ing, 'dd/MM/yyyy, hh:mm:ss') AS fecha_ing_str

                    FROM db_tesoreria.traslado_liquidacion_detalle x
                    INNER JOIN db_ventas.canal_venta y
                    ON x.codigo_canal_venta = y.codigo_canal_venta
                    LEFT JOIN db_admon.empresa z
                    ON x.codigo_empresa = z.codigo_empresa 
                    LEFT JOIN db_ventas.vendedor w
                    ON x.codigo_vendedor = w.codigo_vendedor
                    WHERE x.codigo_traslado =  @CodigoTraslado";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlDetalleTraslados, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoTraslado", codigoTraslado);
                        cmd.ExecuteNonQuery();

                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            TrasladoLiquidacionDetalleCLS objTrasladoDetalle;
                            lista = new List<TrasladoLiquidacionDetalleCLS>();
                            int postCodigoTransaccion = dr.GetOrdinal("codigo_transaccion");
                            int postCodigoTransaccionAnt = dr.GetOrdinal("codigo_transaccion_ant");
                            int postCodigoTraslado = dr.GetOrdinal("codigo_traslado");
                            int postCodigoEmpresa = dr.GetOrdinal("codigo_empresa");
                            int postNombreEmpresa = dr.GetOrdinal("nombre_empresa");
                            int postFechaOperacion = dr.GetOrdinal("fecha_operacion");
                            int postFechaOperacionStr = dr.GetOrdinal("fecha_operacion_str");
                            int postDiaOperacion = dr.GetOrdinal("dia_operacion");
                            int postNombreDia = dr.GetOrdinal("nombre_dia");
                            int postCodigoVendedor = dr.GetOrdinal("codigo_vendedor");
                            int postNombreVendedor = dr.GetOrdinal("nombre_vendedor");
                            int postRuta = dr.GetOrdinal("ruta");
                            int postCodigoCanalVenta = dr.GetOrdinal("codigo_canal_venta");
                            int postCanalVenta = dr.GetOrdinal("canal_venta");
                            int postMonto = dr.GetOrdinal("monto");
                            int postCodigoTipoTraslado = dr.GetOrdinal("codigo_tipo_traslado");
                            int postTipoTraslado = dr.GetOrdinal("tipo_traslado");
                            int postCodigoEstado = dr.GetOrdinal("codigo_estado");
                            int postEstado = dr.GetOrdinal("estado");
                            int postUsuarioIng = dr.GetOrdinal("usuario_ing");
                            int postFechaIngStr = dr.GetOrdinal("fecha_ing_str");
                            while (dr.Read())
                            {
                                objTrasladoDetalle = new TrasladoLiquidacionDetalleCLS();
                                objTrasladoDetalle.CodigoTransaccion = dr.GetInt64(postCodigoTransaccion);
                                objTrasladoDetalle.CodigoTransaccionAnt = dr.GetInt64(postCodigoTransaccionAnt);
                                objTrasladoDetalle.CodigoTraslado = dr.GetInt64(postCodigoTraslado);
                                objTrasladoDetalle.CodigoEmpresa = (short)dr.GetInt32(postCodigoEmpresa);
                                objTrasladoDetalle.NombreEmpresa = dr.IsDBNull(postNombreEmpresa) ? "" : dr.GetString(postNombreEmpresa);
                                objTrasladoDetalle.FechaOperacion = dr.GetDateTime(postFechaOperacion);
                                objTrasladoDetalle.FechaOperacionStr = dr.GetString(postFechaOperacionStr);
                                objTrasladoDetalle.DiaOperacion = dr.GetByte(postDiaOperacion);
                                objTrasladoDetalle.NombreDia = dr.GetString(postNombreDia);
                                objTrasladoDetalle.CodigoVendedor = dr.GetString(postCodigoVendedor);
                                objTrasladoDetalle.NombreVendedor = dr.IsDBNull(postNombreVendedor) ? "" : dr.GetString(postNombreVendedor);
                                objTrasladoDetalle.Ruta = dr.GetInt16(postRuta);
                                objTrasladoDetalle.CodigoCanalVenta = dr.GetInt16(postCodigoCanalVenta);
                                objTrasladoDetalle.CanalVenta = dr.GetString(postCanalVenta);
                                objTrasladoDetalle.Monto = dr.GetDecimal(postMonto);
                                objTrasladoDetalle.CodigoTipoTraslado = dr.GetByte(postCodigoTipoTraslado);
                                objTrasladoDetalle.TipoTraslado = dr.GetString(postTipoTraslado);
                                objTrasladoDetalle.CodigoEstado = dr.GetByte(postCodigoEstado);
                                objTrasladoDetalle.Estado = dr.GetString(postEstado);
                                objTrasladoDetalle.UsuarioIng = dr.GetString(postUsuarioIng);
                                objTrasladoDetalle.FechaIngStr = dr.GetString(postFechaIngStr);
                                lista.Add(objTrasladoDetalle);
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

        public List<TrasladoLiquidacionCLS> GetTrasladosLiquidacionConsulta(int anioOperacion)
        {
            List<TrasladoLiquidacionCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string filterAnioOperacion = String.Empty;
                    if (anioOperacion != -1) {
                        filterAnioOperacion = "AND x.anio_operacion = " + anioOperacion.ToString();
                    }
                    string sentenciaTraslados = @"
                    SELECT CAST(0 AS BIGINT) AS codigo_traslado, 
                           x.anio_operacion, 
                           x.semana_operacion, 
                           db_admon.GetPeriodoSemana(x.anio_operacion, x.semana_operacion)  AS periodo,
                           count(*) AS cantidad, 
                           SUM(x.monto) AS monto_total,
                           1 AS codigo_estado,
                           'POR GENERAR'  AS estado,
                           1 AS bloqueado 
                    FROM db_tesoreria.transaccion x
                    WHERE x.codigo_estado <> @CodigoEstadoTransaccion
                      AND x.liquidacion = 0
                      AND x.complemento_conta = 0
                      AND x.codigo_categoria_entidad <> @CodigoCategoriaEntidad
                      AND x.codigo_operacion = @CodigoOperacion
                      " + filterAnioOperacion + @"  
                    GROUP BY x.anio_operacion, x.semana_operacion

                    UNION

                    SELECT x.codigo_traslado, 
                           x.anio_operacion, 
                           x.semana_operacion, 
                           db_admon.GetPeriodoSemana(x.anio_operacion,x.semana_operacion)  AS periodo,
                           (SELECT COUNT(*) FROM db_tesoreria.traslado_liquidacion_detalle WHERE codigo_traslado = x.codigo_traslado) AS cantidad,
                           (SELECT SUM(monto) FROM db_tesoreria.traslado_liquidacion_detalle WHERE codigo_traslado = x.codigo_traslado) AS monto_total,
                           x.codigo_estado,
                           y.nombre AS estado,
                           0 AS bloqueado 
                    FROM db_tesoreria.traslado_liquidacion x
                    INNER JOIN db_tesoreria.estado_traslado_liquidacion y
                    ON x.codigo_estado = y.codigo_estado_traslado
                    WHERE x.codigo_estado <> @CodigoEstadoTrasladoAnulado
                    " + filterAnioOperacion;

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sentenciaTraslados, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoTransaccion", Constantes.EstadoTransacccion.ANULADO);
                        cmd.Parameters.AddWithValue("@CodigoCategoriaEntidad", Constantes.Entidad.Categoria.CLIENTES_ESPECIALES_2);
                        cmd.Parameters.AddWithValue("@CodigoOperacion", Constantes.Operacion.Ingreso.VENTAS_EN_RUTA);
                        cmd.Parameters.AddWithValue("@CodigoEstadoTrasladoAnulado", Constantes.Liquidacion.EstadoTraslado.ANULADO);
                        cmd.ExecuteNonQuery();

                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            TrasladoLiquidacionCLS objTraslado;
                            lista = new List<TrasladoLiquidacionCLS>();
                            int postCodigoTraslado = dr.GetOrdinal("codigo_traslado");
                            int postAnioOperacion = dr.GetOrdinal("anio_operacion");
                            int postSemanaOperacion = dr.GetOrdinal("semana_operacion");
                            int postPeriodo = dr.GetOrdinal("periodo");
                            int postCantidad = dr.GetOrdinal("cantidad");
                            int postMontoTotal = dr.GetOrdinal("monto_total");
                            int postCodigoEstado = dr.GetOrdinal("codigo_estado");
                            int postEstado = dr.GetOrdinal("estado");
                            int postBloqueado = dr.GetOrdinal("bloqueado");

                            while (dr.Read())
                            {
                                objTraslado = new TrasladoLiquidacionCLS();
                                objTraslado.CodigoTraslado = dr.GetInt64(postCodigoTraslado);
                                objTraslado.AnioOperacion = dr.GetInt16(postAnioOperacion);
                                objTraslado.SemanaOperacion = dr.GetByte(postSemanaOperacion);
                                objTraslado.Periodo = dr.GetString(postPeriodo);
                                objTraslado.Cantidad = dr.GetInt32(postCantidad);
                                objTraslado.MontoTotal = dr.GetDecimal(postMontoTotal);
                                objTraslado.CodigoEstadoTraslado = (byte)dr.GetInt32(postCodigoEstado);
                                objTraslado.EstadoTraslado = dr.GetString(postEstado);
                                objTraslado.bloqueado = (byte)dr.GetInt32(postBloqueado);
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
        public string GenerarTraslado(int anioOperacion, int semanaOperacion, string usuarioIng)
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
                    string codigoSeguridad = Util.Seguridad.GenerarCadena();
                    int anio = DateTime.Now.Year;
                    string sqlSequence = "SELECT sig_valor FROM db_admon.secuencia_detalle WHERE codigo_secuencia = @CodigoSecuencia AND anio = @AnioTraslado";

                    // ExecuteScalar(), Executes the query, and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.
                    cmd.CommandText = sqlSequence;
                    cmd.Parameters.AddWithValue("@CodigoSecuencia", Constantes.Secuencia.SIT_SEQ_TRASLADO_LIQUIDACION);
                    cmd.Parameters.AddWithValue("@AnioTraslado", anio);
                    long correlativoTraslado = (long)cmd.ExecuteScalar();

                    string sentenciaUpdateSequenciaDetalle = "UPDATE db_admon.secuencia_detalle SET sig_valor = @siguienteValor WHERE codigo_secuencia = @CodigoSecuencia  AND anio = @AnioTraslado";
                    cmd.CommandText = sentenciaUpdateSequenciaDetalle;
                    cmd.Parameters.AddWithValue("@siguienteValor", correlativoTraslado + 1);
                    cmd.Parameters["@CodigoSecuencia"].Value = Constantes.Secuencia.SIT_SEQ_TRASLADO_LIQUIDACION;
                    cmd.Parameters["@AnioTraslado"].Value = anio;
                    cmd.ExecuteScalar();

                    long codigoTraslado = long.Parse(anio.ToString() + correlativoTraslado.ToString("D5"));
                    string sentenciaInsertTraslado = @"
                    INSERT INTO db_tesoreria.traslado_liquidacion(codigo_traslado,anio_operacion,semana_operacion,fecha_generacion,usuario_generacion,fecha_traslado,usuario_traslado,observaciones,codigo_estado,usuario_ing,fecha_ing,usuario_act,fecha_act)
                    VALUES( @CodigoTraslado,
                            @AnioOperacion,
                            @SemanaOperacion,
                            @FechaGeneracion,
                            @UsuarioGeneracion,
                            @FechaTraslado,
                            @UsuarioTraslado,
                            @Observaciones,
                            @CodigoEstado,
                            @UsuarioIng,
                            @FechaIng,
                            @UsuarioAct,
                            @FechaAct)";

                    cmd.CommandText = sentenciaInsertTraslado;
                    cmd.Parameters.AddWithValue("@CodigoTraslado", codigoTraslado);
                    cmd.Parameters.AddWithValue("@AnioOperacion", anioOperacion);
                    cmd.Parameters.AddWithValue("@SemanaOperacion", semanaOperacion);
                    cmd.Parameters.AddWithValue("@FechaGeneracion", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UsuarioGeneracion", usuarioIng);
                    cmd.Parameters.AddWithValue("@FechaTraslado", DBNull.Value);
                    cmd.Parameters.AddWithValue("@UsuarioTraslado", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Observaciones", DBNull.Value);
                    cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.Liquidacion.EstadoTraslado.GENERADO);
                    cmd.Parameters.AddWithValue("@UsuarioIng", usuarioIng);
                    cmd.Parameters.AddWithValue("@FechaIng", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UsuarioAct", DBNull.Value);
                    cmd.Parameters.AddWithValue("@FechaAct", DBNull.Value);
                    cmd.ExecuteNonQuery();


                    string sentenciaInsertDetalleTraslado = @"
                    INSERT INTO db_tesoreria.traslado_liquidacion_detalle(codigo_transaccion, codigo_transaccion_ant, codigo_traslado, codigo_empresa, fecha_operacion, dia_operacion, codigo_vendedor, ruta, codigo_canal_venta, monto, codigo_tipo_traslado, estado, usuario_ing, fecha_ing, usuario_act, fecha_act)
                    SELECT codigo_transaccion, 
	                       NULL AS codigo_transaccion_ant,
	                       @CodigoTraslado AS codigo_traslado,
	                       NULL AS codigo_empresa,
                           fecha_operacion,
                           dia_operacion, 
                           codigo_entidad, 
	                       ruta,
                           codigo_canal_venta,     
	                       monto,
	                       codigo_tipo_traslado,
	                       @CodigoEstadoDetalle AS estado,
	                       @UsuarioIng AS usuario_ing,
	                       GETDATE() AS fecha_ing,
	                       NULL AS usuario_act,
	                       NULL AS fecha_act

                    FROM db_tesoreria.transaccion
                    WHERE codigo_estado <> @CodigoEstadoTransaccion
                      AND liquidacion = 0
                      AND complemento_conta = 0
                      AND codigo_categoria_entidad <> @CodigoCategoriaEntidad
                      AND codigo_operacion = @CodigoOperacion
                      AND anio_operacion = @AnioOperacion
                      AND semana_operacion = @SemanaOperacion";
                    cmd.CommandText = sentenciaInsertDetalleTraslado;
                    cmd.Parameters["@CodigoTraslado"].Value = codigoTraslado;
                    cmd.Parameters.AddWithValue("@CodigoEstadoTransaccion", Constantes.EstadoTransacccion.ANULADO);
                    cmd.Parameters.AddWithValue("@CodigoCategoriaEntidad", Constantes.Entidad.Categoria.CLIENTES_ESPECIALES_2);
                    cmd.Parameters.AddWithValue("@CodigoOperacion", Constantes.Operacion.Ingreso.VENTAS_EN_RUTA);
                    cmd.Parameters.AddWithValue("@CodigoEstadoDetalle", Constantes.EstadoRegistro.ACTIVO);
                    cmd.Parameters["@AnioOperacion"].Value = anioOperacion;
                    cmd.Parameters["@SemanaOperacion"].Value = semanaOperacion;
                    cmd.Parameters["@UsuarioIng"].Value = usuarioIng;
                    cmd.ExecuteNonQuery();

                    string sentenciaUpdateTransaccion = @"
                    UPDATE db_tesoreria.transaccion 
                    SET liquidacion = 1
                    WHERE codigo_estado <> @CodigoEstadoTransaccion
                      AND liquidacion = 0
                      AND complemento_conta = 0
                      AND codigo_categoria_entidad <> @CodigoCategoriaEntidad
                      AND codigo_operacion = @CodigoOperacion
                      AND anio_operacion = @AnioOperacion
                      AND semana_operacion = @SemanaOperacion";

                    cmd.CommandText = sentenciaUpdateTransaccion;
                    cmd.Parameters["@CodigoEstadoTransaccion"].Value = Constantes.EstadoTransacccion.ANULADO;
                    cmd.Parameters["@CodigoCategoriaEntidad"].Value = Constantes.Entidad.Categoria.CLIENTES_ESPECIALES_2;
                    cmd.Parameters["@CodigoOperacion"].Value = Constantes.Operacion.Ingreso.VENTAS_EN_RUTA;
                    cmd.Parameters["@AnioOperacion"].Value = anioOperacion;
                    cmd.Parameters["@SemanaOperacion"].Value = semanaOperacion;
                    cmd.ExecuteNonQuery();

                    // Attempt to commit the transaction.
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

        public string AnularTraslado(long codigoTraslado, string usuarioAct)
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
                    string sentenciaUpdateTransaccion = @"
                    UPDATE db_tesoreria.transaccion 
                    SET liquidacion = 0
                    WHERE codigo_transaccion IN (SELECT codigo_transaccion FROM db_tesoreria.traslado_liquidacion_detalle WHERE codigo_traslado = @CodigoTraslado) ";

                    cmd.CommandText = sentenciaUpdateTransaccion;
                    cmd.Parameters.AddWithValue("@CodigoTraslado", codigoTraslado);
                    int rowTransaccion = cmd.ExecuteNonQuery();


                    string sentenciaDeleteDetalleTraslado = @"
                    DELETE FROM db_tesoreria.traslado_liquidacion_detalle
                    WHERE codigo_traslado = @CodigoTraslado ";

                    cmd.CommandText = sentenciaDeleteDetalleTraslado;
                    cmd.Parameters["@CodigoTraslado"].Value = codigoTraslado;
                    int rowDetalleTraslado = cmd.ExecuteNonQuery();


                    string sentenciaUpdateTraslado = @"
                    UPDATE db_tesoreria.traslado_liquidacion
                    SET codigo_estado = @CodigoEstadoTraslado,
                        usuario_act = @UsuarioAct,
                        fecha_act = @FechaAct
                    WHERE codigo_traslado = @CodigoTraslado ";

                    cmd.CommandText = sentenciaUpdateTraslado;
                    cmd.Parameters.AddWithValue("@CodigoEstadoTraslado", Constantes.Liquidacion.EstadoTraslado.ANULADO);
                    cmd.Parameters.AddWithValue("@UsuarioAct", usuarioAct);
                    cmd.Parameters.AddWithValue("@FechaAct", DateTime.Now);
                    cmd.Parameters["@CodigoTraslado"].Value = codigoTraslado;
                    int rowTraslado = cmd.ExecuteNonQuery();

                    if (rowTransaccion > 0 && rowDetalleTraslado > 0 && rowTraslado > 0)
                    {
                        // Attempt to commit the transaction.
                        transaction.Commit();
                        conexion.Close();

                        resultado = "OK";
                    }
                    else {
                        transaction.Rollback();
                        conexion.Close();
                        resultado = "Error [0]: Error en la actualización";
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

        public string TrasladarParaLiquidacion(long codigoTraslado, string usuarioAct)
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
                    string sentenciaUpdateTraslado = @"
                    UPDATE db_tesoreria.traslado_liquidacion
                    SET codigo_estado = @CodigoEstadoTraslado,
                        usuario_act = @UsuarioAct,
                        fecha_act = @FechaAct
                    WHERE codigo_traslado = @CodigoTraslado";

                    cmd.CommandText = sentenciaUpdateTraslado;
                    cmd.Parameters.AddWithValue("@CodigoEstadoTraslado", Constantes.Liquidacion.EstadoTraslado.TRASLADADO);
                    cmd.Parameters.AddWithValue("@UsuarioAct", usuarioAct);
                    cmd.Parameters.AddWithValue("@FechaAct", DateTime.Now);
                    cmd.Parameters.AddWithValue("@CodigoTraslado", codigoTraslado);
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

        public ReporteTrasladoLiquidacionCLS GetReporteTrasladoLiquidacion(long codigoTraslado, int anioOperacion, int semanaOperacion)
        {
            int postFechaGeneracionStr = 0;
            int postUsuarioGeneracion = 0;
            int postCodigoVendedor = 0;
            int postNombreVendedor = 0;
            int postRuta = 0;
            int postCodigoCanalVenta = 0;
            int postCanalVenta = 0;
            int postMontoLunes = 0;
            int postMontoMartes = 0;
            int postMontoMiercoles = 0;
            int postMontoJueves = 0;
            int postMontoViernes = 0;
            int postMontoSabado = 0;
            int postMontoDomingo = 0;
            int postMontoTotal = 0;


            ReporteTrasladoLiquidacionCLS objReporte = new ReporteTrasladoLiquidacionCLS();
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_tesoreria.uspGetReporteTrasladoLiquidacion", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CodigoTraslado", codigoTraslado);
                        cmd.Parameters.AddWithValue("@AnioOperacion", anioOperacion);
                        cmd.Parameters.AddWithValue("@SemanaOperacion", semanaOperacion);
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr != null)
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
                        {// Encabezado del traslado de liquidación
                            postFechaGeneracionStr = dr.GetOrdinal("fecha_generacion_str");
                            postUsuarioGeneracion = dr.GetOrdinal("usuario_generacion");

                            TrasladoLiquidacionCLS objEncabezadoLiquidacion;
                            while (dr.Read())
                            {
                                objEncabezadoLiquidacion = new TrasladoLiquidacionCLS();
                                objEncabezadoLiquidacion.FechaGeneracionStr = dr.GetString(postFechaGeneracionStr);
                                objEncabezadoLiquidacion.UsuarioGeneracion = dr.GetString(postFechaGeneracionStr);
                                objReporte.encabezado = objEncabezadoLiquidacion;

                            }// fin while
                        }// fin if

                        if (dr.NextResult())
                        {// Detalle de pagos de vendedores
                            postCodigoVendedor = dr.GetOrdinal("codigo_vendedor");
                            postNombreVendedor = dr.GetOrdinal("nombre_vendedor");
                            postRuta = dr.GetOrdinal("ruta");
                            postCodigoCanalVenta = dr.GetOrdinal("codigo_canal_venta");
                            postCanalVenta = dr.GetOrdinal("canal_venta");
                            postMontoLunes = dr.GetOrdinal("monto_lunes");
                            postMontoMartes = dr.GetOrdinal("monto_martes");
                            postMontoMiercoles = dr.GetOrdinal("monto_miercoles");
                            postMontoJueves = dr.GetOrdinal("monto_jueves");
                            postMontoViernes = dr.GetOrdinal("monto_viernes");
                            postMontoSabado = dr.GetOrdinal("monto_sabado");
                            postMontoDomingo = dr.GetOrdinal("monto_domingo");
                            postMontoTotal = dr.GetOrdinal("monto_total");

                            List<TrasladoLiquidacionDetalleReporteCLS> listaDetalle = new List<TrasladoLiquidacionDetalleReporteCLS>();
                            TrasladoLiquidacionDetalleReporteCLS objDetalleReporte;
                            while (dr.Read())
                            {
                                objDetalleReporte = new TrasladoLiquidacionDetalleReporteCLS();
                                objDetalleReporte.CodigoVendedor = dr.GetString(postCodigoVendedor);
                                objDetalleReporte.NombreVendedor = dr.IsDBNull(postNombreVendedor) ? "" : dr.GetString(postNombreVendedor);
                                objDetalleReporte.Ruta = dr.GetInt16(postRuta);
                                objDetalleReporte.CodigoCanalVenta = dr.GetInt16(postCodigoCanalVenta);
                                objDetalleReporte.CanalVenta = dr.IsDBNull(postCanalVenta) ? "" : dr.GetString(postCanalVenta);
                                objDetalleReporte.MontoLunes = dr.GetDecimal(postMontoLunes);
                                objDetalleReporte.MontoMartes = dr.GetDecimal(postMontoMartes);
                                objDetalleReporte.MontoMiercoles = dr.GetDecimal(postMontoMiercoles);
                                objDetalleReporte.MontoJueves = dr.GetDecimal(postMontoJueves);
                                objDetalleReporte.MontoViernes = dr.GetDecimal(postMontoViernes);
                                objDetalleReporte.MontoSabado = dr.GetDecimal(postMontoSabado);
                                objDetalleReporte.MontoDomingo = dr.GetDecimal(postMontoDomingo);
                                objDetalleReporte.MontoTotal = dr.GetDecimal(postMontoTotal);
                                listaDetalle.Add(objDetalleReporte);
                            }// fin while
                            objReporte.detalle = listaDetalle;

                        }// fin if
                    }
                    
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                    objReporte = null;
                }

                return objReporte;
            }
        }

    }
}
