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
    public class PagoDescuentoDAL: CadenaConexion
    {
        public List<SaldoPrestamoCLS> GetEmpleadosCuentasPorCobrarPlanilla()
        {
            List<SaldoPrestamoCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaContabilidad))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_contabilidad.uspGetCuentasPorCobrarPlanilla", conexion))
                    {
                        // CommandType, Gets or sets a value indicating how the CommandText property is to be interpreted.
                        // The Text CommandType.Text is used when the command is raw SQL
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            SaldoPrestamoCLS objSaldoPrestamoCLS;
                            lista = new List<SaldoPrestamoCLS>();
                            int postCodigoEmpresa = dr.GetOrdinal("codigo_empresa");
                            int postNombreEmpresa = dr.GetOrdinal("nombre_empresa");
                            int postCodigoEmpleado = dr.GetOrdinal("codigo_empleado");
                            int postNombreCompleto = dr.GetOrdinal("nombre_completo");
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postCodigoOperacionDescuento = dr.GetOrdinal("codigo_operacion_descuento");
                            int postOperacion = dr.GetOrdinal("operacion");
                            int postCodigoFrecuenciaPago = dr.GetOrdinal("codigo_frecuencia_pago");
                            int postFrecuenciaPago = dr.GetOrdinal("frecuencia_pago");
                            int postSaldoPendiente = dr.GetOrdinal("saldo_pendiente");
                            int postMontoDescuento = dr.GetOrdinal("monto_descuento_prestamo");
                            while (dr.Read())
                            {
                                objSaldoPrestamoCLS = new SaldoPrestamoCLS();
                                objSaldoPrestamoCLS.CodigoEmpresa = (short)dr.GetInt32(postCodigoEmpresa);
                                objSaldoPrestamoCLS.NombreEmpresa = dr.GetString(postNombreEmpresa);
                                objSaldoPrestamoCLS.CodigoEmpleado = dr.GetString(postCodigoEmpleado);
                                objSaldoPrestamoCLS.NombreCompleto = dr.GetString(postNombreCompleto);
                                objSaldoPrestamoCLS.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objSaldoPrestamoCLS.CodigoOperacionDescuento = (short)dr.GetInt32(postCodigoOperacionDescuento);
                                objSaldoPrestamoCLS.Operacion = dr.GetString(postOperacion);
                                objSaldoPrestamoCLS.CodigoFrecuenciaPago = dr.GetByte(postCodigoFrecuenciaPago);
                                objSaldoPrestamoCLS.FrecuenciaPago = dr.GetString(postFrecuenciaPago);
                                objSaldoPrestamoCLS.SaldoPendiente = dr.GetDecimal(postSaldoPendiente);
                                objSaldoPrestamoCLS.MontoDescuento = dr.GetDecimal(postMontoDescuento);
                                lista.Add(objSaldoPrestamoCLS);
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

        public string GuardarDescuentoDevolucion(int codigoEmpresa, string codigoEmpleado, int codigoOperacion, decimal monto, string usuarioIng)
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
                    string sentenciaInsertPagos = String.Empty;
                    string sqlSequence = "SELECT NEXT VALUE FOR db_contabilidad.SQ_PAGO_DESCUENTO";
                    cmd.CommandText = sqlSequence;
                    long codigoPagoDescuento = (long)cmd.ExecuteScalar();

                    string sentenciaInsertPago = @"
                    INSERT INTO db_contabilidad.pagos_y_descuentos(codigo_pago,codigo_tipo_planilla,codigo_empresa,codigo_empleado,codigo_frecuencia_pago,codigo_operacion,anio,mes,codigo_quincena,numero_semana,monto,codigo_estado,usuario_ing,fecha_ing)
                    VALUES(@CodigoPago, @CodigoTipoPlanilla, @CodigoEmpresa, @CodigoEmpleado, @CodigoFrecuenciaPago, @CodigoOperacion, @Anio, @Mes, @CodigoQuincena, @NumeroSemana, @Monto, @CodigoEstado, @UsuarioIng, @FechaIng)";

                    cmd.CommandText = sentenciaInsertPago;
                    cmd.Parameters.AddWithValue("@CodigoPago", codigoPagoDescuento);
                    cmd.Parameters.AddWithValue("@CodigoTipoPlanilla", 0);
                    cmd.Parameters.AddWithValue("@CodigoEmpresa", codigoEmpresa);
                    cmd.Parameters.AddWithValue("@CodigoEmpleado", codigoEmpleado);
                    cmd.Parameters.AddWithValue("@CodigoFrecuenciaPago", 0);
                    cmd.Parameters.AddWithValue("@CodigoOperacion", codigoOperacion);
                    cmd.Parameters.AddWithValue("@Anio", 0);
                    cmd.Parameters.AddWithValue("@NumeroSemana", 0);
                    cmd.Parameters.AddWithValue("@CodigoQuincena", 0);
                    cmd.Parameters.AddWithValue("@Mes", 0);
                    cmd.Parameters.AddWithValue("@Monto", monto);
                    cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.Planilla.EstadoPagoDescuento.COBRADO);
                    cmd.Parameters.AddWithValue("@UsuarioIng", usuarioIng);
                    cmd.Parameters.AddWithValue("@FechaIng", DateTime.Now);
                    cmd.ExecuteNonQuery();

                    string sentenciaInsertCxC = @"
                    INSERT INTO db_contabilidad.cuenta_por_cobrar
                    SELECT NEXT VALUE FOR db_contabilidad.SQ_CUENTA_POR_COBRAR AS codigo_cxc, 
		                   @CodigoTipoCuentaPorCobrar AS codigo_tipo_cxc,
		                   @CodigoCategoriaEntidad AS codigo_categoria_entidad,
		                   @CodigoCategoriaEntidad AS codigo_categoria,
		                   x.codigo_empleado AS codigo_entidad,
		                   db_rrhh.GetNombreCompletoEmpleado(y.cui) AS nombre_entidad,
		                   @FechaPrestamo AS fecha_prestamo,
 		                   @FechaInicioPago AS fecha_inicio_pago,
		                   0 AS anio_operacion,
		                   0 AS semana_operacion,
		                   x.monto,
                           NULL AS observaciones, 
		                   @CodigoTransaccion AS codigo_transaccion,
		                   @CodigoPlanilla AS codigo_planilla,
		                   x.codigo_operacion,
		                   @CodigoEstado AS codigo_estado,
		                   @UsuarioIng AS usuario_ing,
		                   @FechaIng AS fecha_ing,
                           NULL AS usuario_act,
                           NULL AS fecha_act,
                           @CargaInicial AS carga_inicial,
                           x.codigo_pago,
                           0 AS codigo_reporte 
                    FROM db_contabilidad.pagos_y_descuentos x
                    INNER JOIN db_rrhh.empleado y
                    ON x.codigo_empresa = y.codigo_empresa AND x.codigo_empleado = y.codigo_empleado
                    WHERE x.codigo_pago = @CodigoPago";

                    cmd.CommandText = sentenciaInsertCxC;
                    cmd.Parameters["@CodigoPago"].Value = codigoPagoDescuento;
                    cmd.Parameters.AddWithValue("@CodigoTipoCuentaPorCobrar", Constantes.CuentaPorCobrar.Tipo.NO_APLICA);
                    cmd.Parameters.AddWithValue("@CodigoCategoriaEntidad", Constantes.CuentaPorCobrar.Categoria.EMPLEADO);
                    cmd.Parameters.AddWithValue("@FechaPrestamo", DBNull.Value);
                    cmd.Parameters.AddWithValue("@FechaInicioPago", DBNull.Value);
                    cmd.Parameters.AddWithValue("@CodigoTransaccion", DBNull.Value);
                    cmd.Parameters.AddWithValue("@CodigoPlanilla", DBNull.Value);
                    cmd.Parameters.AddWithValue("@AnioOperacion", 0);
                    cmd.Parameters.AddWithValue("@SemanaOperacion", 0);
                    cmd.Parameters["@CodigoEstado"].Value = Constantes.CuentaPorCobrar.Estado.PARA_INCLUIR_EN_REPORTE;
                    cmd.Parameters["@UsuarioIng"].Value = usuarioIng;
                    cmd.Parameters["@FechaIng"].Value = DateTime.Now;
                    cmd.Parameters.AddWithValue("@CargaInicial", 0);
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

        public List<PagoDescuentoCLS> GetPagosDescuentos()
        {
            List<PagoDescuentoCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaContabilidad))
            {
                try
                {
                    string sql = @"
                    SELECT x.codigo_pago,
	                       x.codigo_tipo_planilla,
	                       y.nombre AS tipo_planilla,
	                       x.codigo_empresa,
	                       z.nombre_comercial AS nombre_empresa,
	                       x.codigo_empleado,
                           m.nombre_completo,
	                       x.codigo_frecuencia_pago,
	                       a.nombre AS frecuencia_pago,
                           x.codigo_operacion, 
                           b.nombre_operacion AS operacion,
	                       x.anio,
	                       x.mes,
                           db_admon.GetPeriodo(x.codigo_frecuencia_pago,x.anio,x.codigo_quincena, x.numero_semana) AS periodo,
	                       CASE
	                         WHEN x.mes = 1 THEN 'ENERO'
		                     WHEN x.mes = 2 THEN 'FEBRERO'
		                     WHEN x.mes = 3 THEN 'MARZO'
		                     WHEN x.mes = 4 THEN 'ABRIL'
		                     WHEN x.mes = 5 THEN 'MAYO'
		                     WHEN x.mes = 6 THEN 'JUNIO'
		                     WHEN x.mes = 7 THEN 'JULIO'
		                     WHEN x.mes = 8 THEN 'AGOSTO'
		                     WHEN x.mes = 9 THEN 'SEPTIEMBRE'
		                     WHEN x.mes = 10 THEN 'OCTUBRE'
		                     WHEN x.mes = 11 THEN 'NOVIEMBRE'
		                     WHEN x.mes = 12 THEN 'DICIEMBRE'
		                   ELSE 'NO DEFINIDO' 
	                       END AS nombre_mes,
	                       x.monto, 
	                       x.codigo_estado,
                           1 AS permiso_anular

                    FROM db_contabilidad.pagos_y_descuentos x
                    INNER JOIN db_contabilidad.tipo_planilla y
                    ON x.codigo_tipo_planilla = y.codigo_tipo_planilla
                    INNER JOIN db_admon.empresa z
                    ON x.codigo_empresa = z.codigo_empresa
                    INNER JOIN db_rrhh.empleado w
                    ON x.codigo_empleado = w.codigo_empleado
                    INNER JOIN db_rrhh.persona m
                    ON w.cui = m.cui
                    INNER JOIN db_rrhh.frecuencia_pago a
                    ON x.codigo_frecuencia_pago = a.codigo_frecuencia_pago
                    INNER JOIN db_tesoreria.operacion b
                    ON x.codigo_operacion = b.codigo_operacion
                    WHERE x.codigo_estado = @codigoEstadoActivo
                    ORDER BY x.codigo_empresa, m.nombre_completo, y.nombre";
                    

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        // CommandType, Gets or sets a value indicating how the CommandText property is to be interpreted.
                        // The Text CommandType.Text is used when the command is raw SQL
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@codigoEstadoActivo", Constantes.Planilla.EstadoPagoDescuento.ACTIVO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            PagoDescuentoCLS objPagoDescuentoCLS;
                            lista = new List<PagoDescuentoCLS>();
                            int postCodigoPago = dr.GetOrdinal("codigo_pago");
                            int postCodigoTipoPlanilla = dr.GetOrdinal("codigo_tipo_planilla");
                            int postTipoPlanilla = dr.GetOrdinal("tipo_planilla");
                            int postCodigoEmpresa = dr.GetOrdinal("codigo_empresa");
                            int postNombreEmpresa = dr.GetOrdinal("nombre_empresa");
                            int postCodigoEmpleado = dr.GetOrdinal("codigo_empleado");
                            int postNombreCompleto = dr.GetOrdinal("nombre_completo");
                            int postCodigoFrecuenciaPago = dr.GetOrdinal("codigo_frecuencia_pago");
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postOperacion = dr.GetOrdinal("operacion");
                            int postFrecuenciaPago = dr.GetOrdinal("frecuencia_pago");
                            int postAnio = dr.GetOrdinal("anio");
                            int postMes = dr.GetOrdinal("mes");
                            int postNombreMes = dr.GetOrdinal("nombre_mes");
                            int postPeriodo = dr.GetOrdinal("periodo");
                            int postMonto = dr.GetOrdinal("monto");
                            int postPermisoAnular = dr.GetOrdinal("permiso_anular");
                            while (dr.Read())
                            {
                                objPagoDescuentoCLS = new PagoDescuentoCLS();
                                objPagoDescuentoCLS.CodigoPago = dr.GetInt32(postCodigoPago);
                                objPagoDescuentoCLS.CodigoTipoPlanilla = dr.GetInt16(postCodigoTipoPlanilla);
                                objPagoDescuentoCLS.TipoPlanilla = dr.GetString(postTipoPlanilla);
                                objPagoDescuentoCLS.CodigoEmpresa = dr.GetInt16(postCodigoEmpresa);
                                objPagoDescuentoCLS.NombreEmpresa = dr.GetString(postNombreEmpresa);
                                objPagoDescuentoCLS.CodigoEmpleado = dr.GetString(postCodigoEmpleado);
                                objPagoDescuentoCLS.NombreCompleto = dr.GetString(postNombreCompleto);
                                objPagoDescuentoCLS.CodigoFrecuenciaPago = dr.GetByte(postCodigoFrecuenciaPago);
                                objPagoDescuentoCLS.FrecuenciaPago = dr.GetString(postFrecuenciaPago);
                                objPagoDescuentoCLS.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objPagoDescuentoCLS.Operacion = dr.GetString(postOperacion);
                                objPagoDescuentoCLS.Anio = dr.GetInt16(postAnio);
                                objPagoDescuentoCLS.Mes = dr.GetByte(postMes);
                                objPagoDescuentoCLS.NombreMes = dr.GetString(postNombreMes);
                                objPagoDescuentoCLS.Periodo = dr.GetString(postPeriodo);
                                objPagoDescuentoCLS.Monto = dr.GetDecimal(postMonto);
                                objPagoDescuentoCLS.PermisoAnular = (byte)dr.GetInt32(postPermisoAnular);
                                lista.Add(objPagoDescuentoCLS);
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

        public List<PagoDescuentoCLS> GetPagosDescuentosConsulta(int anio, int mes, int codigoEmpresa)
        {
            List<PagoDescuentoCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaContabilidad))
            {
                try
                {
                    string filterMes = string.Empty;
                    string filterEmpresa = string.Empty;
                    if (mes != -1) {
                        filterMes = " AND x.mes = " + mes.ToString();
                    }

                    if (codigoEmpresa != -1) {
                        filterEmpresa = " AND x.codigo_empresa = " + codigoEmpresa.ToString();
                    }

                    string sql = @"
                    SELECT x.codigo_pago,
	                       x.codigo_tipo_planilla,
	                       y.nombre AS tipo_planilla,
	                       x.codigo_empresa,
	                       z.nombre_comercial AS nombre_empresa,
	                       x.codigo_empleado,
                           m.nombre_completo,
	                       x.codigo_frecuencia_pago,
	                       a.nombre AS frecuencia_pago,
                           x.codigo_operacion, 
                           b.nombre_operacion AS operacion,
	                       x.anio,
	                       x.mes,
	                       CASE
	                         WHEN x.mes = 1 THEN 'ENERO'
		                     WHEN x.mes = 2 THEN 'FEBRERO'
		                     WHEN x.mes = 3 THEN 'MARZO'
		                     WHEN x.mes = 4 THEN 'ABRIL'
		                     WHEN x.mes = 5 THEN 'MAYO'
		                     WHEN x.mes = 6 THEN 'JUNIO'
		                     WHEN x.mes = 7 THEN 'JULIO'
		                     WHEN x.mes = 8 THEN 'AGOSTO'
		                     WHEN x.mes = 9 THEN 'SEPTIEMBRE'
		                     WHEN x.mes = 10 THEN 'OCTUBRE'
		                     WHEN x.mes = 11 THEN 'NOVIEMBRE'
		                     WHEN x.mes = 12 THEN 'DICIEMBRE'
		                   ELSE 'NO DEFINIDO' 
	                       END AS nombre_mes,
	                       x.monto, 
	                       x.codigo_estado

                    FROM db_contabilidad.pagos_y_descuentos x
                    INNER JOIN db_contabilidad.tipo_planilla y
                    ON x.codigo_tipo_planilla = y.codigo_tipo_planilla
                    INNER JOIN db_admon.empresa z
                    ON x.codigo_empresa = z.codigo_empresa
                    INNER JOIN db_rrhh.empleado w
                    ON x.codigo_empleado = w.codigo_empleado
                    INNER JOIN db_rrhh.persona m
                    ON w.cui = m.cui
                    INNER JOIN db_rrhh.frecuencia_pago a
                    ON x.codigo_frecuencia_pago = a.codigo_frecuencia_pago
                    INNER JOIN db_tesoreria.operacion b
                    ON x.codigo_operacion = b.codigo_operacion
                    WHERE x.codigo_estado = @CodigoEstadoPorCobrar AND 
                          x.anio = @Anio  
                          " + filterEmpresa + @"
                          " + filterMes + @"
                    ORDER BY x.codigo_empresa, m.nombre_completo, y.nombre";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        // CommandType, Gets or sets a value indicating how the CommandText property is to be interpreted.
                        // The Text CommandType.Text is used when the command is raw SQL
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoPorCobrar", Constantes.Planilla.EstadoPagoDescuento.COBRADO);
                        cmd.Parameters.AddWithValue("@Anio", anio);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            PagoDescuentoCLS objPagoDescuentoCLS;
                            lista = new List<PagoDescuentoCLS>();
                            int postCodigoPago = dr.GetOrdinal("codigo_pago");
                            int postCodigoTipoPlanilla = dr.GetOrdinal("codigo_tipo_planilla");
                            int postTipoPlanilla = dr.GetOrdinal("tipo_planilla");
                            int postCodigoEmpresa = dr.GetOrdinal("codigo_empresa");
                            int postNombreEmpresa = dr.GetOrdinal("nombre_empresa");
                            int postCodigoEmpleado = dr.GetOrdinal("codigo_empleado");
                            int postNombreCompleto = dr.GetOrdinal("nombre_completo");
                            int postCodigoFrecuenciaPago = dr.GetOrdinal("codigo_frecuencia_pago");
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postOperacion = dr.GetOrdinal("operacion");
                            int postFrecuenciaPago = dr.GetOrdinal("frecuencia_pago");
                            int postAnio = dr.GetOrdinal("anio");
                            int postMes = dr.GetOrdinal("mes");
                            int postNombreMes = dr.GetOrdinal("nombre_mes");
                            int postMonto = dr.GetOrdinal("monto");
                            while (dr.Read())
                            {
                                objPagoDescuentoCLS = new PagoDescuentoCLS();
                                objPagoDescuentoCLS.CodigoPago = dr.GetInt32(postCodigoPago);
                                objPagoDescuentoCLS.CodigoTipoPlanilla = dr.GetInt16(postCodigoTipoPlanilla);
                                objPagoDescuentoCLS.TipoPlanilla = dr.GetString(postTipoPlanilla);
                                objPagoDescuentoCLS.CodigoEmpresa = dr.GetInt16(postCodigoEmpresa);
                                objPagoDescuentoCLS.NombreEmpresa = dr.GetString(postNombreEmpresa);
                                objPagoDescuentoCLS.CodigoEmpleado = dr.GetString(postCodigoEmpleado);
                                objPagoDescuentoCLS.NombreCompleto = dr.GetString(postNombreCompleto);
                                objPagoDescuentoCLS.CodigoFrecuenciaPago = dr.GetByte(postCodigoFrecuenciaPago);
                                objPagoDescuentoCLS.FrecuenciaPago = dr.GetString(postFrecuenciaPago);
                                objPagoDescuentoCLS.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objPagoDescuentoCLS.Operacion = dr.GetString(postOperacion);
                                objPagoDescuentoCLS.Anio = dr.GetInt16(postAnio);
                                objPagoDescuentoCLS.Mes = dr.GetByte(postMes);
                                objPagoDescuentoCLS.NombreMes = dr.GetString(postNombreMes);
                                objPagoDescuentoCLS.Monto = dr.GetDecimal(postMonto);
                                lista.Add(objPagoDescuentoCLS);
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

        public string AnularPagoDescuento(int codigoPago, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaContabilidad))
            {
                try
                {
                    string sql = "UPDATE db_contabilidad.pagos_y_descuentos SET codigo_estado = @CodigoEstadoAnulado WHERE codigo_pago = @CodigoPago";
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoAnulado", Constantes.Planilla.EstadoPagoDescuento.BLOQUEADO);
                        cmd.Parameters.AddWithValue("@CodigoPago", codigoPago);

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



    }
}
