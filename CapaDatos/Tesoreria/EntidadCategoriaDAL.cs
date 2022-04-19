using CapaEntidad.Tesoreria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace CapaDatos.Tesoreria
{
    public class EntidadCategoriaDAL:CadenaConexion
    {
        public List<EntidadCategoriaCLS> GetAllCategoriaEntidades()
        {
            List<EntidadCategoriaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    select codigo_categoria_entidad, 
                           nombre, 
                           descripcion, 
                           estado 
                    FROM db_tesoreria.entidad_categoria
                    WHERE estado = @CodigoEstadoActivo
                    ORDER BY nombre ASC";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoActivo", Constantes.EstadoRegistro.ACTIVO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            EntidadCategoriaCLS objTipoOperacion;
                            lista = new List<EntidadCategoriaCLS>();
                            int postCodigoCategoriaEntidad = dr.GetOrdinal("codigo_categoria_entidad");
                            int postNombre = dr.GetOrdinal("nombre");
                            int postDescripcion = dr.GetOrdinal("descripcion");
                            int postEstado = dr.GetOrdinal("estado");

                            while (dr.Read())
                            {
                                objTipoOperacion = new EntidadCategoriaCLS();
                                objTipoOperacion.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objTipoOperacion.NombreCategoriaEntidad = dr.GetString(postNombre);
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

        public List<EntidadCategoriaCLS> GetCategoriasParaRegistrarEntidad()
        {
            List<EntidadCategoriaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    SELECT codigo_categoria_entidad, 
                           nombre
                    FROM db_tesoreria.entidad_categoria
                    WHERE excluir_registro_entidad = 0
                    ORDER BY nombre ASC";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            EntidadCategoriaCLS objTipoOperacion;
                            lista = new List<EntidadCategoriaCLS>();
                            int postCodigoCategoriaEntidad = dr.GetOrdinal("codigo_categoria_entidad");
                            int postNombre = dr.GetOrdinal("nombre");
                            
                            while (dr.Read())
                            {
                                objTipoOperacion = new EntidadCategoriaCLS();
                                objTipoOperacion.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objTipoOperacion.NombreCategoriaEntidad = dr.GetString(postNombre);
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

        public List<EntidadCategoriaCLS> GetCategoriaParaAsignacionDeOperacion()
        {
            List<EntidadCategoriaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    select codigo_categoria_entidad, 
                           nombre, 
                           descripcion, 
                           estado 
                    FROM db_tesoreria.entidad_categoria
                    WHERE estado = @CodigoEstadoActivo
                      AND incluir_config_operacion = 1
                    ORDER BY nombre ASC";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoActivo", Constantes.EstadoRegistro.ACTIVO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            EntidadCategoriaCLS objTipoOperacion;
                            lista = new List<EntidadCategoriaCLS>();
                            int postCodigoCategoriaEntidad = dr.GetOrdinal("codigo_categoria_entidad");
                            int postNombre = dr.GetOrdinal("nombre");
                            int postDescripcion = dr.GetOrdinal("descripcion");
                            int postEstado = dr.GetOrdinal("estado");

                            while (dr.Read())
                            {
                                objTipoOperacion = new EntidadCategoriaCLS();
                                objTipoOperacion.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objTipoOperacion.NombreCategoriaEntidad = dr.GetString(postNombre);
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

        public List<EntidadCategoriaCLS> filtrarCategoriaEntidades(string nombreCategoria)
        {
            List<EntidadCategoriaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string filterNombreCategoria = "";
                    if (!string.IsNullOrEmpty(nombreCategoria))
                    {
                        if (nombreCategoria != "" && nombreCategoria.Length != 0)
                            filterNombreCategoria = "where nombre like '%" + nombreCategoria + "%'";
                    }

                    string sql = @"
                    select codigo_categoria_entidad, 
                           nombre, 
                           descripcion, 
                           estado 
                    from db_tesoreria.entidad_categoria
                    " + filterNombreCategoria;

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            EntidadCategoriaCLS objTipoOperacion;
                            lista = new List<EntidadCategoriaCLS>();
                            int postCodigoCategoriaEntidad = dr.GetOrdinal("codigo_categoria_entidad");
                            int postNombre = dr.GetOrdinal("nombre");
                            int postDescripcion = dr.GetOrdinal("descripcion");
                            int postEstado = dr.GetOrdinal("estado");

                            while (dr.Read())
                            {
                                objTipoOperacion = new EntidadCategoriaCLS();
                                objTipoOperacion.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objTipoOperacion.NombreCategoriaEntidad = dr.GetString(postNombre);
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
