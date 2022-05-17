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
    public class TipoReporteDAL: CadenaConexion
    {
        public List<TipoReporteCLS> GetAllTiposReportes()
        {
            List<TipoReporteCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT codigo_tipo_reporte, 
	                       nombre, 
	                       descripcion, 
	                       nombre_controlador,
	                       nombre_accion,
	                       pdf,
	                       excel,
	                       web,
                           1 AS permiso_editar,
                           1 AS permiso_anular 
                    FROM db_admon.tipo_reporte";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            TipoReporteCLS objTipoReporte;
                            lista = new List<TipoReporteCLS>();
                            int postCodigoTipoReporte = dr.GetOrdinal("codigo_tipo_reporte");
                            int postNombre = dr.GetOrdinal("nombre");
                            int postDescripcion = dr.GetOrdinal("descripcion");
                            int postNombreControlador = dr.GetOrdinal("nombre_controlador");
                            int postNombreAccion = dr.GetOrdinal("nombre_accion");
                            int postPdf = dr.GetOrdinal("pdf");
                            int postExcel = dr.GetOrdinal("excel");
                            int postWeb = dr.GetOrdinal("web");
                            int postPermisoEditar = dr.GetOrdinal("permiso_editar");
                            int postPermisoAnular = dr.GetOrdinal("permiso_anular");

                            while (dr.Read())
                            {
                                objTipoReporte = new TipoReporteCLS();
                                objTipoReporte.CodigoTipoReporte = dr.GetInt32(postCodigoTipoReporte);
                                objTipoReporte.Nombre = dr.GetString(postNombre);
                                objTipoReporte.Descripcion = dr.GetString(postDescripcion);
                                objTipoReporte.NombreControlador = dr.GetString(postNombreControlador);
                                objTipoReporte.NombreAccion = dr.GetString(postNombreAccion);
                                objTipoReporte.Pdf = dr.GetByte(postPdf);
                                objTipoReporte.Excel = dr.GetByte(postExcel);
                                objTipoReporte.Web = dr.GetByte(postWeb);
                                objTipoReporte.PermisoEditar = dr.GetInt32(postPermisoEditar);
                                objTipoReporte.PermisoAnular = dr.GetInt32(postPermisoAnular);
                                lista.Add(objTipoReporte);
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

        public List<TipoReporteCLS> GetTiposDeReportesAsignados(string idUsuario, int superAdmin)
        {
            List<TipoReporteCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    string sql = String.Empty;
                    if (superAdmin == 1)
                    {
                        sql = @"
                        SELECT x.codigo_tipo_reporte, 
	                           x.nombre, 
	                           x.descripcion, 
	                           x.nombre_controlador,
	                           x.nombre_accion,
	                           x.pdf,
	                           x.excel,
	                           x.web,
                               y.nombre AS categoria 
                         FROM db_admon.tipo_reporte x
                         INNER JOIN db_admon.categoria_reporte y
                         ON x.codigo_categoria = y.codigo_categoria";
                    }
                    else
                    {
                        sql = @"
                        SELECT x.codigo_tipo_reporte, 
	                           x.nombre, 
	                           x.descripcion, 
	                           x.nombre_controlador,
	                           x.nombre_accion,
	                           x.pdf,
	                           x.excel,
	                           x.web,
                               y.nombre AS categoria 
                         FROM db_admon.tipo_reporte x
                         INNER JOIN db_admon.categoria_reporte y                                         
                         ON x.codigo_categoria = y.codigo_categoria
                         WHERE x.codigo_tipo_reporte IN (SELECT codigo_tipo_reporte FROM db_admon.usuario_tipo_reporte WHERE id_usuario = @IdUsuario)";
                    }
                    

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        if (superAdmin == 0)
                        {
                            cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                        }
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            TipoReporteCLS objTipoReporte;
                            lista = new List<TipoReporteCLS>();
                            int postCodigoTipoReporte = dr.GetOrdinal("codigo_tipo_reporte");
                            int postNombre = dr.GetOrdinal("nombre");
                            int postDescripcion = dr.GetOrdinal("descripcion");
                            int postNombreControlador = dr.GetOrdinal("nombre_controlador");
                            int postNombreAccion = dr.GetOrdinal("nombre_accion");
                            int postPdf = dr.GetOrdinal("pdf");
                            int postExcel = dr.GetOrdinal("excel");
                            int postWeb = dr.GetOrdinal("web");
                            int postCategoria = dr.GetOrdinal("categoria");

                            while (dr.Read())
                            {
                                objTipoReporte = new TipoReporteCLS();
                                objTipoReporte.CodigoTipoReporte = dr.GetInt32(postCodigoTipoReporte);
                                objTipoReporte.Nombre = dr.GetString(postNombre);
                                objTipoReporte.Descripcion = dr.GetString(postDescripcion);
                                objTipoReporte.NombreControlador = dr.GetString(postNombreControlador);
                                objTipoReporte.NombreAccion = dr.GetString(postNombreAccion);
                                objTipoReporte.Pdf = dr.GetByte(postPdf);
                                objTipoReporte.Excel = dr.GetByte(postExcel);
                                objTipoReporte.Web = dr.GetByte(postWeb);
                                objTipoReporte.Categoria = dr.GetString(postCategoria);
                                lista.Add(objTipoReporte);
                            }
                        }
                    }
                    conexion.Close();
                }
                catch (Exception e)
                {
                    conexion.Close();
                    lista = null;
                }

                return lista;
            }
        }

        public string GuardarTipoReporte(TipoReporteCLS objTipoReporte, string usuarioIng)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                conexion.Open();
                SqlCommand cmd = conexion.CreateCommand();

                try
                {
                    string sentenciaSQL = @"
                    INSERT INTO db_admon.tipo_reporte(codigo_tipo_reporte,nombre,descripcion,nombre_controlador,nombre_accion,pdf,excel,web,estado,usuario_ing,fecha_ing,usuario_act,fecha_act)
                    VALUES(NEXT VALUE FOR db_admon.SQ_TIPO_REPORTE,
                           @Nombre,
                           @Descripcion,
                           @NombreControlador,
                           @NombreAccion,
                           @Pdf,
                           @Excel,
                           @Web,
                           @CodigoEstado,
                           @UsuarioIng,
                           @FechaIng,
                           @UsuarioAct,
                           @FechaAct)";

                    cmd.CommandText = sentenciaSQL;
                    cmd.Parameters.AddWithValue("@Nombre", objTipoReporte.Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", objTipoReporte.Descripcion);
                    cmd.Parameters.AddWithValue("@NombreControlador", objTipoReporte.NombreControlador);
                    cmd.Parameters.AddWithValue("@NombreAccion", objTipoReporte.NombreAccion);
                    cmd.Parameters.AddWithValue("@Pdf", objTipoReporte.Pdf);
                    cmd.Parameters.AddWithValue("@Excel", objTipoReporte.Excel);
                    cmd.Parameters.AddWithValue("@Web", objTipoReporte.Web);
                    cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.EstadoRegistro.ACTIVO);
                    cmd.Parameters.AddWithValue("@UsuarioIng", usuarioIng);
                    cmd.Parameters.AddWithValue("@FechaIng", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UsuarioAct", DBNull.Value);
                    cmd.Parameters.AddWithValue("@FechaAct", DBNull.Value);
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

        public string ActualizarTipoReporte(TipoReporteCLS objTipoReporte, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                conexion.Open();
                SqlCommand cmd = conexion.CreateCommand();

                try
                {
                    string sentenciaSQL = @"
                    UPDATE db_admon.tipo_reporte
                    SET  nombre = @Nombre,
                         descripcion = @Descripcion,
                         nombre_controlador = @NombreControlador,
                         nombre_accion = @NombreAccion,
                         estado = @CodigoEstado,
                         pdf = @Pdf,
                         excel = @Excel,
                         web = @Web,
                         usuario_act = @UsuarioAct,
                         fecha_act = @FechaAct   
                    WHERE codigo_tipo_reporte = @CodigoTipoReporte";

                    cmd.CommandText = sentenciaSQL;
                    cmd.Parameters.AddWithValue("@CodigoTipoReporte", objTipoReporte.CodigoTipoReporte);
                    cmd.Parameters.AddWithValue("@Nombre", objTipoReporte.Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", objTipoReporte.Descripcion);
                    cmd.Parameters.AddWithValue("@NombreControlador", objTipoReporte.NombreControlador);
                    cmd.Parameters.AddWithValue("@NombreAccion", objTipoReporte.NombreAccion);
                    cmd.Parameters.AddWithValue("@Pdf", objTipoReporte.Pdf);
                    cmd.Parameters.AddWithValue("@Excel", objTipoReporte.Excel);
                    cmd.Parameters.AddWithValue("@Web", objTipoReporte.Web);
                    cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.EstadoRegistro.ACTIVO);
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
    }
}
