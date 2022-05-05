using CapaEntidad.RRHH;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.RRHH
{
    public class PersonaDAL: CadenaConexion
    {
        public List<PersonaCLS> GetAllPersonas(int noIncluidoEnPlanilla)
        {
            List<PersonaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaRRHH))
            {
                try
                {
                    string filterNoIncluidoEnPlanilla = String.Empty;
                    if (noIncluidoEnPlanilla == 1)
                    {
                        filterNoIncluidoEnPlanilla = " WHERE x.no_incluido_en_planilla = 1";
                    }
                    conexion.Open();
                    string sql = @"
                    SELECT x.cui, 
                           x.primer_nombre,
                           x.segundo_nombre,
                           x.tercer_nombre,
                           x.primer_apellido,
                           x.segundo_apellido,
                           x.apellido_casada, 
		                   x.nombre_completo, 
		                   x.fecha_nacimiento, 
                           FORMAT(x.fecha_nacimiento,'dd/MM/yyyy') AS fecha_nacimiento_str,
		                   x.codigo_genero,
                           CASE
                             WHEN x.codigo_genero = 'M' THEN 'MASCULINO'
                             WHEN x.codigo_genero = 'F' THEN 'FEMENINO'
                             ELSE 'NO DEFINIDO'   
                           END AS genero, 
		                   x.correo_electronico,
		                   x.codigo_departamento_residencia,
		                   z.nombre AS departamento_residencia,
		                   x.codigo_municipio_residencia,
		                   y.nombre AS municipio_residencia,
		                   x.zona,
		                   x.direccion_residencia,
		                   x.no_incluido_en_planilla,
		                   COALESCE(x.codigo_area,0) AS codigo_area,
		                   w.nombre AS area,
		                   x.codigo_estado,
		                   m.nombre AS estado,
                           1 AS permiso_editar
                    FROM db_rrhh.persona x
                    LEFT JOIN db_admon.municipio y
                    ON x.codigo_municipio_residencia = y.codigo_municipio AND x.codigo_departamento_residencia = y.codigo_departamento
                    LEFT JOIN db_admon.departamento z
                    ON x.codigo_departamento_residencia = z.codigo_departamento
                    LEFT JOIN db_rrhh.area w
                    ON x.codigo_area = w.codigo_area
                    INNER JOIN db_rrhh.estado_empleado m
                    ON x.codigo_estado = m.codigo_estado_empleado
                    " + filterNoIncluidoEnPlanilla;

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            PersonaCLS objPersona;
                            lista = new List<PersonaCLS>();
                            int postCui = dr.GetOrdinal("cui");
                            int postPrimerNombre = dr.GetOrdinal("primer_nombre");
                            int postSegundoNombre = dr.GetOrdinal("segundo_nombre");
                            int postTercerNombre = dr.GetOrdinal("tercer_nombre");
                            int postPrimerApellido = dr.GetOrdinal("primer_apellido");
                            int postSegundoApellido = dr.GetOrdinal("segundo_apellido");
                            int postApellidoCasada = dr.GetOrdinal("apellido_casada");
                            int postNombreCompleto = dr.GetOrdinal("nombre_completo");
                            int postFechaNacimiento = dr.GetOrdinal("fecha_nacimiento");
                            int postFechaNacimientoStr = dr.GetOrdinal("fecha_nacimiento_str");
                            int postCodigoGenero = dr.GetOrdinal("codigo_genero");
                            int postGenero = dr.GetOrdinal("genero");
                            int postCorreoElectronico = dr.GetOrdinal("correo_electronico");
                            int postCodigoDepartamentoResidencia = dr.GetOrdinal("codigo_departamento_residencia");
                            int postDepartamentoResidencia = dr.GetOrdinal("departamento_residencia");
                            int postCodigoMunicipioResidencia = dr.GetOrdinal("codigo_municipio_residencia");
                            int postMunicipioResidencia = dr.GetOrdinal("municipio_residencia");
                            int postZona = dr.GetOrdinal("zona");
                            int postDireccionResidencia = dr.GetOrdinal("direccion_residencia");
                            int postNoIncluidoEnPlanilla = dr.GetOrdinal("no_incluido_en_planilla");
                            int postCodigoArea = dr.GetOrdinal("codigo_area");
                            int postArea = dr.GetOrdinal("area");
                            int postCodigoEstado = dr.GetOrdinal("codigo_estado");
                            int postEstado = dr.GetOrdinal("estado");
                            int postPermisoEditar = dr.GetOrdinal("permiso_editar");

                            while (dr.Read())
                            {
                                objPersona = new PersonaCLS();
                                objPersona.Cui = dr.GetString(postCui);
                                objPersona.PrimerNombre = dr.GetString(postPrimerNombre);
                                objPersona.SegundoNombre = dr.IsDBNull(postSegundoNombre) ? "" : dr.GetString(postSegundoNombre);
                                objPersona.TercerNombre = dr.IsDBNull(postTercerNombre) ? "" : dr.GetString(postTercerNombre);
                                objPersona.PrimerApellido = dr.GetString(postPrimerApellido);
                                objPersona.SegundoApellido = dr.IsDBNull(postSegundoApellido) ? "" : dr.GetString(postSegundoApellido);
                                objPersona.ApellidoCasada = dr.IsDBNull(postApellidoCasada) ? "" : dr.GetString(postApellidoCasada);
                                objPersona.NombreCompleto = dr.GetString(postNombreCompleto);
                                objPersona.FechaNacimiento = dr.GetDateTime(postFechaNacimiento);
                                objPersona.FechaNacimientoStr = dr.GetString(postFechaNacimientoStr);
                                objPersona.CodigoGenero = dr.GetString(postCodigoGenero);
                                objPersona.Genero = dr.GetString(postGenero);
                                objPersona.CorreoElectronico = dr.IsDBNull(postCorreoElectronico) ? "": dr.GetString(postCorreoElectronico);
                                objPersona.CodigoDepartamentoResidencia = dr.GetInt16(postCodigoDepartamentoResidencia);
                                objPersona.DepartamentoResidencia = dr.GetString(postDepartamentoResidencia);
                                objPersona.CodigoMunicipioResidencia = dr.GetInt16(postCodigoMunicipioResidencia);
                                objPersona.MunicipioResidencia = dr.GetString(postMunicipioResidencia);
                                objPersona.Zona = dr.GetByte(postZona);
                                objPersona.DireccionResidencia = dr.IsDBNull(postDireccionResidencia) ? "" : dr.GetString(postDireccionResidencia);
                                objPersona.NoIncluidoEnPlanilla = dr.GetByte(postNoIncluidoEnPlanilla);
                                objPersona.CodigoArea = (short)dr.GetInt32(postCodigoArea);
                                objPersona.Area = dr.IsDBNull(postArea) ? "" : dr.GetString(postArea);
                                objPersona.CodigoEstado = dr.GetInt16(postCodigoEstado);
                                objPersona.Estado = dr.GetString(postEstado);
                                objPersona.PermisoEditar = dr.GetInt32(postPermisoEditar);
                                lista.Add(objPersona);
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
            }
            return lista;
        }

        public List<PersonaCLS> GetAllPersonasSinUsuario()
        {
            List<PersonaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaRRHH))
            {
                try
                {
                    string sql = @"
                    SELECT x.cui, 
                           x.primer_nombre,
                           x.segundo_nombre,
                           x.tercer_nombre,
                           x.primer_apellido,
                           x.segundo_apellido,
                           x.apellido_casada, 
		                   x.nombre_completo, 
		                   x.fecha_nacimiento, 
                           FORMAT(x.fecha_nacimiento,'dd/MM/yyyy') AS fecha_nacimiento_str,
		                   x.codigo_genero,
                           CASE
                             WHEN x.codigo_genero = 'M' THEN 'MASCULINO'
                             WHEN x.codigo_genero = 'F' THEN 'FEMENINO'
                             ELSE 'NO DEFINIDO'   
                           END AS genero, 
		                   x.correo_electronico,
		                   x.codigo_departamento_residencia,
		                   z.nombre AS departamento_residencia,
		                   x.codigo_municipio_residencia,
		                   y.nombre AS municipio_residencia,
		                   x.zona,
		                   x.direccion_residencia,
		                   x.no_incluido_en_planilla,
		                   COALESCE(x.codigo_area,0) AS codigo_area,
		                   w.nombre AS area,
		                   x.codigo_estado,
		                   m.nombre AS estado,
                           1 AS permiso_editar
                    FROM db_rrhh.persona x
                    LEFT JOIN db_admon.municipio y
                    ON x.codigo_municipio_residencia = y.codigo_municipio AND x.codigo_departamento_residencia = y.codigo_departamento
                    LEFT JOIN db_admon.departamento z
                    ON x.codigo_departamento_residencia = z.codigo_departamento
                    LEFT JOIN db_rrhh.area w
                    ON x.codigo_area = w.codigo_area
                    INNER JOIN db_rrhh.estado_empleado m
                    ON x.codigo_estado = m.codigo_estado_empleado
                    WHERE x.codigo_estado = @CodigoEstadoEmpleado
                      AND x.no_incluido_en_planilla = 0
                      AND x.cui NOT IN (SELECT cui FROM db_admon.usuario)";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoEmpleado", Constantes.Empleado.EstadoEmpleado.ACTIVO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            PersonaCLS objPersona;
                            lista = new List<PersonaCLS>();
                            int postCui = dr.GetOrdinal("cui");
                            int postPrimerNombre = dr.GetOrdinal("primer_nombre");
                            int postSegundoNombre = dr.GetOrdinal("segundo_nombre");
                            int postTercerNombre = dr.GetOrdinal("tercer_nombre");
                            int postPrimerApellido = dr.GetOrdinal("primer_apellido");
                            int postSegundoApellido = dr.GetOrdinal("segundo_apellido");
                            int postApellidoCasada = dr.GetOrdinal("apellido_casada");
                            int postNombreCompleto = dr.GetOrdinal("nombre_completo");
                            int postFechaNacimiento = dr.GetOrdinal("fecha_nacimiento");
                            int postFechaNacimientoStr = dr.GetOrdinal("fecha_nacimiento_str");
                            int postCodigoGenero = dr.GetOrdinal("codigo_genero");
                            int postGenero = dr.GetOrdinal("genero");
                            int postCorreoElectronico = dr.GetOrdinal("correo_electronico");
                            int postCodigoDepartamentoResidencia = dr.GetOrdinal("codigo_departamento_residencia");
                            int postDepartamentoResidencia = dr.GetOrdinal("departamento_residencia");
                            int postCodigoMunicipioResidencia = dr.GetOrdinal("codigo_municipio_residencia");
                            int postMunicipioResidencia = dr.GetOrdinal("municipio_residencia");
                            int postZona = dr.GetOrdinal("zona");
                            int postDireccionResidencia = dr.GetOrdinal("direccion_residencia");
                            int postNoIncluidoEnPlanilla = dr.GetOrdinal("no_incluido_en_planilla");
                            int postCodigoArea = dr.GetOrdinal("codigo_area");
                            int postArea = dr.GetOrdinal("area");
                            int postCodigoEstado = dr.GetOrdinal("codigo_estado");
                            int postEstado = dr.GetOrdinal("estado");
                            int postPermisoEditar = dr.GetOrdinal("permiso_editar");

                            while (dr.Read())
                            {
                                objPersona = new PersonaCLS();
                                objPersona.Cui = dr.GetString(postCui);
                                objPersona.PrimerNombre = dr.GetString(postPrimerNombre);
                                objPersona.SegundoNombre = dr.IsDBNull(postSegundoNombre) ? "" : dr.GetString(postSegundoNombre);
                                objPersona.TercerNombre = dr.IsDBNull(postTercerNombre) ? "" : dr.GetString(postTercerNombre);
                                objPersona.PrimerApellido = dr.GetString(postPrimerApellido);
                                objPersona.SegundoApellido = dr.IsDBNull(postSegundoApellido) ? "" : dr.GetString(postSegundoApellido);
                                objPersona.ApellidoCasada = dr.IsDBNull(postApellidoCasada) ? "" : dr.GetString(postApellidoCasada);
                                objPersona.NombreCompleto = dr.GetString(postNombreCompleto);
                                objPersona.FechaNacimiento = dr.GetDateTime(postFechaNacimiento);
                                objPersona.FechaNacimientoStr = dr.GetString(postFechaNacimientoStr);
                                objPersona.CodigoGenero = dr.GetString(postCodigoGenero);
                                objPersona.Genero = dr.GetString(postGenero);
                                objPersona.CorreoElectronico = dr.IsDBNull(postCorreoElectronico) ? "" : dr.GetString(postCorreoElectronico);
                                objPersona.CodigoDepartamentoResidencia = dr.GetInt16(postCodigoDepartamentoResidencia);
                                objPersona.DepartamentoResidencia = dr.GetString(postDepartamentoResidencia);
                                objPersona.CodigoMunicipioResidencia = dr.GetInt16(postCodigoMunicipioResidencia);
                                objPersona.MunicipioResidencia = dr.GetString(postMunicipioResidencia);
                                objPersona.Zona = dr.GetByte(postZona);
                                objPersona.DireccionResidencia = dr.IsDBNull(postDireccionResidencia) ? "" : dr.GetString(postDireccionResidencia);
                                objPersona.NoIncluidoEnPlanilla = dr.GetByte(postNoIncluidoEnPlanilla);
                                objPersona.CodigoArea = (short)dr.GetInt32(postCodigoArea);
                                objPersona.Area = dr.IsDBNull(postArea) ? "" : dr.GetString(postArea);
                                objPersona.CodigoEstado = dr.GetInt16(postCodigoEstado);
                                objPersona.Estado = dr.GetString(postEstado);
                                objPersona.PermisoEditar = dr.GetInt32(postPermisoEditar);
                                lista.Add(objPersona);
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
            }
            return lista;
        }

        public PersonaCLS GetDataPersona(string cui)
        {
            PersonaCLS objPersona = new PersonaCLS();
            using (SqlConnection conexion = new SqlConnection(cadenaRRHH))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT x.cui, 
                           x.primer_nombre,
                           x.segundo_nombre,
                           x.tercer_nombre,
                           x.primer_apellido,
                           x.segundo_apellido,
                           x.apellido_casada, 
		                   x.nombre_completo, 
		                   x.fecha_nacimiento, 
                           FORMAT(x.fecha_nacimiento,'dd/MM/yyyy') AS fecha_nacimiento_str,
		                   x.codigo_genero,
                           CASE
                             WHEN x.codigo_genero = 'M' THEN 'MASCULINO'
                             WHEN x.codigo_genero = 'F' THEN 'FEMENINO'
                             ELSE 'NO DEFINIDO'   
                           END AS genero, 
		                   x.correo_electronico,
		                   x.codigo_departamento_residencia,
		                   z.nombre AS departamento_residencia,
		                   x.codigo_municipio_residencia,
		                   y.nombre AS municipio_residencia,
		                   x.zona,
		                   x.direccion_residencia,
		                   x.no_incluido_en_planilla,
		                   COALESCE(x.codigo_area,0) AS codigo_area,
		                   w.nombre AS area,
		                   x.codigo_estado,
		                   m.nombre AS estado
                    FROM db_rrhh.persona x
                    LEFT JOIN db_admon.municipio y
                    ON x.codigo_municipio_residencia = y.codigo_municipio AND x.codigo_departamento_residencia = y.codigo_departamento
                    LEFT JOIN db_admon.departamento z
                    ON x.codigo_departamento_residencia = z.codigo_departamento
                    LEFT JOIN db_rrhh.area w
                    ON x.codigo_area = w.codigo_area
                    INNER JOIN db_rrhh.estado_empleado m
                    ON x.codigo_estado = m.codigo_estado_empleado
                    WHERE x.cui = @cui";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@cui", cui);
                        SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleResult);
                        if (dr != null)
                        {
                            int postCui = dr.GetOrdinal("cui");
                            int postPrimerNombre = dr.GetOrdinal("primer_nombre");
                            int postSegundoNombre = dr.GetOrdinal("segundo_nombre");
                            int postTercerNombre = dr.GetOrdinal("tercer_nombre");
                            int postPrimerApellido = dr.GetOrdinal("primer_apellido");
                            int postSegundoApellido = dr.GetOrdinal("segundo_apellido");
                            int postApellidoCasada = dr.GetOrdinal("apellido_casada");
                            int postNombreCompleto = dr.GetOrdinal("nombre_completo");
                            int postFechaNacimiento = dr.GetOrdinal("fecha_nacimiento");
                            int postFechaNacimientoStr = dr.GetOrdinal("fecha_nacimiento_str");
                            int postCodigoGenero = dr.GetOrdinal("codigo_genero");
                            int postGenero = dr.GetOrdinal("genero");
                            int postCorreoElectronico = dr.GetOrdinal("correo_electronico");
                            int postCodigoDepartamentoResidencia = dr.GetOrdinal("codigo_departamento_residencia");
                            int postDepartamentoResidencia = dr.GetOrdinal("departamento_residencia");
                            int postCodigoMunicipioResidencia = dr.GetOrdinal("codigo_municipio_residencia");
                            int postMunicipioResidencia = dr.GetOrdinal("municipio_residencia");
                            int postZona = dr.GetOrdinal("zona");
                            int postDireccionResidencia = dr.GetOrdinal("direccion_residencia");
                            int postNoIncluidoEnPlanilla = dr.GetOrdinal("no_incluido_en_planilla");
                            int postCodigoArea = dr.GetOrdinal("codigo_area");
                            int postArea = dr.GetOrdinal("area");
                            int postCodigoEstado = dr.GetOrdinal("codigo_estado");
                            int postEstado = dr.GetOrdinal("estado");

                            while (dr.Read())
                            {
                                objPersona = new PersonaCLS();
                                objPersona.Cui = dr.GetString(postCui);
                                objPersona.PrimerNombre = dr.GetString(postPrimerNombre);
                                objPersona.SegundoNombre = dr.IsDBNull(postSegundoNombre) ? "" : dr.GetString(postSegundoNombre);
                                objPersona.TercerNombre = dr.IsDBNull(postTercerNombre) ? "" : dr.GetString(postTercerNombre);
                                objPersona.PrimerApellido = dr.GetString(postPrimerApellido);
                                objPersona.SegundoApellido = dr.IsDBNull(postSegundoApellido) ? "" : dr.GetString(postSegundoApellido);
                                objPersona.ApellidoCasada = dr.IsDBNull(postApellidoCasada) ? "" : dr.GetString(postApellidoCasada);
                                objPersona.NombreCompleto = dr.GetString(postNombreCompleto);
                                objPersona.FechaNacimiento = dr.GetDateTime(postFechaNacimiento);
                                objPersona.FechaNacimientoStr = dr.GetString(postFechaNacimientoStr);
                                objPersona.CodigoGenero = dr.GetString(postCodigoGenero);
                                objPersona.Genero = dr.GetString(postGenero);
                                objPersona.CorreoElectronico = dr.IsDBNull(postCorreoElectronico) ? "" : dr.GetString(postCorreoElectronico);
                                objPersona.CodigoDepartamentoResidencia = dr.GetInt16(postCodigoDepartamentoResidencia);
                                objPersona.DepartamentoResidencia = dr.GetString(postDepartamentoResidencia);
                                objPersona.CodigoMunicipioResidencia = dr.GetInt16(postCodigoMunicipioResidencia);
                                objPersona.MunicipioResidencia = dr.GetString(postMunicipioResidencia);
                                objPersona.Zona = dr.GetByte(postZona);
                                objPersona.DireccionResidencia = dr.IsDBNull(postDireccionResidencia) ? "" : dr.GetString(postDireccionResidencia);
                                objPersona.NoIncluidoEnPlanilla = dr.GetByte(postNoIncluidoEnPlanilla);
                                objPersona.CodigoArea = (short)dr.GetInt32(postCodigoArea);
                                objPersona.Area = dr.IsDBNull(postArea) ? "" : dr.GetString(postArea);
                                objPersona.CodigoEstado = dr.GetInt16(postCodigoEstado);
                                objPersona.Estado = dr.GetString(postEstado);
                            }
                        }
                    }
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                    objPersona = null;
                }
            }
            return objPersona;
        }

        public string GuardarPersona(PersonaCLS objPersona, string usuarioIng)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaRRHH))
            {
                conexion.Open();
                SqlCommand cmd = conexion.CreateCommand();

                try
                {
                    string sentenciaSQL = @"
                    INSERT INTO db_rrhh.persona( cui,primer_nombre,segundo_nombre,tercer_nombre,primer_apellido,segundo_apellido,apellido_casada,nombre_completo,
	                                             fecha_nacimiento,codigo_genero,correo_electronico,codigo_departamento_residencia,codigo_municipio_residencia,
	                                             zona,direccion_residencia,codigo_estado,usuario_ing,fecha_ing,usuario_act,fecha_act,no_incluido_en_planilla,codigo_area)

                                        VALUES ( @Cui,
                                                 @PrimerNombre,
                                                 @SegundoNombre,
                                                 @TercerNombre,
                                                 @PrimerApellido,
                                                 @SegundoApellido,
                                                 @ApellidoCasada,
                                                 @NombreCompleto,
                                                 @FechaNacimiento,
	                                             @CodigoGenero,
                                                 @CorreoElectronico,
                                                 @CodigoDepartamentoResidencia,
                                                 @CodigoMunicipioResidencia,
                                                 @Zona,
                                                 @DireccionResidencia,
	                                             @CodigoEstado,
                                                 @UsuarioIng,
                                                 @FechaIng,
                                                 @UsuarioAct,
                                                 @FechaAct,
                                                 @NoIncluidoEnPlanilla,
                                                 @CodigoArea)";

                    cmd.CommandText = sentenciaSQL;
                    string nombreCompleto = objPersona.PrimerNombre + (objPersona.SegundoNombre == null ? "" : " " + objPersona.SegundoNombre) + (objPersona.TercerNombre == null ? "" : " " + objPersona.TercerNombre) + (objPersona.PrimerApellido == null ? "" : " " + objPersona.PrimerApellido) + (objPersona.SegundoApellido == null ? "" : " " + objPersona.SegundoApellido);
                    cmd.Parameters.AddWithValue("@Cui", objPersona.Cui);
                    cmd.Parameters.AddWithValue("@PrimerNombre", objPersona.PrimerNombre);
                    cmd.Parameters.AddWithValue("@SegundoNombre", objPersona.SegundoNombre == null ? DBNull.Value : objPersona.SegundoNombre);
                    cmd.Parameters.AddWithValue("@TercerNombre", objPersona.TercerNombre == null ? DBNull.Value : objPersona.TercerNombre);
                    cmd.Parameters.AddWithValue("@PrimerApellido", objPersona.PrimerApellido);
                    cmd.Parameters.AddWithValue("@SegundoApellido", objPersona.SegundoApellido == null ? DBNull.Value : objPersona.SegundoApellido);
                    cmd.Parameters.AddWithValue("@ApellidoCasada", objPersona.ApellidoCasada == null ? DBNull.Value : objPersona.ApellidoCasada);
                    cmd.Parameters.AddWithValue("@NombreCompleto", nombreCompleto);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", objPersona.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@CodigoGenero", objPersona.CodigoGenero);
                    cmd.Parameters.AddWithValue("@CorreoElectronico", objPersona.CorreoElectronico == null ? DBNull.Value : objPersona.CorreoElectronico);
                    cmd.Parameters.AddWithValue("@CodigoDepartamentoResidencia", objPersona.CodigoDepartamentoResidencia);
                    cmd.Parameters.AddWithValue("@CodigoMunicipioResidencia", objPersona.CodigoMunicipioResidencia);
                    cmd.Parameters.AddWithValue("@Zona", objPersona.Zona);
                    cmd.Parameters.AddWithValue("@DireccionResidencia", objPersona.DireccionResidencia);
                    cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.EstadoRegistro.ACTIVO);
                    cmd.Parameters.AddWithValue("@UsuarioIng", usuarioIng);
                    cmd.Parameters.AddWithValue("@FechaIng", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UsuarioAct", DBNull.Value);
                    cmd.Parameters.AddWithValue("@FechaAct",DBNull.Value);
                    cmd.Parameters.AddWithValue("@NoIncluidoEnPlanilla", objPersona.NoIncluidoEnPlanilla);
                    cmd.Parameters.AddWithValue("@CodigoArea", objPersona.CodigoArea == -1 ? DBNull.Value : objPersona.CodigoArea);

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

        public string GuardarPersonaIndirecta(PersonaCLS objPersona, string usuarioIng)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaRRHH))
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
                    string sqlSequence = "SELECT NEXT VALUE FOR db_rrhh.SQ_CUI_GENERICO";
                    cmd.CommandText = sqlSequence;
                    long codigo = (long)cmd.ExecuteScalar();
                    string cui = codigo.ToString("D13");

                    string sentenciaSQL = @"
                    INSERT INTO db_rrhh.persona( cui,primer_nombre,segundo_nombre,tercer_nombre,primer_apellido,segundo_apellido,apellido_casada,nombre_completo,
	                                             fecha_nacimiento,codigo_genero,correo_electronico,codigo_departamento_residencia,codigo_municipio_residencia,
	                                             zona,direccion_residencia,codigo_estado,usuario_ing,fecha_ing,usuario_act,fecha_act,no_incluido_en_planilla,codigo_area)

                                        VALUES ( @Cui,
                                                 @PrimerNombre,
                                                 @SegundoNombre,
                                                 @TercerNombre,
                                                 @PrimerApellido,
                                                 @SegundoApellido,
                                                 @ApellidoCasada,
                                                 @NombreCompleto,
                                                 @FechaNacimiento,
	                                             @CodigoGenero,
                                                 @CorreoElectronico,
                                                 @CodigoDepartamentoResidencia,
                                                 @CodigoMunicipioResidencia,
                                                 @Zona,
                                                 @DireccionResidencia,
	                                             @CodigoEstado,
                                                 @UsuarioIng,
                                                 @FechaIng,
                                                 @UsuarioAct,
                                                 @FechaAct,
                                                 @NoIncluidoEnPlanilla,
                                                 @CodigoArea)";

                    cmd.CommandText = sentenciaSQL;
                    string nombreCompleto = objPersona.PrimerNombre + (objPersona.SegundoNombre == null ? "" : " " + objPersona.SegundoNombre) + (objPersona.TercerNombre == null ? "" : " " + objPersona.TercerNombre) + (objPersona.PrimerApellido == null ? "" : " " + objPersona.PrimerApellido) + (objPersona.SegundoApellido == null ? "" : " " + objPersona.SegundoApellido);
                    cmd.Parameters.AddWithValue("@Cui", cui);
                    cmd.Parameters.AddWithValue("@PrimerNombre", objPersona.PrimerNombre);
                    cmd.Parameters.AddWithValue("@SegundoNombre", objPersona.SegundoNombre == null ? DBNull.Value : objPersona.SegundoNombre);
                    cmd.Parameters.AddWithValue("@TercerNombre", objPersona.TercerNombre == null ? DBNull.Value : objPersona.TercerNombre);
                    cmd.Parameters.AddWithValue("@PrimerApellido", objPersona.PrimerApellido);
                    cmd.Parameters.AddWithValue("@SegundoApellido", objPersona.SegundoApellido == null ? DBNull.Value : objPersona.SegundoApellido);
                    cmd.Parameters.AddWithValue("@ApellidoCasada", objPersona.ApellidoCasada == null ? DBNull.Value : objPersona.ApellidoCasada);
                    cmd.Parameters.AddWithValue("@NombreCompleto", nombreCompleto);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", new DateTime(2030, 1, 1));
                    cmd.Parameters.AddWithValue("@CodigoGenero", objPersona.CodigoGenero);
                    cmd.Parameters.AddWithValue("@CorreoElectronico", objPersona.CorreoElectronico == null ? DBNull.Value : objPersona.CorreoElectronico);
                    cmd.Parameters.AddWithValue("@CodigoDepartamentoResidencia", Constantes.Departamento.GUATEMALA);
                    cmd.Parameters.AddWithValue("@CodigoMunicipioResidencia", Constantes.Municipio.GUATEMALA);
                    cmd.Parameters.AddWithValue("@Zona", 0);
                    cmd.Parameters.AddWithValue("@DireccionResidencia", objPersona.DireccionResidencia == null ? "" : objPersona.DireccionResidencia);
                    cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.EstadoRegistro.ACTIVO);
                    cmd.Parameters.AddWithValue("@UsuarioIng", usuarioIng);
                    cmd.Parameters.AddWithValue("@FechaIng", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UsuarioAct", DBNull.Value);
                    cmd.Parameters.AddWithValue("@FechaAct", DBNull.Value);
                    cmd.Parameters.AddWithValue("@NoIncluidoEnPlanilla", objPersona.NoIncluidoEnPlanilla);
                    cmd.Parameters.AddWithValue("@CodigoArea", objPersona.CodigoArea == -1 ? DBNull.Value : objPersona.CodigoArea);
                    cmd.ExecuteNonQuery();

                    transaction.Commit();
                    conexion.Close();

                    resultado = cui;
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    resultado = "Error [0]: " + ex.Message;
                }
            }

            return resultado;
        }

        public string ActualizarPersona(PersonaCLS objPersona, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaRRHH))
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
                    string sentenciaInsertHistorial = @"
                    INSERT INTO db_rrhh.persona_hist
                    SELECT NEXT VALUE FOR db_rrhh.SQ_CODIGO_HIST_PERSONA,
                           cui,
	                       primer_nombre,
	                       segundo_nombre,
	                       tercer_nombre,
	                       primer_apellido,
	                       segundo_apellido,
	                       apellido_casada,
	                       nombre_completo,	
	                       fecha_nacimiento,
	                       codigo_genero,
	                       correo_electronico,
	                       codigo_departamento_residencia,
	                       codigo_municipio_residencia,
	                       zona,
	                       direccion_residencia,		
	                       codigo_estado,
                           @UsuarioIng AS usuario_ing,
                           @FechaIng AS fecha_ing,
                           NULL AS usuario_act,
                           NULL AS fecha_act,
	                       no_incluido_en_planilla,
	                       codigo_area		
                    FROM db_rrhh.persona
                    WHERE cui = @Cui";

                    cmd.CommandText = sentenciaInsertHistorial;
                    cmd.Parameters.AddWithValue("@Cui", objPersona.Cui);
                    cmd.Parameters.AddWithValue("@UsuarioIng", usuarioAct);
                    cmd.Parameters.AddWithValue("@FechaIng", DateTime.Now);
                    cmd.ExecuteNonQuery();


                    string sentenciaSQL = @"
                    UPDATE db_rrhh.persona
                    SET primer_nombre = @PrimerNombre,
                        segundo_nombre = @SegundoNombre,
                        tercer_nombre = @TercerNombre,
                        primer_apellido = @PrimerApellido,
                        segundo_apellido = @SegundoApellido,
                        apellido_casada = @ApellidoCasada,
                        nombre_completo = @NombreCompleto,
	                    fecha_nacimiento = @FechaNacimiento,
                        codigo_genero = @CodigoGenero,
                        correo_electronico = @CorreoElectronico,
                        codigo_departamento_residencia = @CodigoDepartamentoResidencia,
                        codigo_municipio_residencia = @CodigoMunicipioResidencia,
	                    zona = @Zona,
                        direccion_residencia = @DireccionResidencia,
                        usuario_act = @UsuarioAct,
                        fecha_act = @FechaAct,
                        no_incluido_en_planilla = @NoIncluidoEnPlanilla,
                        codigo_area = @CodigoArea
                    WHERE cui = @Cui";

                    cmd.CommandText = sentenciaSQL;
                    string nombreCompleto = objPersona.PrimerNombre + (objPersona.SegundoNombre == null ? "" : " " + objPersona.SegundoNombre) + (objPersona.TercerNombre == null ? "" : " " + objPersona.TercerNombre) + (objPersona.PrimerApellido == null ? "" : " " + objPersona.PrimerApellido) + (objPersona.SegundoApellido == null ? "" : " " + objPersona.SegundoApellido);
                    //cmd.Parameters.AddWithValue("@Cui", objPersona.Cui);
                    cmd.Parameters.AddWithValue("@PrimerNombre", objPersona.PrimerNombre);
                    cmd.Parameters.AddWithValue("@SegundoNombre", objPersona.SegundoNombre == null ? DBNull.Value : objPersona.SegundoNombre);
                    cmd.Parameters.AddWithValue("@TercerNombre", objPersona.TercerNombre == null ? DBNull.Value : objPersona.TercerNombre);
                    cmd.Parameters.AddWithValue("@PrimerApellido", objPersona.PrimerApellido);
                    cmd.Parameters.AddWithValue("@SegundoApellido", objPersona.SegundoApellido == null ? DBNull.Value : objPersona.SegundoApellido);
                    cmd.Parameters.AddWithValue("@ApellidoCasada", objPersona.ApellidoCasada == null ? DBNull.Value : objPersona.ApellidoCasada);
                    cmd.Parameters.AddWithValue("@NombreCompleto", nombreCompleto);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", objPersona.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@CodigoGenero", objPersona.CodigoGenero);
                    cmd.Parameters.AddWithValue("@CorreoElectronico", objPersona.CorreoElectronico == null ? DBNull.Value : objPersona.CorreoElectronico);
                    cmd.Parameters.AddWithValue("@CodigoDepartamentoResidencia", objPersona.CodigoDepartamentoResidencia);
                    cmd.Parameters.AddWithValue("@CodigoMunicipioResidencia", objPersona.CodigoMunicipioResidencia);
                    cmd.Parameters.AddWithValue("@Zona", objPersona.Zona);
                    cmd.Parameters.AddWithValue("@DireccionResidencia", objPersona.DireccionResidencia);
                    cmd.Parameters.AddWithValue("@UsuarioAct", usuarioAct);
                    cmd.Parameters.AddWithValue("@FechaAct", DateTime.Now);
                    cmd.Parameters.AddWithValue("@NoIncluidoEnPlanilla", objPersona.NoIncluidoEnPlanilla);
                    cmd.Parameters.AddWithValue("@CodigoArea", objPersona.CodigoArea == -1 ? DBNull.Value : objPersona.CodigoArea);
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
