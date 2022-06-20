using CapaEntidad.CROM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.CROM
{
    public class Especiales2DAL: CadenaConexion
    {
        public string GuardarTraslados(List<TrasladoEspeciales2DetalleCLS> listDetalle, int codigoTraslado, DateTime fechaOperacion, string usuarioIng)
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
                    string sentenciaInsertEncabezado = @"
                    INSERT INTO db_tesoreria.traslado_especiales2(codigo_traslado,fecha_operacion,fecha_recepcion,usuario_recepcion,observaciones_recepcion,codigo_estado,usuario_ing,fecha_ing,usuario_act,fecha_act)
                    VALUES( @CodigoTraslado,
                            @FechaOperacion,
                            @FechaRecepcion,
                            @UsuarioRecepcion,
                            @ObservacionesRecepcion,
                            @CodigoEstado,
                            @UsuarioIng,
                            @FechaIng,
                            @UsuarioAct,
                            @FechaAct)";

                    cmd.CommandText = sentenciaInsertEncabezado;
                    cmd.Parameters.AddWithValue("@CodigoTraslado", codigoTraslado);
                    cmd.Parameters.AddWithValue("@FechaOperacion", fechaOperacion);
                    cmd.Parameters.AddWithValue("@FechaRecepcion", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UsuarioRecepcion", usuarioIng);
                    cmd.Parameters.AddWithValue("@ObservacionesRecepcion", DBNull.Value);
                    cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.Especiales2.EstadoTraslado.RECEPCIONADO);
                    cmd.Parameters.AddWithValue("@UsuarioIng", usuarioIng);
                    cmd.Parameters.AddWithValue("@FechaIng", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UsuarioAct", DBNull.Value);
                    cmd.Parameters.AddWithValue("@FechaAct", DBNull.Value);
                    cmd.ExecuteNonQuery();

                    string sentenciaInsertDetalle = String.Empty;
                    int contador = 0;
                    foreach (TrasladoEspeciales2DetalleCLS objDetalle in listDetalle)
                    {
                        if (contador == 0)
                        {
                            cmd.Parameters.Add("@Serie", SqlDbType.VarChar);
                            cmd.Parameters.Add("@Pedido", SqlDbType.BigInt);
                            cmd.Parameters.Add("@Empresa", SqlDbType.VarChar);
                            //cmd.Parameters.Add("@CodigoTraslado", SqlDbType.Int);
                            cmd.Parameters.Add("@CodigoCliente", SqlDbType.VarChar);
                            cmd.Parameters.Add("@NombreCliente", SqlDbType.VarChar);
                            cmd.Parameters.Add("@CodigoEntidad", SqlDbType.VarChar);
                            cmd.Parameters.Add("@NombreEntidad", SqlDbType.VarChar);
                            cmd.Parameters.Add("@Monto", SqlDbType.Decimal);
                            cmd.Parameters.Add("@FechaGrabado", SqlDbType.DateTime);
                            cmd.Parameters.Add("@Modificacion", SqlDbType.TinyInt);
                            cmd.Parameters.Add("@CodigoEstadoDetalle", SqlDbType.TinyInt);
                            cmd.Parameters.Add("@CodigoEstadoDepuracion", SqlDbType.TinyInt);
                            //cmd.Parameters.Add("@UsuarioIng", SqlDbType.VarChar);
                            //cmd.Parameters.Add("@FechaIng", SqlDbType.DateTime);
                            //cmd.Parameters.Add("@UsuarioAct", SqlDbType.VarChar);
                            //cmd.Parameters.Add("@FechaAct", SqlDbType.DateTime);
                        }
                        sentenciaInsertDetalle = @"
                        INSERT INTO db_tesoreria.traslado_detalle_especiales2(serie,pedido,empresa,codigo_traslado,codigo_cliente,nombre_cliente,codigo_entidad,nombre_entidad,monto,fecha_grabado,modificacion,estado,codigo_estado_depuracion,usuario_ing,fecha_ing,usuario_act,fecha_act)
                        VALUES( @Serie,
                                @Pedido,
                                @Empresa,
                                @CodigoTraslado,
                                @CodigoCliente,
                                @NombreCliente,
                                @CodigoEntidad,
                                @NombreEntidad,
                                @Monto,
                                @FechaGrabado,
                                @Modificacion,
                                @CodigoEstadoDetalle,
                                @CodigoEstadoDepuracion,
                                @UsuarioIng,
                                @FechaIng,
                                @UsuarioAct,
                                @FechaAct)";

                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sentenciaInsertDetalle;

                        cmd.Parameters["@Serie"].Value = objDetalle.Serie;
                        cmd.Parameters["@Pedido"].Value = objDetalle.NumeroPedido;
                        cmd.Parameters["@Empresa"].Value = objDetalle.CodigoEmpresa;
                        cmd.Parameters["@CodigoTraslado"].Value = objDetalle.CodigoTraslado;
                        cmd.Parameters["@CodigoCliente"].Value = objDetalle.CodigoCliente;
                        cmd.Parameters["@NombreCliente"].Value = objDetalle.NombreCliente;
                        cmd.Parameters["@CodigoEntidad"].Value = objDetalle.CodigoCliente;
                        cmd.Parameters["@NombreEntidad"].Value = objDetalle.NombreCliente;
                        cmd.Parameters["@Monto"].Value = objDetalle.Monto;
                        cmd.Parameters["@FechaGrabado"].Value = objDetalle.FechaGrabadoStr;
                        cmd.Parameters["@Modificacion"].Value = 0;
                        cmd.Parameters["@CodigoEstadoDetalle"].Value = Constantes.EstadoRegistro.ACTIVO;
                        cmd.Parameters["@CodigoEstadoDepuracion"].Value = Constantes.Especiales2.EstadoDetalleTraslado.NO_DEPURADO;
                        cmd.Parameters["@UsuarioIng"].Value = usuarioIng;
                        cmd.Parameters["@FechaIng"].Value = DateTime.Now;
                        cmd.Parameters["@UsuarioAct"].Value = DBNull.Value;
                        cmd.Parameters["@FechaAct"].Value = DBNull.Value;
                        cmd.ExecuteNonQuery();
                        contador++;
                    }

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
