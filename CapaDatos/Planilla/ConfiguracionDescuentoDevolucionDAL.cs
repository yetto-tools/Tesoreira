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
    public class ConfiguracionDescuentoDevolucionDAL: CadenaConexion
    {
        public List<ConfiguracionPrestamoCLS> GetEmpleadosCuentasPorCobrar(int codigoFrecuenciaPago)
        {
            List<ConfiguracionPrestamoCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaContabilidad))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_rrhh.uspGetCuentasPorCobrarConfiguracion", conexion))
                    {
                        // CommandType, Gets or sets a value indicating how the CommandText property is to be interpreted.
                        // The Text CommandType.Text is used when the command is raw SQL
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CodigoFrecuenciaPago", codigoFrecuenciaPago);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            ConfiguracionPrestamoCLS objConfigPrestamoCLS;
                            lista = new List<ConfiguracionPrestamoCLS>();
                            int postCodigoEmpresa = dr.GetOrdinal("codigo_empresa");
                            int postNombreEmpresa = dr.GetOrdinal("nombre_empresa");
                            int postCodigoEmpleado = dr.GetOrdinal("codigo_empleado");
                            int postNombreCompleto = dr.GetOrdinal("nombre_completo");
                            int postCodigoFrecuenciaPago = dr.GetOrdinal("codigo_frecuencia_pago");
                            int postFrecuenciaPago = dr.GetOrdinal("frecuencia_pago");
                            int postMontoDescuentoPrestamo = dr.GetOrdinal("monto_descuento_prestamo");
                            int postSaldoPrestamo = dr.GetOrdinal("saldo_prestamo");
                            while (dr.Read())
                            {
                                objConfigPrestamoCLS = new ConfiguracionPrestamoCLS();
                                objConfigPrestamoCLS.CodigoEmpresa = (short)dr.GetInt32(postCodigoEmpresa);
                                objConfigPrestamoCLS.NombreEmpresa = dr.GetString(postNombreEmpresa);
                                objConfigPrestamoCLS.CodigoEmpleado = dr.GetString(postCodigoEmpleado);
                                objConfigPrestamoCLS.NombreCompleto = dr.GetString(postNombreCompleto);
                                objConfigPrestamoCLS.CodigoFrecuenciaPago = dr.GetByte(postCodigoFrecuenciaPago);
                                objConfigPrestamoCLS.FrecuenciaPago = dr.GetString(postFrecuenciaPago);
                                objConfigPrestamoCLS.MontoDescuentoPrestamo = dr.GetDecimal(postMontoDescuentoPrestamo);
                                objConfigPrestamoCLS.SaldoPrestamo = dr.GetDecimal(postSaldoPrestamo);
                                lista.Add(objConfigPrestamoCLS);
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

        public List<ConfiguracionPrestamoCLS> GetEmpleadosBackToBack()
        {
            List<ConfiguracionPrestamoCLS> lista = null;
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
                           x.bono_decreto_37_2001,
	                       x.salario_diario
                    FROM db_rrhh.empleado x
                    INNER JOIN db_admon.empresa y
                    ON x.codigo_empresa = y.codigo_empresa
                    INNER JOIN db_rrhh.frecuencia_pago z
                    ON x.codigo_frecuencia_pago = z.codigo_frecuencia_pago
                    INNER JOIN db_rrhh.tipo_btb w
                    ON x.codigo_tipo_btb = w.codigo_tipo_btb
                    WHERE w.tesoreria = 1 AND 
                          x.codigo_estado NOT IN (@CodigoEstadoInactivo, @CodigoEstadoRetirado)";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        // CommandType, Gets or sets a value indicating how the CommandText property is to be interpreted.
                        // The Text CommandType.Text is used when the command is raw SQL
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoInactivo", Constantes.Empleado.EstadoEmpleado.INACTIVO);
                        cmd.Parameters.AddWithValue("@CodigoEstadoRetirado", Constantes.Empleado.EstadoEmpleado.RETIRADO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            ConfiguracionPrestamoCLS objPagoDescuentoCLS;
                            lista = new List<ConfiguracionPrestamoCLS>();
                            int postCodigoEmpresa = dr.GetOrdinal("codigo_empresa");
                            int postNombreEmpresa = dr.GetOrdinal("nombre_empresa");
                            int postCodigoEmpleado = dr.GetOrdinal("codigo_empleado");
                            int postNombreCompleto = dr.GetOrdinal("nombre_completo");
                            int postCodigoFrecuenciaPago = dr.GetOrdinal("codigo_frecuencia_pago");
                            int postFrecuenciaPago = dr.GetOrdinal("frecuencia_pago");
                            int postBonoDecreto372001 = dr.GetOrdinal("bono_decreto_37_2001");
                            int postMontoSalarioDiario = dr.GetOrdinal("salario_diario");
                            while (dr.Read())
                            {
                                objPagoDescuentoCLS = new ConfiguracionPrestamoCLS();
                                objPagoDescuentoCLS.CodigoEmpresa = dr.GetInt16(postCodigoEmpresa);
                                objPagoDescuentoCLS.NombreEmpresa = dr.GetString(postNombreEmpresa);
                                objPagoDescuentoCLS.CodigoEmpleado = dr.GetString(postCodigoEmpleado);
                                objPagoDescuentoCLS.NombreCompleto = dr.GetString(postNombreCompleto);
                                objPagoDescuentoCLS.CodigoFrecuenciaPago = dr.GetByte(postCodigoFrecuenciaPago);
                                objPagoDescuentoCLS.FrecuenciaPago = dr.GetString(postFrecuenciaPago);
                                objPagoDescuentoCLS.BonoDecreto372001 = dr.GetDecimal(postBonoDecreto372001);
                                objPagoDescuentoCLS.SalarioDiario = dr.GetDecimal(postMontoSalarioDiario);
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

        public string RegistrarConfiguracionDevolucionBTB(int codigoEmpresa, string codigoEmpleado, decimal montoSalarioDiario, decimal montoBonoDecreto372001, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaRRHH))
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
                    string sentenciaInsertHistorial = @"
                    INSERT INTO db_rrhh.empleado_hist SELECT NEXT VALUE FOR db_rrhh.SQ_CODIGO_HIST_EMPLEADO,
                                                        codigo_empresa,
                                                        codigo_empleado,
                                                        codigo_tipo_identificacion,
                                                        cui,
                                                        nit,
                                                        correo_electronico,
                                                        numero_afiliacion,
                                                        empleado_externo,
                                                        codigo_area,
                                                        codigo_seccion,
                                                        codigo_puesto,
                                                        codigo_tipo_cuenta,
                                                        codigo_ubicacion,
                                                        numero_cuenta,
                                                        monto_devengado,
                                                        codigo_jornada,
                                                        codigo_frecuencia_pago,
                                                        igss,
                                                        bono_de_ley,
                                                        retencion_isr,
                                                        fecha_ingreso,
                                                        fecha_egreso,
                                                        codigo_motivo_baja,
                                                        observaciones,
                                                        codigo_estado,
                                                        @UsuarioIng AS usuario_ing,
                                                        @FechaIng AS fecha_ing,
                                                        NULL AS usuario_act,
                                                        NULL AS fecha_act,
                                                        back_to_back,
                                                        monto_descuento_prestamo
                                                    FROM db_rrhh.empleado
                                                    WHERE codigo_empresa = @CodigoEmpresa
                                                      AND codigo_empleado = @CodigoEmpleado";

                    cmd.CommandText = sentenciaInsertHistorial;
                    cmd.Parameters.AddWithValue("@CodigoEmpresa", codigoEmpresa);
                    cmd.Parameters.AddWithValue("@CodigoEmpleado", codigoEmpleado);
                    cmd.Parameters.AddWithValue("@UsuarioIng", usuarioAct);
                    cmd.Parameters.AddWithValue("@FechaIng", DateTime.Now);
                    cmd.ExecuteNonQuery();

                    string sentenciaSQL = @"
                    UPDATE db_rrhh.empleado 
                    SET salario_diario = @MontoSalarioDiario,
                        bono_decreto_37_2001 = @MontoDrecreto372001,
                        usuario_act = @UsuarioAct,
                        fecha_act = @FechaAct
                    WHERE codigo_empleado = @CodigoEmpleado 
                      AND codigo_empresa = @CodigoEmpresa";

                    cmd.CommandText = sentenciaSQL;
                    cmd.Parameters.AddWithValue("@MontoSalarioDiario", montoSalarioDiario);
                    cmd.Parameters.AddWithValue("@MontoDrecreto372001", montoBonoDecreto372001);
                    cmd.Parameters["@CodigoEmpresa"].Value = codigoEmpresa;
                    cmd.Parameters["@CodigoEmpleado"].Value = codigoEmpleado;
                    cmd.Parameters.AddWithValue("@UsuarioAct", usuarioAct);
                    cmd.Parameters.AddWithValue("@FechaAct", DateTime.Now);
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

        public string RegistrarConfiguracionPrestamo(int codigoEmpresa, string codigoEmpleado, decimal montoDescuentoPrestamo, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaRRHH))
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
                    string sentenciaInsertHistorial = @"
                    INSERT INTO db_rrhh.empleado_hist SELECT NEXT VALUE FOR db_rrhh.SQ_CODIGO_HIST_EMPLEADO,
                                                        codigo_empresa,
                                                        codigo_empleado,
                                                        codigo_tipo_identificacion,
                                                        cui,
                                                        nit,
                                                        correo_electronico,
                                                        numero_afiliacion,
                                                        empleado_externo,
                                                        codigo_area,
                                                        codigo_seccion,
                                                        codigo_puesto,
                                                        codigo_tipo_cuenta,
                                                        codigo_ubicacion,
                                                        numero_cuenta,
                                                        monto_devengado,
                                                        codigo_jornada,
                                                        codigo_frecuencia_pago,
                                                        igss,
                                                        bono_de_ley,
                                                        retencion_isr,
                                                        fecha_ingreso,
                                                        fecha_egreso,
                                                        codigo_motivo_baja,
                                                        observaciones,
                                                        codigo_estado,
                                                        @UsuarioIng AS usuario_ing,
                                                        @FechaIng AS fecha_ing,
                                                        NULL AS usuario_act,
                                                        NULL AS fecha_act,
                                                        back_to_back,
                                                        monto_descuento_prestamo
                                                    FROM db_rrhh.empleado
                                                    WHERE codigo_empresa = @CodigoEmpresa
                                                      AND codigo_empleado = @CodigoEmpleado";

                    cmd.CommandText = sentenciaInsertHistorial;
                    cmd.Parameters.AddWithValue("@CodigoEmpresa", codigoEmpresa);
                    cmd.Parameters.AddWithValue("@CodigoEmpleado", codigoEmpleado);
                    cmd.Parameters.AddWithValue("@UsuarioIng", usuarioAct);
                    cmd.Parameters.AddWithValue("@FechaIng", DateTime.Now);
                    cmd.ExecuteNonQuery();

                    string sentenciaSQL = @"  
                    UPDATE db_rrhh.empleado
                    SET monto_descuento_prestamo = @MontoDescuentoPrestamo,
                        usuario_act = @UsuarioAct,
                        fecha_act = @FechaAct
                    WHERE codigo_empleado = @CodigoEmpleado
                      AND codigo_empresa = @CodigoEmpresa";

                    cmd.CommandText = sentenciaSQL;
                    cmd.Parameters.AddWithValue("@MontoDescuentoPrestamo", montoDescuentoPrestamo);
                    cmd.Parameters["@CodigoEmpresa"].Value = codigoEmpresa;
                    cmd.Parameters["@CodigoEmpleado"].Value = codigoEmpleado;
                    cmd.Parameters.AddWithValue("@UsuarioAct", usuarioAct);
                    cmd.Parameters.AddWithValue("@FechaAct", DateTime.Now);
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


    }
}
