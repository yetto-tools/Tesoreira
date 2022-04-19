using CapaEntidad.Administracion;
using CapaEntidad.Contabilidad;
using CapaEntidad.Tesoreria;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Contabilidad
{
    public class ReportesTesoreriaDAL: CadenaConexion
    {

        public List<ReporteCajaChicaCLS> GetCortesCajaChica(int anioOperacion, int codigoCajaChica)
        {
            List<ReporteCajaChicaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string filterAnioOperacion = String.Empty;
                    string filterCajaChica = String.Empty;
                    if (anioOperacion != 0) {
                        filterAnioOperacion = "AND x.anio_operacion = " + anioOperacion.ToString();
                    }

                    if (codigoCajaChica != -1) {
                        filterCajaChica = "AND x.codigo_caja_chica = " + codigoCajaChica.ToString();
                    }

                    string sql = @"
                    SELECT x.codigo_reporte, 
                           @CodigoTipoReporte AS codigo_tipo_reporte,
                           x.anio_operacion, 
                           x.semana_operacion,
                           db_admon.GetPeriodoSemana(x.anio_operacion,x.semana_operacion) AS semana, 
                           x.observaciones, 
                           x.codigo_estado,
                           y.nombre AS estado,
                           x.usuario_ing, 
                           x.fecha_ing,
                           FORMAT(x.fecha_ing, 'dd/MM/yyyy, hh:mm:ss') AS fecha_ing_str,
                           x.arqueo,
                           CASE
                              WHEN x.arqueo = 1 THEN 'ARQUEO'
                              ELSE ''
                           END AS tipo,
                           z.nombre_controlador,
                           z.nombre_accion,
                           CASE
                             WHEN @CodigoTipoReporte = " + Constantes.Reporte.CIERRE.ToString() + @" AND x.arqueo = 1 THEN 0
                             ELSE z.pdf
                           END AS pdf,
                           CASE
                             WHEN @CodigoTipoReporte = " + Constantes.Reporte.CIERRE.ToString() + @" AND x.arqueo = 1 THEN 0
                             ELSE z.excel
                           END AS excel,
                           CASE
                              WHEN @CodigoTipoReporte = " + Constantes.Reporte.CIERRE.ToString() + @" AND x.arqueo = 1 THEN 0
                              ELSE z.web
                           END AS web,
                           w.nombre_caja_chica

                        FROM db_tesoreria.reporte_caja_chica x
                        INNER JOIN db_tesoreria.estado_reporte_caja_chica y
                        ON x.codigo_estado = y.codigo_estado
                        INNER JOIN  db_admon.tipo_reporte z
                        ON @CodigoTipoReporte = z.codigo_tipo_reporte
                        INNER JOIN  db_admon.caja_chica w
                        ON x.codigo_caja_chica = w.codigo_caja_chica
                        WHERE x.codigo_estado <> @CodigoEstadoAnulado
                        " + filterAnioOperacion + @"
                        " + filterCajaChica + @"
                        ORDER BY x.codigo_reporte DESC";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoAnulado", Constantes.ReporteCaja.Estado.ANULADO);
                        cmd.Parameters.AddWithValue("@CodigoTipoReporte", Constantes.Reporte.CORTE_CAJA_CHICA);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            ReporteCajaChicaCLS objReporteCaja;
                            lista = new List<ReporteCajaChicaCLS>();
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
                            int postArqueo = dr.GetOrdinal("arqueo");
                            int postTipo = dr.GetOrdinal("tipo");
                            int postCodigoTipoReporte = dr.GetOrdinal("codigo_tipo_reporte");
                            int postNombreControlador = dr.GetOrdinal("nombre_controlador");
                            int postNombreAccion = dr.GetOrdinal("nombre_accion");
                            int postPdf = dr.GetOrdinal("pdf");
                            int postExcel = dr.GetOrdinal("excel");
                            int postWeb = dr.GetOrdinal("web");
                            int postNombreCajaChica = dr.GetOrdinal("nombre_caja_chica");
                            while (dr.Read())
                            {
                                objReporteCaja = new ReporteCajaChicaCLS();
                                objReporteCaja.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objReporteCaja.AnioOperacion = dr.GetInt16(postAnio);
                                objReporteCaja.SemanaOperacion = dr.GetByte(postNumeroSemana);
                                objReporteCaja.Periodo = dr.GetString(postSemana);
                                objReporteCaja.Observaciones = dr.IsDBNull(postObservaciones) ? "" : dr.GetString(postObservaciones);
                                objReporteCaja.CodigoEstado = dr.GetByte(postCodigoEstado);
                                objReporteCaja.Estado = dr.GetString(postEstado);
                                objReporteCaja.UsuarioIng = dr.GetString(postUsuarioIng);
                                objReporteCaja.FechaIng = dr.GetDateTime(postFechaIng);
                                objReporteCaja.FechaIngStr = dr.GetString(postFechaIngStr);
                                objReporteCaja.Arqueo = dr.GetByte(postArqueo);
                                objReporteCaja.Tipo = dr.GetString(postTipo);
                                objReporteCaja.CodigoTipoReporte = dr.GetInt32(postCodigoTipoReporte); 
                                objReporteCaja.NombreControlador = dr.GetString(postNombreControlador);
                                objReporteCaja.NombreAccion = dr.GetString(postNombreAccion);
                                objReporteCaja.Pdf = (byte)dr.GetInt32(postPdf);
                                objReporteCaja.Excel = (byte)dr.GetInt32(postExcel);
                                objReporteCaja.Web = (byte)dr.GetInt32(postWeb);
                                objReporteCaja.NombreCajaChica = dr.GetString(postNombreCajaChica);

                                lista.Add(objReporteCaja);
                            }
                        }
                    }
                    conexion.Close();
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    lista = null;
                }

                return lista;
            }
        }

        public List<ReporteCajaCLS> GetReportes(int codigoTipoReporte)
        {
            List<ReporteCajaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    SELECT x.codigo_reporte, 
                           @CodigoTipoReporte AS codigo_tipo_reporte, 
                           x.anio AS anio_operacion, 
                           x.numero_semana AS semana_operacion,
                           db_admon.GetPeriodoSemana(x.anio,x.numero_semana) AS semana, 
                           x.observaciones, 
                           x.codigo_estado,
                           y.nombre AS estado,
                           x.usuario_ing, 
                           x.fecha_ing,
                           FORMAT(x.fecha_ing, 'dd/MM/yyyy, hh:mm:ss') AS fecha_ing_str,
                           x.arqueo,
                           CASE
                             WHEN x.arqueo = 1 THEN 'ARQUEO'
                             ELSE ''
                           END AS tipo, 
                           z.nombre_controlador,
                           z.nombre_accion,
                           CASE
                              WHEN @CodigoTipoReporte = " + Constantes.Reporte.CIERRE.ToString() + @" AND x.arqueo = 1 THEN 0
                              ELSE z.pdf
                           END AS pdf,
                           CASE
                              WHEN @CodigoTipoReporte = " + Constantes.Reporte.CIERRE.ToString() + @" AND x.arqueo = 1 THEN 0
                              ELSE z.excel
                           END AS excel,
                           CASE
                              WHEN @CodigoTipoReporte = " + Constantes.Reporte.CIERRE.ToString() + @" AND x.arqueo = 1 THEN 0
                              ELSE z.web
                           END AS web,
                           z.nombre AS nombre_reporte

                    FROM db_tesoreria.reporte_caja x
                    INNER JOIN db_tesoreria.estado_reporte_caja y
                    ON x.codigo_estado = y.codigo_estado_reporte_caja
                    INNER JOIN  db_admon.tipo_reporte z
                    ON @CodigoTipoReporte = z.codigo_tipo_reporte
                    WHERE x.codigo_estado <> @CodigoEstadoAnulado
                      AND x.codigo_reporte NOT IN (0)  
                    ORDER BY x.codigo_reporte DESC";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoAnulado", Constantes.ReporteCaja.Estado.ANULADO);
                        cmd.Parameters.AddWithValue("@CodigoTipoReporte", codigoTipoReporte);
                        
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
                            int postArqueo = dr.GetOrdinal("arqueo");
                            int postTipo = dr.GetOrdinal("tipo");
                            int postCodigoTipoReporte = dr.GetOrdinal("codigo_tipo_reporte");
                            int postNombreControlador = dr.GetOrdinal("nombre_controlador");
                            int postNombreAccion = dr.GetOrdinal("nombre_accion");
                            int postPdf = dr.GetOrdinal("pdf");
                            int postExcel = dr.GetOrdinal("excel");
                            int postWeb = dr.GetOrdinal("web");
                            int postNombreReporte = dr.GetOrdinal("nombre_reporte");
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
                                objReporteCaja.Arqueo = dr.GetByte(postArqueo);
                                objReporteCaja.Tipo = dr.GetString(postTipo);
                                objReporteCaja.CodigoTipoReporte = dr.GetInt32(postCodigoTipoReporte);
                                objReporteCaja.NombreControlador = dr.GetString(postNombreControlador);
                                objReporteCaja.NombreAccion = dr.GetString(postNombreAccion);
                                objReporteCaja.Pdf = (byte)dr.GetInt32(postPdf);
                                objReporteCaja.Excel = (byte)dr.GetInt32(postExcel);
                                objReporteCaja.Web = (byte)dr.GetInt32(postWeb);
                                objReporteCaja.NombreReporte = dr.GetString(postNombreReporte);

                                lista.Add(objReporteCaja);
                            }
                        }
                    }
                    conexion.Close();
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    lista = null;
                }

                return lista;
            }
        }

        public ReporteOperacionesCajaListCLS GetReporteResumenOperacionCaja(int anioOperacion, int semanaOperacion, int codigoReporte, int arqueo)
        {
            int postIdTipoOperacion = 0;
            int postTipoOperacion = 0;
            int postMontoLunes = 0;
            int postMontoMartes = 0;
            int postMontoMiercoles = 0;
            int postMontoJueves = 0;
            int postMontoViernes = 0;
            int postMontoSabado = 0;
            int postMontoDomingo = 0;
            int postMontoSemana = 0;

            ReporteOperacionesCajaListCLS objReporte = new ReporteOperacionesCajaListCLS();
            using (SqlConnection conexion = new SqlConnection(cadenaContabilidad))
            {
                try
                {
                    string complemento = string.Empty;
                    if (arqueo == 1) {
                        complemento = "arqueo";
                    }
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_contabilidad.uspGetReporteResumenOperacionesCaja" + complemento, conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@AnioReporte", anioOperacion);
                        cmd.Parameters.AddWithValue("@NumeroSemanaReporte", semanaOperacion);
                        cmd.Parameters.AddWithValue("@CodigoReporte", codigoReporte);
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr != null)
                        {// Encabezado de la fecha de inicio y fin del reporte
                            int postFechaInicioStr = dr.GetOrdinal("fecha_operacion_minima");
                            int postFechaFinStr = dr.GetOrdinal("fecha_operacion_maxima");
                            List<ProgramacionSemanalCLS> listaEncabezado = new List<ProgramacionSemanalCLS>();
                            ProgramacionSemanalCLS objEncabezado;
                            while (dr.Read())
                            {
                                objEncabezado = new ProgramacionSemanalCLS();
                                objEncabezado.FechaInicioSemana = dr.GetString(postFechaInicioStr);
                                objEncabezado.FechaFinSemana = dr.GetString(postFechaFinStr);
                                listaEncabezado.Add(objEncabezado);
                            }
                            objReporte.listaEncabezado = listaEncabezado;
                        }

                        if (dr.NextResult())
                        {// Encabezado de las fechas de la semana indicada
                            int postFechaStr = dr.GetOrdinal("fecha_str");
                            List<ProgramacionSemanalCLS> listaEncabezadoFechas = new List<ProgramacionSemanalCLS>();
                            ProgramacionSemanalCLS objEncabezadoFechas;
                            while (dr.Read())
                            {
                                objEncabezadoFechas = new ProgramacionSemanalCLS();
                                objEncabezadoFechas.FechaStr = dr.GetString(postFechaStr);
                                listaEncabezadoFechas.Add(objEncabezadoFechas);
                            }
                            objReporte.listaEncabezadoFechas = listaEncabezadoFechas;
                        }

                        if (dr.NextResult())
                        {// Operacion: Ingresos
                            postIdTipoOperacion = dr.GetOrdinal("id");
                            postTipoOperacion = dr.GetOrdinal("tipo_operacion");
                            postMontoLunes = dr.GetOrdinal("monto_lunes");
                            postMontoMartes = dr.GetOrdinal("monto_martes");
                            postMontoMiercoles = dr.GetOrdinal("monto_miercoles");
                            postMontoJueves = dr.GetOrdinal("monto_jueves");
                            postMontoViernes = dr.GetOrdinal("monto_viernes");
                            postMontoSabado = dr.GetOrdinal("monto_sabado");
                            postMontoDomingo = dr.GetOrdinal("monto_domingo");
                            postMontoSemana = dr.GetOrdinal("monto_semana");

                            List<ReporteResumenOperacionesCajaCLS> listaIngresos = new List<ReporteResumenOperacionesCajaCLS>();
                            ReporteResumenOperacionesCajaCLS objReporteDetalle;
                            while (dr.Read())
                            {
                                objReporteDetalle = new ReporteResumenOperacionesCajaCLS();
                                objReporteDetalle.IdTipoOperacion = dr.GetString(postIdTipoOperacion);
                                objReporteDetalle.TipoOperacion = dr.GetString(postTipoOperacion);
                                objReporteDetalle.MontoLunes = dr.GetDecimal(postMontoLunes);
                                objReporteDetalle.MontoMartes = dr.GetDecimal(postMontoMartes);
                                objReporteDetalle.MontoMiercoles = dr.GetDecimal(postMontoMiercoles);
                                objReporteDetalle.MontoJueves = dr.GetDecimal(postMontoJueves);
                                objReporteDetalle.MontoViernes = dr.GetDecimal(postMontoViernes);
                                objReporteDetalle.MontoSabado = dr.GetDecimal(postMontoSabado);
                                objReporteDetalle.MontoDomingo = dr.GetDecimal(postMontoDomingo);
                                objReporteDetalle.MontoSemana = dr.GetDecimal(postMontoSemana);
                                listaIngresos.Add(objReporteDetalle);
                            }// fin while
                            objReporte.listaIngresos = listaIngresos;
                        }// fin if

                        if (dr.NextResult())
                        {// Operacion: Egresos
                            postIdTipoOperacion = dr.GetOrdinal("id");
                            postTipoOperacion = dr.GetOrdinal("tipo_operacion");
                            postMontoLunes = dr.GetOrdinal("monto_lunes");
                            postMontoMartes = dr.GetOrdinal("monto_martes");
                            postMontoMiercoles = dr.GetOrdinal("monto_miercoles");
                            postMontoJueves = dr.GetOrdinal("monto_jueves");
                            postMontoViernes = dr.GetOrdinal("monto_viernes");
                            postMontoSabado = dr.GetOrdinal("monto_sabado");
                            postMontoDomingo = dr.GetOrdinal("monto_domingo");
                            postMontoSemana = dr.GetOrdinal("monto_semana");

                            List<ReporteResumenOperacionesCajaCLS> listaEgresos = new List<ReporteResumenOperacionesCajaCLS>();
                            ReporteResumenOperacionesCajaCLS objReporteDetalle;
                            while (dr.Read())
                            {
                                objReporteDetalle = new ReporteResumenOperacionesCajaCLS();
                                objReporteDetalle.IdTipoOperacion = dr.GetString(postIdTipoOperacion);
                                objReporteDetalle.TipoOperacion = dr.GetString(postTipoOperacion);
                                objReporteDetalle.MontoLunes = dr.GetDecimal(postMontoLunes);
                                objReporteDetalle.MontoMartes = dr.GetDecimal(postMontoMartes);
                                objReporteDetalle.MontoMiercoles = dr.GetDecimal(postMontoMiercoles);
                                objReporteDetalle.MontoJueves = dr.GetDecimal(postMontoJueves);
                                objReporteDetalle.MontoViernes = dr.GetDecimal(postMontoViernes);
                                objReporteDetalle.MontoSabado = dr.GetDecimal(postMontoSabado);
                                objReporteDetalle.MontoDomingo = dr.GetDecimal(postMontoDomingo);
                                objReporteDetalle.MontoSemana = dr.GetDecimal(postMontoSemana);
                                listaEgresos.Add(objReporteDetalle);
                            }// fin while
                            objReporte.listaEgresos = listaEgresos;
                        }// fin if

                    }
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                    objReporte = null;
                }

                return objReporte;
            }
        }

        public ReporteOperacionesCajaListCLS GetReporteOperacionCaja(int anioOperacion, int semanaOperacion, int codigoReporte, int arqueo)
        {
            int postCodigoTipoOperacion = 0;
            int postTipoOperacion = 0;
            int postIdTipoOperacion = 0;
            int postSigno = 0;
            int postCodigoCategoriaOperacion = 0;
            int postCategoriaOperacion = 0;
            int postCodigoOperacion = 0;
            int postOperacion = 0;
            int postCodigoCategoriaEntidad = 0;
            int postCategoriaEntidad = 0;
            int postCodigoEntidad = 0;
            int postNombreEntidad = 0;
            int postNombreEntidadCompleto = 0;
            int postDescripcion = 0;
            int postMontoLunes = 0;
            int postMontoMartes = 0;
            int postMontoMiercoles = 0;
            int postMontoJueves = 0;
            int postMontoViernes = 0;
            int postMontoSabado = 0;
            int postMontoDomingo = 0;
            int postMontoSemana = 0;
            int postMontoTotalLunes = 0;
            int postMontoTotalMartes = 0;
            int postMontoTotalMiercoles = 0;
            int postMontoTotalJueves = 0;
            int postMontoTotalViernes = 0;
            int postMontoTotalSabado = 0;
            int postMontoTotalDomingo = 0;
            int postMontoTotalSemana = 0;

            ReporteOperacionesCajaListCLS objReporte = new ReporteOperacionesCajaListCLS();
            using (SqlConnection conexion = new SqlConnection(cadenaContabilidad))
            {
                try
                {
                    string complemento = string.Empty;
                    if (arqueo == 1) {
                        complemento = "Arqueo";
                    }
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_contabilidad.uspGetReporteOperaciones" + complemento, conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@AnioReporte", anioOperacion);
                        cmd.Parameters.AddWithValue("@NumeroSemanaReporte", semanaOperacion);
                        cmd.Parameters.AddWithValue("@CodigoReporte", codigoReporte);
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr != null)
                        {// Encabezado de la fecha de inicio y fin del reporte
                            int postFechaInicioStr = dr.GetOrdinal("fecha_operacion_minima");
                            int postFechaFinStr = dr.GetOrdinal("fecha_operacion_maxima");
                            List<ProgramacionSemanalCLS> listaEncabezado = new List<ProgramacionSemanalCLS>();
                            ProgramacionSemanalCLS objEncabezado;
                            while (dr.Read())
                            {
                                objEncabezado = new ProgramacionSemanalCLS();
                                objEncabezado.FechaInicioSemana = dr.GetString(postFechaInicioStr);
                                objEncabezado.FechaFinSemana = dr.GetString(postFechaFinStr);
                                listaEncabezado.Add(objEncabezado);
                            }
                            objReporte.listaEncabezado = listaEncabezado;
                        }

                        if (dr.NextResult())
                        {// Encabezado de las fechas de la semana indicada
                            int postFechaStr = dr.GetOrdinal("fecha_str");
                            List<ProgramacionSemanalCLS> listaEncabezadoFechas = new List<ProgramacionSemanalCLS>();
                            ProgramacionSemanalCLS objEncabezadoFechas;
                            while (dr.Read())
                            {
                                objEncabezadoFechas = new ProgramacionSemanalCLS();
                                objEncabezadoFechas.FechaStr = dr.GetString(postFechaStr);
                                listaEncabezadoFechas.Add(objEncabezadoFechas);
                            }
                            objReporte.listaEncabezadoFechas = listaEncabezadoFechas;
                        }

                        if (dr.NextResult())
                        {// Operacion: Todas las operaciones
                            postCodigoTipoOperacion = dr.GetOrdinal("codigo_tipo_operacion");
                            postTipoOperacion = dr.GetOrdinal("tipo_operacion");
                            postIdTipoOperacion = dr.GetOrdinal("id_tipo_operacion");
                            postSigno = dr.GetOrdinal("signo");
                            postCodigoCategoriaOperacion = dr.GetOrdinal("codigo_categoria_operacion");
                            postCategoriaOperacion = dr.GetOrdinal("categoria_operacion");
                            postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            postOperacion = dr.GetOrdinal("operacion");
                            postCodigoCategoriaEntidad = dr.GetOrdinal("codigo_categoria_entidad");
                            postCategoriaEntidad = dr.GetOrdinal("categoria_entidad");
                            postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            postNombreEntidad = dr.GetOrdinal("nombre_entidad");
                            postNombreEntidadCompleto = dr.GetOrdinal("nombre_entidad_completo");
                            postDescripcion = dr.GetOrdinal("descripcion");
                            postMontoLunes = dr.GetOrdinal("monto_lunes");
                            postMontoMartes = dr.GetOrdinal("monto_martes");
                            postMontoMiercoles = dr.GetOrdinal("monto_miercoles");
                            postMontoJueves = dr.GetOrdinal("monto_jueves");
                            postMontoViernes = dr.GetOrdinal("monto_viernes");
                            postMontoSabado = dr.GetOrdinal("monto_sabado");
                            postMontoDomingo = dr.GetOrdinal("monto_domingo");
                            postMontoSemana = dr.GetOrdinal("monto_semana");
                            List<ReporteOperacionesCajaCLS> listaTransacciones = new List<ReporteOperacionesCajaCLS>();
                            ReporteOperacionesCajaCLS objReporteDetalle;
                            while (dr.Read())
                            {
                                objReporteDetalle = new ReporteOperacionesCajaCLS();
                                objReporteDetalle.CodigoTipoOperacion = dr.GetInt16(postCodigoTipoOperacion);
                                objReporteDetalle.TipoOperacion = dr.GetString(postTipoOperacion);
                                objReporteDetalle.IdTipoOperacion = dr.GetString(postIdTipoOperacion);
                                objReporteDetalle.Signo = dr.GetInt16(postSigno);
                                objReporteDetalle.CodigoCategoriaOperacion = dr.GetInt16(postCodigoCategoriaOperacion);
                                objReporteDetalle.CategoriaOperacion = dr.GetString(postCategoriaOperacion);
                                objReporteDetalle.CodigoOperacion = (short)dr.GetInt32(postCodigoOperacion);
                                objReporteDetalle.Operacion = dr.GetString(postOperacion);
                                objReporteDetalle.CodigoCategoriaEntidad = (short)dr.GetInt32(postCodigoCategoriaEntidad);
                                objReporteDetalle.CategoriaEntidad = dr.GetString(postCategoriaEntidad);
                                objReporteDetalle.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objReporteDetalle.NombreEntidad = dr.GetString(postNombreEntidad);
                                objReporteDetalle.NombreEntidadCompleto = dr.GetString(postNombreEntidadCompleto);
                                objReporteDetalle.Descripcion = dr.IsDBNull(postDescripcion) ? "" : dr.GetString(postDescripcion);
                                objReporteDetalle.MontoLunes = dr.GetDecimal(postMontoLunes);
                                objReporteDetalle.MontoMartes = dr.GetDecimal(postMontoMartes);
                                objReporteDetalle.MontoMiercoles = dr.GetDecimal(postMontoMiercoles);
                                objReporteDetalle.MontoJueves = dr.GetDecimal(postMontoJueves);
                                objReporteDetalle.MontoViernes = dr.GetDecimal(postMontoViernes);
                                objReporteDetalle.MontoSabado = dr.GetDecimal(postMontoSabado);
                                objReporteDetalle.MontoDomingo = dr.GetDecimal(postMontoDomingo);
                                objReporteDetalle.MontoSemana = dr.GetDecimal(postMontoSemana);
                                listaTransacciones.Add(objReporteDetalle);
                            }// fin while
                            objReporte.listaTransaccciones = listaTransacciones;
                        }// fin if


                        if (dr.NextResult())
                        {// Operacion: Montos total del los tipos de operacion, del ID del tipo de operacion
                            postIdTipoOperacion = dr.GetOrdinal("id_tipo_operacion");
                            postSigno = dr.GetOrdinal("signo");
                            postMontoTotalLunes = dr.GetOrdinal("monto_total_lunes");
                            postMontoTotalMartes = dr.GetOrdinal("monto_total_martes");
                            postMontoTotalMiercoles = dr.GetOrdinal("monto_total_miercoles");
                            postMontoTotalJueves = dr.GetOrdinal("monto_total_jueves");
                            postMontoTotalViernes = dr.GetOrdinal("monto_total_viernes");
                            postMontoTotalSabado = dr.GetOrdinal("monto_total_sabado");
                            postMontoTotalDomingo = dr.GetOrdinal("monto_total_domingo");
                            postMontoTotalSemana = dr.GetOrdinal("monto_total_semana");

                            List<TipoOperacionCLS> listaMontosTipoOperacion = new List<TipoOperacionCLS>();
                            TipoOperacionCLS objMontoTipoOperacion;
                            while (dr.Read())
                            {
                                objMontoTipoOperacion = new TipoOperacionCLS();
                                objMontoTipoOperacion.IdTipoOperacion = dr.GetString(postIdTipoOperacion);
                                objMontoTipoOperacion.Signo = dr.GetInt16(postSigno);
                                objMontoTipoOperacion.MontoTotalLunes = dr.GetDecimal(postMontoTotalLunes);
                                objMontoTipoOperacion.MontoTotalMartes = dr.GetDecimal(postMontoTotalMartes);
                                objMontoTipoOperacion.MontoTotalMiercoles = dr.GetDecimal(postMontoTotalMiercoles);
                                objMontoTipoOperacion.MontoTotalJueves = dr.GetDecimal(postMontoTotalJueves);
                                objMontoTipoOperacion.MontoTotalViernes = dr.GetDecimal(postMontoTotalViernes);
                                objMontoTipoOperacion.MontoTotalSabado = dr.GetDecimal(postMontoTotalSabado);
                                objMontoTipoOperacion.MontoTotalDomingo = dr.GetDecimal(postMontoTotalDomingo);
                                objMontoTipoOperacion.MontoTotalSemana = dr.GetDecimal(postMontoTotalSemana);

                                listaMontosTipoOperacion.Add(objMontoTipoOperacion);
                            }// fin while
                            objReporte.listaMontosTiposDeOperacion = listaMontosTipoOperacion;
                        }// fin if
                    }
                    conexion.Close();
                }
                catch (Exception e)
                {
                    conexion.Close();
                    objReporte = null;
                }

                return objReporte;
            }
        }

        public ReporteOperacionesCajaListCLS GetReporteDepositosBancarios(int anioOperacion, int semanaOperacion, int codigoReporte, int arqueo)
        {
            int postCodigoTipoOperacion = 0;
            int postTipoOperacion = 0;
            int postIdTipoOperacion = 0;
            int postSigno = 0;
            int postCodigoCategoriaOperacion = 0;
            int postCategoriaOperacion = 0;
            int postCodigoOperacion = 0;
            int postOperacion = 0;
            int postCodigoOrigen = 0;
            int postOrigen = 0;
            int postCodigoCategoriaEntidad = 0;
            int postCategoriaEntidad = 0;
            int postCodigoEntidad = 0;
            int postNombreEntidad = 0;
            int postNombreEntidadCompleto = 0;
            int postCodigoBancoDeposito = 0;
            int postCodigoEmpresa = 0;
            int postNombreEmpresa = 0;
            int postNumeroCuenta = 0;
            int postNumeroBoleta = 0;
            int postMontoLunes = 0;
            int postMontoMartes = 0;
            int postMontoMiercoles = 0;
            int postMontoJueves = 0;
            int postMontoViernes = 0;
            int postMontoSabado = 0;
            int postMontoDomingo = 0;
            int postMontoSemana = 0;
            int postMontoTotalLunes = 0;
            int postMontoTotalMartes = 0;
            int postMontoTotalMiercoles = 0;
            int postMontoTotalJueves = 0;
            int postMontoTotalViernes = 0;
            int postMontoTotalSabado = 0;
            int postMontoTotalDomingo = 0;
            int postMontoTotalSemana = 0;
            int postMontoTotal = 0;

            ReporteOperacionesCajaListCLS objReporte = new ReporteOperacionesCajaListCLS();
            using (SqlConnection conexion = new SqlConnection(cadenaContabilidad))
            {
                try
                {
                    string complemento = string.Empty;
                    if (arqueo == 1)
                    {
                        complemento = "Arqueo";
                    }
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_contabilidad.uspGetReporteDepositosBancarios" + complemento, conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@AnioReporte", anioOperacion);
                        cmd.Parameters.AddWithValue("@NumeroSemanaReporte", semanaOperacion);
                        cmd.Parameters.AddWithValue("@CodigoReporte", codigoReporte);
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr != null)
                        {// Encabezado de la fecha de inicio y fin del reporte
                            int postFechaInicioStr = dr.GetOrdinal("fecha_operacion_minima");
                            int postFechaFinStr = dr.GetOrdinal("fecha_operacion_maxima");
                            List<ProgramacionSemanalCLS> listaEncabezado = new List<ProgramacionSemanalCLS>();
                            ProgramacionSemanalCLS objEncabezado;
                            while (dr.Read())
                            {
                                objEncabezado = new ProgramacionSemanalCLS();
                                objEncabezado.FechaInicioSemana = dr.GetString(postFechaInicioStr);
                                objEncabezado.FechaFinSemana = dr.GetString(postFechaFinStr);
                                listaEncabezado.Add(objEncabezado);
                            }
                            objReporte.listaEncabezado = listaEncabezado;
                        }

                        if (dr.NextResult())
                        {// Encabezado de las fechas de la semana indicada
                            int postFechaStr = dr.GetOrdinal("fecha_str");
                            List<ProgramacionSemanalCLS> listaEncabezadoFechas = new List<ProgramacionSemanalCLS>();
                            ProgramacionSemanalCLS objEncabezadoFechas;
                            while (dr.Read())
                            {
                                objEncabezadoFechas = new ProgramacionSemanalCLS();
                                objEncabezadoFechas.FechaStr = dr.GetString(postFechaStr);
                                listaEncabezadoFechas.Add(objEncabezadoFechas);
                            }
                            objReporte.listaEncabezadoFechas = listaEncabezadoFechas;
                        }

                        if (dr.NextResult())
                        {// Operacion: Todas las operaciones
                            postCodigoTipoOperacion = dr.GetOrdinal("codigo_tipo_operacion");
                            postTipoOperacion = dr.GetOrdinal("tipo_operacion");
                            postIdTipoOperacion = dr.GetOrdinal("id_tipo_operacion");
                            postSigno = dr.GetOrdinal("signo");
                            postCodigoCategoriaOperacion = dr.GetOrdinal("codigo_categoria_operacion");
                            postCategoriaOperacion = dr.GetOrdinal("categoria_operacion");
                            postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            postOperacion = dr.GetOrdinal("operacion");
                            postCodigoOrigen = dr.GetOrdinal("codigo_origen");
                            postOrigen = dr.GetOrdinal("origen");
                            postCodigoCategoriaEntidad = dr.GetOrdinal("codigo_categoria_entidad");
                            postCategoriaEntidad = dr.GetOrdinal("categoria_entidad");
                            postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            postNombreEntidad = dr.GetOrdinal("nombre_entidad");
                            postNombreEntidadCompleto = dr.GetOrdinal("nombre_entidad_completo");
                            postCodigoEmpresa = dr.GetOrdinal("codigo_empresa");
                            postNombreEmpresa = dr.GetOrdinal("nombre_empresa");
                            postNumeroCuenta = dr.GetOrdinal("numero_cuenta");
                            postNumeroBoleta = dr.GetOrdinal("numero_boleta");
                            postMontoLunes = dr.GetOrdinal("monto_lunes");
                            postMontoMartes = dr.GetOrdinal("monto_martes");
                            postMontoMiercoles = dr.GetOrdinal("monto_miercoles");
                            postMontoJueves = dr.GetOrdinal("monto_jueves");
                            postMontoViernes = dr.GetOrdinal("monto_viernes");
                            postMontoSabado = dr.GetOrdinal("monto_sabado");
                            postMontoDomingo = dr.GetOrdinal("monto_domingo");
                            postMontoSemana = dr.GetOrdinal("monto_semana");
                            List<ReporteOperacionesCajaCLS> listaTransacciones = new List<ReporteOperacionesCajaCLS>();
                            ReporteOperacionesCajaCLS objReporteDetalle;
                            while (dr.Read())
                            {
                                objReporteDetalle = new ReporteOperacionesCajaCLS();
                                objReporteDetalle.CodigoTipoOperacion = dr.GetInt16(postCodigoTipoOperacion);
                                objReporteDetalle.TipoOperacion = dr.GetString(postTipoOperacion);
                                objReporteDetalle.IdTipoOperacion = dr.GetString(postIdTipoOperacion);
                                objReporteDetalle.Signo = dr.GetInt16(postSigno);
                                objReporteDetalle.CodigoCategoriaOperacion = dr.GetInt16(postCodigoCategoriaOperacion);
                                objReporteDetalle.CategoriaOperacion = dr.GetString(postCategoriaOperacion);
                                objReporteDetalle.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objReporteDetalle.Operacion = dr.GetString(postOperacion);
                                objReporteDetalle.CodigoOrigen = (byte)dr.GetInt32(postCodigoOrigen);
                                objReporteDetalle.Origen = dr.GetString(postOrigen);
                                objReporteDetalle.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objReporteDetalle.CategoriaEntidad = dr.GetString(postCategoriaEntidad);
                                objReporteDetalle.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objReporteDetalle.NombreEntidad = dr.GetString(postNombreEntidad);
                                objReporteDetalle.NombreEntidadCompleto = dr.GetString(postNombreEntidadCompleto);
                                objReporteDetalle.CodigoEmpresa = dr.GetInt16(postCodigoEmpresa);
                                objReporteDetalle.NombreEmpresa = dr.GetString(postNombreEmpresa);
                                objReporteDetalle.NumeroCuenta = dr.GetString(postNumeroCuenta);
                                objReporteDetalle.NumeroBoleta = dr.GetString(postNumeroBoleta);
                                objReporteDetalle.MontoLunes = dr.GetDecimal(postMontoLunes);
                                objReporteDetalle.MontoMartes = dr.GetDecimal(postMontoMartes);
                                objReporteDetalle.MontoMiercoles = dr.GetDecimal(postMontoMiercoles);
                                objReporteDetalle.MontoJueves = dr.GetDecimal(postMontoJueves);
                                objReporteDetalle.MontoViernes = dr.GetDecimal(postMontoViernes);
                                objReporteDetalle.MontoSabado = dr.GetDecimal(postMontoSabado);
                                objReporteDetalle.MontoDomingo = dr.GetDecimal(postMontoDomingo);
                                objReporteDetalle.MontoSemana = dr.GetDecimal(postMontoSemana);
                                listaTransacciones.Add(objReporteDetalle);
                            }// fin while
                            objReporte.listaTransaccciones = listaTransacciones;
                        }// fin if


                        if (dr.NextResult())
                        {// Operacion: Montos total del los tipos de operacion, del ID del tipo de operacion
                            postCodigoOrigen = dr.GetOrdinal("codigo_origen");
                            postMontoTotalLunes = dr.GetOrdinal("monto_total_lunes");
                            postMontoTotalMartes = dr.GetOrdinal("monto_total_martes");
                            postMontoTotalMiercoles = dr.GetOrdinal("monto_total_miercoles");
                            postMontoTotalJueves = dr.GetOrdinal("monto_total_jueves");
                            postMontoTotalViernes = dr.GetOrdinal("monto_total_viernes");
                            postMontoTotalSabado = dr.GetOrdinal("monto_total_sabado");
                            postMontoTotalDomingo = dr.GetOrdinal("monto_total_domingo");
                            postMontoTotalSemana = dr.GetOrdinal("monto_total_semana");
                            postCodigoBancoDeposito = dr.GetOrdinal("codigo_banco_deposito");
                            postCodigoEmpresa = dr.GetOrdinal("codigo_empresa");
                            postNumeroCuenta = dr.GetOrdinal("numero_cuenta");


                            List<TipoOperacionCLS> listaMontosTipoOperacion = new List<TipoOperacionCLS>();
                            TipoOperacionCLS objMontoTipoOperacion;
                            while (dr.Read())
                            {
                                objMontoTipoOperacion = new TipoOperacionCLS();
                                objMontoTipoOperacion.CodigoOrigen = (byte)dr.GetInt32(postCodigoOrigen);
                                objMontoTipoOperacion.MontoTotalLunes = dr.GetDecimal(postMontoTotalLunes);
                                objMontoTipoOperacion.MontoTotalMartes = dr.GetDecimal(postMontoTotalMartes);
                                objMontoTipoOperacion.MontoTotalMiercoles = dr.GetDecimal(postMontoTotalMiercoles);
                                objMontoTipoOperacion.MontoTotalJueves = dr.GetDecimal(postMontoTotalJueves);
                                objMontoTipoOperacion.MontoTotalViernes = dr.GetDecimal(postMontoTotalViernes);
                                objMontoTipoOperacion.MontoTotalSabado = dr.GetDecimal(postMontoTotalSabado);
                                objMontoTipoOperacion.MontoTotalDomingo = dr.GetDecimal(postMontoTotalDomingo);
                                objMontoTipoOperacion.MontoTotalSemana = dr.GetDecimal(postMontoTotalSemana);
                                objMontoTipoOperacion.CodigoEmpresa = dr.GetInt16(postCodigoEmpresa);
                                objMontoTipoOperacion.CodigoBancoDeposito = dr.GetInt16(postCodigoBancoDeposito);
                                objMontoTipoOperacion.NumeroCuenta = dr.GetString(postNumeroCuenta);

                                listaMontosTipoOperacion.Add(objMontoTipoOperacion);
                            }// fin while
                            objReporte.listaMontosTiposDeOperacion = listaMontosTipoOperacion;
                        }// fin if

                        if (dr.NextResult())
                        {// Operacion: Montos total por Origen del Depósito Bancario
                            postCodigoOrigen = dr.GetOrdinal("codigo_origen");
                            postMontoTotal = dr.GetOrdinal("monto_total");
                            
                            List<OrigenDepositoCLS> listaMontosPorOrigen = new List<OrigenDepositoCLS>();
                            OrigenDepositoCLS objOrigenDeposito;
                            while (dr.Read())
                            {
                                objOrigenDeposito = new OrigenDepositoCLS();
                                objOrigenDeposito.CodigoOrigen = (byte)dr.GetInt32(postCodigoOrigen);
                                objOrigenDeposito.MontoTotal = dr.GetDecimal(postMontoTotal);
                                listaMontosPorOrigen.Add(objOrigenDeposito);
                            }// fin while
                            objReporte.listaMontosPorOrigen = listaMontosPorOrigen;
                        }// fin if
                        
                    }
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                    objReporte = null;
                }

                return objReporte;
            }
        }

        public ReporteOperacionesCajaListCLS GetReporteOperacionesDeSocios(int anioOperacion, int semanaOperacion, int codigoReporte, int arqueo)
        {
            int postCodigoTipoOperacion = 0;
            int postTipoOperacion = 0;
            int postIdTipoOperacion = 0;
            int postSigno = 0;
            int postCodigoCategoriaOperacion = 0;
            int postCategoriaOperacion = 0;
            int postCodigoOperacion = 0;
            int postOperacion = 0;
            int postCodigoCategoriaEntidad = 0;
            int postCategoriaEntidad = 0;
            int postCodigoEntidad = 0;
            int postNombreEntidad = 0;
            int postNombreEntidadCompleto = 0;
            int postDescripcion = 0;
            int postMontoSemana = 0;
            int postMontoTotalEntidad = 0;
            int postFechaOperacionStr = 0;
            int postDescripcionLibre = 0;

            ReporteOperacionesCajaListCLS objReporte = new ReporteOperacionesCajaListCLS();
            using (SqlConnection conexion = new SqlConnection(cadenaContabilidad))
            {
                try
                {
                    string complemento = string.Empty;
                    if (arqueo == 1)
                    {
                        complemento = "Arqueo";
                    }
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_contabilidad.uspGetReporteOperacionesSocio" + complemento, conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@AnioReporte", anioOperacion);
                        cmd.Parameters.AddWithValue("@NumeroSemanaReporte", semanaOperacion);
                        cmd.Parameters.AddWithValue("@CodigoReporte", codigoReporte);
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr != null)
                        {// Encabezado de la fecha de inicio y fin del reporte
                            int postFechaInicioStr = dr.GetOrdinal("fecha_operacion_minima");
                            int postFechaFinStr = dr.GetOrdinal("fecha_operacion_maxima");
                            List<ProgramacionSemanalCLS> listaEncabezado = new List<ProgramacionSemanalCLS>();
                            ProgramacionSemanalCLS objEncabezado;
                            while (dr.Read())
                            {
                                objEncabezado = new ProgramacionSemanalCLS();
                                objEncabezado.FechaInicioSemana = dr.GetString(postFechaInicioStr);
                                objEncabezado.FechaFinSemana = dr.GetString(postFechaFinStr);
                                listaEncabezado.Add(objEncabezado);
                            }
                            objReporte.listaEncabezado = listaEncabezado;
                        }

                        if (dr.NextResult())
                        {// Encabezado de las fechas de la semana indicada
                            int postFechaStr = dr.GetOrdinal("fecha_str");
                            List<ProgramacionSemanalCLS> listaEncabezadoFechas = new List<ProgramacionSemanalCLS>();
                            ProgramacionSemanalCLS objEncabezadoFechas;
                            while (dr.Read())
                            {
                                objEncabezadoFechas = new ProgramacionSemanalCLS();
                                objEncabezadoFechas.FechaStr = dr.GetString(postFechaStr);
                                listaEncabezadoFechas.Add(objEncabezadoFechas);
                            }
                            objReporte.listaEncabezadoFechas = listaEncabezadoFechas;
                        }

                        if (dr.NextResult())
                        {// Operacion: Todas las operaciones
                            postCodigoTipoOperacion = dr.GetOrdinal("codigo_tipo_operacion");
                            postTipoOperacion = dr.GetOrdinal("tipo_operacion");
                            postIdTipoOperacion = dr.GetOrdinal("id_tipo_operacion");
                            postSigno = dr.GetOrdinal("signo");
                            postCodigoCategoriaOperacion = dr.GetOrdinal("codigo_categoria_operacion");
                            postCategoriaOperacion = dr.GetOrdinal("categoria_operacion");
                            postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            postOperacion = dr.GetOrdinal("operacion");
                            postCodigoCategoriaEntidad = dr.GetOrdinal("codigo_categoria_entidad");
                            postCategoriaEntidad = dr.GetOrdinal("categoria_entidad");
                            postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            postNombreEntidad = dr.GetOrdinal("nombre_entidad");
                            postNombreEntidadCompleto = dr.GetOrdinal("nombre_entidad_completo");
                            postDescripcion = dr.GetOrdinal("descripcion");
                            postMontoSemana = dr.GetOrdinal("monto_semana");
                            postFechaOperacionStr = dr.GetOrdinal("fecha_operacion_str");
                            postDescripcionLibre = dr.GetOrdinal("descripcion_libre");
                            List<ReporteOperacionesCajaCLS> listaTransacciones = new List<ReporteOperacionesCajaCLS>();
                            ReporteOperacionesCajaCLS objReporteDetalle;
                            while (dr.Read())
                            {
                                objReporteDetalle = new ReporteOperacionesCajaCLS();
                                objReporteDetalle.CodigoTipoOperacion = dr.GetInt16(postCodigoTipoOperacion);
                                objReporteDetalle.TipoOperacion = dr.GetString(postTipoOperacion);
                                objReporteDetalle.IdTipoOperacion = dr.GetString(postIdTipoOperacion);
                                objReporteDetalle.Signo = dr.GetInt16(postSigno);
                                objReporteDetalle.CodigoCategoriaOperacion = dr.GetInt16(postCodigoCategoriaOperacion);
                                objReporteDetalle.CategoriaOperacion = dr.GetString(postCategoriaOperacion);
                                objReporteDetalle.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objReporteDetalle.Operacion = dr.GetString(postOperacion);
                                objReporteDetalle.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objReporteDetalle.CategoriaEntidad = dr.GetString(postCategoriaEntidad);
                                objReporteDetalle.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objReporteDetalle.NombreEntidad = dr.GetString(postNombreEntidad);
                                objReporteDetalle.NombreEntidadCompleto = dr.GetString(postNombreEntidadCompleto);
                                objReporteDetalle.Descripcion = dr.IsDBNull(postDescripcion) ? "" : dr.GetString(postDescripcion);
                                objReporteDetalle.MontoSemana = dr.GetDecimal(postMontoSemana);
                                objReporteDetalle.FechaOperacionStr = dr.GetString(postFechaOperacionStr);
                                objReporteDetalle.DescripcionLibre = dr.GetString(postDescripcionLibre);
                                listaTransacciones.Add(objReporteDetalle);
                            }// fin while
                            objReporte.listaTransaccciones = listaTransacciones;
                        }// fin if


                        if (dr.NextResult())
                        {// Operacion: Montos total del los tipos de operacion, del ID del tipo de operacion
                            postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            postMontoTotalEntidad = dr.GetOrdinal("monto_total_entidad");
                            List<ReporteOperacionesCajaCLS> listaMontosEntidad = new List<ReporteOperacionesCajaCLS>();
                            ReporteOperacionesCajaCLS objMontoEntidad;
                            while (dr.Read())
                            {
                                objMontoEntidad = new ReporteOperacionesCajaCLS();
                                objMontoEntidad.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objMontoEntidad.MontoTotalEntidad = dr.GetDecimal(postMontoTotalEntidad);

                                listaMontosEntidad.Add(objMontoEntidad);
                            }// fin while
                            objReporte.listaMontosEntidad = listaMontosEntidad;
                        }// fin if
                    }
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                    objReporte = null;
                }

                return objReporte;
            }
        }

        public ReporteOperacionesCajaListCLS GetReporteReservasYCajas(int anioOperacion, int semanaOperacion, int codigoReporte, int arqueo)
        {
            int postCodigoTipoOperacion = 0;
            int postTipoOperacion = 0;
            int postIdTipoOperacion = 0;
            int postSigno = 0;
            int postCodigoCategoriaOperacion = 0;
            int postCategoriaOperacion = 0;
            int postCodigoOperacion = 0;
            int postOperacion = 0;
            int postCodigoCategoriaEntidad = 0;
            int postCategoriaEntidad = 0;
            int postCodigoEntidad = 0;
            int postNombreEntidad = 0;
            int postNombreEntidadCompleto = 0;
            int postDescripcion = 0;
            int postMontoSemana = 0;
            int postMontoTotalCategoriaOperacion = 0;
            int postFechaOperacionStr = 0;
            int postDescripcionLibre = 0;

            ReporteOperacionesCajaListCLS objReporte = new ReporteOperacionesCajaListCLS();
            using (SqlConnection conexion = new SqlConnection(cadenaContabilidad))
            {
                try
                {
                    string complemento = string.Empty;
                    if (arqueo == 1)
                    {
                        complemento = "Arqueo";
                    }
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_contabilidad.uspGetReporteReservasYCajas" + complemento, conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@AnioReporte", anioOperacion);
                        cmd.Parameters.AddWithValue("@NumeroSemanaReporte", semanaOperacion);
                        cmd.Parameters.AddWithValue("@CodigoReporte", codigoReporte);
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr != null)
                        {// Encabezado de la fecha de inicio y fin del reporte
                            int postFechaInicioStr = dr.GetOrdinal("fecha_operacion_minima");
                            int postFechaFinStr = dr.GetOrdinal("fecha_operacion_maxima");
                            List<ProgramacionSemanalCLS> listaEncabezado = new List<ProgramacionSemanalCLS>();
                            ProgramacionSemanalCLS objEncabezado;
                            while (dr.Read())
                            {
                                objEncabezado = new ProgramacionSemanalCLS();
                                objEncabezado.FechaInicioSemana = dr.GetString(postFechaInicioStr);
                                objEncabezado.FechaFinSemana = dr.GetString(postFechaFinStr);
                                listaEncabezado.Add(objEncabezado);
                            }
                            objReporte.listaEncabezado = listaEncabezado;
                        }

                        if (dr.NextResult())
                        {// Encabezado de las fechas de la semana indicada
                            int postFechaStr = dr.GetOrdinal("fecha_str");
                            List<ProgramacionSemanalCLS> listaEncabezadoFechas = new List<ProgramacionSemanalCLS>();
                            ProgramacionSemanalCLS objEncabezadoFechas;
                            while (dr.Read())
                            {
                                objEncabezadoFechas = new ProgramacionSemanalCLS();
                                objEncabezadoFechas.FechaStr = dr.GetString(postFechaStr);
                                listaEncabezadoFechas.Add(objEncabezadoFechas);
                            }
                            objReporte.listaEncabezadoFechas = listaEncabezadoFechas;
                        }

                        if (dr.NextResult())
                        {// Operacion: Todas las operaciones
                            postCodigoTipoOperacion = dr.GetOrdinal("codigo_tipo_operacion");
                            postTipoOperacion = dr.GetOrdinal("tipo_operacion");
                            postIdTipoOperacion = dr.GetOrdinal("id_tipo_operacion");
                            postSigno = dr.GetOrdinal("signo");
                            postCodigoCategoriaOperacion = dr.GetOrdinal("codigo_categoria_operacion");
                            postCategoriaOperacion = dr.GetOrdinal("categoria_operacion");
                            postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            postOperacion = dr.GetOrdinal("operacion");
                            postCodigoCategoriaEntidad = dr.GetOrdinal("codigo_categoria_entidad");
                            postCategoriaEntidad = dr.GetOrdinal("categoria_entidad");
                            postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            postNombreEntidad = dr.GetOrdinal("nombre_entidad");
                            postNombreEntidadCompleto = dr.GetOrdinal("nombre_entidad_completo");
                            postDescripcion = dr.GetOrdinal("descripcion");
                            postMontoSemana = dr.GetOrdinal("monto_semana");
                            postFechaOperacionStr = dr.GetOrdinal("fecha_operacion_str");
                            postDescripcionLibre = dr.GetOrdinal("descripcion_libre");
                            List<ReporteOperacionesCajaCLS> listaTransacciones = new List<ReporteOperacionesCajaCLS>();
                            ReporteOperacionesCajaCLS objReporteDetalle;
                            while (dr.Read())
                            {
                                objReporteDetalle = new ReporteOperacionesCajaCLS();
                                objReporteDetalle.CodigoTipoOperacion = dr.GetInt16(postCodigoTipoOperacion);
                                objReporteDetalle.TipoOperacion = dr.GetString(postTipoOperacion);
                                objReporteDetalle.IdTipoOperacion = dr.GetString(postIdTipoOperacion);
                                objReporteDetalle.Signo = dr.GetInt16(postSigno);
                                objReporteDetalle.CodigoCategoriaOperacion = dr.GetInt16(postCodigoCategoriaOperacion);
                                objReporteDetalle.CategoriaOperacion = dr.GetString(postCategoriaOperacion);
                                objReporteDetalle.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objReporteDetalle.Operacion = dr.GetString(postOperacion);
                                objReporteDetalle.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objReporteDetalle.CategoriaEntidad = dr.GetString(postCategoriaEntidad);
                                objReporteDetalle.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objReporteDetalle.NombreEntidad = dr.GetString(postNombreEntidad);
                                objReporteDetalle.NombreEntidadCompleto = dr.GetString(postNombreEntidadCompleto);
                                objReporteDetalle.Descripcion = dr.IsDBNull(postDescripcion) ? "" : dr.GetString(postDescripcion);
                                objReporteDetalle.MontoSemana = dr.GetDecimal(postMontoSemana);
                                objReporteDetalle.FechaOperacionStr = dr.GetString(postFechaOperacionStr);
                                objReporteDetalle.DescripcionLibre = dr.GetString(postDescripcionLibre);
                                listaTransacciones.Add(objReporteDetalle);
                            }// fin while
                            objReporte.listaTransaccciones = listaTransacciones;
                        }// fin if


                        if (dr.NextResult())
                        {// Operacion: Montos total del los tipos de operacion, del ID del tipo de operacion
                            postIdTipoOperacion = dr.GetOrdinal("id_tipo_operacion");
                            postMontoTotalCategoriaOperacion = dr.GetOrdinal("monto_categoria_operacion");
                            List<ReporteOperacionesCajaCLS> listaMontosEntidad = new List<ReporteOperacionesCajaCLS>();
                            ReporteOperacionesCajaCLS objMontoEntidad;
                            while (dr.Read())
                            {
                                objMontoEntidad = new ReporteOperacionesCajaCLS();
                                objMontoEntidad.IdTipoOperacion = dr.GetString(postIdTipoOperacion);
                                objMontoEntidad.MontoTotalEntidad = dr.GetDecimal(postMontoTotalCategoriaOperacion);

                                listaMontosEntidad.Add(objMontoEntidad);
                            }// fin while
                            objReporte.listaMontosEntidad = listaMontosEntidad;
                        }// fin if
                    }
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                    objReporte = null;
                }

                return objReporte;
            }
        }

        public ReporteOperacionesCajaListCLS GetReporteCierre(int anioOperacion, int semanaOperacion, int codigoReporte)
        {
            ReporteOperacionesCajaListCLS objReporte = new ReporteOperacionesCajaListCLS();
            using (SqlConnection conexion = new SqlConnection(cadenaContabilidad))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_contabilidad.uspGetReporteCierre", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@AnioReporte", anioOperacion);
                        cmd.Parameters.AddWithValue("@NumeroSemanaReporte", semanaOperacion);
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr != null)
                        {// Encabezado de la fecha de inicio y fin del reporte
                            int postFechaInicioStr = dr.GetOrdinal("fecha_operacion_minima");
                            int postFechaFinStr = dr.GetOrdinal("fecha_operacion_maxima");
                            List<ProgramacionSemanalCLS> listaEncabezado = new List<ProgramacionSemanalCLS>();
                            ProgramacionSemanalCLS objEncabezado;
                            while (dr.Read())
                            {
                                objEncabezado = new ProgramacionSemanalCLS();
                                objEncabezado.FechaInicioSemana = dr.GetString(postFechaInicioStr);
                                objEncabezado.FechaFinSemana = dr.GetString(postFechaFinStr);
                                listaEncabezado.Add(objEncabezado);
                            }
                            objReporte.listaEncabezado = listaEncabezado;
                        }

                        if (dr.NextResult())
                        {// Encabezado de las fechas de la semana indicada
                            int postFechaStr = dr.GetOrdinal("fecha_str");
                            List<ProgramacionSemanalCLS> listaEncabezadoFechas = new List<ProgramacionSemanalCLS>();
                            ProgramacionSemanalCLS objEncabezadoFechas;
                            while (dr.Read())
                            {
                                objEncabezadoFechas = new ProgramacionSemanalCLS();
                                objEncabezadoFechas.FechaStr = dr.GetString(postFechaStr);
                                listaEncabezadoFechas.Add(objEncabezadoFechas);
                            }
                            objReporte.listaEncabezadoFechas = listaEncabezadoFechas;
                        }

                        ReporteCierreCLS objCierre = new ReporteCierreCLS();
                        if (dr.NextResult())
                        {// Operacion: Todas las operaciones
                            int postMontoReserva = dr.GetOrdinal("monto_reserva");
                            while (dr.Read())
                            {
                                objCierre.MontoReserva = dr.GetDecimal(postMontoReserva);
                            }// fin while
                        }// fin if

                        if (dr.NextResult())
                        {// Operacion: Todas las operaciones
                            int postMontoLibre = dr.GetOrdinal("monto_libre");
                            while (dr.Read())
                            {
                                objCierre.MontoLibre = dr.GetDecimal(postMontoLibre);
                            }// fin while
                        }// fin if

                        if (dr.NextResult())
                        {// Operacion: Todas las operaciones
                            int postMontoSaldoAnterior = dr.GetOrdinal("monto_saldo_anterior");
                            while (dr.Read())
                            {
                                objCierre.MontoSaldoAnterior = dr.GetDecimal(postMontoSaldoAnterior);
                            }// fin while
                        }// fin if

                        if (dr.NextResult())
                        {// Operacion: Todas las operaciones
                            int postMontoFacturado = dr.GetOrdinal("monto_facturado");
                            while (dr.Read())
                            {
                                objCierre.MontoFacturado = dr.GetDecimal(postMontoFacturado);
                            }// fin while
                        }// fin if

                        if (dr.NextResult())
                        {// Operacion: Todas las operaciones
                            int postMontoCompras = dr.GetOrdinal("monto_compras");
                            while (dr.Read())
                            {
                                objCierre.MontoCompras = dr.GetDecimal(postMontoCompras);
                            }// fin while
                        }// fin if

                        if (dr.NextResult())
                        {// Operacion: Todas las operaciones
                            int postMontoDepositos = dr.GetOrdinal("monto_depositos");
                            while (dr.Read())
                            {
                                objCierre.MontoDepositos = dr.GetDecimal(postMontoDepositos);
                            }// fin while
                        }// fin if

                        objReporte.objCierre = objCierre;
                    }
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                    objReporte = null;
                }

                return objReporte;
            }
        }


    }
}

