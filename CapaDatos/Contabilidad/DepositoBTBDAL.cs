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
    public class DepositoBTBDAL: CadenaConexion
    {
        public string GuardarDepositosBTB(List<DepositoBTBCLS> listaDepositos, string usuarioIng)
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
                    string sentenciaInsertDepositos = String.Empty;
                    int contador = 0;
                    foreach (DepositoBTBCLS objDeposito in listaDepositos)
                    {
                        if (contador == 0)
                        {
                            cmd.Parameters.Add("@CodigoTipoPlanilla", SqlDbType.SmallInt);
                            cmd.Parameters.Add("@AnioPlanilla", SqlDbType.SmallInt);
                            cmd.Parameters.Add("@MesPlanilla", SqlDbType.TinyInt);
                            cmd.Parameters.Add("@CodigoEmpresa", SqlDbType.SmallInt);
                            cmd.Parameters.Add("@CodigoEmpleado", SqlDbType.VarChar);
                            cmd.Parameters.Add("@CodigoFrecuenciaPago", SqlDbType.TinyInt);
                            cmd.Parameters.Add("@CodigoOperacion", SqlDbType.SmallInt);
                            cmd.Parameters.Add("@AnioOperacion", SqlDbType.SmallInt);
                            cmd.Parameters.Add("@SemanaOperacion", SqlDbType.TinyInt);
                            cmd.Parameters.Add("@CodigoReporte", SqlDbType.Int);
                            cmd.Parameters.Add("@CodigoBancoDeposito", SqlDbType.SmallInt);
                            cmd.Parameters.Add("@NumeroCuenta", SqlDbType.VarChar);
                            cmd.Parameters.Add("@NumeroBoleta", SqlDbType.VarChar);
                            cmd.Parameters.Add("@Monto", SqlDbType.Decimal);
                            cmd.Parameters.Add("@Observaciones", SqlDbType.VarChar);
                            cmd.Parameters.Add("@CodigoEstado", SqlDbType.TinyInt);
                            cmd.Parameters.Add("@UsuarioIng", SqlDbType.VarChar);
                            cmd.Parameters.Add("@FechaIng", SqlDbType.DateTime);
                            cmd.Parameters.Add("@UsuarioAct", SqlDbType.VarChar);
                            cmd.Parameters.Add("@FechaAct", SqlDbType.DateTime);
                        }
                        sentenciaInsertDepositos = @"
                        INSERT INTO db_contabilidad.deposito_btb(codigo_deposito_btb,codigo_tipo_planilla,anio_planilla,mes_planilla,codigo_empresa,codigo_empleado,codigo_frecuencia_pago,codigo_operacion,anio_operacion,semana_operacion,codigo_reporte,codigo_banco_deposito,numero_cuenta,numero_boleta,monto,observaciones,estado,usuario_ing,fecha_ing,usuario_act,fecha_act)
                        VALUES( NEXT VALUE FOR db_contabilidad.SQ_CODIGO_DEPOSITO_BTB,
                                @CodigoTipoPlanilla,
                                @AnioPlanilla,
                                @MesPlanilla,
                                @CodigoEmpresa,
                                @CodigoEmpleado,
                                @CodigoFrecuenciaPago,
                                @CodigoOperacion,
                                @AnioOperacion,
                                @SemanaOperacion,
                                @CodigoReporte,
                                @CodigoBancoDeposito,
                                @NumeroCuenta,
                                @NumeroBoleta,
                                @Monto,
                                @Observaciones,
                                @CodigoEstado,
                                @UsuarioIng,
                                @FechaIng,
                                @UsuarioAct,
                                @FechaAct)";

                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sentenciaInsertDepositos;
                        cmd.Parameters["@CodigoTipoPlanilla"].Value = objDeposito.CodigoTipoPlanilla;
                        cmd.Parameters["@AnioPlanilla"].Value = objDeposito.AnioPlanilla;
                        cmd.Parameters["@MesPlanilla"].Value = objDeposito.MesPlanilla;
                        cmd.Parameters["@CodigoEmpresa"].Value = objDeposito.CodigoEmpresa;
                        cmd.Parameters["@CodigoEmpleado"].Value = objDeposito.CodigoEmpleado;
                        cmd.Parameters["@CodigoFrecuenciaPago"].Value = objDeposito.CodigoFrecuenciaPago;
                        cmd.Parameters["@CodigoOperacion"].Value = objDeposito.CodigoOperacion;
                        cmd.Parameters["@AnioOperacion"].Value = objDeposito.AnioOperacion;
                        cmd.Parameters["@SemanaOperacion"].Value = objDeposito.SemanaOperacion;
                        cmd.Parameters["@CodigoReporte"].Value = objDeposito.CodigoReporte;
                        cmd.Parameters["@CodigoBancoDeposito"].Value = objDeposito.CodigoBancoDeposito;
                        cmd.Parameters["@NumeroCuenta"].Value = objDeposito.NumeroCuenta;
                        cmd.Parameters["@NumeroBoleta"].Value = objDeposito.NumeroBoleta;
                        cmd.Parameters["@Monto"].Value = objDeposito.Monto;
                        cmd.Parameters["@Observaciones"].Value = objDeposito.Observaciones == null ? DBNull.Value : objDeposito.Observaciones;
                        cmd.Parameters["@CodigoEstado"].Value = Constantes.EstadoRegistro.ACTIVO;
                        cmd.Parameters["@UsuarioIng"].Value = usuarioIng;
                        cmd.Parameters["@FechaIng"].Value = DateTime.Now;
                        cmd.Parameters["@UsuarioAct"].Value = DBNull.Value;
                        cmd.Parameters["@FechaAct"].Value = DBNull.Value;
                        cmd.ExecuteNonQuery();
                        contador++;
                    }

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

        public string ActualizarDeposito(DepositoBTBCLS objDeposito, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaContabilidad))
            {
                string sentenciaSQL = @"
                UPDATE db_contabilidad.deposito_btb
                SET monto = @Monto,
                    codigo_banco_deposito = @CodigoBancoDeposito,
                    numero_cuenta = @NumeroCuenta,
                    numero_boleta = @NumeroBoleta,
                    usuario_act = @UsuarioAct,
                    fecha_act = @FechaAct
                WHERE codigo_deposito_btb = @CodigoDepositoBTB";
                conexion.Open();
                try
                {
                    SqlCommand cmd = conexion.CreateCommand();
                    cmd.CommandText = sentenciaSQL;
                    cmd.Parameters.AddWithValue("@Monto", objDeposito.Monto);
                    cmd.Parameters.AddWithValue("@CodigoBancoDeposito", objDeposito.CodigoBancoDeposito);
                    cmd.Parameters.AddWithValue("@NumeroCuenta", objDeposito.NumeroCuenta);
                    cmd.Parameters.AddWithValue("@NumeroBoleta", objDeposito.NumeroBoleta);
                    cmd.Parameters.AddWithValue("@UsuarioAct", usuarioAct);
                    cmd.Parameters.AddWithValue("@FechaAct", DateTime.Now);
                    cmd.Parameters.AddWithValue("@CodigoDepositoBTB", objDeposito.CodigoDepositoBTB);
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

        public string AnularDeposito(int codigoDepositoBTB, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaContabilidad))
            {
                string sentenciaSQL = @"
                UPDATE db_contabilidad.deposito_btb
                SET estado = @CodigoEstadoAnulado,
                    usuario_act = @UsuarioAct,
                    fecha_act = @FechaAct
                WHERE codigo_deposito_btb = @CodigoDepositoBTB";
                conexion.Open();
                try
                {
                    SqlCommand cmd = conexion.CreateCommand();
                    cmd.CommandText = sentenciaSQL;
                    cmd.Parameters.AddWithValue("@CodigoEstadoAnulado", Constantes.EstadoRegistro.BLOQUEADO);
                    cmd.Parameters.AddWithValue("@UsuarioAct", usuarioAct);
                    cmd.Parameters.AddWithValue("@FechaAct", DateTime.Now);
                    cmd.Parameters.AddWithValue("@CodigoDepositoBTB", codigoDepositoBTB);
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

        public List<DepositoBTBCLS> GetDepositosBTB(int anioPlanilla, int anioOperacion, int semanaOperacion, int codigoReporte)
        {
            List<DepositoBTBCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaContabilidad))
            {
                try
                {
                    string filterAnioPlanilla = String.Empty;
                    string filterAnioOperacion = String.Empty;
                    string filterSemanaOperacion = String.Empty;
                    string filterReporte = String.Empty;
                    string filterEmpresa = String.Empty;
                    if (anioPlanilla != -1) {
                        filterAnioPlanilla = "AND x.anio_planilla = " + anioPlanilla.ToString();
                    }
                    if (anioOperacion != -1)
                    {
                        filterAnioOperacion = "AND x.anio_operacion = " + anioOperacion.ToString();
                    }
                    if (semanaOperacion != -1)
                    {
                        filterSemanaOperacion = " AND x.semana_operacion = " + semanaOperacion.ToString();
                    }
                    if (codigoReporte != -1)
                    {
                        filterReporte = " AND x.codigo_reporte = " + codigoReporte.ToString();
                    }
                    string sql = @" 
                    SELECT x.codigo_deposito_btb, 
	                       x.codigo_tipo_planilla,
	                       y.nombre AS tipo_planilla,
	                       x.anio_planilla,
	                       x.mes_planilla,
	                       CASE
		                     WHEN x.mes_planilla = 1 THEN 'ENERO'
		                     WHEN x.mes_planilla = 2 THEN 'FEBRERO'
		                     WHEN x.mes_planilla = 3 THEN 'MARZO'
		                     WHEN x.mes_planilla = 4 THEN 'ABRIL'
		                     WHEN x.mes_planilla = 5 THEN 'MAYO'
		                     WHEN x.mes_planilla = 6 THEN 'JUNIO'
		                     WHEN x.mes_planilla = 7 THEN 'JULIO'
		                     WHEN x.mes_planilla = 8 THEN 'AGOSTO'
		                     WHEN x.mes_planilla = 9 THEN 'SEPTIEMBRE'
		                     WHEN x.mes_planilla = 10 THEN 'OCTUBRE'
		                     WHEN x.mes_planilla = 11 THEN 'NOVIEMBRE'
		                     WHEN x.mes_planilla = 12 THEN 'DICIEMBRE'
		                     ELSE 'NO DEFINIDO'
	                       END AS nombre_mes_planilla,
	                       x.codigo_empresa,
	                       z.nombre_comercial AS nombre_empresa,
	                       x.codigo_empleado,
	                       db_rrhh.GetNombreCompletoEmpleado(w.cui) AS nombre_empleado,
	                       x.codigo_frecuencia_pago,
	                       a.nombre AS frecuencia_pago,
                           x.anio_operacion, 
	                       x.semana_operacion,
                           x.codigo_reporte, 
                           db_admon.GetPeriodoSemana(x.anio_operacion,x.semana_operacion) AS periodo,
	                       x.codigo_banco_deposito,
	                       b.nombre AS banco_deposito,
	                       x.numero_cuenta,
	                       x.numero_boleta,
	                       x.monto,
	                       x.fecha_ing,
                           FORMAT(x.fecha_ing, 'dd/MM/yyyy, hh:mm:ss') AS fecha_ing_str,
	                       x.usuario_ing,
                           1 AS permiso_editar,
                           1 AS permiso_anular 

                    FROM db_contabilidad.deposito_btb x
                    INNER JOIN db_contabilidad.tipo_planilla y
                    ON x.codigo_tipo_planilla = y.codigo_tipo_planilla
                    INNER JOIN db_admon.empresa z
                    ON x.codigo_empresa = z.codigo_empresa
                    INNER JOIN db_rrhh.empleado w
                    ON x.codigo_empleado = w.codigo_empleado AND x.codigo_empresa = w.codigo_empresa
                    INNER JOIN db_rrhh.frecuencia_pago a
                    ON x.codigo_frecuencia_pago = a.codigo_frecuencia_pago
                    INNER JOIN db_admon.banco b
                    ON x.codigo_banco_deposito = b.codigo_banco
                    WHERE x.estado <> 0	                    
                    " + filterAnioPlanilla + @"
                    " + filterAnioOperacion + @"
                    " + filterSemanaOperacion + @"
                    " + filterReporte;



                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        // CommandType, Gets or sets a value indicating how the CommandText property is to be interpreted.
                        // The Text CommandType.Text is used when the command is raw SQL
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            DepositoBTBCLS objDepositosBTBCLS;
                            lista = new List<DepositoBTBCLS>();
                            int postCodigoDepositoBTB = dr.GetOrdinal("codigo_deposito_btb");
                            int postTipoPlanilla = dr.GetOrdinal("tipo_planilla");
                            int postAnioPlanilla = dr.GetOrdinal("anio_planilla");
                            int postMesPlanilla = dr.GetOrdinal("mes_planilla");
                            int postNombreMesPlanilla = dr.GetOrdinal("nombre_mes_planilla");
                            int postCodigoEmpresa = dr.GetOrdinal("codigo_empresa");
                            int postNombreEmpresa = dr.GetOrdinal("nombre_empresa");
                            int postCodigoEmpleado = dr.GetOrdinal("codigo_empleado");
                            int postNombreEmpleado = dr.GetOrdinal("nombre_empleado");
                            int postAnioOperacion = dr.GetOrdinal("anio_operacion");
                            int postSemanaOperacion = dr.GetOrdinal("semana_operacion");
                            int postCodigoReporte = dr.GetOrdinal("codigo_reporte");
                            int postPeriodo = dr.GetOrdinal("periodo");
                            int postCodigoBancoDeposito = dr.GetOrdinal("codigo_banco_deposito");
                            int postBancoDeposito = dr.GetOrdinal("banco_deposito");
                            int postNumeroCuenta = dr.GetOrdinal("numero_cuenta");
                            int postNumeroBoleta = dr.GetOrdinal("numero_boleta");
                            int postMonto = dr.GetOrdinal("monto");
                            int postUsuarioIng = dr.GetOrdinal("usuario_ing");
                            int postFechaIngStr = dr.GetOrdinal("fecha_ing_str");
                            int postPermisoEditar = dr.GetOrdinal("permiso_editar");
                            int postPermisoAnular = dr.GetOrdinal("permiso_anular");
                            while (dr.Read())
                            {
                                objDepositosBTBCLS = new DepositoBTBCLS();
                                objDepositosBTBCLS.CodigoDepositoBTB = dr.GetInt64(postCodigoDepositoBTB);
                                objDepositosBTBCLS.TipoPlanilla = dr.GetString(postTipoPlanilla);
                                objDepositosBTBCLS.AnioPlanilla = dr.GetInt16(postAnioPlanilla);
                                objDepositosBTBCLS.MesPlanilla = dr.GetByte(postMesPlanilla);
                                objDepositosBTBCLS.NombreMesPlanilla = dr.GetString(postNombreMesPlanilla);
                                objDepositosBTBCLS.CodigoEmpresa = dr.GetInt16(postCodigoEmpresa);
                                objDepositosBTBCLS.NombreEmpresa = dr.GetString(postNombreEmpresa);
                                objDepositosBTBCLS.CodigoEmpleado = dr.GetString(postCodigoEmpleado);
                                objDepositosBTBCLS.NombreEmpleado = dr.GetString(postNombreEmpleado);
                                objDepositosBTBCLS.AnioOperacion = dr.GetInt16(postAnioOperacion);
                                objDepositosBTBCLS.SemanaOperacion = dr.GetByte(postSemanaOperacion);
                                objDepositosBTBCLS.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objDepositosBTBCLS.Periodo = dr.GetString(postPeriodo);
                                objDepositosBTBCLS.CodigoBancoDeposito = dr.GetInt16(postCodigoBancoDeposito);
                                objDepositosBTBCLS.BancoDeposito = dr.GetString(postBancoDeposito);
                                objDepositosBTBCLS.NumeroCuenta = dr.GetString(postNumeroCuenta);
                                objDepositosBTBCLS.NumeroBoleta = dr.GetString(postNumeroBoleta);
                                objDepositosBTBCLS.Monto = dr.GetDecimal(postMonto);
                                objDepositosBTBCLS.UsuarioIng = dr.GetString(postUsuarioIng);
                                objDepositosBTBCLS.FechaIngStr = dr.GetString(postFechaIngStr);
                                objDepositosBTBCLS.PermisoEditar = (byte)dr.GetInt32(postPermisoEditar);
                                objDepositosBTBCLS.PermisoAnular = (byte)dr.GetInt32(postPermisoAnular);
                                lista.Add(objDepositosBTBCLS);
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
