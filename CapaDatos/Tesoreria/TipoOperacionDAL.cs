using CapaEntidad.Tesoreria;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Tesoreria
{
    public class TipoOperacionDAL: CadenaConexion
    {
        public List<TipoOperacionCLS> listarTiposOperacion()
        {
            List<TipoOperacionCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("select codigo_tipo_operacion, nombre, id, signo, descripcion, estado from db_tesoreria.tipo_operacion", conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            TipoOperacionCLS objTipoOperacion;
                            lista = new List<TipoOperacionCLS>();
                            int postCodigoTipoOperacion = dr.GetOrdinal("codigo_tipo_operacion");
                            int postNombre = dr.GetOrdinal("nombre");
                            int postId = dr.GetOrdinal("id");
                            int postSigno = dr.GetOrdinal("signo");
                            int postDescripcion = dr.GetOrdinal("descripcion");
                            int postEstado = dr.GetOrdinal("estado");

                            while (dr.Read())
                            {
                                objTipoOperacion = new TipoOperacionCLS();
                                objTipoOperacion.CodigoTipoOperacion = dr.GetInt16(postCodigoTipoOperacion);
                                objTipoOperacion.Nombre = dr.GetString(postNombre);
                                objTipoOperacion.IdTipoOperacion = dr.GetString(postId);
                                objTipoOperacion.Signo = dr.GetInt16(postSigno);
                                objTipoOperacion.Descripcion = dr.IsDBNull(postDescripcion) ? "" : dr.GetString(postDescripcion);
                                objTipoOperacion.Estado = dr.GetByte(postEstado);
                                lista.Add(objTipoOperacion);
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

    }
}
