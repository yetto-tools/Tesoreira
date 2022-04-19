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
    public class RolDAL: CadenaConexion
    {
        public List<RolCLS> GetAllRoles()
        {
            List<RolCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT codigo_rol,
                           nombre, 
	                       descripcion,
                           1 AS permiso_anular,
                           1 AS permiso_editar 
                    FROM db_admon.rol
                    WHERE estado = @CodigoEstado";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.EstadoRegistro.ACTIVO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            RolCLS objRol;
                            lista = new List<RolCLS>();
                            int postCodigoRol = dr.GetOrdinal("codigo_rol");
                            int postNombre = dr.GetOrdinal("nombre");
                            int postDescripcion = dr.GetOrdinal("descripcion");
                            int postPermisoAnular = dr.GetOrdinal("permiso_anular");
                            int postPermisoEditar = dr.GetOrdinal("permiso_editar");
                            while (dr.Read())
                            {
                                objRol = new RolCLS();
                                objRol.CodigoRol = dr.GetInt32(postCodigoRol);
                                objRol.Nombre = dr.GetString(postNombre);
                                objRol.Descripcion = dr.GetString(postNombre);
                                objRol.PermisoAnular = dr.GetInt32(postPermisoAnular);
                                objRol.PermisoEditar = dr.GetInt32(postPermisoEditar);
                                lista.Add(objRol);
                            }// fin while
                        }// fin if
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

        public RolCLS GetDataRol(int codigoRol)
        {
            RolCLS objRol = null;
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    conexion.Open();
                    string sqlRol = @"
                    SELECT codigo_rol,
                           nombre, 
	                       descripcion
                    FROM db_admon.rol
                    WHERE codigo_rol = @CodigoRol";

                    using (SqlCommand cmd = new SqlCommand(sqlRol, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoRol", codigoRol);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            int postCodigoRol = dr.GetOrdinal("codigo_rol");
                            int postNombre = dr.GetOrdinal("nombre");
                            int postDescripcion = dr.GetOrdinal("descripcion");
                            while (dr.Read())
                            {
                                objRol = new RolCLS();
                                objRol.CodigoRol = dr.GetInt32(postCodigoRol);
                                objRol.Nombre = dr.GetString(postNombre);
                                objRol.Descripcion = dr.IsDBNull(postDescripcion) ? "" : dr.GetString(postDescripcion);
                            }// fin while
                        }// fin if
                    }
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                    objRol = null;
                }

                return objRol;
            }
        }

        public List<SiteMapCLS> GetConfiguracionRol(int codigoRol)
        {
            List<SiteMapCLS> list = null;
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    conexion.Open();
                    string sqlRol = @"
                     SELECT distinct 
	                        codigo_sitemap
                    FROM db_admon.rol_sitemap
                    WHERE codigo_rol = @CodigoRol";

                    using (SqlCommand cmd = new SqlCommand(sqlRol, conexion))
                    {
                        list = new List<SiteMapCLS>();
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoRol", codigoRol);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            SiteMapCLS objSiteMap;
                            int postCodigoSiteMap = dr.GetOrdinal("codigo_sitemap");
                            while (dr.Read())
                            {
                                objSiteMap = new SiteMapCLS();
                                objSiteMap.CodigoSitemap = dr.GetInt32(postCodigoSiteMap);
                                list.Add(objSiteMap);
                            }// fin while
                        }// fin if
                    }
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                    list = null;
                }

                return list;
            }
        }
    }
}
