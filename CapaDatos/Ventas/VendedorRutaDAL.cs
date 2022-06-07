using CapaEntidad.Ventas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Ventas
{
    public class VendedorRutaDAL: CadenaConexion
    {
        public List<VendedorRutaCLS> GetListaVendedores()
        {
            List<VendedorRutaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaVentas))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT x.codigo_vendedor, 
	                       y.nombre_completo,
	                       x.codigo_canal_venta,
	                       z.nombre as canal_venta,
	                       x.ruta,
                           z.codigo_categoria_entidad 
                    FROM db_ventas.config_vendedor_ruta x
                    INNER JOIN db_ventas.vendedor y
                    ON x.codigo_vendedor = y.codigo_vendedor
                    INNER JOIN db_ventas.canal_venta z
                    ON x.codigo_canal_venta = z.codigo_canal_venta
                    WHERE x.estado = @EstadoVendedorRuta
                    ORDER BY z.nombre";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@EstadoVendedorRuta", Constantes.EstadoRegistro.ACTIVO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            VendedorRutaCLS objVendedor;
                            lista = new List<VendedorRutaCLS>();
                            int postCodigoVendedor = dr.GetOrdinal("codigo_vendedor");
                            int postNombreVendedor = dr.GetOrdinal("nombre_completo");
                            int postCodigoCanalVenta = dr.GetOrdinal("codigo_canal_venta");
                            int postCanalVenta = dr.GetOrdinal("canal_venta");
                            int postRuta = dr.GetOrdinal("ruta");
                            int postCodigoCategoriaEntidad = dr.GetOrdinal("codigo_categoria_entidad");

                            while (dr.Read())
                            {
                                objVendedor = new VendedorRutaCLS();
                                objVendedor.CodigoVendedor = dr.GetString(postCodigoVendedor);
                                objVendedor.NombreVendedor = dr.GetString(postNombreVendedor);
                                objVendedor.CodigoCanalVenta = dr.GetInt16(postCodigoCanalVenta);
                                objVendedor.CanalVenta = dr.GetString(postCanalVenta);
                                objVendedor.Ruta = dr.GetInt16(postRuta);
                                objVendedor.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);

                                lista.Add(objVendedor);
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

        public List<VendedorRutaCLS> GetListaVendedores(int codigoCanalVenta)
        {
            List<VendedorRutaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaVentas))
            {
                try
                {
                    string filterCanalVenta = String.Empty;
                    if (codigoCanalVenta != -1)
                    {
                        filterCanalVenta = "AND x.codigo_canal_venta = " + codigoCanalVenta.ToString();
                    }

                    string sql = @"
                    SELECT x.codigo_configuracion,
                           x.codigo_vendedor, 
                           y.nombre_completo,
                           x.codigo_canal_venta,
                           z.nombre as canal_venta,
                           x.ruta,
                           x.estado AS codigo_estado_ruta_vendedor,
                           CASE  
                             WHEN x.estado = 0 THEN 'BLOQUEADO'
                             WHEN x.estado = 1 THEN 'ACTIVO'
                             ELSE 'NO DEFINIDO'
                           END AS estado_ruta_vendedor,
                           1 AS permiso_anular, 
                           1 AS permiso_editar
                    FROM db_ventas.config_vendedor_ruta x
                    INNER JOIN db_ventas.vendedor y
                    ON x.codigo_vendedor = y.codigo_vendedor
                    INNER JOIN db_ventas.canal_venta z
                    ON x.codigo_canal_venta = z.codigo_canal_venta
                    WHERE x.estado = 1
                    " + filterCanalVenta + @"
                    ORDER BY z.nombre";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            VendedorRutaCLS objVendedor;
                            lista = new List<VendedorRutaCLS>();
                            int postCodigoConfiguracion = dr.GetOrdinal("codigo_configuracion");
                            int postCodigoVendedor = dr.GetOrdinal("codigo_vendedor");
                            int postNombreVendedor = dr.GetOrdinal("nombre_completo");
                            int postCodigoCanalVenta = dr.GetOrdinal("codigo_canal_venta");
                            int postCanalVenta = dr.GetOrdinal("canal_venta");
                            int postRuta = dr.GetOrdinal("ruta");
                            int postCodigoEstadoRutaVendedor = dr.GetOrdinal("codigo_estado_ruta_vendedor");
                            int postEstadoRutaVendedor = dr.GetOrdinal("estado_ruta_vendedor");
                            int postPermisoAnular = dr.GetOrdinal("permiso_anular");
                            int postPermisoEditar = dr.GetOrdinal("permiso_editar");

                            while (dr.Read())
                            {
                                objVendedor = new VendedorRutaCLS();
                                objVendedor.CodigoConfiguracion = dr.GetInt32(postCodigoConfiguracion);
                                objVendedor.CodigoVendedor = dr.GetString(postCodigoVendedor);
                                objVendedor.NombreVendedor = dr.GetString(postNombreVendedor);
                                objVendedor.CodigoCanalVenta = dr.GetInt16(postCodigoCanalVenta);
                                objVendedor.CanalVenta = dr.GetString(postCanalVenta);
                                objVendedor.Ruta = dr.GetInt16(postRuta);
                                objVendedor.CodigoEstadoRutaVendedor = dr.GetByte(postCodigoEstadoRutaVendedor);
                                objVendedor.EstadoRutaVendedor = dr.GetString(postEstadoRutaVendedor);
                                objVendedor.PermisoAnular = (byte)dr.GetInt32(postPermisoAnular);
                                objVendedor.PermisoEditar = (byte)dr.GetInt32(postPermisoEditar);
                                lista.Add(objVendedor);
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

        public string AnularConfiguracionVendedorRuta(int codigoConfiguracion, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaVentas))
            {
                try
                {
                    string sql = @"
                    UPDATE db_ventas.config_vendedor_ruta
                    SET estado = @CodigoEstadoBloqueado,
                        usuario_act = @UsuarioAct,
                        fecha_act = @FechaAct
                    WHERE codigo_configuracion = @CodigoConfiguracion";
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoBloqueado", Constantes.EstadoRegistro.BLOQUEADO);
                        cmd.Parameters.AddWithValue("@CodigoConfiguracion", codigoConfiguracion);
                        cmd.Parameters.AddWithValue("@UsuarioAct", usuarioAct);
                        cmd.Parameters.AddWithValue("@FechaAct", DateTime.Now);

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


        //public string BloquearVendedorRuta(string codigoVendedor, int codigoCanalVenta, int ruta)
        //{
        //    string resultado = "";
        //    using (SqlConnection conexion = new SqlConnection(cadenaVentas))
        //    {
        //        try
        //        {
        //            string sql = @"
        //            UPDATE db_ventas.vendedor_ruta
        //            SET estado = @CodigoEstadoBloqueado
        //            WHERE codigo_vendedor = @CodigoVendedor AND 
        //                  codigo_canal_venta = @CodigoCanalVenta AND 
        //                  ruta = @Ruta";
        //            conexion.Open();
        //            using (SqlCommand cmd = new SqlCommand(sql, conexion))
        //            {
        //                cmd.CommandType = CommandType.Text;
        //                cmd.Parameters.AddWithValue("@CodigoEstadoBloqueado", Constantes.EstadoRegistro.BLOQUEADO);
        //                cmd.Parameters.AddWithValue("@CodigoVendedor", codigoVendedor);
        //                cmd.Parameters.AddWithValue("@CodigoCanalVenta", codigoCanalVenta);
        //                cmd.Parameters.AddWithValue("@Ruta", ruta);

        //                // rows sotred the number of rows affected
        //                int rows = cmd.ExecuteNonQuery();
        //                conexion.Close();
        //            }
        //            resultado = "OK";
        //        }
        //        catch (Exception ex)
        //        {
        //            resultado = "Error [0]: " + ex.Message;
        //            conexion.Close();
        //        }

        //        return resultado;
        //    }
        //}

        //public string DesbloquearVendedorRuta(string codigoVendedor, int codigoCanalVenta, int ruta)
        //{
        //    string resultado = "";
        //    using (SqlConnection conexion = new SqlConnection(cadenaVentas))
        //    {
        //        try
        //        {
        //            string sql = @"
        //            UPDATE db_ventas.vendedor_ruta
        //            SET estado = @CodigoEstadoDesbloqueado
        //            WHERE codigo_vendedor = @CodigoVendedor AND 
        //                  codigo_canal_venta = @CodigoCanalVenta AND 
        //                  ruta = @Ruta";
        //            conexion.Open();
        //            using (SqlCommand cmd = new SqlCommand(sql, conexion))
        //            {
        //                cmd.CommandType = CommandType.Text;
        //                cmd.Parameters.AddWithValue("@CodigoEstadoDesbloqueado", Constantes.EstadoRegistro.ACTIVO);
        //                cmd.Parameters.AddWithValue("@CodigoVendedor", codigoVendedor);
        //                cmd.Parameters.AddWithValue("@CodigoCanalVenta", codigoCanalVenta);
        //                cmd.Parameters.AddWithValue("@Ruta", ruta);

        //                // rows sotred the number of rows affected
        //                int rows = cmd.ExecuteNonQuery();
        //                conexion.Close();
        //            }
        //            resultado = "OK";
        //        }
        //        catch (Exception ex)
        //        {
        //            resultado = "Error [0]: " + ex.Message;
        //            conexion.Close();
        //        }

        //        return resultado;
        //    }
        //}

        public string GuardarVendedorRuta(VendedorRutaCLS objVendedorRuta, string usuarioIng)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaVentas))
            {
                conexion.Open();

                SqlCommand cmd = conexion.CreateCommand();

                try
                {
                    string sentenciaSQL = @"
                    INSERT INTO db_ventas.config_vendedor_ruta(codigo_configuracion, codigo_vendedor, codigo_canal_venta, ruta, descripcion, estado, usuario_ing, fecha_ing, usuario_act, fecha_act)
                    VALUES(NEXT VALUE FOR db_ventas.SQ_CONFIG_VENDEDOR_RUTA, @CodigoVendedor,@CodigoCanalVenta,@Ruta,@Descripcion,@CodigoEstado,@UsuarioIng,@FechaIng,@UsuarioAct,@FechaAct)";

                    cmd.CommandText = sentenciaSQL;
                    cmd.Parameters.AddWithValue("@CodigoVendedor", objVendedorRuta.CodigoVendedor);
                    cmd.Parameters.AddWithValue("@CodigoCanalVenta", objVendedorRuta.CodigoCanalVenta);
                    cmd.Parameters.AddWithValue("@Ruta", objVendedorRuta.Ruta);
                    cmd.Parameters.AddWithValue("@Descripcion", DBNull.Value);
                    cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.EstadoRegistro.ACTIVO);
                    cmd.Parameters.AddWithValue("@UsuarioIng", usuarioIng);
                    cmd.Parameters.AddWithValue("@FechaIng", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UsuarioAct", DBNull.Value);
                    cmd.Parameters.AddWithValue("@FechaAct", DBNull.Value);
                    cmd.ExecuteNonQuery();

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

        public string ActualizarConfiguracionVendedorRuta(VendedorRutaCLS objVendedorRuta, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaVentas))
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
                    string sentenciaUpdateConfiguracion = @"
                    UPDATE db_ventas.config_vendedor_ruta
                    SET estado = @CodigoEstadoBloqueado,
                        usuario_act = @UsuarioAct,
                        fecha_act = @FechaAct
                    WHERE codigo_configuracion = @CodigoConfiguracion";

                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sentenciaUpdateConfiguracion;
                    cmd.Parameters.AddWithValue("@CodigoEstadoBloqueado", Constantes.EstadoRegistro.BLOQUEADO);
                    cmd.Parameters.AddWithValue("@CodigoConfiguracion", objVendedorRuta.CodigoConfiguracion);
                    cmd.Parameters.AddWithValue("@UsuarioAct", usuarioAct);
                    cmd.Parameters.AddWithValue("@FechaAct", DateTime.Now);
                    // rows sotred the number of rows affected
                    int rowsUpdate = cmd.ExecuteNonQuery();


                    string sentenciaSQL = @"
                    INSERT INTO db_ventas.config_vendedor_ruta(codigo_configuracion, codigo_vendedor, codigo_canal_venta, ruta, descripcion, estado, usuario_ing, fecha_ing, usuario_act, fecha_act)
                    VALUES(NEXT VALUE FOR db_ventas.SQ_CONFIG_VENDEDOR_RUTA, @CodigoVendedor,@CodigoCanalVenta,@Ruta,@Descripcion,@CodigoEstado,@UsuarioIng,@FechaIng,NULL,NULL)";

                    cmd.CommandText = sentenciaSQL;
                    cmd.Parameters.AddWithValue("@CodigoVendedor", objVendedorRuta.CodigoVendedor);
                    cmd.Parameters.AddWithValue("@CodigoCanalVenta", objVendedorRuta.CodigoCanalVenta);
                    cmd.Parameters.AddWithValue("@Ruta", objVendedorRuta.Ruta);
                    cmd.Parameters.AddWithValue("@Descripcion", objVendedorRuta.Descripcion == null ? DBNull.Value : objVendedorRuta.Descripcion);
                    cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.EstadoRegistro.ACTIVO);
                    cmd.Parameters.AddWithValue("@UsuarioIng", usuarioAct);
                    cmd.Parameters.AddWithValue("@FechaIng", DateTime.Now);
                    int rowInsert = cmd.ExecuteNonQuery();


                    if (rowsUpdate > 0 && rowInsert > 0)
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

        public List<VendedorRutaCLS> GetRutasDelVendedor(int codigoCategoriaEntidad, string codigoVendedor)
        {
            List<VendedorRutaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaVentas))
            {
                try
                {
                    string sql = @"
                    SELECT x.ruta
                    FROM db_ventas.config_vendedor_ruta x
                    INNER JOIN db_ventas.canal_venta y
                    ON x.codigo_canal_venta = y.codigo_canal_venta
                    WHERE x.estado = @CodigoEstadoConfiguracion
                      AND x.codigo_vendedor = @CodigoVendedor
                      AND y.codigo_categoria_entidad = @CodigoCategoriaEntidad";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoConfiguracion", Constantes.EstadoRegistro.ACTIVO);
                        cmd.Parameters.AddWithValue("@CodigoVendedor", codigoVendedor);
                        cmd.Parameters.AddWithValue("@CodigoCategoriaEntidad", codigoCategoriaEntidad);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            VendedorRutaCLS objVendedorRuta;
                            lista = new List<VendedorRutaCLS>();
                            int postRutaVendedor = dr.GetOrdinal("ruta");

                            while (dr.Read())
                            {
                                objVendedorRuta = new VendedorRutaCLS();
                                objVendedorRuta.Ruta = dr.GetInt16(postRutaVendedor);
                                lista.Add(objVendedorRuta);
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

        public int ExisteConfiguracionVendedorRuta(string codigoVendedor, int codigoCanalVenta, int ruta)
        {
            int  resultado = 1;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    SELECT count(*) AS contador
                    FROM db_ventas.config_vendedor_ruta 
                    WHERE codigo_vendedor = @CodigoVendedor
                      AND codigo_canal_venta = @CodigoCanalVenta
                      AND ruta = @Ruta
                      AND estado = @CodigoEstadoConfiguracion";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoVendedor", codigoVendedor);
                        cmd.Parameters.AddWithValue("@CodigoCanalVenta", codigoCanalVenta);
                        cmd.Parameters.AddWithValue("@Ruta", ruta);
                        cmd.Parameters.AddWithValue("@CodigoEstadoConfiguracion", Constantes.EstadoRegistro.ACTIVO);
                        resultado = (int)cmd.ExecuteScalar();
                    }
                    conexion.Close();
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    resultado = -1;
                }
            }
            return resultado;
        }
    }





}
