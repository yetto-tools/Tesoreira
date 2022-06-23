using CapaEntidad.Tesoreria;
using CapaEntidad.Ventas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Tesoreria
{
    public class Especiales2DAL: CadenaConexion
    {
         public List<TrasladoEspeciales2DetalleCLS> GetTrasladosParaDepuracion(int codigoTraslado)
         {
            List<TrasladoEspeciales2DetalleCLS> listaDetalle = new List<TrasladoEspeciales2DetalleCLS>();
            List<ClienteCLS> listaClientesEspeciales2 = new List<ClienteCLS>();
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    int postCodigoCliente = 0;
                    int postNombreCliente = 0;
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_tesoreria.uspGetTrasladoEspeciales2ParaDepuracion", conexion))
                    {
                        // le indico que es un procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CodigoTraslado", codigoTraslado);
                        // Devolvera varias tablas
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr != null)
                        {
                            postCodigoCliente = dr.GetOrdinal("codigo_cliente");
                            postNombreCliente = dr.GetOrdinal("nombre_completo");

                            ClienteCLS objCliente;
                            while (dr.Read())
                            {
                                objCliente = new ClienteCLS();
                                objCliente.CodigoCliente = dr.GetString(postCodigoCliente);
                                objCliente.NombreCompleto = dr.GetString(postNombreCliente);
                                listaClientesEspeciales2.Add(objCliente);
                            }//fin while
                        }// fin if

                        if (dr.NextResult())
                        {
                            int postSerie = dr.GetOrdinal("serie");
                            int postNumeroPedido = dr.GetOrdinal("pedido");
                            int postCodigoEmpresa = dr.GetOrdinal("empresa");
                            postCodigoCliente = dr.GetOrdinal("codigo_cliente");
                            postNombreCliente = dr.GetOrdinal("nombre_cliente");
                            int postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            int postNombreEntidad = dr.GetOrdinal("nombre_entidad");
                            int postMonto = dr.GetOrdinal("monto");
                            int postModificacion = dr.GetOrdinal("modificacion");
                            int postCodigoEstadoDepuracion = dr.GetOrdinal("codigo_estado_depuracion");

                            int ratio = 0;
                            int ratioAnterior = 0;
                            TrasladoEspeciales2DetalleCLS objDetalle;
                            while (dr.Read())
                            {
                                objDetalle = new TrasladoEspeciales2DetalleCLS();
                                objDetalle.Serie = dr.GetString(postSerie);
                                objDetalle.NumeroPedido = dr.GetInt64(postNumeroPedido);
                                objDetalle.CodigoEmpresa = dr.GetString(postCodigoEmpresa);
                                objDetalle.CodigoCliente = dr.GetString(postCodigoCliente);
                                objDetalle.NombreCliente = dr.GetString(postNombreCliente);
                                objDetalle.CodigoEstadoDepuracion = (byte)dr.GetInt32(postCodigoEstadoDepuracion);
                                if (objDetalle.CodigoEstadoDepuracion == Constantes.Especiales2.EstadoDetalleTraslado.NO_DEPURADO)
                                {
                                    objDetalle.NombreCliente = new String(objDetalle.NombreCliente.Where(f => char.IsLetter(f) || f == ' ').ToArray());
                                    string[] textSplit = objDetalle.NombreCliente.Split();
                                    int len = textSplit.Length;
                                    if (len > 1)
                                    {
                                        if (textSplit[1] == "TRAE")
                                        {
                                            objDetalle.NombreCliente = textSplit[0];
                                        }
                                        else {
                                            objDetalle.NombreCliente = textSplit[0] + " " + textSplit[1];
                                        }
                                        if (textSplit[1] == "DE" || textSplit[1] == "LA")
                                        {
                                            if (len > 2)
                                            {
                                                if (textSplit[2] == "LAS")
                                                {
                                                    if (len > 3)
                                                        objDetalle.NombreCliente = objDetalle.NombreCliente + " " + textSplit[2] + " " + textSplit[3];
                                                    else
                                                        objDetalle.NombreCliente = objDetalle.NombreCliente + " " + textSplit[2];
                                                }
                                                else
                                                {
                                                    objDetalle.NombreCliente = objDetalle.NombreCliente + " " + textSplit[2];
                                                }
                                            }
                                        }
                                    }
                                    else {
                                        objDetalle.NombreCliente = textSplit[0];
                                    }

                                    ratioAnterior = 100;
                                    foreach (ClienteCLS clienteEspecial2 in listaClientesEspeciales2)
                                    {
                                        ratio = Util.Manipulacion.Compute(clienteEspecial2.NombreCompleto, objDetalle.NombreCliente);
                                        if (ratio < ratioAnterior && ratio < 2)
                                        {
                                            objDetalle.CodigoEntidad = clienteEspecial2.CodigoCliente;
                                            objDetalle.NombreEntidad = clienteEspecial2.NombreCompleto;
                                            objDetalle.CodigoEstadoDepuracion = Constantes.Especiales2.EstadoDetalleTraslado.EXISTE_COINCIDENCIA;
                                            ratioAnterior = ratio;
                                        }
                                    }
                                }
                                else {
                                    objDetalle.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                    objDetalle.NombreEntidad = dr.GetString(postNombreEntidad);
                                }
                                objDetalle.RatioSimilitud = ratioAnterior;
                                objDetalle.Monto = dr.GetDecimal(postMonto);
                                objDetalle.modificacion = dr.GetByte(postModificacion);
                                

                                listaDetalle.Add(objDetalle);
                            }//fin while
                        }// fin if
                    }// fin using
                    conexion.Close();
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    listaDetalle = null;

                }
                return listaDetalle;
            }
        }

        public string ActualizarDetalleEspeciales2(List<TrasladoEspeciales2DetalleCLS> listDetalle, int codigoTraslado, string usuarioAct)
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
                    string sentenciaUpdateDetalle = String.Empty;
                    int contador = 0;
                    foreach (TrasladoEspeciales2DetalleCLS objDetalle in listDetalle)
                    {
                        if (contador == 0)
                        {
                            cmd.Parameters.Add("@Serie", SqlDbType.VarChar);
                            cmd.Parameters.Add("@Pedido", SqlDbType.BigInt);
                            cmd.Parameters.Add("@Empresa", SqlDbType.VarChar);
                            cmd.Parameters.Add("@CodigoEntidad", SqlDbType.VarChar);
                            cmd.Parameters.Add("@NombreEntidad", SqlDbType.VarChar);
                            cmd.Parameters.Add("@Modificacion", SqlDbType.TinyInt);
                            cmd.Parameters.Add("@CodigoEstadoDepuracion", SqlDbType.TinyInt);
                        }

                        sentenciaUpdateDetalle = @"
                        UPDATE db_tesoreria.traslado_detalle_especiales2 
                        SET codigo_entidad = @CodigoEntidad,
                            nombre_entidad = @NombreEntidad,
                            codigo_estado_depuracion = @CodigoEstadoDepuracion
                        WHERE serie = @Serie
                          AND pedido = @Pedido
                          AND empresa = @Empresa";

                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sentenciaUpdateDetalle;

                        cmd.Parameters["@Serie"].Value = objDetalle.Serie;
                        cmd.Parameters["@Pedido"].Value = objDetalle.NumeroPedido;
                        cmd.Parameters["@Empresa"].Value = objDetalle.CodigoEmpresa;
                        if (objDetalle.CodigoEstadoDepuracion == Constantes.Especiales2.EstadoDetalleTraslado.NO_DEPURADO)
                        {
                            cmd.Parameters["@CodigoEntidad"].Value = objDetalle.CodigoCliente;
                            cmd.Parameters["@NombreEntidad"].Value = objDetalle.NombreCliente;
                        }
                        else
                        {
                            cmd.Parameters["@CodigoEntidad"].Value = objDetalle.CodigoEntidad;
                            cmd.Parameters["@NombreEntidad"].Value = objDetalle.NombreEntidad;
                        }
                        cmd.Parameters["@Modificacion"].Value = 1;
                        cmd.Parameters["@CodigoEstadoDepuracion"].Value = objDetalle.CodigoEstadoDepuracion;
                        cmd.ExecuteNonQuery();
                        contador++;
                    }

                    string sentenciaUpdateEncabezado = @"
                    UPDATE db_tesoreria.traslado_especiales2 
                    SET codigo_estado = @CodigoEstadoTraslado,
                        usuario_act = @UsuarioAct,
                        fecha_act = @FechaAct
                    WHERE codigo_traslado = @CodigoTraslado";

                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sentenciaUpdateEncabezado;
                    cmd.Parameters.AddWithValue("@CodigoTraslado", codigoTraslado);
                    cmd.Parameters.AddWithValue("@CodigoEstadoTraslado", Constantes.Especiales2.EstadoTraslado.DEPURADO);
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

                return resultado;
            }// fin using
        }// fin metodo

        public List<TrasladoEspeciales2DetalleCLS> GetDetalleUnificadoEspeciales2(int codigoTraslado)
        {
            List<TrasladoEspeciales2DetalleCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    SELECT codigo_entidad, 
	                       nombre_entidad, 
	                       codigo_estado_depuracion, 
	                       SUM(monto) AS monto
                    FROM db_tesoreria.traslado_detalle_especiales2
                    WHERE codigo_traslado = @CodigoTraslado
                    GROUP BY codigo_entidad, nombre_entidad, codigo_estado_depuracion";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoTraslado", codigoTraslado);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            TrasladoEspeciales2DetalleCLS objDetalle;
                            lista = new List<TrasladoEspeciales2DetalleCLS>();
                            int postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            int postNombreEntidad = dr.GetOrdinal("nombre_entidad");
                            int postMonto = dr.GetOrdinal("monto");
                            int postCodigoEstadoDepuracion = dr.GetOrdinal("codigo_estado_depuracion");
                            while (dr.Read())
                            {
                                objDetalle = new TrasladoEspeciales2DetalleCLS();
                                objDetalle.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objDetalle.NombreEntidad = dr.GetString(postNombreEntidad);
                                objDetalle.Monto = dr.GetDecimal(postMonto);
                                objDetalle.CodigoEstadoDepuracion = dr.GetByte(postCodigoEstadoDepuracion);
                                lista.Add(objDetalle);
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

        public List<TrasladoEspeciales2DetalleCLS> GetDetalleEspeciales2(int codigoTraslado)
        {
            List<TrasladoEspeciales2DetalleCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    SELECT x.serie,
		                   x.pedido,
		                   x.empresa,
		                   x.codigo_cliente,
		                   x.nombre_cliente,
		                   CASE
			                 WHEN (x.codigo_entidad = '000001' OR x.codigo_entidad IS NULL OR x.codigo_entidad = '') THEN NULL
			                 ELSE x.codigo_entidad
		                   END AS codigo_entidad,
		                   CASE
			                 WHEN (x.codigo_entidad = '000001' OR x.codigo_entidad IS NULL OR x.codigo_entidad = '') THEN NULL
			                 ELSE x.nombre_entidad
		                   END AS nombre_entidad,
		                   x.monto,
                           FORMAT(x.fecha_grabado, 'dd/MM/yyyy, hh:mm:ss') AS fecha_grabado_str,
		                   x.modificacion,
		                   x.codigo_estado_depuracion,
                           y.nombre AS estado_depuracion 

	                FROM db_tesoreria.traslado_detalle_especiales2 x
                    INNER JOIN db_tesoreria.estado_depuracion_especiales2 y
                    ON x.codigo_estado_depuracion = y.codigo_estado_depuracion
                    WHERE x.codigo_traslado = @CodigoTraslado
	                ORDER BY x.nombre_cliente";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoTraslado", codigoTraslado);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            TrasladoEspeciales2DetalleCLS objDetalle;
                            lista = new List<TrasladoEspeciales2DetalleCLS>();
                            int postSerie = dr.GetOrdinal("serie");
                            int postNumeroPedido = dr.GetOrdinal("pedido");
                            int postCodigoEmpresa = dr.GetOrdinal("empresa");
                            int postCodigoCliente = dr.GetOrdinal("codigo_cliente");
                            int postNombreCliente = dr.GetOrdinal("nombre_cliente");
                            int postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            int postNombreEntidad = dr.GetOrdinal("nombre_entidad");
                            int postMonto = dr.GetOrdinal("monto");
                            int postFechaGrabadoStr = dr.GetOrdinal("fecha_grabado_str");
                            int postCodigoEstadoDepuracion = dr.GetOrdinal("codigo_estado_depuracion");
                            int postEstadoDepuracion = dr.GetOrdinal("estado_depuracion");
                            while (dr.Read())
                            {
                                objDetalle = new TrasladoEspeciales2DetalleCLS();
                                objDetalle.Serie = dr.GetString(postSerie);
                                objDetalle.NumeroPedido = dr.GetInt64(postNumeroPedido);
                                objDetalle.CodigoEmpresa = dr.GetString(postCodigoEmpresa);
                                objDetalle.CodigoCliente = dr.GetString(postCodigoCliente);
                                objDetalle.NombreCliente = dr.GetString(postNombreCliente);
                                objDetalle.CodigoEstadoDepuracion = dr.GetByte(postCodigoEstadoDepuracion);
                                if (objDetalle.CodigoEstadoDepuracion == Constantes.Especiales2.EstadoDetalleTraslado.NO_DEPURADO)
                                {
                                    objDetalle.NombreCliente = new String(objDetalle.NombreCliente.Where(f => char.IsLetter(f) || f == ' ').ToArray());
                                    string[] textSplit = objDetalle.NombreCliente.Split();
                                    int len = textSplit.Length;
                                    if (len > 1)
                                    {
                                        if (textSplit[1] == "TRAE")
                                        {
                                            objDetalle.NombreCliente = textSplit[0];
                                        }
                                        else
                                        {
                                            objDetalle.NombreCliente = textSplit[0] + " " + textSplit[1];
                                        }
                                        if (textSplit[1] == "DE" || textSplit[1] == "LA")
                                        {
                                            if (len > 2)
                                            {
                                                if (textSplit[2] == "LAS")
                                                {
                                                    if (len > 3)
                                                        objDetalle.NombreCliente = objDetalle.NombreCliente + " " + textSplit[2] + " " + textSplit[3];
                                                    else
                                                        objDetalle.NombreCliente = objDetalle.NombreCliente + " " + textSplit[2];
                                                }
                                                else
                                                {
                                                    objDetalle.NombreCliente = objDetalle.NombreCliente + " " + textSplit[2];
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        objDetalle.NombreCliente = textSplit[0];
                                    }
                                }
                                else
                                {
                                    objDetalle.CodigoEntidad = dr.IsDBNull(postCodigoEntidad) ? "" : dr.GetString(postCodigoEntidad);
                                    objDetalle.NombreEntidad = dr.IsDBNull(postNombreEntidad) ? "" : dr.GetString(postNombreEntidad);
                                }

                                objDetalle.Monto = dr.GetDecimal(postMonto);
                                objDetalle.FechaGrabadoStr = dr.GetString(postFechaGrabadoStr);
                                objDetalle.EstadoDepuracion = dr.GetString(postEstadoDepuracion);
                                lista.Add(objDetalle);
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

        //public string RegistrarEspeciales2(List<TrasladoEspeciales2DetalleCLS> listDetalle, int codigoTraslado, DateTime fechaOperacion, int semanaOperacion, int anioOperacion, string usuarioIng)
        //{
        //    string resultado = "";
        //    using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
        //    {
        //        conexion.Open();

        //        SqlCommand cmd = conexion.CreateCommand();
        //        SqlTransaction transaction;

        //        // Start a local transaction.
        //        transaction = conexion.BeginTransaction("SampleTransaction");

        //        // Must assign both transaction object and connection
        //        // to Command object for a pending local transaction
        //        cmd.Connection = conexion;
        //        cmd.Transaction = transaction;

        //        try
        //        {
        //            string sentenciaGeneracionTransaccion = String.Empty;
        //            string sentenciaUpdateTransaccion = String.Empty;
        //            string sqlSequenceCliente = String.Empty;
        //            string sentenciaInsertCliente = String.Empty;
        //            string sqlSequence = String.Empty;

        //            long correlativoTransaccion = 0;
        //            long codigoTransaccion = 0;
        //            string codigoCliente = String.Empty;
        //            int contador = 0;
        //            int contadorCliente = 0;
        //            long codigoSecuencia = 0;
        //            long correlativoRecibo = 0;
        //            string codigoSeguridad = String.Empty;

        //            int anio = DateTime.Now.Year;
        //            foreach (TrasladoEspeciales2DetalleCLS objDetalle in listDetalle)
        //            {
        //                if (contador == 0)
        //                {
        //                    cmd.Parameters.Add("@CodigoSecuencia", SqlDbType.BigInt);
        //                    cmd.Parameters.Add("@AnioTransaccion", SqlDbType.SmallInt);
        //                    cmd.Parameters.Add("@CodigoTransaccion", SqlDbType.BigInt);
        //                    cmd.Parameters.Add("@CodigoSeguridad", SqlDbType.VarChar);
        //                    cmd.Parameters.Add("@CodigoEmpresa", SqlDbType.SmallInt);
        //                    cmd.Parameters.Add("@CodigoOperacion", SqlDbType.SmallInt);
        //                    cmd.Parameters.Add("@CodigoOperacionCaja", SqlDbType.SmallInt);
        //                    cmd.Parameters.Add("@CodigoTipoCuentaPorCobrar", SqlDbType.TinyInt);
        //                    cmd.Parameters.Add("@CodigoCuentaPorCobrar", SqlDbType.BigInt);
        //                    cmd.Parameters.Add("@CodigoArea", SqlDbType.SmallInt);
        //                    cmd.Parameters.Add("@CodigoCategoriaEntidad", SqlDbType.SmallInt);
        //                    cmd.Parameters.Add("@CodigoEntidad", SqlDbType.VarChar);
        //                    cmd.Parameters.Add("@CodigoTipoOperacion", SqlDbType.TinyInt);
        //                    cmd.Parameters.Add("@CodigoTipoTransaccion", SqlDbType.VarChar);
        //                    cmd.Parameters.Add("@CodigoTipoDocumento", SqlDbType.TinyInt);
        //                    cmd.Parameters.Add("@Efectivo", SqlDbType.TinyInt);
        //                    cmd.Parameters.Add("@Deposito", SqlDbType.TinyInt);
        //                    cmd.Parameters.Add("@Cheque", SqlDbType.TinyInt);
        //                    cmd.Parameters.Add("@NitProveedor", SqlDbType.VarChar);
        //                    cmd.Parameters.Add("@SerieFactura", SqlDbType.VarChar);
        //                    cmd.Parameters.Add("@NumeroDocumento", SqlDbType.Int);
        //                    cmd.Parameters.Add("@FechaDocumento", SqlDbType.Date);
        //                    cmd.Parameters.Add("@ConcederIva", SqlDbType.TinyInt);
        //                    cmd.Parameters.Add("@NitEmpresaConcedeIva", SqlDbType.VarChar);
        //                    cmd.Parameters.Add("@CodigoBancoDeposito", SqlDbType.SmallInt);
        //                    cmd.Parameters.Add("@NumeroCuenta", SqlDbType.VarChar);
        //                    cmd.Parameters.Add("@NumeroBoleta", SqlDbType.VarChar);
        //                    cmd.Parameters.Add("@NumeroRecibo", SqlDbType.BigInt);
        //                    cmd.Parameters.Add("@FechaRecibo", SqlDbType.DateTime);
        //                    cmd.Parameters.Add("@FechaOperacion", SqlDbType.DateTime);
        //                    cmd.Parameters.Add("@AnioOperacion", SqlDbType.SmallInt);
        //                    cmd.Parameters.Add("@SemanaOperacion", SqlDbType.TinyInt);
        //                    cmd.Parameters.Add("@DiaOperacion", SqlDbType.TinyInt);
        //                    cmd.Parameters.Add("@CodigoBoletaComision", SqlDbType.Int);
        //                    cmd.Parameters.Add("@Ruta", SqlDbType.SmallInt);
        //                    cmd.Parameters.Add("@CodigoVendedor", SqlDbType.VarChar);
        //                    cmd.Parameters.Add("@SemanaComision", SqlDbType.TinyInt);
        //                    cmd.Parameters.Add("@AnioComision", SqlDbType.SmallInt);
        //                    cmd.Parameters.Add("@MontoEfectivo", SqlDbType.Decimal);
        //                    cmd.Parameters.Add("@MontoCheques", SqlDbType.Decimal);
        //                    cmd.Parameters.Add("@Monto", SqlDbType.Decimal);
        //                    cmd.Parameters.Add("@CodigoFrecuenciaPago", SqlDbType.TinyInt);
        //                    cmd.Parameters.Add("@CodigoTipoPago", SqlDbType.TinyInt);
        //                    cmd.Parameters.Add("@CodigoPlanilla", SqlDbType.BigInt);
        //                    cmd.Parameters.Add("@CodigoPagoPlanilla", SqlDbType.BigInt);
        //                    cmd.Parameters.Add("@AnioPlanilla", SqlDbType.SmallInt);
        //                    cmd.Parameters.Add("@MesPlanilla", SqlDbType.TinyInt);
        //                    cmd.Parameters.Add("@SemanaPlanilla", SqlDbType.TinyInt);
        //                    cmd.Parameters.Add("@CodigoQuincenaPlanilla", SqlDbType.Int);
        //                    cmd.Parameters.Add("@CodigoBonoExtra", SqlDbType.TinyInt);
        //                    cmd.Parameters.Add("@TipoEspeciales1", SqlDbType.TinyInt);
        //                    cmd.Parameters.Add("@Observaciones", SqlDbType.VarChar);
        //                    cmd.Parameters.Add("@FechaConfirmacion", SqlDbType.Decimal);
        //                    cmd.Parameters.Add("@MotivoAnulacion", SqlDbType.VarChar);
        //                    cmd.Parameters.Add("@UsuarioAnulacion", SqlDbType.VarChar);
        //                    cmd.Parameters.Add("@FechaAnulacion", SqlDbType.DateTime);
        //                    cmd.Parameters.Add("@CodigoEstado", SqlDbType.SmallInt);
        //                    cmd.Parameters.Add("@UsuarioIng", SqlDbType.VarChar);
        //                    cmd.Parameters.Add("@FechaIng", SqlDbType.DateTime);
        //                    cmd.Parameters.Add("@FechaAct", SqlDbType.DateTime);
        //                    cmd.Parameters.Add("@UsuarioAct", SqlDbType.VarChar);
        //                    cmd.Parameters.Add("@AnioSueldoIndirecto", SqlDbType.SmallInt);
        //                    cmd.Parameters.Add("@MesSueldoIndirecto", SqlDbType.TinyInt);
        //                    cmd.Parameters.Add("@ComplementoConta", SqlDbType.TinyInt);
        //                    cmd.Parameters.Add("@CodigoReporte", SqlDbType.Int);
        //                    cmd.Parameters.Add("@CodigoTipoDocumentoDeposito", SqlDbType.TinyInt);
        //                    cmd.Parameters.Add("@NumeroVoucher", SqlDbType.VarChar);
        //                    cmd.Parameters.Add("@NombreProveedor", SqlDbType.VarChar);
        //                    cmd.Parameters.Add("@CodigoCanalVenta", SqlDbType.SmallInt);
        //                    cmd.Parameters.Add("@CodigoOtroIngreso", SqlDbType.SmallInt);
        //                    cmd.Parameters.Add("@NumeroReciboReferencia", SqlDbType.BigInt);
        //                    cmd.Parameters.Add("@MontoSaldoAnteriorCuentaPorCobrar", SqlDbType.Decimal);
        //                    cmd.Parameters.Add("@MontoSaldoActualCuentaPorCobrar", SqlDbType.Decimal);

        //                }

        //                codigoCliente = objDetalle.CodigoEntidad;
        //                if (objDetalle.CodigoEstadoDepuracion == Constantes.Especiales2.EstadoDetalleTraslado.NO_DEPURADO ||
        //                    objDetalle.CodigoEstadoDepuracion == Constantes.Especiales2.EstadoDetalleTraslado.NO_EXISTE_CLIENTE)
        //                {
        //                    if (contadorCliente == 0)
        //                    {
        //                        cmd.Parameters.Add("@CodigoCliente", SqlDbType.VarChar);
        //                        cmd.Parameters.Add("@NombreCompleto", SqlDbType.VarChar);
        //                        cmd.Parameters.Add("@NombreCorto", SqlDbType.VarChar);
        //                        cmd.Parameters.Add("@CodigoTipoCliente", SqlDbType.SmallInt);
        //                        cmd.Parameters.Add("@Descripcion", SqlDbType.VarChar);
        //                        cmd.Parameters.Add("@CodigoClienteOrigen", SqlDbType.VarChar);
        //                        cmd.Parameters.Add("@CodigoEstadoCliente", SqlDbType.TinyInt);
        //                        cmd.Parameters.Add("@UsuarioIng", SqlDbType.VarChar);
        //                        cmd.Parameters.Add("@FechaIng", SqlDbType.DateTime);
        //                    }

        //                    /* Se debe de crear la entidad en la tabla db_ventas.cliente con el tipo de cliente  = 3 (Especiales II) */
        //                    sqlSequenceCliente = "SELECT NEXT VALUE FOR db_ventas.SQ_CLIENTE";
        //                    cmd.CommandText = sqlSequenceCliente;
        //                    codigoSecuencia = (long)cmd.ExecuteScalar();
        //                    sentenciaInsertCliente = @"
        //                    INSERT INTO db_ventas.cliente(codigo_cliente, nombre_completo, nombre_corto, codigo_tipo_cliente, descripcion, estado, usuario_ing, fecha_ing, codigo_cliente_origen)
        //                    VALUES(@CodigoCliente,@NombreCompleto,@NombreCorto,@CodigoTipoCliente,@Descripcion,@CodigoEstado, @UsuarioIng, @FechaIng, @CodigoClienteOrigen)";

        //                    cmd.CommandText = sentenciaInsertCliente;
        //                    codigoCliente = "9" + codigoSecuencia.ToString("D6");
        //                    cmd.Parameters["@CodigoCliente"].Value = codigoCliente;
        //                    cmd.Parameters["@NombreCompleto"].Value = objDetalle.NombreEntidad;
        //                    cmd.Parameters["@NombreCorto"].Value = objDetalle.NombreEntidad;
        //                    cmd.Parameters["@CodigoTipoCliente"].Value = Constantes.Cliente.Tipo.ESPECIALES_2;
        //                    cmd.Parameters["@Descripcion"].Value = DBNull.Value;

        //                    if (objDetalle.CodigoEstadoDepuracion == Constantes.Especiales2.EstadoDetalleTraslado.NO_EXISTE_CLIENTE)
        //                        cmd.Parameters["@CodigoClienteOrigen"].Value = objDetalle.CodigoEntidad;
        //                    else
        //                        cmd.Parameters["@CodigoClienteOrigen"].Value = DBNull.Value;
        //                    cmd.Parameters["@CodigoEstadoCliente"].Value = Constantes.EstadoRegistro.ACTIVO;
        //                    cmd.Parameters["@UsuarioIng"].Value = usuarioIng;
        //                    cmd.Parameters["@FechaIng"].Value = DateTime.Now;
        //                    cmd.ExecuteNonQuery();

        //                    contadorCliente++;
        //                }

        //                codigoSeguridad = Util.Seguridad.GenerarCadena();

        //                sentenciaGeneracionTransaccion = @"SELECT sig_valor FROM db_admon.secuencia_detalle WHERE codigo_secuencia = @CodigoSecuencia AND anio = @AnioTransaccion";
        //                cmd.CommandText = sentenciaGeneracionTransaccion;
        //                cmd.Parameters["@CodigoSecuencia"].Value = Constantes.Secuencia.SIT_SEQ_TRANSACCION;
        //                cmd.Parameters["@AnioTransaccion"].Value = anio;
        //                //cmd.Parameters.AddWithValue("@CodigoSecuencia", Constantes.Secuencia.SIT_SEQ_TRANSACCION);
        //                //cmd.Parameters.AddWithValue("@AnioTransaccion", anio);

        //                correlativoTransaccion = (long)cmd.ExecuteScalar();

        //                cmd.CommandText = "SELECT NEXT VALUE FOR db_tesoreria.SQ_RECIBO_INGRESO";
        //                correlativoRecibo = (long)cmd.ExecuteScalar();

        //                codigoTransaccion = long.Parse(anio.ToString() + correlativoTransaccion.ToString("D6"));
        //                string sentenciaSQL = @"
        //                INSERT INTO db_tesoreria.transaccion( codigo_transaccion,
        //                                                      codigo_seguridad,  
        //                                                      codigo_empresa,
        //                                                      codigo_operacion,
        //                                                      codigo_operacion_caja,
        //                                                      codigo_tipo_cxc,
        //                                                      codigo_cxc,
        //                                                      codigo_area,
        //                                                      codigo_categoria_entidad,
        //                                                      codigo_entidad,
        //                                                      codigo_tipo_operacion,  
        //                                                      codigo_tipo_transaccion,
        //                                                      codigo_tipo_documento,  
        //                                                      efectivo,
        //                                                      deposito,
        //                                                      cheque,  
        //                                                      nit_proveedor,  
        //                                                      serie_factura,
        //                                                      numero_documento,
        //                                                      fecha_documento,
        //                                                      conceder_iva,
        //                                                      nit_empresa_concede_iva,  
        //                                                      codigo_banco_deposito,
        //                                                      numero_cuenta,  
        //                                                      numero_boleta, 
        //                                                      numero_recibo,
        //                                                      fecha_recibo,
        //                                                      fecha_operacion,
        //                                                      anio_operacion,
        //                                                      semana_operacion,
        //                                                      dia_operacion,
        //                                                      codigo_boleta_comision,  
        //                                                      ruta,
        //                                                      codigo_vendedor,  
        //                                                      semana_comision,
        //                                                      anio_comision,
        //                                                      monto_efectivo,
        //                                                      monto_cheques,  
        //                                                      monto,
        //                                                      codigo_frecuencia_pago,  
        //                                                      codigo_tipo_pago,		
        //                                                   codigo_planilla,
        //                                                   codigo_pago_planilla,
        //                                                   anio_planilla,	
        //                                                   mes_planilla,		
        //                                                   semana_planilla,		
        //                                                   codigo_quincena_planilla,
        //                                                      codigo_bono_extra,  
        //                                                      tipo_especiales_1,
        //                                                      observaciones,
        //                                                      fecha_confirmacion,
        //                                                      motivo_anulacion,
        //                                                      usuario_anulacion,
        //                                                      fecha_anulacion,  
        //                                                      codigo_estado,
        //                                                      usuario_ing,
        //                                                      fecha_ing,
        //                                                      usuario_act,
        //                                                      fecha_act,
        //                                                      anio_sueldo_indirecto,
        //                                                      mes_sueldo_indirecto,
        //                                                      complemento_conta,
        //                                                      codigo_reporte,
        //                                                      codigo_tipo_doc_deposito,
        //                                                      numero_voucher,
        //                                                      nombre_proveedor,
        //                                                      codigo_canal_venta,
        //                                                      codigo_otro_ingreso,
        //                                                      numero_recibo_referencia,
        //                                                      monto_saldo_anterior_cxc,
        //                                                      monto_saldo_actual_cxc)
        //                VALUES(@CodigoTransaccion,
        //                       @CodigoSeguridad,
        //                       @CodigoEmpresa,
        //                       @CodigoOperacion,
        //                       @CodigoOperacionCaja,
        //                       @CodigoTipoCuentaPorCobrar,
        //                       @CodigoCuentaPorCobrar,
        //                       @CodigoArea,
        //                       @CodigoCategoriaEntidad,
        //                       @CodigoEntidad,
        //                       @CodigoTipoOperacion, 
        //                       @CodigoTipoTransaccion,
        //                       @CodigoTipoDocumento, 
        //                       @Efectivo,
        //                       @Deposito,
        //                       @Cheque, 
        //                       @NitProveedor, 
        //                       @SerieFactura,
        //                       @NumeroDocumento,
        //                       @FechaDocumento,
        //                       @ConcederIva,
        //                       @NitEmpresaConcedeIva, 
        //                       @CodigoBancoDeposito,
        //                       @NumeroCuenta,
        //                       @NumeroBoleta, 
        //                       @NumeroRecibo,
        //                       @FechaRecibo,
        //                       @FechaOperacion,
        //                       @AnioOperacion,
        //                       @SemanaOperacion,
        //                       @DiaOperacion,
        //                       @CodigoBoletaComision,
        //                       @Ruta,
        //                       @CodigoVendedor,
        //                       @SemanaComision, 
        //                       @AnioComision,
        //                       @MontoEfectivo,
        //                       @MontoCheques, 
        //                       @Monto,
        //                       @CodigoFrecuenciaPago, 
        //                       @CodigoTipoPago,
        //                       @CodigoPlanilla,
        //                       @CodigoPagoPlanilla,
        //                       @AnioPlanilla,
        //                       @MesPlanilla,
        //                       @SemanaPlanilla,
        //                       @CodigoQuincenaPlanilla, 
        //                       @CodigoBonoExtra,
        //                       @TipoEspeciales1,
        //                       @Observaciones,
        //                       @FechaConfirmacion,
        //                       @MotivoAnulacion,
        //                       @UsuarioAnulacion,
        //                       @FechaAnulacion, 
        //                       @CodigoEstado,
        //                       @UsuarioIng,
        //                       @FechaIng,
        //                       @UsuarioAct,
        //                       @FechaAct,
        //                       @AnioSueldoIndirecto,
        //                       @MesSueldoIndirecto,
        //                       @ComplementoConta,
        //                       @CodigoReporte,
        //                       @CodigoTipoDocumentoDeposito,
        //                       @NumeroVoucher,
        //                       @NombreProveedor,
        //                       @CodigoCanalVenta,
        //                       @CodigoOtroIngreso,
        //                       @NumeroReciboReferencia,
        //                       @MontoSaldoAnteriorCuentaPorCobrar,
        //                       @MontoSaldoActualCuentaPorCobrar)";

        //                cmd.CommandText = sentenciaSQL;
        //                cmd.Parameters["@CodigoTransaccion"].Value = codigoTransaccion;
        //                cmd.Parameters["@CodigoSeguridad"].Value = codigoSeguridad;
        //                cmd.Parameters["@CodigoEmpresa"].Value = DBNull.Value;
        //                cmd.Parameters["@CodigoOperacion"].Value = Constantes.Operacion.Ingreso.VENTAS_EN_RUTA;
        //                cmd.Parameters["@CodigoOperacionCaja"].Value  = Constantes.Operacion.Ingreso.ESPECIALES_2;
        //                cmd.Parameters["@CodigoTipoCuentaPorCobrar"].Value = 0;
        //                cmd.Parameters["@CodigoCuentaPorCobrar"].Value = DBNull.Value;
        //                cmd.Parameters["@CodigoArea"].Value = 0;
        //                cmd.Parameters["@CodigoCategoriaEntidad"].Value = Constantes.Entidad.Categoria.CLIENTES_ESPECIALES_2;
        //                cmd.Parameters["@CodigoEntidad"].Value = codigoCliente;
        //                cmd.Parameters["@CodigoTipoOperacion"].Value = Constantes.TipoOperacion.INGRESO;
        //                cmd.Parameters["@CodigoTipoTransaccion"].Value = "NF";
        //                cmd.Parameters["@CodigoTipoDocumento"].Value = Constantes.TipoDocumento.VALE;
        //                cmd.Parameters["@Efectivo"].Value = 1;
        //                cmd.Parameters["@Deposito"].Value = 0;
        //                cmd.Parameters["@Cheque"].Value = 0;
        //                cmd.Parameters["@NitProveedor"].Value = DBNull.Value;
        //                cmd.Parameters["@SerieFactura"].Value = DBNull.Value;
        //                cmd.Parameters["@NumeroDocumento"].Value = DBNull.Value;
        //                cmd.Parameters["@FechaDocumento"].Value = DBNull.Value;
        //                cmd.Parameters["@ConcederIva"].Value = 0;
        //                cmd.Parameters["@NitEmpresaConcedeIva"].Value = DBNull.Value;
        //                cmd.Parameters["@CodigoBancoDeposito"].Value = DBNull.Value;
        //                cmd.Parameters["@NumeroCuenta"].Value = DBNull.Value;
        //                cmd.Parameters["@NumeroBoleta"].Value = String.Empty;
        //                cmd.Parameters["@NumeroRecibo"].Value = correlativoRecibo;
        //                cmd.Parameters["@FechaRecibo"].Value = DateTime.Now;
        //                cmd.Parameters["@FechaOperacion"].Value = fechaOperacion;
        //                cmd.Parameters["@AnioOperacion"].Value = anioOperacion;
        //                cmd.Parameters["@SemanaOperacion"].Value = semanaOperacion;
        //                cmd.Parameters["@DiaOperacion"].Value = Util.Conversion.DayOfWeek(fechaOperacion);
        //                cmd.Parameters["@CodigoBoletaComision"].Value = DBNull.Value;
        //                cmd.Parameters["@Ruta"].Value = 0;
        //                cmd.Parameters["@CodigoVendedor"].Value = DBNull.Value;
        //                cmd.Parameters["@SemanaComision"].Value = 0;
        //                cmd.Parameters["@AnioComision"].Value = 0;
        //                cmd.Parameters["@MontoEfectivo"].Value = 0.00;
        //                cmd.Parameters["@MontoCheques"].Value = 0.00;
        //                cmd.Parameters["@Monto"].Value = objDetalle.Monto;
        //                cmd.Parameters["@CodigoFrecuenciaPago"].Value = 0;
        //                cmd.Parameters["@CodigoTipoPago"].Value = 0;
        //                cmd.Parameters["@CodigoPlanilla"].Value = 0;
        //                cmd.Parameters["@CodigoPagoPlanilla"].Value = 0;
        //                cmd.Parameters["@AnioPlanilla"].Value = 0;
        //                cmd.Parameters["@MesPlanilla"].Value = 0;
        //                cmd.Parameters["@SemanaPlanilla"].Value = 0;
        //                cmd.Parameters["@CodigoQuincenaPlanilla"].Value = 0;
        //                cmd.Parameters["@CodigoBonoExtra"].Value = 0;
        //                cmd.Parameters["@TipoEspeciales1"].Value = 0;
        //                cmd.Parameters["@Observaciones"].Value = DBNull.Value;
        //                cmd.Parameters["@FechaConfirmacion"].Value = DBNull.Value;
        //                cmd.Parameters["@MotivoAnulacion"].Value = DBNull.Value;
        //                cmd.Parameters["@UsuarioAnulacion"].Value = DBNull.Value;
        //                cmd.Parameters["@FechaAnulacion"].Value = DBNull.Value;
        //                cmd.Parameters["@CodigoEstado"].Value = Constantes.EstadoTransacccion.REGISTRADO;
        //                cmd.Parameters["@UsuarioIng"].Value = usuarioIng;
        //                cmd.Parameters["@FechaIng"].Value = DateTime.Now;
        //                cmd.Parameters["@FechaAct"].Value = DBNull.Value;
        //                cmd.Parameters["@UsuarioAct"].Value = DBNull.Value;
        //                cmd.Parameters["@AnioSueldoIndirecto"].Value = 0;
        //                cmd.Parameters["@MesSueldoIndirecto"].Value = 0;
        //                cmd.Parameters["@ComplementoConta"].Value = 0;
        //                cmd.Parameters["@CodigoReporte"].Value = DBNull.Value;
        //                cmd.Parameters["@CodigoTipoDocumentoDeposito"].Value = 0;
        //                cmd.Parameters["@NumeroVoucher"].Value = String.Empty;
        //                cmd.Parameters["@NombreProveedor"].Value = DBNull.Value;
        //                cmd.Parameters["@CodigoCanalVenta"].Value = 0;
        //                cmd.Parameters["@CodigoOtroIngreso"].Value = 0;
        //                cmd.Parameters["@NumeroReciboReferencia"].Value = 0;
        //                cmd.Parameters["@MontoSaldoAnteriorCuentaPorCobrar"].Value = 0;
        //                cmd.Parameters["@MontoSaldoActualCuentaPorCobrar"].Value = 0;
        //                cmd.ExecuteNonQuery();

        //                cmd.CommandText = "UPDATE db_admon.secuencia_detalle SET sig_valor = @siguienteValor WHERE codigo_secuencia = @CodigoSecuencia  AND anio = @AnioTransaccion";
        //                cmd.Parameters.AddWithValue("@siguienteValor", correlativoTransaccion + 1);
        //                cmd.Parameters["@CodigoSecuencia"].Value = Constantes.Secuencia.SIT_SEQ_TRANSACCION;
        //                cmd.Parameters["@AnioTransaccion"].Value = anio;
        //                cmd.ExecuteNonQuery();

        //                contador++;
        //            }

        //            string sentenciaUpdateEncabezado = @"
        //            UPDATE db_tesoreria.traslado_especiales2 
        //            SET codigo_estado = @CodigoEstadoTraslado,
        //                usuario_act = @UsuarioAct,
        //                fecha_act = @FechaAct
        //            WHERE codigo_traslado = @CodigoTraslado";

        //            cmd.CommandType = CommandType.Text;
        //            cmd.CommandText = sentenciaUpdateEncabezado;
        //            cmd.Parameters.AddWithValue("@CodigoTraslado", codigoTraslado);
        //            cmd.Parameters.AddWithValue("@CodigoEstadoTraslado", Constantes.Especiales2.EstadoTraslado.CARGA_COMPLETADA);
        //            cmd.Parameters.AddWithValue("@UsuarioAct", usuarioIng);
        //            cmd.Parameters.AddWithValue("@FechaAct", DateTime.Now);
        //            cmd.ExecuteNonQuery();

        //            transaction.Commit();
        //            conexion.Close();

        //            resultado = "OK";

        //        }
        //        catch (Exception ex)
        //        {
        //            transaction.Rollback();
        //            conexion.Close();
        //            resultado = "Error [0]: " + ex.Message;
        //        }

        //        return resultado;
        //    }// fin using
        //}// fin metodo

        public string RegistrarEspeciales2(int codigoTraslado, string fechaOperacionStr, int semanaOperacion, int anioOperacion, string usuarioIng)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string[] word = fechaOperacionStr.Split('/');
                    int anio = Convert.ToInt32(word[2]);
                    int mes = Convert.ToInt32(word[1]);
                    int dia = Convert.ToInt32(word[0]);
                    string fechaOperacion = anio.ToString() + "-" + mes.ToString("D2") + "-" + dia.ToString("D2");
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_tesoreria.uspRegistrarEspeciales2", conexion))
                    {
                        // le indico que es un procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CodigoTraslado", codigoTraslado);
                        cmd.Parameters.AddWithValue("@FechaOperacion", fechaOperacion);
                        cmd.Parameters.AddWithValue("@DiaOperacion", dia);
                        cmd.Parameters.AddWithValue("@SemanaOperacion", semanaOperacion);
                        cmd.Parameters.AddWithValue("@AnioOperacion", anioOperacion);
                        cmd.Parameters.AddWithValue("@UsuarioIng", usuarioIng);

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

    }
}
