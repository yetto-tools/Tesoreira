using CapaEntidad.Administracion;
using CapaEntidad.RRHH;
using CapaEntidad.Tesoreria;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Tesoreria
{
    public class TransaccionDAL:CadenaConexion
    {
        /// <summary>
        /// Registra una nueva transaccion
        /// </summary>
        /// <param name="objTransaccion"></param>
        /// <param name="usuarioIng"></param>
        /// <returns></returns>
        public string GuardarTransaccion(TransaccionCLS objTransaccion, string usuarioIng) 
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
                    string sqlSequence = "SELECT sig_valor FROM db_admon.secuencia_detalle WHERE codigo_secuencia = @CodigoSecuencia AND anio = @AnioTransaccion";

                    // ExecuteScalar(), Executes the query, and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.
                    cmd.CommandText = sqlSequence;
                    cmd.Parameters.AddWithValue("@CodigoSecuencia", Constantes.Secuencia.SIT_SEQ_TRANSACCION);
                    cmd.Parameters.AddWithValue("@AnioTransaccion", anio);
                    long correlativoTransaccion = (long) cmd.ExecuteScalar();

                    string sentenciaUpdateTransaccion = string.Empty;
                    string sentenciaUpdateRecibo = string.Empty;
                    long correlativoRecibo = 0;
                    long correlativoReciboReferencia = 0;
                    if (objTransaccion.ComplementoConta == 0)
                    {
                        sqlSequence = "";
                        if (objTransaccion.CodigoTipoOperacion == 1)
                        { // Ingreso
                            sqlSequence = "SELECT NEXT VALUE FOR db_tesoreria.SQ_RECIBO_INGRESO";
                        }
                        else
                        { // Egreso
                            sqlSequence = "SELECT NEXT VALUE FOR db_tesoreria.SQ_RECIBO_EGRESO";
                        }
                        cmd.CommandText = sqlSequence;
                        correlativoRecibo = (long)cmd.ExecuteScalar();
                    }

                    if (objTransaccion.NumeroReciboReferencia == -1)
                    {
                        correlativoReciboReferencia = 0;
                    }
                    else {
                        correlativoReciboReferencia = objTransaccion.NumeroReciboReferencia;
                    }

                    sentenciaUpdateTransaccion = "UPDATE db_admon.secuencia_detalle SET sig_valor = @siguienteValor WHERE codigo_secuencia = @CodigoSecuencia  AND anio = @AnioTransaccion";
                    long codigoTransaccion = long.Parse(anio.ToString() + correlativoTransaccion.ToString("D6"));

                    string sentenciaSQL = @"
                    INSERT INTO db_tesoreria.transaccion( codigo_transaccion,
                                                          codigo_seguridad,  
                                                          codigo_empresa,
                                                          codigo_operacion,
                                                          codigo_operacion_caja,
                                                          codigo_tipo_cxc,
                                                          codigo_cxc,
                                                          codigo_area,
                                                          codigo_categoria_entidad,
                                                          codigo_entidad,
                                                          codigo_tipo_operacion,  
                                                          codigo_tipo_transaccion,
                                                          codigo_tipo_documento,  
                                                          efectivo,
                                                          deposito,
                                                          cheque,  
                                                          nit_proveedor,  
                                                          serie_factura,
                                                          numero_documento,
                                                          fecha_documento,
                                                          conceder_iva,
                                                          nit_empresa_concede_iva,  
                                                          codigo_banco_deposito,
                                                          numero_cuenta,  
                                                          numero_boleta, 
                                                          numero_recibo,
                                                          fecha_recibo,
                                                          fecha_operacion,
                                                          anio_operacion,
                                                          semana_operacion,
                                                          dia_operacion,
                                                          codigo_boleta_comision,  
                                                          ruta,
                                                          codigo_vendedor,  
                                                          semana_comision,
                                                          anio_comision,
                                                          monto_efectivo,
                                                          monto_cheques,  
                                                          monto,
                                                          codigo_frecuencia_pago,  
                                                          codigo_tipo_pago,		
	                                                      codigo_planilla,
	                                                      codigo_pago_planilla,
	                                                      anio_planilla,	
	                                                      mes_planilla,		
	                                                      semana_planilla,		
	                                                      codigo_quincena_planilla,
                                                          codigo_bono_extra,  
                                                          tipo_especiales_1,
                                                          observaciones,
                                                          fecha_confirmacion,
                                                          motivo_anulacion,
                                                          usuario_anulacion,
                                                          fecha_anulacion,  
                                                          codigo_estado,
                                                          usuario_ing,
                                                          fecha_ing,
                                                          usuario_act,
                                                          fecha_act,
                                                          anio_sueldo_indirecto,
                                                          mes_sueldo_indirecto,
                                                          complemento_conta,
                                                          codigo_reporte,
                                                          codigo_tipo_doc_deposito,
                                                          numero_voucher,
                                                          nombre_proveedor,
                                                          codigo_canal_venta,
                                                          codigo_otro_ingreso,
                                                          numero_recibo_referencia,
                                                          monto_saldo_anterior_cxc,
                                                          monto_saldo_actual_cxc)
                    VALUES(@CodigoTransaccion,
                           @CodigoSeguridad,
                           @CodigoEmpresa,
                           @CodigoOperacion,
                           @CodigoOperacionCaja,
                           @CodigoTipoCuentaPorCobrar,
                           @CodigoCuentaPorCobrar,
                           @CodigoArea,
                           @CodigoCategoriaEntidad,
                           @CodigoEntidad,
                           @CodigoTipoOperacion, 
                           @CodigoTipoTransaccion,
                           @CodigoTipoDocumento, 
                           @Efectivo,
                           @Deposito,
                           @Cheque, 
                           @NitProveedor, 
                           @SerieFactura,
                           @NumeroDocumento,
                           @FechaDocumento,
                           @ConcederIva,
                           @NitEmpresaConcedeIva, 
                           @CodigoBancoDeposito,
                           @NumeroCuenta,
                           @NumeroBoleta, 
                           @NumeroRecibo,
                           @FechaRecibo,
                           @FechaOperacion,
                           @AnioOperacion,
                           @SemanaOperacion,
                           @DiaOperacion,
                           @CodigoBoletaComision,
                           @Ruta,
                           @CodigoVendedor,
                           @SemanaComision, 
                           @AnioComision,
                           @MontoEfectivo,
                           @MontoCheques, 
                           @Monto,
                           @CodigoFrecuenciaPago, 
                           @CodigoTipoPago,
                           @CodigoPlanilla,
                           @CodigoPagoPlanilla,
                           @AnioPlanilla,
                           @MesPlanilla,
                           @SemanaPlanilla,
                           @CodigoQuincenaPlanilla, 
                           @CodigoBonoExtra,
                           @TipoEspeciales1,
                           @Observaciones,
                           @FechaConfirmacion,
                           @MotivoAnulacion,
                           @UsuarioAnulacion,
                           @FechaAnulacion, 
                           @CodigoEstado,
                           @UsuarioIng,
                           @FechaIng,
                           @UsuarioAct,
                           @FechaAct,
                           @AnioSueldoIndirecto,
                           @MesSueldoIndirecto,
                           @ComplementoConta,
                           @CodigoReporte,
                           @CodigoTipoDocumentoDeposito,
                           @NumeroVoucher,
                           @NombreProveedor,
                           @CodigoCanalVenta,
                           @CodigoOtroIngreso,
                           @NumeroReciboReferencia,
                           @MontoSaldoAnteriorCuentaPorCobrar,
                           @MontoSaldoActualCuentaPorCobrar)";

                    cmd.CommandText = sentenciaSQL;
                    cmd.Parameters.AddWithValue("@CodigoTransaccion", codigoTransaccion);
                    cmd.Parameters.AddWithValue("@CodigoSeguridad", codigoSeguridad);
                    cmd.Parameters.AddWithValue("@CodigoEmpresa", objTransaccion.CodigoEmpresa == null ? DBNull.Value : objTransaccion.CodigoEmpresa);
                    cmd.Parameters.AddWithValue("@CodigoOperacion", objTransaccion.CodigoOperacion);
                    cmd.Parameters.AddWithValue("@CodigoOperacionCaja", objTransaccion.CodigoOperacionCaja);
                    cmd.Parameters.AddWithValue("@CodigoTipoCuentaPorCobrar", objTransaccion.CodigoTipoCuentaPorCobrar);
                    cmd.Parameters.AddWithValue("@CodigoCuentaPorCobrar", objTransaccion.CodigoCuentaPorCobrar == null ? DBNull.Value : objTransaccion.CodigoCuentaPorCobrar);
                    cmd.Parameters.AddWithValue("@CodigoArea", objTransaccion.CodigoArea);
                    cmd.Parameters.AddWithValue("@CodigoCategoriaEntidad", objTransaccion.CodigoCategoriaEntidad);
                    cmd.Parameters.AddWithValue("@CodigoEntidad", objTransaccion.CodigoEntidad);
                    cmd.Parameters.AddWithValue("@CodigoTipoOperacion", objTransaccion.CodigoTipoOperacion);
                    cmd.Parameters.AddWithValue("@CodigoTipoTransaccion", objTransaccion.CodigoTipoTransaccion);
                    cmd.Parameters.AddWithValue("@CodigoTipoDocumento", objTransaccion.CodigoTipoDocumento);
                    cmd.Parameters.AddWithValue("@Efectivo", objTransaccion.Efectivo);
                    cmd.Parameters.AddWithValue("@Deposito", objTransaccion.Deposito);
                    cmd.Parameters.AddWithValue("@Cheque", objTransaccion.Cheque);
                    cmd.Parameters.AddWithValue("@NitProveedor", objTransaccion.NitProveedor == null ? DBNull.Value : objTransaccion.NitProveedor);
                    cmd.Parameters.AddWithValue("@SerieFactura", objTransaccion.SerieFactura == null ? DBNull.Value : objTransaccion.SerieFactura);
                    cmd.Parameters.AddWithValue("@NumeroDocumento", objTransaccion.NumeroDocumento == null ? DBNull.Value : objTransaccion.NumeroDocumento);
                    cmd.Parameters.AddWithValue("@FechaDocumento", objTransaccion.FechaDocumento == null ? DBNull.Value : objTransaccion.FechaDocumento);
                    cmd.Parameters.AddWithValue("@ConcederIva", objTransaccion.ConcederIva);
                    cmd.Parameters.AddWithValue("@NitEmpresaConcedeIva", objTransaccion.NitEmpresaConcedeIva == null ? DBNull.Value : objTransaccion.NitEmpresaConcedeIva);
                    cmd.Parameters.AddWithValue("@CodigoBancoDeposito", objTransaccion.CodigoBancoDeposito == null? DBNull.Value : objTransaccion.CodigoBancoDeposito);
                    cmd.Parameters.AddWithValue("@NumeroCuenta", objTransaccion.NumeroCuenta == null ? DBNull.Value : objTransaccion.NumeroCuenta);
                    cmd.Parameters.AddWithValue("@NumeroBoleta", objTransaccion.NumeroBoleta == null ? DBNull.Value : objTransaccion.NumeroBoleta);
                    cmd.Parameters.AddWithValue("@NumeroRecibo", correlativoRecibo);
                    cmd.Parameters.AddWithValue("@FechaRecibo", DateTime.Now);
                    cmd.Parameters.AddWithValue("@FechaOperacion", objTransaccion.FechaOperacion);
                    cmd.Parameters.AddWithValue("@AnioOperacion", objTransaccion.AnioOperacion);
                    cmd.Parameters.AddWithValue("@SemanaOperacion", objTransaccion.SemanaOperacion);
                    cmd.Parameters.AddWithValue("@DiaOperacion", Util.Conversion.DayOfWeek(objTransaccion.FechaOperacion));
                    cmd.Parameters.AddWithValue("@CodigoBoletaComision", objTransaccion.CodigoBoletaComision == null ? DBNull.Value : objTransaccion.CodigoBoletaComision);
                    cmd.Parameters.AddWithValue("@Ruta", objTransaccion.Ruta);
                    cmd.Parameters.AddWithValue("@CodigoVendedor", objTransaccion.CodigoVendedor == null ? DBNull.Value : objTransaccion.CodigoVendedor);
                    cmd.Parameters.AddWithValue("@SemanaComision", objTransaccion.SemanaComision);
                    cmd.Parameters.AddWithValue("@AnioComision", objTransaccion.AnioComision);
                    cmd.Parameters.AddWithValue("@MontoEfectivo", objTransaccion.MontoEfectivo);
                    cmd.Parameters.AddWithValue("@MontoCheques", objTransaccion.MontoCheques);
                    cmd.Parameters.AddWithValue("@Monto", objTransaccion.Monto);
                    cmd.Parameters.AddWithValue("@CodigoFrecuenciaPago", objTransaccion.CodigoFrecuenciaPago);
                    cmd.Parameters.AddWithValue("@CodigoTipoPago", objTransaccion.CodigoTipoPago);
                    cmd.Parameters.AddWithValue("@CodigoPlanilla", objTransaccion.CodigoPlanilla);
                    cmd.Parameters.AddWithValue("@CodigoPagoPlanilla", objTransaccion.CodigoPagoPlanilla);
                    cmd.Parameters.AddWithValue("@AnioPlanilla", objTransaccion.AnioPlanilla);
                    cmd.Parameters.AddWithValue("@MesPlanilla", objTransaccion.MesPlanilla);
                    cmd.Parameters.AddWithValue("@SemanaPlanilla", objTransaccion.SemanaPlanilla);
                    cmd.Parameters.AddWithValue("@CodigoQuincenaPlanilla", objTransaccion.CodigoQuincenaPlanilla);
                    cmd.Parameters.AddWithValue("@CodigoBonoExtra", objTransaccion.CodigoBonoExtra);
                    cmd.Parameters.AddWithValue("@TipoEspeciales1", objTransaccion.TipoEspeciales1);
                    cmd.Parameters.AddWithValue("@Observaciones", objTransaccion.Observaciones == null ? DBNull.Value : objTransaccion.Observaciones);
                    cmd.Parameters.AddWithValue("@FechaConfirmacion", DBNull.Value);
                    cmd.Parameters.AddWithValue("@MotivoAnulacion", objTransaccion.MotivoAnulacion == null ? DBNull.Value : objTransaccion.MotivoAnulacion);
                    cmd.Parameters.AddWithValue("@UsuarioAnulacion", objTransaccion.UsuarioAnulacion == null ? DBNull.Value : objTransaccion.UsuarioAnulacion);
                    cmd.Parameters.AddWithValue("@FechaAnulacion", objTransaccion.FechaAnulacion == null ? DBNull.Value : objTransaccion.FechaAnulacion);
                    cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.EstadoTransacccion.REGISTRADO);
                    cmd.Parameters.AddWithValue("@UsuarioIng", usuarioIng);
                    cmd.Parameters.AddWithValue("@FechaIng", DateTime.Now);
                    cmd.Parameters.AddWithValue("@FechaAct", DBNull.Value);
                    cmd.Parameters.AddWithValue("@UsuarioAct", DBNull.Value);
                    cmd.Parameters.AddWithValue("@AnioSueldoIndirecto", objTransaccion.AnioSueldoIndirecto == -1 ? 0 : objTransaccion.AnioSueldoIndirecto);
                    cmd.Parameters.AddWithValue("@MesSueldoIndirecto", objTransaccion.MesSueldoIndirecto);
                    cmd.Parameters.AddWithValue("@ComplementoConta", objTransaccion.ComplementoConta);
                    cmd.Parameters.AddWithValue("@CodigoReporte", objTransaccion.CodigoReporte == -1 ? DBNull.Value : objTransaccion.CodigoReporte);
                    cmd.Parameters.AddWithValue("@CodigoTipoDocumentoDeposito", objTransaccion.CodigoTipoDocumentoDeposito);
                    cmd.Parameters.AddWithValue("@NumeroVoucher", objTransaccion.NumeroVoucher == null ? DBNull.Value : objTransaccion.NumeroVoucher);
                    cmd.Parameters.AddWithValue("@NombreProveedor", objTransaccion.NombreProveedor == null ? DBNull.Value : objTransaccion.NombreProveedor);
                    cmd.Parameters.AddWithValue("@CodigoCanalVenta", objTransaccion.CodigoCanalVenta);
                    cmd.Parameters.AddWithValue("@CodigoOtroIngreso", objTransaccion.CodigoOtroIngreso == -1 ? 0 : objTransaccion.CodigoOtroIngreso);
                    cmd.Parameters.AddWithValue("@NumeroReciboReferencia", correlativoReciboReferencia);
                    cmd.Parameters.AddWithValue("@MontoSaldoAnteriorCuentaPorCobrar", objTransaccion.MontoSaldoAnteriorCxC);
                    cmd.Parameters.AddWithValue("@MontoSaldoActualCuentaPorCobrar", objTransaccion.MontoSaldoActualCxC);


                    cmd.ExecuteNonQuery();

                    #region Registro en Cuenta Corriente

                    if (objTransaccion.CodigoOperacion == Constantes.CuentaPorCobrar.Operacion.ANTICIPO_LIQUIDABLE ||
                        objTransaccion.CodigoOperacion == Constantes.CuentaPorCobrar.Operacion.DEVOLUCION_ANTICIPO_LIQUIDABLE ||
                        objTransaccion.CodigoOperacion == Constantes.CuentaPorCobrar.Operacion.ANTICIPO_SALARIO ||
                        objTransaccion.CodigoOperacion == Constantes.CuentaPorCobrar.Operacion.DESCUENTO_PLANILLA_POR_ANTICIPO_SALARIO ||
                        objTransaccion.CodigoOperacion == Constantes.CuentaPorCobrar.Operacion.PRESTAMO ||
                        objTransaccion.CodigoOperacion == Constantes.CuentaPorCobrar.Operacion.ABONO_PRESTAMO ||
                        objTransaccion.CodigoOperacion == Constantes.CuentaPorCobrar.Operacion.BACK_TO_BACK ||
                        objTransaccion.CodigoOperacion == Constantes.CuentaPorCobrar.Operacion.RETIRO_SOCIOS ||
                        objTransaccion.CodigoOperacion == Constantes.CuentaPorCobrar.Operacion.DEVOLUCION_SOCIOS)
                    {
                        int codigoCategoria = 0;
                        switch (objTransaccion.CodigoCategoriaEntidad)
                        {
                            case Constantes.Entidad.Categoria.VENDEDOR:
                                codigoCategoria = Constantes.Entidad.Categoria.VENDEDORES;
                                break;
                            case Constantes.Entidad.Categoria.RUTERO_LOCAL:
                                codigoCategoria = Constantes.Entidad.Categoria.VENDEDORES;
                                break;
                            case Constantes.Entidad.Categoria.RUTERO_INTERIOR:
                                codigoCategoria = Constantes.Entidad.Categoria.VENDEDORES;
                                break;
                            case Constantes.Entidad.Categoria.CAFETERIA:
                                codigoCategoria = Constantes.Entidad.Categoria.VENDEDORES;
                                break;
                            case Constantes.Entidad.Categoria.SUPERMERCADO:
                                codigoCategoria = Constantes.Entidad.Categoria.VENDEDORES;
                                break;
                            default:
                                codigoCategoria = objTransaccion.CodigoCategoriaEntidad;
                                break;
                        }
                        sqlSequence = "SELECT NEXT VALUE FOR db_contabilidad.SQ_CUENTA_POR_COBRAR";
                        cmd.CommandText = sqlSequence;
                        long codigoCuentaPorCobrar = (long)cmd.ExecuteScalar();

                        sentenciaSQL = @"
                        INSERT INTO db_contabilidad.cuenta_por_cobrar(codigo_cxc,codigo_tipo_cxc,codigo_categoria_entidad,codigo_categoria,codigo_entidad,nombre_entidad,fecha_prestamo,fecha_inicio_pago,anio_operacion,semana_operacion,monto,observaciones,codigo_transaccion,codigo_planilla,codigo_operacion,codigo_estado,usuario_ing,fecha_ing,usuario_act,fecha_act,carga_inicial)
                        VALUES( @CodigoCuentaCobrar,
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
                                @CodigoPlanillaDescuento,
                                @CodigoOperacion,
                                @CodigoEstadoCxC,
                                @UsuarioIng,
                                @FechaIng,
                                @UsuarioAct,
                                @FechaAct,
                                @CargaInicial)";

                        cmd.CommandText = sentenciaSQL;
                        cmd.Parameters.AddWithValue("@CodigoCuentaCobrar", codigoCuentaPorCobrar);
                        //cmd.Parameters.AddWithValue("@CodigoTipoCuentaPorCobrar", objTransaccion.CodigoTipoCuentaPorCobrar);
                        //cmd.Parameters.AddWithValue("@CodigoCategoriaEntidad", objTransaccion.CodigoCategoriaEntidad);
                        cmd.Parameters.AddWithValue("@CodigoCategoria", codigoCategoria);
                        //cmd.Parameters.AddWithValue("@CodigoEntidad", objTransaccion.CodigoEntidad);
                        cmd.Parameters.AddWithValue("@NombreEntidad", objTransaccion.NombreEntidad);
                        cmd.Parameters.AddWithValue("@FechaPrestamo", objTransaccion.FechaPrestamo == null ? DBNull.Value : objTransaccion.FechaPrestamo);
                        cmd.Parameters.AddWithValue("@FechaInicioPago", objTransaccion.FechaInicioPago == null ? DBNull.Value : objTransaccion.FechaInicioPago);
                        //cmd.Parameters.AddWithValue("@AnioOperacion", objTransaccion.AnioOperacion);
                        //cmd.Parameters.AddWithValue("@SemanaOperacion", objTransaccion.SemanaOperacion);
                        //cmd.Parameters.AddWithValue("@Monto", objTransaccion.Monto);
                        //cmd.Parameters.AddWithValue("@Observaciones", objTransaccion.Observaciones == null ? DBNull.Value : objTransaccion.Observaciones);
                        //cmd.Parameters.AddWithValue("@codigoTransaccion", codigoTransaccion);
                        cmd.Parameters.AddWithValue("@CodigoPlanillaDescuento", DBNull.Value);
                        //cmd.Parameters.AddWithValue("@CodigoOperacion", objTransaccion.CodigoOperacion);
                        cmd.Parameters.AddWithValue("@CodigoEstadoCxC", Constantes.CuentaPorCobrar.Estado.REGISTRADO);
                        //cmd.Parameters.AddWithValue("@UsuarioIng", usuarioIng);
                        //cmd.Parameters.AddWithValue("@FechaIng", DateTime.Now);
                        //cmd.Parameters.AddWithValue("@UsuarioAct", DBNull.Value);
                        //cmd.Parameters.AddWithValue("@FechaAct", DBNull.Value);
                        cmd.Parameters.AddWithValue("@CargaInicial", 0);
                        cmd.ExecuteNonQuery();

                        sentenciaSQL = @"
                        UPDATE db_tesoreria.transaccion 
                        SET codigo_cxc = @CodigoCxC
                        WHERE codigo_transaccion = @CodigoTrans";

                        cmd.CommandText = sentenciaSQL;
                        cmd.Parameters.AddWithValue("@CodigoTrans", codigoTransaccion);
                        cmd.Parameters.AddWithValue("@CodigoCxC", codigoCuentaPorCobrar);
                        cmd.ExecuteNonQuery();

                    }// fin if

                    #endregion

                    cmd.CommandText = sentenciaUpdateTransaccion;
                    cmd.Parameters.AddWithValue("@siguienteValor", correlativoTransaccion + 1);
                    cmd.ExecuteNonQuery();

                    //if (objTransaccion.NumeroRecibo == -1)
                    //{
                        //cmd.CommandText = sentenciaUpdateRecibo;
                        //cmd.Parameters.AddWithValue("@siguienteValorRecibo", correlativoRecibo + 1);
                        //cmd.ExecuteNonQuery();
                    //}

                    // Attempt to commit the transaction.
                    transaction.Commit();
                    conexion.Close();

                    //resultado = "OK";
                    resultado = codigoTransaccion.ToString();
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

        public string ActualizarTransaccion(TransaccionCLS objTransaccion, string usuarioAct)
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
                    string sqlSequence = "SELECT sig_valor FROM db_admon.secuencia_detalle WHERE codigo_secuencia = @CodigoSecuencia AND anio = @AnioTransaccion";

                    // ExecuteScalar(), Executes the query, and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.
                    cmd.CommandText = sqlSequence;
                    cmd.Parameters.AddWithValue("@CodigoSecuencia", Constantes.Secuencia.SIT_SEQ_TRANSACCION);
                    cmd.Parameters.AddWithValue("@AnioTransaccion", anio);
                    long correlativoTransaccion = (long)cmd.ExecuteScalar();

                    string sentenciaUpdateTransaccion = string.Empty;
                    string sentenciaUpdateRecibo = string.Empty;

                    //long correlativoRecibo = objTransaccion.NumeroRecibo;
                    //long correlativoReciboReferencia = objTransaccion.NumeroReciboReferencia;

                    /*long correlativoRecibo = 0;
                    if (objTransaccion.NumeroRecibo == -1)
                    {
                        sqlSequence = "SELECT sig_valor FROM db_admon.secuencia_detalle WHERE codigo_secuencia = @CodigoSecuenciaRecibo AND anio = @AnioRecibo";
                        cmd.CommandText = sqlSequence;
                        if (objTransaccion.CodigoTipoOperacion == 1)
                        { // Ingreso
                            cmd.Parameters.AddWithValue("@CodigoSecuenciaRecibo", Constantes.Secuencia.SIT_SEQ_RECIBO_INGRESO);
                        }
                        else
                        { // Egreso
                            cmd.Parameters.AddWithValue("@CodigoSecuenciaRecibo", Constantes.Secuencia.SIT_SEQ_RECIBO_EGRESO);
                        }
                        cmd.Parameters.AddWithValue("@AnioRecibo", anio);
                        correlativoRecibo = (long)cmd.ExecuteScalar();
                        sentenciaUpdateRecibo = "UPDATE db_admon.secuencia_detalle SET sig_valor = @siguienteValorRecibo WHERE codigo_secuencia = @CodigoSecuenciaRecibo  AND anio = @AnioRecibo";
                    }
                    else
                    {
                        correlativoRecibo = objTransaccion.NumeroRecibo;
                    }*/

                    sentenciaUpdateTransaccion = "UPDATE db_admon.secuencia_detalle SET sig_valor = @siguienteValor WHERE codigo_secuencia = @CodigoSecuencia  AND anio = @AnioTransaccion";
                    long codigoTransaccion = long.Parse(anio.ToString() + correlativoTransaccion.ToString("D6"));

                    string sentenciaSQL = @"
                    INSERT INTO db_tesoreria.transaccion( codigo_transaccion,
                                                          codigo_seguridad,  
                                                          codigo_empresa,
                                                          codigo_operacion,
                                                          codigo_operacion_caja,
                                                          codigo_tipo_cxc,
                                                          codigo_cxc,
                                                          codigo_area,
                                                          codigo_categoria_entidad,
                                                          codigo_entidad,
                                                          codigo_tipo_operacion,  
                                                          codigo_tipo_transaccion,
                                                          codigo_tipo_documento,  
                                                          efectivo,
                                                          deposito,
                                                          cheque,  
                                                          nit_proveedor,  
                                                          serie_factura,
                                                          numero_documento,
                                                          fecha_documento,
                                                          conceder_iva,
                                                          nit_empresa_concede_iva,  
                                                          codigo_banco_deposito,
                                                          numero_cuenta,  
                                                          numero_boleta, 
                                                          numero_recibo,
                                                          fecha_recibo,
                                                          fecha_operacion,
                                                          anio_operacion,
                                                          semana_operacion,
                                                          dia_operacion,
                                                          codigo_boleta_comision,  
                                                          ruta,
                                                          codigo_vendedor,  
                                                          semana_comision,
                                                          anio_comision,
                                                          monto_efectivo,
                                                          monto_cheques,  
                                                          monto,
                                                          codigo_frecuencia_pago,  
                                                          codigo_tipo_pago,		
	                                                      codigo_planilla,
	                                                      codigo_pago_planilla,
	                                                      anio_planilla,	
	                                                      mes_planilla,		
	                                                      semana_planilla,		
	                                                      codigo_quincena_planilla,
                                                          codigo_bono_extra,  
                                                          tipo_especiales_1,
                                                          observaciones,
                                                          fecha_confirmacion,
                                                          motivo_anulacion,
                                                          usuario_anulacion,
                                                          fecha_anulacion,  
                                                          codigo_estado,
                                                          usuario_ing,
                                                          fecha_ing,
                                                          usuario_act,
                                                          fecha_act,
                                                          anio_sueldo_indirecto,
                                                          mes_sueldo_indirecto,
                                                          complemento_conta,
                                                          codigo_reporte,
                                                          codigo_tipo_doc_deposito,
                                                          numero_voucher,  
                                                          nombre_proveedor,  
                                                          codigo_transaccion_ant, 
                                                          codigo_canal_venta,
                                                          codigo_otro_ingreso,
                                                          numero_recibo_referencia,
                                                          monto_saldo_anterior_cxc,
                                                          monto_saldo_actual_cxc)
                    VALUES(@CodigoTransaccion,
                           @CodigoSeguridad,
                           @CodigoEmpresa,
                           @CodigoOperacion,
                           @CodigoOperacionCaja,
                           @CodigoTipoCuentaPorCobrar,
                           @CodigoCuentaPorCobrar,
                           @CodigoArea,
                           @CodigoCategoriaEntidad,
                           @CodigoEntidad,
                           @CodigoTipoOperacion, 
                           @CodigoTipoTransaccion,
                           @CodigoTipoDocumento, 
                           @Efectivo,
                           @Deposito,
                           @Cheque, 
                           @NitProveedor, 
                           @SerieFactura,
                           @NumeroDocumento,
                           @FechaDocumento,
                           @ConcederIva,
                           @NitEmpresaConcedeIva, 
                           @CodigoBancoDeposito,
                           @NumeroCuenta,
                           @NumeroBoleta, 
                           @NumeroRecibo,
                           @FechaRecibo,
                           @FechaOperacion,
                           @AnioOperacion,
                           @SemanaOperacion,
                           @DiaOperacion,
                           @CodigoBoletaComision,
                           @Ruta,
                           @CodigoVendedor,
                           @SemanaComision, 
                           @AnioComision,
                           @MontoEfectivo,
                           @MontoCheques, 
                           @Monto,
                           @CodigoFrecuenciaPago, 
                           @CodigoTipoPago,
                           @CodigoPlanilla,
                           @CodigoPagoPlanilla,
                           @AnioPlanilla,
                           @MesPlanilla,
                           @SemanaPlanilla,
                           @CodigoQuincenaPlanilla, 
                           @CodigoBonoExtra,
                           @TipoEspeciales1,
                           @Observaciones,
                           @FechaConfirmacion,
                           @MotivoAnulacion,
                           @UsuarioAnulacion,
                           @FechaAnulacion, 
                           @CodigoEstado,
                           @UsuarioIng,
                           @FechaIng,
                           @UsuarioAct,
                           @FechaAct,
                           @AnioSueldoIndirecto,
                           @MesSueldoIndirecto,
                           @ComplementoConta,
                           @CodigoReporte,
                           @CodigoTipoDocumentoDeposito,
                           @NumeroVoucher, 
                           @NombreProveedor, 
                           @CodigoTransaccionAnt,
                           @CodigoCanalVenta,
                           @CodigoOtroIngreso,
                           @NumeroReciboReferencia,
                           @MontoSaldoAnteriorCxC,
                           @MontoSaldoActualCxC)";

                    cmd.CommandText = sentenciaSQL;
                    cmd.Parameters.AddWithValue("@CodigoTransaccion", codigoTransaccion);
                    cmd.Parameters.AddWithValue("@CodigoSeguridad", codigoSeguridad);
                    cmd.Parameters.AddWithValue("@CodigoEmpresa", objTransaccion.CodigoEmpresa == null ? DBNull.Value : objTransaccion.CodigoEmpresa);
                    cmd.Parameters.AddWithValue("@CodigoOperacion", objTransaccion.CodigoOperacion);
                    cmd.Parameters.AddWithValue("@CodigoOperacionCaja", objTransaccion.CodigoOperacionCaja);
                    cmd.Parameters.AddWithValue("@CodigoTipoCuentaPorCobrar", objTransaccion.CodigoTipoCuentaPorCobrar);
                    cmd.Parameters.AddWithValue("@CodigoCuentaPorCobrar", objTransaccion.CodigoCuentaPorCobrar == null ? DBNull.Value : objTransaccion.CodigoCuentaPorCobrar);
                    cmd.Parameters.AddWithValue("@CodigoArea", objTransaccion.CodigoArea);
                    cmd.Parameters.AddWithValue("@CodigoCategoriaEntidad", objTransaccion.CodigoCategoriaEntidad);
                    cmd.Parameters.AddWithValue("@CodigoEntidad", objTransaccion.CodigoEntidad);
                    cmd.Parameters.AddWithValue("@CodigoTipoOperacion", objTransaccion.CodigoTipoOperacion);
                    cmd.Parameters.AddWithValue("@CodigoTipoTransaccion", objTransaccion.CodigoTipoTransaccion);
                    cmd.Parameters.AddWithValue("@CodigoTipoDocumento", objTransaccion.CodigoTipoDocumento);
                    cmd.Parameters.AddWithValue("@Efectivo", objTransaccion.Efectivo);
                    cmd.Parameters.AddWithValue("@Deposito", objTransaccion.Deposito);
                    cmd.Parameters.AddWithValue("@Cheque", objTransaccion.Cheque);
                    cmd.Parameters.AddWithValue("@NitProveedor", objTransaccion.NitProveedor == null ? DBNull.Value : objTransaccion.NitProveedor);
                    cmd.Parameters.AddWithValue("@SerieFactura", objTransaccion.SerieFactura == null ? DBNull.Value : objTransaccion.SerieFactura);
                    cmd.Parameters.AddWithValue("@NumeroDocumento", objTransaccion.NumeroDocumento == null ? DBNull.Value : objTransaccion.NumeroDocumento);
                    cmd.Parameters.AddWithValue("@FechaDocumento", objTransaccion.FechaDocumento == null ? DBNull.Value : objTransaccion.FechaDocumento);
                    cmd.Parameters.AddWithValue("@ConcederIva", objTransaccion.ConcederIva);
                    cmd.Parameters.AddWithValue("@NitEmpresaConcedeIva", objTransaccion.NitEmpresaConcedeIva == null ? DBNull.Value : objTransaccion.NitEmpresaConcedeIva);
                    cmd.Parameters.AddWithValue("@CodigoBancoDeposito", objTransaccion.CodigoBancoDeposito == null ? DBNull.Value : objTransaccion.CodigoBancoDeposito);
                    cmd.Parameters.AddWithValue("@NumeroCuenta", objTransaccion.NumeroCuenta == null ? DBNull.Value : objTransaccion.NumeroCuenta);
                    cmd.Parameters.AddWithValue("@NumeroBoleta", objTransaccion.NumeroBoleta == null ? DBNull.Value : objTransaccion.NumeroBoleta);
                    cmd.Parameters.AddWithValue("@NumeroRecibo", objTransaccion.NumeroRecibo);
                    cmd.Parameters.AddWithValue("@FechaRecibo", objTransaccion.FechaRecibo);
                    cmd.Parameters.AddWithValue("@FechaOperacion", objTransaccion.FechaOperacion);
                    cmd.Parameters.AddWithValue("@AnioOperacion", objTransaccion.AnioOperacion);
                    cmd.Parameters.AddWithValue("@SemanaOperacion", objTransaccion.SemanaOperacion);
                    cmd.Parameters.AddWithValue("@DiaOperacion", Util.Conversion.DayOfWeek(objTransaccion.FechaOperacion));
                    cmd.Parameters.AddWithValue("@CodigoBoletaComision", objTransaccion.CodigoBoletaComision == null ? DBNull.Value : objTransaccion.CodigoBoletaComision);
                    cmd.Parameters.AddWithValue("@Ruta", objTransaccion.Ruta);
                    cmd.Parameters.AddWithValue("@CodigoVendedor", objTransaccion.CodigoVendedor == null ? DBNull.Value : objTransaccion.CodigoVendedor);
                    cmd.Parameters.AddWithValue("@SemanaComision", objTransaccion.SemanaComision);
                    cmd.Parameters.AddWithValue("@AnioComision", objTransaccion.AnioComision);
                    cmd.Parameters.AddWithValue("@MontoEfectivo", objTransaccion.MontoEfectivo);
                    cmd.Parameters.AddWithValue("@MontoCheques", objTransaccion.MontoCheques);
                    cmd.Parameters.AddWithValue("@Monto", objTransaccion.Monto);
                    cmd.Parameters.AddWithValue("@CodigoFrecuenciaPago", objTransaccion.CodigoFrecuenciaPago);
                    cmd.Parameters.AddWithValue("@CodigoTipoPago", objTransaccion.CodigoTipoPago);
                    cmd.Parameters.AddWithValue("@CodigoPlanilla", objTransaccion.CodigoPlanilla);
                    cmd.Parameters.AddWithValue("@CodigoPagoPlanilla", objTransaccion.CodigoPagoPlanilla);
                    cmd.Parameters.AddWithValue("@AnioPlanilla", objTransaccion.AnioPlanilla);
                    cmd.Parameters.AddWithValue("@MesPlanilla", objTransaccion.MesPlanilla);
                    cmd.Parameters.AddWithValue("@SemanaPlanilla", objTransaccion.SemanaPlanilla);
                    cmd.Parameters.AddWithValue("@CodigoQuincenaPlanilla", objTransaccion.CodigoQuincenaPlanilla);
                    cmd.Parameters.AddWithValue("@CodigoBonoExtra", objTransaccion.CodigoBonoExtra);
                    cmd.Parameters.AddWithValue("@TipoEspeciales1", objTransaccion.TipoEspeciales1);
                    cmd.Parameters.AddWithValue("@Observaciones", objTransaccion.Observaciones == null ? DBNull.Value : objTransaccion.Observaciones);
                    cmd.Parameters.AddWithValue("@FechaConfirmacion", DBNull.Value);
                    cmd.Parameters.AddWithValue("@MotivoAnulacion", objTransaccion.MotivoAnulacion == null ? DBNull.Value : objTransaccion.MotivoAnulacion);
                    cmd.Parameters.AddWithValue("@UsuarioAnulacion", objTransaccion.UsuarioAnulacion == null ? DBNull.Value : objTransaccion.UsuarioAnulacion);
                    cmd.Parameters.AddWithValue("@FechaAnulacion", objTransaccion.FechaAnulacion == null ? DBNull.Value : objTransaccion.FechaAnulacion);
                    cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.EstadoTransacccion.REGISTRADO);
                    cmd.Parameters.AddWithValue("@UsuarioIng", usuarioAct);
                    cmd.Parameters.AddWithValue("@FechaIng", DateTime.Now);
                    cmd.Parameters.AddWithValue("@FechaAct", DBNull.Value);
                    cmd.Parameters.AddWithValue("@UsuarioAct", DBNull.Value);
                    cmd.Parameters.AddWithValue("@AnioSueldoIndirecto", objTransaccion.AnioSueldoIndirecto == -1 ? 0 : objTransaccion.AnioSueldoIndirecto);
                    cmd.Parameters.AddWithValue("@MesSueldoIndirecto", objTransaccion.MesSueldoIndirecto);
                    cmd.Parameters.AddWithValue("@ComplementoConta", objTransaccion.ComplementoConta);
                    cmd.Parameters.AddWithValue("@CodigoReporte", DBNull.Value);
                    cmd.Parameters.AddWithValue("@CodigoTipoDocumentoDeposito", objTransaccion.CodigoTipoDocumentoDeposito);
                    cmd.Parameters.AddWithValue("@NumeroVoucher", objTransaccion.NumeroVoucher == null ? DBNull.Value : objTransaccion.NumeroVoucher);
                    cmd.Parameters.AddWithValue("@NombreProveedor", objTransaccion.NombreProveedor == null ? DBNull.Value : objTransaccion.NombreProveedor);
                    cmd.Parameters.AddWithValue("@CodigoTransaccionAnt", objTransaccion.CodigoTransaccion);
                    cmd.Parameters.AddWithValue("@CodigoCanalVenta", objTransaccion.CodigoCanalVenta);
                    cmd.Parameters.AddWithValue("@CodigoOtroIngreso", objTransaccion.CodigoOtroIngreso == -1 ? 0 : objTransaccion.CodigoOtroIngreso);
                    cmd.Parameters.AddWithValue("@NumeroReciboReferencia", objTransaccion.NumeroReciboReferencia);
                    cmd.Parameters.AddWithValue("@MontoSaldoAnteriorCxC", objTransaccion.MontoSaldoAnteriorCxC);
                    cmd.Parameters.AddWithValue("@MontoSaldoActualCxC", objTransaccion.MontoSaldoActualCxC);

                    cmd.ExecuteNonQuery();

                    #region Registro en Cuenta Corriente

                    if (objTransaccion.CodigoOperacion == Constantes.CuentaPorCobrar.Operacion.ANTICIPO_LIQUIDABLE ||
                        objTransaccion.CodigoOperacion == Constantes.CuentaPorCobrar.Operacion.DEVOLUCION_ANTICIPO_LIQUIDABLE ||
                        objTransaccion.CodigoOperacion == Constantes.CuentaPorCobrar.Operacion.ANTICIPO_SALARIO ||
                        objTransaccion.CodigoOperacion == Constantes.CuentaPorCobrar.Operacion.DESCUENTO_PLANILLA_POR_ANTICIPO_SALARIO ||
                        objTransaccion.CodigoOperacion == Constantes.CuentaPorCobrar.Operacion.PRESTAMO ||
                        objTransaccion.CodigoOperacion == Constantes.CuentaPorCobrar.Operacion.ABONO_PRESTAMO ||
                        objTransaccion.CodigoOperacion == Constantes.CuentaPorCobrar.Operacion.BACK_TO_BACK ||
                        objTransaccion.CodigoOperacion == Constantes.CuentaPorCobrar.Operacion.RETIRO_SOCIOS ||
                        objTransaccion.CodigoOperacion == Constantes.CuentaPorCobrar.Operacion.DEVOLUCION_SOCIOS)
                    {
                        int codigoCategoria = 0;
                        switch (objTransaccion.CodigoCategoriaEntidad)
                        {
                            case Constantes.Entidad.Categoria.VENDEDOR:
                                codigoCategoria = Constantes.Entidad.Categoria.VENDEDORES;
                                break;
                            case Constantes.Entidad.Categoria.RUTERO_LOCAL:
                                codigoCategoria = Constantes.Entidad.Categoria.VENDEDORES;
                                break;
                            case Constantes.Entidad.Categoria.RUTERO_INTERIOR:
                                codigoCategoria = Constantes.Entidad.Categoria.VENDEDORES;
                                break;
                            case Constantes.Entidad.Categoria.CAFETERIA:
                                codigoCategoria = Constantes.Entidad.Categoria.VENDEDORES;
                                break;
                            case Constantes.Entidad.Categoria.SUPERMERCADO:
                                codigoCategoria = Constantes.Entidad.Categoria.VENDEDORES;
                                break;
                            default:
                                codigoCategoria = objTransaccion.CodigoCategoriaEntidad;
                                break;
                        }
                        sqlSequence = "SELECT NEXT VALUE FOR db_contabilidad.SQ_CUENTA_POR_COBRAR";
                        cmd.CommandText = sqlSequence;
                        long codigoCuentaPorCobrar = (long)cmd.ExecuteScalar();

                        sentenciaSQL = @"
                        INSERT INTO db_contabilidad.cuenta_por_cobrar(codigo_cxc,codigo_tipo_cxc,codigo_categoria_entidad,codigo_categoria,codigo_entidad,nombre_entidad,fecha_prestamo,fecha_inicio_pago,anio_operacion,semana_operacion,monto,observaciones,codigo_transaccion,codigo_planilla,codigo_operacion,codigo_estado,usuario_ing,fecha_ing,usuario_act,fecha_act,carga_inicial)
                        VALUES( @CodigoCuentaCobrar,
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
                                @CodigoPlanillaDescuento,
                                @CodigoOperacion,
                                @CodigoEstadoCxC,
                                @UsuarioIng,
                                @FechaIng,
                                @UsuarioAct,
                                @FechaAct,
                                @CargaInicial)";

                        cmd.CommandText = sentenciaSQL;
                        cmd.Parameters.AddWithValue("@CodigoCuentaCobrar", codigoCuentaPorCobrar);
                        cmd.Parameters.AddWithValue("@CodigoCategoria", codigoCategoria);
                        cmd.Parameters.AddWithValue("@NombreEntidad", objTransaccion.NombreEntidad);
                        cmd.Parameters.AddWithValue("@FechaPrestamo", objTransaccion.FechaPrestamo == null ? DBNull.Value : objTransaccion.FechaPrestamo);
                        cmd.Parameters.AddWithValue("@FechaInicioPago", objTransaccion.FechaInicioPago == null ? DBNull.Value : objTransaccion.FechaInicioPago);
                        cmd.Parameters.AddWithValue("@CodigoPlanillaDescuento", DBNull.Value);
                        cmd.Parameters.AddWithValue("@CodigoEstadoCxC", Constantes.CuentaPorCobrar.Estado.REGISTRADO);
                        cmd.Parameters.AddWithValue("@CargaInicial", 0);

                        cmd.Parameters["@CodigoTipoCuentaPorCobrar"].Value = objTransaccion.CodigoTipoCuentaPorCobrar;
                        cmd.Parameters["@CodigoCategoriaEntidad"].Value = objTransaccion.CodigoCategoriaEntidad;
                        cmd.Parameters["@CodigoEntidad"].Value = objTransaccion.CodigoEntidad;
                        cmd.Parameters["@AnioOperacion"].Value = objTransaccion.AnioOperacion;
                        cmd.Parameters["@SemanaOperacion"].Value = objTransaccion.SemanaOperacion;
                        cmd.Parameters["@Monto"].Value = objTransaccion.Monto;
                        cmd.Parameters["@Observaciones"].Value = objTransaccion.Observaciones == null ? DBNull.Value : objTransaccion.Observaciones;
                        cmd.Parameters["@codigoTransaccion"].Value = codigoTransaccion;
                        cmd.Parameters["@CodigoOperacion"].Value = objTransaccion.CodigoOperacion;
                        cmd.Parameters["@UsuarioIng"].Value = usuarioAct;
                        cmd.Parameters["@FechaIng"].Value = DateTime.Now;
                        cmd.Parameters["@UsuarioAct"].Value = DBNull.Value;
                        cmd.Parameters["@FechaAct"].Value = DBNull.Value;
                        cmd.ExecuteNonQuery();

                        sentenciaSQL = @"
                        UPDATE db_tesoreria.transaccion 
                        SET codigo_cxc = @CodigoCxC
                        WHERE codigo_transaccion = @CodigoTrans";

                        cmd.CommandText = sentenciaSQL;
                        cmd.Parameters.AddWithValue("@CodigoTrans", codigoTransaccion);
                        cmd.Parameters.AddWithValue("@CodigoCxC", codigoCuentaPorCobrar);
                        cmd.ExecuteNonQuery();
                    }// fin if

                    // Cambiando el estado de la transacción que se está editando
                    sentenciaSQL = @"
                    UPDATE db_tesoreria.transaccion
                    SET codigo_estado = @CodigoEstadoAnulado,
                        usuario_act = @UsuarioAct,
                        fecha_act = @FechaAct
                    WHERE codigo_transaccion = @CodigoTransaccionAnt";

                    cmd.CommandText = sentenciaSQL;
                    cmd.Parameters.AddWithValue("@CodigoEstadoAnulado", Constantes.EstadoTransacccion.ANULADO);
                    cmd.Parameters["@CodigoTransaccionAnt"].Value = objTransaccion.CodigoTransaccion;
                    cmd.Parameters["@UsuarioAct"].Value = usuarioAct;
                    cmd.Parameters["@FechaAct"].Value = DateTime.Now;
                    cmd.ExecuteNonQuery();

                    // Cambiando el estado de cuentas por cobrar, de la transacción anulada
                    sentenciaSQL = @"
                    UPDATE db_contabilidad.cuenta_por_cobrar
                    SET codigo_estado = @CodigoEstadoAnulado,
                        usuario_act = @UsuarioAct,
                        fecha_act = @FechaAct
                    WHERE codigo_transaccion = @CodigoTransaccionAnt";

                    cmd.CommandText = sentenciaSQL;
                    cmd.Parameters["@CodigoEstadoAnulado"].Value = Constantes.CuentaPorCobrar.Estado.ANULADO;
                    cmd.Parameters["@CodigoTransaccionAnt"].Value = objTransaccion.CodigoTransaccion;
                    cmd.Parameters["@UsuarioAct"].Value = usuarioAct;
                    cmd.Parameters["@FechaAct"].Value = DateTime.Now;
                    cmd.ExecuteNonQuery();

                    #endregion

                    cmd.CommandText = sentenciaUpdateTransaccion;
                    cmd.Parameters.AddWithValue("@siguienteValor", correlativoTransaccion + 1);
                    cmd.ExecuteNonQuery();

                    //if (objTransaccion.NumeroRecibo == -1)
                    //{
                    //    cmd.CommandText = sentenciaUpdateRecibo;
                    //    cmd.Parameters.AddWithValue("@siguienteValorRecibo", correlativoRecibo + 1);
                    //    cmd.ExecuteNonQuery();
                    //}

                    transaction.Commit();
                    conexion.Close();

                    resultado = codigoTransaccion.ToString();
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

        static async Task SearchContentAsync(string conn, long codigoTransaccion, int codigoTipoCorreccion, string observaciones, string usuarioIng)
        {
            List<PersonaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(conn))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT  correo_electronico
                    FROM db_admon.persona_notificacion x
                    INNER JOIN db_rrhh.persona y
                    ON x.cui = y.cui
                    WHERE codigo_tipo_notificacion = @CodigoTipoNotificacion";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoTipoNotificacion", Constantes.TipoNotificacion.CORRECCION_OPERACION_CAJA_TESORERIA);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            PersonaCLS objPersona;
                            lista = new List<PersonaCLS>();
                            int postCorreoElectronico = dr.GetOrdinal("correo_electronico");
                            while (dr.Read())
                            {
                                objPersona = new PersonaCLS();
                                if (dr.GetString(postCorreoElectronico) != String.Empty)
                                { 
                                    objPersona.CorreoElectronico = dr.GetString(postCorreoElectronico);
                                    lista.Add(objPersona);
                                }
                            }// fin whiel
                        }
                    }
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    lista = null;
                }
            }

            string tipoCorreccion = String.Empty;
            switch (codigoTipoCorreccion)
            {
                case Constantes.Correccion.TipoCorreccion.MODIFICACION:
                    tipoCorreccion = "MODIFICACIÓN";
                    break;
                case Constantes.Correccion.TipoCorreccion.ANULACION:
                    tipoCorreccion = "ANULACIÓN";
                    break;
                default:
                    tipoCorreccion = "NO DEFINIDO";
                    break;
            }

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("webmail.americana.com.gt");
            mail.From = new MailAddress("notificaciones_tesoreria@americana.com.gt");
            if (lista == null)
            {
                mail.To.Add("notificaciones_tesoreria@americana.com.gt");
                mail.Subject = "Error de envío de Notificación de solicitud corrección de transacción" + codigoTransaccion.ToString() + " de caja de tesorería";
            }
            else {
                foreach (PersonaCLS objPersona in lista)
                {
                    mail.To.Add(objPersona.CorreoElectronico);
                }
                mail.Subject = "Notificación de solicitud corrección de transacción" + codigoTransaccion.ToString() + " de caja de tesorería";
            }
            //MailMessage message = new MailMessage(from, to);
            //MailAddress bcc = new MailAddress("manager1@contoso.com");
            //mail.Bcc.Add(bcc);
            //mail.Body = "This is for testing SMTP mail from GMAIL";
            mail.IsBodyHtml = true;
            mail.Body = @"
                    <p>Notificación de Corrección de transacciones de tesorería</p>
                    <br />
                    <p>Se ha realizado una solicitud para corregir una transacción</p>
                    <br />
                    <H2>Datos de solicitud de corrección</H2>
                    <table border='1'>
                        <tr>
                            <th>CÓDIGO TRANSACCIÓN</th>
                            <th>MOTIVO</th>
                            <th>TIPO DE CORRECCIÓN</th>
                            <th>SOLICITANTE</th>
                            <th>FECHA SOLICITUD</th>
                        </tr>
                        <tr>
                            <td>" + codigoTransaccion.ToString() + @"</td>
                            <td>" + observaciones + @"</td>
                            <td>" + tipoCorreccion + @"</td>
                            <td> " + usuarioIng + @"</td>
                            <td>" + DateTime.Now.ToString() + @"</td>
                        </tr>
                    </table>
                    <br />
                    <p>Sistema de Notificaciones,<br>Panificadora Americana</br></p>";

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("notificaciones_tesoreria@americana.com.gt", "9a#EO#2TN)");
            SmtpServer.EnableSsl = false;
            await SmtpServer.SendMailAsync(mail);
            //SmtpServer.Send(mail);
         
        }

        public string RegistrarSolicitudAprobacionDeCorreccion(long codigoTransaccion, int codigoTipoCorreccion, string observaciones, string usuarioIng)
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

                    string sentenciaSQL = @"
                    INSERT INTO db_tesoreria.solicitud_correccion(codigo_transaccion,codigo_transaccion_correcta,observaciones_solicitud,observaciones_aprobacion,resultado,usuario_aprobacion,fecha_aprobacion,estado,usuario_ing,fecha_ing,usuario_act,fecha_act,codigo_tipo_correccion)
                    VALUES(@CodigoTransaccion,
                           @CodigoTransaccionCorrecta,
                           @ObservacionesSolicitud,
                           @ObservacionesAprobacion,
                           @Resultado,
                           @UsuarioAprobacion,
                           @FechaAprobacion,
                           @CodigoEstado,
                           @UsuarioIng,
                           @FechaIng,
                           @UsuarioAct,
                           @FechaAct,
                           @CodigoTipoCorreccion)"; 

                    cmd.CommandText = sentenciaSQL;
                    cmd.Parameters.AddWithValue("@CodigoTransaccion", codigoTransaccion);
                    cmd.Parameters.AddWithValue("@CodigoTransaccionCorrecta", DBNull.Value);
                    cmd.Parameters.AddWithValue("@ObservacionesSolicitud", observaciones);
                    cmd.Parameters.AddWithValue("@ObservacionesAprobacion", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Resultado", Constantes.Correccion.ResultadoSolicitudCorreccion.SIN_RESULTADO);
                    cmd.Parameters.AddWithValue("@UsuarioAprobacion", DBNull.Value);
                    cmd.Parameters.AddWithValue("@FechaAprobacion", DBNull.Value);
                    cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.EstadoRegistro.ACTIVO);
                    cmd.Parameters.AddWithValue("@UsuarioIng", usuarioIng);
                    cmd.Parameters.AddWithValue("@FechaIng", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UsuarioAct", DBNull.Value);
                    cmd.Parameters.AddWithValue("@FechaAct", DBNull.Value);
                    cmd.Parameters.AddWithValue("@CodigoTipoCorreccion", codigoTipoCorreccion);
                    cmd.ExecuteNonQuery();

                    sentenciaSQL = @"
                    UPDATE db_tesoreria.transaccion
                    SET correccion = 1,
                        codigo_estado_solicitud_correccion = @CodigoEstadoSolicitudCorreccion
                    WHERE codigo_transaccion = @CodigoTransaccion";
                    cmd.CommandText = sentenciaSQL;
                    cmd.Parameters.AddWithValue("@CodigoEstadoSolicitudCorreccion", Constantes.Correccion.EstadoSolicitudcorreccion.SOLICITA_APROBACION);
                    cmd.Parameters["@CodigoTransaccion"].Value = codigoTransaccion;
                    cmd.ExecuteNonQuery();

                    transaction.Commit();
                    conexion.Close();
                    resultado = "OK";

                    //SearchContentAsync(cadenaTesoreria,codigoTransaccion, codigoTipoCorreccion, observaciones, usuarioIng).Wait();
                    SearchContentAsync(cadenaTesoreria, codigoTransaccion, codigoTipoCorreccion, observaciones, usuarioIng);

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

        public string RegistrarCorreccion(TransaccionCLS objTransaccion, string usuarioAct)
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
                    int rows = 0;
                    string codigoSeguridad = Util.Seguridad.GenerarCadena();
                    int anio = DateTime.Now.Year;
                    string sqlSequence = "SELECT sig_valor FROM db_admon.secuencia_detalle WHERE codigo_secuencia = @CodigoSecuencia AND anio = @AnioTransaccion";

                    // ExecuteScalar(), Executes the query, and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.
                    cmd.CommandText = sqlSequence;
                    cmd.Parameters.AddWithValue("@CodigoSecuencia", Constantes.Secuencia.SIT_SEQ_TRANSACCION);
                    cmd.Parameters.AddWithValue("@AnioTransaccion", anio);
                    long correlativoTransaccion = (long)cmd.ExecuteScalar();
                    string sentenciaUpdateTransaccion = string.Empty;

                    sentenciaUpdateTransaccion = "UPDATE db_admon.secuencia_detalle SET sig_valor = @siguienteValor WHERE codigo_secuencia = @CodigoSecuencia  AND anio = @AnioTransaccion";
                    long codigoTransaccion = long.Parse(anio.ToString() + correlativoTransaccion.ToString("D6"));

                    string sentenciaSQL = @"
                    INSERT INTO db_tesoreria.transaccion( codigo_transaccion,
                                                          codigo_seguridad,  
                                                          codigo_empresa,
                                                          codigo_operacion,
                                                          codigo_operacion_caja,
                                                          codigo_tipo_cxc,
                                                          codigo_cxc,
                                                          codigo_area,
                                                          codigo_categoria_entidad,
                                                          codigo_entidad,
                                                          codigo_tipo_operacion,  
                                                          codigo_tipo_transaccion,
                                                          codigo_tipo_documento,  
                                                          efectivo,
                                                          deposito,
                                                          cheque,  
                                                          nit_proveedor,  
                                                          serie_factura,
                                                          numero_documento,
                                                          fecha_documento,
                                                          conceder_iva,
                                                          nit_empresa_concede_iva,  
                                                          codigo_banco_deposito,
                                                          numero_cuenta,  
                                                          numero_boleta, 
                                                          numero_recibo,
                                                          fecha_recibo,
                                                          fecha_operacion,
                                                          anio_operacion,
                                                          semana_operacion,
                                                          dia_operacion,
                                                          codigo_boleta_comision,  
                                                          ruta,
                                                          codigo_vendedor,  
                                                          semana_comision,
                                                          anio_comision,
                                                          monto_efectivo,
                                                          monto_cheques,  
                                                          monto,
                                                          codigo_frecuencia_pago,  
                                                          codigo_tipo_pago,		
	                                                      codigo_planilla,
	                                                      codigo_pago_planilla,
	                                                      anio_planilla,	
	                                                      mes_planilla,		
	                                                      semana_planilla,		
	                                                      codigo_quincena_planilla,
                                                          codigo_bono_extra,  
                                                          tipo_especiales_1,
                                                          observaciones,
                                                          fecha_confirmacion,
                                                          motivo_anulacion,
                                                          usuario_anulacion,
                                                          fecha_anulacion,  
                                                          codigo_estado,
                                                          usuario_ing,
                                                          fecha_ing,
                                                          usuario_act,
                                                          fecha_act,
                                                          anio_sueldo_indirecto,
                                                          mes_sueldo_indirecto,
                                                          complemento_conta,
                                                          codigo_reporte,
                                                          codigo_tipo_doc_deposito,
                                                          numero_voucher,  
                                                          nombre_proveedor,  
                                                          codigo_transaccion_ant,
                                                          codigo_canal_venta,
                                                          codigo_otro_ingreso,
                                                          numero_recibo_referencia,
                                                          monto_saldo_anterior_cxc,
                                                          monto_saldo_actual_cxc)
                    VALUES(@CodigoTransaccion,
                           @CodigoSeguridad,
                           @CodigoEmpresa,
                           @CodigoOperacion,
                           @CodigoOperacionCaja,
                           @CodigoTipoCuentaPorCobrar,
                           @CodigoCuentaPorCobrar,
                           @CodigoArea,
                           @CodigoCategoriaEntidad,
                           @CodigoEntidad,
                           @CodigoTipoOperacion, 
                           @CodigoTipoTransaccion,
                           @CodigoTipoDocumento, 
                           @Efectivo,
                           @Deposito,
                           @Cheque, 
                           @NitProveedor, 
                           @SerieFactura,
                           @NumeroDocumento,
                           @FechaDocumento,
                           @ConcederIva,
                           @NitEmpresaConcedeIva, 
                           @CodigoBancoDeposito,
                           @NumeroCuenta,
                           @NumeroBoleta, 
                           @NumeroRecibo,
                           @FechaRecibo,
                           @FechaOperacion,
                           @AnioOperacion,
                           @SemanaOperacion,
                           @DiaOperacion,
                           @CodigoBoletaComision,
                           @Ruta,
                           @CodigoVendedor,
                           @SemanaComision, 
                           @AnioComision,
                           @MontoEfectivo,
                           @MontoCheques, 
                           @Monto,
                           @CodigoFrecuenciaPago, 
                           @CodigoTipoPago,
                           @CodigoPlanilla,
                           @CodigoPagoPlanilla,
                           @AnioPlanilla,
                           @MesPlanilla,
                           @SemanaPlanilla,
                           @CodigoQuincenaPlanilla, 
                           @CodigoBonoExtra,
                           @TipoEspeciales1,
                           @Observaciones,
                           @FechaConfirmacion,
                           @MotivoAnulacion,
                           @UsuarioAnulacion,
                           @FechaAnulacion, 
                           @CodigoEstado,
                           @UsuarioIng,
                           @FechaIng,
                           @UsuarioAct,
                           @FechaAct,
                           @AnioSueldoIndirecto,
                           @MesSueldoIndirecto,
                           @ComplementoConta,
                           @CodigoReporte,
                           @CodigoTipoDocumentoDeposito,
                           @NumeroVaucher,
                           @NombreProveedor,
                           @CodigoTransaccionAnt,
                           @CodigoCanalVenta,
                           @CodigoOtroIngreso,
                           @NumeroReciboReferencia,
                           @MontoSaldoAnteriorCxC,
                           @MontoSaldoActualCxC)";

                    cmd.CommandText = sentenciaSQL;
                    cmd.Parameters.AddWithValue("@CodigoTransaccion", codigoTransaccion);
                    cmd.Parameters.AddWithValue("@CodigoSeguridad", codigoSeguridad);
                    cmd.Parameters.AddWithValue("@CodigoEmpresa", objTransaccion.CodigoEmpresa == null ? DBNull.Value : objTransaccion.CodigoEmpresa);
                    cmd.Parameters.AddWithValue("@CodigoOperacion", objTransaccion.CodigoOperacion);
                    cmd.Parameters.AddWithValue("@CodigoOperacionCaja", objTransaccion.CodigoOperacionCaja);
                    cmd.Parameters.AddWithValue("@CodigoTipoCuentaPorCobrar", objTransaccion.CodigoTipoCuentaPorCobrar);
                    cmd.Parameters.AddWithValue("@CodigoCuentaPorCobrar", objTransaccion.CodigoCuentaPorCobrar == null ? DBNull.Value : objTransaccion.CodigoCuentaPorCobrar);
                    cmd.Parameters.AddWithValue("@CodigoArea", objTransaccion.CodigoArea);
                    cmd.Parameters.AddWithValue("@CodigoCategoriaEntidad", objTransaccion.CodigoCategoriaEntidad);
                    cmd.Parameters.AddWithValue("@CodigoEntidad", objTransaccion.CodigoEntidad);
                    cmd.Parameters.AddWithValue("@CodigoTipoOperacion", objTransaccion.CodigoTipoOperacion);
                    cmd.Parameters.AddWithValue("@CodigoTipoTransaccion", objTransaccion.CodigoTipoTransaccion);
                    cmd.Parameters.AddWithValue("@CodigoTipoDocumento", objTransaccion.CodigoTipoDocumento);
                    cmd.Parameters.AddWithValue("@Efectivo", objTransaccion.Efectivo);
                    cmd.Parameters.AddWithValue("@Deposito", objTransaccion.Deposito);
                    cmd.Parameters.AddWithValue("@Cheque", objTransaccion.Cheque);
                    cmd.Parameters.AddWithValue("@NitProveedor", objTransaccion.NitProveedor == null ? DBNull.Value : objTransaccion.NitProveedor);
                    cmd.Parameters.AddWithValue("@SerieFactura", objTransaccion.SerieFactura == null ? DBNull.Value : objTransaccion.SerieFactura);
                    cmd.Parameters.AddWithValue("@NumeroDocumento", objTransaccion.NumeroDocumento == null ? DBNull.Value : objTransaccion.NumeroDocumento);
                    cmd.Parameters.AddWithValue("@FechaDocumento", objTransaccion.FechaDocumento == null ? DBNull.Value : objTransaccion.FechaDocumento);
                    cmd.Parameters.AddWithValue("@ConcederIva", objTransaccion.ConcederIva);
                    cmd.Parameters.AddWithValue("@NitEmpresaConcedeIva", objTransaccion.NitEmpresaConcedeIva == null ? DBNull.Value : objTransaccion.NitEmpresaConcedeIva);
                    cmd.Parameters.AddWithValue("@CodigoBancoDeposito", objTransaccion.CodigoBancoDeposito == null ? DBNull.Value : objTransaccion.CodigoBancoDeposito);
                    cmd.Parameters.AddWithValue("@NumeroCuenta", objTransaccion.NumeroCuenta == null ? DBNull.Value : objTransaccion.NumeroCuenta);
                    cmd.Parameters.AddWithValue("@NumeroBoleta", objTransaccion.NumeroBoleta == null ? DBNull.Value : objTransaccion.NumeroBoleta);
                    cmd.Parameters.AddWithValue("@NumeroRecibo", objTransaccion.NumeroRecibo);
                    cmd.Parameters.AddWithValue("@FechaRecibo", objTransaccion.FechaRecibo);
                    cmd.Parameters.AddWithValue("@FechaOperacion", objTransaccion.FechaOperacion);
                    cmd.Parameters.AddWithValue("@AnioOperacion", objTransaccion.AnioOperacion);
                    cmd.Parameters.AddWithValue("@SemanaOperacion", objTransaccion.SemanaOperacion);
                    cmd.Parameters.AddWithValue("@DiaOperacion", Util.Conversion.DayOfWeek(objTransaccion.FechaOperacion));
                    cmd.Parameters.AddWithValue("@CodigoBoletaComision", objTransaccion.CodigoBoletaComision == null ? DBNull.Value : objTransaccion.CodigoBoletaComision);
                    cmd.Parameters.AddWithValue("@Ruta", objTransaccion.Ruta);
                    cmd.Parameters.AddWithValue("@CodigoVendedor", objTransaccion.CodigoVendedor == null ? DBNull.Value : objTransaccion.CodigoVendedor);
                    cmd.Parameters.AddWithValue("@SemanaComision", objTransaccion.SemanaComision);
                    cmd.Parameters.AddWithValue("@AnioComision", objTransaccion.AnioComision);
                    cmd.Parameters.AddWithValue("@MontoEfectivo", objTransaccion.MontoEfectivo);
                    cmd.Parameters.AddWithValue("@MontoCheques", objTransaccion.MontoCheques);
                    cmd.Parameters.AddWithValue("@Monto", objTransaccion.Monto);
                    cmd.Parameters.AddWithValue("@CodigoFrecuenciaPago", objTransaccion.CodigoFrecuenciaPago);
                    cmd.Parameters.AddWithValue("@CodigoTipoPago", objTransaccion.CodigoTipoPago);
                    cmd.Parameters.AddWithValue("@CodigoPlanilla", objTransaccion.CodigoPlanilla);
                    cmd.Parameters.AddWithValue("@CodigoPagoPlanilla", objTransaccion.CodigoPagoPlanilla);
                    cmd.Parameters.AddWithValue("@AnioPlanilla", objTransaccion.AnioPlanilla);
                    cmd.Parameters.AddWithValue("@MesPlanilla", objTransaccion.MesPlanilla);
                    cmd.Parameters.AddWithValue("@SemanaPlanilla", objTransaccion.SemanaPlanilla);
                    cmd.Parameters.AddWithValue("@CodigoQuincenaPlanilla", objTransaccion.CodigoQuincenaPlanilla);
                    cmd.Parameters.AddWithValue("@CodigoBonoExtra", objTransaccion.CodigoBonoExtra);
                    cmd.Parameters.AddWithValue("@TipoEspeciales1", objTransaccion.TipoEspeciales1);
                    cmd.Parameters.AddWithValue("@Observaciones", objTransaccion.Observaciones == null ? DBNull.Value : objTransaccion.Observaciones);
                    cmd.Parameters.AddWithValue("@FechaConfirmacion", DBNull.Value);
                    cmd.Parameters.AddWithValue("@MotivoAnulacion", objTransaccion.MotivoAnulacion == null ? DBNull.Value : objTransaccion.MotivoAnulacion);
                    cmd.Parameters.AddWithValue("@UsuarioAnulacion", objTransaccion.UsuarioAnulacion == null ? DBNull.Value : objTransaccion.UsuarioAnulacion);
                    cmd.Parameters.AddWithValue("@FechaAnulacion", objTransaccion.FechaAnulacion == null ? DBNull.Value : objTransaccion.FechaAnulacion);
                    cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.EstadoTransacccion.INCLUIDO_EN_REPORTE);
                    cmd.Parameters.AddWithValue("@UsuarioIng", usuarioAct);
                    cmd.Parameters.AddWithValue("@FechaIng", DateTime.Now);
                    cmd.Parameters.AddWithValue("@FechaAct", DBNull.Value);
                    cmd.Parameters.AddWithValue("@UsuarioAct", DBNull.Value);
                    cmd.Parameters.AddWithValue("@AnioSueldoIndirecto", objTransaccion.AnioSueldoIndirecto == -1 ? 0 : objTransaccion.AnioSueldoIndirecto);
                    cmd.Parameters.AddWithValue("@MesSueldoIndirecto", objTransaccion.MesSueldoIndirecto);
                    cmd.Parameters.AddWithValue("@ComplementoConta", objTransaccion.ComplementoConta);
                    cmd.Parameters.AddWithValue("@CodigoReporte", objTransaccion.CodigoReporte);
                    cmd.Parameters.AddWithValue("@CodigoTipoDocumentoDeposito", objTransaccion.CodigoTipoDocumentoDeposito);
                    cmd.Parameters.AddWithValue("@NumeroVaucher", objTransaccion.NumeroVoucher == null ? DBNull.Value : objTransaccion.NumeroVoucher);
                    cmd.Parameters.AddWithValue("@NombreProveedor", objTransaccion.NombreProveedor == null ? DBNull.Value : objTransaccion.NombreProveedor);
                    cmd.Parameters.AddWithValue("@CodigoTransaccionAnt", objTransaccion.CodigoTransaccion);
                    cmd.Parameters.AddWithValue("@CodigoCanalVenta", objTransaccion.CodigoCanalVenta);
                    cmd.Parameters.AddWithValue("@CodigoOtroIngreso", objTransaccion.CodigoOtroIngreso == -1 ? 0 : objTransaccion.CodigoOtroIngreso);
                    cmd.Parameters.AddWithValue("@NumeroReciboReferencia", objTransaccion.NumeroReciboReferencia);
                    cmd.Parameters.AddWithValue("@MontoSaldoAnteriorCxC", objTransaccion.MontoSaldoAnteriorCxC);
                    cmd.Parameters.AddWithValue("@MontoSaldoActualCxC", objTransaccion.MontoSaldoActualCxC);


                    cmd.ExecuteNonQuery();

                    #region Registro en Cuenta Corriente

                    if (objTransaccion.CodigoOperacion == Constantes.CuentaPorCobrar.Operacion.ANTICIPO_LIQUIDABLE ||
                        objTransaccion.CodigoOperacion == Constantes.CuentaPorCobrar.Operacion.DEVOLUCION_ANTICIPO_LIQUIDABLE ||
                        objTransaccion.CodigoOperacion == Constantes.CuentaPorCobrar.Operacion.ANTICIPO_SALARIO ||
                        objTransaccion.CodigoOperacion == Constantes.CuentaPorCobrar.Operacion.DESCUENTO_PLANILLA_POR_ANTICIPO_SALARIO ||
                        objTransaccion.CodigoOperacion == Constantes.CuentaPorCobrar.Operacion.PRESTAMO ||
                        objTransaccion.CodigoOperacion == Constantes.CuentaPorCobrar.Operacion.ABONO_PRESTAMO ||
                        objTransaccion.CodigoOperacion == Constantes.CuentaPorCobrar.Operacion.BACK_TO_BACK ||
                        objTransaccion.CodigoOperacion == Constantes.CuentaPorCobrar.Operacion.RETIRO_SOCIOS ||
                        objTransaccion.CodigoOperacion == Constantes.CuentaPorCobrar.Operacion.DEVOLUCION_SOCIOS)
                    {
                        int codigoCategoria = 0;
                        switch (objTransaccion.CodigoCategoriaEntidad)
                        {
                            case Constantes.Entidad.Categoria.VENDEDOR:
                                codigoCategoria = Constantes.Entidad.Categoria.VENDEDORES;
                                break;
                            case Constantes.Entidad.Categoria.RUTERO_LOCAL:
                                codigoCategoria = Constantes.Entidad.Categoria.VENDEDORES;
                                break;
                            case Constantes.Entidad.Categoria.RUTERO_INTERIOR:
                                codigoCategoria = Constantes.Entidad.Categoria.VENDEDORES;
                                break;
                            case Constantes.Entidad.Categoria.CAFETERIA:
                                codigoCategoria = Constantes.Entidad.Categoria.VENDEDORES;
                                break;
                            case Constantes.Entidad.Categoria.SUPERMERCADO:
                                codigoCategoria = Constantes.Entidad.Categoria.VENDEDORES;
                                break;
                            default:
                                codigoCategoria = objTransaccion.CodigoCategoriaEntidad;
                                break;
                        }
                        sqlSequence = "SELECT NEXT VALUE FOR db_contabilidad.SQ_CUENTA_POR_COBRAR";
                        cmd.CommandText = sqlSequence;
                        long codigoCuentaPorCobrar = (long)cmd.ExecuteScalar();

                        sentenciaSQL = @"
                        INSERT INTO db_contabilidad.cuenta_por_cobrar(codigo_cxc,codigo_tipo_cxc,codigo_categoria_entidad,codigo_categoria,codigo_entidad,nombre_entidad,fecha_prestamo,fecha_inicio_pago,anio_operacion,semana_operacion,monto,observaciones,codigo_transaccion,codigo_planilla,codigo_operacion,codigo_estado,usuario_ing,fecha_ing,usuario_act,fecha_act,carga_inicial,codigo_reporte)
                        VALUES( @CodigoCuentaCobrar,
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
                                @CodigoPlanillaDescuento,
                                @CodigoOperacion,
                                @CodigoEstadoCxC,
                                @UsuarioIng,
                                @FechaIng,
                                @UsuarioAct,
                                @FechaAct,
                                @CargaInicial,
                                @CodigoReporte)";

                        cmd.CommandText = sentenciaSQL;
                        cmd.Parameters.AddWithValue("@CodigoCuentaCobrar", codigoCuentaPorCobrar);
                        cmd.Parameters.AddWithValue("@CodigoCategoria", codigoCategoria);
                        cmd.Parameters.AddWithValue("@NombreEntidad", objTransaccion.NombreEntidad);
                        cmd.Parameters.AddWithValue("@FechaPrestamo", objTransaccion.FechaPrestamo == null ? DBNull.Value : objTransaccion.FechaPrestamo);
                        cmd.Parameters.AddWithValue("@FechaInicioPago", objTransaccion.FechaInicioPago == null ? DBNull.Value : objTransaccion.FechaInicioPago);
                        cmd.Parameters.AddWithValue("@CodigoPlanillaDescuento", DBNull.Value);
                        cmd.Parameters.AddWithValue("@CodigoEstadoCxC", Constantes.CuentaPorCobrar.Estado.PARA_INCLUIR_EN_REPORTE);
                        cmd.Parameters.AddWithValue("@CargaInicial", 0);

                        cmd.Parameters["@CodigoTipoCuentaPorCobrar"].Value = objTransaccion.CodigoTipoCuentaPorCobrar;
                        cmd.Parameters["@CodigoCategoriaEntidad"].Value = objTransaccion.CodigoCategoriaEntidad;
                        cmd.Parameters["@CodigoEntidad"].Value = objTransaccion.CodigoEntidad;
                        cmd.Parameters["@AnioOperacion"].Value = objTransaccion.AnioOperacion;
                        cmd.Parameters["@SemanaOperacion"].Value = objTransaccion.SemanaOperacion;
                        cmd.Parameters["@Monto"].Value = objTransaccion.Monto;
                        cmd.Parameters["@Observaciones"].Value = objTransaccion.Observaciones == null ? DBNull.Value : objTransaccion.Observaciones;
                        cmd.Parameters["@codigoTransaccion"].Value = codigoTransaccion;
                        cmd.Parameters["@CodigoOperacion"].Value = objTransaccion.CodigoOperacion;
                        cmd.Parameters["@UsuarioIng"].Value = usuarioAct;
                        cmd.Parameters["@FechaIng"].Value = DateTime.Now;
                        cmd.Parameters["@UsuarioAct"].Value = DBNull.Value;
                        cmd.Parameters["@FechaAct"].Value = DBNull.Value;
                        cmd.Parameters["@CodigoReporte"].Value = objTransaccion.CodigoReporte;
                        cmd.ExecuteNonQuery();

                        sentenciaSQL = @"
                        UPDATE db_tesoreria.transaccion 
                        SET codigo_cxc = @CodigoCxC
                        WHERE codigo_transaccion = @CodigoTrans";

                        cmd.CommandText = sentenciaSQL;
                        cmd.Parameters.AddWithValue("@CodigoTrans", codigoTransaccion);
                        cmd.Parameters.AddWithValue("@CodigoCxC", codigoCuentaPorCobrar);
                        cmd.ExecuteNonQuery();

                    }// fin if

                    // Cambiando el estado de cuentas por cobrar, de la transacción anulada
                    sentenciaSQL = @"
                    UPDATE db_contabilidad.cuenta_por_cobrar
                    SET codigo_estado = @CodigoEstadoAnulado,
                        usuario_act = @UsuarioAct,
                        fecha_act = @FechaAct
                    WHERE codigo_transaccion = @CodigoTransaccionAnt";

                    cmd.CommandText = sentenciaSQL;
                    cmd.Parameters.AddWithValue("@CodigoEstadoAnulado", Constantes.CuentaPorCobrar.Estado.ANULADO);
                    cmd.Parameters["@CodigoTransaccionAnt"].Value = objTransaccion.CodigoTransaccion;
                    cmd.Parameters["@UsuarioAct"].Value = usuarioAct;
                    cmd.Parameters["@FechaAct"].Value = DateTime.Now;
                    cmd.ExecuteNonQuery();

                    // Actualizar la solicitud de corrección
                    sentenciaSQL = @"
                    UPDATE db_tesoreria.solicitud_correccion
                    SET codigo_transaccion_correcta = @CodigoTransaccion
                    WHERE codigo_transaccion = @CodigoTransaccionAnt";

                    cmd.CommandText = sentenciaSQL;
                    cmd.Parameters["@CodigoTransaccion"].Value = codigoTransaccion;
                    cmd.Parameters["@CodigoTransaccionAnt"].Value = objTransaccion.CodigoTransaccion;
                    rows = cmd.ExecuteNonQuery();
                    bool exitoHistorialTransaccion = false;
                    if (rows > 0)
                    {
                        exitoHistorialTransaccion = true;
                    }

                    // Actualizar transacción que está corrigiendo
                    sentenciaSQL = @"
                    UPDATE db_tesoreria.transaccion
                    SET codigo_estado = @CodigoEstadoAnulado,
                        correccion = 1
                    WHERE codigo_transaccion = @CodigoTransaccionAnt";

                    cmd.CommandText = sentenciaSQL;
                    cmd.Parameters["@CodigoEstadoAnulado"].Value = Constantes.EstadoTransacccion.ANULADO;
                    cmd.Parameters["@CodigoTransaccionAnt"].Value = objTransaccion.CodigoTransaccion;
                    rows = cmd.ExecuteNonQuery();
                    bool exitoTransaccion = false;
                    if (rows > 0){
                        exitoTransaccion = true;
                    }

                    #endregion

                    // Actualización de la secuencia de la transacción
                    cmd.CommandText = sentenciaUpdateTransaccion;
                    cmd.Parameters.AddWithValue("@siguienteValor", correlativoTransaccion + 1);
                    cmd.ExecuteNonQuery();

                    /*bool exitoCorreccion = false;
                    // limpiar los parametros para enviarles solo los que necesito el stored procedure
                    cmd.Parameters.Clear();
                    // le indico que es un procedure
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "db_contabilidad.uspCorreccionTransaccion";
                    cmd.Parameters.AddWithValue("@CodigoTransaccionAnt", objTransaccion.CodigoTransaccion);
                    cmd.Parameters.AddWithValue("@codigoTransaccion", codigoTransaccion);
                    cmd.Parameters.AddWithValue("@CodigoReporte", objTransaccion.CodigoReporte);
                    cmd.Parameters.AddWithValue("@AnioOperacion", objTransaccion.AnioOperacion);
                    cmd.Parameters.AddWithValue("@SemanaOperacion", objTransaccion.SemanaOperacion);
                    cmd.Parameters.AddWithValue("@UsuarioAct", usuarioAct);

                    //Set SqlParameter
                    SqlParameter outParameter = new SqlParameter
                    {
                        ParameterName = "@Resultado", //Parameter name defined in stored procedure
                                                      //SqlDbType = SqlDbType.Int, //Data Type of Parameter
                        Size = 2000,
                        SqlDbType = SqlDbType.VarChar, //Data Type of Parameter
                        Direction = ParameterDirection.Output //Specify the parameter as ouput
                    };

                    //add the parameter to the SqlCommand object
                    cmd.Parameters.Add(outParameter);
                    cmd.ExecuteReader();

                    resultado = outParameter.Value.ToString();
                    if (resultado == "OK")
                    {
                        exitoCorreccion = true;
                    }*/

                    // Attempt to commit the transaction.
                    if (exitoHistorialTransaccion == true && exitoTransaccion == true)
                    {
                        transaction.Commit();
                        conexion.Close();
                        resultado = "OK";
                    }
                    else {
                        transaction.Rollback();
                        conexion.Close();
                        resultado = "Error [0]: Error al registrar la corrección de la transacción";
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

        public List<TransaccionCLS> BuscarTransacciones(string idUsuario, int codigoTipoOperacion, int codigoOperacion, int codigoCategoriaEntidad, int diaOperacion, int esSuperAdmin, int setSemanaAnterior)
        {
            List<TransaccionCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string filterUsuarioIngreso = String.Empty;
                    string filterTipoOperacion = string.Empty;
                    string filterOperaciones = string.Empty;
                    string filterCategoriaEntidad = string.Empty;
                    string filterDiaOperacion = string.Empty;
                    if (codigoOperacion != -1) {
                        filterOperaciones = " AND x.codigo_operacion = " + codigoOperacion.ToString();
                    }
                    if (codigoTipoOperacion != -1 && codigoOperacion == -1)
                    {
                        switch (codigoTipoOperacion)
                        {
                            case 1: // INGRESO
                                filterTipoOperacion = " AND e.signo = 1";
                                break;
                            case 2: // EGRESO
                                filterTipoOperacion = " AND e.signo = -1";
                                break;
                            default:
                                break;
                        }
                    }

                    if (codigoCategoriaEntidad != -1) {
                        filterCategoriaEntidad = " AND x.codigo_categoria_entidad = " + codigoCategoriaEntidad.ToString();
                    }
                    if (diaOperacion != -1)
                    {
                        filterDiaOperacion = " AND x.dia_operacion = " + diaOperacion.ToString();
                    }

                    if (esSuperAdmin == 0) {
                        filterUsuarioIngreso = "AND x.usuario_ing = '" + idUsuario + "'";
                    }
                    string sql = @"
                    SELECT  x.codigo_transaccion,
                            x.anio_operacion,
                            x.semana_operacion,
		                    x.codigo_tipo_transaccion,
                            x.serie_factura,
    	                    x.numero_documento,
		                    x.fecha_documento,
		                    x.numero_recibo,
		                    x.fecha_recibo,
                            FORMAT(x.fecha_recibo,'dd/MM/yyyy') AS fecha_recibo_str,
                            REPLACE(STR(CAST(x.numero_recibo AS varchar), 6),' ','0') AS numero_recibo_str,
		                    x.codigo_entidad,
                            db_tesoreria.GetNombreEntidad(x.codigo_categoria_entidad, x.codigo_entidad) AS entidad,
                            x.codigo_categoria_entidad,
                            d.nombre AS categoria_entidad,
		                    x.codigo_operacion,
		                    w.nombre_operacion AS operacion,
                            w.nombre_reporte_caja AS operacion_caja,
		                    x.codigo_tipo_cxc,
		                    a.nombre AS tipo_cxc,
                            x.codigo_cxc,
		                    x.codigo_area,
		                    b.nombre AS area,
		                    x.fecha_operacion,
                            FORMAT(x.fecha_operacion,'dd/MM/yyyy') AS fecha_operacion_str,
                            CASE
                              WHEN x.dia_operacion = 1 THEN 'LUNES'
                              WHEN x.dia_operacion = 2 THEN 'MARTES'
                              WHEN x.dia_operacion = 3 THEN 'MIERCOLES'
                              WHEN x.dia_operacion = 4 THEN 'JUEVES'
                              WHEN x.dia_operacion = 5 THEN 'VIERNES'
                              WHEN x.dia_operacion = 6 THEN 'SABADO' 
                              WHEN x.dia_operacion = 7 THEN 'DOMINGO' 
                              ELSE 'NO DEFINIDO'
                            END AS nombre_dia_operacion,
		                    x.monto,
		                    x.codigo_estado,
		                    c.nombre AS estado,
                            x.fecha_ing,
                            x.usuario_ing,
                            FORMAT(x.fecha_ing, 'dd/MM/yyyy, hh:mm:ss') AS fecha_ing_str,
                            CASE  
                             WHEN (((x.codigo_estado = @CodigoEstadoTransaccion AND 1 = " + setSemanaAnterior.ToString() + @") OR 1 = " + esSuperAdmin.ToString() + @") AND x.liquidacion = 0) THEN 1
                             ELSE 0
                            END AS permiso_anular,
                            CASE  
                             WHEN (((x.codigo_estado = @CodigoEstadoTransaccion  AND 0 = " + setSemanaAnterior.ToString() + @") OR 1 = " + esSuperAdmin.ToString() + @") AND x.liquidacion = 0) THEN 1
                             ELSE 0
                            END AS permiso_editar,
                            e.signo,
                            x.ruta,
                            FORMAT(GETDATE(), 'dd/MM/yyyy, hh:mm:ss') AS fecha_impresion_str,
                            CONCAT(CASE WHEN x.efectivo = 1 THEN 'Efectivo,' ELSE '' END,CASE WHEN x.deposito = 1 THEN 'Depósito,' ELSE '' END,CASE WHEN x.cheque = 1 THEN 'Cheque' ELSE '' END) AS recursos,
                            COALESCE(x.codigo_transaccion_ant,0) AS codigo_transaccion_ant,
                            x.correccion,
                            x.codigo_seguridad,
                            CASE
                              WHEN e.signo = 1 THEN 'INGRESO'  
                              WHEN e.signo = -1 THEN 'EGRESO'  
                              ELSE 'NEUTRO'
                            END AS tipo_operacion_contable,
                            x.monto_saldo_anterior_cxc,
                            x.monto_saldo_actual_cxc,
                            x.numero_cuenta

                    FROM db_tesoreria.transaccion x
                    INNER JOIN db_tesoreria.operacion w
                    ON x.codigo_operacion = w.codigo_operacion
                    LEFT JOIN db_contabilidad.tipo_cxc a
                    ON x.codigo_tipo_cxc = a.codigo_tipo_cxc
                    LEFT JOIN db_rrhh.area b
                    ON x.codigo_area = b.codigo_area
                    INNER JOIN db_tesoreria.estado_transaccion c
                    ON x.codigo_estado = c.codigo_estado_transaccion
                    INNER JOIN db_tesoreria.entidad_categoria d
                    ON x.codigo_categoria_entidad = d.codigo_categoria_entidad
                    INNER JOIN db_tesoreria.tipo_operacion e
                    ON w.codigo_tipo_operacion = e.codigo_tipo_operacion
                    WHERE x.complemento_conta = 0
                      AND x.codigo_estado = @CodigoEstadoTransaccion
                    " + filterUsuarioIngreso + @"    
                    " + filterTipoOperacion + @" 
                    " + filterOperaciones + @"    
                    " + filterCategoriaEntidad + @"    
                    " + filterDiaOperacion + @"    
                    ORDER BY x.codigo_transaccion DESC";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoTransaccion", Constantes.EstadoTransacccion.REGISTRADO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            TransaccionCLS objTransaccion;
                            lista = new List<TransaccionCLS>();
                            int postCodigoTransaccion = dr.GetOrdinal("codigo_transaccion");
                            int postAnioOperacion = dr.GetOrdinal("anio_operacion");
                            int postSemanaOperacion = dr.GetOrdinal("semana_operacion");
                            int postCodigoTipoTransaccion = dr.GetOrdinal("codigo_tipo_transaccion");
                            int postSerieFactura = dr.GetOrdinal("serie_factura");
                            int postNumeroDocumento = dr.GetOrdinal("numero_documento");
                            int postFechaDocumento = dr.GetOrdinal("fecha_documento");
                            int postNumeroRecibo = dr.GetOrdinal("numero_recibo");
                            int postNumeroReciboStr = dr.GetOrdinal("numero_recibo_str");
                            int postFechaRecibo = dr.GetOrdinal("fecha_recibo");
                            int postFechaReciboStr = dr.GetOrdinal("fecha_recibo_str");
                            int postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            int postEntidad = dr.GetOrdinal("entidad");
                            int postCodigoCategoriaEntidad = dr.GetOrdinal("codigo_categoria_entidad");
                            int postCategoriaEntidad = dr.GetOrdinal("categoria_entidad");
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postOperacion = dr.GetOrdinal("operacion");
                            int postCodigoTipoCuentaPorCobrar = dr.GetOrdinal("codigo_tipo_cxc");
                            int postTipoCuentaPorCobrar = dr.GetOrdinal("tipo_cxc");
                            int postCodigoCuentaPorCobrar = dr.GetOrdinal("codigo_cxc");
                            int postCodigoArea = dr.GetOrdinal("codigo_area");
                            int postArea = dr.GetOrdinal("area");
                            int postFechaOperacion = dr.GetOrdinal("fecha_operacion");
                            int postFechaOperacionStr = dr.GetOrdinal("fecha_operacion_str");
                            int postNombreDiaOperacion = dr.GetOrdinal("nombre_dia_operacion");
                            int postMonto = dr.GetOrdinal("monto");
                            int postCodigoEstado = dr.GetOrdinal("codigo_estado");
                            int postEstado = dr.GetOrdinal("estado");
                            int postFechaIng = dr.GetOrdinal("fecha_ing");
                            int postUsuarioIng = dr.GetOrdinal("usuario_ing");
                            int postFechaIngStr = dr.GetOrdinal("fecha_ing_str");
                            int postPermisoAnular = dr.GetOrdinal("permiso_anular");
                            int postPermisoEditar = dr.GetOrdinal("permiso_editar");
                            int postSigno = dr.GetOrdinal("signo");
                            int postRuta = dr.GetOrdinal("ruta");
                            int postFechaImpresionStr = dr.GetOrdinal("fecha_impresion_str");
                            int postRecursos = dr.GetOrdinal("recursos");
                            int postCodigoTransaccionAnt = dr.GetOrdinal("codigo_transaccion_ant");
                            int postCorreccion = dr.GetOrdinal("correccion");
                            int postCodigoSeguridad = dr.GetOrdinal("codigo_seguridad");
                            int postTipoOperacionContable = dr.GetOrdinal("tipo_operacion_contable");
                            int postMontoSaldoAnteriorCxC = dr.GetOrdinal("monto_saldo_anterior_cxc");
                            int postMontoSaldoActualCxC = dr.GetOrdinal("monto_saldo_actual_cxc");
                            int postNumeroCuenta = dr.GetOrdinal("numero_cuenta");

                            while (dr.Read())
                            {
                                objTransaccion = new TransaccionCLS();
                                objTransaccion.CodigoTransaccion = dr.GetInt64(postCodigoTransaccion);
                                objTransaccion.AnioOperacion = dr.GetInt16(postAnioOperacion);
                                objTransaccion.SemanaOperacion = dr.GetByte(postSemanaOperacion);
                                objTransaccion.CodigoTipoTransaccion = dr.GetString(postCodigoTipoTransaccion);
                                objTransaccion.SerieFactura = dr.IsDBNull(postSerieFactura) ? "" : dr.GetString(postSerieFactura);
                                objTransaccion.NumeroDocumento = dr.IsDBNull(postNumeroDocumento) ? null : dr.GetInt32(postNumeroDocumento);
                                objTransaccion.FechaDocumento = dr.IsDBNull(postFechaDocumento) ? null : dr.GetDateTime(postFechaDocumento);
                                objTransaccion.NumeroRecibo = dr.GetInt64(postNumeroRecibo);
                                objTransaccion.NumeroReciboStr = dr.GetString(postNumeroReciboStr);
                                objTransaccion.FechaRecibo = dr.GetDateTime(postFechaRecibo);
                                objTransaccion.FechaReciboStr = dr.GetString(postFechaReciboStr);
                                objTransaccion.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objTransaccion.NombreEntidad = dr.IsDBNull(postEntidad) ? "" : dr.GetString(postEntidad);
                                objTransaccion.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objTransaccion.CategoriaEntidad = dr.GetString(postCategoriaEntidad);
                                objTransaccion.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objTransaccion.Operacion = dr.GetString(postOperacion);
                                objTransaccion.CodigoTipoCuentaPorCobrar = dr.GetByte(postCodigoTipoCuentaPorCobrar);
                                objTransaccion.TipoCuentaPorCobrar = dr.GetString(postTipoCuentaPorCobrar);
                                objTransaccion.CodigoCuentaPorCobrar = dr.IsDBNull(postCodigoCuentaPorCobrar) ? -1 : dr.GetInt64(postCodigoCuentaPorCobrar);
                                objTransaccion.CodigoArea = dr.GetInt16(postCodigoArea);
                                objTransaccion.Area = dr.GetString(postArea);
                                objTransaccion.FechaOperacion = dr.GetDateTime(postFechaOperacion);
                                objTransaccion.FechaStr = dr.GetString(postFechaOperacionStr);
                                objTransaccion.NombreDiaOperacion = dr.GetString(postNombreDiaOperacion);
                                objTransaccion.Monto = dr.GetDecimal(postMonto);
                                objTransaccion.CodigoEstado = dr.GetInt16(postCodigoEstado);
                                objTransaccion.Estado = dr.GetString(postEstado);
                                objTransaccion.FechaIng = dr.GetDateTime(postFechaIng);
                                objTransaccion.FechaIngStr = dr.GetString(postFechaIngStr);
                                objTransaccion.UsuarioIng = dr.GetString(postUsuarioIng);
                                objTransaccion.PermisoAnular = (Byte)dr.GetInt32(postPermisoAnular);
                                objTransaccion.PermisoEditar = (Byte)dr.GetInt32(postPermisoEditar);
                                objTransaccion.Signo = dr.GetInt16(postSigno);
                                objTransaccion.Ruta = dr.GetInt16(postRuta);
                                objTransaccion.FechaImpresionStr = dr.GetString(postFechaImpresionStr);
                                objTransaccion.Recursos = dr.GetString(postRecursos);
                                objTransaccion.CodigoTransaccionAnt = dr.GetInt64(postCodigoTransaccionAnt);
                                objTransaccion.Correccion = dr.GetByte(postCorreccion);
                                objTransaccion.CodigoSeguridad = dr.GetString(postCodigoSeguridad);
                                objTransaccion.TipoOperacionContable = dr.GetString(postTipoOperacionContable);
                                objTransaccion.MontoSaldoAnteriorCxC = dr.GetDecimal(postMontoSaldoAnteriorCxC);
                                objTransaccion.MontoSaldoActualCxC = dr.GetDecimal(postMontoSaldoActualCxC);
                                objTransaccion.NumeroCuenta = dr.IsDBNull(postNumeroCuenta) ? "" : dr.GetString(postNumeroCuenta);

                                lista.Add(objTransaccion);
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

        public List<TransaccionCLS> BuscarTransaccionesConsulta(int anioOperacion, int semanaOperacion, int codigoReporte, int codigoOperacion, int codigoCategoriaEntidad, int diaOperacion)
        {
            List<TransaccionCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string filterOperaciones = string.Empty;
                    string filterCategoriaEntidad = string.Empty;
                    string filterDiaOperacion = string.Empty;
                    if (codigoOperacion != -1)
                    {
                        filterOperaciones = " AND x.codigo_operacion = " + codigoOperacion.ToString();
                    }
                    if (codigoCategoriaEntidad != -1)
                    {
                        filterCategoriaEntidad = " AND x.codigo_categoria_entidad = " + codigoCategoriaEntidad.ToString();
                    }
                    if (diaOperacion != -1)
                    {
                        filterDiaOperacion = " AND x.dia_operacion = " + diaOperacion.ToString();
                    }
                    string sql = @"
                    SELECT  x.codigo_transaccion,
                            x.anio_operacion,
                            x.semana_operacion,
		                    x.codigo_tipo_transaccion,
                            x.serie_factura,
    	                    x.numero_documento,
		                    x.fecha_documento,
		                    x.numero_recibo,
		                    x.fecha_recibo,
                            FORMAT(x.fecha_recibo,'dd/MM/yyyy') AS fecha_recibo_str,
                            REPLACE(STR(CAST(x.numero_recibo AS varchar), 10),' ','0') AS numero_recibo_str,
		                    x.codigo_entidad,
                            db_tesoreria.GetNombreEntidad(x.codigo_categoria_entidad, x.codigo_entidad) AS entidad,
                            x.codigo_categoria_entidad,
                            d.nombre AS categoria_entidad,
		                    x.codigo_operacion,
		                    w.nombre_operacion AS operacion,
                            w.nombre_reporte_caja AS operacion_caja,
		                    x.codigo_tipo_cxc,
		                    a.nombre AS tipo_cxc,
                            x.codigo_cxc,
		                    x.codigo_area,
		                    b.nombre AS area,
		                    x.fecha_operacion,
                            FORMAT(x.fecha_operacion,'dd/MM/yyyy') AS fecha_operacion_str,
                            CASE
                              WHEN x.dia_operacion = 1 THEN 'LUNES'
                              WHEN x.dia_operacion = 2 THEN 'MARTES'
                              WHEN x.dia_operacion = 3 THEN 'MIERCOLES'
                              WHEN x.dia_operacion = 4 THEN 'JUEVES'
                              WHEN x.dia_operacion = 5 THEN 'VIERNES'
                              WHEN x.dia_operacion = 6 THEN 'SABADO' 
                              WHEN x.dia_operacion = 7 THEN 'DOMINGO' 
                              ELSE 'NO DEFINIDO'
                            END AS nombre_dia_operacion,
                            x.dia_operacion,
		                    x.monto,
		                    x.codigo_estado,
		                    c.nombre AS estado,
                            x.fecha_ing,
                            x.usuario_ing,
                            FORMAT(x.fecha_ing, 'dd/MM/yyyy, hh:mm:ss') AS fecha_ing_str,
                            e.signo,
                            x.ruta,
                            FORMAT(GETDATE(), 'dd/MM/yyyy, hh:mm:ss') AS fecha_impresion_str,
                            CONCAT(CASE WHEN x.efectivo = 1 THEN 'Efectivo,' ELSE '' END,CASE WHEN x.deposito = 1 THEN 'Depósito,' ELSE '' END,CASE WHEN x.cheque = 1 THEN 'Cheque' ELSE '' END) AS recursos,
                            x.complemento_conta,
                            CASE 
                               WHEN x.complemento_conta = 1 THEN 'SI'
                               ELSE 'NO' 
                            END AS es_complemento_de_informacion,
                            COALESCE(x.codigo_transaccion_ant,0) AS codigo_transaccion_ant,
                            x.revisado,
                            x.correccion,
                            x.codigo_seguridad,
                            CASE
                              WHEN e.signo = 1 THEN 'INGRESO'  
                              WHEN e.signo = -1 THEN 'EGRESO'  
                              ELSE 'NEUTRO'
                            END AS tipo_operacion_contable,
                            x.monto_saldo_anterior_cxc,
                            x.monto_saldo_actual_cxc,
                            x.numero_cuenta,
                            x.nombre_proveedor

                    FROM db_tesoreria.transaccion x
                    INNER JOIN db_tesoreria.operacion w
                    ON x.codigo_operacion = w.codigo_operacion
                    LEFT JOIN db_contabilidad.tipo_cxc a
                    ON x.codigo_tipo_cxc = a.codigo_tipo_cxc
                    LEFT JOIN db_rrhh.area b
                    ON x.codigo_area = b.codigo_area
                    INNER JOIN db_tesoreria.estado_transaccion c
                    ON x.codigo_estado = c.codigo_estado_transaccion
                    INNER JOIN db_tesoreria.entidad_categoria d
                    ON x.codigo_categoria_entidad = d.codigo_categoria_entidad
                    INNER JOIN db_tesoreria.tipo_operacion e
                    ON w.codigo_tipo_operacion = e.codigo_tipo_operacion
                    WHERE x.codigo_estado <> 0
                      AND x.anio_operacion = @AnioOperacion
                      AND x.semana_operacion = @SemanaOperacion  
                      AND x.codigo_reporte = @CodigoReporte
                    " + filterOperaciones + @"    
                    " + filterCategoriaEntidad + @"
                    " + filterDiaOperacion + @"
                    ORDER BY x.codigo_transaccion DESC";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@AnioOperacion", anioOperacion);
                        cmd.Parameters.AddWithValue("@SemanaOperacion", semanaOperacion);
                        cmd.Parameters.AddWithValue("@CodigoReporte", codigoReporte);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            TransaccionCLS objTransaccion;
                            lista = new List<TransaccionCLS>();
                            int postCodigoTransaccion = dr.GetOrdinal("codigo_transaccion");
                            int postAnioOperacion = dr.GetOrdinal("anio_operacion");
                            int postSemanaOperacion = dr.GetOrdinal("semana_operacion");
                            int postCodigoTipoTransaccion = dr.GetOrdinal("codigo_tipo_transaccion");
                            int postSerieFactura = dr.GetOrdinal("serie_factura");
                            int postNumeroDocumento = dr.GetOrdinal("numero_documento");
                            int postFechaDocumento = dr.GetOrdinal("fecha_documento");
                            int postNumeroRecibo = dr.GetOrdinal("numero_recibo");
                            int postNumeroReciboStr = dr.GetOrdinal("numero_recibo_str");
                            int postFechaRecibo = dr.GetOrdinal("fecha_recibo");
                            int postFechaReciboStr = dr.GetOrdinal("fecha_recibo_str");
                            int postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            int postEntidad = dr.GetOrdinal("entidad");
                            int postCodigoCategoriaEntidad = dr.GetOrdinal("codigo_categoria_entidad");
                            int postCategoriaEntidad = dr.GetOrdinal("categoria_entidad");
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postOperacion = dr.GetOrdinal("operacion");
                            int postCodigoTipoCuentaPorCobrar = dr.GetOrdinal("codigo_tipo_cxc");
                            int postTipoCuentaPorCobrar = dr.GetOrdinal("tipo_cxc");
                            int postCodigoCuentaPorCobrar = dr.GetOrdinal("codigo_cxc");
                            int postCodigoArea = dr.GetOrdinal("codigo_area");
                            int postArea = dr.GetOrdinal("area");
                            int postFechaOperacion = dr.GetOrdinal("fecha_operacion");
                            int postFechaOperacionStr = dr.GetOrdinal("fecha_operacion_str");
                            int postDiaOperacion = dr.GetOrdinal("dia_operacion");
                            int postNombreDiaOperacion = dr.GetOrdinal("nombre_dia_operacion");
                            int postMonto = dr.GetOrdinal("monto");
                            int postCodigoEstado = dr.GetOrdinal("codigo_estado");
                            int postEstado = dr.GetOrdinal("estado");
                            int postFechaIng = dr.GetOrdinal("fecha_ing");
                            int postUsuarioIng = dr.GetOrdinal("usuario_ing");
                            int postFechaIngStr = dr.GetOrdinal("fecha_ing_str");
                            int postSigno = dr.GetOrdinal("signo");
                            int postRuta = dr.GetOrdinal("ruta");
                            int postFechaImpresionStr = dr.GetOrdinal("fecha_impresion_str");
                            int postRecursos = dr.GetOrdinal("recursos");
                            int postComplementoConta = dr.GetOrdinal("complemento_conta");
                            int postEsComplementoDeInformacion = dr.GetOrdinal("es_complemento_de_informacion");
                            int postCodigoTransaccionAnt = dr.GetOrdinal("codigo_transaccion_ant");
                            int postRevisado = dr.GetOrdinal("revisado");
                            int postCorreccion = dr.GetOrdinal("correccion");
                            int postCodigoSeguridad = dr.GetOrdinal("codigo_seguridad");
                            int postTipoOperacionContable = dr.GetOrdinal("tipo_operacion_contable");
                            int postMontoSaldoAnteriorCxC = dr.GetOrdinal("monto_saldo_anterior_cxc");
                            int postMontoSaldoActualCxC = dr.GetOrdinal("monto_saldo_actual_cxc");
                            int postNumeroCuenta = dr.GetOrdinal("numero_cuenta");
                            int postNombreProveedor = dr.GetOrdinal("nombre_proveedor");

                            while (dr.Read())
                            {
                                objTransaccion = new TransaccionCLS();
                                objTransaccion.CodigoTransaccion = dr.GetInt64(postCodigoTransaccion);
                                objTransaccion.AnioOperacion = dr.GetInt16(postAnioOperacion);
                                objTransaccion.SemanaOperacion = dr.GetByte(postSemanaOperacion);
                                objTransaccion.CodigoTipoTransaccion = dr.GetString(postCodigoTipoTransaccion);
                                objTransaccion.SerieFactura = dr.IsDBNull(postSerieFactura) ? "" : dr.GetString(postSerieFactura);
                                objTransaccion.NumeroDocumento = dr.IsDBNull(postNumeroDocumento) ? null : dr.GetInt32(postNumeroDocumento);
                                objTransaccion.FechaDocumento = dr.IsDBNull(postFechaDocumento) ? null : dr.GetDateTime(postFechaDocumento);
                                objTransaccion.NumeroRecibo = dr.GetInt64(postNumeroRecibo);
                                objTransaccion.NumeroReciboStr = dr.GetString(postNumeroReciboStr);
                                objTransaccion.FechaRecibo = dr.GetDateTime(postFechaRecibo);
                                objTransaccion.FechaReciboStr = dr.GetString(postFechaReciboStr);
                                objTransaccion.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objTransaccion.NombreEntidad = dr.IsDBNull(postEntidad) ? "" : dr.GetString(postEntidad);
                                objTransaccion.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objTransaccion.CategoriaEntidad = dr.GetString(postCategoriaEntidad);
                                objTransaccion.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objTransaccion.Operacion = dr.GetString(postOperacion);
                                objTransaccion.CodigoTipoCuentaPorCobrar = dr.GetByte(postCodigoTipoCuentaPorCobrar);
                                objTransaccion.TipoCuentaPorCobrar = dr.GetString(postTipoCuentaPorCobrar);
                                objTransaccion.CodigoCuentaPorCobrar = dr.IsDBNull(postCodigoCuentaPorCobrar) ? -1 : dr.GetInt64(postCodigoCuentaPorCobrar);
                                objTransaccion.CodigoArea = dr.GetInt16(postCodigoArea);
                                objTransaccion.Area = dr.GetString(postArea);
                                objTransaccion.FechaOperacion = dr.GetDateTime(postFechaOperacion);
                                objTransaccion.FechaStr = dr.GetString(postFechaOperacionStr);
                                objTransaccion.DiaOperacion = dr.GetByte(postDiaOperacion);
                                objTransaccion.NombreDiaOperacion = dr.GetString(postNombreDiaOperacion);
                                objTransaccion.Monto = dr.GetDecimal(postMonto);
                                objTransaccion.CodigoEstado = dr.GetInt16(postCodigoEstado);
                                objTransaccion.Estado = dr.GetString(postEstado);
                                objTransaccion.FechaIng = dr.GetDateTime(postFechaIng);
                                objTransaccion.FechaIngStr = dr.GetString(postFechaIngStr);
                                objTransaccion.UsuarioIng = dr.GetString(postUsuarioIng);
                                objTransaccion.Signo = dr.GetInt16(postSigno);
                                objTransaccion.Ruta = dr.GetInt16(postRuta);
                                objTransaccion.FechaImpresionStr = dr.GetString(postFechaImpresionStr);
                                objTransaccion.Recursos = dr.GetString(postRecursos);
                                objTransaccion.ComplementoConta = dr.GetByte(postComplementoConta);
                                objTransaccion.EscomplementoDeInformacion = dr.GetString(postEsComplementoDeInformacion);
                                objTransaccion.CodigoTransaccionAnt = dr.GetInt64(postCodigoTransaccionAnt);
                                objTransaccion.Revisado = dr.GetByte(postRevisado);
                                objTransaccion.Correccion = dr.GetByte(postCorreccion);
                                objTransaccion.CodigoSeguridad = dr.GetString(postCodigoSeguridad);
                                objTransaccion.TipoOperacionContable = dr.GetString(postTipoOperacionContable);
                                objTransaccion.MontoSaldoAnteriorCxC = dr.GetDecimal(postMontoSaldoAnteriorCxC);
                                objTransaccion.MontoSaldoActualCxC = dr.GetDecimal(postMontoSaldoActualCxC);
                                objTransaccion.NumeroCuenta = dr.IsDBNull(postNumeroCuenta) ? "" : dr.GetString(postNumeroCuenta);
                                objTransaccion.NombreProveedor = dr.IsDBNull(postNombreProveedor) ? "" : dr.GetString(postNombreProveedor);

                                lista.Add(objTransaccion);
                            }
                        }
                    }
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

        public List<TransaccionCLS> BuscarTransaccionesConsultaContabilidad(int anioOperacion, int semanaOperacion, int codigoTipoOperacion, int codigoOperacion, int codigoCategoriaEntidad, string nombreEntidad, string fechaInicioStr, string fechaFinStr)
        {
            List<TransaccionCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string filterAnioOperacion = string.Empty;
                    string filterSemanaOperacion = string.Empty;
                    string filterTipoOperacion = string.Empty;
                    string filterOperaciones = string.Empty;
                    string filterCategoriaEntidad = string.Empty;
                    string filterNombreEntidad = string.Empty;
                    string filterRangoFechasOperacion = string.Empty;

                    if (anioOperacion != -1)
                    {
                        filterAnioOperacion = " AND x.anio_operacion = " + anioOperacion.ToString();
                    }
                    if (semanaOperacion != -1)
                    {
                        filterSemanaOperacion = " AND x.semana_operacion = " + semanaOperacion.ToString();
                    }
                    switch (codigoTipoOperacion)
                    {
                        case 1: // INGRESO
                            filterTipoOperacion = " AND e.signo = 1";
                            break;
                        case 2: // EGRESO
                            filterTipoOperacion = " AND e.signo = -1";
                            break;
                        default:
                            break;
                    }
                    if (codigoOperacion != -1)
                    {
                        filterOperaciones = " AND x.codigo_operacion = " + codigoOperacion.ToString();
                    }
                    if (codigoCategoriaEntidad != -1)
                    {
                        filterCategoriaEntidad = " AND x.codigo_categoria_entidad = " + codigoCategoriaEntidad.ToString();
                    }
                    if (nombreEntidad != "" && nombreEntidad != null && !nombreEntidad.Equals(string.Empty))
                    {
                        string phrase = nombreEntidad;
                        string[] words = phrase.Split(' ');
                        StringBuilder sb = new StringBuilder();
                        sb.Append('%');
                        foreach (var word in words)
                        {
                            sb.Append(word);
                            sb.Append('%');
                        }

                        filterNombreEntidad = " WHERE m.entidad like '" + sb.ToString() + "'";
                    }

                    if (fechaInicioStr != null && fechaFinStr != null)
                    {
                        if (fechaInicioStr != "" && fechaFinStr != "")
                        {
                            filterRangoFechasOperacion = " AND CONVERT(INT,CONVERT(DATETIME,FORMAT(x.fecha_operacion,'dd/MM/yyyy'),103)) BETWEEN CONVERT(INT,CONVERT(DATETIME,'" + fechaInicioStr + "',103)) AND CONVERT(INT,CONVERT(DATETIME,'" + fechaFinStr + "',103))";
                        }
                    }

                    string sql = @"
                    SELECT m.* 
                    FROM (
                    SELECT  x.codigo_transaccion,
                            x.anio_operacion,
                            x.semana_operacion,
		                    x.codigo_tipo_transaccion,
                            x.serie_factura,
    	                    x.numero_documento,
		                    x.fecha_documento,
		                    x.numero_recibo,
		                    x.fecha_recibo,
                            FORMAT(x.fecha_recibo,'dd/MM/yyyy') AS fecha_recibo_str,
                            REPLACE(STR(CAST(x.numero_recibo AS varchar), 10),' ','0') AS numero_recibo_str,
		                    x.codigo_entidad,
                            db_tesoreria.GetNombreEntidad(x.codigo_categoria_entidad, x.codigo_entidad) AS entidad,
                            x.codigo_categoria_entidad,
                            d.nombre AS categoria_entidad,
		                    x.codigo_operacion,
		                    w.nombre_operacion AS operacion,
                            w.nombre_reporte_caja AS operacion_caja,
		                    x.codigo_tipo_cxc,
		                    a.nombre AS tipo_cxc,
                            x.codigo_cxc,
		                    x.codigo_area,
		                    b.nombre AS area,
		                    x.fecha_operacion,
                            FORMAT(x.fecha_operacion,'dd/MM/yyyy') AS fecha_operacion_str,
                            CASE
                              WHEN x.dia_operacion = 1 THEN 'LUNES'
                              WHEN x.dia_operacion = 2 THEN 'MARTES'
                              WHEN x.dia_operacion = 3 THEN 'MIERCOLES'
                              WHEN x.dia_operacion = 4 THEN 'JUEVES'
                              WHEN x.dia_operacion = 5 THEN 'VIERNES'
                              WHEN x.dia_operacion = 6 THEN 'SABADO' 
                              WHEN x.dia_operacion = 7 THEN 'DOMINGO' 
                              ELSE 'NO DEFINIDO'
                            END AS nombre_dia_operacion,
                            x.dia_operacion,
		                    x.monto,
		                    x.codigo_estado,
		                    c.nombre AS estado,
                            x.fecha_ing,
                            x.usuario_ing,
                            FORMAT(x.fecha_ing, 'dd/MM/yyyy, hh:mm:ss') AS fecha_ing_str,
                            e.signo,
                            x.ruta,
                            FORMAT(GETDATE(), 'dd/MM/yyyy, hh:mm:ss') AS fecha_impresion_str,
                            CONCAT(CASE WHEN x.efectivo = 1 THEN 'Efectivo,' ELSE '' END,CASE WHEN x.deposito = 1 THEN 'Depósito,' ELSE '' END,CASE WHEN x.cheque = 1 THEN 'Cheque' ELSE '' END) AS recursos,
                            x.complemento_conta,
                            CASE 
                               WHEN x.complemento_conta = 1 THEN 'SI'
                               ELSE 'NO' 
                            END AS es_complemento_de_informacion,
                            COALESCE(x.codigo_transaccion_ant,0) AS codigo_transaccion_ant,
                            x.revisado,
                            x.correccion,
                            x.codigo_seguridad,
                            CASE
                              WHEN e.signo = 1 THEN 'INGRESO'  
                              WHEN e.signo = -1 THEN 'EGRESO'  
                              ELSE 'NEUTRO'
                            END AS tipo_operacion_contable,
                            x.monto_saldo_anterior_cxc,
                            x.monto_saldo_actual_cxc,
                            x.numero_cuenta,
                            x.nombre_proveedor

                    FROM db_tesoreria.transaccion x
                    INNER JOIN db_tesoreria.operacion w
                    ON x.codigo_operacion = w.codigo_operacion
                    LEFT JOIN db_contabilidad.tipo_cxc a
                    ON x.codigo_tipo_cxc = a.codigo_tipo_cxc
                    LEFT JOIN db_rrhh.area b
                    ON x.codigo_area = b.codigo_area
                    INNER JOIN db_tesoreria.estado_transaccion c
                    ON x.codigo_estado = c.codigo_estado_transaccion
                    INNER JOIN db_tesoreria.entidad_categoria d
                    ON x.codigo_categoria_entidad = d.codigo_categoria_entidad
                    INNER JOIN db_tesoreria.tipo_operacion e
                    ON w.codigo_tipo_operacion = e.codigo_tipo_operacion
                    WHERE x.codigo_estado <> 0
                    " + filterAnioOperacion + @"    
                    " + filterSemanaOperacion + @"    
                    " + filterTipoOperacion + @"    
                    " + filterOperaciones + @"    
                    " + filterCategoriaEntidad + @"
                    " + filterRangoFechasOperacion + @"
                    ) m
                    " + filterNombreEntidad;

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            TransaccionCLS objTransaccion;
                            lista = new List<TransaccionCLS>();
                            int postCodigoTransaccion = dr.GetOrdinal("codigo_transaccion");
                            int postAnioOperacion = dr.GetOrdinal("anio_operacion");
                            int postSemanaOperacion = dr.GetOrdinal("semana_operacion");
                            int postCodigoTipoTransaccion = dr.GetOrdinal("codigo_tipo_transaccion");
                            int postSerieFactura = dr.GetOrdinal("serie_factura");
                            int postNumeroDocumento = dr.GetOrdinal("numero_documento");
                            int postFechaDocumento = dr.GetOrdinal("fecha_documento");
                            int postNumeroRecibo = dr.GetOrdinal("numero_recibo");
                            int postNumeroReciboStr = dr.GetOrdinal("numero_recibo_str");
                            int postFechaRecibo = dr.GetOrdinal("fecha_recibo");
                            int postFechaReciboStr = dr.GetOrdinal("fecha_recibo_str");
                            int postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            int postEntidad = dr.GetOrdinal("entidad");
                            int postCodigoCategoriaEntidad = dr.GetOrdinal("codigo_categoria_entidad");
                            int postCategoriaEntidad = dr.GetOrdinal("categoria_entidad");
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postOperacion = dr.GetOrdinal("operacion");
                            int postCodigoTipoCuentaPorCobrar = dr.GetOrdinal("codigo_tipo_cxc");
                            int postTipoCuentaPorCobrar = dr.GetOrdinal("tipo_cxc");
                            int postCodigoCuentaPorCobrar = dr.GetOrdinal("codigo_cxc");
                            int postCodigoArea = dr.GetOrdinal("codigo_area");
                            int postArea = dr.GetOrdinal("area");
                            int postFechaOperacion = dr.GetOrdinal("fecha_operacion");
                            int postFechaOperacionStr = dr.GetOrdinal("fecha_operacion_str");
                            int postDiaOperacion = dr.GetOrdinal("dia_operacion");
                            int postNombreDiaOperacion = dr.GetOrdinal("nombre_dia_operacion");
                            int postMonto = dr.GetOrdinal("monto");
                            int postCodigoEstado = dr.GetOrdinal("codigo_estado");
                            int postEstado = dr.GetOrdinal("estado");
                            int postFechaIng = dr.GetOrdinal("fecha_ing");
                            int postUsuarioIng = dr.GetOrdinal("usuario_ing");
                            int postFechaIngStr = dr.GetOrdinal("fecha_ing_str");
                            int postSigno = dr.GetOrdinal("signo");
                            int postRuta = dr.GetOrdinal("ruta");
                            int postFechaImpresionStr = dr.GetOrdinal("fecha_impresion_str");
                            int postRecursos = dr.GetOrdinal("recursos");
                            int postComplementoConta = dr.GetOrdinal("complemento_conta");
                            int postEsComplementoDeInformacion = dr.GetOrdinal("es_complemento_de_informacion");
                            int postCodigoTransaccionAnt = dr.GetOrdinal("codigo_transaccion_ant");
                            int postRevisado = dr.GetOrdinal("revisado");
                            int postCorreccion = dr.GetOrdinal("correccion");
                            int postCodigoSeguridad = dr.GetOrdinal("codigo_seguridad");
                            int postTipoOperacionContable = dr.GetOrdinal("tipo_operacion_contable");
                            int postMontoSaldoAnteriorCxC = dr.GetOrdinal("monto_saldo_anterior_cxc");
                            int postMontoSaldoActualCxC = dr.GetOrdinal("monto_saldo_actual_cxc");
                            int postNumeroCuenta = dr.GetOrdinal("numero_cuenta");
                            int postNombreProveedor = dr.GetOrdinal("nombre_proveedor");

                            while (dr.Read())
                            {
                                objTransaccion = new TransaccionCLS();
                                objTransaccion.CodigoTransaccion = dr.GetInt64(postCodigoTransaccion);
                                objTransaccion.AnioOperacion = dr.GetInt16(postAnioOperacion);
                                objTransaccion.SemanaOperacion = dr.GetByte(postSemanaOperacion);
                                objTransaccion.CodigoTipoTransaccion = dr.GetString(postCodigoTipoTransaccion);
                                objTransaccion.SerieFactura = dr.IsDBNull(postSerieFactura) ? "" : dr.GetString(postSerieFactura);
                                objTransaccion.NumeroDocumento = dr.IsDBNull(postNumeroDocumento) ? null : dr.GetInt32(postNumeroDocumento);
                                objTransaccion.FechaDocumento = dr.IsDBNull(postFechaDocumento) ? null : dr.GetDateTime(postFechaDocumento);
                                objTransaccion.NumeroRecibo = dr.GetInt64(postNumeroRecibo);
                                objTransaccion.NumeroReciboStr = dr.GetString(postNumeroReciboStr);
                                objTransaccion.FechaRecibo = dr.GetDateTime(postFechaRecibo);
                                objTransaccion.FechaReciboStr = dr.GetString(postFechaReciboStr);
                                objTransaccion.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objTransaccion.NombreEntidad = dr.IsDBNull(postEntidad) ? "" : dr.GetString(postEntidad);
                                objTransaccion.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objTransaccion.CategoriaEntidad = dr.GetString(postCategoriaEntidad);
                                objTransaccion.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objTransaccion.Operacion = dr.GetString(postOperacion);
                                objTransaccion.CodigoTipoCuentaPorCobrar = dr.GetByte(postCodigoTipoCuentaPorCobrar);
                                objTransaccion.TipoCuentaPorCobrar = dr.GetString(postTipoCuentaPorCobrar);
                                objTransaccion.CodigoCuentaPorCobrar = dr.IsDBNull(postCodigoCuentaPorCobrar) ? -1 : dr.GetInt64(postCodigoCuentaPorCobrar);
                                objTransaccion.CodigoArea = dr.GetInt16(postCodigoArea);
                                objTransaccion.Area = dr.GetString(postArea);
                                objTransaccion.FechaOperacion = dr.GetDateTime(postFechaOperacion);
                                objTransaccion.FechaStr = dr.GetString(postFechaOperacionStr);
                                objTransaccion.DiaOperacion = dr.GetByte(postDiaOperacion);
                                objTransaccion.NombreDiaOperacion = dr.GetString(postNombreDiaOperacion);
                                objTransaccion.Monto = dr.GetDecimal(postMonto);
                                objTransaccion.CodigoEstado = dr.GetInt16(postCodigoEstado);
                                objTransaccion.Estado = dr.GetString(postEstado);
                                objTransaccion.FechaIng = dr.GetDateTime(postFechaIng);
                                objTransaccion.FechaIngStr = dr.GetString(postFechaIngStr);
                                objTransaccion.UsuarioIng = dr.GetString(postUsuarioIng);
                                objTransaccion.Signo = dr.GetInt16(postSigno);
                                objTransaccion.Ruta = dr.GetInt16(postRuta);
                                objTransaccion.FechaImpresionStr = dr.GetString(postFechaImpresionStr);
                                objTransaccion.Recursos = dr.GetString(postRecursos);
                                objTransaccion.ComplementoConta = dr.GetByte(postComplementoConta);
                                objTransaccion.EscomplementoDeInformacion = dr.GetString(postEsComplementoDeInformacion);
                                objTransaccion.CodigoTransaccionAnt = dr.GetInt64(postCodigoTransaccionAnt);
                                objTransaccion.Revisado = dr.GetByte(postRevisado);
                                objTransaccion.Correccion = dr.GetByte(postCorreccion);
                                objTransaccion.CodigoSeguridad = dr.GetString(postCodigoSeguridad);
                                objTransaccion.TipoOperacionContable = dr.GetString(postTipoOperacionContable);
                                objTransaccion.MontoSaldoAnteriorCxC = dr.GetDecimal(postMontoSaldoAnteriorCxC);
                                objTransaccion.MontoSaldoActualCxC = dr.GetDecimal(postMontoSaldoActualCxC);
                                objTransaccion.NumeroCuenta = dr.IsDBNull(postNumeroCuenta) ? "" : dr.GetString(postNumeroCuenta);
                                objTransaccion.NombreProveedor = dr.IsDBNull(postNombreProveedor) ? "" : dr.GetString(postNombreProveedor);

                                lista.Add(objTransaccion);
                            }
                        }
                    }
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

        public List<TransaccionCLS> BuscarTransaccionesParaCorreccion(int anioOperacion, int semanaOperacion, int codigoReporte, int codigoOperacion, int codigoCategoriaEntidad, int esSuperAdmin)
        {
            List<TransaccionCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string filterOperaciones = string.Empty;
                    string filterCategoriaEntidad = string.Empty;
                    string filterEstadoTrasaccion = string.Empty;
                    if (codigoOperacion != -1)
                    {
                        filterOperaciones = " AND x.codigo_operacion = " + codigoOperacion.ToString();
                    }
                    if (codigoCategoriaEntidad != -1)
                    {
                        filterCategoriaEntidad = " AND x.codigo_categoria_entidad = " + codigoCategoriaEntidad.ToString();
                    }

                    string sql = @"
                    SELECT  x.codigo_transaccion,
		                    x.codigo_tipo_transaccion,
                            x.serie_factura,
    	                    x.numero_documento,
		                    x.fecha_documento,
		                    x.numero_recibo,
		                    x.fecha_recibo,
                            FORMAT(x.fecha_recibo,'dd/MM/yyyy') AS fecha_recibo_str,
                            REPLACE(STR(CAST(x.numero_recibo AS varchar), 10),' ','0') AS numero_recibo_str,
		                    x.codigo_entidad,
                            db_tesoreria.GetNombreEntidad(x.codigo_categoria_entidad, x.codigo_entidad) AS entidad,
                            x.codigo_categoria_entidad,
                            d.nombre AS categoria_entidad,
		                    x.codigo_operacion,
		                    w.nombre_operacion AS operacion,
                            w.nombre_reporte_caja AS operacion_caja,
		                    x.codigo_tipo_cxc,
		                    a.nombre AS tipo_cxc,
                            x.codigo_cxc,
		                    x.codigo_area,
		                    b.nombre AS area,
		                    x.fecha_operacion,
                            FORMAT(x.fecha_operacion,'dd/MM/yyyy') AS fecha_operacion_str,
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
                            END AS nombre_dia_operacion,
		                    x.monto,
		                    x.codigo_estado,
		                    c.nombre AS estado,
                            x.fecha_ing,
                            x.usuario_ing,
                            FORMAT(x.fecha_ing, 'dd/MM/yyyy, hh:mm:ss') AS fecha_ing_str,
                            e.signo,
                            x.ruta,
                            FORMAT(GETDATE(), 'dd/MM/yyyy, hh:mm:ss') AS fecha_impresion_str,
                            CONCAT(CASE WHEN x.efectivo = 1 THEN 'Efectivo,' ELSE '' END,CASE WHEN x.deposito = 1 THEN 'Depósito,' ELSE '' END,CASE WHEN x.cheque = 1 THEN 'Cheque' ELSE '' END) AS recursos,
                            x.complemento_conta,
                            CASE 
                               WHEN x.complemento_conta = 1 THEN 'SI'
                               ELSE 'NO' 
                            END AS es_complemento_de_informacion,
                            CASE
                              WHEN (x.correccion = 1 AND (g.codigo_estado = @CodigoEstadoReporteCajaPorRevisar OR g.codigo_estado = @CodigoEstadoReporteCajaVistoBueno) AND 1 = " + esSuperAdmin.ToString() + @" AND x.codigo_estado_solicitud_correccion = " + Constantes.Correccion.EstadoSolicitudcorreccion.VISTO_BUENO.ToString() + @" AND f.codigo_tipo_correccion = " + Constantes.Correccion.TipoCorreccion.MODIFICACION.ToString() + @") THEN 1
                              WHEN (x.correccion = 1 AND g.codigo_estado = @CodigoEstadoReporteCajaPorRevisar AND x.codigo_estado_solicitud_correccion = " + Constantes.Correccion.EstadoSolicitudcorreccion.VISTO_BUENO.ToString() + @" AND f.codigo_tipo_correccion = " + Constantes.Correccion.TipoCorreccion.MODIFICACION.ToString() + @") THEN 1
                              ELSE 0
                            END AS permiso_editar,
                            CASE
                              WHEN (x.correccion = 1 AND (g.codigo_estado = @CodigoEstadoReporteCajaPorRevisar OR g.codigo_estado = @CodigoEstadoReporteCajaVistoBueno) AND 1 = " + esSuperAdmin.ToString() + @" AND x.codigo_estado_solicitud_correccion = " + Constantes.Correccion.EstadoSolicitudcorreccion.VISTO_BUENO.ToString() + @" AND f.codigo_tipo_correccion = " + Constantes.Correccion.TipoCorreccion.ANULACION.ToString() + @") THEN 1
                              WHEN (x.correccion = 1 AND g.codigo_estado = @CodigoEstadoReporteCajaPorRevisar AND x.codigo_estado_solicitud_correccion = " + Constantes.Correccion.EstadoSolicitudcorreccion.VISTO_BUENO.ToString() + @" AND f.codigo_tipo_correccion = " + Constantes.Correccion.TipoCorreccion.ANULACION.ToString() + @") THEN 1
                              ELSE 0
                            END AS permiso_anular,
                            g.codigo_estado AS codigo_estado_reporte_caja,
                            x.revisado,
                            COALESCE(x.codigo_transaccion_ant,0) AS codigo_transaccion_ant,
                            x.codigo_estado_solicitud_correccion,
                            h.nombre AS estado_solicitud_correccion,
                            COALESCE(f.codigo_transaccion_correcta,0) AS codigo_transaccion_correcta,
                            CASE
                              WHEN (x.correccion = 0 AND (g.codigo_estado = @CodigoEstadoReporteCajaPorRevisar OR g.codigo_estado = @CodigoEstadoReporteCajaVistoBueno) AND 1 = " + esSuperAdmin.ToString() + @") THEN 1
                              WHEN (x.correccion = 0 AND g.codigo_estado = @CodigoEstadoReporteCajaPorRevisar) THEN 1
                              ELSE 0
                            END AS permiso_corregir,
                            x.correccion,
                            x.codigo_seguridad,
                            CASE
                              WHEN e.signo = 1 THEN 'INGRESO'  
                              WHEN e.signo = -1 THEN 'EGRESO'  
                              ELSE 'NEUTRO'
                            END AS tipo_operacion_contable,
                            x.monto_saldo_anterior_cxc,
                            x.monto_saldo_actual_cxc,
                            x.numero_cuenta

                    FROM db_tesoreria.transaccion x
                    INNER JOIN db_tesoreria.operacion w
                    ON x.codigo_operacion = w.codigo_operacion
                    LEFT JOIN db_contabilidad.tipo_cxc a
                    ON x.codigo_tipo_cxc = a.codigo_tipo_cxc
                    LEFT JOIN db_rrhh.area b
                    ON x.codigo_area = b.codigo_area
                    INNER JOIN db_tesoreria.estado_transaccion c
                    ON x.codigo_estado = c.codigo_estado_transaccion
                    INNER JOIN db_tesoreria.entidad_categoria d
                    ON x.codigo_categoria_entidad = d.codigo_categoria_entidad
                    INNER JOIN db_tesoreria.tipo_operacion e
                    ON w.codigo_tipo_operacion = e.codigo_tipo_operacion
                    LEFT JOIN db_tesoreria.solicitud_correccion f
                    ON x.codigo_transaccion = f.codigo_transaccion
                    INNER JOIN db_tesoreria.reporte_caja g
                    ON x.codigo_reporte = g.codigo_reporte
                    INNER JOIN db_tesoreria.estado_solicitud_correccion h
                    ON x.codigo_estado_solicitud_correccion = h.codigo_estado_solicitud_correccion
                    WHERE x.codigo_estado = @CodigoEstadoIncluidoEnReporte
                      AND x.anio_operacion = @AnioOperacion
                      AND x.semana_operacion = @SemanaOperacion  
                      AND x.codigo_reporte = @CodigoReporte
                      AND x.complemento_Conta = 0
                    " + filterOperaciones + @"    
                    " + filterCategoriaEntidad + @"
                    ORDER BY x.codigo_transaccion DESC";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@AnioOperacion", anioOperacion);
                        cmd.Parameters.AddWithValue("@SemanaOperacion", semanaOperacion);
                        cmd.Parameters.AddWithValue("@CodigoReporte", codigoReporte);
                        cmd.Parameters.AddWithValue("@CodigoEstadoIncluidoEnReporte", Constantes.EstadoTransacccion.INCLUIDO_EN_REPORTE);
                        cmd.Parameters.AddWithValue("@CodigoEstadoReporteCajaPorRevisar", Constantes.ReporteCaja.Estado.POR_REVISAR);
                        cmd.Parameters.AddWithValue("@CodigoEstadoReporteCajaVistoBueno", Constantes.ReporteCaja.Estado.VISTO_BUENO);
                        cmd.Parameters.AddWithValue("@CodigoEstadoSolicitudCorreccionVistoBueno", Constantes.Correccion.EstadoSolicitudcorreccion.VISTO_BUENO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            TransaccionCLS objTransaccion;
                            lista = new List<TransaccionCLS>();
                            int postCodigoTransaccion = dr.GetOrdinal("codigo_transaccion");
                            int postCodigoTipoTransaccion = dr.GetOrdinal("codigo_tipo_transaccion");
                            int postSerieFactura = dr.GetOrdinal("serie_factura");
                            int postNumeroDocumento = dr.GetOrdinal("numero_documento");
                            int postFechaDocumento = dr.GetOrdinal("fecha_documento");
                            int postNumeroRecibo = dr.GetOrdinal("numero_recibo");
                            int postNumeroReciboStr = dr.GetOrdinal("numero_recibo_str");
                            int postFechaRecibo = dr.GetOrdinal("fecha_recibo");
                            int postFechaReciboStr = dr.GetOrdinal("fecha_recibo_str");
                            int postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            int postEntidad = dr.GetOrdinal("entidad");
                            int postCodigoCategoriaEntidad = dr.GetOrdinal("codigo_categoria_entidad");
                            int postCategoriaEntidad = dr.GetOrdinal("categoria_entidad");
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postOperacion = dr.GetOrdinal("operacion");
                            int postCodigoTipoCuentaPorCobrar = dr.GetOrdinal("codigo_tipo_cxc");
                            int postTipoCuentaPorCobrar = dr.GetOrdinal("tipo_cxc");
                            int postCodigoCuentaPorCobrar = dr.GetOrdinal("codigo_cxc");
                            int postCodigoArea = dr.GetOrdinal("codigo_area");
                            int postArea = dr.GetOrdinal("area");
                            int postFechaOperacion = dr.GetOrdinal("fecha_operacion");
                            int postFechaOperacionStr = dr.GetOrdinal("fecha_operacion_str");
                            int postDiaOperacion = dr.GetOrdinal("dia_operacion");
                            int postNombreDiaOperacion = dr.GetOrdinal("nombre_dia_operacion");
                            int postMonto = dr.GetOrdinal("monto");
                            int postCodigoEstado = dr.GetOrdinal("codigo_estado");
                            int postEstado = dr.GetOrdinal("estado");
                            int postFechaIng = dr.GetOrdinal("fecha_ing");
                            int postUsuarioIng = dr.GetOrdinal("usuario_ing");
                            int postFechaIngStr = dr.GetOrdinal("fecha_ing_str");
                            int postSigno = dr.GetOrdinal("signo");
                            int postRuta = dr.GetOrdinal("ruta");
                            int postFechaImpresionStr = dr.GetOrdinal("fecha_impresion_str");
                            int postRecursos = dr.GetOrdinal("recursos");
                            int postComplementoConta = dr.GetOrdinal("complemento_conta");
                            int postEsComplementoDeInformacion = dr.GetOrdinal("es_complemento_de_informacion");
                            int postPermisoEditar = dr.GetOrdinal("permiso_editar");
                            int postCodigoEstadoReporteCaja = dr.GetOrdinal("codigo_estado_reporte_caja");
                            int postRevisado = dr.GetOrdinal("revisado");
                            int postCodigoTransaccionAnt = dr.GetOrdinal("codigo_transaccion_ant");
                            int postCodigoTransaccionCorrecta = dr.GetOrdinal("codigo_transaccion_correcta");
                            int postCodigoEstadoSolicitudCorreccion = dr.GetOrdinal("codigo_estado_solicitud_correccion");
                            int postEstadoSolicitudCorreccion = dr.GetOrdinal("estado_solicitud_correccion");
                            int postPermisoCorregir = dr.GetOrdinal("permiso_corregir");
                            int postCorreccion = dr.GetOrdinal("correccion");
                            int postPermisoAnular = dr.GetOrdinal("permiso_anular");
                            int postCodigoSeguridad = dr.GetOrdinal("codigo_seguridad");
                            int postTipoOperacionContable = dr.GetOrdinal("tipo_operacion_contable");
                            int postMontoSaldoAnteriorCxC = dr.GetOrdinal("monto_saldo_anterior_cxc");
                            int postMontoSaldoActualCxC = dr.GetOrdinal("monto_saldo_actual_cxc");
                            int postNumeroCuenta = dr.GetOrdinal("numero_cuenta");

                            while (dr.Read())
                            {
                                objTransaccion = new TransaccionCLS();
                                objTransaccion.CodigoTransaccion = dr.GetInt64(postCodigoTransaccion);
                                objTransaccion.CodigoTipoTransaccion = dr.GetString(postCodigoTipoTransaccion);
                                objTransaccion.SerieFactura = dr.IsDBNull(postSerieFactura) ? "" : dr.GetString(postSerieFactura);
                                objTransaccion.NumeroDocumento = dr.IsDBNull(postNumeroDocumento) ? null : dr.GetInt32(postNumeroDocumento);
                                objTransaccion.FechaDocumento = dr.IsDBNull(postFechaDocumento) ? null : dr.GetDateTime(postFechaDocumento);
                                objTransaccion.NumeroRecibo = dr.GetInt64(postNumeroRecibo);
                                objTransaccion.NumeroReciboStr = dr.GetString(postNumeroReciboStr);
                                objTransaccion.FechaRecibo = dr.GetDateTime(postFechaRecibo);
                                objTransaccion.FechaReciboStr = dr.GetString(postFechaReciboStr);
                                objTransaccion.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objTransaccion.NombreEntidad = dr.IsDBNull(postEntidad) ? "" : dr.GetString(postEntidad);
                                objTransaccion.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objTransaccion.CategoriaEntidad = dr.GetString(postCategoriaEntidad);
                                objTransaccion.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objTransaccion.Operacion = dr.GetString(postOperacion);
                                objTransaccion.CodigoTipoCuentaPorCobrar = dr.GetByte(postCodigoTipoCuentaPorCobrar);
                                objTransaccion.TipoCuentaPorCobrar = dr.GetString(postTipoCuentaPorCobrar);
                                objTransaccion.CodigoCuentaPorCobrar = dr.IsDBNull(postCodigoCuentaPorCobrar) ? -1 : dr.GetInt64(postCodigoCuentaPorCobrar);
                                objTransaccion.CodigoArea = dr.GetInt16(postCodigoArea);
                                objTransaccion.Area = dr.GetString(postArea);
                                objTransaccion.FechaOperacion = dr.GetDateTime(postFechaOperacion);
                                objTransaccion.FechaStr = dr.GetString(postFechaOperacionStr);
                                objTransaccion.DiaOperacion = dr.GetByte(postDiaOperacion);
                                objTransaccion.NombreDiaOperacion = dr.GetString(postNombreDiaOperacion);
                                objTransaccion.Monto = dr.GetDecimal(postMonto);
                                objTransaccion.CodigoEstado = dr.GetInt16(postCodigoEstado);
                                objTransaccion.Estado = dr.GetString(postEstado);
                                objTransaccion.FechaIng = dr.GetDateTime(postFechaIng);
                                objTransaccion.FechaIngStr = dr.GetString(postFechaIngStr);
                                objTransaccion.UsuarioIng = dr.GetString(postUsuarioIng);
                                objTransaccion.Signo = dr.GetInt16(postSigno);
                                objTransaccion.Ruta = dr.GetInt16(postRuta);
                                objTransaccion.FechaImpresionStr = dr.GetString(postFechaImpresionStr);
                                objTransaccion.Recursos = dr.GetString(postRecursos);
                                objTransaccion.ComplementoConta = dr.GetByte(postComplementoConta);
                                objTransaccion.EscomplementoDeInformacion = dr.GetString(postEsComplementoDeInformacion);
                                objTransaccion.PermisoEditar = (byte)dr.GetInt32(postPermisoEditar);
                                objTransaccion.CodigoEstadoReporteCaja = dr.GetByte(postCodigoEstadoReporteCaja);
                                objTransaccion.Revisado = dr.GetByte(postRevisado);
                                objTransaccion.CodigoTransaccionAnt = dr.GetInt64(postCodigoTransaccionAnt);
                                objTransaccion.CodigoTransaccionCorrecta = dr.GetInt64(postCodigoTransaccionCorrecta);
                                objTransaccion.CodigoEstadoSolicitudCorreccion = dr.GetByte(postCodigoEstadoSolicitudCorreccion);
                                objTransaccion.EstadoSolicitudCorreccion = dr.GetString(postEstadoSolicitudCorreccion);
                                objTransaccion.PermisoCorregir = (byte)dr.GetInt32(postPermisoCorregir);
                                objTransaccion.Correccion = dr.GetByte(postCorreccion);
                                objTransaccion.PermisoAnular = (byte)dr.GetInt32(postPermisoAnular);
                                objTransaccion.CodigoSeguridad = dr.GetString(postCodigoSeguridad);
                                objTransaccion.TipoOperacionContable = dr.GetString(postTipoOperacionContable);
                                objTransaccion.MontoSaldoAnteriorCxC = dr.GetDecimal(postMontoSaldoAnteriorCxC);
                                objTransaccion.MontoSaldoActualCxC = dr.GetDecimal(postMontoSaldoActualCxC);
                                objTransaccion.NumeroCuenta = dr.IsDBNull(postNumeroCuenta) ? "" : dr.GetString(postNumeroCuenta);

                                lista.Add(objTransaccion);
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

        public List<TransaccionCLS> BuscarTransaccionesDepositosBancarios(int anioOperacion, int semanaOperacion, int codigoReporte, int esSuperAdmin)
        {
            List<TransaccionCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string filterCategoriaEntidad = string.Empty;
                    string filterEstadoTrasaccion = string.Empty;
                    string sql = @"
                    SELECT  x.codigo_transaccion,
		                    x.codigo_tipo_transaccion,
                            x.serie_factura,
    	                    x.numero_documento,
		                    x.fecha_documento,
		                    x.numero_recibo,
		                    x.fecha_recibo,
                            FORMAT(x.fecha_recibo,'dd/MM/yyyy') AS fecha_recibo_str,
                            REPLACE(STR(CAST(x.numero_recibo AS varchar), 6),' ','0') AS numero_recibo_str,
		                    x.codigo_entidad,
                            db_tesoreria.GetNombreEntidad(x.codigo_categoria_entidad, x.codigo_entidad) AS entidad,
                            x.codigo_categoria_entidad,
                            d.nombre AS categoria_entidad,
		                    x.codigo_operacion,
		                    w.nombre_operacion AS operacion,
                            w.nombre_reporte_caja AS operacion_caja,
		                    x.codigo_tipo_cxc,
		                    a.nombre AS tipo_cxc,
                            x.codigo_cxc,
		                    x.codigo_area,
		                    b.nombre AS area,
		                    x.fecha_operacion,
                            FORMAT(x.fecha_operacion,'dd/MM/yyyy') AS fecha_operacion_str,
                            CASE
                              WHEN x.dia_operacion = 1 THEN 'LUNES'
                              WHEN x.dia_operacion = 2 THEN 'MARTES'
                              WHEN x.dia_operacion = 3 THEN 'MIERCOLES'
                              WHEN x.dia_operacion = 4 THEN 'JUEVES'
                              WHEN x.dia_operacion = 5 THEN 'VIERNES'
                              WHEN x.dia_operacion = 6 THEN 'SABADO' 
                              WHEN x.dia_operacion = 7 THEN 'DOMINGO' 
                              ELSE 'NO DEFINIDO'
                            END AS nombre_dia_operacion,
		                    x.monto,
		                    x.codigo_estado,
		                    c.nombre AS estado,
                            x.fecha_ing,
                            x.usuario_ing,
                            FORMAT(x.fecha_ing, 'dd/MM/yyyy, hh:mm:ss') AS fecha_ing_str,
                            e.signo,
                            x.ruta,
                            FORMAT(GETDATE(), 'dd/MM/yyyy, hh:mm:ss') AS fecha_impresion_str,
                            CONCAT(CASE WHEN x.efectivo = 1 THEN 'Efectivo,' ELSE '' END,CASE WHEN x.deposito = 1 THEN 'Depósito,' ELSE '' END,CASE WHEN x.cheque = 1 THEN 'Cheque' ELSE '' END) AS recursos,
                            x.complemento_conta,
                            CASE 
                               WHEN x.complemento_conta = 1 THEN 'SI'
                               ELSE 'NO' 
                            END AS es_complemento_de_informacion,
                            g.codigo_estado AS codigo_estado_reporte_caja,
                            x.revisado,
                            COALESCE(x.codigo_transaccion_ant,0) AS codigo_transaccion_ant,
                            x.codigo_estado_solicitud_correccion,
                            h.nombre AS estado_solicitud_correccion,
                            COALESCE(f.codigo_transaccion_correcta,0) AS codigo_transaccion_correcta,
                            x.correccion,
                            x.codigo_tipo_doc_deposito,
                            x.numero_boleta,
                            x.numero_voucher,
                            x.numero_cuenta,
                            x.monto_efectivo,
                            x.monto_cheques,
                            CASE
                              WHEN (g.codigo_estado = @CodigoEstadoReportePorRevisar OR 1 = " + esSuperAdmin.ToString() + @") THEN 1
                              ELSE 0
                            END AS permiso_actualizar
                    FROM db_tesoreria.transaccion x
                    INNER JOIN db_tesoreria.operacion w
                    ON x.codigo_operacion = w.codigo_operacion
                    LEFT JOIN db_contabilidad.tipo_cxc a
                    ON x.codigo_tipo_cxc = a.codigo_tipo_cxc
                    LEFT JOIN db_rrhh.area b
                    ON x.codigo_area = b.codigo_area
                    INNER JOIN db_tesoreria.estado_transaccion c
                    ON x.codigo_estado = c.codigo_estado_transaccion
                    INNER JOIN db_tesoreria.entidad_categoria d
                    ON x.codigo_categoria_entidad = d.codigo_categoria_entidad
                    INNER JOIN db_tesoreria.tipo_operacion e
                    ON w.codigo_tipo_operacion = e.codigo_tipo_operacion
                    LEFT JOIN db_tesoreria.solicitud_correccion f
                    ON x.codigo_transaccion = f.codigo_transaccion
                    INNER JOIN db_tesoreria.reporte_caja g
                    ON x.codigo_reporte = g.codigo_reporte
                    INNER JOIN db_tesoreria.estado_solicitud_correccion h
                    ON x.codigo_estado_solicitud_correccion = h.codigo_estado_solicitud_correccion
                    WHERE x.codigo_estado = @CodigoEstadoIncluidoEnReporte
                      AND x.anio_operacion = @AnioOperacion
                      AND x.semana_operacion = @SemanaOperacion  
                      AND x.codigo_reporte = @CodigoReporte
                      AND x.complemento_Conta = 0
                      AND x.codigo_operacion =  @CodigoOperacionDepositoBancario
                    ORDER BY x.codigo_transaccion DESC";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@AnioOperacion", anioOperacion);
                        cmd.Parameters.AddWithValue("@SemanaOperacion", semanaOperacion);
                        cmd.Parameters.AddWithValue("@CodigoReporte", codigoReporte);
                        cmd.Parameters.AddWithValue("@CodigoEstadoIncluidoEnReporte", Constantes.EstadoTransacccion.INCLUIDO_EN_REPORTE);
                        cmd.Parameters.AddWithValue("@CodigoOperacionDepositoBancario", Constantes.Operacion.Egreso.DEPOSITOS_BANCARIOS);
                        cmd.Parameters.AddWithValue("@CodigoEstadoReportePorRevisar", Constantes.ReporteCaja.Estado.POR_REVISAR);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            TransaccionCLS objTransaccion;
                            lista = new List<TransaccionCLS>();
                            int postCodigoTransaccion = dr.GetOrdinal("codigo_transaccion");
                            int postCodigoTipoTransaccion = dr.GetOrdinal("codigo_tipo_transaccion");
                            int postSerieFactura = dr.GetOrdinal("serie_factura");
                            int postNumeroDocumento = dr.GetOrdinal("numero_documento");
                            int postFechaDocumento = dr.GetOrdinal("fecha_documento");
                            int postNumeroRecibo = dr.GetOrdinal("numero_recibo");
                            int postNumeroReciboStr = dr.GetOrdinal("numero_recibo_str");
                            int postFechaRecibo = dr.GetOrdinal("fecha_recibo");
                            int postFechaReciboStr = dr.GetOrdinal("fecha_recibo_str");
                            int postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            int postEntidad = dr.GetOrdinal("entidad");
                            int postCodigoCategoriaEntidad = dr.GetOrdinal("codigo_categoria_entidad");
                            int postCategoriaEntidad = dr.GetOrdinal("categoria_entidad");
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postOperacion = dr.GetOrdinal("operacion");
                            int postCodigoTipoCuentaPorCobrar = dr.GetOrdinal("codigo_tipo_cxc");
                            int postTipoCuentaPorCobrar = dr.GetOrdinal("tipo_cxc");
                            int postCodigoCuentaPorCobrar = dr.GetOrdinal("codigo_cxc");
                            int postCodigoArea = dr.GetOrdinal("codigo_area");
                            int postArea = dr.GetOrdinal("area");
                            int postFechaOperacion = dr.GetOrdinal("fecha_operacion");
                            int postFechaOperacionStr = dr.GetOrdinal("fecha_operacion_str");
                            int postNombreDiaOperacion = dr.GetOrdinal("nombre_dia_operacion");
                            int postMonto = dr.GetOrdinal("monto");
                            int postCodigoEstado = dr.GetOrdinal("codigo_estado");
                            int postEstado = dr.GetOrdinal("estado");
                            int postFechaIng = dr.GetOrdinal("fecha_ing");
                            int postUsuarioIng = dr.GetOrdinal("usuario_ing");
                            int postFechaIngStr = dr.GetOrdinal("fecha_ing_str");
                            int postSigno = dr.GetOrdinal("signo");
                            int postRuta = dr.GetOrdinal("ruta");
                            int postFechaImpresionStr = dr.GetOrdinal("fecha_impresion_str");
                            int postRecursos = dr.GetOrdinal("recursos");
                            int postComplementoConta = dr.GetOrdinal("complemento_conta");
                            int postEsComplementoDeInformacion = dr.GetOrdinal("es_complemento_de_informacion");
                            int postCodigoEstadoReporteCaja = dr.GetOrdinal("codigo_estado_reporte_caja");
                            int postRevisado = dr.GetOrdinal("revisado");
                            int postCodigoTransaccionAnt = dr.GetOrdinal("codigo_transaccion_ant");
                            int postCodigoTransaccionCorrecta = dr.GetOrdinal("codigo_transaccion_correcta");
                            int postCodigoEstadoSolicitudCorreccion = dr.GetOrdinal("codigo_estado_solicitud_correccion");
                            int postEstadoSolicitudCorreccion = dr.GetOrdinal("estado_solicitud_correccion");
                            int postCorreccion = dr.GetOrdinal("correccion");
                            int postCodigoTipoDocumentoDeposito = dr.GetOrdinal("codigo_tipo_doc_deposito");
                            int postNumeroCuenta = dr.GetOrdinal("numero_cuenta");
                            int postNumeroBoleta = dr.GetOrdinal("numero_boleta");
                            int postNumeroVoucher = dr.GetOrdinal("numero_voucher");
                            int postMontoEfectivo = dr.GetOrdinal("monto_efectivo");
                            int postMontoCheques = dr.GetOrdinal("monto_cheques");
                            int postPermisoActualizar = dr.GetOrdinal("permiso_actualizar");

                            while (dr.Read())
                            {
                                objTransaccion = new TransaccionCLS();
                                objTransaccion.CodigoTransaccion = dr.GetInt64(postCodigoTransaccion);
                                objTransaccion.CodigoTipoTransaccion = dr.GetString(postCodigoTipoTransaccion);
                                objTransaccion.SerieFactura = dr.IsDBNull(postSerieFactura) ? "" : dr.GetString(postSerieFactura);
                                objTransaccion.NumeroDocumento = dr.IsDBNull(postNumeroDocumento) ? null : dr.GetInt32(postNumeroDocumento);
                                objTransaccion.FechaDocumento = dr.IsDBNull(postFechaDocumento) ? null : dr.GetDateTime(postFechaDocumento);
                                objTransaccion.NumeroRecibo = dr.GetInt64(postNumeroRecibo);
                                objTransaccion.NumeroReciboStr = dr.GetString(postNumeroReciboStr);
                                objTransaccion.FechaRecibo = dr.GetDateTime(postFechaRecibo);
                                objTransaccion.FechaReciboStr = dr.GetString(postFechaReciboStr);
                                objTransaccion.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objTransaccion.NombreEntidad = dr.IsDBNull(postEntidad) ? "" : dr.GetString(postEntidad);
                                objTransaccion.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objTransaccion.CategoriaEntidad = dr.GetString(postCategoriaEntidad);
                                objTransaccion.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objTransaccion.Operacion = dr.GetString(postOperacion);
                                objTransaccion.CodigoTipoCuentaPorCobrar = dr.GetByte(postCodigoTipoCuentaPorCobrar);
                                objTransaccion.TipoCuentaPorCobrar = dr.GetString(postTipoCuentaPorCobrar);
                                objTransaccion.CodigoCuentaPorCobrar = dr.IsDBNull(postCodigoCuentaPorCobrar) ? -1 : dr.GetInt64(postCodigoCuentaPorCobrar);
                                objTransaccion.CodigoArea = dr.GetInt16(postCodigoArea);
                                objTransaccion.Area = dr.GetString(postArea);
                                objTransaccion.FechaOperacion = dr.GetDateTime(postFechaOperacion);
                                objTransaccion.FechaStr = dr.GetString(postFechaOperacionStr);
                                objTransaccion.NombreDiaOperacion = dr.GetString(postNombreDiaOperacion);
                                objTransaccion.Monto = dr.GetDecimal(postMonto);
                                objTransaccion.CodigoEstado = dr.GetInt16(postCodigoEstado);
                                objTransaccion.Estado = dr.GetString(postEstado);
                                objTransaccion.FechaIng = dr.GetDateTime(postFechaIng);
                                objTransaccion.FechaIngStr = dr.GetString(postFechaIngStr);
                                objTransaccion.UsuarioIng = dr.GetString(postUsuarioIng);
                                objTransaccion.Signo = dr.GetInt16(postSigno);
                                objTransaccion.Ruta = dr.GetInt16(postRuta);
                                objTransaccion.FechaImpresionStr = dr.GetString(postFechaImpresionStr);
                                objTransaccion.Recursos = dr.GetString(postRecursos);
                                objTransaccion.ComplementoConta = dr.GetByte(postComplementoConta);
                                objTransaccion.EscomplementoDeInformacion = dr.GetString(postEsComplementoDeInformacion);
                                objTransaccion.CodigoEstadoReporteCaja = dr.GetByte(postCodigoEstadoReporteCaja);
                                objTransaccion.Revisado = dr.GetByte(postRevisado);
                                objTransaccion.CodigoTransaccionAnt = dr.GetInt64(postCodigoTransaccionAnt);
                                objTransaccion.CodigoTransaccionCorrecta = dr.GetInt64(postCodigoTransaccionCorrecta);
                                objTransaccion.CodigoEstadoSolicitudCorreccion = dr.GetByte(postCodigoEstadoSolicitudCorreccion);
                                objTransaccion.EstadoSolicitudCorreccion = dr.GetString(postEstadoSolicitudCorreccion);
                                objTransaccion.Correccion = dr.GetByte(postCorreccion);
                                objTransaccion.CodigoTipoDocumentoDeposito = dr.GetByte(postCodigoTipoDocumentoDeposito);
                                objTransaccion.NumeroCuenta = dr.IsDBNull(postNumeroCuenta) ? "" : dr.GetString(postNumeroCuenta);
                                objTransaccion.NumeroBoleta = dr.IsDBNull(postNumeroBoleta) ? "" : dr.GetString(postNumeroBoleta);
                                objTransaccion.NumeroVoucher = dr.IsDBNull(postNumeroVoucher) ? "" : dr.GetString(postNumeroVoucher);
                                objTransaccion.MontoEfectivo = dr.GetDecimal(postMontoEfectivo);
                                objTransaccion.MontoCheques = dr.GetDecimal(postMontoCheques);
                                objTransaccion.PermisoActualizar = dr.GetInt32(postPermisoActualizar);

                                lista.Add(objTransaccion);
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


        public List<TransaccionCLS> BuscarTransaccionesGasto(int anioOperacion, int semanaOperacion, int codigoReporte, int esSuperAdmin)
        {
            List<TransaccionCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string filterCategoriaEntidad = string.Empty;
                    string filterEstadoTrasaccion = string.Empty;
                    string sql = @"
                    SELECT  x.codigo_transaccion,
		                    x.codigo_tipo_transaccion,
                            x.serie_factura,
    	                    x.numero_documento,
		                    x.fecha_documento,
		                    x.numero_recibo,
		                    x.fecha_recibo,
                            FORMAT(x.fecha_recibo,'dd/MM/yyyy') AS fecha_recibo_str,
                            REPLACE(STR(CAST(x.numero_recibo AS varchar), 10),' ','0') AS numero_recibo_str,
		                    x.codigo_entidad,
                            db_tesoreria.GetNombreEntidad(x.codigo_categoria_entidad, x.codigo_entidad) AS entidad,
                            x.codigo_categoria_entidad,
                            d.nombre AS categoria_entidad,
		                    x.codigo_operacion,
		                    w.nombre_operacion AS operacion,
                            w.nombre_reporte_caja AS operacion_caja,
		                    x.codigo_tipo_cxc,
		                    a.nombre AS tipo_cxc,
                            x.codigo_cxc,
		                    x.codigo_area,
		                    b.nombre AS area,
		                    x.fecha_operacion,
                            FORMAT(x.fecha_operacion,'dd/MM/yyyy') AS fecha_operacion_str,
                            CASE
                              WHEN x.dia_operacion = 1 THEN 'LUNES'
                              WHEN x.dia_operacion = 2 THEN 'MARTES'
                              WHEN x.dia_operacion = 3 THEN 'MIERCOLES'
                              WHEN x.dia_operacion = 4 THEN 'JUEVES'
                              WHEN x.dia_operacion = 5 THEN 'VIERNES'
                              WHEN x.dia_operacion = 6 THEN 'SABADO' 
                              WHEN x.dia_operacion = 7 THEN 'DOMINGO' 
                              ELSE 'NO DEFINIDO'
                            END AS nombre_dia_operacion,
		                    x.monto,
		                    x.codigo_estado,
		                    c.nombre AS estado,
                            x.fecha_ing,
                            x.usuario_ing,
                            FORMAT(x.fecha_ing, 'dd/MM/yyyy, hh:mm:ss') AS fecha_ing_str,
                            e.signo,
                            x.ruta,
                            FORMAT(GETDATE(), 'dd/MM/yyyy, hh:mm:ss') AS fecha_impresion_str,
                            CONCAT(CASE WHEN x.efectivo = 1 THEN 'Efectivo,' ELSE '' END,CASE WHEN x.deposito = 1 THEN 'Depósito,' ELSE '' END,CASE WHEN x.cheque = 1 THEN 'Cheque' ELSE '' END) AS recursos,
                            x.complemento_conta,
                            CASE 
                               WHEN x.complemento_conta = 1 THEN 'SI'
                               ELSE 'NO' 
                            END AS es_complemento_de_informacion,
                            g.codigo_estado AS codigo_estado_reporte_caja,
                            x.revisado,
                            COALESCE(x.codigo_transaccion_ant,0) AS codigo_transaccion_ant,
                            x.codigo_estado_solicitud_correccion,
                            h.nombre AS estado_solicitud_correccion,
                            COALESCE(f.codigo_transaccion_correcta,0) AS codigo_transaccion_correcta,
                            x.correccion,
                            x.codigo_tipo_doc_deposito,
                            x.numero_boleta,
                            x.numero_voucher,
                            x.numero_cuenta,
                            x.monto_efectivo,
                            x.monto_cheques,
                            CASE
                              WHEN (g.codigo_estado = @CodigoEstadoReportePorRevisar OR 1 = " + esSuperAdmin.ToString() + @") THEN 1
                              ELSE 0
                            END AS permiso_actualizar
                    FROM db_tesoreria.transaccion x
                    INNER JOIN db_tesoreria.operacion w
                    ON x.codigo_operacion = w.codigo_operacion
                    LEFT JOIN db_contabilidad.tipo_cxc a
                    ON x.codigo_tipo_cxc = a.codigo_tipo_cxc
                    LEFT JOIN db_rrhh.area b
                    ON x.codigo_area = b.codigo_area
                    INNER JOIN db_tesoreria.estado_transaccion c
                    ON x.codigo_estado = c.codigo_estado_transaccion
                    INNER JOIN db_tesoreria.entidad_categoria d
                    ON x.codigo_categoria_entidad = d.codigo_categoria_entidad
                    INNER JOIN db_tesoreria.tipo_operacion e
                    ON w.codigo_tipo_operacion = e.codigo_tipo_operacion
                    LEFT JOIN db_tesoreria.solicitud_correccion f
                    ON x.codigo_transaccion = f.codigo_transaccion
                    INNER JOIN db_tesoreria.reporte_caja g
                    ON x.codigo_reporte = g.codigo_reporte
                    INNER JOIN db_tesoreria.estado_solicitud_correccion h
                    ON x.codigo_estado_solicitud_correccion = h.codigo_estado_solicitud_correccion
                    WHERE x.codigo_estado = @CodigoEstadoIncluidoEnReporte
                      AND x.anio_operacion = @AnioOperacion
                      AND x.semana_operacion = @SemanaOperacion  
                      AND x.codigo_reporte = @CodigoReporte
                      AND e.signo = -1  
                    ORDER BY x.codigo_transaccion DESC";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@AnioOperacion", anioOperacion);
                        cmd.Parameters.AddWithValue("@SemanaOperacion", semanaOperacion);
                        cmd.Parameters.AddWithValue("@CodigoReporte", codigoReporte);
                        cmd.Parameters.AddWithValue("@CodigoEstadoIncluidoEnReporte", Constantes.EstadoTransacccion.INCLUIDO_EN_REPORTE);
                        cmd.Parameters.AddWithValue("@CodigoEstadoReportePorRevisar", Constantes.ReporteCaja.Estado.POR_REVISAR);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            TransaccionCLS objTransaccion;
                            lista = new List<TransaccionCLS>();
                            int postCodigoTransaccion = dr.GetOrdinal("codigo_transaccion");
                            int postCodigoTipoTransaccion = dr.GetOrdinal("codigo_tipo_transaccion");
                            int postSerieFactura = dr.GetOrdinal("serie_factura");
                            int postNumeroDocumento = dr.GetOrdinal("numero_documento");
                            int postFechaDocumento = dr.GetOrdinal("fecha_documento");
                            int postNumeroRecibo = dr.GetOrdinal("numero_recibo");
                            int postNumeroReciboStr = dr.GetOrdinal("numero_recibo_str");
                            int postFechaRecibo = dr.GetOrdinal("fecha_recibo");
                            int postFechaReciboStr = dr.GetOrdinal("fecha_recibo_str");
                            int postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            int postEntidad = dr.GetOrdinal("entidad");
                            int postCodigoCategoriaEntidad = dr.GetOrdinal("codigo_categoria_entidad");
                            int postCategoriaEntidad = dr.GetOrdinal("categoria_entidad");
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postOperacion = dr.GetOrdinal("operacion");
                            int postCodigoTipoCuentaPorCobrar = dr.GetOrdinal("codigo_tipo_cxc");
                            int postTipoCuentaPorCobrar = dr.GetOrdinal("tipo_cxc");
                            int postCodigoCuentaPorCobrar = dr.GetOrdinal("codigo_cxc");
                            int postCodigoArea = dr.GetOrdinal("codigo_area");
                            int postArea = dr.GetOrdinal("area");
                            int postFechaOperacion = dr.GetOrdinal("fecha_operacion");
                            int postFechaOperacionStr = dr.GetOrdinal("fecha_operacion_str");
                            int postNombreDiaOperacion = dr.GetOrdinal("nombre_dia_operacion");
                            int postMonto = dr.GetOrdinal("monto");
                            int postCodigoEstado = dr.GetOrdinal("codigo_estado");
                            int postEstado = dr.GetOrdinal("estado");
                            int postFechaIng = dr.GetOrdinal("fecha_ing");
                            int postUsuarioIng = dr.GetOrdinal("usuario_ing");
                            int postFechaIngStr = dr.GetOrdinal("fecha_ing_str");
                            int postSigno = dr.GetOrdinal("signo");
                            int postRuta = dr.GetOrdinal("ruta");
                            int postFechaImpresionStr = dr.GetOrdinal("fecha_impresion_str");
                            int postRecursos = dr.GetOrdinal("recursos");
                            int postComplementoConta = dr.GetOrdinal("complemento_conta");
                            int postEsComplementoDeInformacion = dr.GetOrdinal("es_complemento_de_informacion");
                            int postCodigoEstadoReporteCaja = dr.GetOrdinal("codigo_estado_reporte_caja");
                            int postRevisado = dr.GetOrdinal("revisado");
                            int postCodigoTransaccionAnt = dr.GetOrdinal("codigo_transaccion_ant");
                            int postCodigoTransaccionCorrecta = dr.GetOrdinal("codigo_transaccion_correcta");
                            int postCodigoEstadoSolicitudCorreccion = dr.GetOrdinal("codigo_estado_solicitud_correccion");
                            int postEstadoSolicitudCorreccion = dr.GetOrdinal("estado_solicitud_correccion");
                            int postCorreccion = dr.GetOrdinal("correccion");
                            int postCodigoTipoDocumentoDeposito = dr.GetOrdinal("codigo_tipo_doc_deposito");
                            int postNumeroCuenta = dr.GetOrdinal("numero_cuenta");
                            int postNumeroBoleta = dr.GetOrdinal("numero_boleta");
                            int postNumeroVoucher = dr.GetOrdinal("numero_voucher");
                            int postMontoEfectivo = dr.GetOrdinal("monto_efectivo");
                            int postMontoCheques = dr.GetOrdinal("monto_cheques");
                            int postPermisoActualizar = dr.GetOrdinal("permiso_actualizar");

                            while (dr.Read())
                            {
                                objTransaccion = new TransaccionCLS();
                                objTransaccion.CodigoTransaccion = dr.GetInt64(postCodigoTransaccion);
                                objTransaccion.CodigoTipoTransaccion = dr.GetString(postCodigoTipoTransaccion);
                                objTransaccion.SerieFactura = dr.IsDBNull(postSerieFactura) ? "" : dr.GetString(postSerieFactura);
                                objTransaccion.NumeroDocumento = dr.IsDBNull(postNumeroDocumento) ? null : dr.GetInt32(postNumeroDocumento);
                                objTransaccion.FechaDocumento = dr.IsDBNull(postFechaDocumento) ? null : dr.GetDateTime(postFechaDocumento);
                                objTransaccion.NumeroRecibo = dr.GetInt64(postNumeroRecibo);
                                objTransaccion.NumeroReciboStr = dr.GetString(postNumeroReciboStr);
                                objTransaccion.FechaRecibo = dr.GetDateTime(postFechaRecibo);
                                objTransaccion.FechaReciboStr = dr.GetString(postFechaReciboStr);
                                objTransaccion.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objTransaccion.NombreEntidad = dr.IsDBNull(postEntidad) ? "" : dr.GetString(postEntidad);
                                objTransaccion.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objTransaccion.CategoriaEntidad = dr.GetString(postCategoriaEntidad);
                                objTransaccion.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objTransaccion.Operacion = dr.GetString(postOperacion);
                                objTransaccion.CodigoTipoCuentaPorCobrar = dr.GetByte(postCodigoTipoCuentaPorCobrar);
                                objTransaccion.TipoCuentaPorCobrar = dr.GetString(postTipoCuentaPorCobrar);
                                objTransaccion.CodigoCuentaPorCobrar = dr.IsDBNull(postCodigoCuentaPorCobrar) ? -1 : dr.GetInt64(postCodigoCuentaPorCobrar);
                                objTransaccion.CodigoArea = dr.GetInt16(postCodigoArea);
                                objTransaccion.Area = dr.GetString(postArea);
                                objTransaccion.FechaOperacion = dr.GetDateTime(postFechaOperacion);
                                objTransaccion.FechaStr = dr.GetString(postFechaOperacionStr);
                                objTransaccion.NombreDiaOperacion = dr.GetString(postNombreDiaOperacion);
                                objTransaccion.Monto = dr.GetDecimal(postMonto);
                                objTransaccion.CodigoEstado = dr.GetInt16(postCodigoEstado);
                                objTransaccion.Estado = dr.GetString(postEstado);
                                objTransaccion.FechaIng = dr.GetDateTime(postFechaIng);
                                objTransaccion.FechaIngStr = dr.GetString(postFechaIngStr);
                                objTransaccion.UsuarioIng = dr.GetString(postUsuarioIng);
                                objTransaccion.Signo = dr.GetInt16(postSigno);
                                objTransaccion.Ruta = dr.GetInt16(postRuta);
                                objTransaccion.FechaImpresionStr = dr.GetString(postFechaImpresionStr);
                                objTransaccion.Recursos = dr.GetString(postRecursos);
                                objTransaccion.ComplementoConta = dr.GetByte(postComplementoConta);
                                objTransaccion.EscomplementoDeInformacion = dr.GetString(postEsComplementoDeInformacion);
                                objTransaccion.CodigoEstadoReporteCaja = dr.GetByte(postCodigoEstadoReporteCaja);
                                objTransaccion.Revisado = dr.GetByte(postRevisado);
                                objTransaccion.CodigoTransaccionAnt = dr.GetInt64(postCodigoTransaccionAnt);
                                objTransaccion.CodigoTransaccionCorrecta = dr.GetInt64(postCodigoTransaccionCorrecta);
                                objTransaccion.CodigoEstadoSolicitudCorreccion = dr.GetByte(postCodigoEstadoSolicitudCorreccion);
                                objTransaccion.EstadoSolicitudCorreccion = dr.GetString(postEstadoSolicitudCorreccion);
                                objTransaccion.Correccion = dr.GetByte(postCorreccion);
                                objTransaccion.CodigoTipoDocumentoDeposito = dr.GetByte(postCodigoTipoDocumentoDeposito);
                                objTransaccion.NumeroCuenta = dr.IsDBNull(postNumeroCuenta) ? "" : dr.GetString(postNumeroCuenta);
                                objTransaccion.NumeroBoleta = dr.IsDBNull(postNumeroBoleta) ? "" : dr.GetString(postNumeroBoleta);
                                objTransaccion.NumeroVoucher = dr.IsDBNull(postNumeroVoucher) ? "" : dr.GetString(postNumeroVoucher);
                                objTransaccion.MontoEfectivo = dr.GetDecimal(postMontoEfectivo);
                                objTransaccion.MontoCheques = dr.GetDecimal(postMontoCheques);
                                objTransaccion.PermisoActualizar = dr.GetInt32(postPermisoActualizar);

                                lista.Add(objTransaccion);
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

        public List<TransaccionCLS> GetSolicitudesDeCorreccion(int anioOperacion, int semanaOperacion, int codigoReporte, int codigoOperacion, int codigoCategoriaEntidad, int esSuperAdmin)
        {
            List<TransaccionCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string filterAnioOperacion = string.Empty;
                    string filterSemanaOperacion = string.Empty;
                    string filterReporte = string.Empty;
                    string filterOperaciones = string.Empty;
                    string filterCategoriaEntidad = string.Empty;
                    string filterEstadoTrasaccion = string.Empty;
                    if (anioOperacion != -1)
                    {
                        filterAnioOperacion = "AND x.anio_operacion = " + anioOperacion.ToString();
                    }
                    if (semanaOperacion != -1)
                    {
                        filterSemanaOperacion = "AND x.semana_operacion = " + semanaOperacion.ToString();
                    }
                    if (codigoReporte != -1)
                    {
                        filterReporte = "AND x.codigo_reporte = " + codigoReporte.ToString();
                    }
                    if (codigoOperacion != -1)
                    {
                        filterOperaciones = " AND x.codigo_operacion = " + codigoOperacion.ToString();
                    }
                    if (codigoCategoriaEntidad != -1)
                    {
                        filterCategoriaEntidad = " AND x.codigo_categoria_entidad = " + codigoCategoriaEntidad.ToString();
                    }

                    string sql = @"
                    SELECT  x.codigo_transaccion,
		                    x.codigo_tipo_transaccion,
                            x.serie_factura,
    	                    x.numero_documento,
		                    x.fecha_documento,
		                    x.numero_recibo,
		                    x.fecha_recibo,
                            FORMAT(x.fecha_recibo,'dd/MM/yyyy') AS fecha_recibo_str,
                            REPLACE(STR(CAST(x.numero_recibo AS varchar), 10),' ','0') AS numero_recibo_str,
		                    x.codigo_entidad,
                            db_tesoreria.GetNombreEntidad(x.codigo_categoria_entidad, x.codigo_entidad) AS entidad,
                            x.codigo_categoria_entidad,
                            d.nombre AS categoria_entidad,
		                    x.codigo_operacion,
		                    w.nombre_operacion AS operacion,
                            w.nombre_reporte_caja AS operacion_caja,
		                    x.codigo_tipo_cxc,
		                    a.nombre AS tipo_cxc,
                            x.codigo_cxc,
		                    x.codigo_area,
		                    b.nombre AS area,
		                    x.fecha_operacion,
                            FORMAT(x.fecha_operacion,'dd/MM/yyyy') AS fecha_operacion_str,
                            CASE
                              WHEN x.dia_operacion = 1 THEN 'LUNES'
                              WHEN x.dia_operacion = 2 THEN 'MARTES'
                              WHEN x.dia_operacion = 3 THEN 'MIERCOLES'
                              WHEN x.dia_operacion = 4 THEN 'JUEVES'
                              WHEN x.dia_operacion = 5 THEN 'VIERNES'
                              WHEN x.dia_operacion = 6 THEN 'SABADO' 
                              WHEN x.dia_operacion = 7 THEN 'DOMINGO' 
                              ELSE 'NO DEFINIDO'
                            END AS nombre_dia_operacion,
		                    x.monto,
		                    x.codigo_estado,
		                    c.nombre AS estado,
                            x.fecha_ing,
                            x.usuario_ing,
                            FORMAT(x.fecha_ing, 'dd/MM/yyyy, hh:mm:ss') AS fecha_ing_str,
                            e.signo,
                            x.ruta,
                            FORMAT(GETDATE(), 'dd/MM/yyyy, hh:mm:ss') AS fecha_impresion_str,
                            CONCAT(CASE WHEN x.efectivo = 1 THEN 'Efectivo,' ELSE '' END,CASE WHEN x.deposito = 1 THEN 'Depósito,' ELSE '' END,CASE WHEN x.cheque = 1 THEN 'Cheque' ELSE '' END) AS recursos,
                            x.complemento_conta,
                            CASE 
                               WHEN x.complemento_conta = 1 THEN 'SI'
                               ELSE 'NO' 
                            END AS es_complemento_de_informacion,
                            0 AS permiso_editar,
                            g.codigo_estado AS codigo_estado_reporte_caja,
                            x.revisado,
                            COALESCE(x.codigo_transaccion_ant,0) AS codigo_transaccion_ant,
                            x.codigo_estado_solicitud_correccion,
                            h.nombre AS estado_solicitud_correccion,
                            COALESCE(f.codigo_transaccion_correcta,0) AS codigo_transaccion_correcta,
                            0 AS permiso_corregir,
                            CASE
                               WHEN x.codigo_estado_solicitud_correccion = @CodigoEstadoSolicitaAprobacion THEN 1
                               ELSE 0
                            END AS permiso_autorizar,
                            x.correccion,
                            COALESCE(f.codigo_tipo_correccion,0) AS codigo_tipo_correccion,
                            CASE
                              WHEN f.codigo_tipo_correccion = 1 THEN 'MODIFICACIÓN'
                              WHEN f.codigo_tipo_correccion = 2 THEN 'ANULACIÓN'
                              ELSE ''
                            END AS tipo_correccion

                    FROM db_tesoreria.transaccion x
                    INNER JOIN db_tesoreria.operacion w
                    ON x.codigo_operacion = w.codigo_operacion
                    LEFT JOIN db_contabilidad.tipo_cxc a
                    ON x.codigo_tipo_cxc = a.codigo_tipo_cxc
                    LEFT JOIN db_rrhh.area b
                    ON x.codigo_area = b.codigo_area
                    INNER JOIN db_tesoreria.estado_transaccion c
                    ON x.codigo_estado = c.codigo_estado_transaccion
                    INNER JOIN db_tesoreria.entidad_categoria d
                    ON x.codigo_categoria_entidad = d.codigo_categoria_entidad
                    INNER JOIN db_tesoreria.tipo_operacion e
                    ON w.codigo_tipo_operacion = e.codigo_tipo_operacion
                    LEFT JOIN db_tesoreria.solicitud_correccion f
                    ON x.codigo_transaccion = f.codigo_transaccion
                    INNER JOIN db_tesoreria.reporte_caja g
                    ON x.codigo_reporte = g.codigo_reporte
                    INNER JOIN db_tesoreria.estado_solicitud_correccion h
                    ON x.codigo_estado_solicitud_correccion = h.codigo_estado_solicitud_correccion
                    WHERE x.correccion = 1
                      AND x.complemento_Conta = 0
                    " + filterAnioOperacion + @"
                    " + filterSemanaOperacion + @"
                    " + filterReporte + @"
                    " + filterOperaciones + @"    
                    " + filterCategoriaEntidad + @"
                    ORDER BY x.codigo_transaccion DESC";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoSolicitaAprobacion", Constantes.Correccion.EstadoSolicitudcorreccion.SOLICITA_APROBACION);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            TransaccionCLS objTransaccion;
                            lista = new List<TransaccionCLS>();
                            int postCodigoTransaccion = dr.GetOrdinal("codigo_transaccion");
                            int postCodigoTipoTransaccion = dr.GetOrdinal("codigo_tipo_transaccion");
                            int postSerieFactura = dr.GetOrdinal("serie_factura");
                            int postNumeroDocumento = dr.GetOrdinal("numero_documento");
                            int postFechaDocumento = dr.GetOrdinal("fecha_documento");
                            int postNumeroRecibo = dr.GetOrdinal("numero_recibo");
                            int postNumeroReciboStr = dr.GetOrdinal("numero_recibo_str");
                            int postFechaRecibo = dr.GetOrdinal("fecha_recibo");
                            int postFechaReciboStr = dr.GetOrdinal("fecha_recibo_str");
                            int postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            int postEntidad = dr.GetOrdinal("entidad");
                            int postCodigoCategoriaEntidad = dr.GetOrdinal("codigo_categoria_entidad");
                            int postCategoriaEntidad = dr.GetOrdinal("categoria_entidad");
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postOperacion = dr.GetOrdinal("operacion");
                            int postCodigoTipoCuentaPorCobrar = dr.GetOrdinal("codigo_tipo_cxc");
                            int postTipoCuentaPorCobrar = dr.GetOrdinal("tipo_cxc");
                            int postCodigoCuentaPorCobrar = dr.GetOrdinal("codigo_cxc");
                            int postCodigoArea = dr.GetOrdinal("codigo_area");
                            int postArea = dr.GetOrdinal("area");
                            int postFechaOperacion = dr.GetOrdinal("fecha_operacion");
                            int postFechaOperacionStr = dr.GetOrdinal("fecha_operacion_str");
                            int postNombreDiaOperacion = dr.GetOrdinal("nombre_dia_operacion");
                            int postMonto = dr.GetOrdinal("monto");
                            int postCodigoEstado = dr.GetOrdinal("codigo_estado");
                            int postEstado = dr.GetOrdinal("estado");
                            int postFechaIng = dr.GetOrdinal("fecha_ing");
                            int postUsuarioIng = dr.GetOrdinal("usuario_ing");
                            int postFechaIngStr = dr.GetOrdinal("fecha_ing_str");
                            int postSigno = dr.GetOrdinal("signo");
                            int postRuta = dr.GetOrdinal("ruta");
                            int postFechaImpresionStr = dr.GetOrdinal("fecha_impresion_str");
                            int postRecursos = dr.GetOrdinal("recursos");
                            int postComplementoConta = dr.GetOrdinal("complemento_conta");
                            int postEsComplementoDeInformacion = dr.GetOrdinal("es_complemento_de_informacion");
                            int postPermisoEditar = dr.GetOrdinal("permiso_editar");
                            int postCodigoEstadoReporteCaja = dr.GetOrdinal("codigo_estado_reporte_caja");
                            int postRevisado = dr.GetOrdinal("revisado");
                            int postCodigoTransaccionAnt = dr.GetOrdinal("codigo_transaccion_ant");
                            int postCodigoTransaccionCorrecta = dr.GetOrdinal("codigo_transaccion_correcta");
                            int postCodigoEstadoSolicitudCorreccion = dr.GetOrdinal("codigo_estado_solicitud_correccion");
                            int postEstadoSolicitudCorreccion = dr.GetOrdinal("estado_solicitud_correccion");
                            int postPermisoCorregir = dr.GetOrdinal("permiso_corregir");
                            int postPermisoAutorizar = dr.GetOrdinal("permiso_autorizar");
                            int postCorreccion = dr.GetOrdinal("correccion");
                            int postCodigoTipoCorreccion = dr.GetOrdinal("codigo_tipo_correccion");
                            int postTipoCorreccion = dr.GetOrdinal("tipo_correccion");

                            while (dr.Read())
                            {
                                objTransaccion = new TransaccionCLS();
                                objTransaccion.CodigoTransaccion = dr.GetInt64(postCodigoTransaccion);
                                objTransaccion.CodigoTipoTransaccion = dr.GetString(postCodigoTipoTransaccion);
                                objTransaccion.SerieFactura = dr.IsDBNull(postSerieFactura) ? "" : dr.GetString(postSerieFactura);
                                objTransaccion.NumeroDocumento = dr.IsDBNull(postNumeroDocumento) ? null : dr.GetInt32(postNumeroDocumento);
                                objTransaccion.FechaDocumento = dr.IsDBNull(postFechaDocumento) ? null : dr.GetDateTime(postFechaDocumento);
                                objTransaccion.NumeroRecibo = dr.GetInt64(postNumeroRecibo);
                                objTransaccion.NumeroReciboStr = dr.GetString(postNumeroReciboStr);
                                objTransaccion.FechaRecibo = dr.GetDateTime(postFechaRecibo);
                                objTransaccion.FechaReciboStr = dr.GetString(postFechaReciboStr);
                                objTransaccion.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objTransaccion.NombreEntidad = dr.IsDBNull(postEntidad) ? "" : dr.GetString(postEntidad);
                                objTransaccion.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objTransaccion.CategoriaEntidad = dr.GetString(postCategoriaEntidad);
                                objTransaccion.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objTransaccion.Operacion = dr.GetString(postOperacion);
                                objTransaccion.CodigoTipoCuentaPorCobrar = dr.GetByte(postCodigoTipoCuentaPorCobrar);
                                objTransaccion.TipoCuentaPorCobrar = dr.GetString(postTipoCuentaPorCobrar);
                                objTransaccion.CodigoCuentaPorCobrar = dr.IsDBNull(postCodigoCuentaPorCobrar) ? -1 : dr.GetInt64(postCodigoCuentaPorCobrar);
                                objTransaccion.CodigoArea = dr.GetInt16(postCodigoArea);
                                objTransaccion.Area = dr.GetString(postArea);
                                objTransaccion.FechaOperacion = dr.GetDateTime(postFechaOperacion);
                                objTransaccion.FechaStr = dr.GetString(postFechaOperacionStr);
                                objTransaccion.NombreDiaOperacion = dr.GetString(postNombreDiaOperacion);
                                objTransaccion.Monto = dr.GetDecimal(postMonto);
                                objTransaccion.CodigoEstado = dr.GetInt16(postCodigoEstado);
                                objTransaccion.Estado = dr.GetString(postEstado);
                                objTransaccion.FechaIng = dr.GetDateTime(postFechaIng);
                                objTransaccion.FechaIngStr = dr.GetString(postFechaIngStr);
                                objTransaccion.UsuarioIng = dr.GetString(postUsuarioIng);
                                objTransaccion.Signo = dr.GetInt16(postSigno);
                                objTransaccion.Ruta = dr.GetInt16(postRuta);
                                objTransaccion.FechaImpresionStr = dr.GetString(postFechaImpresionStr);
                                objTransaccion.Recursos = dr.GetString(postRecursos);
                                objTransaccion.ComplementoConta = dr.GetByte(postComplementoConta);
                                objTransaccion.EscomplementoDeInformacion = dr.GetString(postEsComplementoDeInformacion);
                                objTransaccion.PermisoEditar = (byte)dr.GetInt32(postPermisoEditar);
                                objTransaccion.CodigoEstadoReporteCaja = dr.GetByte(postCodigoEstadoReporteCaja);
                                objTransaccion.Revisado = dr.GetByte(postRevisado);
                                objTransaccion.CodigoTransaccionAnt = dr.GetInt64(postCodigoTransaccionAnt);
                                objTransaccion.CodigoTransaccionCorrecta = dr.GetInt64(postCodigoTransaccionCorrecta);
                                objTransaccion.CodigoEstadoSolicitudCorreccion = dr.GetByte(postCodigoEstadoSolicitudCorreccion);
                                objTransaccion.EstadoSolicitudCorreccion = dr.GetString(postEstadoSolicitudCorreccion);
                                objTransaccion.PermisoCorregir = (byte)dr.GetInt32(postPermisoCorregir);
                                objTransaccion.PermisoAutorizar = (byte)dr.GetInt32(postPermisoAutorizar);
                                objTransaccion.Correccion = dr.GetByte(postCorreccion);
                                objTransaccion.CodigoTipoCorreccion = (Byte)dr.GetInt32(postCodigoTipoCorreccion);
                                objTransaccion.TipoCorreccion = dr.GetString(postTipoCorreccion);
                                lista.Add(objTransaccion);
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

        public List<TransaccionCLS> BuscarTransaccionesDesglocePagoPlanillas()
        {
            List<TransaccionCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string filterOperaciones = string.Empty;
                    string filterCategoriaEntidad = string.Empty;
                    string sql = @"
                    SELECT  x.codigo_transaccion,
		                    x.codigo_tipo_transaccion,
                            x.serie_factura,
    	                    x.numero_documento,
		                    x.fecha_documento,
		                    x.numero_recibo,
		                    x.fecha_recibo,
                            FORMAT(x.fecha_recibo,'dd/MM/yyyy') AS fecha_recibo_str,
                            REPLACE(STR(CAST(x.numero_recibo AS varchar), 10),' ','0') AS numero_recibo_str,
		                    x.codigo_entidad,
                            db_tesoreria.GetNombreEntidad(x.codigo_categoria_entidad, x.codigo_entidad) AS entidad,
                            x.codigo_categoria_entidad,
                            d.nombre AS categoria_entidad,
		                    x.codigo_operacion,
		                    w.nombre_reporte_caja AS operacion,
		                    x.codigo_tipo_cxc,
		                    a.nombre AS tipo_cxc,
                            x.codigo_cxc,
		                    x.codigo_area,
		                    b.nombre AS area,
		                    x.fecha_operacion,
		                    x.monto,
		                    x.codigo_estado,
		                    c.nombre AS estado,
                            x.fecha_ing,
                            x.usuario_ing,
                            FORMAT(x.fecha_ing, 'dd/MM/yyyy, hh:mm:ss') AS fecha_ing_str,
                            CASE  
                             WHEN x.codigo_estado = @CodigoEstadoTransaccion THEN 1
                             ELSE 0
                            END AS permiso_anular,
                            CASE  
                             WHEN x.codigo_estado = @CodigoEstadoTransaccion THEN 1
                             ELSE 0
                            END AS permiso_editar,
                            e.signo,
                            x.ruta,
                            FORMAT(GETDATE(), 'dd/MM/yyyy, hh:mm:ss') AS fecha_impresion_str,
                            CONCAT(CASE WHEN x.efectivo = 1 THEN 'Efectivo,' ELSE '' END,CASE WHEN x.deposito = 1 THEN 'Depósito,' ELSE '' END,CASE WHEN x.cheque = 1 THEN 'Cheque' ELSE '' END) AS recursos
                    FROM db_tesoreria.transaccion x
                    INNER JOIN db_tesoreria.operacion w
                    ON x.codigo_operacion_caja = w.codigo_operacion
                    LEFT JOIN db_contabilidad.tipo_cxc a
                    ON x.codigo_tipo_cxc = a.codigo_tipo_cxc
                    LEFT JOIN db_rrhh.area b
                    ON x.codigo_area = b.codigo_area
                    INNER JOIN db_tesoreria.estado_transaccion c
                    ON x.codigo_estado = c.codigo_estado_transaccion
                    INNER JOIN db_tesoreria.entidad_categoria d
                    ON x.codigo_categoria_entidad = d.codigo_categoria_entidad
                    INNER JOIN db_tesoreria.tipo_operacion e
                    ON w.codigo_tipo_operacion = e.codigo_tipo_operacion
                    WHERE x.codigo_estado = @CodigoEstadoTransaccion
                      AND x.complemento_conta = 1  
                      AND x.codigo_operacion = @CodigoOperacion
                    ORDER BY x.codigo_transaccion DESC";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoTransaccion", Constantes.EstadoTransacccion.REGISTRADO);
                        cmd.Parameters.AddWithValue("@CodigoOperacion", Constantes.Operacion.Egreso.PLANILLA_PAGO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            TransaccionCLS objTransaccion;
                            lista = new List<TransaccionCLS>();
                            int postCodigoTransaccion = dr.GetOrdinal("codigo_transaccion");
                            int postCodigoTipoTransaccion = dr.GetOrdinal("codigo_tipo_transaccion");
                            int postSerieFactura = dr.GetOrdinal("serie_factura");
                            int postNumeroDocumento = dr.GetOrdinal("numero_documento");
                            int postFechaDocumento = dr.GetOrdinal("fecha_documento");
                            int postNumeroRecibo = dr.GetOrdinal("numero_recibo");
                            int postNumeroReciboStr = dr.GetOrdinal("numero_recibo_str");
                            int postFechaRecibo = dr.GetOrdinal("fecha_recibo");
                            int postFechaReciboStr = dr.GetOrdinal("fecha_recibo_str");
                            int postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            int postEntidad = dr.GetOrdinal("entidad");
                            int postCodigoCategoriaEntidad = dr.GetOrdinal("codigo_categoria_entidad");
                            int postCategoriaEntidad = dr.GetOrdinal("categoria_entidad");
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postOperacion = dr.GetOrdinal("operacion");
                            int postCodigoTipoCuentaPorCobrar = dr.GetOrdinal("codigo_tipo_cxc");
                            int postTipoCuentaPorCobrar = dr.GetOrdinal("tipo_cxc");
                            int postCodigoCuentaPorCobrar = dr.GetOrdinal("codigo_cxc");
                            int postCodigoArea = dr.GetOrdinal("codigo_area");
                            int postArea = dr.GetOrdinal("area");
                            int postFechaOperacion = dr.GetOrdinal("fecha_operacion");
                            int postMonto = dr.GetOrdinal("monto");
                            int postCodigoEstado = dr.GetOrdinal("codigo_estado");
                            int postEstado = dr.GetOrdinal("estado");
                            int postFechaIng = dr.GetOrdinal("fecha_ing");
                            int postUsuarioIng = dr.GetOrdinal("usuario_ing");
                            int postFechaIngStr = dr.GetOrdinal("fecha_ing_str");
                            int postPermisoAnular = dr.GetOrdinal("permiso_anular");
                            int postPermisoEditar = dr.GetOrdinal("permiso_editar");
                            int postSigno = dr.GetOrdinal("signo");
                            int postRuta = dr.GetOrdinal("ruta");
                            int postFechaImpresionStr = dr.GetOrdinal("fecha_impresion_str");
                            int postRecursos = dr.GetOrdinal("recursos");

                            while (dr.Read())
                            {
                                objTransaccion = new TransaccionCLS();
                                objTransaccion.CodigoTransaccion = dr.GetInt64(postCodigoTransaccion);
                                objTransaccion.CodigoTipoTransaccion = dr.GetString(postCodigoTipoTransaccion);
                                objTransaccion.SerieFactura = dr.IsDBNull(postSerieFactura) ? "" : dr.GetString(postSerieFactura);
                                objTransaccion.NumeroDocumento = dr.IsDBNull(postNumeroDocumento) ? null : dr.GetInt32(postNumeroDocumento);
                                objTransaccion.FechaDocumento = dr.IsDBNull(postFechaDocumento) ? null : dr.GetDateTime(postFechaDocumento);
                                objTransaccion.NumeroRecibo = dr.GetInt64(postNumeroRecibo);
                                objTransaccion.NumeroReciboStr = dr.GetString(postNumeroReciboStr);
                                objTransaccion.FechaRecibo = dr.GetDateTime(postFechaRecibo);
                                objTransaccion.FechaReciboStr = dr.GetString(postFechaReciboStr);
                                objTransaccion.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objTransaccion.NombreEntidad = dr.IsDBNull(postEntidad) ? "" : dr.GetString(postEntidad);
                                objTransaccion.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objTransaccion.CategoriaEntidad = dr.GetString(postCategoriaEntidad);
                                objTransaccion.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objTransaccion.Operacion = dr.GetString(postOperacion);
                                objTransaccion.CodigoTipoCuentaPorCobrar = dr.GetByte(postCodigoTipoCuentaPorCobrar);
                                objTransaccion.TipoCuentaPorCobrar = dr.GetString(postTipoCuentaPorCobrar);
                                objTransaccion.CodigoCuentaPorCobrar = dr.IsDBNull(postCodigoCuentaPorCobrar) ? -1 : dr.GetInt64(postCodigoCuentaPorCobrar);
                                objTransaccion.CodigoArea = dr.GetInt16(postCodigoArea);
                                objTransaccion.Area = dr.GetString(postArea);
                                objTransaccion.FechaOperacion = dr.GetDateTime(postFechaOperacion);
                                objTransaccion.Monto = dr.GetDecimal(postMonto);
                                objTransaccion.CodigoEstado = dr.GetInt16(postCodigoEstado);
                                objTransaccion.Estado = dr.GetString(postEstado);
                                objTransaccion.FechaIng = dr.GetDateTime(postFechaIng);
                                objTransaccion.FechaIngStr = dr.GetString(postFechaIngStr);
                                objTransaccion.UsuarioIng = dr.GetString(postUsuarioIng);
                                objTransaccion.PermisoAnular = (Byte)dr.GetInt32(postPermisoAnular);
                                objTransaccion.PermisoEditar = (Byte)dr.GetInt32(postPermisoEditar);
                                objTransaccion.Signo = dr.GetInt16(postSigno);
                                objTransaccion.Ruta = dr.GetInt16(postRuta);
                                objTransaccion.FechaImpresionStr = dr.GetString(postFechaImpresionStr);
                                objTransaccion.Recursos = dr.GetString(postRecursos);

                                lista.Add(objTransaccion);
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

        public List<TransaccionCLS> BuscarTransaccionesReporteFacturadoAlContado()
        {
            List<TransaccionCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string filterOperaciones = string.Empty;
                    string filterCategoriaEntidad = string.Empty;
                    string sql = @"
                    SELECT  x.codigo_transaccion,
		                    x.codigo_tipo_transaccion,
                            x.serie_factura,
    	                    x.numero_documento,
		                    x.fecha_documento,
		                    x.numero_recibo,
		                    x.fecha_recibo,
                            FORMAT(x.fecha_recibo,'dd/MM/yyyy') AS fecha_recibo_str,
                            REPLACE(STR(CAST(x.numero_recibo AS varchar), 10),' ','0') AS numero_recibo_str,
		                    x.codigo_entidad,
                            db_tesoreria.GetNombreEntidad(x.codigo_categoria_entidad, x.codigo_entidad) AS entidad,
                            x.codigo_categoria_entidad,
                            d.nombre AS categoria_entidad,
		                    x.codigo_operacion,
		                    w.nombre_reporte_caja AS operacion,
		                    x.codigo_tipo_cxc,
		                    a.nombre AS tipo_cxc,
                            x.codigo_cxc,
		                    x.codigo_area,
		                    b.nombre AS area,
		                    x.fecha_operacion,
		                    x.monto,
		                    x.codigo_estado,
		                    c.nombre AS estado,
                            x.fecha_ing,
                            x.usuario_ing,
                            FORMAT(x.fecha_ing, 'dd/MM/yyyy, hh:mm:ss') AS fecha_ing_str,
                            CASE  
                             WHEN x.codigo_estado = @CodigoEstadoTransaccion THEN 1
                             ELSE 0
                            END AS permiso_anular,
                            CASE  
                             WHEN x.codigo_estado = @CodigoEstadoTransaccion THEN 1
                             ELSE 0
                            END AS permiso_editar,
                            e.signo,
                            x.ruta,
                            x.anio_operacion,
                            x.semana_operacion,
                            x.dia_operacion,
                            CASE
                              WHEN x.dia_operacion = 1 THEN  'LUNES'
                              WHEN x.dia_operacion = 2 THEN  'MARTES'
                              WHEN x.dia_operacion = 3 THEN  'MIERCOLES'
                              WHEN x.dia_operacion = 4 THEN  'JUEVES'
                              WHEN x.dia_operacion = 5 THEN  'VIERNES'
                              WHEN x.dia_operacion = 6 THEN  'SABADO'
                              WHEN x.dia_operacion = 7 THEN  'DOMINGO'
                              ELSE 'NO DEFINIDO'  
                            END nombre_dia_operacion,
                            FORMAT(GETDATE(), 'dd/MM/yyyy, hh:mm:ss') AS fecha_impresion_str,
                            CONCAT(CASE WHEN x.efectivo = 1 THEN 'Efectivo,' ELSE '' END,CASE WHEN x.deposito = 1 THEN 'Depósito,' ELSE '' END,CASE WHEN x.cheque = 1 THEN 'Cheque' ELSE '' END) AS recursos
                    FROM db_tesoreria.transaccion x
                    INNER JOIN db_tesoreria.operacion w
                    ON x.codigo_operacion_caja = w.codigo_operacion
                    LEFT JOIN db_contabilidad.tipo_cxc a
                    ON x.codigo_tipo_cxc = a.codigo_tipo_cxc
                    LEFT JOIN db_rrhh.area b
                    ON x.codigo_area = b.codigo_area
                    INNER JOIN db_tesoreria.estado_transaccion c
                    ON x.codigo_estado = c.codigo_estado_transaccion
                    INNER JOIN db_tesoreria.entidad_categoria d
                    ON x.codigo_categoria_entidad = d.codigo_categoria_entidad
                    INNER JOIN db_tesoreria.tipo_operacion e
                    ON w.codigo_tipo_operacion = e.codigo_tipo_operacion
                    WHERE x.codigo_estado = @CodigoEstadoTransaccion
                      AND x.complemento_conta = 1 
                      AND x.codigo_operacion = @CodigoOperacion
                    ORDER BY x.codigo_transaccion DESC";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoTransaccion", Constantes.EstadoTransacccion.REGISTRADO);
                        cmd.Parameters.AddWithValue("@CodigoOperacion", Constantes.Operacion.Neutro.REGISTRO_FACTURAS_AL_CONTADO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            TransaccionCLS objTransaccion;
                            lista = new List<TransaccionCLS>();
                            int postCodigoTransaccion = dr.GetOrdinal("codigo_transaccion");
                            int postCodigoTipoTransaccion = dr.GetOrdinal("codigo_tipo_transaccion");
                            int postSerieFactura = dr.GetOrdinal("serie_factura");
                            int postNumeroDocumento = dr.GetOrdinal("numero_documento");
                            int postFechaDocumento = dr.GetOrdinal("fecha_documento");
                            int postNumeroRecibo = dr.GetOrdinal("numero_recibo");
                            int postNumeroReciboStr = dr.GetOrdinal("numero_recibo_str");
                            int postFechaRecibo = dr.GetOrdinal("fecha_recibo");
                            int postFechaReciboStr = dr.GetOrdinal("fecha_recibo_str");
                            int postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            int postEntidad = dr.GetOrdinal("entidad");
                            int postCodigoCategoriaEntidad = dr.GetOrdinal("codigo_categoria_entidad");
                            int postCategoriaEntidad = dr.GetOrdinal("categoria_entidad");
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postOperacion = dr.GetOrdinal("operacion");
                            int postCodigoTipoCuentaPorCobrar = dr.GetOrdinal("codigo_tipo_cxc");
                            int postTipoCuentaPorCobrar = dr.GetOrdinal("tipo_cxc");
                            int postCodigoCuentaPorCobrar = dr.GetOrdinal("codigo_cxc");
                            int postCodigoArea = dr.GetOrdinal("codigo_area");
                            int postArea = dr.GetOrdinal("area");
                            int postFechaOperacion = dr.GetOrdinal("fecha_operacion");
                            int postMonto = dr.GetOrdinal("monto");
                            int postCodigoEstado = dr.GetOrdinal("codigo_estado");
                            int postEstado = dr.GetOrdinal("estado");
                            int postFechaIng = dr.GetOrdinal("fecha_ing");
                            int postUsuarioIng = dr.GetOrdinal("usuario_ing");
                            int postFechaIngStr = dr.GetOrdinal("fecha_ing_str");
                            int postPermisoAnular = dr.GetOrdinal("permiso_anular");
                            int postPermisoEditar = dr.GetOrdinal("permiso_editar");
                            int postSigno = dr.GetOrdinal("signo");
                            int postRuta = dr.GetOrdinal("ruta");
                            int postAnioOperacion = dr.GetOrdinal("anio_operacion");
                            int postSemanaOperacion = dr.GetOrdinal("semana_operacion");
                            int postDiaOperacion = dr.GetOrdinal("dia_operacion");
                            int postNombreDiaOperacion = dr.GetOrdinal("nombre_dia_operacion");
                            int postFechaImpresionStr = dr.GetOrdinal("fecha_impresion_str");
                            int postRecursos = dr.GetOrdinal("recursos");

                            while (dr.Read())
                            {
                                objTransaccion = new TransaccionCLS();
                                objTransaccion.CodigoTransaccion = dr.GetInt64(postCodigoTransaccion);
                                objTransaccion.CodigoTipoTransaccion = dr.GetString(postCodigoTipoTransaccion);
                                objTransaccion.SerieFactura = dr.IsDBNull(postSerieFactura) ? "" : dr.GetString(postSerieFactura);
                                objTransaccion.NumeroDocumento = dr.IsDBNull(postNumeroDocumento) ? null : dr.GetInt32(postNumeroDocumento);
                                objTransaccion.FechaDocumento = dr.IsDBNull(postFechaDocumento) ? null : dr.GetDateTime(postFechaDocumento);
                                objTransaccion.NumeroRecibo = dr.GetInt64(postNumeroRecibo);
                                objTransaccion.NumeroReciboStr = dr.GetString(postNumeroReciboStr);
                                objTransaccion.FechaRecibo = dr.GetDateTime(postFechaRecibo);
                                objTransaccion.FechaReciboStr = dr.GetString(postFechaReciboStr);
                                objTransaccion.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objTransaccion.NombreEntidad = dr.IsDBNull(postEntidad) ? "" : dr.GetString(postEntidad);
                                objTransaccion.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objTransaccion.CategoriaEntidad = dr.GetString(postCategoriaEntidad);
                                objTransaccion.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objTransaccion.Operacion = dr.GetString(postOperacion);
                                objTransaccion.CodigoTipoCuentaPorCobrar = dr.GetByte(postCodigoTipoCuentaPorCobrar);
                                objTransaccion.TipoCuentaPorCobrar = dr.GetString(postTipoCuentaPorCobrar);
                                objTransaccion.CodigoCuentaPorCobrar = dr.IsDBNull(postCodigoCuentaPorCobrar) ? -1 : dr.GetInt64(postCodigoCuentaPorCobrar);
                                objTransaccion.CodigoArea = dr.GetInt16(postCodigoArea);
                                objTransaccion.Area = dr.GetString(postArea);
                                objTransaccion.FechaOperacion = dr.GetDateTime(postFechaOperacion);
                                objTransaccion.Monto = dr.GetDecimal(postMonto);
                                objTransaccion.CodigoEstado = dr.GetInt16(postCodigoEstado);
                                objTransaccion.Estado = dr.GetString(postEstado);
                                objTransaccion.FechaIng = dr.GetDateTime(postFechaIng);
                                objTransaccion.FechaIngStr = dr.GetString(postFechaIngStr);
                                objTransaccion.UsuarioIng = dr.GetString(postUsuarioIng);
                                objTransaccion.PermisoAnular = (Byte)dr.GetInt32(postPermisoAnular);
                                objTransaccion.PermisoEditar = (Byte)dr.GetInt32(postPermisoEditar);
                                objTransaccion.Signo = dr.GetInt16(postSigno);
                                objTransaccion.Ruta = dr.GetInt16(postRuta);
                                objTransaccion.AnioOperacion = dr.GetInt16(postAnioOperacion);
                                objTransaccion.SemanaOperacion = dr.GetByte(postSemanaOperacion);
                                objTransaccion.DiaOperacion = dr.GetByte(postDiaOperacion);
                                objTransaccion.NombreDiaOperacion = dr.GetString(postNombreDiaOperacion);
                                objTransaccion.FechaImpresionStr = dr.GetString(postFechaImpresionStr);
                                objTransaccion.Recursos = dr.GetString(postRecursos);

                                lista.Add(objTransaccion);
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

        public TransaccionCLS GetDataTransaccion(long codigoTransaccion)
        {
            TransaccionCLS objTransaccion = new TransaccionCLS();
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    SELECT  x.codigo_transaccion,
                            x.codigo_seguridad,
		                    x.codigo_tipo_transaccion,
                            x.serie_factura,
    	                    x.numero_documento,
		                    x.fecha_documento,
                            CASE
                               WHEN x.fecha_documento IS NULL THEN ''
                               ELSE FORMAT(x.fecha_documento, 'dd/MM/yyyy') 
                            END AS fecha_documento_str,
		                    x.numero_recibo,
                            x.numero_recibo_referencia,
		                    x.fecha_recibo,
                            FORMAT(x.fecha_recibo, 'dd/MM/yyyy') AS fecha_recibo_str,
                            REPLACE(STR(CAST(x.numero_recibo AS varchar), 10),' ','0') AS numero_recibo_str,
		                    x.codigo_entidad,
                            db_tesoreria.GetNombreEntidad(x.codigo_categoria_entidad, x.codigo_entidad) AS entidad,
                            x.codigo_categoria_entidad,
                            d.nombre AS categoria_entidad,
		                    x.codigo_operacion,
                            x.codigo_operacion_caja,
		                    w.nombre_reporte_caja AS operacion,
		                    x.codigo_tipo_cxc,
		                    a.nombre AS tipo_cxc,
                            x.codigo_cxc,
		                    x.codigo_area,
		                    b.nombre AS area,
		                    x.fecha_operacion,
                            FORMAT(x.fecha_operacion, 'dd/MM/yyyy') AS fecha_operacion_str,
                            FORMAT(x.fecha_operacion, 'hh:mm:ss') AS hora_operacion_str,
                            x.monto_efectivo,
                            x.monto_cheques,
		                    x.monto,
		                    x.codigo_estado,
		                    c.nombre AS estado,
                            x.fecha_ing,
                            FORMAT(x.fecha_ing, 'dd/MM/yyyy, hh:mm:ss') AS fecha_ing_str,
                            x.efectivo,
                            x.deposito,
                            x.cheque,
                            x.ruta,
                            x.anio_operacion,
                            x.semana_operacion,
                            x.anio_comision,
                            x.semana_comision,
                            x.anio_planilla,
                            x.mes_planilla,
                            x.codigo_frecuencia_pago,
                            x.semana_planilla,
                            x.codigo_tipo_pago,
                            x.codigo_quincena_planilla,
                            e.signo,
                            x.codigo_bono_extra,
                            x.codigo_tipo_documento,
                            x.observaciones,
                            COALESCE(x.codigo_reporte,0) AS codigo_reporte,
                            COALESCE(x.codigo_banco_deposito,0) AS codigo_banco_deposito,
                            x.codigo_tipo_doc_deposito,
                            x.numero_boleta,
                            x.numero_voucher,
                            x.numero_cuenta,
                            x.tipo_especiales_1,
                            x.nombre_proveedor,
                            x.codigo_canal_venta,
                            CONCAT(CASE WHEN x.efectivo = 1 THEN 'Efectivo,' ELSE '' END,CASE WHEN x.deposito = 1 THEN 'Depósito,' ELSE '' END,CASE WHEN x.cheque = 1 THEN 'Cheque' ELSE '' END) AS recursos,
                            FORMAT(GETDATE(), 'dd/MM/yyyy, hh:mm:ss') AS fecha_impresion_str,
                            x.usuario_ing,
                            x.codigo_otro_ingreso,
                            monto_saldo_anterior_cxc,
                            monto_saldo_actual_cxc    
                            
                    FROM db_tesoreria.transaccion x
                    INNER JOIN db_tesoreria.operacion w
                    ON x.codigo_operacion = w.codigo_operacion
                    LEFT JOIN db_contabilidad.tipo_cxc a
                    ON x.codigo_tipo_cxc = a.codigo_tipo_cxc
                    LEFT JOIN db_rrhh.area b
                    ON x.codigo_area = b.codigo_area
                    INNER JOIN db_tesoreria.estado_transaccion c
                    ON x.codigo_estado = c.codigo_estado_transaccion
                    INNER JOIN db_tesoreria.entidad_categoria d
                    ON x.codigo_categoria_entidad = d.codigo_categoria_entidad
                    INNER JOIN db_tesoreria.tipo_operacion e
                    ON w.codigo_tipo_operacion = e.codigo_tipo_operacion
                    WHERE x.codigo_transaccion = @CodigoTransaccion";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoTransaccion", codigoTransaccion);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            int postCodigoTransaccion = dr.GetOrdinal("codigo_transaccion");
                            int postCodigoSeguridad = dr.GetOrdinal("codigo_seguridad");
                            int postCodigoTipoTransaccion = dr.GetOrdinal("codigo_tipo_transaccion");
                            int postSerieFactura = dr.GetOrdinal("serie_factura");
                            int postNumeroDocumento = dr.GetOrdinal("numero_documento");
                            int postFechaDocumento = dr.GetOrdinal("fecha_documento");
                            int postFechaDocumentoStr = dr.GetOrdinal("fecha_documento_str");
                            int postNumeroRecibo = dr.GetOrdinal("numero_recibo");
                            int postNumeroReciboReferencia = dr.GetOrdinal("numero_recibo_referencia");
                            int postFechaRecibo = dr.GetOrdinal("fecha_recibo");
                            int postFechaReciboStr = dr.GetOrdinal("fecha_recibo_str");
                            int postNumeroReciboStr = dr.GetOrdinal("numero_recibo_str");
                            int postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            int postEntidad = dr.GetOrdinal("entidad");
                            int postCodigoCategoriaEntidad = dr.GetOrdinal("codigo_categoria_entidad");
                            int postCategoriaEntidad = dr.GetOrdinal("categoria_entidad");
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postCodigoOperacionCaja = dr.GetOrdinal("codigo_operacion_caja");
                            int postOperacion = dr.GetOrdinal("operacion");
                            int postCodigoTipoCuentaPorCobrar = dr.GetOrdinal("codigo_tipo_cxc");
                            int postTipoCuentaPorCobrar = dr.GetOrdinal("tipo_cxc");
                            int postCodigoCuentaPorCobrar = dr.GetOrdinal("codigo_cxc");
                            int postCodigoArea = dr.GetOrdinal("codigo_area");
                            int postArea = dr.GetOrdinal("area");
                            int postFechaOperacion = dr.GetOrdinal("fecha_operacion");
                            int postFechaOperacionStr = dr.GetOrdinal("fecha_operacion_str");
                            int postHoraOperacionStr = dr.GetOrdinal("hora_operacion_str");
                            int postMonto = dr.GetOrdinal("monto");
                            int postCodigoEstado = dr.GetOrdinal("codigo_estado");
                            int postEstado = dr.GetOrdinal("estado");
                            int postFechaIng = dr.GetOrdinal("fecha_ing");
                            int postFechaIngStr = dr.GetOrdinal("fecha_ing_str");
                            int postEfectivo = dr.GetOrdinal("efectivo");
                            int postDeposito = dr.GetOrdinal("deposito");
                            int postCheque = dr.GetOrdinal("cheque");
                            int postRuta = dr.GetOrdinal("ruta");
                            int postAnioOperacion = dr.GetOrdinal("anio_operacion");
                            int postSemanaOperacion = dr.GetOrdinal("semana_operacion");
                            int postAnioComision = dr.GetOrdinal("anio_comision");
                            int postSemanaComision = dr.GetOrdinal("semana_comision");
                            int postAnioPlanilla = dr.GetOrdinal("anio_planilla");
                            int postMesPlanilla = dr.GetOrdinal("mes_planilla");
                            int postCodigoFrecuenciaPago = dr.GetOrdinal("codigo_frecuencia_pago");
                            int postSemanaPlanilla = dr.GetOrdinal("semana_planilla");
                            int postCodigoTipoPago = dr.GetOrdinal("codigo_tipo_pago");
                            int postCodigoQuincenaPlanilla = dr.GetOrdinal("codigo_quincena_planilla");
                            int postSignoOperacion = dr.GetOrdinal("signo");
                            int postCodigoBonoExtra = dr.GetOrdinal("codigo_bono_extra");
                            int postCodigoTipoDocumento = dr.GetOrdinal("codigo_tipo_documento");
                            int postObservaciones = dr.GetOrdinal("observaciones");
                            int postCodigoReporte = dr.GetOrdinal("codigo_reporte");
                            int postCodigoBancoDeposito = dr.GetOrdinal("codigo_banco_deposito");
                            int postCodigoTipoDocumentoDeposito = dr.GetOrdinal("codigo_tipo_doc_deposito");
                            int postNumeroCuenta = dr.GetOrdinal("numero_cuenta");
                            int postNumeroBoleta = dr.GetOrdinal("numero_boleta");
                            int postNumeroVoucher = dr.GetOrdinal("numero_voucher");
                            int postMontoEfectivo = dr.GetOrdinal("monto_efectivo");
                            int postMontoCheques = dr.GetOrdinal("monto_cheques");
                            int postTipoEspecial1 = dr.GetOrdinal("tipo_especiales_1");
                            int postNombreProveedor = dr.GetOrdinal("nombre_proveedor");
                            int postCodigoCanalVenta = dr.GetOrdinal("codigo_canal_venta");
                            int postRecursos = dr.GetOrdinal("recursos");
                            int postFechaImpresionStr = dr.GetOrdinal("fecha_impresion_str");
                            int postUsuarioIng = dr.GetOrdinal("usuario_ing");
                            int postCodigoOtroIngreso = dr.GetOrdinal("codigo_otro_ingreso");
                            int postMontoSaldoAnteriorCxC = dr.GetOrdinal("monto_saldo_anterior_cxc");
                            int postMontoSaldoActualCxC = dr.GetOrdinal("monto_saldo_actual_cxc");


                            while (dr.Read())
                            {
                                objTransaccion.CodigoTransaccion = dr.GetInt64(postCodigoTransaccion);
                                objTransaccion.CodigoSeguridad = dr.GetString(postCodigoSeguridad);
                                objTransaccion.CodigoTipoTransaccion = dr.GetString(postCodigoTipoTransaccion);
                                objTransaccion.SerieFactura = dr.IsDBNull(postSerieFactura) ? "" : dr.GetString(postSerieFactura);
                                objTransaccion.NumeroDocumento = dr.IsDBNull(postNumeroDocumento) ? null : dr.GetInt32(postNumeroDocumento);
                                objTransaccion.FechaDocumento = dr.IsDBNull(postFechaDocumento) ? null : dr.GetDateTime(postFechaDocumento);
                                objTransaccion.FechaDocumentoStr = dr.GetString(postFechaDocumentoStr);
                                objTransaccion.NumeroRecibo = dr.GetInt64(postNumeroRecibo);
                                objTransaccion.NumeroReciboReferencia = dr.GetInt64(postNumeroReciboReferencia);
                                objTransaccion.FechaRecibo = dr.GetDateTime(postFechaRecibo);
                                objTransaccion.FechaReciboStr = dr.GetString(postFechaReciboStr);
                                objTransaccion.NumeroReciboStr = dr.GetString(postNumeroReciboStr);
                                objTransaccion.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objTransaccion.NombreEntidad = dr.IsDBNull(postEntidad) ? "" : dr.GetString(postEntidad);
                                objTransaccion.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objTransaccion.CategoriaEntidad = dr.GetString(postCategoriaEntidad);
                                objTransaccion.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objTransaccion.CodigoOperacionCaja = dr.GetInt16(postCodigoOperacionCaja);
                                objTransaccion.Operacion = dr.GetString(postOperacion);
                                objTransaccion.CodigoTipoCuentaPorCobrar = dr.GetByte(postCodigoTipoCuentaPorCobrar);
                                objTransaccion.TipoCuentaPorCobrar = dr.GetString(postTipoCuentaPorCobrar);
                                objTransaccion.CodigoCuentaPorCobrar = dr.IsDBNull(postCodigoCuentaPorCobrar) ? -1 : dr.GetInt64(postCodigoCuentaPorCobrar);
                                objTransaccion.CodigoArea = dr.GetInt16(postCodigoArea);
                                objTransaccion.Area = dr.GetString(postArea);
                                objTransaccion.FechaOperacion = dr.GetDateTime(postFechaOperacion);
                                objTransaccion.FechaOperacionStr = dr.GetString(postFechaOperacionStr);
                                objTransaccion.HoraOperacionStr = dr.GetString(postHoraOperacionStr);
                                objTransaccion.Monto = dr.GetDecimal(postMonto);
                                objTransaccion.CodigoEstado = dr.GetInt16(postCodigoEstado);
                                objTransaccion.Estado = dr.GetString(postEstado);
                                objTransaccion.FechaIng = dr.GetDateTime(postFechaIng);
                                objTransaccion.FechaIngStr = dr.GetString(postFechaIngStr);
                                objTransaccion.Efectivo = dr.GetByte(postEfectivo);
                                objTransaccion.Deposito = dr.GetByte(postDeposito);
                                objTransaccion.Cheque = dr.GetByte(postCheque);
                                objTransaccion.Ruta = dr.GetInt16(postRuta);
                                objTransaccion.AnioOperacion = dr.GetInt16(postAnioOperacion);
                                objTransaccion.SemanaOperacion = dr.GetByte(postSemanaOperacion);
                                objTransaccion.AnioComision = dr.GetInt16(postAnioComision);
                                objTransaccion.SemanaComision = dr.GetByte(postSemanaComision);
                                objTransaccion.AnioPlanilla = dr.GetInt16(postAnioPlanilla);
                                objTransaccion.MesPlanilla = dr.GetByte(postMesPlanilla);
                                objTransaccion.CodigoFrecuenciaPago = dr.GetByte(postCodigoFrecuenciaPago);
                                objTransaccion.SemanaPlanilla = dr.GetByte(postSemanaPlanilla);
                                objTransaccion.CodigoTipoPago = dr.GetByte(postCodigoTipoPago);
                                objTransaccion.CodigoQuincenaPlanilla = dr.GetInt32(postCodigoQuincenaPlanilla);
                                objTransaccion.Signo = dr.GetInt16(postSignoOperacion);
                                objTransaccion.CodigoBonoExtra = dr.GetByte(postCodigoBonoExtra);
                                objTransaccion.CodigoTipoDocumento = dr.GetByte(postCodigoTipoDocumento);
                                objTransaccion.Observaciones = dr.IsDBNull(postObservaciones) ? "" : dr.GetString(postObservaciones);
                                objTransaccion.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objTransaccion.CodigoBancoDeposito = (short)dr.GetInt32(postCodigoBancoDeposito);
                                objTransaccion.NumeroCuenta = dr.IsDBNull(postNumeroCuenta) ? "-1" : dr.GetString(postNumeroCuenta);
                                objTransaccion.CodigoTipoDocumentoDeposito = dr.GetByte(postCodigoTipoDocumentoDeposito);
                                objTransaccion.NumeroBoleta = dr.IsDBNull(postNumeroBoleta) ? "" : dr.GetString(postNumeroBoleta);
                                objTransaccion.NumeroVoucher = dr.IsDBNull(postNumeroVoucher) ? "" : dr.GetString(postNumeroVoucher);
                                objTransaccion.MontoEfectivo = dr.GetDecimal(postMontoEfectivo);
                                objTransaccion.MontoCheques = dr.GetDecimal(postMontoCheques);
                                objTransaccion.TipoEspeciales1 = dr.GetByte(postTipoEspecial1);
                                objTransaccion.NombreProveedor = dr.IsDBNull(postNombreProveedor) ? "" : dr.GetString(postNombreProveedor);
                                objTransaccion.CodigoCanalVenta = dr.GetInt16(postCodigoCanalVenta);
                                objTransaccion.Recursos = dr.IsDBNull(postRecursos) ? "" : dr.GetString(postRecursos);
                                objTransaccion.FechaImpresionStr = dr.GetString(postFechaImpresionStr);
                                objTransaccion.UsuarioIng = dr.GetString(postUsuarioIng);
                                objTransaccion.CodigoOtroIngreso = dr.GetInt16(postCodigoOtroIngreso);
                                objTransaccion.MontoSaldoAnteriorCxC = dr.GetDecimal(postMontoSaldoAnteriorCxC);
                                objTransaccion.MontoSaldoActualCxC = dr.GetDecimal(postMontoSaldoActualCxC);
                            }
                        }
                    }
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                }

                return objTransaccion;
            }
        }

        public TransaccionComboCLS fillCombosNewTransaccion(int codigoTipoOperacion, string idUsuario)
        {
            TransaccionComboCLS objTransaccionComboCLS = new TransaccionComboCLS();
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_tesoreria.uspFillComboNewTransaccion", conexion))
                    {
                        // le indico que es un procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@operacion", codigoTipoOperacion);
                        // Devolvera varias tablas
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr != null)
                        {
                            List<EmpresaCLS> listaEmpresas = new List<EmpresaCLS>();
                            int postCodigoEmpresa = dr.GetOrdinal("codigo_empresa");
                            int postNombreEmpresa = dr.GetOrdinal("nombre_comercial");

                            EmpresaCLS objEmpresaCLS;
                            while (dr.Read())
                            {
                                objEmpresaCLS = new EmpresaCLS();
                                objEmpresaCLS.CodigoEmpresa = dr.GetInt16(postCodigoEmpresa);
                                objEmpresaCLS.NombreComercial = dr.GetString(postNombreEmpresa);
                                listaEmpresas.Add(objEmpresaCLS);
                            }//fin while
                            objTransaccionComboCLS.listaEmpresasTesoreria = listaEmpresas;
                        }// fin if

                        if (dr.NextResult())
                        {
                            List<OperacionCLS> listaOperaciones = new List<OperacionCLS>();
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postNombre = dr.GetOrdinal("nombre");

                            OperacionCLS objOperacionCLS;
                            while (dr.Read())
                            {
                                objOperacionCLS = new OperacionCLS();
                                objOperacionCLS.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objOperacionCLS.Nombre = dr.GetString(postNombre);
                                listaOperaciones.Add(objOperacionCLS);
                            }//fin while
                            objTransaccionComboCLS.listaOperaciones = listaOperaciones;
                        }// fin if

                        if (dr.NextResult())
                        {
                            List<ProgramacionSemanalCLS> listaProgrmacionSemanal = new List<ProgramacionSemanalCLS>();
                            int postAnioOperacion = dr.GetOrdinal("anio");
                            int postFecha = dr.GetOrdinal("fecha");
                            int postFechaStr = dr.GetOrdinal("fechaStr");
                            int postDia = dr.GetOrdinal("dia");
                            int postNumeroSemana  = dr.GetOrdinal("numero_semana");

                            ProgramacionSemanalCLS objProgramacionSemanalCLS;
                            while (dr.Read())
                            {
                                objProgramacionSemanalCLS = new ProgramacionSemanalCLS();
                                objProgramacionSemanalCLS.FechaStr = dr.GetString(postFechaStr);
                                objProgramacionSemanalCLS.Dia = dr.GetString(postDia);
                                objProgramacionSemanalCLS.NumeroSemana = dr.GetByte(postNumeroSemana);

                                objTransaccionComboCLS.AnioOperacion = dr.GetInt16(postAnioOperacion);
                                objTransaccionComboCLS.NumeroSemana = dr.GetByte(postNumeroSemana);

                                listaProgrmacionSemanal.Add(objProgramacionSemanalCLS);

                            }//fin while
                            objTransaccionComboCLS.listaProgramacionSemanal = listaProgrmacionSemanal;
                        }
                    }// fin using
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                    objTransaccionComboCLS = null;

                }
                return objTransaccionComboCLS;
            }
        }

        public TransaccionComboCLS FillCombosEditTransaccion(int codigoTipoOperacion, int semanaOperacion, int anioOperacion)
        {
            TransaccionComboCLS objTransaccionComboCLS = new TransaccionComboCLS();
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_tesoreria.uspFillComboEditTransaccion", conexion))
                    {
                        // le indico que es un procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@operacion", codigoTipoOperacion);
                        cmd.Parameters.AddWithValue("@SemanaOperacion", semanaOperacion);
                        cmd.Parameters.AddWithValue("@AnioOperacion", anioOperacion);
                        // Devolvera varias tablas
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr != null)
                        {
                            List<EmpresaCLS> listaEmpresas = new List<EmpresaCLS>();
                            int postCodigoEmpresa = dr.GetOrdinal("codigo_empresa");
                            int postNombreEmpresa = dr.GetOrdinal("nombre_comercial");

                            EmpresaCLS objEmpresaCLS;
                            while (dr.Read())
                            {
                                objEmpresaCLS = new EmpresaCLS();
                                objEmpresaCLS.CodigoEmpresa = dr.GetInt16(postCodigoEmpresa);
                                objEmpresaCLS.NombreComercial = dr.GetString(postNombreEmpresa);
                                listaEmpresas.Add(objEmpresaCLS);
                            }//fin while
                            objTransaccionComboCLS.listaEmpresasTesoreria = listaEmpresas;
                        }// fin if

                        if (dr.NextResult())
                        {
                            List<OperacionCLS> listaOperaciones = new List<OperacionCLS>();
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postNombre = dr.GetOrdinal("nombre");

                            OperacionCLS objOperacionCLS;
                            while (dr.Read())
                            {
                                objOperacionCLS = new OperacionCLS();
                                objOperacionCLS.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objOperacionCLS.Nombre = dr.GetString(postNombre);
                                listaOperaciones.Add(objOperacionCLS);
                            }//fin while
                            objTransaccionComboCLS.listaOperaciones = listaOperaciones;
                        }// fin if

                        if (dr.NextResult())
                        {
                            List<ProgramacionSemanalCLS> listaProgrmacionSemanal = new List<ProgramacionSemanalCLS>();
                            int postAnioOperacion = dr.GetOrdinal("anio");
                            int postFecha = dr.GetOrdinal("fecha");
                            int postFechaStr = dr.GetOrdinal("fechaStr");
                            int postDia = dr.GetOrdinal("dia");
                            int postNumeroSemana = dr.GetOrdinal("numero_semana");

                            ProgramacionSemanalCLS objProgramacionSemanalCLS;
                            while (dr.Read())
                            {
                                objProgramacionSemanalCLS = new ProgramacionSemanalCLS();
                                objProgramacionSemanalCLS.FechaStr = dr.GetString(postFechaStr);
                                objProgramacionSemanalCLS.Dia = dr.GetString(postDia);
                                objProgramacionSemanalCLS.NumeroSemana = dr.GetByte(postNumeroSemana);
                                listaProgrmacionSemanal.Add(objProgramacionSemanalCLS);

                            }//fin while
                            objTransaccionComboCLS.listaProgramacionSemanal = listaProgrmacionSemanal;
                        }
                    }// fin using
                    conexion.Close();
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    objTransaccionComboCLS = null;

                }
                return objTransaccionComboCLS;
            }
        }

        public TransaccionComboCLS FillCombosConsultaTransacciones()
        {
            TransaccionComboCLS objTransaccionComboCLS = new TransaccionComboCLS();
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_tesoreria.uspFillComboFiltroConsultaTransacciones", conexion))
                    {
                        // le indico que es un procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        // Devolvera varias tablas
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr != null)
                        {
                            List<OperacionCLS> listaOperaciones = new List<OperacionCLS>();
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postNombre = dr.GetOrdinal("nombre");

                            OperacionCLS objOperacionCLS;
                            while (dr.Read())
                            {
                                objOperacionCLS = new OperacionCLS();
                                objOperacionCLS.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objOperacionCLS.Nombre = dr.GetString(postNombre);
                                listaOperaciones.Add(objOperacionCLS);
                            }//fin while
                            objTransaccionComboCLS.listaOperaciones = listaOperaciones;
                        }// fin if

                        if (dr.NextResult())
                        {
                            List<EntidadCategoriaCLS> listaCategoriasEntidades = new List<EntidadCategoriaCLS>();
                            int postCodigoCategoriaEntidad = dr.GetOrdinal("codigo_categoria_entidad");
                            int postNombreCategoria = dr.GetOrdinal("nombre");

                            EntidadCategoriaCLS objEntidadCategoriaCLS;
                            while (dr.Read())
                            {
                                objEntidadCategoriaCLS = new EntidadCategoriaCLS();
                                objEntidadCategoriaCLS.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objEntidadCategoriaCLS.NombreCategoriaEntidad = dr.GetString(postNombreCategoria);

                                listaCategoriasEntidades.Add(objEntidadCategoriaCLS);

                            }//fin while
                            objTransaccionComboCLS.listaCategoriasEntidades = listaCategoriasEntidades;
                        }

                        if (dr.NextResult())
                        {
                            List<AnioCLS> listaAnios = new List<AnioCLS>();
                            int postAnioOperacion = dr.GetOrdinal("anio_operacion");
                            AnioCLS objAnioCLS;
                            while (dr.Read())
                            {
                                objAnioCLS = new AnioCLS();
                                objAnioCLS.Anio = dr.GetInt16(postAnioOperacion);
                                listaAnios.Add(objAnioCLS);

                            }//fin while
                            objTransaccionComboCLS.listaAnios = listaAnios;
                        }
                    }// fin using
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                    objTransaccionComboCLS = null;

                }
                return objTransaccionComboCLS;
            }
        }

        public TransaccionComboCLS fillComboSemana(int habilitarSemanaAnterior)
        {
            TransaccionComboCLS objTransaccionComboCLS = new TransaccionComboCLS();
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_tesoreria.uspFillComboSemana", conexion))
                    {
                        // le indico que es un procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@semanaAnterior", habilitarSemanaAnterior);
                        // Devolvera varias tablas
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            List<ProgramacionSemanalCLS> listaProgrmacionSemanal = new List<ProgramacionSemanalCLS>();
                            int postFecha = dr.GetOrdinal("fecha");
                            int postFechaStr = dr.GetOrdinal("fechaStr");
                            int postDia = dr.GetOrdinal("dia");
                            int postNumeroSemana = dr.GetOrdinal("numero_semana");

                            ProgramacionSemanalCLS objProgramacionSemanalCLS;
                            while (dr.Read())
                            {
                                objProgramacionSemanalCLS = new ProgramacionSemanalCLS();
                                objProgramacionSemanalCLS.FechaStr = dr.GetString(postFechaStr);
                                objProgramacionSemanalCLS.Dia = dr.GetString(postDia);
                                objProgramacionSemanalCLS.NumeroSemana = dr.GetByte(postNumeroSemana);
                                objTransaccionComboCLS.NumeroSemana = dr.GetByte(postNumeroSemana);

                                listaProgrmacionSemanal.Add(objProgramacionSemanalCLS);

                            }//fin while
                            objTransaccionComboCLS.listaProgramacionSemanal = listaProgrmacionSemanal;
                        }// fin if
                    }// fin using
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                    objTransaccionComboCLS = null;

                }
                return objTransaccionComboCLS;
            }
        }

        public List<ContribuyenteCLS> GetEmpresasConcecionIVA() 
        {
            List<ContribuyenteCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT x.nit, 
                           y.nombre
                    FROM db_tesoreria.empresa_concede_iva x
                    INNER JOIN db_tesoreria.contribuyente y
                    ON x.nit = y.nit
                    WHERE x.estado = @CodigoEstado";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.EstadoRegistro.ACTIVO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            ContribuyenteCLS objContribuyente;
                            lista = new List<ContribuyenteCLS>();
                            int postNit = dr.GetOrdinal("nit");
                            int postNombreContribuyente = dr.GetOrdinal("nombre");

                            while (dr.Read())
                            {
                                objContribuyente = new ContribuyenteCLS();
                                objContribuyente.Nit = dr.GetString(postNit);
                                objContribuyente.Nombre = dr.GetString(postNombreContribuyente);

                                lista.Add(objContribuyente);
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

        public string AnularTransaccion(long codigoTransaccion, int codigoOperacion, long codigoCuentaPorCobrar, string observaciones, string usuarioAct)
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
                    SET codigo_cxc = null,
                        codigo_estado = @EstadoAnulado,
                        motivo_anulacion = @MotivoAnulacion,
                        usuario_anulacion = @UsuarioAnulacion,
                        fecha_anulacion = @FechaAnulacion
                    WHERE codigo_transaccion = @CodigoTransaccion";

                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sentenciaUpdateTransaccion;
                    cmd.Parameters.AddWithValue("@EstadoAnulado", Constantes.EstadoTransacccion.ANULADO);
                    cmd.Parameters.AddWithValue("@MotivoAnulacion", observaciones == null ? DBNull.Value : observaciones);
                    cmd.Parameters.AddWithValue("@UsuarioAnulacion", usuarioAct);
                    cmd.Parameters.AddWithValue("@FechaAnulacion", DateTime.Now);
                    cmd.Parameters.AddWithValue("@CodigoTransaccion", codigoTransaccion);
                    // rows sotred the number of rows affected
                    int rowsTransaccion = cmd.ExecuteNonQuery();

                    bool exitoso = true;
                    if (codigoOperacion == Constantes.CuentaPorCobrar.Operacion.ANTICIPO_SALARIO ||
                        codigoOperacion == Constantes.CuentaPorCobrar.Operacion.DESCUENTO_PLANILLA_POR_ANTICIPO_SALARIO ||
                        codigoOperacion == Constantes.CuentaPorCobrar.Operacion.PRESTAMO ||
                        codigoOperacion == Constantes.CuentaPorCobrar.Operacion.ABONO_PRESTAMO ||
                        codigoOperacion == Constantes.CuentaPorCobrar.Operacion.BACK_TO_BACK ||
                        codigoOperacion == Constantes.CuentaPorCobrar.Operacion.RETIRO_SOCIOS ||
                        codigoOperacion == Constantes.CuentaPorCobrar.Operacion.DEVOLUCION_SOCIOS)
                    {

                        exitoso = false;
                        string sentenciaUpdateCxC = @"
                        UPDATE db_contabilidad.cuenta_por_cobrar
                        SET codigo_estado = @EstadoAnuladoCxC
                        WHERE codigo_cxc = @CodigoCuentaPorCobrar";

                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sentenciaUpdateCxC;
                        cmd.Parameters.AddWithValue("@EstadoAnuladoCxC", Constantes.CuentaPorCobrar.Estado.ANULADO);
                        cmd.Parameters.AddWithValue("@CodigoCuentaPorCobrar", codigoCuentaPorCobrar);
                        // rows sotred the number of rows affected
                        int rowsCxC = cmd.ExecuteNonQuery();
                        if (rowsCxC > 0)
                            exitoso = true;
                    }

                    if (rowsTransaccion > 0 && exitoso == true)
                    {
                        transaction.Commit();
                        conexion.Close();
                        resultado = "OK";
                    }
                    else
                    {
                        transaction.Rollback();
                        conexion.Close();
                        resultado = "Error [0]: Cero Actualizaciones";
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    conexion.Close();
                    resultado = "Error [0]: " + ex.Message;
                }

                return resultado;
            }
        }

        public string AnularTransaccionComplementoContabilidad(long codigoTransaccion, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    UPDATE db_tesoreria.transaccion
                    SET codigo_estado = @CodigoEstado,
                        usuario_act = @UsuarioActualizacion,
                        fecha_act = @FechaActualizacion
                    WHERE codigo_transaccion = @CodigoTransaccion";
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.EstadoTransacccion.ANULADO);
                        cmd.Parameters.AddWithValue("@UsuarioActualizacion", usuarioAct);
                        cmd.Parameters.AddWithValue("@FechaActualizacion", DateTime.Now);
                        cmd.Parameters.AddWithValue("@CodigoTransaccion", codigoTransaccion);

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

        public string AceptarTransaccionComplementoContabilidad(long codigoTransaccion, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    UPDATE db_tesoreria.transaccion
                    SET codigo_estado = @CodigoEstado,
                        usuario_act = @UsuarioActualizacion,
                        fecha_act = @FechaActualizacion
                    WHERE codigo_transaccion = @CodigoTransaccion";
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.EstadoTransacccion.INCLUIDO_EN_REPORTE);
                        cmd.Parameters.AddWithValue("@UsuarioActualizacion", usuarioAct);
                        cmd.Parameters.AddWithValue("@FechaActualizacion", DateTime.Now);
                        cmd.Parameters.AddWithValue("@CodigoTransaccion", codigoTransaccion);

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

        public string ActualizarNumeroBoletaDeposito(long codigoTransaccion, string numeroBoletaDeposito, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    UPDATE db_tesoreria.transaccion
                    SET numero_boleta = @NumeroBoleta,
                        usuario_act = @UsuarioActualizacion,
                        fecha_act = @FechaActualizacion
                    WHERE codigo_transaccion = @CodigoTransaccion";
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@NumeroBoleta", numeroBoletaDeposito);
                        cmd.Parameters.AddWithValue("@UsuarioActualizacion", usuarioAct);
                        cmd.Parameters.AddWithValue("@FechaActualizacion", DateTime.Now);
                        cmd.Parameters.AddWithValue("@CodigoTransaccion", codigoTransaccion);

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

        /// <summary>
        /// Método temporal, para trasladar las transacciones en estado 1.Registrado, a la semana que esté operando el asitente contable.
        /// Esto mientras el ingreso de las transacciones lo estén realizando del lado de contabilidad.
        /// </summary>
        /// <returns></returns>
        public List<ReporteCajaCLS> GetTransaccionParaCambioDeSemanaOperacion(string usuarioIng)
        {
            List<ReporteCajaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    SELECT anio_operacion, 
                           semana_operacion, 
                           count(*) AS cantidad_transacciones,
                           1 AS permiso_editar 
					FROM db_tesoreria.transaccion
					WHERE codigo_estado = @CodigoEstadoRegistrado
                      AND usuario_ing = '" + usuarioIng + @"'
					GROUP BY anio_operacion, semana_operacion";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoRegistrado", Constantes.EstadoTransacccion.REGISTRADO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            ReporteCajaCLS objReporteCaja;
                            lista = new List<ReporteCajaCLS>();
                            int postAnioOperacion = dr.GetOrdinal("anio_operacion");
                            int postSemanaOperacion = dr.GetOrdinal("semana_operacion");
                            int postCantidadTransacciones = dr.GetOrdinal("cantidad_transacciones");
                            int postPermisoEditar = dr.GetOrdinal("permiso_editar");
                            while (dr.Read())
                            {
                                objReporteCaja = new ReporteCajaCLS();
                                objReporteCaja.Anio = dr.GetInt16(postAnioOperacion);
                                objReporteCaja.NumeroSemana = dr.GetByte(postSemanaOperacion);
                                objReporteCaja.CantidadTransacciones= dr.GetInt32(postCantidadTransacciones);
                                objReporteCaja.PermisoEditar = (byte)dr.GetInt32(postPermisoEditar);
                                lista.Add(objReporteCaja);
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

        public string CambiarSemanaOperacionTransacciones(int anioOperacion, int semanaOperacion, string usuarioIng)
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
                    SET anio_operacion = @AnioOperacion,
                        semana_operacion = @SemanaOperacion,
                        fecha_operacion = db_tesoreria.GetFechaOperacion(@AnioOperacion, @SemanaOperacion, dia_operacion)
                    WHERE codigo_estado = @CodigoEstadoRegistrado
                      AND usuario_ing = '" + usuarioIng + "'";

                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sentenciaUpdateTransaccion;
                    cmd.Parameters.AddWithValue("@CodigoEstadoRegistrado", Constantes.EstadoTransacccion.REGISTRADO);
                    cmd.Parameters.AddWithValue("@SemanaOperacion", semanaOperacion);
                    cmd.Parameters.AddWithValue("@AnioOperacion", anioOperacion);
                    cmd.ExecuteNonQuery();

                    string sentenciaUpdateCuentaPorCobrar = @"
                    UPDATE db_contabilidad.cuenta_por_cobrar
                    SET semana_operacion = @SemanaOperacion,
                        anio_operacion = @AnioOperacion
                    WHERE codigo_transaccion IN (SELECT codigo_transaccion FROM db_tesoreria.transaccion WHERE codigo_estado = @CodigoEstadoRegistrado AND usuario_ing = '" + usuarioIng + "')";
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sentenciaUpdateCuentaPorCobrar;
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

                return resultado;
            }
        }

        public string AceptarRevision(long codigoTransaccion, int revisado, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    UPDATE db_tesoreria.transaccion
                    SET revisado = @Revisado,
                        usuario_revision = @UsuarioRevision,
                        fecha_revision = @FechaRevision
                    WHERE codigo_transaccion = @CodigoTransaccion";
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Revisado", revisado);
                        cmd.Parameters.AddWithValue("@UsuarioRevision", usuarioAct);
                        cmd.Parameters.AddWithValue("@FechaRevision", DateTime.Now);
                        cmd.Parameters.AddWithValue("@CodigoTransaccion", codigoTransaccion);

                        // rows sotred the number of rows affected
                        cmd.ExecuteNonQuery();
                        
                    }
                    conexion.Close();
                    resultado = "OK";
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    resultado = "Error [0]: " + ex.Message;
                }

                return resultado;
            }
        }

        public string AutorizarCorreccion(long codigoTransaccion, string observaciones, int codigoResultado, string usuarioAct)
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
                    string sentenciaUpdateSolicitudCorreccion = @"
                    UPDATE db_tesoreria.solicitud_correccion
                    SET observaciones_aprobacion = @ObservacionesAprobacion,
                        resultado = @Resultado,
                        usuario_aprobacion = @UsuarioAprobacion,
                        fecha_aprobacion = @FechaAprobacion,
                        usuario_act = @UsuarioAct,
                        fecha_act = @FechaAct
                    WHERE codigo_transaccion = @CodigoTransaccion";

                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sentenciaUpdateSolicitudCorreccion;
                    cmd.Parameters.AddWithValue("@CodigoTransaccion", codigoTransaccion);
                    cmd.Parameters.AddWithValue("@ObservacionesAprobacion", observaciones);
                    cmd.Parameters.AddWithValue("@Resultado", codigoResultado);
                    cmd.Parameters.AddWithValue("@UsuarioAprobacion", usuarioAct);
                    cmd.Parameters.AddWithValue("@FechaAprobacion", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UsuarioAct", usuarioAct);
                    cmd.Parameters.AddWithValue("@FechaAct", DateTime.Now);
                    // rows sotred the number of rows affected
                    int rowSolicitud = cmd.ExecuteNonQuery();

                    string sentenciaUpdateTransaccion = @"
                    UPDATE db_tesoreria.transaccion
                    SET codigo_estado_solicitud_correccion = @CodigoEstadoResultadoAprobacion
                    WHERE codigo_transaccion = @CodigoTransaccion";

                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sentenciaUpdateTransaccion;
                    cmd.Parameters["@CodigoTransaccion"].Value = codigoTransaccion;
                    if (codigoResultado == Constantes.Correccion.ResultadoSolicitudCorreccion.APROBADA)
                    {
                        cmd.Parameters.AddWithValue("@CodigoEstadoResultadoAprobacion", Constantes.Correccion.EstadoSolicitudcorreccion.VISTO_BUENO);
                    }
                    else {
                        cmd.Parameters.AddWithValue("@CodigoEstadoResultadoAprobacion", Constantes.Correccion.EstadoSolicitudcorreccion.DENEGADO);
                    }
                    // rows sotred the number of rows affected
                    int rowTransaccion = cmd.ExecuteNonQuery();

                    if (rowSolicitud > 0 && rowTransaccion > 0)
                    {
                        transaction.Commit();
                        conexion.Close();
                        resultado = "OK";
                    }
                    else {
                        transaction.Rollback();
                        conexion.Close();
                        resultado = "Error [0]: No se pudo registrar la transacción";
                    }

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    conexion.Close();
                    resultado = "Error [0]: " + ex.Message;
                }

                return resultado;
            }
        }

        public SolicitudCorreccionCLS GetDataCorreccion(long codigoTransaccion)
        {
            SolicitudCorreccionCLS objSolicitudCorreccion = new SolicitudCorreccionCLS();
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    SELECT codigo_transaccion, 
	                       codigo_transaccion_correcta, 
	                       observaciones_solicitud,
	                       observaciones_aprobacion,
	                       CASE	
		                      WHEN resultado = 1 THEN 'APROBADO'
		                      WHEN resultado = 2 THEN 'DENEGADO'
		                      ELSE 'Sin resultado'
	                       END AS resultado,
	                       usuario_ing AS usuario_solicitud,
	                       fecha_ing AS fecha_solicitud,
                           FORMAT(fecha_ing, 'dd/MM/yyyy, hh:mm:ss') AS fecha_solicitud_str,
	                       usuario_aprobacion,
	                       fecha_aprobacion,
                           FORMAT(fecha_aprobacion, 'dd/MM/yyyy, hh:mm:ss') AS fecha_aprobacion_str,
                           codigo_tipo_correccion,
                           CASE
                             WHEN codigo_tipo_correccion = 1 THEN 'MODIFICACIÓN'
                             WHEN codigo_tipo_correccion = 2 THEN 'ANULACIÓN'
                             ELSE ''
                           END AS tipo_correccion 
                    FROM db_tesoreria.solicitud_correccion
                    WHERE codigo_transaccion = @CodigoTransaccion";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoTransaccion", codigoTransaccion);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            int postCodigoTransaccion = dr.GetOrdinal("codigo_transaccion");
                            int postCodigoTransaccionCorrecta = dr.GetOrdinal("codigo_transaccion_correcta");
                            int postObservacionesSolicitud = dr.GetOrdinal("observaciones_solicitud");
                            int postObservacionesAprobacion = dr.GetOrdinal("observaciones_aprobacion");
                            int postResultado = dr.GetOrdinal("resultado");
                            int postUsuarioSolicitud = dr.GetOrdinal("usuario_solicitud");
                            int postFechaSolicitudStr = dr.GetOrdinal("fecha_solicitud_str");
                            int postUsuarioAprobacion = dr.GetOrdinal("usuario_aprobacion");
                            int postFechaAprobacionStr = dr.GetOrdinal("fecha_aprobacion_str");
                            int postCodigoTipoCorreccion = dr.GetOrdinal("codigo_tipo_correccion");
                            int postTipoCorreccion = dr.GetOrdinal("tipo_correccion");
                            while (dr.Read())
                            {
                                objSolicitudCorreccion.CodigoTransaccion = dr.GetInt64(postCodigoTransaccion);
                                objSolicitudCorreccion.CodigoTransaccionCorrecta = dr.IsDBNull(postCodigoTransaccionCorrecta) ? 0 : dr.GetInt64(postCodigoTransaccionCorrecta);
                                objSolicitudCorreccion.ObservacionesSolicitud = dr.IsDBNull(postObservacionesSolicitud) ? "" : dr.GetString(postObservacionesSolicitud);
                                objSolicitudCorreccion.ObservacionesAprobacion = dr.IsDBNull(postObservacionesAprobacion) ? "" : dr.GetString(postObservacionesAprobacion);
                                objSolicitudCorreccion.Resultado = dr.GetString(postResultado);
                                objSolicitudCorreccion.UsuarioIng = dr.GetString(postUsuarioSolicitud);
                                objSolicitudCorreccion.FechaIngStr = dr.GetString(postFechaSolicitudStr);
                                objSolicitudCorreccion.UsuarioAprobacion = dr.IsDBNull(postUsuarioAprobacion) ? "" : dr.GetString(postUsuarioAprobacion);
                                objSolicitudCorreccion.FechaAprobacionStr = dr.IsDBNull(postFechaAprobacionStr) ? "" : dr.GetString(postFechaAprobacionStr);
                                objSolicitudCorreccion.CodigoTipoCorreccion = dr.GetByte(postCodigoTipoCorreccion);
                                objSolicitudCorreccion.TipoCorreccion = dr.GetString(postTipoCorreccion);
                            }
                        }
                    }
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                }

                return objSolicitudCorreccion;
            }
        }

        public decimal GetMontoPlanillaParaDesglosar(int anioOperacion, int semanaOperacion, int codigoReporte)
        {
            decimal resultado = 0;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    SELECT COALESCE(sum(monto),0) AS monto_planilla_pago_a_desglosar
                    FROM db_tesoreria.transaccion x
					INNER JOIN db_tesoreria.operacion y
					ON x.codigo_operacion = y.codigo_operacion
					INNER JOIN db_tesoreria.tipo_operacion z
					ON y.codigo_tipo_operacion = z.codigo_tipo_operacion
					WHERE x.codigo_estado <> 0 
					  AND x.anio_operacion = @AnioOperacion
					  AND x.semana_operacion = @SemanaOperacion
					  AND x.codigo_reporte = @CodigoReporte
					  AND x.codigo_operacion = @CodigoOperacion
					  AND x.codigo_categoria_entidad = @CodigoCategoriaEntidad
					  AND z.signo = -1
					  AND x.complemento_conta = 0";
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@AnioOperacion", anioOperacion);
                        cmd.Parameters.AddWithValue("@SemanaOperacion", semanaOperacion);
                        cmd.Parameters.AddWithValue("@CodigoReporte", codigoReporte);
                        cmd.Parameters.AddWithValue("@CodigoOperacion", Constantes.Operacion.Egreso.PLANILLA_PAGO);
                        cmd.Parameters.AddWithValue("@CodigoCategoriaEntidad", Constantes.Entidad.Categoria.EMPLEADO);

                        // rows sotred the number of rows affected
                        //cmd.ExecuteNonQuery();
                        resultado = (decimal) cmd.ExecuteScalar();
                    }
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                    resultado = -1;
                }

                return resultado;
            }
        }



    }
}
