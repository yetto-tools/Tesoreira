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
    public class TipoNotificacionDAL: CadenaConexion
    {
        public List<TipoNotificacionCLS> GetAllTipoNotificacion()
        {
            List<TipoNotificacionCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT codigo_tipo_notificacion, 
	                       nombre 
                    FROM db_admon.tipo_notificacion
                    WHERE estado = @CodigoEstado
                    ORDER BY nombre ASC";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.EstadoRegistro.ACTIVO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            TipoNotificacionCLS objTipoNotiticacion;
                            lista = new List<TipoNotificacionCLS>();
                            int postCodigoTipoNotificacion = dr.GetOrdinal("codigo_tipo_notificacion");
                            int postTipoNotificacion = dr.GetOrdinal("nombre");
                            while (dr.Read())
                            {
                                objTipoNotiticacion = new TipoNotificacionCLS();

                                objTipoNotiticacion.CodigoTipoNotificacion = dr.GetInt16(postCodigoTipoNotificacion);
                                objTipoNotiticacion.Nombre = dr.GetString(postTipoNotificacion);
                                lista.Add(objTipoNotiticacion);
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


    }
}
