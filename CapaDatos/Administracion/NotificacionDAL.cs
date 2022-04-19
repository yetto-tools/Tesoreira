using CapaEntidad.Administracion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Administracion
{
    public class NotificacionDAL: CadenaConexion
    {
        public string GuardarConfiguracion(NotificacionCLS objNotificacion, string usuarioIng)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                conexion.Open();
                SqlCommand cmd = conexion.CreateCommand();

                try
                {
                    string sentenciaSQL = @"
                    INSERT INTO persona_notificacion(cui,codigo_tipo_notificacion,estado,usuario_ing,fecha_ing)
                    VALUES(@Cui,@CodigoTipoNotificacion,@CodigoEstado,@UsuarioIng,@FechaIng)";

                    cmd.CommandText = sentenciaSQL;

                    cmd.Parameters.AddWithValue("@Cui", objNotificacion.Cui);
                    cmd.Parameters.AddWithValue("@CodigoTipoNotificacion", objNotificacion.CodigoTipoNotificacion);
                    cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.EstadoRegistro.ACTIVO);
                    cmd.Parameters.AddWithValue("@UsuarioIng", usuarioIng);
                    cmd.Parameters.AddWithValue("@FechaIng", DateTime.Now);
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

        public List<NotificacionCLS> GetAllConfiguraciones()
        {
            List<NotificacionCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT x.cui, 
	                       y.nombre_completo, 
	                       y.correo_electronico,
	                       x.codigo_tipo_notificacion, 
	                       z.nombre AS tipo_notificacion,
                           x.estado AS codigo_estado,
                           CASE  
                             WHEN x.estado = 0 THEN 'BLOQUEADO'
                             WHEN x.estado = 1 THEN 'ACTIVO'
                             ELSE 'NO DEFINIDO'
                           END AS estado, 
                           1 AS permiso_anular 
                    FROM db_admon.persona_notificacion x
                    INNER JOIN db_rrhh.persona y
                    ON x.cui = y.cui
                    INNER JOIN db_admon.tipo_notificacion z
                    ON x.codigo_tipo_notificacion = z.codigo_tipo_notificacion";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            NotificacionCLS objNotificacion;
                            lista = new List<NotificacionCLS>();
                            int postCui = dr.GetOrdinal("cui");
                            int postNombreCompleto = dr.GetOrdinal("nombre_completo");
                            int postCorreoElectronico = dr.GetOrdinal("correo_electronico");
                            int postCodigoTipoNotificacion = dr.GetOrdinal("codigo_tipo_notificacion");
                            int postTipoNotificacion = dr.GetOrdinal("tipo_notificacion");
                            int postCodigoEstado = dr.GetOrdinal("codigo_estado");
                            int postEstado = dr.GetOrdinal("estado");
                            int postPermisoAnular = dr.GetOrdinal("permiso_anular");
                            while (dr.Read())
                            {
                                objNotificacion = new NotificacionCLS();
                                objNotificacion.Cui = dr.GetString(postCui);
                                objNotificacion.NombreCompleto = dr.GetString(postNombreCompleto);
                                objNotificacion.CorreoElectronico = dr.GetString(postCorreoElectronico);
                                objNotificacion.CodigoTipoNotificacion = dr.GetInt16(postCodigoTipoNotificacion);
                                objNotificacion.TipoNotificacion = dr.GetString(postTipoNotificacion);
                                objNotificacion.CodigoEstado = dr.GetByte(postCodigoEstado);
                                objNotificacion.Estado = dr.GetString(postEstado);
                                objNotificacion.PermisoAnular = (byte)dr.GetInt32(postPermisoAnular);
                                lista.Add(objNotificacion);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    conexion.Close();
                    lista = null;
                }

                return lista;
            }
        }

        public string EliminarConfiguracion(string cui, int codigoTipoNotificacion)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                string sentenciaSQL = @"
                DELETE FROM db_admon.persona_notificacion
                WHERE cui = @Cui
                  AND codigo_tipo_notificacion = @CodigoTipoNotificacion";

                conexion.Open();
                try
                {
                    SqlCommand cmd = conexion.CreateCommand();
                    cmd.CommandText = sentenciaSQL;
                    cmd.Parameters.AddWithValue("@Cui", cui);
                    cmd.Parameters.AddWithValue("@CodigoTipoNotificacion", codigoTipoNotificacion);
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



    }
}
