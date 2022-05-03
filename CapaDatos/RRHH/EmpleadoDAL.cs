using CapaEntidad.Administracion;
using CapaEntidad.RRHH;
using CapaEntidad.Tesoreria;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.RRHH
{
    public class EmpleadoDAL: CadenaConexion
    {
        public List<EntidadGenericaCLS> ListarEntidadesGenericasByCategoria(int codigoCategoria)
        {
            List<EntidadGenericaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaRRHH))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT codigo_empleado,
                           db_rrhh.GetNombreCompletoEmpleado(cui) AS nombre_completo,
                           2 AS codigo_categoria_entidad 
                    FROM db_rrhh.empleado
                    WHERE codigo_estado <> @CodigoEstado";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.Empleado.EstadoEmpleado.INACTIVO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            EntidadGenericaCLS objEntidad;
                            lista = new List<EntidadGenericaCLS>();
                            int postCodigoEntidad = dr.GetOrdinal("codigo_empleado");
                            int postNombreEntidad = dr.GetOrdinal("nombre_completo");
                            int postCodigoCategoriaEntidad = dr.GetOrdinal("codigo_categoria_entidad");

                            while (dr.Read())
                            {
                                objEntidad = new EntidadGenericaCLS();
                                objEntidad.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objEntidad.NombreEntidad = dr.GetString(postNombreEntidad);
                                objEntidad.CodigoCategoriaEntidad = dr.GetInt32(postCodigoCategoriaEntidad);
                                lista.Add(objEntidad);
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

        public List<EmpleadoCLS> GetListaEmpleados(int codigoEmpresa, int codigoArea, int codigoPuesto, int codigoEstado, int btb, int saldoPrestamo)
        {
            List<EmpleadoCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaRRHH))
            {
                try
                {
                    string filterEmpresa = string.Empty;
                    string filterArea = string.Empty;
                    string filterPuesto = string.Empty;
                    string filterEstadoEmpleado = string.Empty;
                    string filterBackToBack = string.Empty;
                    string filterSaldoPrestamo = string.Empty;
                    if (codigoEmpresa != -1) {
                        filterEmpresa = " AND x.codigo_empresa = " + codigoEmpresa.ToString();
                    }
                    if (codigoArea != -1) {
                        filterArea = " AND x.codigo_area = " + codigoArea.ToString(); ;
                    }
                    if (codigoPuesto != -1) {
                        filterPuesto = " AND x.codigo_puesto = " + codigoPuesto.ToString();
                    }
                    if (codigoEstado != -1) {
                        filterEstadoEmpleado = "AND x.codigo_estado = " + codigoEstado.ToString();
                    }
                    if (btb != 0)
                    {
                        filterBackToBack = "AND x.back_to_back = " + btb.ToString();
                    }

                    if (saldoPrestamo != 0) {
                        filterSaldoPrestamo = "AND x.saldo_prestamo =" + saldoPrestamo.ToString();
                    }
                    string sql = @"
                    SELECT x.codigo_empresa,
	                       s.nombre_comercial AS empresa,			
	                       x.codigo_empleado,
                           db_rrhh.GetNombreCompletoEmpleado(x.cui) AS nombre_completo,
	                       x.codigo_tipo_identificacion,
	                       x.cui,
	                       x.codigo_area,
	                       y.nombre AS area,
	                       x.codigo_seccion,
	                       z.nombre AS seccion,
	                       x.codigo_puesto,
	                       w.nombre AS puesto,
	                       x.codigo_ubicacion,
	                       m.nombre AS ubicacion,
	                       x.codigo_jornada,
	                       n.nombre AS jornada,
                           x.codigo_frecuencia_pago,
                           p.nombre as frecuencia_pago, 
                           FORMAT(x.fecha_ingreso, 'dd/MM/yyyy') AS fecha_ingreso_str,
                           CASE
                             WHEN x.fecha_egreso IS NOT NULL THEN FORMAT(x.fecha_egreso, 'dd/MM/yyyy')
                             ELSE ''
                           END fecha_egreso_str, 
                           x.codigo_estado,
                           o.nombre as estado_empleado,
                           CASE
                            WHEN x.codigo_estado = @CodigoEstadoRetirado THEN 0
                            ELSE 1
                           END AS  permiso_anular,
                           1 AS permiso_editar
                            
                    FROM db_rrhh.empleado x
                    INNER JOIN db_admon.empresa s
                    ON x.codigo_empresa = s.codigo_empresa
                    INNER JOIN db_rrhh.area y
                    ON x.codigo_area = y.codigo_area
                    INNER JOIN db_rrhh.seccion z
                    ON x.codigo_seccion = z.codigo_seccion
                    INNER JOIN db_rrhh.puesto w
                    ON x.codigo_puesto = w.codigo_puesto
                    INNER JOIN db_rrhh.ubicacion m
                    ON x.codigo_ubicacion = m.codigo_ubicacion
                    INNER JOIN db_rrhh.jornada n
                    ON x.codigo_jornada = n.codigo_jornada
                    INNER JOIN db_rrhh.estado_empleado o
                    ON x.codigo_estado = o.codigo_estado_empleado
                    INNER JOIN db_rrhh.frecuencia_pago p
                    ON x.codigo_frecuencia_pago = p.codigo_frecuencia_pago
                    WHERE x.codigo_estado <> @CodigoEstado
                    " + filterEmpresa + @"    
                    " + filterArea + @"    
                    " + filterPuesto + @"
                    " + filterEstadoEmpleado + @"
                    " + filterBackToBack + @"
                    " + filterSaldoPrestamo;

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.Empleado.EstadoEmpleado.INACTIVO);
                        cmd.Parameters.AddWithValue("@CodigoEstadoRetirado", Constantes.Empleado.EstadoEmpleado.RETIRADO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            EmpleadoCLS objEmpleado;
                            lista = new List<EmpleadoCLS>();
                            int postCodigoEmpresa = dr.GetOrdinal("codigo_empresa");
                            int postEmpresa = dr.GetOrdinal("empresa");
                            int postCodigoEmpleado = dr.GetOrdinal("codigo_empleado");
                            int postNombreCompleto = dr.GetOrdinal("nombre_completo");
                            int postCodigoTipoIdentificacion = dr.GetOrdinal("codigo_tipo_identificacion");
                            int postCui = dr.GetOrdinal("cui");
                            int postCodigoArea = dr.GetOrdinal("codigo_area");
                            int postArea = dr.GetOrdinal("area");
                            int postCodigoSeccion = dr.GetOrdinal("codigo_seccion");
                            int postSeccion = dr.GetOrdinal("seccion");
                            int postCodigoPuesto = dr.GetOrdinal("codigo_puesto");
                            int postPuesto = dr.GetOrdinal("puesto");
                            int postCodigoUbicacion = dr.GetOrdinal("codigo_ubicacion");
                            int postUbicacion = dr.GetOrdinal("ubicacion");
                            int postCodigoJornada = dr.GetOrdinal("codigo_jornada");
                            int postJornada = dr.GetOrdinal("jornada");
                            int postCodigoFrecuenciaPago = dr.GetOrdinal("codigo_frecuencia_pago");
                            int postFrecuenciaPago = dr.GetOrdinal("frecuencia_pago");
                            int postFechaIngresoStr = dr.GetOrdinal("fecha_ingreso_str");
                            int postFechaEgresoStr = dr.GetOrdinal("fecha_egreso_str");
                            int postCodigoEstadoEmpleado = dr.GetOrdinal("codigo_estado");
                            int postEstadoEmpleado = dr.GetOrdinal("estado_empleado");
                            int postPermisoAnular = dr.GetOrdinal("permiso_anular");
                            int postPermisoEditar = dr.GetOrdinal("permiso_editar");

                            while (dr.Read())
                            {
                                objEmpleado = new EmpleadoCLS();
                                objEmpleado.CodigoEmpresa = dr.GetInt16(postCodigoEmpresa);
                                objEmpleado.Empresa = dr.GetString(postEmpresa);
                                objEmpleado.CodigoEmpleado = dr.GetString(postCodigoEmpleado);
                                objEmpleado.NombreCompleto = dr.GetString(postNombreCompleto);
                                objEmpleado.CodigoTipoIdentificacion = dr.GetString(postCodigoTipoIdentificacion);
                                objEmpleado.Cui = dr.GetString(postCui);
                                objEmpleado.CodigoArea = dr.GetInt16(postCodigoArea);
                                objEmpleado.Area = dr.GetString(postArea);
                                objEmpleado.CodigoSeccion = dr.GetInt16(postCodigoSeccion);
                                objEmpleado.Seccion = dr.GetString(postSeccion);
                                objEmpleado.CodigoPuesto = dr.GetInt16(postCodigoPuesto);
                                objEmpleado.Puesto = dr.GetString(postPuesto);
                                objEmpleado.CodigoUbicacion = dr.GetInt16(postCodigoUbicacion);
                                objEmpleado.Ubicacion = dr.GetString(postUbicacion);
                                objEmpleado.CodigoJornada = dr.GetByte(postCodigoJornada);
                                objEmpleado.Jornada = dr.GetString(postJornada);
                                objEmpleado.CodigoFrecuenciaPago = dr.GetByte(postCodigoFrecuenciaPago);
                                objEmpleado.FrecuenciaPago = dr.GetString(postFrecuenciaPago);
                                objEmpleado.FechaIngresoStr = dr.GetString(postFechaIngresoStr);
                                objEmpleado.FechaEgresoStr = dr.GetString(postFechaEgresoStr);
                                objEmpleado.CodigoEstado = dr.GetInt16(postCodigoEstadoEmpleado);
                                objEmpleado.EstadoEmpleado = dr.GetString(postEstadoEmpleado);
                                objEmpleado.PermisoAnular = (byte)dr.GetInt32(postPermisoAnular);
                                objEmpleado.PermisoEditar = (byte)dr.GetInt32(postPermisoEditar);
                                lista.Add(objEmpleado);
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

        public List<EmpleadoCLS> GetListaEmpleadosRetirados(int codigoEmpresa, int codigoArea, int codigoPuesto)
        {
            List<EmpleadoCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaRRHH))
            {
                try
                {
                    string filterEmpresa = string.Empty;
                    string filterArea = string.Empty;
                    string filterPuesto = string.Empty;
                    string filterSaldoPrestamo = string.Empty;
                    if (codigoEmpresa != -1)
                    {
                        filterEmpresa = " AND x.codigo_empresa = " + codigoEmpresa.ToString();
                    }
                    if (codigoArea != -1)
                    {
                        filterArea = " AND x.codigo_area = " + codigoArea.ToString(); ;
                    }
                    if (codigoPuesto != -1)
                    {
                        filterPuesto = " AND x.codigo_puesto = " + codigoPuesto.ToString();
                    }

                    string sql = @"
                    SELECT x.codigo_empresa,
	                       s.nombre_comercial AS empresa,			
	                       x.codigo_empleado,
                           db_rrhh.GetNombreCompletoEmpleado(x.cui) AS nombre_completo,
	                       x.codigo_tipo_identificacion,
	                       x.cui,
	                       x.codigo_area,
	                       y.nombre AS area,
	                       x.codigo_seccion,
	                       z.nombre AS seccion,
	                       x.codigo_puesto,
	                       w.nombre AS puesto,
	                       x.codigo_ubicacion,
	                       m.nombre AS ubicacion,
	                       x.codigo_jornada,
	                       n.nombre AS jornada,
                           x.codigo_frecuencia_pago,
                           p.nombre as frecuencia_pago, 
                           x.fecha_ingreso,
                           FORMAT(x.fecha_ingreso, 'dd/MM/yyyy') AS fecha_ingreso_str,
                           x.fecha_egreso,
                           CASE
                             WHEN x.fecha_egreso IS NOT NULL THEN FORMAT(x.fecha_egreso, 'dd/MM/yyyy')
                             ELSE ''
                           END fecha_egreso_str, 
                           x.codigo_estado,
                           o.nombre as estado_empleado,
                           CASE
                            WHEN x.codigo_estado = @CodigoEstadoRetirado THEN 0
                            ELSE 1
                           END AS  permiso_anular,
                           1 AS permiso_editar,
                           x.saldo_prestamo,
                           CASE
                             WHEN x.saldo_prestamo = 1 THEN 'SI'
                             ELSE 'NO'
                           END AS saldo_prestamo_str, 
                           x.pago_pendiente,
                           CASE
                             WHEN x.pago_pendiente = 1 THEN 'SI'
                             ELSE 'NO'
                           END AS pago_pendiente_str 
                            
                    FROM db_rrhh.empleado x
                    INNER JOIN db_admon.empresa s
                    ON x.codigo_empresa = s.codigo_empresa
                    INNER JOIN db_rrhh.area y
                    ON x.codigo_area = y.codigo_area
                    INNER JOIN db_rrhh.seccion z
                    ON x.codigo_seccion = z.codigo_seccion
                    INNER JOIN db_rrhh.puesto w
                    ON x.codigo_puesto = w.codigo_puesto
                    INNER JOIN db_rrhh.ubicacion m
                    ON x.codigo_ubicacion = m.codigo_ubicacion
                    INNER JOIN db_rrhh.jornada n
                    ON x.codigo_jornada = n.codigo_jornada
                    INNER JOIN db_rrhh.estado_empleado o
                    ON x.codigo_estado = o.codigo_estado_empleado
                    INNER JOIN db_rrhh.frecuencia_pago p
                    ON x.codigo_frecuencia_pago = p.codigo_frecuencia_pago
                    WHERE x.codigo_estado = @CodigoEstadoRetirado
                    " + filterEmpresa + @"    
                    " + filterArea + @"    
                    " + filterPuesto + @"
                    " + filterSaldoPrestamo;

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoRetirado", Constantes.Empleado.EstadoEmpleado.RETIRADO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            EmpleadoCLS objEmpleado;
                            lista = new List<EmpleadoCLS>();
                            int postCodigoEmpresa = dr.GetOrdinal("codigo_empresa");
                            int postEmpresa = dr.GetOrdinal("empresa");
                            int postCodigoEmpleado = dr.GetOrdinal("codigo_empleado");
                            int postNombreCompleto = dr.GetOrdinal("nombre_completo");
                            int postCodigoTipoIdentificacion = dr.GetOrdinal("codigo_tipo_identificacion");
                            int postCui = dr.GetOrdinal("cui");
                            int postCodigoArea = dr.GetOrdinal("codigo_area");
                            int postArea = dr.GetOrdinal("area");
                            int postCodigoSeccion = dr.GetOrdinal("codigo_seccion");
                            int postSeccion = dr.GetOrdinal("seccion");
                            int postCodigoPuesto = dr.GetOrdinal("codigo_puesto");
                            int postPuesto = dr.GetOrdinal("puesto");
                            int postCodigoUbicacion = dr.GetOrdinal("codigo_ubicacion");
                            int postUbicacion = dr.GetOrdinal("ubicacion");
                            int postCodigoJornada = dr.GetOrdinal("codigo_jornada");
                            int postJornada = dr.GetOrdinal("jornada");
                            int postCodigoFrecuenciaPago = dr.GetOrdinal("codigo_frecuencia_pago");
                            int postFrecuenciaPago = dr.GetOrdinal("frecuencia_pago");
                            int postFechaIngreso = dr.GetOrdinal("fecha_ingreso");
                            int postFechaIngresoStr = dr.GetOrdinal("fecha_ingreso_str");
                            int postFechaEgreso = dr.GetOrdinal("fecha_egreso");
                            int postFechaEgresoStr = dr.GetOrdinal("fecha_egreso_str");
                            int postCodigoEstadoEmpleado = dr.GetOrdinal("codigo_estado");
                            int postEstadoEmpleado = dr.GetOrdinal("estado_empleado");
                            int postPermisoAnular = dr.GetOrdinal("permiso_anular");
                            int postPermisoEditar = dr.GetOrdinal("permiso_editar");
                            int postSaldoPrestamo = dr.GetOrdinal("saldo_prestamo");
                            int postSaldoPrestamoStr = dr.GetOrdinal("saldo_prestamo_str");
                            int postPagoPendiente = dr.GetOrdinal("pago_pendiente");
                            int postPagoPendienteStr = dr.GetOrdinal("pago_pendiente_str");

                            while (dr.Read())
                            {
                                objEmpleado = new EmpleadoCLS();
                                objEmpleado.CodigoEmpresa = dr.GetInt16(postCodigoEmpresa);
                                objEmpleado.Empresa = dr.GetString(postEmpresa);
                                objEmpleado.CodigoEmpleado = dr.GetString(postCodigoEmpleado);
                                objEmpleado.NombreCompleto = dr.GetString(postNombreCompleto);
                                objEmpleado.CodigoTipoIdentificacion = dr.GetString(postCodigoTipoIdentificacion);
                                objEmpleado.Cui = dr.GetString(postCui);
                                objEmpleado.CodigoArea = dr.GetInt16(postCodigoArea);
                                objEmpleado.Area = dr.GetString(postArea);
                                objEmpleado.CodigoSeccion = dr.GetInt16(postCodigoSeccion);
                                objEmpleado.Seccion = dr.GetString(postSeccion);
                                objEmpleado.CodigoPuesto = dr.GetInt16(postCodigoPuesto);
                                objEmpleado.Puesto = dr.GetString(postPuesto);
                                objEmpleado.CodigoUbicacion = dr.GetInt16(postCodigoUbicacion);
                                objEmpleado.Ubicacion = dr.GetString(postUbicacion);
                                objEmpleado.CodigoJornada = dr.GetByte(postCodigoJornada);
                                objEmpleado.Jornada = dr.GetString(postJornada);
                                objEmpleado.CodigoFrecuenciaPago = dr.GetByte(postCodigoFrecuenciaPago);
                                objEmpleado.FrecuenciaPago = dr.GetString(postFrecuenciaPago);
                                objEmpleado.FechaIngreso = dr.GetDateTime(postFechaIngreso).Date;
                                objEmpleado.FechaIngresoStr = dr.GetString(postFechaIngresoStr);
                                objEmpleado.FechaEgreso = dr.IsDBNull(postFechaEgreso) ? null :  dr.GetDateTime(postFechaEgreso);
                                objEmpleado.FechaEgresoStr = dr.GetString(postFechaEgresoStr);
                                objEmpleado.CodigoEstado = dr.GetInt16(postCodigoEstadoEmpleado);
                                objEmpleado.EstadoEmpleado = dr.GetString(postEstadoEmpleado);
                                objEmpleado.PermisoAnular = (byte)dr.GetInt32(postPermisoAnular);
                                objEmpleado.PermisoEditar = (byte)dr.GetInt32(postPermisoEditar);
                                objEmpleado.SaldoPrestamo = dr.GetByte(postSaldoPrestamo);
                                objEmpleado.SaldoPrestamoStr = dr.GetString(postSaldoPrestamoStr);
                                objEmpleado.PagoPendiente = dr.GetByte(postPagoPendiente);
                                objEmpleado.PagoPendienteStr = dr.GetString(postPagoPendienteStr);
                                lista.Add(objEmpleado);
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

        public EmpleadoCLS GetDataEmpleado(string codigoEmpleado)
        {
            EmpleadoCLS objEmpleado = null;
            using (SqlConnection conexion = new SqlConnection(cadenaRRHH))
            {
                try
                {
                    string sql = @"
                    SELECT x.codigo_empresa,
	                       s.nombre_comercial AS empresa,
	                       x.codigo_empleado,
                           db_rrhh.GetNombreCompletoEmpleado(x.cui) AS nombre_completo,
	                       x.codigo_tipo_identificacion,
	                       x.cui,
                           t.primer_nombre,
                           t.segundo_nombre,
                           t.tercer_nombre,
                           t.primer_apellido,
                           t.segundo_apellido,
                           t.apellido_casada, 
                           FORMAT(t.fecha_nacimiento, 'dd/MM/yyyy') AS fecha_nacimiento_str,
                           t.codigo_genero, 
                           x.nit,
                           x.correo_electronico, 
                           x.numero_afiliacion, 
                           x.empleado_externo, 
	                       x.codigo_area,
	                       y.nombre AS area,
	                       x.codigo_seccion,
	                       z.nombre AS seccion,
	                       x.codigo_puesto,
	                       w.nombre AS puesto,
                           x.codigo_tipo_cuenta, 
	                       x.codigo_ubicacion,
	                       m.nombre AS ubicacion,
                           x.numero_cuenta, 
                           x.monto_devengado, 
	                       x.codigo_jornada,
	                       n.nombre AS jornada,
                           x.codigo_frecuencia_pago,
                           p.nombre as frecuencia_pago, 
                           x.igss,
                           x.bono_de_ley,
                           x.retencion_isr,
                           x.fecha_ingreso,
                           FORMAT(x.fecha_ingreso, 'dd/MM/yyyy') AS fecha_ingreso_str,
                           x.fecha_egreso,
                           CASE
                            WHEN x.fecha_egreso IS NOT NULL THEN FORMAT(x.fecha_egreso, 'dd/MM/yyyy')
                            ELSE ''
                           END fecha_egreso_str, 
                           x.back_to_back, 
                           x.codigo_tipo_btb, 
                           x.codigo_estado,
                           o.nombre as estado_empleado,
                           1 AS permiso_anular,
                           1 AS permiso_editar,
                           t.foto,
                           salario_diario,
                           bono_decreto_37_2001 
                            
                    FROM db_rrhh.empleado x
                    LEFT JOIN db_rrhh.persona t
                    ON x.cui = t.cui
                    INNER JOIN db_admon.empresa s
                    ON x.codigo_empresa = s.codigo_empresa
                    INNER JOIN db_rrhh.area y
                    ON x.codigo_area = y.codigo_area
                    INNER JOIN db_rrhh.seccion z
                    ON x.codigo_seccion = z.codigo_seccion
                    INNER JOIN db_rrhh.puesto w
                    ON x.codigo_puesto = w.codigo_puesto
                    INNER JOIN db_rrhh.ubicacion m
                    ON x.codigo_ubicacion = m.codigo_ubicacion
                    INNER JOIN db_rrhh.jornada n
                    ON x.codigo_jornada = n.codigo_jornada
                    INNER JOIN db_rrhh.estado_empleado o
                    ON x.codigo_estado = o.codigo_estado_empleado
                    INNER JOIN db_rrhh.frecuencia_pago p
                    ON x.codigo_frecuencia_pago = p.codigo_frecuencia_pago
                    WHERE x.codigo_empleado = @CodigoEmpleado";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEmpleado", codigoEmpleado);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            int postCodigoEmpresa = dr.GetOrdinal("codigo_empresa");
                            int postEmpresa = dr.GetOrdinal("empresa");
                            int postCodigoEmpleado = dr.GetOrdinal("codigo_empleado");
                            int postNombreCompleto = dr.GetOrdinal("nombre_completo");
                            int postCodigoTipoIdentificacion = dr.GetOrdinal("codigo_tipo_identificacion");
                            int postCui = dr.GetOrdinal("cui");
                            int postPrimerNombre = dr.GetOrdinal("primer_nombre");
                            int postSegundoNombre = dr.GetOrdinal("segundo_nombre");
                            int postTercerNombre = dr.GetOrdinal("tercer_nombre");
                            int postPrimerApellido = dr.GetOrdinal("primer_apellido");
                            int postSegundoApellido = dr.GetOrdinal("segundo_apellido");
                            int postApellidoCasada = dr.GetOrdinal("apellido_casada");
                            int postFechaNacimientoStr = dr.GetOrdinal("fecha_nacimiento_str");
                            int postCodigoGenero = dr.GetOrdinal("codigo_genero");
                            int postNit = dr.GetOrdinal("nit");
                            int postCorreoElectronico = dr.GetOrdinal("correo_electronico");
                            int postNumeroAfiliacion = dr.GetOrdinal("numero_afiliacion");
                            int postEmpleadoExterno = dr.GetOrdinal("empleado_externo");
                            int postCodigoArea = dr.GetOrdinal("codigo_area");
                            int postArea = dr.GetOrdinal("area");
                            int postCodigoSeccion = dr.GetOrdinal("codigo_seccion");
                            int postSeccion = dr.GetOrdinal("seccion");
                            int postCodigoPuesto = dr.GetOrdinal("codigo_puesto");
                            int postPuesto = dr.GetOrdinal("puesto");
                            int postCodigoTipoCuenta = dr.GetOrdinal("codigo_tipo_cuenta");
                            int postCodigoUbicacion = dr.GetOrdinal("codigo_ubicacion");
                            int postUbicacion = dr.GetOrdinal("ubicacion");
                            int postNumeroCuenta = dr.GetOrdinal("numero_cuenta");
                            int postMontoDevengado = dr.GetOrdinal("monto_devengado");
                            int postCodigoJornada = dr.GetOrdinal("codigo_jornada");
                            int postJornada = dr.GetOrdinal("jornada");
                            int postCodigoFrecuenciaPago = dr.GetOrdinal("codigo_frecuencia_pago");
                            int postFrecuenciaPago = dr.GetOrdinal("frecuencia_pago");
                            int postIgss = dr.GetOrdinal("igss");
                            int postBonoDeLey = dr.GetOrdinal("bono_de_ley");
                            int postRetencionISR = dr.GetOrdinal("retencion_isr");
                            int postFechaIngreso = dr.GetOrdinal("fecha_ingreso");
                            int postFechaIngresoStr = dr.GetOrdinal("fecha_ingreso_str");
                            int postFechaEgreso = dr.GetOrdinal("fecha_egreso");
                            int postFechaEgresoStr = dr.GetOrdinal("fecha_egreso_str");
                            int postBackToBack = dr.GetOrdinal("back_to_back");
                            int postCodigoTipoBackToBack = dr.GetOrdinal("codigo_tipo_btb");
                            int postCodigoEstadoEmpleado = dr.GetOrdinal("codigo_estado");
                            int postEstadoEmpleado = dr.GetOrdinal("estado_empleado");
                            int postPermisoAnular = dr.GetOrdinal("permiso_anular");
                            int postPermisoEditar = dr.GetOrdinal("permiso_editar");
                            int postSalarioDiario = dr.GetOrdinal("salario_diario");
                            int postBonoDecreto372001 = dr.GetOrdinal("bono_decreto_37_2001");
                            int postFoto = dr.GetOrdinal("foto");

                            while (dr.Read())
                            {
                                objEmpleado = new EmpleadoCLS();
                                objEmpleado.CodigoEmpresa = dr.GetInt16(postCodigoEmpresa);
                                objEmpleado.Empresa = dr.GetString(postEmpresa);
                                objEmpleado.CodigoEmpleado = dr.GetString(postCodigoEmpleado);
                                objEmpleado.NombreCompleto = dr.GetString(postNombreCompleto);
                                objEmpleado.CodigoTipoIdentificacion = dr.GetString(postCodigoTipoIdentificacion);
                                objEmpleado.Cui = dr.GetString(postCui);
                                objEmpleado.PrimerNombre = dr.IsDBNull(postPrimerNombre) ? "" : dr.GetString(postPrimerNombre);
                                objEmpleado.SegundoNombre = dr.IsDBNull(postSegundoNombre) ? "" : dr.GetString(postSegundoNombre);
                                objEmpleado.TercerNombre = dr.IsDBNull(postTercerNombre) ? "" : dr.GetString(postTercerNombre);
                                objEmpleado.PrimerApellido = dr.IsDBNull(postPrimerApellido) ? "" : dr.GetString(postPrimerApellido);
                                objEmpleado.SegundoApellido = dr.IsDBNull(postSegundoApellido) ? "" : dr.GetString(postSegundoApellido);
                                objEmpleado.ApellidoCasada = dr.IsDBNull(postApellidoCasada) ? "" : dr.GetString(postApellidoCasada);
                                objEmpleado.FechaNacimientoStr = dr.GetString(postFechaNacimientoStr);
                                objEmpleado.CodigoGenero = dr.GetString(postCodigoGenero);
                                objEmpleado.Nit = dr.IsDBNull(postNit) ? "" : dr.GetString(postNit);
                                objEmpleado.CorreoElectronico = dr.IsDBNull(postCorreoElectronico) ? "" : dr.GetString(postCorreoElectronico);
                                objEmpleado.NumeroAfiliacion = dr.IsDBNull(postNumeroAfiliacion) ? "": dr.GetString(postNumeroAfiliacion);
                                objEmpleado.EmpleadoExterno = dr.GetByte(postEmpleadoExterno);
                                objEmpleado.CodigoArea = dr.GetInt16(postCodigoArea);
                                objEmpleado.Area = dr.GetString(postArea);
                                objEmpleado.CodigoSeccion = dr.GetInt16(postCodigoSeccion);
                                objEmpleado.Seccion = dr.GetString(postSeccion);
                                objEmpleado.CodigoPuesto = dr.GetInt16(postCodigoPuesto);
                                objEmpleado.Puesto = dr.GetString(postPuesto);
                                objEmpleado.CodigoTipoCuenta = dr.GetInt16(postCodigoTipoCuenta);
                                objEmpleado.CodigoUbicacion = dr.GetInt16(postCodigoUbicacion);
                                objEmpleado.Ubicacion = dr.GetString(postUbicacion);
                                objEmpleado.NumeroCuenta = dr.IsDBNull(postNumeroCuenta) ? "" : dr.GetString(postNumeroCuenta);
                                objEmpleado.MontoDevengado = dr.GetDecimal(postMontoDevengado);
                                objEmpleado.CodigoJornada = dr.GetByte(postCodigoJornada);
                                objEmpleado.Jornada = dr.GetString(postJornada);
                                objEmpleado.CodigoFrecuenciaPago = dr.GetByte(postCodigoFrecuenciaPago);
                                objEmpleado.FrecuenciaPago = dr.GetString(postFrecuenciaPago);
                                objEmpleado.Igss = dr.GetByte(postIgss);
                                objEmpleado.BonoDeLey = dr.GetByte(postBonoDeLey);
                                objEmpleado.RetencionIsr = dr.GetByte(postRetencionISR);
                                objEmpleado.FechaIngreso = dr.GetDateTime(postFechaIngreso);
                                objEmpleado.FechaIngresoStr = dr.GetString(postFechaIngresoStr);
                                objEmpleado.FechaEgreso = dr.IsDBNull(postFechaEgreso) ? null : dr.GetDateTime(postFechaEgreso);
                                objEmpleado.FechaEgresoStr = dr.GetString(postFechaEgresoStr);
                                objEmpleado.BackToBack = dr.GetByte(postBackToBack);
                                objEmpleado.CodigoTipoBackToBack = dr.GetByte(postCodigoTipoBackToBack);
                                objEmpleado.CodigoEstado = dr.GetInt16(postCodigoEstadoEmpleado);
                                objEmpleado.EstadoEmpleado = dr.GetString(postEstadoEmpleado);
                                objEmpleado.PermisoAnular = (byte)dr.GetInt32(postPermisoAnular);
                                objEmpleado.PermisoEditar = (byte)dr.GetInt32(postPermisoEditar);
                                objEmpleado.SalarioDiario = dr.GetDecimal(postSalarioDiario);
                                objEmpleado.BonoDecreto372001 = dr.GetDecimal(postBonoDecreto372001);
                                objEmpleado.Foto = dr.IsDBNull(postFoto) ? "" :  dr.GetString(postFoto);
                            }
                        }
                    }
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                    objEmpleado = null;
                }

                return objEmpleado;
            }
        }

        public EmpleadoComboCLS GetListFillCombosNewEmpleado()
        {
            EmpleadoComboCLS objEmpleadoComboCLS = new EmpleadoComboCLS();
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_rrhh.uspFillComboNewEmpleado", conexion))
                    {
                        // le indico que es un procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        // Devolvera varias tablas
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            List<EmpresaCLS> listaEmpresas = new List<EmpresaCLS>();
                            int postCodigoEmpresa = dr.GetOrdinal("codigo_empresa");
                            int postNombreEmpresa = dr.GetOrdinal("nombre_comercial");

                            EmpresaCLS objEmpresaCLS;
                            while (dr.Read())
                            {
                                objEmpresaCLS = new EmpresaCLS();
                                objEmpresaCLS.CodigoEmpresa = dr.GetInt16(postCodigoEmpresa);
                                objEmpresaCLS.NombreComercial = dr.GetString(postNombreEmpresa);
                                listaEmpresas.Add(objEmpresaCLS);
                            }//fin while
                            objEmpleadoComboCLS.listaEmpresas = listaEmpresas;
                        }// fin if

                        if (dr.NextResult())
                        {
                            List<AreaCLS> listaAreas = new List<AreaCLS>();
                            int postCodigoArea = dr.GetOrdinal("codigo_area");
                            int postNombreArea = dr.GetOrdinal("nombre");

                            AreaCLS objAreaCLS;
                            while (dr.Read())
                            {
                                objAreaCLS = new AreaCLS();
                                objAreaCLS.CodigoArea = dr.GetInt16(postCodigoArea);
                                objAreaCLS.Nombre = dr.GetString(postNombreArea);
                                listaAreas.Add(objAreaCLS);
                            }//fin while
                            objEmpleadoComboCLS.listaAreas = listaAreas;
                        }

                        if (dr.NextResult())
                        {
                            List<SeccionCLS> listaSecciones = new List<SeccionCLS>();
                            int postCodigoSeccion = dr.GetOrdinal("codigo_seccion");
                            int postNombreSeccion = dr.GetOrdinal("nombre");

                            SeccionCLS objSeccionCLS;
                            while (dr.Read())
                            {
                                objSeccionCLS = new SeccionCLS();
                                objSeccionCLS.CodigoSeccion = dr.GetInt16(postCodigoSeccion);
                                objSeccionCLS.Nombre = dr.GetString(postNombreSeccion);
                                listaSecciones.Add(objSeccionCLS);
                            }//fin while
                            objEmpleadoComboCLS.listaSecciones = listaSecciones;
                        }

                        if (dr.NextResult())
                        {
                            List<PuestoCLS> listaPuestos = new List<PuestoCLS>();
                            int postCodigoPuesto = dr.GetOrdinal("codigo_puesto");
                            int postNombrePuesto = dr.GetOrdinal("nombre");

                            PuestoCLS objPuestoCLS;
                            while (dr.Read())
                            {
                                objPuestoCLS = new PuestoCLS();
                                objPuestoCLS.CodigoPuesto = dr.GetInt16(postCodigoPuesto);
                                objPuestoCLS.Nombre = dr.GetString(postNombrePuesto);
                                listaPuestos.Add(objPuestoCLS);
                            }//fin while
                            objEmpleadoComboCLS.listaPuestos = listaPuestos;
                        }

                        if (dr.NextResult())
                        {
                            List<TipoBackToBackCLS> listaTiposBackToBack = new List<TipoBackToBackCLS>();
                            int postCodigoTipoBTB = dr.GetOrdinal("codigo_tipo_btb");
                            int postNombreTipoBTB = dr.GetOrdinal("nombre");

                            TipoBackToBackCLS objTipoBackToBackCLS;
                            while (dr.Read())
                            {
                                objTipoBackToBackCLS = new TipoBackToBackCLS();
                                objTipoBackToBackCLS.CodigoTipoBackToBack = dr.GetByte(postCodigoTipoBTB);
                                objTipoBackToBackCLS.Nombre = dr.GetString(postNombreTipoBTB);
                                listaTiposBackToBack.Add(objTipoBackToBackCLS);
                            }//fin while
                            objEmpleadoComboCLS.listaTiposBackToBack = listaTiposBackToBack;
                        }

                    }// fin using
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                    objEmpleadoComboCLS = null;

                }
                return objEmpleadoComboCLS;
            }
        }

        public EmpleadoComboCLS GetListFillCombosConsulta()
        {
            EmpleadoComboCLS objEmpleadoComboCLS = new EmpleadoComboCLS();
            using (SqlConnection conexion = new SqlConnection(cadenaRRHH))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_rrhh.uspFillComboFiltroConsulta", conexion))
                    {
                        // le indico que es un procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        // Devolvera varias tablas
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            List<EmpresaCLS> listaEmpresas = new List<EmpresaCLS>();
                            int postCodigoEmpresa = dr.GetOrdinal("codigo_empresa");
                            int postNombreEmpresa = dr.GetOrdinal("nombre_comercial");

                            EmpresaCLS objEmpresaCLS;
                            while (dr.Read())
                            {
                                objEmpresaCLS = new EmpresaCLS();
                                objEmpresaCLS.CodigoEmpresa = dr.GetInt16(postCodigoEmpresa);
                                objEmpresaCLS.NombreComercial = dr.GetString(postNombreEmpresa);
                                listaEmpresas.Add(objEmpresaCLS);
                            }//fin while
                            objEmpleadoComboCLS.listaEmpresas = listaEmpresas;
                        }// fin if

                        if (dr.NextResult())
                        {
                            List<AreaCLS> listaAreas = new List<AreaCLS>();
                            int postCodigoArea = dr.GetOrdinal("codigo_area");
                            int postNombreArea = dr.GetOrdinal("nombre");

                            AreaCLS objAreaCLS;
                            while (dr.Read())
                            {
                                objAreaCLS = new AreaCLS();
                                objAreaCLS.CodigoArea = dr.GetInt16(postCodigoArea);
                                objAreaCLS.Nombre = dr.GetString(postNombreArea);
                                listaAreas.Add(objAreaCLS);
                            }//fin while
                            objEmpleadoComboCLS.listaAreas = listaAreas;
                        }

                        if (dr.NextResult())
                        {
                            List<PuestoCLS> listaPuestos = new List<PuestoCLS>();
                            int postCodigoPuesto = dr.GetOrdinal("codigo_puesto");
                            int postNombrePuesto = dr.GetOrdinal("nombre");

                            PuestoCLS objPuestoCLS;
                            while (dr.Read())
                            {
                                objPuestoCLS = new PuestoCLS();
                                objPuestoCLS.CodigoPuesto = dr.GetInt16(postCodigoPuesto);
                                objPuestoCLS.Nombre = dr.GetString(postNombrePuesto);
                                listaPuestos.Add(objPuestoCLS);
                            }//fin while
                            objEmpleadoComboCLS.listaPuestos = listaPuestos;
                        }

                        if (dr.NextResult())
                        {
                            List<EstadoEmpleadoCLS> listaEstadosEmpleado = new List<EstadoEmpleadoCLS>();
                            int postCodigoEstadoEmpleado = dr.GetOrdinal("codigo_estado_empleado");
                            int postNombre = dr.GetOrdinal("nombre");

                            EstadoEmpleadoCLS objEstadoEmpleadoCLS;
                            while (dr.Read())
                            {
                                objEstadoEmpleadoCLS = new EstadoEmpleadoCLS();
                                objEstadoEmpleadoCLS.CodigoEstadoEmpleado = dr.GetInt16(postCodigoEstadoEmpleado);
                                objEstadoEmpleadoCLS.EstadoEmpleado = dr.GetString(postNombre);
                                listaEstadosEmpleado.Add(objEstadoEmpleadoCLS);
                            }//fin while
                            objEmpleadoComboCLS.listaEstadosEmpleado = listaEstadosEmpleado;
                        }

                    }// fin using
                    conexion.Close();
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    objEmpleadoComboCLS = null;

                }
                return objEmpleadoComboCLS;
            }
        }

        public string GuardarEmpleado(EmpleadoCLS objEmpleado, string usuarioIng)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaRRHH))
            {
                conexion.Open();

                SqlCommand cmd = conexion.CreateCommand();

                try
                {
                    string sentenciaSQL = @"
                    INSERT INTO db_rrhh.empleado( codigo_empresa,
                                                  codigo_empleado,
                                                  codigo_tipo_identificacion,
                                                  cui,
                                                  nit,
                                                  correo_electronico,
                                                  numero_afiliacion,
                                                  empleado_externo,
                                                  codigo_area,
                                                  codigo_seccion,
                                                  codigo_puesto,
                                                  codigo_tipo_cuenta,
                                                  codigo_ubicacion,
                                                  numero_cuenta,
                                                  monto_devengado,
                                                  codigo_jornada,
                                                  codigo_frecuencia_pago,
                                                  igss,
                                                  bono_de_ley,
                                                  retencion_isr,
                                                  fecha_ingreso,
                                                  fecha_egreso,
                                                  codigo_motivo_baja,
                                                  observaciones,
                                                  codigo_estado,
                                                  usuario_ing,
                                                  fecha_ing,
                                                  usuario_act,
                                                  fecha_act,
                                                  back_to_back,
                                                  actualizado)
                    VALUES( @CodigoEmpresa,
                            @CodigoEmpleado,
                            @CodigoTipoIdentificacion,
                            @Cui,
                            @Nit,
                            @CorreoElectronico,
                            @NumeroAfiliacion,
                            @EmpleadoExterno,
                            @CodigoArea,
                            @CodigoSeccion,
                            @CodigoPuesto,
                            @CodigoTipoCuenta,  
                            @CodigoUbicacion,
                            @NumeroCuenta,
                            @MontoDevengado,
                            @CodigoJornada,
                            @CodigoFrecuenciaPago,
                            @Igss,
                            @BonoDeLey,
                            @RetencionISR,
                            @FechaIngreso,
                            @FechaEgreso,
                            @CodigoMotivoBaja,
                            @Observaciones,
                            @CodigoEstadoEmpleado,
                            @UsuarioIng,
                            @FechaIng,
                            @UsuarioAct,
                            @FechaAct,
                            @BackToBack,
                            @Actualizado
                     )";


                    cmd.CommandText = sentenciaSQL;
                    cmd.Parameters.AddWithValue("@CodigoEmpresa", objEmpleado.CodigoEmpresa);
                    cmd.Parameters.AddWithValue("@CodigoEmpleado", objEmpleado.CodigoEmpleado);
                    cmd.Parameters.AddWithValue("@CodigoTipoIdentificacion", objEmpleado.CodigoTipoIdentificacion);
                    cmd.Parameters.AddWithValue("@Cui", objEmpleado.Cui == null ? DBNull.Value : objEmpleado.Cui);
                    cmd.Parameters.AddWithValue("@Nit", objEmpleado.Nit == null ? DBNull.Value : objEmpleado.Nit);
                    cmd.Parameters.AddWithValue("@CorreoElectronico", objEmpleado.CorreoElectronico == null ? DBNull.Value : objEmpleado.CorreoElectronico);
                    cmd.Parameters.AddWithValue("@NumeroAfiliacion", objEmpleado.NumeroAfiliacion == null ? DBNull.Value : objEmpleado.NumeroAfiliacion);
                    cmd.Parameters.AddWithValue("@EmpleadoExterno", objEmpleado.EmpleadoExterno);
                    cmd.Parameters.AddWithValue("@CodigoArea", objEmpleado.CodigoArea);
                    cmd.Parameters.AddWithValue("@CodigoSeccion", objEmpleado.CodigoSeccion);
                    cmd.Parameters.AddWithValue("@CodigoPuesto", objEmpleado.CodigoPuesto);
                    cmd.Parameters.AddWithValue("@CodigoTipoCuenta", objEmpleado.CodigoTipoCuenta);
                    cmd.Parameters.AddWithValue("@CodigoUbicacion", objEmpleado.CodigoUbicacion);
                    cmd.Parameters.AddWithValue("@NumeroCuenta", objEmpleado.NumeroCuenta == null ? DBNull.Value : objEmpleado.NumeroCuenta);
                    cmd.Parameters.AddWithValue("@MontoDevengado", objEmpleado.MontoDevengado);
                    cmd.Parameters.AddWithValue("@CodigoJornada", objEmpleado.CodigoJornada);
                    cmd.Parameters.AddWithValue("@CodigoFrecuenciaPago", objEmpleado.CodigoFrecuenciaPago);
                    cmd.Parameters.AddWithValue("@Igss", objEmpleado.Igss);
                    cmd.Parameters.AddWithValue("@BonoDeLey", objEmpleado.BonoDeLey);
                    cmd.Parameters.AddWithValue("@RetencionISR", objEmpleado.RetencionIsr);
                    cmd.Parameters.AddWithValue("@FechaIngreso", objEmpleado.FechaIngreso);
                    cmd.Parameters.AddWithValue("@FechaEgreso", DBNull.Value);
                    cmd.Parameters.AddWithValue("@CodigoMotivoBaja", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Observaciones", objEmpleado.Observaciones == null ? DBNull.Value : objEmpleado.Observaciones);
                    cmd.Parameters.AddWithValue("@CodigoEstadoEmpleado", Constantes.Empleado.EstadoEmpleado.ACTIVO);
                    cmd.Parameters.AddWithValue("@UsuarioIng", usuarioIng);
                    cmd.Parameters.AddWithValue("@FechaIng",DateTime.Now);
                    cmd.Parameters.AddWithValue("@UsuarioAct",DBNull.Value);
                    cmd.Parameters.AddWithValue("@FechaAct",DBNull.Value);
                    cmd.Parameters.AddWithValue("@BackToBack", objEmpleado.BackToBack);
                    cmd.Parameters.AddWithValue("@Actualizado", 1);


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

        public string ActualizarEmpleado(EmpleadoCLS objEmpleado, string usuarioAct)
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
                    INSERT INTO db_rrhh.empleado_hist SELECT NEXT VALUE FOR db_rrhh.SQ_CODIGO_HIST_EMPLEADO,
                                                        codigo_empresa,
                                                        codigo_empleado,
                                                        codigo_tipo_identificacion,
                                                        cui,
                                                        nit,
                                                        correo_electronico,
                                                        numero_afiliacion,
                                                        empleado_externo,
                                                        codigo_area,
                                                        codigo_seccion,
                                                        codigo_puesto,
                                                        codigo_tipo_cuenta,
                                                        codigo_ubicacion,
                                                        numero_cuenta,
                                                        monto_devengado,
                                                        codigo_jornada,
                                                        codigo_frecuencia_pago,
                                                        igss,
                                                        bono_de_ley,
                                                        retencion_isr,
                                                        fecha_ingreso,
                                                        fecha_egreso,
                                                        codigo_motivo_baja,
                                                        observaciones,
                                                        codigo_estado,
                                                        @UsuarioIng AS usuario_ing,
                                                        @FechaIng AS fecha_ing,
                                                        NULL AS usuario_act,
                                                        NULL AS fecha_act,
                                                        back_to_back,
                                                        monto_descuento_prestamo

                                                    FROM db_rrhh.empleado
                                                    WHERE codigo_empresa = @CodigoEmpresa AND
                                                          codigo_empleado = @CodigoEmpleado";

                    cmd.CommandText = sentenciaInsertHistorial;
                    cmd.Parameters.AddWithValue("@CodigoEmpresa", objEmpleado.CodigoEmpresa);
                    cmd.Parameters.AddWithValue("@CodigoEmpleado", objEmpleado.CodigoEmpleado);
                    cmd.Parameters.AddWithValue("@UsuarioIng", usuarioAct);
                    cmd.Parameters.AddWithValue("@FechaIng", DateTime.Now);
                    cmd.ExecuteNonQuery();

                    string sentenciaSQL = @"
                    UPDATE db_rrhh.empleado 
                    SET nit = @Nit,
                        correo_electronico = @CorreoElectronico,
                        numero_afiliacion = @NumeroAfiliacion,
                        empleado_externo = @EmpleadoExterno,
                        codigo_area = @CodigoArea,
                        codigo_seccion = @CodigoSeccion,
                        codigo_puesto = @CodigoPuesto,
                        codigo_tipo_cuenta = @CodigoTipoCuenta,
                        codigo_ubicacion = @CodigoUbicacion,
                        numero_cuenta = @NumeroCuenta,
                        monto_devengado = @MontoDevengado,
                        codigo_jornada = @CodigoJornada,
                        codigo_frecuencia_pago = @CodigoFrecuenciaPago,
                        igss = @Igss,
                        bono_de_ley = @BonoDeLey,
                        retencion_isr = @RetencionISR,
                        fecha_ingreso = @FechaIngreso,
                        usuario_act = @UsuarioAct,
                        fecha_act = @FechaAct,
                        actualizado = @Actualizado,
                        observaciones = @Observaciones
                    WHERE codigo_empleado = @CodigoEmpleado AND codigo_empresa = @CodigoEmpresa";


                    cmd.CommandText = sentenciaSQL;
                    objEmpleado.NombreCompleto = objEmpleado.PrimerNombre + (objEmpleado.SegundoNombre == null ? "" : " " + objEmpleado.SegundoNombre) + (objEmpleado.TercerNombre == null ? "" : " " + objEmpleado.TercerNombre) + (objEmpleado.PrimerApellido == null ? "" : " " + objEmpleado.PrimerApellido) + (objEmpleado.SegundoApellido == null ? "" : " " + objEmpleado.SegundoApellido);
                    //cmd.Parameters.AddWithValue("@CodigoEmpresa", objEmpleado.CodigoEmpresa);
                    //cmd.Parameters.AddWithValue("@CodigoEmpleado", objEmpleado.CodigoEmpleado);
                    cmd.Parameters.AddWithValue("@Nit", objEmpleado.Nit == null ? DBNull.Value : objEmpleado.Nit);
                    cmd.Parameters.AddWithValue("@CorreoElectronico", objEmpleado.CorreoElectronico == null ? DBNull.Value : objEmpleado.CorreoElectronico);
                    cmd.Parameters.AddWithValue("@NumeroAfiliacion", objEmpleado.NumeroAfiliacion == null ? DBNull.Value : objEmpleado.NumeroAfiliacion);
                    cmd.Parameters.AddWithValue("@EmpleadoExterno", objEmpleado.EmpleadoExterno);
                    cmd.Parameters.AddWithValue("@CodigoArea", objEmpleado.CodigoArea);
                    cmd.Parameters.AddWithValue("@CodigoSeccion", objEmpleado.CodigoSeccion);
                    cmd.Parameters.AddWithValue("@CodigoPuesto", objEmpleado.CodigoPuesto);
                    cmd.Parameters.AddWithValue("@CodigoTipoCuenta", objEmpleado.CodigoTipoCuenta);
                    cmd.Parameters.AddWithValue("@CodigoUbicacion", objEmpleado.CodigoUbicacion);
                    cmd.Parameters.AddWithValue("@NumeroCuenta", objEmpleado.NumeroCuenta == null ? DBNull.Value : objEmpleado.NumeroCuenta);
                    cmd.Parameters.AddWithValue("@MontoDevengado", objEmpleado.MontoDevengado);
                    cmd.Parameters.AddWithValue("@CodigoJornada", objEmpleado.CodigoJornada);
                    cmd.Parameters.AddWithValue("@CodigoFrecuenciaPago", objEmpleado.CodigoFrecuenciaPago);
                    cmd.Parameters.AddWithValue("@Igss", objEmpleado.Igss);
                    cmd.Parameters.AddWithValue("@BonoDeLey", objEmpleado.BonoDeLey);
                    cmd.Parameters.AddWithValue("@RetencionISR", objEmpleado.RetencionIsr);
                    cmd.Parameters.AddWithValue("@FechaIngreso", objEmpleado.FechaIngreso);
                    cmd.Parameters.AddWithValue("@UsuarioAct", usuarioAct);
                    cmd.Parameters.AddWithValue("@FechaAct", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Actualizado", 1);
                    cmd.Parameters.AddWithValue("@Observaciones", objEmpleado.Observaciones == null ? DBNull.Value : objEmpleado.Observaciones);
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

        public string ActualizarEmpleadoOperacionPendiente(EmpleadoCLS objEmpleado, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaRRHH))
            {
                try
                {
                    string sql = @"
                    UPDATE db_rrhh.empleado
                    SET saldo_prestamo = @SaldoPrestamo,
                        pago_pendiente = @PagoPendiente,
                        usuario_act = @UsuarioAct,
                        fecha_act = @FechaAct
                    WHERE codigo_empresa = @CodigoEmpresa 
                      AND codigo_empleado = @CodigoEmpleado";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@SaldoPrestamo", objEmpleado.SaldoPrestamo);
                        cmd.Parameters.AddWithValue("@PagoPendiente", objEmpleado.PagoPendiente);
                        cmd.Parameters.AddWithValue("@CodigoEmpresa", objEmpleado.CodigoEmpresa);
                        cmd.Parameters.AddWithValue("@CodigoEmpleado", objEmpleado.CodigoEmpleado);
                        cmd.Parameters.AddWithValue("@UsuarioAct", usuarioAct);
                        cmd.Parameters.AddWithValue("@FechaAct", DateTime.Now);

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

        public string ActualizarEmpleadoPlanilla(EmpleadoCLS objEmpleado, string usuarioAct)
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
                    INSERT INTO db_rrhh.empleado_hist SELECT NEXT VALUE FOR db_rrhh.SQ_CODIGO_HIST_EMPLEADO,
                                                        codigo_empresa,
                                                        codigo_empleado,
                                                        codigo_tipo_identificacion,
                                                        cui,
                                                        nit,
                                                        correo_electronico,
                                                        numero_afiliacion,
                                                        empleado_externo,
                                                        codigo_area,
                                                        codigo_seccion,
                                                        codigo_puesto,
                                                        codigo_tipo_cuenta,
                                                        codigo_ubicacion,
                                                        numero_cuenta,
                                                        monto_devengado,
                                                        codigo_jornada,
                                                        codigo_frecuencia_pago,
                                                        igss,
                                                        bono_de_ley,
                                                        retencion_isr,
                                                        fecha_ingreso,
                                                        fecha_egreso,
                                                        codigo_motivo_baja,
                                                        observaciones,
                                                        codigo_estado,
                                                        @UsuarioIng AS usuario_ing,
                                                        @FechaIng AS fecha_ing,
                                                        NULL AS usuario_act,
                                                        NULL AS fecha_act,
                                                        back_to_back,
                                                        monto_descuento_prestamo
                                                    FROM db_rrhh.empleado
                                                    WHERE codigo_empresa = @CodigoEmpresa AND
                                                          codigo_empleado = @CodigoEmpleado";

                    cmd.CommandText = sentenciaInsertHistorial;
                    cmd.Parameters.AddWithValue("@CodigoEmpresa", objEmpleado.CodigoEmpresa);
                    cmd.Parameters.AddWithValue("@CodigoEmpleado", objEmpleado.CodigoEmpleado);
                    cmd.Parameters.AddWithValue("@UsuarioIng", usuarioAct);
                    cmd.Parameters.AddWithValue("@FechaIng", DateTime.Now);
                    cmd.ExecuteNonQuery();

                    string sentenciaSQL = @"
                    UPDATE db_rrhh.empleado 
                    SET nit = @Nit,
                        correo_electronico = @CorreoElectronico,
                        numero_afiliacion = @NumeroAfiliacion,
                        empleado_externo = @EmpleadoExterno,
                        codigo_area = @CodigoArea,
                        codigo_seccion = @CodigoSeccion,
                        codigo_puesto = @CodigoPuesto,
                        codigo_tipo_cuenta = @CodigoTipoCuenta,
                        codigo_ubicacion = @CodigoUbicacion,
                        numero_cuenta = @NumeroCuenta,
                        monto_devengado = @MontoDevengado,
                        codigo_jornada = @CodigoJornada,
                        codigo_frecuencia_pago = @CodigoFrecuenciaPago,
                        igss = @Igss,
                        bono_de_ley = @BonoDeLey,
                        retencion_isr = @RetencionISR,
                        fecha_ingreso = @FechaIngreso,
                        back_to_back = @BackToBack,
                        codigo_tipo_btb = @CodigoTipoBTB,
                        usuario_act = @UsuarioAct,
                        fecha_act = @FechaAct,
                        actualizado = @Actualizado,
                        observaciones = @Observaciones,
                        salario_diario = @SalarioDiario,
                        bono_decreto_37_2001 = @BonoDecreto372001
                    WHERE codigo_empleado = @CodigoEmpleado AND codigo_empresa = @CodigoEmpresa";


                    cmd.CommandText = sentenciaSQL;
                    //cmd.Parameters.AddWithValue("@CodigoEmpresa", objEmpleado.CodigoEmpresa);
                    //cmd.Parameters.AddWithValue("@CodigoEmpleado", objEmpleado.CodigoEmpleado);
                    cmd.Parameters.AddWithValue("@Nit", objEmpleado.Nit == null ? DBNull.Value : objEmpleado.Nit);
                    cmd.Parameters.AddWithValue("@CorreoElectronico", objEmpleado.CorreoElectronico == null ? DBNull.Value : objEmpleado.CorreoElectronico);
                    cmd.Parameters.AddWithValue("@NumeroAfiliacion", objEmpleado.NumeroAfiliacion == null ? DBNull.Value : objEmpleado.NumeroAfiliacion);
                    cmd.Parameters.AddWithValue("@EmpleadoExterno", objEmpleado.EmpleadoExterno);
                    cmd.Parameters.AddWithValue("@CodigoArea", objEmpleado.CodigoArea);
                    cmd.Parameters.AddWithValue("@CodigoSeccion", objEmpleado.CodigoSeccion);
                    cmd.Parameters.AddWithValue("@CodigoPuesto", objEmpleado.CodigoPuesto);
                    cmd.Parameters.AddWithValue("@CodigoTipoCuenta", objEmpleado.CodigoTipoCuenta);
                    cmd.Parameters.AddWithValue("@CodigoUbicacion", objEmpleado.CodigoUbicacion);
                    cmd.Parameters.AddWithValue("@NumeroCuenta", objEmpleado.NumeroCuenta == null ? DBNull.Value : objEmpleado.NumeroCuenta);
                    cmd.Parameters.AddWithValue("@MontoDevengado", objEmpleado.MontoDevengado);
                    cmd.Parameters.AddWithValue("@CodigoJornada", objEmpleado.CodigoJornada);
                    cmd.Parameters.AddWithValue("@CodigoFrecuenciaPago", objEmpleado.CodigoFrecuenciaPago);
                    cmd.Parameters.AddWithValue("@Igss", objEmpleado.Igss);
                    cmd.Parameters.AddWithValue("@BonoDeLey", objEmpleado.BonoDeLey);
                    cmd.Parameters.AddWithValue("@RetencionISR", objEmpleado.RetencionIsr);
                    cmd.Parameters.AddWithValue("@FechaIngreso", objEmpleado.FechaIngreso);
                    cmd.Parameters.AddWithValue("@UsuarioAct", usuarioAct);
                    cmd.Parameters.AddWithValue("@FechaAct", DateTime.Now);
                    cmd.Parameters.AddWithValue("@CodigoTipoBTB", objEmpleado.CodigoTipoBackToBack);
                    cmd.Parameters.AddWithValue("@SalarioDiario", objEmpleado.SalarioDiario);
                    cmd.Parameters.AddWithValue("@BonoDecreto372001", objEmpleado.BonoDecreto372001);

                    if (objEmpleado.CodigoTipoBackToBack == 0)
                    {
                        cmd.Parameters.AddWithValue("@BackToBack", 0);
                    }
                    else {
                        cmd.Parameters.AddWithValue("@BackToBack", 1);
                    }
                    cmd.Parameters.AddWithValue("@Actualizado", 1);
                    cmd.Parameters.AddWithValue("@Observaciones", objEmpleado.Observaciones == null ? DBNull.Value : objEmpleado.Observaciones);
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

        public string DarDeBajaEmpleadoPorSuspension(SuspensionCLS objSuspension, string usuarioAct)
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
                    string sentenciaUpdateEmpleado = @"
                    UPDATE db_rrhh.empleado
                    SET codigo_estado = @CodigoEstadoEmpleado,
                        usuario_act = @UsuarioAct,
                        fecha_act = @FechaAct
                    WHERE codigo_empresa = @CodigoEmpresa AND
                          codigo_empleado = @CodigoEmpleado";

                    cmd.CommandText = sentenciaUpdateEmpleado;
                    cmd.Parameters.AddWithValue("@CodigoEstadoEmpleado", Constantes.Empleado.EstadoEmpleado.SUSPENDIDO);
                    cmd.Parameters.AddWithValue("@CodigoEmpresa", objSuspension.CodigoEmpresa);
                    cmd.Parameters.AddWithValue("@CodigoEmpleado", objSuspension.CodigoEmpleado);
                    cmd.Parameters.AddWithValue("@UsuarioAct", usuarioAct);
                    cmd.Parameters.AddWithValue("@FechaAct", DateTime.Now);
                    cmd.ExecuteNonQuery();

                    string sentenciaInsertSuspension = @"
                    INSERT INTO db_rrhh.suspension(codigo_suspension,codigo_empresa,codigo_empleado,fecha_inicio_suspension,fecha_fin_suspension,codigo_motivo_suspension,observaciones,fecha_alta,estado,usuario_ing,	fecha_ing,usuario_act,fecha_act)
                    VALUES(NEXT VALUE FOR db_rrhh.SQ_CODIGO_SUSPENSION,
                           @CodigoEmpresa,
                           @CodigoEmpleado,
                           @FechaInicioSuspension,
                           @FechaFinSuspension,
                           @CodigoMotivoSuspension,
                           @Observaciones,
                           @FechaAlta,
                           @CodigoEstado,
                           @UsuarioIng,
                           @FechaIng,
                           NULL,
                           NULL)";

                    cmd.CommandText = sentenciaInsertSuspension;
                    //cmd.Parameters.AddWithValue("@CodigoEmpresa", objEmpleado.CodigoEmpresa);
                    //cmd.Parameters.AddWithValue("@CodigoEmpleado", objEmpleado.CodigoEmpleado);
                    cmd.Parameters.AddWithValue("@FechaInicioSuspension", objSuspension.FechaInicioSuspension);
                    cmd.Parameters.AddWithValue("@FechaFinSuspension", objSuspension.FechaFinSuspension);
                    cmd.Parameters.AddWithValue("@CodigoMotivoSuspension", objSuspension.CodigoMotivoSuspension);
                    cmd.Parameters.AddWithValue("@Observaciones", objSuspension.Observaciones == null ? DBNull.Value : objSuspension.Observaciones);
                    cmd.Parameters.AddWithValue("@FechaAlta", objSuspension.FechaAlta == null ? DBNull.Value : objSuspension.FechaAlta);
                    cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.EstadoRegistro.ACTIVO);
                    cmd.Parameters.AddWithValue("@UsuarioIng", usuarioAct);
                    cmd.Parameters.AddWithValue("@FechaIng", DateTime.Now);
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

        public string DarDeBajaEmpleado(SuspensionCLS objSuspension, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaRRHH))
            {
                try
                {
                    string sql = @"UPDATE db_rrhh.empleado
                    SET codigo_estado = @CodigoEstadoEmpleado,
                        fecha_egreso = @FechaEgreso,
                        usuario_act = @UsuarioAct,
                        fecha_act = @FechaAct,
                        observaciones = @Observaciones
                    WHERE codigo_empresa = @CodigoEmpresa AND
                          codigo_empleado = @CodigoEmpleado";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoEmpleado", Constantes.Empleado.EstadoEmpleado.RETIRADO);
                        cmd.Parameters.AddWithValue("@FechaEgreso", objSuspension.FechaEgreso);
                        cmd.Parameters.AddWithValue("@UsuarioAct", usuarioAct);
                        cmd.Parameters.AddWithValue("@FechaAct", DateTime.Now);
                        cmd.Parameters.AddWithValue("@CodigoEmpresa", objSuspension.CodigoEmpresa);
                        cmd.Parameters.AddWithValue("@CodigoEmpleado", objSuspension.CodigoEmpleado);
                        cmd.Parameters.AddWithValue("@Observaciones", objSuspension.Observaciones == null ? DBNull.Value : objSuspension.Observaciones);

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

    }
}

