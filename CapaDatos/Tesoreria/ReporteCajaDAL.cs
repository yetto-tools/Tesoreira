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
    public class ReporteCajaDAL : CadenaConexion
    {
        public List<ReporteCajaCLS> GetReportesSemanalesCajaGeneracion(string usuarioGeneracion)
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
                      AND x.arqueo = 0";

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

        public List<ReporteCajaCLS> GetReportesSemanalesCajaGeneracionTemporal(string usuarioGeneracion, int semanaOculta)
        {
            string filterSemanaOcultaTransaccion = String.Empty;
            string filterSemanaOcultaReporte = String.Empty;
            if (semanaOculta > 0)
            {
                filterSemanaOcultaTransaccion = " AND semana_operacion <> " + semanaOculta.ToString();
                filterSemanaOcultaReporte = " AND x.numero_semana <> " + semanaOculta.ToString();
            }

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
                           0 AS permiso_anular,
                           0 AS permiso_editar
                            
                    FROM ( SELECT anio_operacion, semana_operacion, codigo_reporte
                           FROM db_tesoreria.transaccion
                           WHERE codigo_estado = @CodigoEstadoRegistrado 
                             AND complemento_conta = 0
                           " + filterSemanaOcultaTransaccion + @" 
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
                    " + filterSemanaOcultaReporte + @"
                      AND x.arqueo = 0";

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

        public List<ReporteCajaCLS> GetReportesSemanalesEnProcesoDeGeneracion(string usuarioGeneracion, int anioOperacion, int semanaOperacion)
        {
            string filterSemanaOcultaTransaccion = String.Empty;

            List<ReporteCajaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    //AND fecha_operacion > DATEADD(week, -2, GETDATE())

                    string sql = @"
                    SELECT 0 AS codigo_reporte, 
	                       x.anio_operacion, 
	                       x.semana_operacion,
	                       db_admon.GetPeriodoSemana(x.anio_operacion,x.semana_operacion) AS semana,
                           '' AS observaciones,
	                       1 AS codigo_estado, 
	                       'Por Generar' AS estado,
                           @UsuarioGeneracion AS usuario_ing,
	                       GETDATE() AS fecha_ing,
                           FORMAT(GETDATE(), 'dd/MM/yyyy, hh:mm:ss') AS fecha_ing_str,
                           1 AS permiso_ver_reporte
                    FROM ( SELECT y.anio_operacion, y.semana_operacion
                           FROM db_tesoreria.transaccion y
                           WHERE y.codigo_estado in (@CodigoEstadoRegistrado,@CodigoEstadoGenerado)
                             AND y.complemento_conta = 0
                             AND (SELECT COALESCE((SELECT TOP (1) codigo_estado FROM db_tesoreria.reporte_caja WHERE anio = y.anio_operacion AND numero_semana = y.semana_operacion and codigo_estado IN (3,4)),0)) = 0
                           GROUP BY anio_operacion, semana_operacion
                         ) x";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoRegistrado", Constantes.EstadoTransacccion.REGISTRADO);
                        cmd.Parameters.AddWithValue("@CodigoEstadoGenerado", Constantes.ReporteCaja.Estado.GENERADO);
                        cmd.Parameters.AddWithValue("@UsuarioGeneracion", usuarioGeneracion);

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
                            int postPermisoVerReporte = dr.GetOrdinal("permiso_ver_reporte");
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
                                objReporteCaja.PermisoVerReporte = (byte)dr.GetInt32(postPermisoVerReporte);

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

        public List<ReporteCajaCLS> GetReportesSemanalesCajaParaVistoBueno()
        {
            List<ReporteCajaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
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
                           1 AS permiso_anular,
                           1 AS permiso_editar
                    FROM db_tesoreria.reporte_caja x
                    INNER JOIN db_tesoreria.estado_reporte_caja y
                    ON x.codigo_estado = y.codigo_estado_reporte_caja
                    WHERE x.codigo_estado = @CodigoEstadoPorRevisar";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoPorRevisar", Constantes.ReporteCaja.Estado.POR_REVISAR);
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
                            while (dr.Read())
                            {
                                objReporteCaja = new ReporteCajaCLS();
                                objReporteCaja.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objReporteCaja.Anio = dr.GetInt16(postAnio);
                                objReporteCaja.NumeroSemana = dr.GetByte(postNumeroSemana);
                                objReporteCaja.Semana = dr.GetString(postSemana);
                                objReporteCaja.Observaciones = dr.IsDBNull(postObservaciones) ? "" : dr.GetString(postObservaciones);
                                objReporteCaja.CodigoEstado = dr.GetByte(postCodigoEstado);
                                objReporteCaja.Estado = dr.GetString(postEstado);
                                objReporteCaja.UsuarioIng = dr.GetString(postUsuarioIng);
                                objReporteCaja.FechaIng = dr.GetDateTime(postFechaIng);
                                objReporteCaja.FechaIngStr = dr.GetString(postFechaIngStr);
                                objReporteCaja.bloqueado = (byte)dr.GetInt32(postBloqueado);
                                objReporteCaja.PermisoEditar = (byte)dr.GetInt32(postPermisoEditar);
                                objReporteCaja.PermisoAnular = (byte)dr.GetInt32(postPermisoAnular);

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

        public List<ReporteCajaCLS> GetReportesSemanalesCaja(int anio)
        {
            List<ReporteCajaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    SELECT x.codigo_reporte, 
                           x.anio AS anio_operacion, 
                           x.numero_semana AS semana_operacion,
                           db_admon.GetPeriodoSemana(x.anio,x.numero_semana) AS semana, 
                           x.observaciones, 
                           x.codigo_estado,
                           y.nombre AS estado,
                           x.usuario_ing, 
                           x.fecha_ing,
                           FORMAT(x.fecha_ing, 'dd/MM/yyyy, hh:mm:ss') AS fecha_ing_str
                    FROM db_tesoreria.reporte_caja x
                    INNER JOIN db_tesoreria.estado_reporte_caja y
                    ON x.codigo_estado = y.codigo_estado_reporte_caja
                    WHERE x.anio = @Anio 
                      AND x.codigo_estado IN (@CodigoEstadoPorRevisar,@CodigoEstadoVistoBueno)
                      AND x.arqueo = 0";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Anio", anio);
                        cmd.Parameters.AddWithValue("@CodigoEstadoPorRevisar", Constantes.ReporteCaja.Estado.POR_REVISAR);
                        cmd.Parameters.AddWithValue("@CodigoEstadoVistoBueno", Constantes.ReporteCaja.Estado.VISTO_BUENO);
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
                            while (dr.Read())
                            {
                                objReporteCaja = new ReporteCajaCLS();
                                objReporteCaja.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objReporteCaja.Anio = dr.GetInt16(postAnio);
                                objReporteCaja.NumeroSemana = dr.GetByte(postNumeroSemana);
                                objReporteCaja.Semana = dr.GetString(postSemana);
                                objReporteCaja.Observaciones = dr.IsDBNull(postObservaciones) ? "" : dr.GetString(postObservaciones);
                                objReporteCaja.CodigoEstado = dr.GetByte(postCodigoEstado);
                                objReporteCaja.Estado = dr.GetString(postEstado);
                                objReporteCaja.UsuarioIng = dr.GetString(postUsuarioIng);
                                objReporteCaja.FechaIng = dr.GetDateTime(postFechaIng);
                                objReporteCaja.FechaIngStr = dr.GetString(postFechaIngStr);

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

        public string GenerarReporteSemanal(int anio, int numeroSemana, string idUsuario)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_tesoreria.uspGenerarReporteCaja", conexion))
                    {
                        // le indico que es un procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Anio", anio);
                        cmd.Parameters.AddWithValue("@NumeroSemana", numeroSemana);
                        cmd.Parameters.AddWithValue("@UsuarioIng", idUsuario);

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

        public string EliminarReporteSemanal(int codigoReporte, int anioOperacion, int semanaOperacion, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                conexion.Open();

                SqlCommand cmd = conexion.CreateCommand();
                SqlTransaction transaction;

                // Start a local transaction.
                transaction = conexion.BeginTransaction("TransactionPropia");

                // Must assign both transaction object and connection
                // to Command object for a pending local transaction
                cmd.Connection = conexion;
                cmd.Transaction = transaction;
                try
                {
                    string sqlDeleteDetalleReporte = "DELETE FROM db_tesoreria.reporte_caja_detalle WHERE codigo_reporte = @CodigoReporte";

                    cmd.CommandText = sqlDeleteDetalleReporte;
                    cmd.Parameters.AddWithValue("@CodigoReporte", codigoReporte);
                    cmd.ExecuteNonQuery();

                    string sqlUpdateReporte = @"
                    UPDATE db_tesoreria.reporte_caja 
                    SET codigo_estado = @CodigoEstadoAnulado,
                        usuario_act = @UsuarioAct,
                        fecha_act = @FechaAct
                    WHERE codigo_reporte = @CodigoReporte";
                    cmd.CommandText = sqlUpdateReporte;
                    cmd.Parameters.AddWithValue("@CodigoEstadoAnulado", Constantes.ReporteCaja.Estado.ANULADO);
                    cmd.Parameters.AddWithValue("@UsuarioAct", usuarioAct);
                    cmd.Parameters.AddWithValue("@FechaAct", DateTime.Now);
                    cmd.ExecuteNonQuery();

                    string sqlUpdateTransaccion = @"
                    UPDATE db_tesoreria.transaccion 
                    SET codigo_estado = @CodigoEstadoRegistrado,
                        codigo_reporte = null
                    WHERE anio_operacion = @AnioOperacion 
                      AND semana_operacion = @SemanaOperacion
                      AND codigo_reporte = @CodigoReporte"; 
                    cmd.CommandText = sqlUpdateTransaccion;
                    cmd.Parameters.AddWithValue("@CodigoEstadoRegistrado", Constantes.EstadoTransacccion.REGISTRADO);
                    cmd.Parameters.AddWithValue("@AnioOperacion", anioOperacion);
                    cmd.Parameters.AddWithValue("@SemanaOperacion", semanaOperacion);
                    cmd.ExecuteNonQuery();

                    // Attempt to commit the transaction.
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

        public string ExisteReporte(int anio, int numeroSemana)
        {
            string existeReporte = "1";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT COUNT(*) AS cantidad 
                    FROM db_tesoreria.reporte_caja
                    WHERE anio = @Anio 
                      AND numero_semana = @NumeroSemana
                      AND codigo_estado <> 0";
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Anio", anio);
                        cmd.Parameters.AddWithValue("@NumeroSemana", numeroSemana);
                        int resultado = (int)cmd.ExecuteScalar();
                        if (resultado == 0)
                            existeReporte = "0";
                    }
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                }
            }
            return existeReporte;
        }

        public string AceptarReporteGenerado(int codigoReporte, int anioOperacion, int semanaOperacion, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_tesoreria.uspVistoBuenoReporteCaja", conexion))
                    {
                        // le indico que es un procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CodigoReporte", codigoReporte);
                        cmd.Parameters.AddWithValue("@AnioOperacion", anioOperacion);
                        cmd.Parameters.AddWithValue("@SemanaOperacion", semanaOperacion);
                        cmd.Parameters.AddWithValue("@UsuarioAct", usuarioAct);

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
            }// fin using
        }

        public string AceptarReportePorContabilidad(int codigoReporte, int anioOperacion, int semanaOperacion, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_tesoreria.uspVistoBuenoReporteCajaContabilidad", conexion))
                    {
                        // le indico que es un procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CodigoReporte", codigoReporte);
                        cmd.Parameters.AddWithValue("@AnioOperacion", anioOperacion);
                        cmd.Parameters.AddWithValue("@SemanaOperacion", semanaOperacion);
                        cmd.Parameters.AddWithValue("@UsuarioAct", usuarioAct);

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
            }// fin using
        }

        public List<ReporteCajaCLS> GetReportesCajaEnProceso(int anioOperacion, int semanaOperacion, int esSuperAdmin)
        {
            List<ReporteCajaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string filterEstadoReporte = String.Empty;
                    if (esSuperAdmin == 0) {
                        filterEstadoReporte = " AND codigo_estado = " + Constantes.ReporteCaja.Estado.POR_REVISAR.ToString();
                    }
                    string sql = @"
                    SELECT codigo_reporte,
                           CONCAT(FORMAT(fecha_ing, 'dd/MM/yyyy, hh:mm:ss'),' [',codigo_reporte,']') AS fecha_corte_str
                    FROM db_tesoreria.reporte_caja
                    WHERE codigo_estado <> 0
                      AND anio = @AnioOperacion
                      AND numero_semana = @SemanaOperacion
                      AND arqueo = 0
                    " + filterEstadoReporte;

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@AnioOperacion", anioOperacion);
                        cmd.Parameters.AddWithValue("@SemanaOperacion", semanaOperacion);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            ReporteCajaCLS objReporteCaja;
                            lista = new List<ReporteCajaCLS>();
                            int postCodigoReporte = dr.GetOrdinal("codigo_reporte");
                            int postFechaCorteStr = dr.GetOrdinal("fecha_corte_str");
                            while (dr.Read())
                            {
                                objReporteCaja = new ReporteCajaCLS();
                                objReporteCaja.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objReporteCaja.FechaCorteStr = dr.GetString(postFechaCorteStr);
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

        public List<ReporteCajaCLS> GetReportesCajaConsulta(int anioOperacion, int semanaOperacion)
        {
            List<ReporteCajaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    SELECT codigo_reporte,
                           CONCAT(FORMAT(fecha_ing, 'dd/MM/yyyy, hh:mm:ss'),' [',codigo_reporte,']') AS fecha_corte_str
                    FROM db_tesoreria.reporte_caja
                    WHERE codigo_estado <> @CodigoEstadoReporteAnulado
                      AND anio = @AnioOperacion
                      AND numero_semana = @SemanaOperacion
                      AND arqueo = 0";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoReporteAnulado", Constantes.ReporteCaja.Estado.ANULADO);
                        cmd.Parameters.AddWithValue("@AnioOperacion", anioOperacion);
                        cmd.Parameters.AddWithValue("@SemanaOperacion", semanaOperacion);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            ReporteCajaCLS objReporteCaja;
                            lista = new List<ReporteCajaCLS>();
                            int postCodigoReporte = dr.GetOrdinal("codigo_reporte");
                            int postFechaCorteStr = dr.GetOrdinal("fecha_corte_str");
                            while (dr.Read())
                            {
                                objReporteCaja = new ReporteCajaCLS();
                                objReporteCaja.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objReporteCaja.FechaCorteStr = dr.GetString(postFechaCorteStr);
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

        public List<ReporteCajaCLS> GetReportesCaja(int anioOperacion, int semanaOperacion)
        {
            List<ReporteCajaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    SELECT codigo_reporte,
                           CONCAT(FORMAT(fecha_ing, 'dd/MM/yyyy, hh:mm:ss'),' [',codigo_reporte,']') AS fecha_corte_str
                    FROM db_tesoreria.reporte_caja
                    WHERE codigo_estado <> 0
                      AND anio = @AnioOperacion
                      ANd numero_semana = @SemanaOperacion";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@AnioOperacion", anioOperacion);
                        cmd.Parameters.AddWithValue("@SemanaOperacion", semanaOperacion);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            ReporteCajaCLS objReporteCaja;
                            lista = new List<ReporteCajaCLS>();
                            int postCodigoReporte = dr.GetOrdinal("codigo_reporte");
                            int postFechaCorteStr = dr.GetOrdinal("fecha_corte_str");
                            while (dr.Read())
                            {
                                objReporteCaja = new ReporteCajaCLS();
                                objReporteCaja.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objReporteCaja.FechaCorteStr = dr.GetString(postFechaCorteStr);
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

    }
}
