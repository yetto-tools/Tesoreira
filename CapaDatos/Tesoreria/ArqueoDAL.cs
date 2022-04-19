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
    public class ArqueoDAL: CadenaConexion
    {
        public List<ReporteCajaCLS> GetListaArqueos(string usuarioGeneracion)
        {
            List<ReporteCajaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    SELECT  COALESCE(x.codigo_reporte,0) AS codigo_reporte, 
	                       x.anio_operacion, 
	                       x.semana_operacion,
	                       db_admon.GetPeriodoSemana(x.anio_operacion,x.semana_operacion) AS semana,
                           '' AS observaciones,
	                       1 AS codigo_estado, 
	                       'Por Generar' AS estado,
                           @UsuarioGeneracion AS usuario_ing,
	                       GETDATE() AS fecha_ing,
                           FORMAT(GETDATE(), 'dd/MM/yyyy, hh:mm:ss') AS fecha_ing_str,
                           0 AS bloqueado, 
                           0 AS permiso_arqueo, 
                           0 AS permiso_anular,
                           0 AS permiso_editar
                            
                    FROM ( SELECT anio_operacion, semana_operacion, codigo_reporte
                           FROM db_tesoreria.transaccion
                           WHERE codigo_estado = @CodigoEstadoRegistrado 
                             AND complemento_conta = 0
                           GROUP BY anio_operacion, semana_operacion, codigo_reporte
                         ) x

                    UNION    

                    SELECT x.codigo_reporte, 
                           x.anio AS anio_operacion, 
                           x.numero_semana AS semana_operacion,
                           db_admon.GetPeriodoSemana(x.anio,x.numero_semana) AS semana, 
                           x.observaciones, 
                           x.codigo_estado,
                           y.nombre AS estado,
                           x.usuario_ing, 
                           x.fecha_ing,
                           FORMAT(x.fecha_ing, 'dd/MM/yyyy, hh:mm:ss') AS fecha_ing_str,
                           1 AS bloqueado,
                           1 AS permiso_arqueo, 
                           CASE  
                             WHEN x.codigo_estado = @CodigoEstadoGenerado THEN 1
                             ELSE 0
                            END AS permiso_anular,
                            CASE  
                             WHEN x.codigo_estado = @CodigoEstadoGenerado THEN 1
                             ELSE 0
                            END AS permiso_editar
                    FROM db_tesoreria.reporte_caja x
                    INNER JOIN db_tesoreria.estado_reporte_caja y
                    ON x.codigo_estado = y.codigo_estado_reporte_caja
                    WHERE x.codigo_estado = @CodigoEstadoGenerado
                      AND x.arqueo = 1";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoRegistrado", Constantes.EstadoTransacccion.REGISTRADO);
                        cmd.Parameters.AddWithValue("@UsuarioGeneracion", usuarioGeneracion);
                        cmd.Parameters.AddWithValue("@CodigoEstadoGenerado", Constantes.ReporteCaja.Estado.GENERADO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            ReporteCajaCLS objReporteCaja;
                            lista = new List<ReporteCajaCLS>();
                            int postCodigoReporte = dr.GetOrdinal("codigo_reporte");
                            int postAnio = dr.GetOrdinal("anio_operacion");
                            int postNumeroSemana = dr.GetOrdinal("semana_operacion");
                            int postSemana = dr.GetOrdinal("semana");
                            int postObservaciones = dr.GetOrdinal("observaciones");
                            int postCodigoEstado = dr.GetOrdinal("codigo_estado");
                            int postEstado = dr.GetOrdinal("estado");
                            int postUsuarioIng = dr.GetOrdinal("usuario_ing");
                            int postFechaIng = dr.GetOrdinal("fecha_ing");
                            int postFechaIngStr = dr.GetOrdinal("fecha_ing_str");
                            int postBloqueado = dr.GetOrdinal("bloqueado");
                            int postPermisoEditar = dr.GetOrdinal("permiso_editar");
                            int postPermisoAnular = dr.GetOrdinal("permiso_anular");
                            int postPermisoArqueo = dr.GetOrdinal("permiso_arqueo");
                            while (dr.Read())
                            {
                                objReporteCaja = new ReporteCajaCLS();
                                objReporteCaja.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objReporteCaja.Anio = dr.GetInt16(postAnio);
                                objReporteCaja.NumeroSemana = dr.GetByte(postNumeroSemana);
                                objReporteCaja.Semana = dr.GetString(postSemana);
                                objReporteCaja.Observaciones = dr.IsDBNull(postObservaciones) ? "" : dr.GetString(postObservaciones);
                                objReporteCaja.CodigoEstado = (byte)dr.GetInt32(postCodigoEstado);
                                objReporteCaja.Estado = dr.GetString(postEstado);
                                objReporteCaja.UsuarioIng = dr.GetString(postUsuarioIng);
                                objReporteCaja.FechaIng = dr.GetDateTime(postFechaIng);
                                objReporteCaja.FechaIngStr = dr.GetString(postFechaIngStr);
                                objReporteCaja.bloqueado = (byte)dr.GetInt32(postBloqueado);
                                objReporteCaja.PermisoEditar = (byte)dr.GetInt32(postPermisoEditar);
                                objReporteCaja.PermisoAnular = (byte)dr.GetInt32(postPermisoAnular);
                                objReporteCaja.PermisoArqueo = (byte)dr.GetInt32(postPermisoArqueo);

                                lista.Add(objReporteCaja);
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

        public string GenerarArqueo(int anio, int numeroSemana, string usuarioIng)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_tesoreria.uspGenerarArqueo", conexion))
                    {
                        // le indico que es un procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Anio", anio);
                        cmd.Parameters.AddWithValue("@NumeroSemana", numeroSemana);
                        cmd.Parameters.AddWithValue("@UsuarioIng", usuarioIng);

                        //Set SqlParameter
                        SqlParameter outParameter = new SqlParameter
                        {
                            ParameterName = "@Resultado", //Parameter name defined in stored procedure
                            //SqlDbType = SqlDbType.Int, //Data Type of Parameter
                            Size = 2000,
                            SqlDbType = SqlDbType.VarChar, //Data Type of Parameter
                            Direction = ParameterDirection.Output //Specify the parameter as ouput
                        };

                        //add the parameter to the SqlCommand object
                        cmd.Parameters.Add(outParameter);
                        cmd.ExecuteReader();
                        //resultado = (string)cmd.Parameters["@Resultado"].Value.ToString();
                        //resultado = outParameter.Value.ToString().Trim();
                        resultado = outParameter.Value.ToString();
                        conexion.Close();
                    }// fin using
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    resultado = "Error [0]: " + ex.Message;
                }
                return resultado;
            }
        }
    }
}
