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
    public class RutaDAL: CadenaConexion
    {

        public string GuardarRuta(RutaCLS objRuta, string usuarioIng)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaVentas))
            {
                try
                {
                    string sql = @"
                    INSERT INTO db_ventas.ruta(ruta,nombre,nombre_corto,descripcion,codigo_tipo_ruta,codigo_canal_venta,migracion_completa,estado,usuario_ing,fecha_ing,usuario_act,fecha_act)
                    VALUES(@Ruta,@Nombre,@NombreCorto,@Descripcion,@CodigoTipoRuta,@CodigoCanalVenta,@MigracionCompleta,@CodigoEstado,@UsuarioIng,@FechaIng,@UsuarioAct,@FechaAct)";
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Ruta", objRuta.Ruta);
                        cmd.Parameters.AddWithValue("@Nombre", objRuta.Nombre == null ? DBNull.Value : objRuta.Nombre);
                        cmd.Parameters.AddWithValue("@NombreCorto", objRuta.NombreCorto == null ? DBNull.Value : objRuta.NombreCorto);
                        cmd.Parameters.AddWithValue("@Descripcion", objRuta.Descripcion == null ? DBNull.Value : objRuta.Descripcion);
                        cmd.Parameters.AddWithValue("@CodigoTipoRuta", objRuta.CodigoTipoRuta);
                        cmd.Parameters.AddWithValue("@CodigoCanalVenta", objRuta.CodigoCanalVenta);
                        cmd.Parameters.AddWithValue("@MigracionCompleta", objRuta.MigracionCompleta);
                        cmd.Parameters.AddWithValue("@CodigoEstado", objRuta.CodigoEstado);
                        cmd.Parameters.AddWithValue("@UsuarioIng", usuarioIng);
                        cmd.Parameters.AddWithValue("@FechaIng", DateTime.Now);
                        cmd.Parameters.AddWithValue("@UsuarioAct", DBNull.Value);
                        cmd.Parameters.AddWithValue("@FechaAct", DBNull.Value);
                        // rows sotred the number of rows affected
                        int rows = cmd.ExecuteNonQuery();
                    }

                    conexion.Close();
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
        public List<RutaCLS> GetListaRutas()
        {
            List<RutaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaVentas))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT x.ruta, 
	                       x.nombre,
	                       x.nombre_corto,
                           x.codigo_tipo_ruta, 
	                       CASE
		                     WHEN x.codigo_tipo_ruta = 1 THEN 'INTERNA'
		                     WHEN x.codigo_tipo_ruta = 2 THEN 'EXTERNA'
		                     ELSE 'NO DEFINIDA'
	                       END AS tipo_ruta,
	                       x.codigo_canal_venta,
	                       y.nombre AS canal_venta,
	                       x.migracion_completa,
                           x.descripcion,     
	                       CASE	
	                          WHEN x.migracion_completa = 1 THEN 'Ruta migrada completamente a PANISA'
		                      ELSE 'Migración incompleta a PANISA '
	                       END AS descripcion_migracion,
	                       x.estado AS codigo_estado,
	                       CASE 
		                      WHEN x.estado = 0 THEN 'BLOQUEADO'
		                      WHEN x.estado = 1 THEN 'ACTIVO'
		                   ELSE 'NO DEFINIDA'
	                       END AS estado,
                           1 AS permiso_anular,
                           1 AS permiso_editar
                    FROM db_ventas.ruta x
                    INNER JOIN db_ventas.canal_venta y
                    ON x.codigo_canal_venta = y.codigo_canal_venta
                    ORDER BY x.ruta";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            RutaCLS objRuta;
                            lista = new List<RutaCLS>();
                            int postRuta = dr.GetOrdinal("ruta");
                            int postNombre = dr.GetOrdinal("nombre");
                            int postNombreCorto = dr.GetOrdinal("nombre_corto");
                            int postDescripcion = dr.GetOrdinal("descripcion");
                            int postCodigoTipoRuta = dr.GetOrdinal("codigo_tipo_ruta");
                            int postTipoRuta = dr.GetOrdinal("tipo_ruta");
                            int postCodigoCanalVenta = dr.GetOrdinal("codigo_canal_venta");
                            int postCanalVenta = dr.GetOrdinal("canal_venta");
                            int postMigracionCompleta = dr.GetOrdinal("migracion_completa");
                            int postDescripcionMigracion = dr.GetOrdinal("descripcion_migracion");
                            int postCodigoEstado = dr.GetOrdinal("codigo_estado");
                            int postEstado = dr.GetOrdinal("estado");
                            int postPermisoAnular = dr.GetOrdinal("permiso_anular");
                            int postPermisoEditar = dr.GetOrdinal("permiso_editar");
                            while (dr.Read())
                            {
                                objRuta = new RutaCLS();
                                objRuta.Ruta = dr.GetInt16(postRuta);
                                objRuta.Nombre = dr.IsDBNull(postNombre) ? "" :  dr.GetString(postNombre);
                                objRuta.NombreCorto = dr.IsDBNull(postNombreCorto) ? "" : dr.GetString(postNombreCorto);
                                objRuta.Descripcion = dr.IsDBNull(postDescripcion) ? "" : dr.GetString(postDescripcion);
                                objRuta.CodigoTipoRuta = dr.GetByte(postCodigoTipoRuta);
                                objRuta.TipoRuta = dr.GetString(postTipoRuta);
                                objRuta.CodigoCanalVenta = dr.GetInt16(postCodigoCanalVenta);
                                objRuta.CanalVenta = dr.GetString(postCanalVenta);
                                objRuta.MigracionCompleta = dr.GetByte(postMigracionCompleta);
                                objRuta.DescripcionMigracion = dr.GetString(postDescripcionMigracion);
                                objRuta.CodigoEstado = dr.GetByte(postCodigoEstado);
                                objRuta.Estado = dr.GetString(postEstado);
                                objRuta.PermisoAnular = dr.GetInt32(postPermisoAnular);
                                objRuta.PermisoEditar = dr.GetInt32(postPermisoEditar);
                                lista.Add(objRuta);
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

        public List<RutaCLS> GetRutas()
        {
            List<RutaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaVentas))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT ruta 
                    FROM db_ventas.ruta
                    WHERE estado <> @CodigoEstadoAnulado
                    ORDER BY ruta";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoAnulado", Constantes.EstadoRegistro.BLOQUEADO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            RutaCLS objRuta;
                            lista = new List<RutaCLS>();
                            int postRuta = dr.GetOrdinal("ruta");
                            while (dr.Read())
                            {
                                objRuta = new RutaCLS();
                                objRuta.Ruta = dr.GetInt16(postRuta);
                                lista.Add(objRuta);
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


        public string ActualizarRuta(RutaCLS objRuta, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaVentas))
            {
                try
                {
                    string sql = @"
                    UPDATE db_ventas.ruta
                    SET nombre = @Nombre,
                        nombre_corto = @NombreCorto,
                        descripcion = @Descripcion,
                        codigo_tipo_ruta = @CodigoTipoRuta,
                        codigo_canal_venta = @CodigoCanalVenta,
                        migracion_completa = @MigracionCompleta,
                        estado = @CodigoEstado,
                        usuario_act = @UsuarioAct,
                        fecha_act = @FechaAct
                    WHERE  ruta = @Ruta";
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Ruta", objRuta.Ruta);
                        cmd.Parameters.AddWithValue("@Nombre", objRuta.Nombre == null ? DBNull.Value : objRuta.Nombre);
                        cmd.Parameters.AddWithValue("@NombreCorto", objRuta.NombreCorto == null ? DBNull.Value : objRuta.NombreCorto);
                        cmd.Parameters.AddWithValue("@Descripcion", objRuta.Descripcion == null ? DBNull.Value : objRuta.Descripcion);
                        cmd.Parameters.AddWithValue("@CodigoTipoRuta", objRuta.CodigoTipoRuta);
                        cmd.Parameters.AddWithValue("@CodigoCanalVenta", objRuta.CodigoCanalVenta);
                        cmd.Parameters.AddWithValue("@MigracionCompleta", objRuta.MigracionCompleta);
                        cmd.Parameters.AddWithValue("@CodigoEstado", objRuta.CodigoEstado);
                        cmd.Parameters.AddWithValue("@UsuarioAct", usuarioAct);
                        cmd.Parameters.AddWithValue("@FechaAct", DateTime.Now);
                        // rows sotred the number of rows affected
                        int rows = cmd.ExecuteNonQuery();
                    }

                    conexion.Close();
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
