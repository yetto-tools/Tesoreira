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
    public class SiteMapDAL: CadenaConexion
    {
        public List<SiteMapCLS> GetMenus(string idUsuario, int esSuperAdmin)
        {
            List<SiteMapCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    string sql = String.Empty;
                    if (esSuperAdmin == 1)
                    {
                        sql = @"
                        SELECT n.codigo_sitemap, 
                               m.codigo_sistema,
                               m.titulo,
                               m.nombre_controller,
                               m.nombre_action,
                               m.nivel,
                               m.cantidad_items,
                               m.cantidad_subitems,
                               m.cantidad_opciones,
                               m.cantidad_subopciones,
                               m.codigo_sitemap_padre 
                        FROM db_admon.TreeSiteMap() AS n
                        LEFT JOIN ( SELECT x.codigo_sitemap,
                                           x.codigo_sistema,
                                           x.titulo,
                                           x.nombre_controller,
                                           x.nombre_action,
                                           x.nivel,
						                   y.cantidad_items,
						                   CASE WHEN r.cantidad_subitems IS NULL THEN 0 ELSE r.cantidad_subitems END AS cantidad_subitems,
						                   CASE
							                  WHEN z.cantidad_opciones IS NULL then 0
							                  ELSE z.cantidad_opciones
						                   END AS cantidad_opciones,
						                   CASE
							                 WHEN w.cantidad_subopciones IS NULL then 0
							                 ELSE w.cantidad_subopciones
						                   END AS cantidad_subopciones,
						                   x.codigo_sitemap_padre
                                    FROM db_admon.sitemap x
					                LEFT JOIN (SELECT codigo_sistema, nivel, count(*) AS cantidad_items FROM  db_admon.sitemap WHERE estado = 1 GROUP BY codigo_sistema, nivel) y
					                ON x.codigo_sistema = y.codigo_sistema AND x.nivel = y.nivel
					                LEFT JOIN (SELECT codigo_sistema, count(*) AS cantidad_opciones FROM db_admon.sitemap  WHERE nivel = 1 AND estado = 1 GROUP BY codigo_sistema) z
					                ON x.codigo_sistema = z.codigo_sistema AND x.nivel = 0
					                LEFT JOIN (SELECT codigo_sistema, nivel, codigo_sitemap_padre, count(*) AS cantidad_subitems FROM  db_admon.sitemap WHERE estado = 1 GROUP BY codigo_sistema, nivel, codigo_sitemap_padre) r
					                ON x.codigo_sistema = r.codigo_sistema AND x.nivel = r.nivel AND x.codigo_sitemap_padre = r.codigo_sitemap_padre
					                LEFT JOIN (SELECT codigo_sistema, codigo_sitemap_padre, count(*) AS cantidad_subopciones FROM db_admon.sitemap  WHERE nivel = 2 AND estado = 1 GROUP BY codigo_sistema, codigo_sitemap_padre) w
					                ON x.codigo_sistema = w.codigo_sistema AND x.codigo_sitemap = w.codigo_sitemap_padre AND x.nivel = 1
					                WHERE x.estado = 1
                                    AND x.codigo_sitemap <> 14
                                  ) m
                        ON n.codigo_sitemap = m.codigo_sitemap
                        WHERE n.codigo_sitemap <> 14
                        ORDER BY cast('/' + RN + '/' as hierarchyid)";
                    }
                    else {
                        sql = @"
                        SELECT n.codigo_sitemap, 
                               m.codigo_sistema,
                               m.titulo,
                               m.nombre_controller,
                               m.nombre_action,
                               m.nivel,
                               m.cantidad_items,
                               m.cantidad_subitems,
                               m.cantidad_opciones,
                               m.cantidad_subopciones,
                               m.codigo_sitemap_padre 
                        FROM db_admon.TreeSiteMap() AS n
                        LEFT JOIN ( SELECT x.codigo_sitemap,
                                           x.codigo_sistema,
                                           x.titulo,
                                           x.nombre_controller,
                                           x.nombre_action,
                                           x.nivel,
						                   y.cantidad_items,
						                   CASE WHEN r.cantidad_subitems IS NULL THEN 0 ELSE r.cantidad_subitems END AS cantidad_subitems,
						                   CASE
							                  WHEN z.cantidad_opciones IS NULL then 0
							                  ELSE z.cantidad_opciones
						                   END AS cantidad_opciones,
						                   CASE
							                 WHEN w.cantidad_subopciones IS NULL then 0
							                 ELSE w.cantidad_subopciones
						                   END AS cantidad_subopciones,
						                   x.codigo_sitemap_padre
                                    FROM db_admon.sitemap x
					                LEFT JOIN ( SELECT codigo_sistema, 
                                                       nivel, 
                                                       count(*) AS cantidad_items 
                                                FROM  db_admon.sitemap 
                                                WHERE estado = 1 AND 
                                                      codigo_sitemap in (SELECT distinct  b.codigo_sitemap FROM db_admon.usuario_rol a INNER JOIN db_admon.rol_sitemap b ON a.codigo_rol = b.codigo_rol WHERE a.id_usuario = @UsuarioLogin)
                                                GROUP BY codigo_sistema, nivel
                                              ) y
					                ON x.codigo_sistema = y.codigo_sistema AND x.nivel = y.nivel
					                LEFT JOIN ( SELECT codigo_sistema, 
                                                       count(*) AS cantidad_opciones 
                                                FROM db_admon.sitemap  
                                                WHERE nivel = 1 AND 
                                                      estado = 1 AND
                                                      codigo_sitemap in (SELECT distinct  b.codigo_sitemap FROM db_admon.usuario_rol a INNER JOIN db_admon.rol_sitemap b ON a.codigo_rol = b.codigo_rol WHERE a.id_usuario = @UsuarioLogin)
                                                GROUP BY codigo_sistema) z
					                ON x.codigo_sistema = z.codigo_sistema AND x.nivel = 0
					                LEFT JOIN ( SELECT codigo_sistema, 
                                                       nivel, 
                                                       codigo_sitemap_padre, 
                                                       count(*) AS cantidad_subitems 
                                                FROM  db_admon.sitemap 
                                                WHERE estado = 1 AND 
                                                      codigo_sitemap in (SELECT distinct  b.codigo_sitemap FROM db_admon.usuario_rol a INNER JOIN db_admon.rol_sitemap b ON a.codigo_rol = b.codigo_rol WHERE a.id_usuario = @UsuarioLogin)
                                                GROUP BY codigo_sistema, nivel, codigo_sitemap_padre
                                               ) r
					                ON x.codigo_sistema = r.codigo_sistema AND x.nivel = r.nivel AND x.codigo_sitemap_padre = r.codigo_sitemap_padre
					                LEFT JOIN ( SELECT codigo_sistema, 
                                                       codigo_sitemap_padre, 
                                                       count(*) AS cantidad_subopciones 
                                                FROM db_admon.sitemap  
                                                WHERE nivel = 2 AND 
                                                      estado = 1 AND
                                                      codigo_sitemap in (SELECT distinct  b.codigo_sitemap FROM db_admon.usuario_rol a INNER JOIN db_admon.rol_sitemap b ON a.codigo_rol = b.codigo_rol WHERE a.id_usuario = @UsuarioLogin)
                                                GROUP BY codigo_sistema, codigo_sitemap_padre
                                              ) w
					                ON x.codigo_sistema = w.codigo_sistema AND x.codigo_sitemap = w.codigo_sitemap_padre AND x.nivel = 1
					                WHERE x.estado = 1
                                    AND x.codigo_sitemap <> 14
                                  ) m
                        ON n.codigo_sitemap = m.codigo_sitemap
                        WHERE n.codigo_sitemap <> 14 AND
                              n.codigo_sitemap in (SELECT distinct  b.codigo_sitemap FROM db_admon.usuario_rol a INNER JOIN db_admon.rol_sitemap b ON a.codigo_rol = b.codigo_rol WHERE a.id_usuario = @UsuarioLogin)
                        ORDER BY cast('/' + RN + '/' as hierarchyid)";
                    }

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.EstadoRegistro.ACTIVO);
                        cmd.Parameters.AddWithValue("@UsuarioLogin", idUsuario);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            SiteMapCLS objSiteMap;
                            lista = new List<SiteMapCLS>();
                            int postCodigoSiteMap = dr.GetOrdinal("codigo_sitemap");
                            int postCodigoSistema = dr.GetOrdinal("codigo_sistema");
                            int postTitulo = dr.GetOrdinal("titulo");
                            int postNombreController = dr.GetOrdinal("nombre_controller");
                            int postNombreAction = dr.GetOrdinal("nombre_action");
                            int postNivel = dr.GetOrdinal("nivel");
                            int postCantidadItems = dr.GetOrdinal("cantidad_items");
                            int postCantidadSubItems = dr.GetOrdinal("cantidad_subitems");
                            int postCantidadOpciones = dr.GetOrdinal("cantidad_opciones");
                            int postCantidadSubOpciones = dr.GetOrdinal("cantidad_subopciones");

                            while (dr.Read())
                            {
                                objSiteMap = new SiteMapCLS();
                                objSiteMap.CodigoSitemap = dr.GetInt32(postCodigoSiteMap);
                                objSiteMap.CodigoSistema = dr.GetInt16(postCodigoSistema);
                                objSiteMap.Titulo = dr.GetString(postTitulo);
                                objSiteMap.NombreController = dr.GetString(postNombreController);
                                objSiteMap.NombreAction = dr.GetString(postNombreAction);
                                objSiteMap.Nivel = dr.GetByte(postNivel);
                                objSiteMap.CantidadItems = dr.GetInt32(postCantidadItems);
                                objSiteMap.CantidadSubItems = dr.GetInt32(postCantidadSubItems);
                                objSiteMap.CantidadOpciones = dr.GetInt32(postCantidadOpciones);
                                objSiteMap.CantidadSubOpciones = dr.GetInt32(postCantidadSubOpciones);
                                lista.Add(objSiteMap);
                            }
                        }
                    }
                    conexion.Close();
                }
                catch (Exception)
                {
                    lista = null;
                    conexion.Close();
                }

                return lista;
            }
        }

        public List<SiteMapCLS> GetConfiguracion(int codigoSistema, int nivel)
        {
            List<SiteMapCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    string filterSistema = String.Empty;
                    string filterNivel = String.Empty;

                    if (codigoSistema != -1) {
                        filterSistema = "AND x.codigo_sistema = " + codigoSistema.ToString();
                    }
                    if (nivel != -1) {
                        filterNivel = "AND x.nivel = " + nivel.ToString();
                    }
                    string sql = @"
                    SELECT x.codigo_sitemap, 
                           x.codigo_sistema,
	                       y.nombre AS sistema,
	                       x.titulo,
	                       x.descripcion,
	                       x.nombre_controller,
	                       x.nombre_action,
	                       x.nivel,
	                       CASE
	                          WHEN x.nivel = 0 THEN 'Módulo'
		                      WHEN x.nivel = 1 THEN 'Opción'
		                      WHEN x.nivel = 2 THEN 'Subopcion'
		                      ELSE 'No definido'
	                       END AS nombre_nivel,
                           x.codigo_sitemap_padre,
	                       z.codigo_sistema AS codigo_sistema_padre,
	                       z.titulo AS titulo_padre,
                           1 AS permiso_anular,
                           1 AS permiso_editar 
                    FROM db_admon.sitemap x
                    INNER JOIN db_admon.sistema y
                    ON x.codigo_sistema = y.codigo_sistema
                    LEFT JOIN db_admon.sitemap z
                    ON x.codigo_sitemap_padre = z.codigo_sitemap
                    WHERE x.estado <> @CodigoEstadoAnulado
                      AND x.codigo_sitemap_padre IS NOT NULL
                    " + filterSistema + @"
                    " + filterNivel;
                   
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoAnulado", Constantes.EstadoRegistro.BLOQUEADO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            SiteMapCLS objSiteMap;
                            lista = new List<SiteMapCLS>();
                            int postCodigoSiteMap = dr.GetOrdinal("codigo_sitemap");
                            int postCodigoSistema = dr.GetOrdinal("codigo_sistema");
                            int postNombreSistema = dr.GetOrdinal("sistema");
                            int postTitulo = dr.GetOrdinal("titulo");
                            int postDescripcion = dr.GetOrdinal("descripcion");
                            int postNombreController = dr.GetOrdinal("nombre_controller");
                            int postNombreAction = dr.GetOrdinal("nombre_action");
                            int postNivel = dr.GetOrdinal("nivel");
                            int postNombreNivel = dr.GetOrdinal("nombre_nivel");
                            int postTituloPadre = dr.GetOrdinal("titulo_padre");
                            int postPermisoAnular = dr.GetOrdinal("permiso_anular");
                            int postPermisoEditar = dr.GetOrdinal("permiso_editar");
                            while (dr.Read())
                            {
                                objSiteMap = new SiteMapCLS();
                                objSiteMap.CodigoSitemap = dr.GetInt32(postCodigoSiteMap);
                                objSiteMap.CodigoSistema = dr.GetInt16(postCodigoSistema);
                                objSiteMap.NombreSistema = dr.GetString(postNombreSistema);
                                objSiteMap.Titulo = dr.GetString(postTitulo);
                                objSiteMap.Descripcion = dr.IsDBNull(postDescripcion) ? "" : dr.GetString(postDescripcion);
                                objSiteMap.NombreController = dr.GetString(postNombreController);
                                objSiteMap.NombreAction = dr.GetString(postNombreAction);
                                objSiteMap.Nivel = dr.GetByte(postNivel);
                                objSiteMap.NombreNivel = dr.GetString(postNombreNivel);
                                objSiteMap.TituloPadre = dr.IsDBNull(postTituloPadre) ? "" : dr.GetString(postTituloPadre);
                                objSiteMap.PermisoAnular = dr.GetInt32(postPermisoAnular);
                                objSiteMap.PermisoEditar = dr.GetInt32(postPermisoEditar);
                                lista.Add(objSiteMap);
                            }
                        }
                    }
                    conexion.Close();
                }
                catch (Exception)
                {
                    lista = null;
                    conexion.Close();
                }

                return lista;
            }

        }

        public SiteMapCLS GetDataItemMenu(int codigoSiteMap)
        {
            SiteMapCLS objItemMenu = null;
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    string sql = @"
                    SELECT x.codigo_sitemap, 
                           x.codigo_sistema,
	                       y.nombre AS sistema,
	                       x.titulo,
	                       x.descripcion,
	                       x.nombre_controller,
	                       x.nombre_action,
	                       x.nivel,
	                       CASE
	                          WHEN x.nivel = 0 THEN 'Módulo'
		                      WHEN x.nivel = 1 THEN 'Opción'
		                      WHEN x.nivel = 2 THEN 'Subopcion'
		                      ELSE 'No definido'
	                       END AS nombre_nivel,
                           x.codigo_sitemap_padre,
	                       z.codigo_sistema AS codigo_sistema_padre,
	                       z.titulo AS titulo_padre
                    FROM db_admon.sitemap x
                    INNER JOIN db_admon.sistema y
                    ON x.codigo_sistema = y.codigo_sistema
                    LEFT JOIN db_admon.sitemap z
                    ON x.codigo_sitemap_padre = z.codigo_sitemap
                    WHERE x.codigo_sitemap = @CodigoSiteMap";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoSiteMap", codigoSiteMap);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            int postCodigoSiteMap = dr.GetOrdinal("codigo_sitemap");
                            int postCodigoSistema = dr.GetOrdinal("codigo_sistema");
                            int postNombreSistema = dr.GetOrdinal("sistema");
                            int postTitulo = dr.GetOrdinal("titulo");
                            int postDescripcion = dr.GetOrdinal("descripcion");
                            int postNombreController = dr.GetOrdinal("nombre_controller");
                            int postNombreAction = dr.GetOrdinal("nombre_action");
                            int postNivel = dr.GetOrdinal("nivel");
                            int postNombreNivel = dr.GetOrdinal("nombre_nivel");
                            int postCodigSitemapPadre = dr.GetOrdinal("codigo_sitemap_padre");
                            int postTituloPadre = dr.GetOrdinal("titulo_padre");
                            while (dr.Read())
                            {
                                objItemMenu = new SiteMapCLS();
                                objItemMenu.CodigoSitemap = dr.GetInt32(postCodigoSiteMap);
                                objItemMenu.CodigoSistema = dr.GetInt16(postCodigoSistema);
                                objItemMenu.NombreSistema = dr.GetString(postNombreSistema);
                                objItemMenu.Titulo = dr.GetString(postTitulo);
                                objItemMenu.Descripcion = dr.IsDBNull(postDescripcion) ? "" : dr.GetString(postDescripcion);
                                objItemMenu.NombreController = dr.GetString(postNombreController);
                                objItemMenu.NombreAction = dr.GetString(postNombreAction);
                                objItemMenu.Nivel = dr.GetByte(postNivel);
                                objItemMenu.NombreNivel = dr.GetString(postNombreNivel);
                                objItemMenu.CodigoSitemapPadre = dr.IsDBNull(postCodigSitemapPadre) ? 0 : dr.GetInt32(postCodigSitemapPadre);
                                objItemMenu.TituloPadre = dr.IsDBNull(postTituloPadre) ? "" : dr.GetString(postTituloPadre);
                            }
                        }
                    }
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                    objItemMenu = null;
                }

                return objItemMenu;
            }

        }

        public List<SiteMapCLS> GetSiteMapsPadre(int codigoSistema, int nivel)
        {
            List<SiteMapCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    string filterSistema = String.Empty;
                    string filterNivel = String.Empty;
                    if (codigoSistema != -1) {
                        filterSistema = "AND x.codigo_sistema = " + codigoSistema.ToString();
                    }
                    if (nivel == 0)
                    {
                        filterNivel = "AND x.nivel = -1";
                    }
                    if (nivel == 1){
                        filterNivel = "AND x.nivel = 0";
                    }
                    if (nivel == 2)
                    {
                        //filterNivel = "AND x.nivel IN (0,1)";
                        filterNivel = "AND x.nivel = 1";
                    }
                    string sql = @"
                    SELECT x.codigo_sitemap, 
	                        x.titulo
                    FROM db_admon.sitemap x
                    INNER JOIN db_admon.sistema y
                    ON x.codigo_sistema = y.codigo_sistema
                    LEFT JOIN db_admon.sitemap z
                    ON x.codigo_sitemap_padre = z.codigo_sitemap
					WHERE x.estado <> @CodigoEstadoAnulado
                      AND x.codigo_sitemap_padre IS NOT NULL
					" + filterNivel + @"
                    " + filterSistema;

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoAnulado", Constantes.EstadoRegistro.BLOQUEADO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            SiteMapCLS objSiteMap;
                            lista = new List<SiteMapCLS>();
                            int postCodigoSiteMap = dr.GetOrdinal("codigo_sitemap");
                            int postTitulo = dr.GetOrdinal("titulo");

                            while (dr.Read())
                            {
                                objSiteMap = new SiteMapCLS();
                                objSiteMap.CodigoSitemap = dr.GetInt32(postCodigoSiteMap);
                                objSiteMap.Titulo = dr.GetString(postTitulo);
                                lista.Add(objSiteMap);
                            }
                        }
                    }
                    conexion.Close();
                }
                catch (Exception)
                {
                    lista = null;
                    conexion.Close();
                }

                return lista;
            }

        }

        public string GuardarItemMenu(SiteMapCLS objMenu, string usuarioIng)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                conexion.Open();

                SqlCommand cmd = conexion.CreateCommand();

                try
                {
                    string sentenciaSQL = @"
                    INSERT INTO db_admon.sitemap(codigo_sitemap, codigo_sistema,titulo,descripcion,nombre_controller,nombre_action,codigo_sitemap_padre,nivel,estado,usuario_ing,fecha_ing)
                    VALUES(NEXT VALUE FOR db_admon.SQ_SITEMAP, @CodigoSistema,@Titulo,@Descripcion,@NombreController,@NombreAction,@CodigoSiteMapPadre,@Nivel,@codigoEstado,@usuarioIng,@fechaIng)";

                    cmd.CommandText = sentenciaSQL;
                    cmd.Parameters.AddWithValue("@codigoSistema", objMenu.CodigoSistema);
                    cmd.Parameters.AddWithValue("@Titulo", objMenu.Titulo);
                    cmd.Parameters.AddWithValue("@Descripcion", objMenu.Descripcion);
                    cmd.Parameters.AddWithValue("@NombreController", objMenu.NombreController);
                    cmd.Parameters.AddWithValue("@NombreAction", objMenu.NombreAction);
                    cmd.Parameters.AddWithValue("@CodigoSiteMapPadre", objMenu.CodigoSitemapPadre);
                    cmd.Parameters.AddWithValue("@Nivel", objMenu.Nivel);
                    cmd.Parameters.AddWithValue("@codigoEstado", Constantes.EstadoRegistro.ACTIVO);
                    cmd.Parameters.AddWithValue("@usuarioIng", usuarioIng);
                    cmd.Parameters.AddWithValue("@fechaIng", DateTime.Now);
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

        public string ActualizarItemMenu(SiteMapCLS objMenu, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                conexion.Open();

                SqlCommand cmd = conexion.CreateCommand();

                try
                {
                    string sentenciaSQL = @"
                    UPDATE db_admon.sitemap
                    SET codigo_sistema = @CodigoSistema,
                        titulo = @Titulo,
                        descripcion = @Descripcion,
                        nombre_controller = @NombreController,
                        nombre_action = @NombreAction,
                        codigo_sitemap_padre = @CodigoSiteMapPadre,
                        nivel = @Nivel,
                        estado = @codigoEstado,
                        usuario_act = @UsuarioAct,
                        fecha_act = @FechaAct
                    WHERE codigo_sitemap = @CodigoSiteMap";

                    cmd.CommandText = sentenciaSQL;
                    cmd.Parameters.AddWithValue("@CodigoSiteMap", objMenu.CodigoSitemap);
                    cmd.Parameters.AddWithValue("@codigoSistema", objMenu.CodigoSistema);
                    cmd.Parameters.AddWithValue("@Titulo", objMenu.Titulo);
                    cmd.Parameters.AddWithValue("@Descripcion", objMenu.Descripcion);
                    cmd.Parameters.AddWithValue("@NombreController", objMenu.NombreController);
                    cmd.Parameters.AddWithValue("@NombreAction", objMenu.NombreAction);
                    cmd.Parameters.AddWithValue("@CodigoSiteMapPadre", objMenu.CodigoSitemapPadre);
                    cmd.Parameters.AddWithValue("@Nivel", objMenu.Nivel);
                    cmd.Parameters.AddWithValue("@codigoEstado", Constantes.EstadoRegistro.ACTIVO);
                    cmd.Parameters.AddWithValue("@UsuarioAct", usuarioAct);
                    cmd.Parameters.AddWithValue("@FechaAct", DateTime.Now);
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

        public string AnularItemMenu(int codigoSitemap, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
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
                    string sentenciaUpdate = @"
                    UPDATE db_admon.sitemap
                    SET estado = @codigoEstado,
                        usuario_act = @UsuarioAct,
                        fecha_act = @FechaAct
                    WHERE codigo_sitemap = @CodigoSiteMap";

                    cmd.CommandText = sentenciaUpdate;
                    cmd.Parameters.AddWithValue("@CodigoSiteMap", codigoSitemap);
                    cmd.Parameters.AddWithValue("@codigoEstado", Constantes.EstadoRegistro.BLOQUEADO);
                    cmd.Parameters.AddWithValue("@UsuarioAct", usuarioAct);
                    cmd.Parameters.AddWithValue("@FechaAct", DateTime.Now);
                    cmd.ExecuteNonQuery();

                    string sentenciaDelete = @"
                    DELETE FROM db_admon.rol_sitemap 
                    WHERE codigo_sitemap = @CodigoSiteMap";
                    cmd.CommandText = sentenciaDelete;
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
            }

            return resultado;
        }


    }
}
