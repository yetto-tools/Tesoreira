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
    public class PagoBackToBackPlanillaDAL : CadenaConexion
    {
        public List<PagoDescuentoCLS> GetEmpleadosBackToBackPlanilla(int codigoTipoPlanilla, int anioPlanilla, int mesPlanilla)
        {
            List<PagoDescuentoCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaContabilidad))
            {
                try
                {
                    string sql = @"
                    SELECT x.codigo_empresa,
                           y.nombre_comercial AS nombre_empresa, 
	                       x.codigo_empleado, 
                           db_rrhh.GetNombreCompletoEmpleado(x.cui) AS nombre_completo,
                           x.codigo_frecuencia_pago, 
                           z.nombre AS frecuencia_pago,     
                           65 AS codigo_operacion,
                           'Pago BTB'  AS operacion,
                           x.salario_diario,
                           x.bono_decreto_37_2001,
                           x.codigo_tipo_btb,
                           w.nombre AS tipo_btb, 
                           db_contabilidad.GetMontoDevolucionBTB(@CodigoTipoPlanilla, @AnioPlanilla, @MesPlanilla, x.salario_diario, x.bono_decreto_37_2001, x.codigo_tipo_btb) AS monto_devolucion_btb,
                           db_contabilidad.ExistePagoBTB(@CodigoTipoPlanilla, @AnioPlanilla, @MesPlanilla, x.codigo_empleado, x.codigo_empresa) AS existe_pago_btb,
                           0.00 AS monto_descuento 
                    FROM db_rrhh.empleado x
                    INNER JOIN db_admon.empresa y
                    ON x.codigo_empresa = y.codigo_empresa
                    INNER JOIN db_rrhh.frecuencia_pago z
                    ON x.codigo_frecuencia_pago = z.codigo_frecuencia_pago
                    INNER JOIN db_rrhh.tipo_btb w
                    ON x.codigo_tipo_btb = w.codigo_tipo_btb
                    WHERE x.codigo_tipo_btb <> 0 
                     AND  w.tesoreria = 1 
                     AND  x.codigo_estado NOT IN (@CodigoEstadoInactivo, @CodigoEstadoRetirado)";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        // CommandType, Gets or sets a value indicating how the CommandText property is to be interpreted.
                        // The Text CommandType.Text is used when the command is raw SQL
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoTipoPlanilla", codigoTipoPlanilla);
                        cmd.Parameters.AddWithValue("@AnioPlanilla", anioPlanilla);
                        cmd.Parameters.AddWithValue("@MesPlanilla", mesPlanilla);
                        cmd.Parameters.AddWithValue("@CodigoEstadoInactivo", Constantes.Empleado.EstadoEmpleado.INACTIVO);
                        cmd.Parameters.AddWithValue("@CodigoEstadoRetirado", Constantes.Empleado.EstadoEmpleado.RETIRADO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            PagoDescuentoCLS objPagoDescuentoCLS;
                            lista = new List<PagoDescuentoCLS>();
                            int postCodigoEmpresa = dr.GetOrdinal("codigo_empresa");
                            int postNombreEmpresa = dr.GetOrdinal("nombre_empresa");
                            int postCodigoEmpleado = dr.GetOrdinal("codigo_empleado");
                            int postNombreCompleto = dr.GetOrdinal("nombre_completo");
                            int postCodigoFrecuenciaPago = dr.GetOrdinal("codigo_frecuencia_pago");
                            int postFrecuenciaPago = dr.GetOrdinal("frecuencia_pago");
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postOperacion = dr.GetOrdinal("operacion");
                            int postSalarioDiario = dr.GetOrdinal("salario_diario");
                            int postBonoDecreto372001 = dr.GetOrdinal("bono_decreto_37_2001");
                            int postMontoDevolucionBTB = dr.GetOrdinal("monto_devolucion_btb");
                            int postCodigoTipoBTB = dr.GetOrdinal("codigo_tipo_btb");
                            int postTipoBTB = dr.GetOrdinal("tipo_btb");
                            int postMontoDescuento = dr.GetOrdinal("monto_descuento");
                            int postExistePagoBTB = dr.GetOrdinal("existe_pago_btb");

                            while (dr.Read())
                            {
                                objPagoDescuentoCLS = new PagoDescuentoCLS();
                                objPagoDescuentoCLS.CodigoEmpresa = dr.GetInt16(postCodigoEmpresa);
                                objPagoDescuentoCLS.NombreEmpresa = dr.GetString(postNombreEmpresa);
                                objPagoDescuentoCLS.CodigoEmpleado = dr.GetString(postCodigoEmpleado);
                                objPagoDescuentoCLS.NombreCompleto = dr.GetString(postNombreCompleto);
                                objPagoDescuentoCLS.CodigoFrecuenciaPago = dr.GetByte(postCodigoFrecuenciaPago);
                                objPagoDescuentoCLS.FrecuenciaPago = dr.GetString(postFrecuenciaPago);
                                objPagoDescuentoCLS.CodigoOperacion = (short)dr.GetInt32(postCodigoOperacion);
                                objPagoDescuentoCLS.Operacion = dr.GetString(postOperacion);
                                objPagoDescuentoCLS.BonoDecreto372001 = dr.GetDecimal(postBonoDecreto372001);
                                objPagoDescuentoCLS.MontoDevolucionBTB = dr.GetDecimal(postMontoDevolucionBTB);
                                objPagoDescuentoCLS.SalarioDiario = dr.GetDecimal(postSalarioDiario);
                                objPagoDescuentoCLS.CodigoTipoBTB = dr.GetByte(postCodigoTipoBTB);
                                objPagoDescuentoCLS.TipoBTB = dr.GetString(postTipoBTB);
                                objPagoDescuentoCLS.MontoDescuento = dr.GetDecimal(postMontoDescuento);
                                objPagoDescuentoCLS.ExistePagoBTB = (byte)dr.GetInt32(postExistePagoBTB);
                                lista.Add(objPagoDescuentoCLS);
                            }//fin while
                        }// fin if
                    }// fin using
                    conexion.Close();
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    lista = null;

                }
                return lista;
            }
        }

        public List<PagoDescuentoCLS> GetEmpleadosBackToBackBoletaDeposito(int codigoTipoPlanilla, int anioPlanilla, int mesPlanilla)
        {
            List<PagoDescuentoCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaContabilidad))
            {
                try
                {
                    string sql = @"
                    SELECT x.codigo_empresa,
                           y.nombre_comercial AS nombre_empresa, 
	                       x.codigo_empleado, 
                           db_rrhh.GetNombreCompletoEmpleado(x.cui) AS nombre_completo,
                           x.codigo_frecuencia_pago, 
                           z.nombre AS frecuencia_pago,     
                           65 AS codigo_operacion,
                           'Pago BTB'  AS operacion,
                           x.salario_diario,
                           x.bono_decreto_37_2001,
                           x.codigo_tipo_btb,
                           db_contabilidad.GetMontoDepositoBTB(@CodigoTipoPlanilla, @AnioPlanilla, @MesPlanilla, x.salario_diario, x.bono_decreto_37_2001, x.codigo_tipo_btb) AS monto_devolucion_btb,
                           '' AS numero_boleta,
                           db_admon.GetComboBancos() AS combo_bancos,
                           CONCAT('<select name=NumeroCuenta id=uiNumeroCuenta class=select-cuenta-bancaria>','<option value=-1>--Sin cuentas--</option>','</select>') AS combo_cuentas

                    FROM db_rrhh.empleado x
                    INNER JOIN db_admon.empresa y
                    ON x.codigo_empresa = y.codigo_empresa
                    INNER JOIN db_rrhh.frecuencia_pago z
                    ON x.codigo_frecuencia_pago = z.codigo_frecuencia_pago
                    INNER JOIN db_rrhh.tipo_btb w
                    ON x.codigo_tipo_btb = w.codigo_tipo_btb
                    WHERE x.codigo_tipo_btb IN (1,3)
                     AND  x.codigo_estado NOT IN (@CodigoEstadoInactivo, @CodigoEstadoRetirado)";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        // CommandType, Gets or sets a value indicating how the CommandText property is to be interpreted.
                        // The Text CommandType.Text is used when the command is raw SQL
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoTipoPlanilla", codigoTipoPlanilla);
                        cmd.Parameters.AddWithValue("@AnioPlanilla", anioPlanilla);
                        cmd.Parameters.AddWithValue("@MesPlanilla", mesPlanilla);
                        cmd.Parameters.AddWithValue("@CodigoEstadoInactivo", Constantes.Empleado.EstadoEmpleado.INACTIVO);
                        cmd.Parameters.AddWithValue("@CodigoEstadoRetirado", Constantes.Empleado.EstadoEmpleado.RETIRADO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            PagoDescuentoCLS objPagoDescuentoCLS;
                            lista = new List<PagoDescuentoCLS>();
                            int postCodigoEmpresa = dr.GetOrdinal("codigo_empresa");
                            int postNombreEmpresa = dr.GetOrdinal("nombre_empresa");
                            int postCodigoEmpleado = dr.GetOrdinal("codigo_empleado");
                            int postNombreCompleto = dr.GetOrdinal("nombre_completo");
                            int postCodigoFrecuenciaPago = dr.GetOrdinal("codigo_frecuencia_pago");
                            int postFrecuenciaPago = dr.GetOrdinal("frecuencia_pago");
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postOperacion = dr.GetOrdinal("operacion");
                            int postSalarioDiario = dr.GetOrdinal("salario_diario");
                            int postBonoDecreto372001 = dr.GetOrdinal("bono_decreto_37_2001");
                            int postMontoDevolucionBTB = dr.GetOrdinal("monto_devolucion_btb");
                            int postCodigoTipoBTB = dr.GetOrdinal("codigo_tipo_btb");
                            int postNumeroBoleta = dr.GetOrdinal("numero_boleta");
                            int postComboBancos = dr.GetOrdinal("combo_bancos");
                            int postComboCuentas = dr.GetOrdinal("combo_cuentas");
                            while (dr.Read())
                            {
                                objPagoDescuentoCLS = new PagoDescuentoCLS();
                                objPagoDescuentoCLS.CodigoEmpresa = dr.GetInt16(postCodigoEmpresa);
                                objPagoDescuentoCLS.NombreEmpresa = dr.GetString(postNombreEmpresa);
                                objPagoDescuentoCLS.CodigoEmpleado = dr.GetString(postCodigoEmpleado);
                                objPagoDescuentoCLS.NombreCompleto = dr.GetString(postNombreCompleto);
                                objPagoDescuentoCLS.CodigoFrecuenciaPago = dr.GetByte(postCodigoFrecuenciaPago);
                                objPagoDescuentoCLS.FrecuenciaPago = dr.GetString(postFrecuenciaPago);
                                objPagoDescuentoCLS.CodigoOperacion = (short)dr.GetInt32(postCodigoOperacion);
                                objPagoDescuentoCLS.Operacion = dr.GetString(postOperacion);
                                objPagoDescuentoCLS.BonoDecreto372001 = dr.GetDecimal(postBonoDecreto372001);
                                objPagoDescuentoCLS.MontoDevolucionBTB = dr.GetDecimal(postMontoDevolucionBTB);
                                objPagoDescuentoCLS.SalarioDiario = dr.GetDecimal(postSalarioDiario);
                                objPagoDescuentoCLS.CodigoTipoBTB = dr.GetByte(postCodigoTipoBTB);
                                objPagoDescuentoCLS.NumeroBoleta = dr.GetString(postNumeroBoleta);
                                objPagoDescuentoCLS.ComboBancos = dr.GetString(postComboBancos);
                                objPagoDescuentoCLS.ComboCuentas = dr.GetString(postComboCuentas);
                                lista.Add(objPagoDescuentoCLS);
                            }//fin while
                        }// fin if
                    }// fin using
                    conexion.Close();
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    lista = null;

                }
                return lista;
            }
        }

        public string GuardarDevolucionesBTB(List<PagoDescuentoCLS> objPagoDescuento, string usuarioIng)
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
                    string sqlSequence = String.Empty;
                    string sentenciaInsertPagos = String.Empty;
                    long codigoPagoDescuento = 0;
                    StringBuilder cadena = new StringBuilder();
                    int contador = 0;
                    foreach (PagoDescuentoCLS objPago in objPagoDescuento)
                    {
                        sqlSequence = "SELECT NEXT VALUE FOR db_contabilidad.SQ_PAGO_DESCUENTO";
                        cmd.CommandText = sqlSequence;
                        codigoPagoDescuento = (long)cmd.ExecuteScalar();

                        if (contador == 0)
                        {
                            cmd.Parameters.Add("@CodigoPago", SqlDbType.Int);
                            cmd.Parameters.Add("@CodigoTipoPlanilla", SqlDbType.SmallInt);
                            cmd.Parameters.Add("@CodigoEmpresa", SqlDbType.SmallInt);
                            cmd.Parameters.Add("@CodigoEmpleado", SqlDbType.VarChar);
                            cmd.Parameters.Add("@CodigoFrecuenciaPago", SqlDbType.TinyInt);
                            cmd.Parameters.Add("@CodigoOperacion", SqlDbType.SmallInt);
                            cmd.Parameters.Add("@Anio", SqlDbType.SmallInt);
                            cmd.Parameters.Add("@Mes", SqlDbType.TinyInt);
                            cmd.Parameters.Add("@CodigoQuincena", SqlDbType.Int);
                            cmd.Parameters.Add("@NumeroSemana", SqlDbType.TinyInt);
                            cmd.Parameters.Add("@Monto", SqlDbType.Decimal);
                            cmd.Parameters.Add("@MontoDescuento", SqlDbType.Decimal);
                            cmd.Parameters.Add("@MontoCalculado", SqlDbType.Decimal);
                            cmd.Parameters.Add("@MontoPlanillaExcel", SqlDbType.Decimal);
                            cmd.Parameters.Add("@CodigoEstado", SqlDbType.TinyInt);
                            cmd.Parameters.Add("@UsuarioIng", SqlDbType.VarChar);
                            cmd.Parameters.Add("@FechaIng", SqlDbType.DateTime);
                        }

                        sentenciaInsertPagos = @"
                        INSERT INTO db_contabilidad.pagos_y_descuentos(codigo_pago,codigo_tipo_planilla,codigo_empresa,codigo_empleado,codigo_frecuencia_pago,codigo_operacion,anio,mes,codigo_quincena,numero_semana,monto,codigo_estado,usuario_ing,fecha_ing, monto_descuento, monto_calculado, monto_planilla_excel)
                        VALUES(@CodigoPago,@CodigoTipoPlanilla,@CodigoEmpresa,@CodigoEmpleado,@CodigoFrecuenciaPago,@CodigoOperacion,@Anio,@Mes,@CodigoQuincena,@NumeroSemana,@Monto,@CodigoEstado,@UsuarioIng,@FechaIng,@MontoDescuento,@MontoCalculado,@MontoPlanillaExcel)";

                        objPago.Monto = objPago.MontoPlanillaExcel - objPago.MontoDescuento;

                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sentenciaInsertPagos;
                        cmd.Parameters["@CodigoPago"].Value = codigoPagoDescuento;
                        cmd.Parameters["@CodigoTipoPlanilla"].Value = objPago.CodigoTipoPlanilla;
                        cmd.Parameters["@CodigoEmpresa"].Value = objPago.CodigoEmpresa;
                        cmd.Parameters["@CodigoEmpleado"].Value = objPago.CodigoEmpleado;
                        cmd.Parameters["@CodigoFrecuenciaPago"].Value = objPago.CodigoFrecuenciaPago;
                        cmd.Parameters["@CodigoOperacion"].Value = objPago.CodigoOperacion;
                        cmd.Parameters["@Anio"].Value = objPago.Anio;
                        cmd.Parameters["@Mes"].Value = objPago.Mes;
                        cmd.Parameters["@CodigoQuincena"].Value = 0;
                        cmd.Parameters["@NumeroSemana"].Value = 0;
                        cmd.Parameters["@Monto"].Value = objPago.Monto;
                        cmd.Parameters["@CodigoEstado"].Value = Constantes.Planilla.EstadoPagoDescuento.COBRADO;
                        cmd.Parameters["@UsuarioIng"].Value = usuarioIng;
                        cmd.Parameters["@FechaIng"].Value = DateTime.Now;
                        cmd.Parameters["@MontoDescuento"].Value = objPago.MontoDescuento;
                        cmd.Parameters["@MontoCalculado"].Value = objPago.MontoCalculado;
                        cmd.Parameters["@MontoPlanillaExcel"].Value = objPago.MontoPlanillaExcel;
                        cmd.ExecuteNonQuery();

                        cadena.Append(codigoPagoDescuento.ToString());
                        cadena.Append(',');
                        contador++;
                    }

                    string listadoPagos = cadena.ToString().TrimEnd(',');
                    string sentenciaSQL = @"
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
                    WHERE x.codigo_pago IN (" + listadoPagos + ")";

                    cmd.CommandText = sentenciaSQL;
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


    }
}
