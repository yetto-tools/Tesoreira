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
    public class EntidadDAL:CadenaConexion
    {

        public List<EntidadCLS> GetEntidadesGasto(int codigoOperacion)
        {
            List<EntidadCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    SELECT codigo_entidad, 
	                       nombre_completo,
                           codigo_categoria_entidad
                    FROM db_tesoreria.entidad 
                    WHERE codigo_operacion = @CodigoOperacion
                      AND estado = @CodigoEstadoEntidad
                    ORDER BY nombre_completo ASC";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoOperacion", codigoOperacion);
                        cmd.Parameters.AddWithValue("@CodigoEstadoEntidad", Constantes.EstadoRegistro.ACTIVO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            EntidadCLS objEntidad;
                            lista = new List<EntidadCLS>();
                            int postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            int postNombreEntidad = dr.GetOrdinal("nombre_completo");
                            int postCodigoCategoriaEntidad = dr.GetOrdinal("codigo_categoria_entidad");
                            while (dr.Read())
                            {
                                objEntidad = new EntidadCLS();
                                objEntidad.CodigoEntidad = dr.GetInt32(postCodigoEntidad);
                                objEntidad.Nombre = dr.GetString(postNombreEntidad);
                                objEntidad.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
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


        public List<EntidadCLS> GetEntidadesGenericasConfiguracion(int codigoCategoriaEntidad)
        {
            List<EntidadCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string filterCategoriaEntidad = string.Empty;
                    if (codigoCategoriaEntidad != -1) {
                        filterCategoriaEntidad = " AND y.codigo_categoria_entidad = " + codigoCategoriaEntidad.ToString();
                    }

                    string sql = @"
                    SELECT x.codigo_entidad, 
		                   x.nombre_completo, 
		                   y.codigo_categoria_entidad, 
		                   y.nombre AS categoria_entidad,
		                   x.codigo_operacion,
		                   z.nombre_operacion,
                           1 AS permiso_editar
                    FROM db_tesoreria.entidad x
                    INNER JOIN db_tesoreria.entidad_categoria y
                    ON x.codigo_categoria_entidad = y.codigo_categoria_entidad
                    INNER JOIN db_tesoreria.operacion z
                    ON x.codigo_operacion = z.codigo_operacion
                    WHERE x.estado = @CodigoEstadoActivo
                      AND y.incluir_config_operacion = 1
                    " + filterCategoriaEntidad;

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoActivo", Constantes.EstadoRegistro.ACTIVO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            EntidadCLS objEntidad;
                            lista = new List<EntidadCLS>();
                            int postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            int postNombreEntidad = dr.GetOrdinal("nombre_completo");
                            int postCodigoCategoriaEntidad = dr.GetOrdinal("codigo_categoria_entidad");
                            int postCategoriaEntidad = dr.GetOrdinal("categoria_entidad");
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postOperacion = dr.GetOrdinal("nombre_operacion");
                            int postPermisoEditar = dr.GetOrdinal("permiso_editar");

                            while (dr.Read())
                            {
                                objEntidad = new EntidadCLS();
                                objEntidad.CodigoEntidad = dr.GetInt32(postCodigoEntidad);
                                objEntidad.Nombre = dr.GetString(postNombreEntidad);
                                objEntidad.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objEntidad.NombreCategoriaEntidad = dr.GetString(postCategoriaEntidad);
                                objEntidad.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objEntidad.Operacion = dr.GetString(postOperacion);
                                objEntidad.PermisoEditar = (byte)dr.GetInt32(postPermisoEditar);
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

        public List<EntidadGenericaCLS> ListarEntidadesGenericas()
        {
            List<EntidadGenericaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    SELECT y.codigo_empleado as codigo_entidad,
                           db_rrhh.GetNombreCompletoEmpleado(y.cui) AS nombre_completo,
                           36 AS codigo_categoria_entidad, 
	                       'Empleado (' + x.nombre + ')' AS categoria_entidad,
                           0 AS codigo_operacion_caja,
                           y.codigo_area,
                           0 AS codigo_operacion_entidad, 
                           0 AS codigo_canal_venta
                    FROM db_rrhh.empleado y
                    INNER JOIN db_rrhh.area x
                    ON y.codigo_area = x.codigo_area
                    WHERE (y.codigo_estado <> @CodigoEstadoEmpleado OR y.saldo_prestamo = 1 OR y.pago_pendiente = 1) 

                    UNION

                    SELECT y.cui as codigo_entidad,
	                       y.nombre_completo,
                           CASE
                             WHEN y.codigo_area = 0 THEN 43
                             ELSE 41
                           END AS codigo_categoria_entidad, 
                           CASE  
                             WHEN y.codigo_area = 0 THEN 'Persona Indirecta'
                             ELSE  CONCAT('Empleado Indirecto', CASE WHEN x.nombre IS NOT NULL THEN CONCAT('(',x.nombre,')') ELSE '' END)
                           END AS  categoria_entidad,
                           0 AS codigo_operacion_caja,
                           ISNULL(x.codigo_area,0) AS codigo_area,
                           0 AS codigo_operacion_entidad, 
                           0 AS codigo_canal_venta,
                           '' AS mes_planilla_btb,
                           0 AS anio_planilla_btb,
                           0.00 AS monto_devolucion_btb
                    FROM db_rrhh.persona y
                    LEFT JOIN db_rrhh.area x
                    ON y.codigo_area = x.codigo_area
                    WHERE y.no_incluido_en_planilla = 1

                    UNION

                    SELECT x.codigo_vendedor as codigo_entidad, 
	                       y.nombre_completo,
	                       CASE
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.VENDEDOR.ToString() + " then " + Constantes.Entidad.Categoria.VENDEDOR.ToString() + @"
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.RUTERO_LOCAL.ToString() + @" then " + Constantes.Entidad.Categoria.RUTERO_LOCAL.ToString() + @"
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.RUTERO_INTERIOR.ToString() + @" then " + Constantes.Entidad.Categoria.RUTERO_INTERIOR.ToString() + @"
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.CAFETERIAS.ToString() + @" then " + Constantes.Entidad.Categoria.CAFETERIA.ToString() + @"
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.SUPERMERCADOS.ToString() + @" then " + Constantes.Entidad.Categoria.SUPERMERCADO.ToString() + @"
		                    ELSE 0
                           END AS codigo_categoria_entidad,
	                       z.nombre as categoria_entidad,
                           CASE
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.VENDEDOR.ToString() + " then " + Constantes.Operacion.Ingreso.VENDEDOR.ToString() + @"
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.RUTERO_LOCAL.ToString() + @" then " + Constantes.Operacion.Ingreso.RUTERO_LOCAL.ToString() + @"
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.RUTERO_INTERIOR.ToString() + @" then " + Constantes.Operacion.Ingreso.RUTERO_INTERIOR.ToString() + @"
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.CAFETERIAS.ToString() + @" then " + Constantes.Operacion.Ingreso.CAFETERIAS.ToString() + @"
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.SUPERMERCADOS.ToString() + @" then " + Constantes.Operacion.Ingreso.SUPERMERCADOS.ToString() + @"
		                    ELSE 0
                           END AS codigo_operacion_caja,
                           0 AS codigo_area,
                           0 AS codigo_operacion_entidad,
                           x.codigo_canal_venta,
                           '' AS mes_planilla_btb,
                           0 AS anio_planilla_btb,
                           0.00 AS monto_devolucion_btb
                    FROM db_ventas.config_vendedor_ruta x
                    INNER JOIN db_ventas.vendedor y
                    ON x.codigo_vendedor = y.codigo_vendedor
                    INNER JOIN db_ventas.canal_venta z
                    ON x.codigo_canal_venta = z.codigo_canal_venta
                    WHERE x.estado = @EstadoVendedor

                    UNION

                    SELECT codigo_cliente as codigo_entidad, 
	                       nombre_completo,
                           CASE 
                             WHEN codigo_tipo_cliente = " + Constantes.Cliente.Tipo.ESPECIALES_1.ToString() + @" THEN " + Constantes.Entidad.Categoria.CLIENTES_ESPECIALES_1.ToString() + @"
                             WHEN codigo_tipo_cliente = " + Constantes.Cliente.Tipo.ESPECIALES_2.ToString() + @" THEN " + Constantes.Entidad.Categoria.CLIENTES_ESPECIALES_2.ToString() + @"
                             ELSE  " + Constantes.Entidad.Categoria.OTROS_CLIENTES.ToString() + @"
                           END AS codigo_categoria_entidad, 
                           CASE 
                             WHEN codigo_tipo_cliente = " + Constantes.Cliente.Tipo.ESPECIALES_1.ToString() + @" THEN 'Cliente (Especiales I)'
                             ELSE 'Cliente (Especiales II)'
                           END AS categoria_entidad,
                           CASE 
                             WHEN codigo_tipo_cliente = " + Constantes.Cliente.Tipo.ESPECIALES_1.ToString() + @" THEN  " + Constantes.Operacion.Ingreso.ESPECIALES_1.ToString() + @"
                             ELSE " + Constantes.Operacion.Ingreso.ESPECIALES_2.ToString() + @" 
                           END AS codigo_operacion_caja,
                           0 AS codigo_area,
                           0 AS codigo_operacion_entidad, 
                           0 AS codigo_canal_venta,
                           '' AS mes_planilla_btb,
                           0 AS anio_planilla_btb,
                           0.00 AS monto_devolucion_btb
                    FROM db_ventas.cliente
                    WHERE estado = @EstadoCliente 
                      AND codigo_tipo_cliente IN (2,3)

                    UNION

                    SELECT CAST (y.codigo_entidad AS VARCHAR(15)) AS codigo_entidad, 
		                   y.nombre_completo, 
		                   y.codigo_categoria_entidad, 
		                   x.nombre AS categoria_entidad,
                           CASE
                            WHEN y.codigo_categoria_entidad = " + Constantes.Entidad.Categoria.CLIENTES_ESPECIALES_1.ToString() + @" then " + Constantes.Operacion.Ingreso.ESPECIALES_1.ToString() + @"
                            ELSE 0
                           END AS codigo_operacion_caja,
                           0 AS codigo_area,
                           y.codigo_operacion AS codigo_operacion_entidad,
                           0 AS codigo_canal_venta,
                           '' AS mes_planilla_btb,
                           0 AS anio_planilla_btb,
                           0.00 AS monto_devolucion_btb
                    FROM db_tesoreria.entidad y
                    INNER JOIN db_tesoreria.entidad_categoria x
                    ON y.codigo_categoria_entidad = x.codigo_categoria_entidad
                    WHERE y.estado = @EstadoEntidad AND y.codigo_entidad <> 0

                    UNION

                    SELECT y.codigo_entidad, 
	                       y.nombre_completo, 
	                       y.codigo_categoria_entidad, 
	                       y.categoria_entidad, 
	                       y.codigo_operacion_caja, 
	                       y.codigo_area,
                           0 AS codigo_operacion_entidad, 
                           0 AS codigo_canal_venta,
                           '' AS mes_planilla_btb,
                           0 AS anio_planilla_btb,
                           0.00 AS monto_devolucion_btb
                    FROM( SELECT CAST(codigo_empresa AS VARCHAR(15)) AS codigo_entidad,
                                 nombre_comercial AS nombre_completo,
                                 1 AS codigo_categoria_entidad,
                                 'Empresa' AS categoria_entidad,
                                 0 AS codigo_operacion_caja,
                                 0 AS codigo_area
                           FROM db_admon.empresa
                           WHERE estado = 1
                        ) y
                    WHERE 1 = 1";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoEmpleado", Constantes.Empleado.EstadoEmpleado.RETIRADO);
                        cmd.Parameters.AddWithValue("@EstadoVendedor", Constantes.EstadoRegistro.ACTIVO);
                        cmd.Parameters.AddWithValue("@EstadoCliente", Constantes.EstadoRegistro.ACTIVO);
                        cmd.Parameters.AddWithValue("@EstadoEntidad", Constantes.EstadoRegistro.ACTIVO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            EntidadGenericaCLS objEntidad;
                            lista = new List<EntidadGenericaCLS>();
                            int postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            int postNombreEntidad = dr.GetOrdinal("nombre_completo");
                            int postCodigoCategoridaEntidad = dr.GetOrdinal("codigo_categoria_entidad");
                            int postCategoridaEntidad = dr.GetOrdinal("categoria_entidad");
                            int postCodigoOperacionCaja = dr.GetOrdinal("codigo_operacion_caja");
                            int postCodigoArea = dr.GetOrdinal("codigo_area");
                            int postCodigoOperacionEntidad = dr.GetOrdinal("codigo_operacion_entidad");
                            int postCodigoCanalVenta = dr.GetOrdinal("codigo_canal_venta");
                            while (dr.Read())
                            {
                                objEntidad = new EntidadGenericaCLS();
                                objEntidad.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objEntidad.NombreEntidad = dr.GetString(postNombreEntidad);
                                objEntidad.CodigoCategoriaEntidad = dr.GetInt32(postCodigoCategoridaEntidad);
                                objEntidad.NombreCategoria = dr.GetString(postCategoridaEntidad);
                                objEntidad.CodigoOperacionCaja = (Int16)dr.GetInt32(postCodigoOperacionCaja);
                                objEntidad.CodigoArea = (Int16)dr.GetInt32(postCodigoArea);
                                objEntidad.CodigoOperacionEntidad = (Int16)dr.GetInt32(postCodigoOperacionEntidad);
                                objEntidad.CodigoCanalVenta = (Int16)dr.GetInt32(postCodigoCanalVenta);
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

        public EntidadGenericaListCLS GetAllEntidadesGenericas()
        {
            EntidadGenericaListCLS objEntidadesGenericas = new EntidadGenericaListCLS();
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    SELECT y.codigo_empleado as codigo_entidad,
                           db_rrhh.GetNombreCompletoEmpleado(y.cui) AS nombre_completo,
                           36 AS codigo_categoria_entidad, 
	                       'Empleado (' + x.nombre + ')' AS categoria_entidad,
                           0 AS codigo_operacion_caja,
                           y.codigo_area,
                           0 AS codigo_operacion_entidad, 
                           0 AS codigo_canal_venta,
                           y.codigo_tipo_btb,
                           '' AS mes_planilla_btb,
                           0 AS anio_planilla_btb,
                           0.00 AS monto_devolucion_btb,
                           0 AS concede_iva,
                           CAST(0 AS BigInt) AS codigo_cxc_btb,
                           0 AS codigo_pago_btb 
                    FROM db_rrhh.empleado y
                    INNER JOIN db_rrhh.area x
                    ON y.codigo_area = x.codigo_area
                    WHERE (y.codigo_estado <> @CodigoEstadoEmpleado OR y.saldo_prestamo = 1 OR y.pago_pendiente = 1) 

                    UNION

                    SELECT x.codigo_entidad, 
	                       x.nombre_entidad AS nombre_completo,
	                       36 AS codigo_categoria_entidad, 
	                       'Empleado (' + w.nombre + ')' AS categoria_entidad,
		                   0 AS codigo_operacion_caja,
                           z.codigo_area,
                           0 AS codigo_operacion_entidad, 
                           0 AS codigo_canal_venta,
                           z.codigo_tipo_btb,
		                   CASE
			                 WHEN y.mes = 1 THEN CONCAT('ENERO',' (',a.nombre,')')
		                     WHEN y.mes = 2 THEN CONCAT('FEBRERO',' (',a.nombre,')')
		                     WHEN y.mes = 3 THEN CONCAT('MARZO',' (',a.nombre,')')
		                     WHEN y.mes = 4 THEN CONCAT('ABRIL',' (',a.nombre,')')
		                     WHEN y.mes = 5 THEN CONCAT('MAYO',' (',a.nombre,')')
		                     WHEN y.mes = 6 THEN CONCAT('JUNIO',' (',a.nombre,')')
		                     WHEN y.mes = 7 THEN CONCAT('JULIO',' (',a.nombre,')')
		                     WHEN y.mes = 8 THEN CONCAT('AGOSTO',' (',a.nombre,')')
		                     WHEN y.mes = 9 THEN CONCAT('SEPTIEMBRE',' (',a.nombre,')')
		                     WHEN y.mes = 10 THEN CONCAT('OCTUBRE',' (',a.nombre,')')
		                     WHEN y.mes = 11 THEN CONCAT('NOVIEMBRE',' (',a.nombre,')')
		                     WHEN y.mes = 12 THEN CONCAT('DICIEMBRE',' (',a.nombre,')')
		                     ELSE 'NO DEFINIDO'
		                   END AS mes_planilla_btb,
                           y.anio AS anio_planilla_btb,
                           x.monto AS monto_devolucion_btb,
                           0 AS concede_iva,
	                       x.codigo_cxc AS codigo_cxc_btb, 
	                       x.codigo_pago AS codigo_pago_btb
                    FROM db_contabilidad.cuenta_por_cobrar x
                    INNER JOIN db_contabilidad.pagos_y_descuentos y
                    ON x.codigo_pago = y.codigo_pago
                    INNER JOIN db_rrhh.empleado z
                    ON y.codigo_empresa = z.codigo_empresa AND y.codigo_empleado = z.codigo_empleado
                    INNER JOIN db_rrhh.area w
                    ON z.codigo_area = w.codigo_area
                    INNER JOIN db_contabilidad.tipo_planilla a
                    ON y.codigo_tipo_planilla = a.codigo_tipo_planilla
                    WHERE y.codigo_operacion = 65
                      AND (x.codigo_estado_pago_btb = 0 OR db_tesoreria.GetEstadoTransaccionBTB(x.codigo_transaccion_pago_btb) = 0)
                      AND x.codigo_estado <> 0

                    UNION

                    SELECT y.cui as codigo_entidad,
	                       y.nombre_completo,
                           CASE
                             WHEN y.codigo_area = 0 THEN 43
                             ELSE 41
                           END AS codigo_categoria_entidad, 
                           CASE  
                             WHEN y.codigo_area = 0 THEN 'Persona Indirecta'
                             ELSE  CONCAT('Empleado Indirecto', CASE WHEN x.nombre IS NOT NULL THEN CONCAT('(',x.nombre,')') ELSE '' END)
                           END AS  categoria_entidad,
                           0 AS codigo_operacion_caja,
                           ISNULL(x.codigo_area,0) AS codigo_area,
                           0 AS codigo_operacion_entidad, 
                           0 AS codigo_canal_venta,
                           0 AS codigo_tipo_btb,
                           '' AS mes_planilla_btb,
                           0 AS anio_planilla_btb,
                           0.00 AS monto_devolucion_btb,
                           0 AS concede_iva,
                           CAST(0 AS BigInt) AS codigo_cxc_btb,
                           0 AS codigo_pago_btb 
                    FROM db_rrhh.persona y
                    LEFT JOIN db_rrhh.area x
                    ON y.codigo_area = x.codigo_area
                    WHERE y.no_incluido_en_planilla = 1

                    UNION

                    SELECT x.codigo_vendedor as codigo_entidad, 
	                       y.nombre_completo,
	                       CASE
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.VENDEDOR.ToString() + " then " + Constantes.Entidad.Categoria.VENDEDOR.ToString() + @"
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.RUTERO_LOCAL.ToString() + @" then " + Constantes.Entidad.Categoria.RUTERO_LOCAL.ToString() + @"
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.RUTERO_INTERIOR.ToString() + @" then " + Constantes.Entidad.Categoria.RUTERO_INTERIOR.ToString() + @"
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.CAFETERIAS.ToString() + @" then " + Constantes.Entidad.Categoria.CAFETERIA.ToString() + @"
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.SUPERMERCADOS.ToString() + @" then " + Constantes.Entidad.Categoria.SUPERMERCADO.ToString() + @"
		                    ELSE 0
                           END AS codigo_categoria_entidad,
	                       z.nombre as categoria_entidad,
                           CASE
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.VENDEDOR.ToString() + " then " + Constantes.Operacion.Ingreso.VENDEDOR.ToString() + @"
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.RUTERO_LOCAL.ToString() + @" then " + Constantes.Operacion.Ingreso.RUTERO_LOCAL.ToString() + @"
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.RUTERO_INTERIOR.ToString() + @" then " + Constantes.Operacion.Ingreso.RUTERO_INTERIOR.ToString() + @"
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.CAFETERIAS.ToString() + @" then " + Constantes.Operacion.Ingreso.CAFETERIAS.ToString() + @"
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.SUPERMERCADOS.ToString() + @" then " + Constantes.Operacion.Ingreso.SUPERMERCADOS.ToString() + @"
		                    ELSE 0
                           END AS codigo_operacion_caja,
                           0 AS codigo_area,
                           0 AS codigo_operacion_entidad,
                           x.codigo_canal_venta,
                           0 AS codigo_tipo_btb,
                           '' AS mes_planilla_btb,
                           0 AS anio_planilla_btb,
                           0.00 AS monto_devolucion_btb,
                           0 AS concede_iva,
                           CAST(0 AS BigInt) AS codigo_cxc_btb,
                           0 AS codigo_pago_btb 
                    FROM db_ventas.config_vendedor_ruta x
                    INNER JOIN db_ventas.vendedor y
                    ON x.codigo_vendedor = y.codigo_vendedor
                    INNER JOIN db_ventas.canal_venta z
                    ON x.codigo_canal_venta = z.codigo_canal_venta
                    WHERE x.estado = @EstadoVendedor

                    UNION

                    SELECT codigo_cliente as codigo_entidad, 
	                       nombre_completo,
                           CASE 
                             WHEN codigo_tipo_cliente = " + Constantes.Cliente.Tipo.ESPECIALES_1.ToString() + @" THEN " + Constantes.Entidad.Categoria.CLIENTES_ESPECIALES_1.ToString() + @"
                             WHEN codigo_tipo_cliente = " + Constantes.Cliente.Tipo.ESPECIALES_2.ToString() + @" THEN " + Constantes.Entidad.Categoria.CLIENTES_ESPECIALES_2.ToString() + @"
                             ELSE  " + Constantes.Entidad.Categoria.OTROS_CLIENTES.ToString() + @"
                           END AS codigo_categoria_entidad, 
                           CASE 
                             WHEN codigo_tipo_cliente = " + Constantes.Cliente.Tipo.ESPECIALES_1.ToString() + @" THEN 'Cliente (Especiales I)'
                             ELSE 'Cliente (Especiales II)'
                           END AS categoria_entidad,
                           CASE 
                             WHEN codigo_tipo_cliente = " + Constantes.Cliente.Tipo.ESPECIALES_1.ToString() + @" THEN  " + Constantes.Operacion.Ingreso.ESPECIALES_1.ToString() + @"
                             ELSE " + Constantes.Operacion.Ingreso.ESPECIALES_2.ToString() + @" 
                           END AS codigo_operacion_caja,
                           0 AS codigo_area,
                           0 AS codigo_operacion_entidad, 
                           0 AS codigo_canal_venta,
                           0 AS codigo_tipo_btb,
                           '' AS mes_planilla_btb,
                           0 AS anio_planilla_btb,
                           0.00 AS monto_devolucion_btb,
                           0 AS concede_iva,
                           CAST(0 AS BigInt) AS codigo_cxc_btb,
                           0 AS codigo_pago_btb 
                    FROM db_ventas.cliente
                    WHERE estado = @EstadoCliente 
                      AND codigo_tipo_cliente IN (2,3)

                    UNION

                    SELECT CAST (y.codigo_entidad AS VARCHAR(15)) AS codigo_entidad, 
		                   y.nombre_completo, 
		                   y.codigo_categoria_entidad, 
		                   x.nombre AS categoria_entidad,
                           CASE
                            WHEN y.codigo_categoria_entidad = " + Constantes.Entidad.Categoria.CLIENTES_ESPECIALES_1.ToString() + @" then " + Constantes.Operacion.Ingreso.ESPECIALES_1.ToString() + @"
                            ELSE 0
                           END AS codigo_operacion_caja,
                           0 AS codigo_area,
                           y.codigo_operacion AS codigo_operacion_entidad,
                           0 AS codigo_canal_venta,
                           0 AS codigo_tipo_btb,
                           '' AS mes_planilla_btb,
                           0 AS anio_planilla_btb,
                           0.00 AS monto_devolucion_btb,
                           y.concede_iva,
                           CAST(0 AS BigInt) AS codigo_cxc_btb,
                           0 AS codigo_pago_btb 
                    FROM db_tesoreria.entidad y
                    INNER JOIN db_tesoreria.entidad_categoria x
                    ON y.codigo_categoria_entidad = x.codigo_categoria_entidad
                    WHERE y.estado = @EstadoEntidad AND y.codigo_entidad <> 0

                    UNION

                    SELECT y.codigo_entidad, 
	                       y.nombre_completo, 
	                       y.codigo_categoria_entidad, 
	                       y.categoria_entidad, 
	                       y.codigo_operacion_caja, 
	                       y.codigo_area,
                           0 AS codigo_operacion_entidad, 
                           0 AS codigo_canal_venta,
                           0 AS codigo_tipo_btb,
                           '' AS mes_planilla_btb,
                           0 AS anio_planilla_btb,
                           0.00 AS monto_devolucion_btb,
                           0 AS concede_iva,
                           CAST(0 AS BigInt) AS codigo_cxc_btb,
                           0 AS codigo_pago_btb 
                    FROM( SELECT CAST(codigo_empresa AS VARCHAR(15)) AS codigo_entidad,
                                 nombre_comercial AS nombre_completo,
                                 1 AS codigo_categoria_entidad,
                                 'Empresa' AS categoria_entidad,
                                 0 AS codigo_operacion_caja,
                                 0 AS codigo_area
                           FROM db_admon.empresa
                           WHERE estado = 1
                        ) y
                    WHERE 1 = 1";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoEmpleado", Constantes.Empleado.EstadoEmpleado.RETIRADO);
                        cmd.Parameters.AddWithValue("@EstadoVendedor", Constantes.EstadoRegistro.ACTIVO);
                        cmd.Parameters.AddWithValue("@EstadoCliente", Constantes.EstadoRegistro.ACTIVO);
                        cmd.Parameters.AddWithValue("@EstadoEntidad", Constantes.EstadoRegistro.ACTIVO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            EntidadGenericaCLS objEntidad;
                            int postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            int postNombreEntidad = dr.GetOrdinal("nombre_completo");
                            int postCodigoCategoridaEntidad = dr.GetOrdinal("codigo_categoria_entidad");
                            int postCategoridaEntidad = dr.GetOrdinal("categoria_entidad");
                            int postCodigoOperacionCaja = dr.GetOrdinal("codigo_operacion_caja");
                            int postCodigoArea = dr.GetOrdinal("codigo_area");
                            int postCodigoOperacionEntidad = dr.GetOrdinal("codigo_operacion_entidad");
                            int postCodigoCanalVenta = dr.GetOrdinal("codigo_canal_venta");
                            int postCodigoTipoBTB = dr.GetOrdinal("codigo_tipo_btb");
                            int postMesPlanillaBTB = dr.GetOrdinal("mes_planilla_btb");
                            int postAnioPlanillaBTB = dr.GetOrdinal("anio_planilla_btb");
                            int postMontoDevolucionBTB = dr.GetOrdinal("monto_devolucion_btb");
                            int postConcedeIva = dr.GetOrdinal("concede_iva");
                            int postCodigoCxCBTB = dr.GetOrdinal("codigo_cxc_btb");
                            int postCodigoPagoBTB = dr.GetOrdinal("codigo_pago_btb");

                            List<EntidadGenericaCLS> listaGenerica = new List<EntidadGenericaCLS>();
                            List<EntidadGenericaCLS> listaEspeciales1 = new List<EntidadGenericaCLS>();
                            List<EntidadGenericaCLS> listaEspeciales2 = new List<EntidadGenericaCLS>();
                            List<EntidadGenericaCLS> listaBackToBack = new List<EntidadGenericaCLS>();
                            List<EntidadGenericaCLS> listaVendedores = new List<EntidadGenericaCLS>();
                            List<EntidadGenericaCLS> listaEmpresasConcedeIVA = new List<EntidadGenericaCLS>();
                            while (dr.Read())
                            {
                                objEntidad = new EntidadGenericaCLS();
                                objEntidad.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objEntidad.NombreEntidad = dr.GetString(postNombreEntidad);
                                objEntidad.CodigoCategoriaEntidad = dr.GetInt32(postCodigoCategoridaEntidad);
                                objEntidad.NombreCategoria = dr.GetString(postCategoridaEntidad);
                                objEntidad.CodigoOperacionCaja = (Int16)dr.GetInt32(postCodigoOperacionCaja);
                                objEntidad.CodigoArea = (Int16)dr.GetInt32(postCodigoArea);
                                objEntidad.CodigoOperacionEntidad = (Int16)dr.GetInt32(postCodigoOperacionEntidad);
                                objEntidad.CodigoCanalVenta = (Int16)dr.GetInt32(postCodigoCanalVenta);
                                objEntidad.CodigoTipoBTB = dr.GetInt32(postCodigoTipoBTB);
                                objEntidad.MesPlanillaBTB = dr.IsDBNull(postMesPlanillaBTB) ? "" : dr.GetString(postMesPlanillaBTB);
                                objEntidad.AnioPlanillaBTB = dr.IsDBNull(postAnioPlanillaBTB) ? (short)0 : (short)dr.GetInt32(postAnioPlanillaBTB);
                                objEntidad.MontoDevolucionBTB = dr.IsDBNull(postMontoDevolucionBTB) ? 0 : dr.GetDecimal(postMontoDevolucionBTB);
                                objEntidad.ConcedeIva = (byte)dr.GetInt32(postConcedeIva);
                                objEntidad.CodigoCxCBTB = dr.GetInt64(postCodigoCxCBTB);
                                objEntidad.CodigoPagoBTB = dr.GetInt32(postCodigoPagoBTB);

                                switch (objEntidad.CodigoCategoriaEntidad)
                                {
                                    case Constantes.Entidad.Categoria.CLIENTES_ESPECIALES_1:
                                        listaEspeciales1.Add(objEntidad);
                                        break;

                                    case Constantes.Entidad.Categoria.CLIENTES_ESPECIALES_2:
                                        listaEspeciales2.Add(objEntidad);
                                        break;
                                    default:
                                        if (objEntidad.CodigoPagoBTB  > 0) 
                                        {// Empleados BTB que tiene pagos por devolver en tesoreria
                                            listaBackToBack.Add(objEntidad);
                                        }
                                        if (objEntidad.CodigoCategoriaEntidad == Constantes.Entidad.Categoria.VENDEDOR ||
                                            objEntidad.CodigoCategoriaEntidad == Constantes.Entidad.Categoria.RUTERO_LOCAL ||
                                            objEntidad.CodigoCategoriaEntidad == Constantes.Entidad.Categoria.RUTERO_INTERIOR ||
                                            objEntidad.CodigoCategoriaEntidad == Constantes.Entidad.Categoria.SUPERMERCADO ||
                                            objEntidad.CodigoCategoriaEntidad == Constantes.Entidad.Categoria.CAFETERIA)
                                        {
                                            listaVendedores.Add(objEntidad);
                                        }
                                        if (objEntidad.ConcedeIva == 1)
                                        {
                                            listaEmpresasConcedeIVA.Add(objEntidad);
                                        }
                                        else {
                                            if (objEntidad.CodigoPagoBTB == 0)
                                            {
                                                listaGenerica.Add(objEntidad);
                                            }
                                        }

                                        break;
                                }
                            }
                            objEntidadesGenericas.listaEntidadesGenericas = listaGenerica;
                            objEntidadesGenericas.listaEntidadesEspeciales1 = listaEspeciales1;
                            objEntidadesGenericas.listaEntidadesEspeciales2 = listaEspeciales2;
                            objEntidadesGenericas.listaEntidadesBackToBack = listaBackToBack;
                            objEntidadesGenericas.listaEntidadesVendedores = listaVendedores;
                            objEntidadesGenericas.listaEmpresasConcedeIVA = listaEmpresasConcedeIVA;

                        }
                    }
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                    objEntidadesGenericas = null;
                }

                return objEntidadesGenericas;
            }
        }

        public List<EntidadGenericaCLS> ListarEntidadesGenericasCxC()
        {
            List<EntidadGenericaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    SELECT y.codigo_empleado as codigo_entidad,
                           db_rrhh.GetNombreCompletoEmpleado(y.cui) AS nombre_completo,
                           36 AS codigo_categoria_entidad, 
	                       'Empleado (' + x.nombre + ')' AS categoria_entidad,
                           0 AS codigo_operacion_caja,
                           y.codigo_area
                    FROM db_rrhh.empleado y
                    INNER JOIN db_rrhh.area x
                    ON y.codigo_area = x.codigo_area

                    UNION

                    SELECT y.cui as codigo_entidad,
	                       y.nombre_completo,
                           CASE
                             WHEN y.codigo_area = 0 THEN 43
                             ELSE 41
                           END AS codigo_categoria_entidad, 
                           CASE  
                             WHEN y.codigo_area = 0 THEN 'Persona Indirecta'
                             ELSE  CONCAT('Empleado Indirecto', CASE WHEN x.nombre IS NOT NULL THEN CONCAT('(',x.nombre,')') ELSE '' END)
                           END AS  categoria_entidad,
                           0 AS codigo_operacion_caja,
                           ISNULL(x.codigo_area,0) AS codigo_area
                    FROM db_rrhh.persona y
                    LEFT JOIN db_rrhh.area x
                    ON y.codigo_area = x.codigo_area
                    WHERE y.no_incluido_en_planilla = 1

                    UNION

                    SELECT x.codigo_vendedor as codigo_entidad, 
	                       y.nombre_completo,
	                       CASE
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.VENDEDOR.ToString() + " then " + Constantes.Entidad.Categoria.VENDEDOR.ToString() + @"
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.RUTERO_LOCAL.ToString() + @" then " + Constantes.Entidad.Categoria.RUTERO_LOCAL.ToString() + @"
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.RUTERO_INTERIOR.ToString() + @" then " + Constantes.Entidad.Categoria.RUTERO_INTERIOR.ToString() + @"
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.CAFETERIAS.ToString() + @" then " + Constantes.Entidad.Categoria.CAFETERIA.ToString() + @"
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.SUPERMERCADOS.ToString() + @" then " + Constantes.Entidad.Categoria.SUPERMERCADO.ToString() + @"
		                    ELSE 0
                           END AS codigo_categoria_entidad,
	                       z.nombre as categoria_entidad,
                           CASE
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.VENDEDOR.ToString() + " then " + Constantes.Operacion.Ingreso.VENDEDOR.ToString() + @"
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.RUTERO_LOCAL.ToString() + @" then " + Constantes.Operacion.Ingreso.RUTERO_LOCAL.ToString() + @"
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.RUTERO_INTERIOR.ToString() + @" then " + Constantes.Operacion.Ingreso.RUTERO_INTERIOR.ToString() + @"
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.CAFETERIAS.ToString() + @" then " + Constantes.Operacion.Ingreso.CAFETERIAS.ToString() + @"
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.SUPERMERCADOS.ToString() + @" then " + Constantes.Operacion.Ingreso.SUPERMERCADOS.ToString() + @"
		                    ELSE 0
                           END AS codigo_operacion_caja,
                           0 AS codigo_area 
                    FROM db_ventas.config_vendedor_ruta x
                    INNER JOIN db_ventas.vendedor y
                    ON x.codigo_vendedor = y.codigo_vendedor
                    INNER JOIN db_ventas.canal_venta z
                    ON x.codigo_canal_venta = z.codigo_canal_venta
                    WHERE x.estado = @EstadoVendedor

                    UNION

                    SELECT CAST (y.codigo_entidad AS VARCHAR(15)) AS codigo_entidad, 
		                   y.nombre_completo, 
		                   y.codigo_categoria_entidad, 
		                   x.nombre AS categoria_entidad,
                           CASE
                            WHEN y.codigo_categoria_entidad = " + Constantes.Entidad.Categoria.CLIENTES_ESPECIALES_1.ToString() + @" then " + Constantes.Operacion.Ingreso.ESPECIALES_1.ToString() + @"
                            ELSE 0
                           END AS codigo_operacion_caja,
                           0 AS codigo_area
                        
                    FROM db_tesoreria.entidad y
                    INNER JOIN db_tesoreria.entidad_categoria x
                    ON y.codigo_categoria_entidad = x.codigo_categoria_entidad
                    WHERE y.estado = @EstadoEntidad 
                      AND y.codigo_entidad <> 0
                      AND y.codigo_categoria_entidad = " + Constantes.Entidad.Categoria.SOCIO.ToString();

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        //cmd.Parameters.AddWithValue("@CodigoEstadoEmpleado", Constantes.Empleado.EstadoEmpleado.RETIRADO);
                        cmd.Parameters.AddWithValue("@EstadoVendedor", Constantes.EstadoRegistro.ACTIVO);
                        cmd.Parameters.AddWithValue("@EstadoEntidad", Constantes.EstadoRegistro.ACTIVO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            EntidadGenericaCLS objEntidad;
                            lista = new List<EntidadGenericaCLS>();
                            int postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            int postNombreEntidad = dr.GetOrdinal("nombre_completo");
                            int postCodigoCategoridaEntidad = dr.GetOrdinal("codigo_categoria_entidad");
                            int postCategoridaEntidad = dr.GetOrdinal("categoria_entidad");
                            int postCodigoOperacionCaja = dr.GetOrdinal("codigo_operacion_caja");
                            int postCodigoArea = dr.GetOrdinal("codigo_area");

                            while (dr.Read())
                            {
                                objEntidad = new EntidadGenericaCLS();
                                objEntidad.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objEntidad.NombreEntidad = dr.GetString(postNombreEntidad);
                                objEntidad.CodigoCategoriaEntidad = dr.GetInt32(postCodigoCategoridaEntidad);
                                objEntidad.NombreCategoria = dr.GetString(postCategoridaEntidad);
                                objEntidad.CodigoOperacionCaja = (Int16)dr.GetInt32(postCodigoOperacionCaja);
                                objEntidad.CodigoArea = (Int16)dr.GetInt32(postCodigoArea);
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

        public List<EntidadGenericaCLS> ListarEntidadesGenericasCxCPorPrestamosNoRegistradosEnTesoreria()
        {
            List<EntidadGenericaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    SELECT y.codigo_empleado as codigo_entidad,
                           db_rrhh.GetNombreCompletoEmpleado(y.cui) AS nombre_completo,
                           36 AS codigo_categoria_entidad, 
	                       'Empleado (' + x.nombre + ')' AS categoria_entidad,
                           0 AS codigo_operacion_caja,
                           y.codigo_area
                    FROM db_rrhh.empleado y
                    INNER JOIN db_rrhh.area x
                    ON y.codigo_area = x.codigo_area
                    WHERE (y.codigo_estado NOT IN (@CodigoEstadoEmpleadoInactivo, @CodigoEstadoEmpleadoRetirado) OR y.saldo_prestamo = 1)

                    UNION

                    SELECT y.cui as codigo_entidad,
	                       y.nombre_completo,
                           CASE
                             WHEN y.codigo_area = 0 THEN 43
                             ELSE 41
                           END AS codigo_categoria_entidad, 
                           CASE  
                             WHEN y.codigo_area = 0 THEN 'Persona Indirecta'
                             ELSE  CONCAT('Empleado Indirecto', CASE WHEN x.nombre IS NOT NULL THEN CONCAT('(',x.nombre,')') ELSE '' END)
                           END AS  categoria_entidad,
                           0 AS codigo_operacion_caja,
                           ISNULL(x.codigo_area,0) AS codigo_area
                    FROM db_rrhh.persona y
                    LEFT JOIN db_rrhh.area x
                    ON y.codigo_area = x.codigo_area
                    WHERE y.no_incluido_en_planilla = 1

                    UNION

                    SELECT x.codigo_vendedor as codigo_entidad, 
	                       y.nombre_completo,
	                       CASE
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.VENDEDOR.ToString() + " then " + Constantes.Entidad.Categoria.VENDEDOR.ToString() + @"
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.RUTERO_LOCAL.ToString() + @" then " + Constantes.Entidad.Categoria.RUTERO_LOCAL.ToString() + @"
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.RUTERO_INTERIOR.ToString() + @" then " + Constantes.Entidad.Categoria.RUTERO_INTERIOR.ToString() + @"
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.CAFETERIAS.ToString() + @" then " + Constantes.Entidad.Categoria.CAFETERIA.ToString() + @"
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.SUPERMERCADOS.ToString() + @" then " + Constantes.Entidad.Categoria.SUPERMERCADO.ToString() + @"
		                    ELSE 0
                           END AS codigo_categoria_entidad,
	                       z.nombre as categoria_entidad,
                           CASE
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.VENDEDOR.ToString() + " then " + Constantes.Operacion.Ingreso.VENDEDOR.ToString() + @"
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.RUTERO_LOCAL.ToString() + @" then " + Constantes.Operacion.Ingreso.RUTERO_LOCAL.ToString() + @"
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.RUTERO_INTERIOR.ToString() + @" then " + Constantes.Operacion.Ingreso.RUTERO_INTERIOR.ToString() + @"
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.CAFETERIAS.ToString() + @" then " + Constantes.Operacion.Ingreso.CAFETERIAS.ToString() + @"
		                    when x.codigo_canal_venta = " + Constantes.CanalVenta.SUPERMERCADOS.ToString() + @" then " + Constantes.Operacion.Ingreso.SUPERMERCADOS.ToString() + @"
		                    ELSE 0
                           END AS codigo_operacion_caja,
                           0 AS codigo_area 
                    FROM db_ventas.config_vendedor_ruta x
                    INNER JOIN db_ventas.vendedor y
                    ON x.codigo_vendedor = y.codigo_vendedor
                    INNER JOIN db_ventas.canal_venta z
                    ON x.codigo_canal_venta = z.codigo_canal_venta
                    WHERE x.estado = @EstadoVendedor

                    UNION

                    SELECT CAST (y.codigo_entidad AS VARCHAR(15)) AS codigo_entidad, 
		                   y.nombre_completo, 
		                   y.codigo_categoria_entidad, 
		                   x.nombre AS categoria_entidad,
                           CASE
                            WHEN y.codigo_categoria_entidad = " + Constantes.Entidad.Categoria.CLIENTES_ESPECIALES_1.ToString() + @" then " + Constantes.Operacion.Ingreso.ESPECIALES_1.ToString() + @"
                            ELSE 0
                           END AS codigo_operacion_caja,
                           0 AS codigo_area
                        
                    FROM db_tesoreria.entidad y
                    INNER JOIN db_tesoreria.entidad_categoria x
                    ON y.codigo_categoria_entidad = x.codigo_categoria_entidad
                    WHERE y.estado = @EstadoEntidad 
                      AND y.codigo_entidad <> 0
                      AND y.codigo_categoria_entidad = " + Constantes.Entidad.Categoria.SOCIO.ToString();

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoEmpleadoInactivo", Constantes.Empleado.EstadoEmpleado.INACTIVO);
                        cmd.Parameters.AddWithValue("@CodigoEstadoEmpleadoRetirado", Constantes.Empleado.EstadoEmpleado.RETIRADO);
                        cmd.Parameters.AddWithValue("@EstadoVendedor", Constantes.EstadoRegistro.ACTIVO);
                        cmd.Parameters.AddWithValue("@EstadoEntidad", Constantes.EstadoRegistro.ACTIVO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            EntidadGenericaCLS objEntidad;
                            lista = new List<EntidadGenericaCLS>();
                            int postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            int postNombreEntidad = dr.GetOrdinal("nombre_completo");
                            int postCodigoCategoridaEntidad = dr.GetOrdinal("codigo_categoria_entidad");
                            int postCategoridaEntidad = dr.GetOrdinal("categoria_entidad");
                            int postCodigoOperacionCaja = dr.GetOrdinal("codigo_operacion_caja");
                            int postCodigoArea = dr.GetOrdinal("codigo_area");

                            while (dr.Read())
                            {
                                objEntidad = new EntidadGenericaCLS();
                                objEntidad.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objEntidad.NombreEntidad = dr.GetString(postNombreEntidad);
                                objEntidad.CodigoCategoriaEntidad = dr.GetInt32(postCodigoCategoridaEntidad);
                                objEntidad.NombreCategoria = dr.GetString(postCategoridaEntidad);
                                objEntidad.CodigoOperacionCaja = (Int16)dr.GetInt32(postCodigoOperacionCaja);
                                objEntidad.CodigoArea = (Int16)dr.GetInt32(postCodigoArea);
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

        public string GuardarEntidad(EntidadGenericaCLS objEntidad, string usuarioIng)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                conexion.Open();

                SqlCommand cmd = conexion.CreateCommand();

                try
                {
                    string sqlSequence = "SELECT NEXT VALUE FOR db_tesoreria.SQ_ENTIDAD";
                    cmd.CommandText = sqlSequence;
                    long codigoEntidad = (long)cmd.ExecuteScalar();

                    string sentenciaSQL = @"
                    INSERT INTO db_tesoreria.entidad (codigo_entidad, codigo_categoria_entidad, nombre_completo, descripcion, estado, usuario_ing, fecha_ing)
                    VALUES(@codigoEntidad, @codigoCategoriaEntidad, @nombreCompleto, @descripcion, @codigoEstado, @usuarioIng, @fechaIng)";

                    cmd.CommandText = sentenciaSQL;
                    cmd.Parameters.AddWithValue("@codigoEntidad", (int)codigoEntidad);
                    cmd.Parameters.AddWithValue("@codigoCategoriaEntidad", objEntidad.CodigoCategoriaEntidad);
                    cmd.Parameters.AddWithValue("@nombreCompleto", objEntidad.NombreEntidad.ToUpper());
                    cmd.Parameters.AddWithValue("@descripcion", objEntidad.Descripcion == null ? DBNull.Value : objEntidad.Descripcion);
                    cmd.Parameters.AddWithValue("@codigoEstado", Constantes.EstadoRegistro.ACTIVO);
                    cmd.Parameters.AddWithValue("@usuarioIng", usuarioIng);
                    cmd.Parameters.AddWithValue("@fechaIng", DateTime.Now);

                    cmd.ExecuteNonQuery();

                    resultado = codigoEntidad.ToString();
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    resultado = "Error [0]: " + ex.Message;
                }
            }

            return resultado;
        }

        public string ActualizarOperacionEntidad(int codigoEntidad, int codigoOperacion, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                conexion.Open();
                SqlCommand cmd = conexion.CreateCommand();

                try
                {
                    string sentenciaUpdateEntidad = @"
                    UPDATE db_tesoreria.entidad
                    SET codigo_Operacion = @CodigoOperacion,
                        usuario_act = @UsuarioAct,
                        fecha_act = @FechaAct
                    WHERE codigo_entidad = @CodigoEntidad";

                    cmd.CommandText = sentenciaUpdateEntidad;
                    cmd.Parameters.AddWithValue("@CodigoOperacion", codigoOperacion);
                    cmd.Parameters.AddWithValue("@UsuarioAct", usuarioAct);
                    cmd.Parameters.AddWithValue("@FechaAct", DateTime.Now);
                    cmd.Parameters.AddWithValue("@CodigoEntidad", codigoEntidad);
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

