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
    public class ConfiguracionSueldoIndirectoDAL: CadenaConexion
    {
        public string GuardarConfiguracion(ConfiguracionSueldoIndirectoCLS objConfiguracion, string usuarioIng)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                string sentenciaSQL = @"
                INSERT INTO db_tesoreria.config_sueldos_indirectos(anio, mes, monto, estado, usuario_ing, fecha_ing)
                VALUES (@Anio,@Mes,@Monto,@CodigoEstado,@UsuarioIng,@FechaIng)";

                conexion.Open();
                try
                {
                    SqlCommand cmd = conexion.CreateCommand();
                    cmd.CommandText = sentenciaSQL;
                    cmd.Parameters.AddWithValue("@Anio", objConfiguracion.Anio);
                    cmd.Parameters.AddWithValue("@Mes", objConfiguracion.Mes);
                    cmd.Parameters.AddWithValue("@Monto", objConfiguracion.Monto);
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

        public string ActualizarConfiguracion(ConfiguracionSueldoIndirectoCLS objConfiguracion, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                string sentenciaSQL = @"
                UPDATE db_tesoreria.config_sueldos_indirectos
                SET monto = @Monto
                WHERE anio = @Anio 
                  AND mes = @Mes"; 
                conexion.Open();
                try
                {
                    SqlCommand cmd = conexion.CreateCommand();
                    cmd.CommandText = sentenciaSQL;
                    cmd.Parameters.AddWithValue("@Anio", objConfiguracion.Anio);
                    cmd.Parameters.AddWithValue("@Mes", objConfiguracion.Mes);
                    cmd.Parameters.AddWithValue("@Monto", objConfiguracion.Monto);
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


        public List<ConfiguracionSueldoIndirectoCLS> GetConfiguracionSueldoIndirecto(int anio)
        {
            List<ConfiguracionSueldoIndirectoCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    SELECT anio, 
	                       mes, 
	                       CASE
		                      WHEN mes = 1 THEN 'ENERO'
		                      WHEN mes = 2 THEN 'FEBRERO'
		                      WHEN mes = 3 THEN 'MARZO'
		                      WHEN mes = 4 THEN 'ABRIL'
		                      WHEN mes = 5 THEN 'MAYO'
		                      WHEN mes = 6 THEN 'JUNIO'
		                      WHEN mes = 7 THEN 'JULIO'
		                      WHEN mes = 8 THEN 'AGOSTO'
		                      WHEN mes = 9 THEN 'SEPTIEMBRE'
		                      WHEN mes = 10 THEN 'OCTUBRE'
		                      WHEN mes = 11 THEN 'NOVIEMBRE'
		                      WHEN mes = 12 THEN 'DICIEMBRE'
		                    ELSE 'NO DEFINIDO'
	                       END AS nombre_mes,
	                       monto, 
	                       estado AS codigo_estado,
                           CASE
                              WHEN estado = 1 THEN 'ACTIVO'
                              ELSE 'BLOQUEADO'
                           END AS estado,
                           1 AS permiso_editar
                    FROM db_tesoreria.config_sueldos_indirectos
                    WHERE anio = @Anio
                    ORDER BY mes DESC";
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Anio", anio);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            ConfiguracionSueldoIndirectoCLS objConfigSueldoIndirecto;
                            lista = new List<ConfiguracionSueldoIndirectoCLS>();
                            int postAnio = dr.GetOrdinal("anio");
                            int postMes = dr.GetOrdinal("mes");
                            int postNombreMes = dr.GetOrdinal("nombre_mes");
                            int postMonto = dr.GetOrdinal("monto");
                            int postCodigoEstado = dr.GetOrdinal("codigo_estado");
                            int postEstado = dr.GetOrdinal("estado");
                            int postPermisoEditar = dr.GetOrdinal("permiso_editar");
                            while (dr.Read())
                            {
                                objConfigSueldoIndirecto = new ConfiguracionSueldoIndirectoCLS();
                                objConfigSueldoIndirecto.Anio = dr.GetInt16(postAnio);
                                objConfigSueldoIndirecto.Mes = dr.GetByte(postMes);
                                objConfigSueldoIndirecto.NombreMes = dr.GetString(postNombreMes);
                                objConfigSueldoIndirecto.Monto = dr.GetDecimal(postMonto);
                                objConfigSueldoIndirecto.CodigoEstado = dr.GetByte(postCodigoEstado);
                                objConfigSueldoIndirecto.Estado = dr.GetString(postEstado);
                                objConfigSueldoIndirecto.PermisoEditar = (byte)dr.GetInt32(postPermisoEditar);
                                lista.Add(objConfigSueldoIndirecto);
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
