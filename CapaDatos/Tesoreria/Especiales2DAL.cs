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
                                ratioAnterior = 100;
                                foreach (ClienteCLS clienteEspecial2 in listaClientesEspeciales2)
                                {
                                    ratio = Util.Manipulacion.Compute(clienteEspecial2.NombreCompleto, objDetalle.NombreCliente);
                                    if (ratio < ratioAnterior)
                                    {
                                        objDetalle.CodigoEntidad = clienteEspecial2.CodigoCliente;
                                        objDetalle.NombreEntidad = clienteEspecial2.NombreCompleto;
                                        ratioAnterior = ratio;
                                    }
                                }
                                objDetalle.RatioSimilitud = ratioAnterior;
                                objDetalle.Monto = dr.GetDecimal(postMonto);
                                objDetalle.modificacion = dr.GetByte(postModificacion);
                                objDetalle.CodigoEstadoDepuracion = (byte)dr.GetInt32(postCodigoEstadoDepuracion);

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





    }
}
