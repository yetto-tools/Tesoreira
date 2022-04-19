using CapaEntidad.Administracion;
using CapaEntidad.Tesoreria;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Administracion
{
    public class UsuarioDAL: CadenaConexion
    {
        public string GuardarUsuario(UsuarioCLS objUsuario)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                conexion.Open();
                SqlCommand cmd = conexion.CreateCommand();

                try
                {
                    string sentenciaSQL = @"
                    INSERT INTO db_admon.usuario (id_usuario,nombre_usuario,contrasenia,cui,super_admin,estado,usuario_ing,fecha_ing) 
                    VALUES( @IdUsuario,@NombreUsuario,@Contrasenia,@Cui,@SuperAdmin,@CodigoEstado,@UsuarioIng,@FechaIng)";

                    cmd.CommandText = sentenciaSQL;
                    cmd.Parameters.AddWithValue("@IdUsuario", objUsuario.IdUsuario);
                    cmd.Parameters.AddWithValue("@NombreUsuario", objUsuario.PrimerNombre + " " + objUsuario.PrimerApellido);
                    cmd.Parameters.AddWithValue("@Contrasenia", Util.Seguridad.CifrarCadena(objUsuario.Contrasenia));
                    cmd.Parameters.AddWithValue("@Cui", objUsuario.Cui);
                    cmd.Parameters.AddWithValue("@SuperAdmin", objUsuario.SuperAdmin);
                    cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.EstadoRegistro.ACTIVO);
                    cmd.Parameters.AddWithValue("@UsuarioIng", objUsuario.UsuarioIng);
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

        public string ActualizarContrasenia(UsuarioCLS objUsuario)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                conexion.Open();
                SqlCommand cmd = conexion.CreateCommand();

                try
                {
                    string sentenciaSQL = @"
                    UPDATE db_admon.usuario
                    SET contrasenia = @Contrasenia
                    WHERE id_usuario = @IdUsuario";

                    cmd.CommandText = sentenciaSQL;
                    cmd.Parameters.AddWithValue("@IdUsuario", objUsuario.IdUsuario);
                    cmd.Parameters.AddWithValue("@Contrasenia", Util.Seguridad.CifrarCadena(objUsuario.Contrasenia));
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

        public List<UsuarioCLS> GetListaUsuarios()
        {
            List<UsuarioCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT x.id_usuario, 
	                       x.nombre_usuario, 
	                       y.primer_nombre,
	                       y.primer_apellido,
	                       y.codigo_genero,
	                       x.cui, 
	                       x.estado AS codigo_estado,
                           CASE
                             WHEN x.estado = 0 THEN 'BLOQUEADO'
                             WHEN x.estado = 1 THEN 'ACTIVO'
                             ELSE 'NO DEFINIDO'
                           END AS estado, 
                           x.set_semana_ant,
                           x.super_admin,
                           CASE 
                              WHEN x.super_admin = 1 THEN 'SI'
                              ELSE 'NO'
                           END AS es_super_admin, 
                           1 AS permiso_editar
                    FROM db_admon.usuario x
                    INNER JOIN db_rrhh.persona y
                    ON x.cui = y.cui";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            UsuarioCLS objUsuario;
                            lista = new List<UsuarioCLS>();
                            int postIdUsuario = dr.GetOrdinal("id_usuario");
                            int postNombreUsuario = dr.GetOrdinal("nombre_usuario");
                            int postCodigoEstado = dr.GetOrdinal("codigo_estado");
                            int postEstado = dr.GetOrdinal("estado");
                            int postPermisoEditar = dr.GetOrdinal("permiso_editar");
                            int postSuperAdmin = dr.GetOrdinal("super_admin");
                            int postEsSuperAdministrador = dr.GetOrdinal("es_super_admin");
                            while (dr.Read())
                            {
                                objUsuario = new UsuarioCLS();
                                objUsuario.IdUsuario = dr.GetString(postIdUsuario);
                                objUsuario.NombreUsuario = dr.GetString(postNombreUsuario);
                                objUsuario.CodigoEstado = dr.GetByte(postCodigoEstado);
                                objUsuario.Estado = dr.GetString(postEstado);
                                objUsuario.SuperAdmin = dr.GetByte(postSuperAdmin);
                                objUsuario.EsSuperAdmin = dr.GetString(postEsSuperAdministrador);
                                objUsuario.PermisoEditar = dr.GetInt32(postPermisoEditar);
                                lista.Add(objUsuario);
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

        public UsuarioCLS Login(string idUsuario, string contrasenia)
        {
            UsuarioCLS objUsuario = new UsuarioCLS();
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT x.id_usuario, 
	                       x.nombre_usuario, 
	                       y.primer_nombre,
	                       y.primer_apellido,
	                       y.codigo_genero,
	                       x.cui, 
	                       x.estado,
                           x.set_semana_ant,
                           x.super_admin
                    FROM db_admon.usuario x
                    INNER JOIN db_rrhh.persona y
                    ON x.cui = y.cui
                    WHERE x.id_usuario = @IdUsuario
                      AND x.contrasenia = @Contrasenia";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                        cmd.Parameters.AddWithValue("@Contrasenia", Util.Seguridad.CifrarCadena(contrasenia));
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            int postIdUsuario = dr.GetOrdinal("id_usuario");
                            int postNombreUsuario = dr.GetOrdinal("nombre_usuario");
                            int postEstado = dr.GetOrdinal("estado");
                            int postSetSemanaAnt = dr.GetOrdinal("set_semana_ant");
                            int postSuperAdmin = dr.GetOrdinal("super_admin");

                            while (dr.Read())
                            {
                                objUsuario = new UsuarioCLS();
                                objUsuario.IdUsuario = dr.GetString(postIdUsuario);
                                objUsuario.NombreUsuario = dr.GetString(postNombreUsuario);
                                objUsuario.SetSemanaAnterior = dr.GetByte(postSetSemanaAnt);
                                objUsuario.SuperAdmin = dr.GetByte(postSuperAdmin);
                            }
                        }
                    }
                    conexion.Close();
                }
                catch (Exception ex)
                {
                    conexion.Close();
                }

                return objUsuario;
            }
        }

        public UsuarioCLS GetDataUsuario(string idUsuario)
        {
            UsuarioCLS objUsuario = new UsuarioCLS();
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT x.id_usuario, 
	                       x.nombre_usuario, 
	                       y.primer_nombre,
	                       y.primer_apellido,
	                       y.codigo_genero,
	                       x.cui, 
	                       x.estado,
                           x.set_semana_ant,
                           x.super_admin
                    FROM db_admon.usuario x
                    INNER JOIN db_rrhh.persona y
                    ON x.cui = y.cui
                    WHERE x.id_usuario = @IdUsuario";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            int postIdUsuario = dr.GetOrdinal("id_usuario");
                            int postCui = dr.GetOrdinal("cui");
                            int postNombreUsuario = dr.GetOrdinal("nombre_usuario");
                            int postEstado = dr.GetOrdinal("estado");
                            int postSetSemanaAnt = dr.GetOrdinal("set_semana_ant");
                            int postSuperAdmin = dr.GetOrdinal("super_admin");
                            while (dr.Read())
                            {
                                objUsuario = new UsuarioCLS();
                                objUsuario.Cui = dr.GetString(postCui);
                                objUsuario.IdUsuario = dr.GetString(postIdUsuario);
                                objUsuario.NombreUsuario = dr.GetString(postNombreUsuario);
                                objUsuario.SetSemanaAnterior = dr.GetByte(postSetSemanaAnt);
                                objUsuario.SuperAdmin = dr.GetByte(postSuperAdmin);
                            }
                        }
                    }
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                }

                return objUsuario;
            }
        }

        public List<RolCLS> GetPermisoRoles(string idUsuario)
        {
            List<RolCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT x.codigo_rol, 
	                       x.nombre AS nombre_rol,
	                       x.descripcion,
	                       CASE	
	                         WHEN y.codigo_rol IS NOT NULL THEN 1
		                     ELSE 0
	                       END asignado
                    FROM db_admon.rol x
                    LEFT JOIN ( SELECT distinct codigo_rol
                                FROM db_admon.usuario_rol
                                WHERE id_usuario = @IdUsuario
                              ) y
                    ON x.codigo_rol = y.codigo_rol";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            List<RolCLS> listaRoles = new List<RolCLS>();
                            int postCodigoRol = dr.GetOrdinal("codigo_rol");
                            int postNombreRol = dr.GetOrdinal("nombre_rol");
                            int postDescripcion = dr.GetOrdinal("descripcion");
                            int postAsignado = dr.GetOrdinal("asignado");
                            RolCLS objRol;
                            while (dr.Read())
                            {
                                objRol = new RolCLS();
                                objRol.CodigoRol = dr.GetInt32(postCodigoRol);
                                objRol.Nombre = dr.GetString(postNombreRol);
                                objRol.Descripcion = dr.GetString(postDescripcion);
                                objRol.Asignado = dr.GetInt32(postAsignado);
                                listaRoles.Add(objRol);
                            }//fin while
                            lista = listaRoles;
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

        public List<ConfiguracionCajaChicaCLS> GetPermisoCajasChicas(string idUsuario)
        {
            List<ConfiguracionCajaChicaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT x.codigo_caja_chica, 
                           x.nombre_caja_chica, 
	                       CASE
		                      WHEN y.codigo_caja_chica IS NOT NULL THEN 1
		                      ELSE 0
                           END asignado
                    FROM db_admon.caja_chica x
                    LEFT JOIN ( SELECT codigo_caja_chica
                                FROM db_admon.usuario_caja_chica
                                WHERE id_usuario = @IdUsuario
                              ) y
                    ON x.codigo_caja_chica = y.codigo_caja_chica";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            List<ConfiguracionCajaChicaCLS> listaCajasChicas = new List<ConfiguracionCajaChicaCLS>();
                            int postCodigoCajaChica = dr.GetOrdinal("codigo_caja_chica");
                            int postNombreCajaChica = dr.GetOrdinal("nombre_caja_chica");
                            int postAsignado = dr.GetOrdinal("asignado");

                            ConfiguracionCajaChicaCLS objCajaChica;
                            while (dr.Read())
                            {
                                objCajaChica = new ConfiguracionCajaChicaCLS();
                                objCajaChica.CodigoCajaChica = dr.GetInt16(postCodigoCajaChica);
                                objCajaChica.NombreCajaChica = dr.GetString(postNombreCajaChica);
                                objCajaChica.Asignado = dr.GetInt32(postAsignado);
                                listaCajasChicas.Add(objCajaChica);
                            }//fin while
                            lista = listaCajasChicas;
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

        public List<EmpresaCLS> GetPermisoEmpresas(string idUsuario)
        {
            List<EmpresaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT x.codigo_empresa,
	                       z.nombre_comercial AS nombre_empresa,
	                       CASE
		                      WHEN y.codigo_empresa IS NOT NULL THEN 1
		                      ELSE 0
                           END asignado
                    FROM db_admon.empresa x
                    LEFT JOIN ( SELECT codigo_empresa 
                                FROM db_admon.usuario_empresa
                                WHERE id_usuario = @IdUsuario
                              ) y
                    ON x.codigo_empresa = y.codigo_empresa
                    INNER JOIN db_admon.empresa z
                    ON x.codigo_empresa = z.codigo_empresa";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            List<EmpresaCLS> listaEmpresas = new List<EmpresaCLS>();
                            int postCodigoEmpresa = dr.GetOrdinal("codigo_empresa");
                            int postNombreEmpresa = dr.GetOrdinal("nombre_empresa");
                            int postAsignado = dr.GetOrdinal("asignado");

                            EmpresaCLS objEmpresa;
                            while (dr.Read())
                            {
                                objEmpresa = new EmpresaCLS();
                                objEmpresa.CodigoEmpresa = dr.GetInt16(postCodigoEmpresa);
                                objEmpresa.NombreComercial = dr.GetString(postNombreEmpresa);
                                objEmpresa.Asignado = dr.GetInt32(postAsignado);
                                listaEmpresas.Add(objEmpresa);
                            }//fin while
                            lista = listaEmpresas;
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

        public List<TipoReporteCLS> GetPermisoReportes(string idUsuario)
        {
            List<TipoReporteCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT x.codigo_tipo_reporte, 
	                       x.nombre AS nombre_reporte,
	                       CASE
		                      WHEN y.codigo_tipo_reporte IS NOT NULL THEN 1
		                      ELSE 0
                           END asignado
                    FROM db_admon.tipo_reporte x
                    LEFT JOIN ( SELECT codigo_tipo_reporte 
                                FROM db_admon.usuario_tipo_reporte
                                WHERE id_usuario = @IdUsuario
                              ) y
                    ON x.codigo_tipo_reporte = y.codigo_tipo_reporte
                    ORDER BY x.nombre ASC";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            List<TipoReporteCLS> listaReportes = new List<TipoReporteCLS>();
                            int postCodigoTipoReporte = dr.GetOrdinal("codigo_tipo_reporte");
                            int postNombreReporte = dr.GetOrdinal("nombre_reporte");
                            int postAsignado = dr.GetOrdinal("asignado");

                            TipoReporteCLS objTipoReporte;
                            while (dr.Read())
                            {
                                objTipoReporte = new TipoReporteCLS();
                                objTipoReporte.CodigoTipoReporte = dr.GetInt32(postCodigoTipoReporte);
                                objTipoReporte.Nombre = dr.GetString(postNombreReporte);
                                objTipoReporte.Asignado = dr.GetInt32(postAsignado);
                                listaReportes.Add(objTipoReporte);
                            }//fin while
                            lista = listaReportes;
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


        public string GuardarPermisos(List<PermisoCLS> objPermisos, string idUsuario, string usuarioIng)
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
                    string sqlDeleteRoles = "DELETE FROM db_admon.usuario_rol WHERE id_usuario = '" + idUsuario +  "'";
                    string sqlDeleteCajasChicas = "DELETE FROM db_admon.usuario_caja_chica WHERE id_usuario = '" + idUsuario + "'";
                    string sqlDeleteEmpresas = "DELETE FROM db_admon.usuario_empresa WHERE id_usuario = '" + idUsuario + "'";
                    string sqlDeleteReportes = "DELETE FROM db_admon.usuario_tipo_reporte WHERE id_usuario = '" + idUsuario + "'";
                    cmd.CommandText = sqlDeleteRoles;
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = sqlDeleteCajasChicas;
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = sqlDeleteEmpresas;
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = sqlDeleteReportes;
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.Add("@IdUsuario", SqlDbType.VarChar);
                    cmd.Parameters.Add("@CodigoRol", SqlDbType.Int);
                    cmd.Parameters.Add("@CodigoEstado", SqlDbType.TinyInt);
                    cmd.Parameters.Add("@UsuarioIng", SqlDbType.VarChar);
                    cmd.Parameters.Add("@FechaIng", SqlDbType.DateTime);

                    string sqlInsertRoles = @"
                    INSERT INTO db_admon.usuario_rol(id_usuario, codigo_rol, estado, usuario_ing, fecha_ing)
                    VALUES(@IdUsuario,@CodigoRol,@CodigoEstado,@UsuarioIng,@FechaIng)";
                    foreach (PermisoCLS objPermiso in objPermisos)
                    {
                        if (objPermiso.codigoTipoPermiso == 1 && objPermiso.Asignado == 1)
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = sqlInsertRoles;
                            cmd.Parameters["@IdUsuario"].Value = idUsuario;
                            cmd.Parameters["@CodigoRol"].Value = objPermiso.Codigo;
                            cmd.Parameters["@CodigoEstado"].Value = Constantes.EstadoRegistro.ACTIVO;
                            cmd.Parameters["@UsuarioIng"].Value = usuarioIng;
                            cmd.Parameters["@FechaIng"].Value = DateTime.Now;
                            cmd.ExecuteNonQuery();
                        }
                    }

                    cmd.Parameters.Add("@CodigoCajaChica", SqlDbType.SmallInt);
                    string sqlInsertCajasChicas = @"
                    INSERT INTO db_admon.usuario_caja_chica(id_usuario, codigo_caja_chica, estado, usuario_ing, fecha_ing)
                    VALUES(@IdUsuario,@CodigoCajaChica,@CodigoEstado,@UsuarioIng,@FechaIng)";
                    foreach (PermisoCLS objPermiso in objPermisos)
                    {
                        if (objPermiso.codigoTipoPermiso == 2 && objPermiso.Asignado == 1)
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = sqlInsertCajasChicas;
                            cmd.Parameters["@IdUsuario"].Value = idUsuario;
                            cmd.Parameters["@CodigoCajaChica"].Value = (short)objPermiso.Codigo;
                            cmd.Parameters["@CodigoEstado"].Value = Constantes.EstadoRegistro.ACTIVO;
                            cmd.Parameters["@UsuarioIng"].Value = usuarioIng;
                            cmd.Parameters["@FechaIng"].Value = DateTime.Now;
                            cmd.ExecuteNonQuery();
                        }
                    }

                    cmd.Parameters.Add("@CodigoEmpresa", SqlDbType.SmallInt);
                    string sqlInsertEmpresas = @"
                    INSERT INTO db_admon.usuario_empresa(codigo_empresa, id_usuario, estado, usuario_ing, fecha_ing)
                    VALUES(@CodigoEmpresa,@IdUsuario,@CodigoEstado,@UsuarioIng,@FechaIng)";
                    foreach (PermisoCLS objPermiso in objPermisos)
                    {
                        if (objPermiso.codigoTipoPermiso == 3 && objPermiso.Asignado == 1)
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = sqlInsertEmpresas;
                            cmd.Parameters["@IdUsuario"].Value = idUsuario;
                            cmd.Parameters["@CodigoEmpresa"].Value = (short)objPermiso.Codigo;
                            cmd.Parameters["@CodigoEstado"].Value = Constantes.EstadoRegistro.ACTIVO;
                            cmd.Parameters["@UsuarioIng"].Value = usuarioIng;
                            cmd.Parameters["@FechaIng"].Value = DateTime.Now;
                            cmd.ExecuteNonQuery();
                        }
                    }

                    cmd.Parameters.RemoveAt("@CodigoEmpresa");
                    cmd.Parameters.Add("@CodigoTipoReporte", SqlDbType.Int);
                    string sqlInsertReportes = @"
                    INSERT INTO db_admon.usuario_tipo_reporte(codigo_tipo_reporte, id_usuario,estado,usuario_ing,fecha_ing)
                    VALUES(@CodigoTipoReporte,@IdUsuario,@CodigoEstado,@UsuarioIng,@FechaIng)";
                    foreach (PermisoCLS objPermiso in objPermisos)
                    {
                        if (objPermiso.codigoTipoPermiso == 4 && objPermiso.Asignado == 1)
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = sqlInsertReportes;
                            cmd.Parameters["@IdUsuario"].Value = idUsuario;
                            cmd.Parameters["@CodigoTipoReporte"].Value = objPermiso.Codigo;
                            cmd.Parameters["@CodigoEstado"].Value = Constantes.EstadoRegistro.ACTIVO;
                            cmd.Parameters["@UsuarioIng"].Value = usuarioIng;
                            cmd.Parameters["@FechaIng"].Value = DateTime.Now;
                            cmd.ExecuteNonQuery();
                        }
                    }


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
