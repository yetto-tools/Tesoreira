using CapaEntidad.Administracion;
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
    public class CajaChicaDAL: CadenaConexion
    {
        public List<ProgramacionSemanalCLS> GetAniosTransacciones()
        {
            List<ProgramacionSemanalCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    SELECT anio_operacion 
                    FROM db_tesoreria.transaccion_caja_chica
                    WHERE codigo_estado <>  0
                    GROUP BY anio_operacion
                    ORDER BY anio_operacion DESC";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        // le indico que es un procedure
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            ProgramacionSemanalCLS objProgramacionSemanal;
                            lista = new List<ProgramacionSemanalCLS>();
                            int postAnio = dr.GetOrdinal("anio_operacion");
                            while (dr.Read())
                            {
                                objProgramacionSemanal = new ProgramacionSemanalCLS();
                                objProgramacionSemanal.Anio = dr.GetInt16(postAnio);
                                lista.Add(objProgramacionSemanal);
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

        public List<ConfiguracionCajaChicaCLS> GetCajasChicas()
        {
            List<ConfiguracionCajaChicaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT codigo_caja_chica,
	                       nombre_caja_chica,		
	                       monto_limite 
                    FROM db_admon.caja_chica
                    WHERE estado = @EstadoRegistro";
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@EstadoRegistro", Constantes.EstadoRegistro.ACTIVO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            ConfiguracionCajaChicaCLS objConfiguracion;
                            lista = new List<ConfiguracionCajaChicaCLS>();
                            int postCodigoCajaChica = dr.GetOrdinal("codigo_caja_chica");
                            int postNombreCajaChica = dr.GetOrdinal("nombre_caja_chica");
                            int postMontoLimite = dr.GetOrdinal("monto_limite");
                            while (dr.Read())
                            {
                                objConfiguracion = new ConfiguracionCajaChicaCLS();
                                objConfiguracion.CodigoCajaChica = dr.GetInt16(postCodigoCajaChica);
                                objConfiguracion.NombreCajaChica = dr.GetString(postNombreCajaChica);
                                objConfiguracion.MontoLimite = dr.GetDecimal(postMontoLimite);
                                lista.Add(objConfiguracion);
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

        public List<ConfiguracionCajaChicaCLS> GetConfiguracionCajasChicas(string idUsuario, int eSuperAdmin)
        {
            List<ConfiguracionCajaChicaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = String.Empty;
                    if (eSuperAdmin == 1)
                    {
                        sql = @"
                        SELECT x.codigo_caja_chica,
	                           x.nombre_caja_chica,		
	                           x.monto_limite, 
	                           x.observaciones, 
	                           x.estado,
                               0 AS permiso_anular,
                               1 AS permiso_editar,
                               (x.monto_disponible - (SELECT COALESCE(SUM(monto),0) AS monto FROM db_tesoreria.transaccion_caja_chica WHERE codigo_estado IN (@CodigoEstadoTransaccionRegistrado,@CodigoEstadoTransaccionIncluidoEnReporte)  AND codigo_caja_chica = x.codigo_caja_chica AND codigo_operacion NOT IN (6,46,69,70))) AS monto_disponible

                        FROM db_admon.caja_chica x
                        WHERE x.estado = @EstadoRegistro";
                    }
                    else {
                        sql = @"
                        SELECT x.codigo_caja_chica,
	                           x.nombre_caja_chica,		
	                           x.monto_limite, 
	                           x.observaciones, 
	                           x.estado,
                               0 AS permiso_anular,
                               1 AS permiso_editar,
                               (x.monto_disponible - (SELECT COALESCE(SUM(monto),0) AS monto FROM db_tesoreria.transaccion_caja_chica WHERE codigo_estado IN (@CodigoEstadoTransaccionRegistrado,@CodigoEstadoTransaccionIncluidoEnReporte)  AND codigo_caja_chica = x.codigo_caja_chica  AND codigo_operacion NOT IN (6,46,69,70))) AS monto_disponible

                        FROM db_admon.caja_chica x
                        WHERE x.estado = @EstadoRegistro
                          AND x.codigo_caja_chica IN (SELECT codigo_caja_chica FROM db_admon.usuario_caja_chica WHERE id_usuario = @IdUsuario)";
                    }
                    
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@EstadoRegistro", Constantes.EstadoRegistro.ACTIVO);
                        cmd.Parameters.AddWithValue("@CodigoEstadoTransaccionRegistrado", Constantes.CajaChica.EstadoTransaccion.REGISTRADO);
                        cmd.Parameters.AddWithValue("@CodigoEstadoTransaccionIncluidoEnReporte", Constantes.CajaChica.EstadoTransaccion.INCLUIDO_EN_REPORTE);
                        cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            ConfiguracionCajaChicaCLS objConfiguracion;
                            lista = new List<ConfiguracionCajaChicaCLS>();
                            int postCodigoCajaChica = dr.GetOrdinal("codigo_caja_chica");
                            int postNombreCajaChica = dr.GetOrdinal("nombre_caja_chica");
                            int postMontoLimite = dr.GetOrdinal("monto_limite");
                            int postPermisoAnular = dr.GetOrdinal("permiso_anular");
                            int postPermisoEditar = dr.GetOrdinal("permiso_editar");
                            int postMontoDisponible = dr.GetOrdinal("monto_disponible");
                            while (dr.Read())
                            {
                                objConfiguracion = new ConfiguracionCajaChicaCLS();
                                objConfiguracion.CodigoCajaChica = dr.GetInt16(postCodigoCajaChica);
                                objConfiguracion.NombreCajaChica = dr.GetString(postNombreCajaChica);
                                objConfiguracion.MontoLimite = dr.GetDecimal(postMontoLimite);
                                objConfiguracion.PermisoAnular = (byte)dr.GetInt32(postPermisoAnular);
                                objConfiguracion.PermisoEditar = (byte)dr.GetInt32(postPermisoEditar);
                                objConfiguracion.MontoDisponible = dr.GetDecimal(postMontoDisponible);
                                objConfiguracion.MontoDisponibleStr = dr.GetDecimal(postMontoDisponible).ToString("N2");

                                lista.Add(objConfiguracion);
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

        public List<CajaChicaCLS> GetMovimientosEnConfiguracionCajasChicas()
        {
            List<CajaChicaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {

                    string sql = @"
                    SELECT x.codigo_transaccion,
	                       COALESCE(x.codigo_reporte,0) AS codigo_reporte,
	                       x.codigo_caja_chica,
	                       z.nombre_caja_chica,
	                       x.codigo_operacion,
	                       y.nombre_operacion,
	                       x.fecha_operacion,
                           FORMAT(x.fecha_operacion,'dd/MM/yyyy') AS fecha_operacion_str,
	                       x.anio_operacion,
	                       x.semana_operacion,
	                       x.dia_operacion,
                           CASE
                             WHEN x.dia_operacion = 1 THEN 'LUNES'
                             WHEN x.dia_operacion = 2 THEN 'MARTES'
                             WHEN x.dia_operacion = 3 THEN 'MIERCOLES'
                             WHEN x.dia_operacion = 4 THEN 'JUEVES'
                             WHEN x.dia_operacion = 5 THEN 'VIERNES'
                             WHEN x.dia_operacion = 6 THEN 'SABADO'
                             WHEN x.dia_operacion = 7 THEN 'DOMINGO'
                             ELSE 'NO DEFINIDO'
                           END AS nombre_dia, 
	                       x.monto,
	                       x.observaciones,
	                       x.codigo_estado,
                           w.nombre AS estado, 
	                       x.usuario_ing,
	                       x.fecha_ing,
	                       FORMAT(x.fecha_ing, 'dd/MM/yyyy, hh:mm:ss') AS fecha_ing_str,
	                       x.codigo_estado_recepcion,
	                       CASE
		                     WHEN x.codigo_estado_recepcion = 0 THEN 'NO APLICA'
		                     WHEN x.codigo_estado_recepcion = 1 THEN 'POR RECEPCIONAR'
		                     WHEN x.codigo_estado_recepcion = 2 THEN 'RECEPCIONADO'
		                     WHEN x.codigo_estado_recepcion = 3 THEN 'REGISTRADO'
		                     ELSE 'NO DEFINIDO'
	                       END AS estado_recepcion,
                           CASE
                             WHEN x.codigo_estado_recepcion = @CodigoEstadoRecepcion THEN 1
                             ELSE 0
                           END AS permiso_anular

                    FROM db_tesoreria.transaccion_caja_chica x
                    INNER JOIN db_tesoreria.operacion y
                    ON x.codigo_operacion = y.codigo_operacion
                    INNER JOIN db_admon.caja_chica z
                    ON x.codigo_caja_chica = z.codigo_caja_chica
                    INNER JOIN db_tesoreria.estado_transaccion w
                    ON x.codigo_estado = w.codigo_estado_transaccion
                    WHERE x.codigo_operacion in (69,70)
                      AND x.codigo_estado <> @CodigoEstadoAnulado
                    ORDER BY x.codigo_transaccion DESC";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoAnulado", Constantes.EstadoTransacccion.ANULADO);
                        cmd.Parameters.AddWithValue("@CodigoEstadoRecepcion", Constantes.CajaChica.EstadoRecepcion.POR_RECEPCIONAR);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            CajaChicaCLS objTransaccion;
                            lista = new List<CajaChicaCLS>();
                            int postCodigoTransaccion = dr.GetOrdinal("codigo_transaccion");
                            int postCodigoReporte = dr.GetOrdinal("codigo_reporte");
                            int postCodigoCajaChica = dr.GetOrdinal("codigo_caja_chica");
                            int postNombreCajaChica = dr.GetOrdinal("nombre_caja_chica");
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postNombreOperacion = dr.GetOrdinal("nombre_operacion");
                            int postFechaOperacion = dr.GetOrdinal("fecha_operacion");
                            int postFechaOperacionStr = dr.GetOrdinal("fecha_operacion_str");
                            int postAnioOperacion = dr.GetOrdinal("anio_operacion");
                            int postSemanaOperacion = dr.GetOrdinal("semana_operacion");
                            int postDiaOperacion = dr.GetOrdinal("dia_operacion");
                            int postNombreDia = dr.GetOrdinal("nombre_dia");
                            int postMonto = dr.GetOrdinal("monto");
                            int postObservaciones = dr.GetOrdinal("observaciones");
                            int postCodigoEstado = dr.GetOrdinal("codigo_estado");
                            int postEstado = dr.GetOrdinal("estado");
                            int postUsuarioIng = dr.GetOrdinal("usuario_ing");
                            int postFechaIng = dr.GetOrdinal("fecha_ing");
                            int postFechaIngStr = dr.GetOrdinal("fecha_ing_str");
                            int postCodigoEstadoRecepcion = dr.GetOrdinal("codigo_estado_recepcion");
                            int postEstadoRecepcion = dr.GetOrdinal("estado_recepcion");
                            int postPermisoAnular = dr.GetOrdinal("permiso_anular");

                            while (dr.Read())
                            {
                                objTransaccion = new CajaChicaCLS();

                                objTransaccion.CodigoTransaccion = dr.GetInt64(postCodigoTransaccion);
                                objTransaccion.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objTransaccion.CodigoCajaChica = dr.GetInt16(postCodigoCajaChica);
                                objTransaccion.NombreCajaChica = dr.GetString(postNombreCajaChica);
                                objTransaccion.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objTransaccion.Operacion = dr.GetString(postNombreOperacion);
                                objTransaccion.FechaOperacion = dr.GetDateTime(postFechaOperacion);
                                objTransaccion.FechaOperacionStr = dr.GetString(postFechaOperacionStr);
                                objTransaccion.AnioOperacion = dr.GetInt16(postAnioOperacion);
                                objTransaccion.SemanaOperacion = dr.GetByte(postSemanaOperacion);
                                objTransaccion.DiaOperacion = dr.GetByte(postDiaOperacion);
                                objTransaccion.NombreDiaOperacion = dr.GetString(postNombreDia);
                                objTransaccion.Monto = dr.GetDecimal(postMonto);
                                objTransaccion.Observaciones = dr.IsDBNull(postObservaciones) ? "" : dr.GetString(postObservaciones);
                                objTransaccion.CodigoEstado = dr.GetInt16(postCodigoEstado);
                                objTransaccion.Estado = dr.GetString(postEstado);
                                objTransaccion.UsuarioIng = dr.GetString(postUsuarioIng);
                                objTransaccion.FechaIng = dr.GetDateTime(postFechaIng);
                                objTransaccion.FechaIngStr = dr.GetString(postFechaIngStr);
                                objTransaccion.CodigoEstadoRecepcion = dr.GetByte(postCodigoEstadoRecepcion);
                                objTransaccion.EstadoRecepcion = dr.GetString(postEstadoRecepcion);
                                objTransaccion.PermisoAnular = (byte)dr.GetInt32(postPermisoAnular);

                                lista.Add(objTransaccion);
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

        public List<CajaChicaCLS> GetAllMovimientosEnCajasChicas(int codigoCajaChica, int anioOperacion, int codigoOperacion)
        {
            List<CajaChicaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string filterCajaChica = String.Empty;
                    string filterOperacion = String.Empty;
                    string filterAnioOperacion = String.Empty;
                    if (codigoCajaChica != -1) {
                        filterCajaChica = "AND x.codigo_caja_chica = " + codigoCajaChica.ToString();
                    }

                    if (anioOperacion != -1) {
                        filterAnioOperacion = "AND x.anio_operacion = " + anioOperacion.ToString();
                    }

                    if (codigoOperacion != -1)
                    {
                        filterOperacion = "AND x.codigo_operacion = " + codigoOperacion.ToString();
                    }

                    string sql = @"
                    SELECT x.codigo_transaccion,
	                       COALESCE(x.codigo_reporte,0) AS codigo_reporte,
	                       x.codigo_caja_chica,
	                       z.nombre_caja_chica,
	                       x.codigo_operacion,
	                       y.nombre_operacion,
	                       x.fecha_operacion,
                           FORMAT(x.fecha_operacion,'dd/MM/yyyy') AS fecha_operacion_str,
	                       x.anio_operacion,
	                       x.semana_operacion,
	                       x.dia_operacion,
                           CASE
                             WHEN x.dia_operacion = 1 THEN 'LUNES'
                             WHEN x.dia_operacion = 2 THEN 'MARTES'
                             WHEN x.dia_operacion = 3 THEN 'MIERCOLES'
                             WHEN x.dia_operacion = 4 THEN 'JUEVES'
                             WHEN x.dia_operacion = 5 THEN 'VIERNES'
                             WHEN x.dia_operacion = 6 THEN 'SABADO'
                             WHEN x.dia_operacion = 7 THEN 'DOMINGO'
                             ELSE 'NO DEFINIDO'
                           END AS nombre_dia, 
	                       x.monto,
	                       x.observaciones,
	                       x.codigo_estado,
                           w.nombre AS estado, 
	                       x.usuario_ing,
	                       x.fecha_ing,
	                       FORMAT(x.fecha_ing, 'dd/MM/yyyy, hh:mm:ss') AS fecha_ing_str,
	                       x.codigo_estado_recepcion,
	                       CASE
		                     WHEN x.codigo_estado_recepcion = 0 THEN 'NO APLICA'
		                     WHEN x.codigo_estado_recepcion = 1 THEN 'POR RECEPCIONAR'
		                     WHEN x.codigo_estado_recepcion = 2 THEN 'RECEPCIONADO'
		                     WHEN x.codigo_estado_recepcion = 3 THEN 'REGISTRADO'
		                     ELSE 'NO DEFINIDO'
	                       END AS estado_recepcion
                    FROM db_tesoreria.transaccion_caja_chica x
                    INNER JOIN db_tesoreria.operacion y
                    ON x.codigo_operacion = y.codigo_operacion
                    INNER JOIN db_admon.caja_chica z
                    ON x.codigo_caja_chica = z.codigo_caja_chica
                    INNER JOIN db_tesoreria.estado_transaccion w
                    ON x.codigo_estado = w.codigo_estado_transaccion
                    WHERE x.codigo_operacion in (6,69,70)
                      AND x.codigo_estado <> @CodigoEstadoAnulado
                    " + filterCajaChica + @"
                    " + filterAnioOperacion + @"
                    " + filterOperacion + @"
                    ORDER BY x.codigo_transaccion DESC";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoAnulado", Constantes.EstadoTransacccion.ANULADO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            CajaChicaCLS objTransaccion;
                            lista = new List<CajaChicaCLS>();
                            int postCodigoTransaccion = dr.GetOrdinal("codigo_transaccion");
                            int postCodigoReporte = dr.GetOrdinal("codigo_reporte");
                            int postCodigoCajaChica = dr.GetOrdinal("codigo_caja_chica");
                            int postNombreCajaChica = dr.GetOrdinal("nombre_caja_chica");
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postNombreOperacion = dr.GetOrdinal("nombre_operacion");
                            int postFechaOperacion = dr.GetOrdinal("fecha_operacion");
                            int postFechaOperacionStr = dr.GetOrdinal("fecha_operacion_str");
                            int postAnioOperacion = dr.GetOrdinal("anio_operacion");
                            int postSemanaOperacion = dr.GetOrdinal("semana_operacion");
                            int postDiaOperacion = dr.GetOrdinal("dia_operacion");
                            int postNombreDia = dr.GetOrdinal("nombre_dia");
                            int postMonto = dr.GetOrdinal("monto");
                            int postObservaciones = dr.GetOrdinal("observaciones");
                            int postCodigoEstado = dr.GetOrdinal("codigo_estado");
                            int postEstado = dr.GetOrdinal("estado");
                            int postUsuarioIng = dr.GetOrdinal("usuario_ing");
                            int postFechaIng = dr.GetOrdinal("fecha_ing");
                            int postFechaIngStr = dr.GetOrdinal("fecha_ing_str");
                            int postCodigoEstadoRecepcion = dr.GetOrdinal("codigo_estado_recepcion");
                            int postEstadoRecepcion = dr.GetOrdinal("estado_recepcion");
                            while (dr.Read())
                            {
                                objTransaccion = new CajaChicaCLS();

                                objTransaccion.CodigoTransaccion = dr.GetInt64(postCodigoTransaccion);
                                objTransaccion.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objTransaccion.CodigoCajaChica = dr.GetInt16(postCodigoCajaChica);
                                objTransaccion.NombreCajaChica = dr.GetString(postNombreCajaChica);
                                objTransaccion.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objTransaccion.Operacion = dr.GetString(postNombreOperacion);
                                objTransaccion.FechaOperacion = dr.GetDateTime(postFechaOperacion);
                                objTransaccion.FechaOperacionStr = dr.GetString(postFechaOperacionStr);
                                objTransaccion.AnioOperacion = dr.GetInt16(postAnioOperacion);
                                objTransaccion.SemanaOperacion = dr.GetByte(postSemanaOperacion);
                                objTransaccion.DiaOperacion = dr.GetByte(postDiaOperacion);
                                objTransaccion.NombreDiaOperacion = dr.GetString(postNombreDia);
                                objTransaccion.Monto = dr.GetDecimal(postMonto);
                                objTransaccion.Observaciones = dr.IsDBNull(postObservaciones) ? "" : dr.GetString(postObservaciones);
                                objTransaccion.CodigoEstado = dr.GetInt16(postCodigoEstado);
                                objTransaccion.Estado = dr.GetString(postEstado);
                                objTransaccion.UsuarioIng = dr.GetString(postUsuarioIng);
                                objTransaccion.FechaIng = dr.GetDateTime(postFechaIng);
                                objTransaccion.FechaIngStr = dr.GetString(postFechaIngStr);
                                objTransaccion.CodigoEstadoRecepcion = dr.GetByte(postCodigoEstadoRecepcion);
                                objTransaccion.EstadoRecepcion = dr.GetString(postEstadoRecepcion);

                                lista.Add(objTransaccion);
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

        public CajaChicaComboCLS FillCombosNewCajaChica(string idUsuario, int esSuperAdmin)
        {
            CajaChicaComboCLS objCajaChicaComboCLS = new CajaChicaComboCLS();
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_tesoreria.uspFillComboNewCajaChica", conexion))
                    {
                        // le indico que es un procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                        cmd.Parameters.AddWithValue("@EsSuperAdmin", esSuperAdmin);
                        // Devolvera varias tablas
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr != null)
                        {
                            List<ConfiguracionCajaChicaCLS> listaCajasChicas = new List<ConfiguracionCajaChicaCLS>();
                            int postCodigoCajaChica = dr.GetOrdinal("codigo_caja_chica");
                            int postNombreCajaChica = dr.GetOrdinal("nombre_caja_chica");

                            ConfiguracionCajaChicaCLS objCajaChicaCLS;
                            while (dr.Read())
                            {
                                objCajaChicaCLS = new ConfiguracionCajaChicaCLS();
                                objCajaChicaCLS.CodigoCajaChica = dr.GetInt16(postCodigoCajaChica);
                                objCajaChicaCLS.NombreCajaChica = dr.GetString(postNombreCajaChica);
                                listaCajasChicas.Add(objCajaChicaCLS);
                            }//fin while
                            objCajaChicaComboCLS.listaCajasChicas = listaCajasChicas;
                        }// fin if

                        if (dr.NextResult())
                        {
                            List<OperacionCLS> listaOperaciones = new List<OperacionCLS>();
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postNombre = dr.GetOrdinal("nombre_operacion");

                            OperacionCLS objOperacionCLS;
                            while (dr.Read())
                            {
                                objOperacionCLS = new OperacionCLS();
                                objOperacionCLS.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objOperacionCLS.Nombre = dr.GetString(postNombre);
                                listaOperaciones.Add(objOperacionCLS);
                            }//fin while
                            objCajaChicaComboCLS.listaOperaciones = listaOperaciones;
                        }// fin if

                        if (dr.NextResult())
                        {
                            List<ProgramacionSemanalCLS> listaProgrmacionSemanal = new List<ProgramacionSemanalCLS>();
                            int postAnioOperacion = dr.GetOrdinal("anio");
                            int postFecha = dr.GetOrdinal("fecha");
                            int postFechaStr = dr.GetOrdinal("fechaStr");
                            int postDia = dr.GetOrdinal("dia");
                            int postNumeroSemana = dr.GetOrdinal("numero_semana");

                            ProgramacionSemanalCLS objProgramacionSemanalCLS;
                            while (dr.Read())
                            {
                                objProgramacionSemanalCLS = new ProgramacionSemanalCLS();
                                objProgramacionSemanalCLS.FechaStr = dr.GetString(postFechaStr);
                                objProgramacionSemanalCLS.Dia = dr.GetString(postDia);
                                objProgramacionSemanalCLS.NumeroSemana = dr.GetByte(postNumeroSemana);
                                objCajaChicaComboCLS.AnioOperacion = dr.GetInt16(postAnioOperacion);
                                objCajaChicaComboCLS.NumeroSemana = dr.GetByte(postNumeroSemana);
                                listaProgrmacionSemanal.Add(objProgramacionSemanalCLS);

                            }//fin while
                            objCajaChicaComboCLS.listaProgramacionSemanal = listaProgrmacionSemanal;
                        }
                    }// fin using
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                    objCajaChicaComboCLS = null;

                }
                return objCajaChicaComboCLS;
            }
        }

        public CajaChicaComboCLS FillCombosEditCajaChica(int anioOperacion, int semanaOperacion, string idUsuario, int esSuperAdmin)
        {
            CajaChicaComboCLS objCajaChicaComboCLS = new CajaChicaComboCLS();
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_tesoreria.uspFillComboEditCajaChica", conexion))
                    {
                        // le indico que es un procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                        cmd.Parameters.AddWithValue("@EsSuperAdmin", esSuperAdmin);
                        cmd.Parameters.AddWithValue("@AnioOperacion", anioOperacion);
                        cmd.Parameters.AddWithValue("@SemanaOperacion", semanaOperacion);
                        // Devolvera varias tablas
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr != null)
                        {
                            List<ConfiguracionCajaChicaCLS> listaCajasChicas = new List<ConfiguracionCajaChicaCLS>();
                            int postCodigoCajaChica = dr.GetOrdinal("codigo_caja_chica");
                            int postNombreCajaChica = dr.GetOrdinal("nombre_caja_chica");

                            ConfiguracionCajaChicaCLS objCajaChicaCLS;
                            while (dr.Read())
                            {
                                objCajaChicaCLS = new ConfiguracionCajaChicaCLS();
                                objCajaChicaCLS.CodigoCajaChica = dr.GetInt16(postCodigoCajaChica);
                                objCajaChicaCLS.NombreCajaChica = dr.GetString(postNombreCajaChica);
                                listaCajasChicas.Add(objCajaChicaCLS);
                            }//fin while
                            objCajaChicaComboCLS.listaCajasChicas = listaCajasChicas;
                        }// fin if

                        if (dr.NextResult())
                        {
                            List<OperacionCLS> listaOperaciones = new List<OperacionCLS>();
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postNombre = dr.GetOrdinal("nombre_operacion");

                            OperacionCLS objOperacionCLS;
                            while (dr.Read())
                            {
                                objOperacionCLS = new OperacionCLS();
                                objOperacionCLS.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objOperacionCLS.Nombre = dr.GetString(postNombre);
                                listaOperaciones.Add(objOperacionCLS);
                            }//fin while
                            objCajaChicaComboCLS.listaOperaciones = listaOperaciones;
                        }// fin if

                        if (dr.NextResult())
                        {
                            List<ProgramacionSemanalCLS> listaProgrmacionSemanal = new List<ProgramacionSemanalCLS>();
                            int postAnioOperacion = dr.GetOrdinal("anio");
                            int postFecha = dr.GetOrdinal("fecha");
                            int postFechaStr = dr.GetOrdinal("fechaStr");
                            int postDia = dr.GetOrdinal("dia");
                            int postNumeroSemana = dr.GetOrdinal("numero_semana");

                            ProgramacionSemanalCLS objProgramacionSemanalCLS;
                            while (dr.Read())
                            {
                                objProgramacionSemanalCLS = new ProgramacionSemanalCLS();
                                objProgramacionSemanalCLS.FechaStr = dr.GetString(postFechaStr);
                                objProgramacionSemanalCLS.Dia = dr.GetString(postDia);
                                objProgramacionSemanalCLS.NumeroSemana = dr.GetByte(postNumeroSemana);
                                objCajaChicaComboCLS.AnioOperacion = dr.GetInt16(postAnioOperacion);
                                objCajaChicaComboCLS.NumeroSemana = dr.GetByte(postNumeroSemana);
                                listaProgrmacionSemanal.Add(objProgramacionSemanalCLS);

                            }//fin while
                            objCajaChicaComboCLS.listaProgramacionSemanal = listaProgrmacionSemanal;
                        }
                    }// fin using
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                    objCajaChicaComboCLS = null;

                }
                return objCajaChicaComboCLS;
            }
        }

        public string GuardarTransaccion(CajaChicaCLS objTransaccion, string usuarioIng)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
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

                    string codigoSeguridad = Util.Seguridad.GenerarCadena();
                    int anio = DateTime.Now.Year;
                    string sqlSequence = "SELECT sig_valor FROM db_admon.secuencia_detalle WHERE codigo_secuencia = @codigoSecuencia AND anio = @AnioTransaccion";

                    // ExecuteScalar(), Executes the query, and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.
                    cmd.CommandText = sqlSequence;
                    cmd.Parameters.AddWithValue("@codigoSecuencia", Constantes.Secuencia.SIT_SEQ_TRANSACCION_CAJA_CHICA);
                    cmd.Parameters.AddWithValue("@AnioTransaccion", anio);
                    long siguienteValor = (long)cmd.ExecuteScalar();

                    long codigoTransaccion = long.Parse(anio.ToString() + siguienteValor.ToString("D6"));
                    string sentenciaUpdate = "UPDATE db_admon.secuencia_detalle SET sig_valor = @siguienteValor WHERE codigo_secuencia = @CodigoSecuencia  AND anio = @AnioTransaccion";
                    string sentenciaSQL = @"
                    INSERT INTO db_tesoreria.transaccion_caja_chica( codigo_transaccion,
                                                                     codigo_reporte,   
                                                                     codigo_tipo_transaccion,
                                                                     codigo_caja_chica,
                                                                     codigo_operacion,
                                                                     codigo_tipo_documento,   
                                                                     nit_proveedor,	
	                                                                 serie_factura,
                                                                     numero_documento,
                                                                     fecha_documento,
                                                                     descripcion,
                                                                     fecha_operacion,
                                                                     anio_operacion,
	                                                                 semana_operacion,
                                                                     dia_operacion,
                                                                     monto,
                                                                     observaciones,
                                                                     observaciones_anulacion,
                                                                     usuario_anulacion,
	                                                                 fecha_anulacion,
                                                                     excluir_factura,
                                                                     codigo_motivo_exclusion,
                                                                     observaciones_exclusion,
                                                                     usuario_revision,
	                                                                 fecha_revision,
                                                                     codigo_estado,
                                                                     usuario_ing,
                                                                     fecha_ing,
                                                                     usuario_act,
                                                                     fecha_act)
                    VALUES( @codigoTransaccion,
                            @codigoReporte,
                            @codigoTipoTransaccion,
                            @codigoCajaChica,
                            @codigoOperacion,
                            @codigoTipoDocumento,
                            @nitProveedor,	
	                        @serieFactura,
                            @numeroDocumento,
                            @fechaDocumento,
                            @descripcion,
                            @fechaOperacion,
                            @anioOperacion,
	                        @semanaOperacion,
                            @diaOperacion,
                            @monto,
                            @observaciones,
                            @observacionesAnulacion,
                            @usuarioAnulacion,
	                        @fechaAnulacion,
                            @excluirFactura,
                            @codigoMotivoExclusion,
                            @observacionesExclusion,
                            @usuarioRevision,
	                        @fechaRevision,
                            @codigoEstado,
                            @usuarioIng,
                            @fechaIng,
                            @usuarioAct,
                            @fechaAct)";

                    cmd.CommandText = sentenciaSQL;
                    cmd.Parameters.AddWithValue("@codigoTransaccion", codigoTransaccion);
                    cmd.Parameters.AddWithValue("@codigoReporte", DBNull.Value);
                    cmd.Parameters.AddWithValue("@codigoTipoTransaccion", objTransaccion.CodigoTipoTransaccion);
                    cmd.Parameters.AddWithValue("@codigoCajaChica", objTransaccion.CodigoCajaChica);
                    cmd.Parameters.AddWithValue("@codigoOperacion", objTransaccion.CodigoOperacion);
                    cmd.Parameters.AddWithValue("@codigoTipoDocumento", objTransaccion.CodigoTipoDocumento);
                    cmd.Parameters.AddWithValue("@nitProveedor", objTransaccion.NitProveedor);
                    cmd.Parameters.AddWithValue("@serieFactura", objTransaccion.SerieFactura.ToUpper());
                    cmd.Parameters.AddWithValue("@numeroDocumento", objTransaccion.NumeroDocumento);
                    cmd.Parameters.AddWithValue("@fechaDocumento", objTransaccion.FechaDocumento);
                    cmd.Parameters.AddWithValue("@descripcion", objTransaccion.Descripcion);
                    cmd.Parameters.AddWithValue("@fechaOperacion", objTransaccion.FechaOperacion);
                    cmd.Parameters.AddWithValue("@anioOperacion", objTransaccion.AnioOperacion);
                    cmd.Parameters.AddWithValue("@semanaOperacion",objTransaccion.SemanaOperacion);
                    cmd.Parameters.AddWithValue("@diaOperacion", Util.Conversion.DayOfWeek(objTransaccion.FechaOperacion));
                    cmd.Parameters.AddWithValue("@monto", objTransaccion.Monto);
                    cmd.Parameters.AddWithValue("@observaciones",  objTransaccion.Observaciones == null ? DBNull.Value : objTransaccion.Observaciones);
                    cmd.Parameters.AddWithValue("@observacionesAnulacion", objTransaccion.ObservacionesAnulacion == null ? DBNull.Value : objTransaccion.ObservacionesAnulacion);
                    cmd.Parameters.AddWithValue("@usuarioAnulacion", objTransaccion.UsuarioAnulacion == null ? DBNull.Value : objTransaccion.UsuarioAnulacion);
                    cmd.Parameters.AddWithValue("@fechaAnulacion", objTransaccion.FechaAnulacion == null ? DBNull.Value : objTransaccion.FechaAnulacion);
                    cmd.Parameters.AddWithValue("@excluirFactura", objTransaccion.ExcluirFactura);
                    cmd.Parameters.AddWithValue("@codigoMotivoExclusion", objTransaccion.CodigoMotivoExclusion == null ? DBNull.Value : objTransaccion.CodigoMotivoExclusion);
                    cmd.Parameters.AddWithValue("@observacionesExclusion", objTransaccion.ObservacionesExclusion == null ? DBNull.Value : objTransaccion.ObservacionesExclusion);
                    cmd.Parameters.AddWithValue("@usuarioRevision", objTransaccion.UsuarioRevision == null ? DBNull.Value : objTransaccion.UsuarioRevision);
                    cmd.Parameters.AddWithValue("@fechaRevision", objTransaccion.FechaRevision == null ? DBNull.Value : objTransaccion.FechaRevision);
                    cmd.Parameters.AddWithValue("@codigoEstado", Constantes.CajaChica.EstadoTransaccion.REGISTRADO);
                    cmd.Parameters.AddWithValue("@usuarioIng", usuarioIng);
                    cmd.Parameters.AddWithValue("@fechaIng", DateTime.Now);
                    cmd.Parameters.AddWithValue("@usuarioAct", DBNull.Value);
                    cmd.Parameters.AddWithValue("@fechaAct", DBNull.Value);


                    cmd.ExecuteNonQuery();
                    cmd.CommandText = sentenciaUpdate;
                    cmd.Parameters.AddWithValue("@siguienteValor", siguienteValor + 1);
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

        public CajaChicaCLS GetDataTransaccion(long codigoTransaccion)
        {
            CajaChicaCLS objTransaccion = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT x.codigo_transaccion, 
                           x.anio_operacion,
                           x.semana_operacion,
                           x.dia_operacion, 
                           CASE
                             WHEN x.dia_operacion = 1 THEN 'LUNES'
                             WHEN x.dia_operacion = 2 THEN 'MARTES'
                             WHEN x.dia_operacion = 3 THEN 'MIERCOLES'
                             WHEN x.dia_operacion = 4 THEN 'JUEVES'
                             WHEN x.dia_operacion = 5 THEN 'VIERNES'
                             WHEN x.dia_operacion = 6 THEN 'SÁBADO'
                             WHEN x.dia_operacion = 7 THEN 'DOMINGO'
                             ELSE 'DESCONOCIDO'
                           END AS nombre_dia_operacion, 
                           x.fecha_operacion,
                           FORMAT(x.fecha_operacion, 'dd/MM/yyyy') AS fecha_operacion_str,
	                       x.codigo_caja_chica, 
	                       y.nombre_caja_chica,
	                       x.codigo_operacion, 
	                       z.nombre_operacion,
	                       x.nit_proveedor,
                           w.nombre AS nombre_proveedor,
	                       x.serie_factura,
	                       x.numero_documento,
	                       x.fecha_documento,
                           FORMAT(x.fecha_documento, 'dd/MM/yyyy') AS fecha_documento_str,
	                       x.monto,
                           x.descripcion, 
                           x.observaciones, 
	                       x.excluir_factura,
	                       x.codigo_estado,
	                       x.usuario_ing,
	                       x.fecha_ing,
                           FORMAT(x.fecha_ing, 'dd/MM/yyyy, hh:mm:ss') AS fecha_ing_str,
                           COALESCE(x.codigo_reporte,0) AS codigo_reporte,
                           x.codigo_tipo_documento

                    FROM db_tesoreria.transaccion_caja_chica x
                    INNER JOIN db_admon.caja_chica y
                    ON x.codigo_caja_chica = y.codigo_caja_chica
                    INNER JOIN db_tesoreria.operacion z
                    ON x.codigo_operacion = z.codigo_operacion
                    INNER JOIN db_tesoreria.contribuyente w
                    ON x.nit_proveedor = w.nit
                    WHERE x.codigo_transaccion = @CodigoTransaccion";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoTransaccion", codigoTransaccion);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            objTransaccion = new CajaChicaCLS(); ;
                            int postCodigoTransaccion = dr.GetOrdinal("codigo_transaccion");
                            int postAnioOperacion = dr.GetOrdinal("anio_operacion");
                            int postSemanaOperacion = dr.GetOrdinal("semana_operacion");
                            int postDiaOperacion = dr.GetOrdinal("dia_operacion");
                            int postNombreDiaOperacion = dr.GetOrdinal("nombre_dia_operacion");
                            int postFechaOperacion = dr.GetOrdinal("fecha_operacion");
                            int postFechaOperacionStr = dr.GetOrdinal("fecha_operacion_str");
                            int postCodigoCajaChica = dr.GetOrdinal("codigo_caja_chica");
                            int postNombreCajaChica = dr.GetOrdinal("nombre_caja_chica");
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postNombreOperacion = dr.GetOrdinal("nombre_operacion");
                            int postNitProveedor = dr.GetOrdinal("nit_proveedor");
                            int postNombreProveedor = dr.GetOrdinal("nombre_proveedor");
                            int postSerieFactura = dr.GetOrdinal("serie_factura");
                            int postNumeroDocumento = dr.GetOrdinal("numero_documento");
                            int postFechaDocumento = dr.GetOrdinal("fecha_documento");
                            int postFechaDocumentoStr = dr.GetOrdinal("fecha_documento_str");
                            int postMonto = dr.GetOrdinal("monto");
                            int postDescripcion = dr.GetOrdinal("descripcion");
                            int postObservaciones = dr.GetOrdinal("observaciones");
                            int postExcluirFactura = dr.GetOrdinal("excluir_factura");
                            int postCodigoEstado = dr.GetOrdinal("codigo_estado");
                            int postUsuarioIng = dr.GetOrdinal("usuario_ing");
                            int postFechaIng = dr.GetOrdinal("fecha_ing");
                            int postFechaIngresoStr = dr.GetOrdinal("fecha_ing_str");
                            int postCodigoReporte = dr.GetOrdinal("codigo_reporte");
                            int postCodigoTipoDocumento = dr.GetOrdinal("codigo_tipo_documento");

                            while (dr.Read())
                            {
                                objTransaccion = new CajaChicaCLS();
                                objTransaccion.CodigoTransaccion = dr.GetInt64(postCodigoTransaccion);
                                objTransaccion.AnioOperacion = dr.GetInt16(postAnioOperacion);
                                objTransaccion.SemanaOperacion = dr.GetByte(postSemanaOperacion);
                                objTransaccion.DiaOperacion = dr.GetByte(postDiaOperacion);
                                objTransaccion.NombreDiaOperacion = dr.GetString(postNombreDiaOperacion);
                                objTransaccion.FechaOperacion = dr.GetDateTime(postFechaOperacion);
                                objTransaccion.FechaStr = dr.GetString(postFechaOperacionStr);
                                objTransaccion.CodigoCajaChica = dr.GetInt16(postCodigoCajaChica);
                                objTransaccion.NombreCajaChica = dr.GetString(postNombreCajaChica);
                                objTransaccion.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objTransaccion.Operacion = dr.GetString(postNombreOperacion);
                                objTransaccion.NitProveedor = dr.GetString(postNitProveedor);
                                objTransaccion.NombreProveedor = dr.GetString(postNombreProveedor);
                                objTransaccion.SerieFactura = dr.GetString(postSerieFactura);
                                objTransaccion.NumeroDocumento = dr.GetInt64(postNumeroDocumento);
                                objTransaccion.FechaDocumento = dr.GetDateTime(postFechaDocumento);
                                objTransaccion.FechaDocumentoStr = dr.GetString(postFechaDocumentoStr);
                                objTransaccion.Monto = dr.GetDecimal(postMonto);
                                objTransaccion.Descripcion = dr.IsDBNull(postDescripcion) ? "" : dr.GetString(postDescripcion);
                                objTransaccion.Observaciones = dr.IsDBNull(postObservaciones) ? "" : dr.GetString(postObservaciones);
                                objTransaccion.ExcluirFactura = dr.GetByte(postExcluirFactura);
                                objTransaccion.CodigoEstado = dr.GetInt16(postCodigoEstado);
                                objTransaccion.UsuarioIng = dr.GetString(postUsuarioIng);
                                objTransaccion.FechaIng = dr.GetDateTime(postFechaIng);
                                objTransaccion.FechaIngStr = dr.GetString(postFechaIngresoStr);
                                objTransaccion.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objTransaccion.CodigoTipoDocumento = dr.GetByte(postCodigoTipoDocumento);


                            }
                        }
                    }
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                    objTransaccion = null;
                }

                return objTransaccion;
            }
        }

        public string RegistrarSolicitudDeCorreccion(long codigoTransaccion, int codigoTipoCorreccion, string observaciones, string usuarioIng)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
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
                    string sentenciaSQL = @"
                    INSERT INTO db_tesoreria.solicitud_correccion_caja_chica(codigo_transaccion,codigo_transaccion_correcta,codigo_tipo_correccion,observaciones_solicitud,observaciones_aprobacion,resultado,usuario_aprobacion,fecha_aprobacion,estado,usuario_ing,fecha_ing,usuario_act,fecha_act)
                    VALUES(@CodigoTransaccion,
                           @CodigoTransaccionCorrecta,
                           @CodigoTipoCorreccion, 
                           @ObservacionesSolicitud,
                           @ObservacionesAprobacion,
                           @Resultado,
                           @UsuarioAprobacion,
                           @FechaAprobacion,
                           @CodigoEstado,
                           @UsuarioIng,
                           @FechaIng,
                           @UsuarioAct,
                           @FechaAct)";

                    cmd.CommandText = sentenciaSQL;
                    cmd.Parameters.AddWithValue("@CodigoTransaccion", codigoTransaccion);
                    cmd.Parameters.AddWithValue("@CodigoTransaccionCorrecta", DBNull.Value);
                    cmd.Parameters.AddWithValue("@CodigoTipoCorreccion", codigoTipoCorreccion);
                    cmd.Parameters.AddWithValue("@ObservacionesSolicitud", observaciones);
                    cmd.Parameters.AddWithValue("@ObservacionesAprobacion", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Resultado", Constantes.Correccion.ResultadoSolicitudCorreccion.SIN_RESULTADO);
                    cmd.Parameters.AddWithValue("@UsuarioAprobacion", DBNull.Value);
                    cmd.Parameters.AddWithValue("@FechaAprobacion", DBNull.Value);
                    cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.EstadoRegistro.ACTIVO);
                    cmd.Parameters.AddWithValue("@UsuarioIng", usuarioIng);
                    cmd.Parameters.AddWithValue("@FechaIng", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UsuarioAct", DBNull.Value);
                    cmd.Parameters.AddWithValue("@FechaAct", DBNull.Value);
                    cmd.ExecuteNonQuery();

                    sentenciaSQL = @"
                    UPDATE db_tesoreria.transaccion_caja_chica
                    SET correccion = 1,
                        codigo_estado_solicitud_correccion = @CodigoEstadoSolicitudCorreccion
                    WHERE codigo_transaccion = @CodigoTransaccion";
                    cmd.CommandText = sentenciaSQL;
                    cmd.Parameters.AddWithValue("@CodigoEstadoSolicitudCorreccion", Constantes.Correccion.EstadoSolicitudcorreccion.SOLICITA_APROBACION);
                    cmd.Parameters["@CodigoTransaccion"].Value = codigoTransaccion;
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

        public List<CajaChicaCLS> GetSolicitudesDeCorreccion(int anioOperacion, int semanaOperacion, int codigoReporte)
        {
            List<CajaChicaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string filterAnioOperacion = string.Empty;
                    string filterSemanaOperacion = string.Empty;
                    string filterReporte = string.Empty;
                    if (anioOperacion != -1)
                    {
                        filterAnioOperacion = "AND x.anio_operacion = " + anioOperacion.ToString();
                    }
                    if (semanaOperacion != -1)
                    {
                        filterSemanaOperacion = "AND x.semana_operacion = " + semanaOperacion.ToString();
                    }
                    if (codigoReporte != -1)
                    {
                        filterReporte = "AND x.codigo_reporte = " + codigoReporte.ToString();
                    }

                    conexion.Open();
                    string sql = @"
                    SELECT x.codigo_transaccion, 
                           x.anio_operacion,
                           x.semana_operacion,
                           x.dia_operacion, 
                           CASE
                             WHEN x.dia_operacion = 1 THEN 'LUNES'
                             WHEN x.dia_operacion = 2 THEN 'MARTES'
                             WHEN x.dia_operacion = 3 THEN 'MIERCOLES'
                             WHEN x.dia_operacion = 4 THEN 'JUEVES'
                             WHEN x.dia_operacion = 5 THEN 'VIERNES'
                             WHEN x.dia_operacion = 6 THEN 'SÁBADO'
                             WHEN x.dia_operacion = 7 THEN 'DOMINGO'
                             ELSE 'DESCONOCIDO'
                           END AS nombre_dia_operacion, 
                           x.fecha_operacion,
                           FORMAT(x.fecha_operacion, 'dd/MM/yyyy') AS fecha_operacion_str,
	                       x.codigo_caja_chica, 
	                       y.nombre_caja_chica,
	                       x.codigo_operacion, 
	                       z.nombre_operacion,
	                       x.nit_proveedor,
                           w.nombre AS nombre_proveedor,
	                       x.serie_factura,
	                       x.numero_documento,
	                       x.fecha_documento,
                           FORMAT(x.fecha_documento, 'dd/MM/yyyy') AS fecha_documento_str,
	                       x.monto,
                           x.descripcion, 
                           x.observaciones, 
	                       x.excluir_factura,
	                       x.codigo_estado,
	                       x.usuario_ing,
	                       x.fecha_ing,
                           FORMAT(x.fecha_ing, 'dd/MM/yyyy, hh:mm:ss') AS fecha_ing_str,
                           COALESCE(x.codigo_reporte,0) AS codigo_reporte,
                           CASE
                            WHEN x.codigo_estado_solicitud_correccion = @CodigoEstadoSolicitaAprobacion THEN 1
                            ELSE 0
                           END AS permiso_autorizar,
                           x.correccion,
                           m.observaciones_solicitud,
                           m.observaciones_aprobacion,
                           COALESCE(x.codigo_transaccion_ant,0) AS codigo_transaccion_ant,
                           CASE
                             WHEN m.resultado = 0 THEN  'SIN RESPUESTA'
                             WHEN m.resultado = 1 THEN  'APROBADA'
                             WHEN m.resultado = 2 THEN  'DENEGADA'
                             ELSE 'NO DEFINIDA'   
                           END AS respuesta_correccion,
                           COALESCE(m.codigo_tipo_correccion,0) AS codigo_tipo_correccion,
                           CASE
                             WHEN m.codigo_tipo_correccion = 1 THEN 'MODICACIÓN'
                             WHEN m.codigo_tipo_correccion = 2 THEN 'ANULACIÓN'
                             ELSE 'NO DEFINIDO'   
                           END AS tipo_correccion

                    FROM db_tesoreria.transaccion_caja_chica x
                    INNER JOIN db_admon.caja_chica y
                    ON x.codigo_caja_chica = y.codigo_caja_chica
                    INNER JOIN db_tesoreria.operacion z
                    ON x.codigo_operacion = z.codigo_operacion
                    INNER JOIN db_tesoreria.contribuyente w
                    ON x.nit_proveedor = w.nit
                    LEFT JOIN db_tesoreria.solicitud_correccion_caja_chica m
                    ON x.codigo_transaccion = m.codigo_transaccion
                    WHERE x.correccion = 1
                      AND x.codigo_operacion NOT IN (6,46,69,70)
                    " + filterAnioOperacion + @"
                    " + filterSemanaOperacion + @"
                    " + filterReporte;

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoSolicitaAprobacion", Constantes.Correccion.EstadoSolicitudcorreccion.SOLICITA_APROBACION);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            CajaChicaCLS objTransaccion;
                            lista = new List<CajaChicaCLS>();
                            int postCodigoTransaccion = dr.GetOrdinal("codigo_transaccion");
                            int postAnioOperacion = dr.GetOrdinal("anio_operacion");
                            int postSemanaOperacion = dr.GetOrdinal("semana_operacion");
                            int postDiaOperacion = dr.GetOrdinal("dia_operacion");
                            int postNombreDiaOperacion = dr.GetOrdinal("nombre_dia_operacion");
                            int postFechaOperacion = dr.GetOrdinal("fecha_operacion");
                            int postFechaOperacionStr = dr.GetOrdinal("fecha_operacion_str");
                            int postCodigoCajaChica = dr.GetOrdinal("codigo_caja_chica");
                            int postNombreCajaChica = dr.GetOrdinal("nombre_caja_chica");
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postNombreOperacion = dr.GetOrdinal("nombre_operacion");
                            int postNitProveedor = dr.GetOrdinal("nit_proveedor");
                            int postNombreProveedor = dr.GetOrdinal("nombre_proveedor");
                            int postSerieFactura = dr.GetOrdinal("serie_factura");
                            int postNumeroDocumento = dr.GetOrdinal("numero_documento");
                            int postFechaDocumento = dr.GetOrdinal("fecha_documento");
                            int postFechaDocumentoStr = dr.GetOrdinal("fecha_documento_str");
                            int postMonto = dr.GetOrdinal("monto");
                            int postDescripcion = dr.GetOrdinal("descripcion");
                            int postObservaciones = dr.GetOrdinal("observaciones");
                            int postExcluirFactura = dr.GetOrdinal("excluir_factura");
                            int postCodigoEstado = dr.GetOrdinal("codigo_estado");
                            int postUsuarioIng = dr.GetOrdinal("usuario_ing");
                            int postFechaIng = dr.GetOrdinal("fecha_ing");
                            int postFechaIngresoStr = dr.GetOrdinal("fecha_ing_str");
                            int postCodigoReporte = dr.GetOrdinal("codigo_reporte");
                            int postPermisoAutorizar = dr.GetOrdinal("permiso_autorizar");
                            int postCorreccion = dr.GetOrdinal("correccion");
                            int postObservacionesSolicitud = dr.GetOrdinal("observaciones_solicitud");
                            int postObservacionesAprobacion = dr.GetOrdinal("observaciones_aprobacion");
                            int postCodigoTransaccionAnt = dr.GetOrdinal("codigo_transaccion_ant");
                            int postRespuestaCorreccion = dr.GetOrdinal("respuesta_correccion");
                            int postCodigoTipoCorreccion = dr.GetOrdinal("codigo_tipo_correccion");
                            int postTipoCorreccion = dr.GetOrdinal("tipo_correccion");

                            while (dr.Read())
                            {
                                objTransaccion = new CajaChicaCLS();
                                objTransaccion.CodigoTransaccion = dr.GetInt64(postCodigoTransaccion);
                                objTransaccion.AnioOperacion = dr.GetInt16(postAnioOperacion);
                                objTransaccion.SemanaOperacion = dr.GetByte(postSemanaOperacion);
                                objTransaccion.DiaOperacion = dr.GetByte(postDiaOperacion);
                                objTransaccion.NombreDiaOperacion = dr.GetString(postNombreDiaOperacion);
                                objTransaccion.FechaOperacion = dr.GetDateTime(postFechaOperacion);
                                objTransaccion.FechaStr = dr.GetString(postFechaOperacionStr);
                                objTransaccion.CodigoCajaChica = dr.GetInt16(postCodigoCajaChica);
                                objTransaccion.NombreCajaChica = dr.GetString(postNombreCajaChica);
                                objTransaccion.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objTransaccion.Operacion = dr.GetString(postNombreOperacion);
                                objTransaccion.NitProveedor = dr.GetString(postNitProveedor);
                                objTransaccion.NombreProveedor = dr.GetString(postNombreProveedor);
                                objTransaccion.SerieFactura = dr.GetString(postSerieFactura);
                                objTransaccion.NumeroDocumento = dr.GetInt64(postNumeroDocumento);
                                objTransaccion.FechaDocumento = dr.GetDateTime(postFechaDocumento);
                                objTransaccion.FechaDocumentoStr = dr.GetString(postFechaDocumentoStr);
                                objTransaccion.Monto = dr.GetDecimal(postMonto);
                                objTransaccion.Descripcion = dr.IsDBNull(postDescripcion) ? "" : dr.GetString(postDescripcion);
                                objTransaccion.Observaciones = dr.IsDBNull(postObservaciones) ? "" : dr.GetString(postObservaciones);
                                objTransaccion.ExcluirFactura = dr.GetByte(postExcluirFactura);
                                objTransaccion.CodigoEstado = dr.GetInt16(postCodigoEstado);
                                objTransaccion.UsuarioIng = dr.GetString(postUsuarioIng);
                                objTransaccion.FechaIng = dr.GetDateTime(postFechaIng);
                                objTransaccion.FechaIngStr = dr.GetString(postFechaIngresoStr);
                                objTransaccion.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objTransaccion.PermisoAutorizar = (byte)dr.GetInt32(postPermisoAutorizar);
                                objTransaccion.Correccion = dr.GetByte(postCorreccion);
                                objTransaccion.ObservacionesSolicitud = dr.IsDBNull(postObservacionesSolicitud) ? "" : dr.GetString(postObservacionesSolicitud);
                                objTransaccion.ObservacionesAprobacion = dr.IsDBNull(postObservacionesAprobacion) ? "" : dr.GetString(postObservacionesAprobacion);
                                objTransaccion.CodigoTransaccionAnt = dr.GetInt64(postCodigoTransaccionAnt);
                                objTransaccion.RespuestaCorreccion = dr.GetString(postRespuestaCorreccion);
                                objTransaccion.CodigoTipoCorreccion = (byte)dr.GetInt32(postCodigoTipoCorreccion);
                                objTransaccion.TipoCorreccion = dr.GetString(postTipoCorreccion);


                                lista.Add(objTransaccion);
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

        public string AutorizarCorreccion(long codigoTransaccion, string observaciones, int codigoResultado, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
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
                    string sentenciaUpdateSolicitudCorreccion = @"
                    UPDATE db_tesoreria.solicitud_correccion_caja_chica
                    SET observaciones_aprobacion = @ObservacionesAprobacion,
                        resultado = @Resultado,
                        usuario_aprobacion = @UsuarioAprobacion,
                        fecha_aprobacion = @FechaAprobacion,
                        usuario_act = @UsuarioAct,
                        fecha_act = @FechaAct
                    WHERE codigo_transaccion = @CodigoTransaccion";

                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sentenciaUpdateSolicitudCorreccion;
                    cmd.Parameters.AddWithValue("@CodigoTransaccion", codigoTransaccion);
                    cmd.Parameters.AddWithValue("@ObservacionesAprobacion", observaciones);
                    cmd.Parameters.AddWithValue("@Resultado", codigoResultado);
                    cmd.Parameters.AddWithValue("@UsuarioAprobacion", usuarioAct);
                    cmd.Parameters.AddWithValue("@FechaAprobacion", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UsuarioAct", usuarioAct);
                    cmd.Parameters.AddWithValue("@FechaAct", DateTime.Now);
                    // rows sotred the number of rows affected
                    int rowSolicitud = cmd.ExecuteNonQuery();

                    string sentenciaUpdateTransaccion = @"
                    UPDATE db_tesoreria.transaccion_caja_chica
                    SET codigo_estado_solicitud_correccion = @CodigoEstadoResultadoAprobacion
                    WHERE codigo_transaccion = @CodigoTransaccion";

                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sentenciaUpdateTransaccion;
                    cmd.Parameters["@CodigoTransaccion"].Value = codigoTransaccion;
                    if (codigoResultado == Constantes.Correccion.ResultadoSolicitudCorreccion.APROBADA)
                    {
                        cmd.Parameters.AddWithValue("@CodigoEstadoResultadoAprobacion", Constantes.Correccion.EstadoSolicitudcorreccion.VISTO_BUENO);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CodigoEstadoResultadoAprobacion", Constantes.Correccion.EstadoSolicitudcorreccion.DENEGADO);
                    }
                    // rows sotred the number of rows affected
                    int rowTransaccion = cmd.ExecuteNonQuery();

                    if (rowSolicitud > 0 && rowTransaccion > 0)
                    {
                        transaction.Commit();
                        conexion.Close();
                        resultado = "OK";
                    }
                    else
                    {
                        transaction.Rollback();
                        conexion.Close();
                        resultado = "Error [0]: No se pudo registrar la transacción";
                    }

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    conexion.Close();
                    resultado = "Error [0]: " + ex.Message;
                }

                return resultado;
            }
        }

        public SolicitudCorreccionCLS GetDataCorreccion(long codigoTransaccion)
        {
            SolicitudCorreccionCLS objSolicitudCorreccion = new SolicitudCorreccionCLS();
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    SELECT codigo_transaccion, 
	                       codigo_transaccion_correcta, 
	                       observaciones_solicitud,
	                       observaciones_aprobacion,
	                       CASE	
		                      WHEN resultado = 1 THEN 'APROBADO'
		                      WHEN resultado = 2 THEN 'DENEGADO'
		                      ELSE 'Sin resultado'
	                       END AS resultado,
	                       usuario_ing AS usuario_solicitud,
	                       fecha_ing AS fecha_solicitud,
                           FORMAT(fecha_ing, 'dd/MM/yyyy, hh:mm:ss') AS fecha_solicitud_str,
	                       usuario_aprobacion,
	                       fecha_aprobacion,
                           FORMAT(fecha_aprobacion, 'dd/MM/yyyy, hh:mm:ss') AS fecha_aprobacion_str
                    FROM db_tesoreria.solicitud_correccion_caja_chica
                    WHERE codigo_transaccion = @CodigoTransaccion";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoTransaccion", codigoTransaccion);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            int postCodigoTransaccion = dr.GetOrdinal("codigo_transaccion");
                            int postCodigoTransaccionCorrecta = dr.GetOrdinal("codigo_transaccion_correcta");
                            int postObservacionesSolicitud = dr.GetOrdinal("observaciones_solicitud");
                            int postObservacionesAprobacion = dr.GetOrdinal("observaciones_aprobacion");
                            int postResultado = dr.GetOrdinal("resultado");
                            int postUsuarioSolicitud = dr.GetOrdinal("usuario_solicitud");
                            int postFechaSolicitudStr = dr.GetOrdinal("fecha_solicitud_str");
                            int postUsuarioAprobacion = dr.GetOrdinal("usuario_aprobacion");
                            int postFechaAprobacionStr = dr.GetOrdinal("fecha_aprobacion_str");
                            while (dr.Read())
                            {
                                objSolicitudCorreccion.CodigoTransaccion = dr.GetInt64(postCodigoTransaccion);
                                objSolicitudCorreccion.CodigoTransaccionCorrecta = dr.IsDBNull(postCodigoTransaccionCorrecta) ? 0 : dr.GetInt64(postCodigoTransaccionCorrecta);
                                objSolicitudCorreccion.ObservacionesSolicitud = dr.IsDBNull(postObservacionesSolicitud) ? "" : dr.GetString(postObservacionesSolicitud);
                                objSolicitudCorreccion.ObservacionesAprobacion = dr.IsDBNull(postObservacionesAprobacion) ? "" : dr.GetString(postObservacionesAprobacion);
                                objSolicitudCorreccion.Resultado = dr.GetString(postResultado);
                                objSolicitudCorreccion.UsuarioIng = dr.GetString(postUsuarioSolicitud);
                                objSolicitudCorreccion.FechaIngStr = dr.GetString(postFechaSolicitudStr);
                                objSolicitudCorreccion.UsuarioAprobacion = dr.IsDBNull(postUsuarioAprobacion) ? "" : dr.GetString(postUsuarioAprobacion);
                                objSolicitudCorreccion.FechaAprobacionStr = dr.IsDBNull(postFechaAprobacionStr) ? "" : dr.GetString(postFechaAprobacionStr);
                            }
                        }
                    }
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                }

                return objSolicitudCorreccion;
            }
        }

        public string ActualizarTransaccion(CajaChicaCLS objTransaccion, int codigoTipoActualizacion, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
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

                    string codigoSeguridad = Util.Seguridad.GenerarCadena();
                    int anio = DateTime.Now.Year;
                    string sqlSequence = "SELECT sig_valor FROM db_admon.secuencia_detalle WHERE codigo_secuencia = @codigoSecuencia AND anio = @AnioTransaccion";
                    cmd.CommandText = sqlSequence;
                    cmd.Parameters.AddWithValue("@codigoSecuencia", Constantes.Secuencia.SIT_SEQ_TRANSACCION_CAJA_CHICA);
                    cmd.Parameters.AddWithValue("@AnioTransaccion", anio);
                    // ExecuteScalar(), Executes the query, and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.
                    long siguienteValor = (long)cmd.ExecuteScalar();

                    long codigoTransaccion = long.Parse(anio.ToString() + siguienteValor.ToString("D6"));
                    string sentenciaSQL = @"
                    INSERT INTO db_tesoreria.transaccion_caja_chica( codigo_transaccion,
                                                                     codigo_reporte,   
                                                                     codigo_tipo_transaccion,
                                                                     codigo_caja_chica,
                                                                     codigo_operacion,
                                                                     nit_proveedor,	
	                                                                 serie_factura,
                                                                     numero_documento,
                                                                     fecha_documento,
                                                                     descripcion,
                                                                     fecha_operacion,
                                                                     anio_operacion,
	                                                                 semana_operacion,
                                                                     dia_operacion,
                                                                     monto,
                                                                     observaciones,
                                                                     observaciones_anulacion,
                                                                     usuario_anulacion,
	                                                                 fecha_anulacion,
                                                                     excluir_factura,
                                                                     codigo_motivo_exclusion,
                                                                     observaciones_exclusion,
                                                                     usuario_revision,
	                                                                 fecha_revision,
                                                                     codigo_estado,
                                                                     usuario_ing,
                                                                     fecha_ing,
                                                                     usuario_act,
                                                                     fecha_act,
                                                                     codigo_transaccion_ant)
                    VALUES( @codigoTransaccion,
                            @codigoReporte,
                            @codigoTipoTransaccion,
                            @codigoCajaChica,
                            @codigoOperacion,
                            @nitProveedor,	
	                        @serieFactura,
                            @numeroDocumento,
                            @fechaDocumento,
                            @descripcion,
                            @fechaOperacion,
                            @anioOperacion,
	                        @semanaOperacion,
                            @diaOperacion,
                            @monto,
                            @observaciones,
                            @observacionesAnulacion,
                            @usuarioAnulacion,
	                        @fechaAnulacion,
                            @excluirFactura,
                            @codigoMotivoExclusion,
                            @observacionesExclusion,
                            @usuarioRevision,
	                        @fechaRevision,
                            @codigoEstado,
                            @usuarioIng,
                            @fechaIng,
                            @usuarioAct,
                            @fechaAct,
                            @CodigoTransaccionAnt)";

                    cmd.CommandText = sentenciaSQL;
                    cmd.Parameters.AddWithValue("@codigoTransaccion", codigoTransaccion);
                    cmd.Parameters.AddWithValue("@codigoReporte", objTransaccion.CodigoReporte == 0 ? DBNull.Value : objTransaccion.CodigoReporte);
                    cmd.Parameters.AddWithValue("@codigoTipoTransaccion", objTransaccion.CodigoTipoTransaccion);
                    cmd.Parameters.AddWithValue("@codigoCajaChica", objTransaccion.CodigoCajaChica);
                    cmd.Parameters.AddWithValue("@codigoOperacion", objTransaccion.CodigoOperacion);
                    cmd.Parameters.AddWithValue("@nitProveedor", objTransaccion.NitProveedor);
                    cmd.Parameters.AddWithValue("@serieFactura", objTransaccion.SerieFactura.ToUpper());
                    cmd.Parameters.AddWithValue("@numeroDocumento", objTransaccion.NumeroDocumento);
                    cmd.Parameters.AddWithValue("@fechaDocumento", objTransaccion.FechaDocumento);
                    cmd.Parameters.AddWithValue("@descripcion", objTransaccion.Descripcion);
                    cmd.Parameters.AddWithValue("@fechaOperacion", objTransaccion.FechaOperacion);
                    cmd.Parameters.AddWithValue("@anioOperacion", objTransaccion.AnioOperacion);
                    cmd.Parameters.AddWithValue("@semanaOperacion", objTransaccion.SemanaOperacion);
                    cmd.Parameters.AddWithValue("@diaOperacion", Util.Conversion.DayOfWeek(objTransaccion.FechaOperacion));
                    cmd.Parameters.AddWithValue("@monto", objTransaccion.Monto);
                    cmd.Parameters.AddWithValue("@observaciones", objTransaccion.Observaciones == null ? DBNull.Value : objTransaccion.Observaciones);
                    cmd.Parameters.AddWithValue("@observacionesAnulacion", objTransaccion.ObservacionesAnulacion == null ? DBNull.Value : objTransaccion.ObservacionesAnulacion);
                    cmd.Parameters.AddWithValue("@usuarioAnulacion", objTransaccion.UsuarioAnulacion == null ? DBNull.Value : objTransaccion.UsuarioAnulacion);
                    cmd.Parameters.AddWithValue("@fechaAnulacion", objTransaccion.FechaAnulacion == null ? DBNull.Value : objTransaccion.FechaAnulacion);
                    cmd.Parameters.AddWithValue("@excluirFactura", objTransaccion.ExcluirFactura);
                    cmd.Parameters.AddWithValue("@codigoMotivoExclusion", objTransaccion.CodigoMotivoExclusion == null ? DBNull.Value : objTransaccion.CodigoMotivoExclusion);
                    cmd.Parameters.AddWithValue("@observacionesExclusion", objTransaccion.ObservacionesExclusion == null ? DBNull.Value : objTransaccion.ObservacionesExclusion);
                    cmd.Parameters.AddWithValue("@usuarioRevision", objTransaccion.UsuarioRevision == null ? DBNull.Value : objTransaccion.UsuarioRevision);
                    cmd.Parameters.AddWithValue("@fechaRevision", objTransaccion.FechaRevision == null ? DBNull.Value : objTransaccion.FechaRevision);
                    cmd.Parameters.AddWithValue("@codigoEstado", codigoTipoActualizacion == 1 ? Constantes.CajaChica.EstadoTransaccion.REGISTRADO : Constantes.CajaChica.EstadoTransaccion.DESEMBOLSADO);
                    cmd.Parameters.AddWithValue("@usuarioIng", usuarioAct);
                    cmd.Parameters.AddWithValue("@fechaIng", DateTime.Now);
                    cmd.Parameters.AddWithValue("@usuarioAct", DBNull.Value);
                    cmd.Parameters.AddWithValue("@fechaAct", DBNull.Value);
                    cmd.Parameters.AddWithValue("@CodigoTransaccionAnt", objTransaccion.CodigoTransaccion);
                    cmd.ExecuteNonQuery();


                    // Actualizar la transacción anterior
                    string sentenciaUpdateTransaccion = @"
                    UPDATE db_tesoreria.transaccion_caja_chica
                    SET codigo_estado = @CodigoEstadoTransaccionAnulado 
                    WHERE codigo_transaccion = @CodigoTransaccionAnt";
                    cmd.CommandText = sentenciaUpdateTransaccion;
                    cmd.Parameters.AddWithValue("@CodigoEstadoTransaccionAnulado", Constantes.EstadoTransacccion.ANULADO);
                    cmd.Parameters["@CodigoTransaccionAnt"].Value = objTransaccion.CodigoTransaccion;
                    cmd.ExecuteNonQuery();

                    // Actualizar la secuencia que genera el código asignado a la transacción
                    string sentenciaUpdate = @"
                    UPDATE db_admon.secuencia_detalle 
                    SET sig_valor = @siguienteValor 
                    WHERE codigo_secuencia = @CodigoSecuencia  
                    AND anio = @AnioTransaccion";
                    cmd.CommandText = sentenciaUpdate;
                    cmd.Parameters.AddWithValue("@siguienteValor", siguienteValor + 1);
                    cmd.Parameters["@CodigoSecuencia"].Value = Constantes.Secuencia.SIT_SEQ_TRANSACCION_CAJA_CHICA;
                    cmd.Parameters["@AnioTransaccion"].Value = anio;
                    cmd.ExecuteNonQuery();

                    if (codigoTipoActualizacion == 2) 
                    { // 2. Actualización desde la fase de revisión, el cual contiene una solicitud de corrección
                        sentenciaUpdate = @"
                        UPDATE db_tesoreria.solicitud_correccion_caja_chica
                        SET codigo_transaccion_correcta = @CodigoTransaccionCorrecta
                        WHERE codigo_transaccion = @CodigoTransaccionAnt";
                        
                        cmd.CommandText = sentenciaUpdate;
                        cmd.Parameters.AddWithValue("@CodigoTransaccionCorrecta", codigoTransaccion);
                        cmd.Parameters["@CodigoTransaccionAnt"].Value = objTransaccion.CodigoTransaccion;
                        cmd.ExecuteNonQuery();
                    }


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

        /// <summary>
        /// Transacción de caja chica para editar o eliminar desde el registro de la transacción
        /// </summary>
        /// <param name="codigoCajaChica"></param>
        /// <param name="usuarioIng"></param>
        /// <param name="esSuperAdmin"></param>
        /// <param name="setSemanaAnterior"></param>
        /// <returns></returns>
        public List<CajaChicaCLS> GetTransaccionesCajaChica(int codigoCajaChica, string usuarioIng, int esSuperAdmin, int setSemanaAnterior)
        {
            List<CajaChicaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string filterCajaChica = "";
                    if (codigoCajaChica == -1)
                    {
                        if (esSuperAdmin != 1) {
                            filterCajaChica = "AND x.codigo_caja_chica IN (SELECT codigo_caja_chica FROM db_admon.usuario_caja_chica WHERE id_usuario = '" + usuarioIng + "')";
                        }
                    }
                    else {
                        filterCajaChica = "AND x.codigo_caja_chica = " + codigoCajaChica.ToString();
                    }
                    conexion.Open();
                    string sql = @"
                    SELECT x.codigo_transaccion, 
                           x.anio_operacion,
                           x.semana_operacion,
                           x.dia_operacion, 
                           CASE
                             WHEN x.dia_operacion = 1 THEN 'LUNES'
                             WHEN x.dia_operacion = 2 THEN 'MARTES'
                             WHEN x.dia_operacion = 3 THEN 'MIERCOLES'
                             WHEN x.dia_operacion = 4 THEN 'JUEVES'
                             WHEN x.dia_operacion = 5 THEN 'VIERNES'
                             WHEN x.dia_operacion = 6 THEN 'SÁBADO'
                             WHEN x.dia_operacion = 7 THEN 'DOMINGO'
                             ELSE 'DESCONOCIDO'
                           END AS nombre_dia_operacion, 
	                       x.codigo_caja_chica, 
	                       y.nombre_caja_chica,
	                       x.codigo_operacion, 
	                       z.nombre_operacion,
	                       x.nit_proveedor,
                           w.nombre AS nombre_proveedor,
	                       x.serie_factura,
	                       x.numero_documento,
	                       x.fecha_documento,
	                       x.monto,
                           x.descripcion, 
	                       x.excluir_factura,
	                       x.codigo_estado,
	                       x.usuario_ing,
	                       x.fecha_ing,
                           FORMAT(x.fecha_ing, 'dd/MM/yyyy, hh:mm:ss') AS fecha_ing_str,
                           FORMAT(GETDATE(), 'dd/MM/yyyy, hh:mm:ss') AS fecha_impresion_str,
                           CASE  
                             WHEN ((x.codigo_estado = @CodigoEstadoTransaccion AND 1 = " + setSemanaAnterior.ToString() + @") OR 1 = " + esSuperAdmin.ToString() + @") THEN 1
                             ELSE 0
                           END AS permiso_anular,
                           CASE  
                             WHEN ((x.codigo_reporte IS NULL AND 0 = " + setSemanaAnterior.ToString() + @") OR 1 = " + esSuperAdmin.ToString() + @") THEN 1
                             ELSE 0
                           END AS permiso_editar

                    FROM db_tesoreria.transaccion_caja_chica x
                    INNER JOIN db_admon.caja_chica y
                    ON x.codigo_caja_chica = y.codigo_caja_chica
                    INNER JOIN db_tesoreria.operacion z
                    ON x.codigo_operacion = z.codigo_operacion
                    INNER JOIN db_tesoreria.contribuyente w
                    ON x.nit_proveedor = w.nit
                    LEFT JOIN db_tesoreria.reporte_caja_chica a
                    ON x.codigo_reporte = a.codigo_reporte
                    WHERE x.codigo_estado = @CodigoEstadoTransaccion
                      AND x.codigo_operacion NOT IN (6,46,69,70)
                      " + filterCajaChica + @"  
                    ORDER BY x.codigo_transaccion DESC";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoTransaccion", Constantes.CajaChica.EstadoTransaccion.REGISTRADO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            CajaChicaCLS objTransaccion;
                            lista = new List<CajaChicaCLS>();
                            int postCodigoTransaccion = dr.GetOrdinal("codigo_transaccion");
                            int postAnioOperacion = dr.GetOrdinal("anio_operacion");
                            int postSemanaOperacion = dr.GetOrdinal("semana_operacion");
                            int postDiaOperacion = dr.GetOrdinal("dia_operacion");
                            int postNombreDiaOperacion = dr.GetOrdinal("nombre_dia_operacion");
                            int postCodigoCajaChica = dr.GetOrdinal("codigo_caja_chica");
                            int postNombreCajaChica = dr.GetOrdinal("nombre_caja_chica");
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postNombreOperacion = dr.GetOrdinal("nombre_operacion");
                            int postNitProveedor = dr.GetOrdinal("nit_proveedor");
                            int postNombreProveedor = dr.GetOrdinal("nombre_proveedor");
                            int postSerieFactura = dr.GetOrdinal("serie_factura");
                            int postNumeroDocumento = dr.GetOrdinal("numero_documento");
                            int postFechaDocumento = dr.GetOrdinal("fecha_documento");
                            int postMonto = dr.GetOrdinal("monto");
                            int postDescripcion = dr.GetOrdinal("descripcion");
                            int postExcluirFactura = dr.GetOrdinal("excluir_factura");
                            int postCodigoEstado = dr.GetOrdinal("codigo_estado");
                            int postUsuarioIng = dr.GetOrdinal("usuario_ing");
                            int postFechaIng = dr.GetOrdinal("fecha_ing");
                            int postFechaIngresoStr = dr.GetOrdinal("fecha_ing_str");
                            int postFechaImpresion = dr.GetOrdinal("fecha_impresion_str");
                            int postPermisoAnular = dr.GetOrdinal("permiso_anular");
                            int postPermisoEditar = dr.GetOrdinal("permiso_editar");

                            while (dr.Read())
                            {
                                objTransaccion = new CajaChicaCLS();
                                objTransaccion.CodigoTransaccion = dr.GetInt64(postCodigoTransaccion);
                                objTransaccion.AnioOperacion = dr.GetInt16(postAnioOperacion);
                                objTransaccion.SemanaOperacion = dr.GetByte(postSemanaOperacion);
                                objTransaccion.DiaOperacion = dr.GetByte(postDiaOperacion);
                                objTransaccion.NombreDiaOperacion = dr.GetString(postNombreDiaOperacion);
                                objTransaccion.CodigoCajaChica = dr.GetInt16(postCodigoCajaChica);
                                objTransaccion.NombreCajaChica = dr.GetString(postNombreCajaChica);
                                objTransaccion.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objTransaccion.Operacion = dr.GetString(postNombreOperacion);
                                objTransaccion.NitProveedor = dr.GetString(postNitProveedor);
                                objTransaccion.NombreProveedor = dr.GetString(postNombreProveedor);
                                objTransaccion.SerieFactura = dr.GetString(postSerieFactura);
                                objTransaccion.NumeroDocumento = dr.GetInt64(postNumeroDocumento);
                                objTransaccion.FechaDocumento = dr.GetDateTime(postFechaDocumento);
                                objTransaccion.Monto = dr.GetDecimal(postMonto);
                                objTransaccion.Descripcion = dr.GetString(postDescripcion);
                                objTransaccion.ExcluirFactura = dr.GetByte(postExcluirFactura);
                                objTransaccion.CodigoEstado = dr.GetInt16(postCodigoEstado);
                                objTransaccion.UsuarioIng = dr.GetString(postUsuarioIng);
                                objTransaccion.FechaIng = dr.GetDateTime(postFechaIng);
                                objTransaccion.FechaIngStr = dr.GetString(postFechaIngresoStr);
                                objTransaccion.fechaImpresionStr = dr.GetString(postFechaImpresion);
                                objTransaccion.PermisoAnular = (Byte)dr.GetInt32(postPermisoAnular);
                                objTransaccion.PermisoEditar = (Byte)dr.GetInt32(postPermisoEditar);

                                lista.Add(objTransaccion);
                            }
                            conexion.Close();
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

        public List<CajaChicaCLS> GetTransaccionesCajaChicaConsulta(int codigoCajaChica, int anioOperacion, int semanaOperacion, string usuarioIng)
        {
            List<CajaChicaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string filterCajaChica = "";
                    if (codigoCajaChica == -1)
                    {
                        filterCajaChica = "AND x.codigo_caja_chica IN (SELECT codigo_caja_chica FROM db_admon.usuario_caja_chica WHERE id_usuario = '" + usuarioIng + "')";
                    }
                    else
                    {
                        filterCajaChica = "AND x.codigo_caja_chica = " + codigoCajaChica.ToString();
                    }

                    conexion.Open();
                    string sql = @"
                    SELECT x.codigo_transaccion, 
	                       x.codigo_caja_chica, 
	                       y.nombre_caja_chica,
	                       x.codigo_operacion, 
	                       z.nombre_operacion,
	                       x.nit_proveedor,
                           w.nombre AS nombre_proveedor,
	                       x.serie_factura,
	                       x.numero_documento,
	                       x.fecha_documento,
	                       x.monto,
                           x.descripcion, 
	                       x.excluir_factura,
	                       x.codigo_estado,
	                       x.usuario_ing,
	                       x.fecha_ing,
                           FORMAT(x.fecha_ing, 'dd/MM/yyyy, hh:mm:ss') AS fecha_ing_str,
                           FORMAT(GETDATE(), 'dd/MM/yyyy, hh:mm:ss') AS fecha_impresion_str

                    FROM db_tesoreria.transaccion_caja_chica x
                    INNER JOIN db_admon.caja_chica y
                    ON x.codigo_caja_chica = y.codigo_caja_chica
                    INNER JOIN db_tesoreria.operacion z
                    ON x.codigo_operacion = z.codigo_operacion
                    INNER JOIN db_tesoreria.contribuyente w
                    ON x.nit_proveedor = w.nit
                    LEFT JOIN db_tesoreria.reporte_caja_chica a
                    ON x.codigo_reporte = a.codigo_reporte
                    WHERE x.anio_operacion = @AnioOperacion 
                      AND x.semana_operacion = @SemanaOperacion
                      AND x.codigo_estado = @CodigoEstadoRegistrado 
                      AND x.codigo_operacion NOT IN (6,46,69,70)  
                      " + filterCajaChica + @"  
                    ORDER BY x.codigo_transaccion DESC";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@AnioOperacion", anioOperacion);
                        cmd.Parameters.AddWithValue("@SemanaOperacion", semanaOperacion);
                        cmd.Parameters.AddWithValue("@CodigoEstadoRegistrado", Constantes.CajaChica.EstadoTransaccion.REGISTRADO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            CajaChicaCLS objTransaccion;
                            lista = new List<CajaChicaCLS>();
                            int postCodigoTransaccion = dr.GetOrdinal("codigo_transaccion");
                            int postCodigoCajaChica = dr.GetOrdinal("codigo_caja_chica");
                            int postNombreCajaChica = dr.GetOrdinal("nombre_caja_chica");
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postNombreOperacion = dr.GetOrdinal("nombre_operacion");
                            int postNitProveedor = dr.GetOrdinal("nit_proveedor");
                            int postNombreProveedor = dr.GetOrdinal("nombre_proveedor");
                            int postSerieFactura = dr.GetOrdinal("serie_factura");
                            int postNumeroDocumento = dr.GetOrdinal("numero_documento");
                            int postFechaDocumento = dr.GetOrdinal("fecha_documento");
                            int postMonto = dr.GetOrdinal("monto");
                            int postDescripcion = dr.GetOrdinal("descripcion");
                            int postExcluirFactura = dr.GetOrdinal("excluir_factura");
                            int postCodigoEstado = dr.GetOrdinal("codigo_estado");
                            int postUsuarioIng = dr.GetOrdinal("usuario_ing");
                            int postFechaIng = dr.GetOrdinal("fecha_ing");
                            int postFechaIngresoStr = dr.GetOrdinal("fecha_ing_str");
                            int postFechaImpresion = dr.GetOrdinal("fecha_impresion_str");

                            while (dr.Read())
                            {
                                objTransaccion = new CajaChicaCLS();
                                objTransaccion.CodigoTransaccion = dr.GetInt64(postCodigoTransaccion);
                                objTransaccion.CodigoCajaChica = dr.GetInt16(postCodigoCajaChica);
                                objTransaccion.NombreCajaChica = dr.GetString(postNombreCajaChica);
                                objTransaccion.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objTransaccion.Operacion = dr.GetString(postNombreOperacion);
                                objTransaccion.NitProveedor = dr.GetString(postNitProveedor);
                                objTransaccion.NombreProveedor = dr.GetString(postNombreProveedor);
                                objTransaccion.SerieFactura = dr.GetString(postSerieFactura);
                                objTransaccion.NumeroDocumento = dr.GetInt64(postNumeroDocumento);
                                objTransaccion.FechaDocumento = dr.GetDateTime(postFechaDocumento);
                                objTransaccion.Monto = dr.GetDecimal(postMonto);
                                objTransaccion.Descripcion = dr.GetString(postDescripcion);
                                objTransaccion.ExcluirFactura = dr.GetByte(postExcluirFactura);
                                objTransaccion.CodigoEstado = dr.GetInt16(postCodigoEstado);
                                objTransaccion.UsuarioIng = dr.GetString(postUsuarioIng);
                                objTransaccion.FechaIng = dr.GetDateTime(postFechaIng);
                                objTransaccion.FechaIngStr = dr.GetString(postFechaIngresoStr);
                                objTransaccion.fechaImpresionStr = dr.GetString(postFechaImpresion);

                                lista.Add(objTransaccion);
                            }
                            conexion.Close();
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

        /// <summary>
        /// Retorna las transacciones de caja chica asociadas a un reporte específico, en la etapa de correción
        /// </summary>
        /// <param name="codigoReporte"></param>
        /// <returns></returns>
        public List<CajaChicaCLS> ListarTransaccionesCajaChica(int codigoReporte)
        {
            List<CajaChicaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT x.codigo_transaccion, 
                           x.codigo_reporte, 
	                       x.codigo_caja_chica, 
	                       y.nombre_caja_chica,
	                       x.codigo_operacion, 
	                       z.nombre_operacion,
	                       x.nit_proveedor,
                           w.nombre AS nombre_proveedor,
	                       x.serie_factura,
	                       x.numero_documento,
	                       x.fecha_documento,
	                       x.monto,
                           x.descripcion, 
	                       x.excluir_factura,
	                       x.codigo_estado,
	                       x.usuario_ing,
	                       x.fecha_ing,
                           CASE  
                             WHEN x.codigo_reporte IS NULL THEN 1
                             ELSE 0
                           END AS permiso_anular,
                           CASE  
                             WHEN (x.codigo_reporte IS NULL OR x.codigo_estado_solicitud_correccion = " + Constantes.Correccion.EstadoSolicitudcorreccion.VISTO_BUENO.ToString() + @") THEN 1
                             ELSE 0
                           END AS permiso_editar,
                           CASE
                             WHEN (a.codigo_estado = @CodigoEstadoReporteEnRevision AND x.codigo_estado_solicitud_correccion = @CodigoEstadoSolicitudCorreccion) THEN 1
                             ELSE 0
                           END AS permiso_corregir,
                           COALESCE(x.codigo_transaccion_ant,0) AS codigo_transaccion_ant,
                           x.codigo_estado_solicitud_correccion,
                           h.nombre AS estado_solicitud_correccion,
                           COALESCE(f.codigo_transaccion_correcta,0) AS codigo_transaccion_correcta,
                           x.correccion

                    FROM db_tesoreria.transaccion_caja_chica x
                    INNER JOIN db_admon.caja_chica y
                    ON x.codigo_caja_chica = y.codigo_caja_chica
                    INNER JOIN db_tesoreria.operacion z
                    ON x.codigo_operacion = z.codigo_operacion
                    INNER JOIN db_tesoreria.contribuyente w
                    ON x.nit_proveedor = w.nit
                    LEFT JOIN db_tesoreria.reporte_caja_chica a
                    ON x.codigo_reporte = a.codigo_reporte
                    LEFT JOIN db_tesoreria.solicitud_correccion_caja_chica f
                    ON x.codigo_transaccion = f.codigo_transaccion
                    INNER JOIN db_tesoreria.estado_solicitud_correccion h
                    ON x.codigo_estado_solicitud_correccion = h.codigo_estado_solicitud_correccion
                    WHERE x.codigo_reporte = @CodigoReporte
                      AND x.codigo_estado <> 0
                      AND x.codigo_operacion NOT IN (6,46,69,70)  
                    ORDER BY x.codigo_transaccion DESC";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoReporte", codigoReporte);
                        cmd.Parameters.AddWithValue("@CodigoEstadoReporteEnRevision", Constantes.CajaChica.EstadoReporte.EN_REVISION);
                        cmd.Parameters.AddWithValue("@CodigoEstadoSolicitudCorreccion", Constantes.Correccion.EstadoSolicitudcorreccion.SIN_SOLICITUD);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            CajaChicaCLS objTransaccion;
                            lista = new List<CajaChicaCLS>();
                            int postCodigoTransaccion = dr.GetOrdinal("codigo_transaccion");
                            int postCodigoReporte = dr.GetOrdinal("codigo_reporte");
                            int postCodigoCajaChica = dr.GetOrdinal("codigo_caja_chica");
                            int postNombreCajaChica = dr.GetOrdinal("nombre_caja_chica");
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postNombreOperacion = dr.GetOrdinal("nombre_operacion");
                            int postNitProveedor = dr.GetOrdinal("nit_proveedor");
                            int postNombreProveedor = dr.GetOrdinal("nombre_proveedor");
                            int postSerieFactura = dr.GetOrdinal("serie_factura");
                            int postNumeroDocumento = dr.GetOrdinal("numero_documento");
                            int postFechaDocumento = dr.GetOrdinal("fecha_documento");
                            int postMonto = dr.GetOrdinal("monto");
                            int postDescripcion = dr.GetOrdinal("descripcion");
                            int postExcluirFactura = dr.GetOrdinal("excluir_factura");
                            int postCodigoEstado = dr.GetOrdinal("codigo_estado");
                            int postUsuarioIng = dr.GetOrdinal("usuario_ing");
                            int postFechaIng = dr.GetOrdinal("fecha_ing");
                            int postPermisoAnular = dr.GetOrdinal("permiso_anular");
                            int postPermisoEditar = dr.GetOrdinal("permiso_editar");
                            int postPermisoCorregir = dr.GetOrdinal("permiso_corregir");
                            int postCorreccion = dr.GetOrdinal("correccion");
                            int postCodigoTransaccionAnt = dr.GetOrdinal("codigo_transaccion_ant");
                            int postCodigoEstadoSolicitudCorreccion = dr.GetOrdinal("codigo_estado_solicitud_correccion");
                            int postEstadoSolicitudCorreccion = dr.GetOrdinal("estado_solicitud_correccion");
                            int postCodigoTransaccionCorrecta = dr.GetOrdinal("codigo_transaccion_correcta");

                            while (dr.Read())
                            {
                                objTransaccion = new CajaChicaCLS();
                                objTransaccion.CodigoTransaccion = dr.GetInt64(postCodigoTransaccion);
                                objTransaccion.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objTransaccion.CodigoCajaChica = dr.GetInt16(postCodigoCajaChica);
                                objTransaccion.NombreCajaChica = dr.GetString(postNombreCajaChica);
                                objTransaccion.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objTransaccion.Operacion = dr.GetString(postNombreOperacion);
                                objTransaccion.NitProveedor = dr.GetString(postNitProveedor);
                                objTransaccion.NombreProveedor = dr.GetString(postNombreProveedor);
                                objTransaccion.SerieFactura = dr.GetString(postSerieFactura);
                                objTransaccion.NumeroDocumento = dr.GetInt64(postNumeroDocumento);
                                objTransaccion.FechaDocumento = dr.GetDateTime(postFechaDocumento);
                                objTransaccion.Monto = dr.GetDecimal(postMonto);
                                objTransaccion.Descripcion = dr.GetString(postDescripcion);
                                objTransaccion.ExcluirFactura = dr.GetByte(postExcluirFactura);
                                objTransaccion.CodigoEstado = dr.GetInt16(postCodigoEstado);
                                objTransaccion.UsuarioIng = dr.GetString(postUsuarioIng);
                                objTransaccion.FechaIng = dr.GetDateTime(postFechaIng);
                                objTransaccion.PermisoAnular = (Byte)dr.GetInt32(postPermisoAnular);
                                objTransaccion.PermisoEditar = (Byte)dr.GetInt32(postPermisoEditar);
                                objTransaccion.PermisoCorregir = (Byte)dr.GetInt32(postPermisoCorregir);
                                objTransaccion.Correccion = dr.GetByte(postCorreccion);
                                objTransaccion.CodigoTransaccionAnt = dr.GetInt64(postCodigoTransaccionAnt);
                                objTransaccion.CodigoEstadoSolicitudCorreccion = dr.GetByte(postCodigoEstadoSolicitudCorreccion);
                                objTransaccion.EstadoSolicitudCorreccion = dr.GetString(postEstadoSolicitudCorreccion);
                                objTransaccion.CodigoTransaccionCorrecta = dr.GetInt64(postCodigoTransaccionCorrecta);
                                lista.Add(objTransaccion);
                            }
                            conexion.Close();
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

        public Boolean ExisteFactura(string nitProveedor, string serieFactura, long numeroFactura)
        {
            bool existeFactura = true;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT COUNT(*) AS cantidad 
                    FROM db_tesoreria.transaccion_caja_chica 
                    WHERE nit_proveedor = @Nit
                      AND serie_factura = @SerieFactura
                      AND numero_documento = @NumeroDocumento
                      AND codigo_estado <> @CodigoEstadoAnulado";
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Nit", nitProveedor);
                        cmd.Parameters.AddWithValue("@SerieFactura", serieFactura.ToUpper());
                        cmd.Parameters.AddWithValue("@NumeroDocumento", numeroFactura);
                        cmd.Parameters.AddWithValue("@CodigoEstadoAnulado", Constantes.CajaChica.EstadoTransaccion.ANULADO);
                        int resultado = (int)cmd.ExecuteScalar();
                        if (resultado == 0)
                        {
                            existeFactura = false;
                        }
                        
                    }
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                }
            }
            return existeFactura;
        }

        public string ExcluirFacturaDeCajaChica(long codigoTransaccion, int excluirFactura, int codigoMotivoExclusion, string usuarioIng, string observaciones)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    UPDATE db_tesoreria.transaccion_caja_chica 
                    SET excluir_factura = @ExcluirFactura,
                        usuario_revision = @UsuarioRevision,
                        codigo_motivo_exclusion = @CodigoMotivoExclusion,
                        observaciones_exclusion = @ObservacionesExclusion,
                        codigo_tipo_transaccion = @CodigoTipoTransaccion
                    WHERE codigo_transaccion = @CodigoTransaccion";
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@ExcluirFactura", excluirFactura);
                        cmd.Parameters.AddWithValue("@CodigoTransaccion", codigoTransaccion);
                        cmd.Parameters.AddWithValue("@CodigoMotivoExclusion", codigoMotivoExclusion == -1 ? DBNull.Value : codigoMotivoExclusion);
                        cmd.Parameters.AddWithValue("@UsuarioRevision", usuarioIng);
                        cmd.Parameters.AddWithValue("@ObservacionesExclusion", observaciones == null ? DBNull.Value : observaciones);
                        cmd.Parameters.AddWithValue("@CodigoTipoTransaccion", excluirFactura == 0 ? "F" : "NF");

                        // rows sotred the number of rows affected
                        int rows = cmd.ExecuteNonQuery();
                        conexion.Close();
                    }
                    resultado = "OK";
                }
                catch (Exception ex)
                {
                    resultado = "Error [0]: " + ex.Message;
                    conexion.Close();
                }

                return resultado;
            }
        }

        public string AnularTransaccionCajaChica(long codigoTransaccion, string observaciones, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    UPDATE db_tesoreria.transaccion_caja_chica 
                    SET codigo_estado = @EstadoAnulado,
                        observaciones_anulacion = @ObservacionesAnulacion,
                        usuario_anulacion = @UsuarioAnulacion,
                        fecha_anulacion = @FechaAnulacion    
                    WHERE codigo_transaccion = @CodigoTransaccion";
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@EstadoAnulado", Constantes.CajaChica.EstadoTransaccion.ANULADO);
                        cmd.Parameters.AddWithValue("@ObservacionesAnulacion", observaciones == null ? DBNull.Value : observaciones);
                        cmd.Parameters.AddWithValue("@UsuarioAnulacion", usuarioAct);
                        cmd.Parameters.AddWithValue("@FechaAnulacion", DateTime.Now);
                        cmd.Parameters.AddWithValue("@CodigoTransaccion", codigoTransaccion);
                        // rows sotred the number of rows affected
                        int rows = cmd.ExecuteNonQuery();
                        conexion.Close();
                    }
                    resultado = "OK";
                }
                catch (Exception ex)
                {
                    resultado = "Error [0]: " + ex.Message;
                    conexion.Close();
                }

                return resultado;
            }
        }

        public string AnularMovimientoCajaChica(long codigoTransaccion, string observaciones, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    int rows = 0;
                    string sql = @"
                    UPDATE db_tesoreria.transaccion_caja_chica 
                    SET codigo_estado = @EstadoAnulado,
                        observaciones_anulacion = @ObservacionesAnulacion,
                        usuario_anulacion = @UsuarioAnulacion,
                        fecha_anulacion = @FechaAnulacion    
                    WHERE codigo_transaccion = @CodigoTransaccion
                      AND codigo_estado_recepcion = @CodigoEstadoRecepcionPorRecepcionar";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@EstadoAnulado", Constantes.CajaChica.EstadoTransaccion.ANULADO);
                        cmd.Parameters.AddWithValue("@ObservacionesAnulacion", observaciones == null ? DBNull.Value : observaciones);
                        cmd.Parameters.AddWithValue("@UsuarioAnulacion", usuarioAct);
                        cmd.Parameters.AddWithValue("@FechaAnulacion", DateTime.Now);
                        cmd.Parameters.AddWithValue("@CodigoTransaccion", codigoTransaccion);
                        cmd.Parameters.AddWithValue("@CodigoEstadoRecepcionPorRecepcionar", Constantes.CajaChica.EstadoRecepcion.POR_RECEPCIONAR);
                        // rows sotred the number of rows affected
                        rows = cmd.ExecuteNonQuery();
                        conexion.Close();
                    }

                    if (rows > 0)
                        resultado = "OK";
                    else
                        resultado = "Error: No se pudo anular la transacción, puede que el estado de la transacción ya haya cambiado";
                }
                catch (Exception ex)
                {
                    resultado = "Error [0]: " + ex.Message;
                    conexion.Close();
                }

                return resultado;
            }
        }

        public string ActualizarTransaccionCajaChica(long codigoTransaccion, int codigoOperacion, string descripcion, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    UPDATE db_tesoreria.transaccion_caja_chica 
                    SET codigo_operacion = @CodigoOperacion,
                        descripcion = @Descripcion,
                        usuario_act = @UsuarioActualizacion,
                        fecha_act = @FechaActualizacion
                    WHERE codigo_transaccion = @CodigoTransaccion";
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoOperacion", codigoOperacion);
                        cmd.Parameters.AddWithValue("@Descripcion", descripcion);
                        cmd.Parameters.AddWithValue("@UsuarioActualizacion", usuarioAct);
                        cmd.Parameters.AddWithValue("@FechaActualizacion", DateTime.Now);
                        cmd.Parameters.AddWithValue("@CodigoTransaccion", codigoTransaccion);

                        // rows sotred the number of rows affected
                        int rows = cmd.ExecuteNonQuery();
                        conexion.Close();
                    }
                    resultado = "OK";
                }
                catch (Exception ex)
                {
                    resultado = "Error [0]: " + ex.Message;
                    conexion.Close();
                }

                return resultado;
            }
        }

        public List<MotivoExclusionCLS> GetMotivosExclusionDeFacturas()
        {
            List<MotivoExclusionCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT codigo_motivo_exclusion, 
                           nombre
                    FROM db_tesoreria.motivo_exclusion
                    WHERE estado = @EstadoRegistro
                    ORDER BY nombre ASC";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@EstadoRegistro", Constantes.EstadoRegistro.ACTIVO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            MotivoExclusionCLS objMotivo;
                            lista = new List<MotivoExclusionCLS>();

                            int postCodigoMotivo = dr.GetOrdinal("codigo_motivo_exclusion");
                            int postNombre = dr.GetOrdinal("nombre");
                            while (dr.Read())
                            {
                                objMotivo = new MotivoExclusionCLS();
                                objMotivo.CodigoMotivoExclusion = dr.GetByte(postCodigoMotivo);
                                objMotivo.Nombre = dr.GetString(postNombre);
                                lista.Add(objMotivo);
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

        public string ActualizarMontoDisponibleCajaChica(int codigoCajaChica, int codigoOperacion, int anioOperacion, int semanaOperacion, decimal monto, string observaciones, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_tesoreria.uspModificarMontoAsignadoCajaChica", conexion))
                    {
                        // le indico que es un procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CodigoCajaChica", codigoCajaChica);
                        cmd.Parameters.AddWithValue("@CodigoOperacion", codigoOperacion);
                        cmd.Parameters.AddWithValue("@AnioOperacion", anioOperacion);
                        cmd.Parameters.AddWithValue("@SemanaOperacion", semanaOperacion);
                        cmd.Parameters.AddWithValue("@DiaOperacion", Util.Conversion.DayOfWeek(DateTime.Now));
                        cmd.Parameters.AddWithValue("@Monto", monto);
                        cmd.Parameters.AddWithValue("@Observaciones", observaciones == null ? "" : observaciones);
                        cmd.Parameters.AddWithValue("@UsuarioAct", usuarioAct);

                        //Set SqlParameter
                        SqlParameter outParameter = new SqlParameter
                        {
                            ParameterName = "@Resultado", //Parameter name defined in stored procedure
                            SqlDbType = SqlDbType.Int, //Data Type of Parameter
                            Direction = ParameterDirection.Output //Specify the parameter as ouput
                        };

                        //add the parameter to the SqlCommand object
                        cmd.Parameters.Add(outParameter);
                        cmd.ExecuteReader();
                        resultado = outParameter.Value.ToString();

                    }// fin using
                    conexion.Close();
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    resultado = "Error [0]: " + ex.Message;
                }
                return resultado;
            }

        }

        /// <summary>
        /// Actualización del límite de la caja chica y el saldo actual, no se registra ningun transacción para ello, y es utilizado solo por el superadministrador. Esto es para realizar correcciones
        /// de montos, en caso de que se requiera modificarlos directamente
        /// </summary>
        /// <param name="codigoCajaChica"></param>
        /// <param name="montoLimite"></param>
        /// <param name="montoDisponible"></param>
        /// <param name="observaciones"></param>
        /// <param name="usuarioAct"></param>
        /// <returns></returns>
        public string ActualizarMontosCajaChicaAdmin(int codigoCajaChica, decimal montoLimite, decimal montoDisponible, string observaciones, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    UPDATE db_admon.caja_chica 
                    SET monto_limite = @MontoLimite,
                        monto_disponible = @MontoDisponible,
                        usuario_act = @UsuarioAct,
                        fecha_act = @FechaAct
                    WHERE codigo_caja_chica = @CodigoCajaChica";
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@MontoLimite", montoLimite);
                        cmd.Parameters.AddWithValue("@MontoDisponible", montoDisponible);
                        cmd.Parameters.AddWithValue("@UsuarioAct", usuarioAct);
                        cmd.Parameters.AddWithValue("@FechaAct", DateTime.Now);
                        cmd.Parameters.AddWithValue("@CodigoCajaChica", codigoCajaChica);

                        // rows sotred the number of rows affected
                        cmd.ExecuteNonQuery();
                        conexion.Close();
                    }
                    resultado = "OK";
                }
                catch (Exception ex)
                {
                    resultado = "Error [0]: " + ex.Message;
                    conexion.Close();
                }

                return resultado;
            }
        }

        public decimal GetSaldoActual(int codigoCajaChica)
        {
            decimal montoSaldo = 0;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    SELECT (monto_disponible - (SELECT COALESCE(SUM(monto),0) AS monto FROM db_tesoreria.transaccion_caja_chica WHERE codigo_estado IN (@CodigoEstadoTransaccionRegistrado,@CodigoEstadoTransaccionIncluidoEnReporte)  AND codigo_caja_chica = @CodigoCajaChica AND codigo_operacion NOT IN (6,46,69,70))) AS saldo
                    FROM db_admon.caja_chica 
                    WHERE codigo_caja_chica = @CodigoCajaChica";
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoTransaccionRegistrado", Constantes.EstadoTransacccion.REGISTRADO);
                        cmd.Parameters.AddWithValue("@CodigoEstadoTransaccionIncluidoEnReporte", Constantes.EstadoTransacccion.INCLUIDO_EN_REPORTE);
                        cmd.Parameters.AddWithValue("@CodigoCajaChica", codigoCajaChica);

                        // rows sotred the number of rows affected
                        montoSaldo = (decimal)cmd.ExecuteScalar();
                    }
                    conexion.Close();
                }
                catch (Exception)
                {
                    montoSaldo = -1;
                    conexion.Close();
                }

                return montoSaldo;
            }
        }

        public List<CajaChicaCLS> GetTransaccionesPorRecepcionarEnTesoreria(string usuarioIng, int esSuperAdmin)
        {
            List<CajaChicaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string filterCajaChica = String.Empty;
                    if (esSuperAdmin != 1)
                    {
                        filterCajaChica = "AND x.codigo_caja_chica IN (SELECT codigo_caja_chica FROM db_admon.usuario_caja_chica WHERE id_usuario = '" + usuarioIng + "')";
                    }

                    conexion.Open();
                    string sql = @"
                    SELECT x.codigo_transaccion, 
		                   COALESCE(x.codigo_reporte,0) AS codigo_reporte,
		                   x.codigo_caja_chica,
		                   y.nombre_caja_chica,
		                   x.codigo_operacion,
		                   z.nombre_operacion,
		                   x.anio_operacion,
		                   x.semana_operacion,
		                   x.monto,
		                   x.usuario_ing,
		                   x.fecha_ing,
                           FORMAT(x.fecha_ing, 'dd/MM/yyyy, hh:mm:ss') AS fecha_ing_str,
		                   x.codigo_estado_recepcion,
		                   CASE			
			                 WHEN x.codigo_estado_recepcion = 0 THEN 'NO APLICA'
			                 WHEN x.codigo_estado_recepcion = 1 THEN 'POR RECEPCIONAR'
			                 WHEN x.codigo_estado_recepcion = 2 THEN 'RECEPCIONADO'
			                 WHEN x.codigo_estado_recepcion = 3 THEN 'REGISTRADO'
			                 ELSE 'NO DEFINIDO'
		                   END AS estado_recepcion,
                           x.observaciones 

                    FROM db_tesoreria.transaccion_caja_chica x
                    INNER JOIN db_admon.caja_chica y
                    ON x.codigo_caja_chica = y.codigo_caja_chica
                    INNER JOIN db_tesoreria.operacion z
                    ON x.codigo_operacion = z.codigo_operacion
                    WHERE x.codigo_estado <> 0
                      AND x.codigo_estado_recepcion = @CodigoEstadoPorRecepcionar
                      AND x.codigo_operacion in (6,69,70)
                      " + filterCajaChica + @"  
                    ORDER BY x.codigo_transaccion ASC";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoPorRecepcionar", Constantes.CajaChica.EstadoRecepcion.POR_RECEPCIONAR);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            CajaChicaCLS objTransaccion;
                            lista = new List<CajaChicaCLS>();
                            int postCodigoTransaccion = dr.GetOrdinal("codigo_transaccion");
                            int postCodigoReporte = dr.GetOrdinal("codigo_reporte");
                            int postCodigoCajaChica = dr.GetOrdinal("codigo_caja_chica");
                            int postNombreCajaChica = dr.GetOrdinal("nombre_caja_chica");
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postNombreOperacion = dr.GetOrdinal("nombre_operacion");
                            int postAnioOperacion = dr.GetOrdinal("anio_operacion");
                            int postSemanaOperacion = dr.GetOrdinal("semana_operacion");
                            int postMonto = dr.GetOrdinal("monto");
                            int postUsuarioIng = dr.GetOrdinal("usuario_ing");
                            int postFechaIngStr = dr.GetOrdinal("fecha_ing_str");
                            int postCodigoEstadoRecepcion = dr.GetOrdinal("codigo_estado_recepcion");
                            int postEstadoRecepcion = dr.GetOrdinal("estado_recepcion");
                            int postObservaciones = dr.GetOrdinal("observaciones");
                            while (dr.Read())
                            {
                                objTransaccion = new CajaChicaCLS();
                                objTransaccion.CodigoTransaccion = dr.GetInt64(postCodigoTransaccion);
                                objTransaccion.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objTransaccion.CodigoCajaChica = dr.GetInt16(postCodigoCajaChica);
                                objTransaccion.NombreCajaChica = dr.GetString(postNombreCajaChica);
                                objTransaccion.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objTransaccion.Operacion = dr.GetString(postNombreOperacion);
                                objTransaccion.AnioOperacion = dr.GetInt16(postAnioOperacion);
                                objTransaccion.SemanaOperacion = dr.GetByte(postSemanaOperacion);
                                objTransaccion.Monto = dr.GetDecimal(postMonto);
                                objTransaccion.UsuarioIng = dr.GetString(postUsuarioIng);
                                objTransaccion.FechaIngStr = dr.GetString(postFechaIngStr);
                                objTransaccion.CodigoEstadoRecepcion = dr.GetByte(postCodigoEstadoRecepcion);
                                objTransaccion.EstadoRecepcion = dr.GetString(postEstadoRecepcion);
                                objTransaccion.Observaciones = dr.IsDBNull(postObservaciones) ? "" : dr.GetString(postObservaciones);
                                lista.Add(objTransaccion);
                            }
                            conexion.Close();
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

        /// <summary>
        /// Recepción de operaciones de reembolos, disminución o Abono a Caja chica
        /// </summary>
        /// <returns></returns>
        public string RecepcionarTransaccion(TransaccionCajaChicaCLS objTransaccion, string usuarioAct, int diaOperacion, DateTime fechaOperacion)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_tesoreria.uspRecepcionarTransaccionCajaChica", conexion))
                    {
                        if (objTransaccion.FechaChequeStr != null)
                        {
                            objTransaccion.FechaCheque = Util.Conversion.ConvertDateSpanishToEnglish(objTransaccion.FechaChequeStr);
                        }
                        else {
                            objTransaccion.FechaCheque = new DateTime(2000, 1, 1);
                        }

                        // le indico que es un procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CodigoTransaccionAnt", objTransaccion.CodigoTransaccion);
                        cmd.Parameters.AddWithValue("@CodigoReporte", objTransaccion.CodigoReporte);
                        cmd.Parameters.AddWithValue("@CodigoCajaChica", objTransaccion.CodigoCajaChica);
                        cmd.Parameters.AddWithValue("@CodigoOperacion", objTransaccion.CodigoOperacion);
                        cmd.Parameters.AddWithValue("@AnioOperacion", objTransaccion.AnioOperacion);
                        cmd.Parameters.AddWithValue("@SemanaOperacion", objTransaccion.SemanaOperacion);

                        cmd.Parameters.AddWithValue("@DiaOperacion", diaOperacion);
                        cmd.Parameters.AddWithValue("@FechaOperacion", fechaOperacion);

                        cmd.Parameters.AddWithValue("@CodigoBanco", objTransaccion.CodigoBanco);
                        cmd.Parameters.AddWithValue("@NumeroCheque", objTransaccion.NumeroCheque == null ? "" : objTransaccion.NumeroCheque);
                        cmd.Parameters.AddWithValue("@FechaCheque", objTransaccion.FechaCheque);
                        cmd.Parameters.AddWithValue("@Monto", objTransaccion.Monto);
                        cmd.Parameters.AddWithValue("@Observaciones", objTransaccion.Observaciones == null ? "" : objTransaccion.Observaciones);
                        cmd.Parameters.AddWithValue("@ObservacionesRecepcion", objTransaccion.ObservacionesRecepcion == null ? "" : objTransaccion.ObservacionesRecepcion);
                        cmd.Parameters.AddWithValue("@UsuarioAct", usuarioAct);

                        //Set SqlParameter
                        SqlParameter outParameter = new SqlParameter
                        {
                            ParameterName = "@Resultado", //Parameter name defined in stored procedure
                            SqlDbType = SqlDbType.Int, //Data Type of Parameter
                            Direction = ParameterDirection.Output //Specify the parameter as ouput
                        };

                        //add the parameter to the SqlCommand object
                        cmd.Parameters.Add(outParameter);
                        cmd.ExecuteReader();
                        resultado = outParameter.Value.ToString();

                    }// fin using
                    conexion.Close();
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    resultado = "Error [0]: " + ex.Message;
                }
                return resultado;
            }

        }// fin metodo

    }
}
