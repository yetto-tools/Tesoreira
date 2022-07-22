using CapaEntidad.QSystems;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.QSystems
{
    public class ReportesDAL: CadenaConexion
    {
        public List<FacturaVentasCLS> GetListaVentaPorRangoFechaDetallado(string codigoEmpresa, string fechaInicio, string fechaFin)
        {
            List<FacturaVentasCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaQSystems))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT convert(char,convert(datetime, convert(int,a.veh_fecha)),103) as fecha_factura_str,
	                       DATENAME(weekday, convert(datetime, convert(int,a.veh_fecha))) AS dia_semana,
	                       a.veh_tienda as codigo_tienda,
                           a.veh_terminal as codigo_caja,
                           a.veh_serie as serie_factura,
                           a.veh_factura as numero_factura,
                           isnull(ab.FEL_SERIE,'') as serie_factura_fel,
                           isnull(ab.FEL_NUMERO,'') as numero_factura_fel,
                           a.veh_cliente as codigo_cliente,
                           c.cli_nombre as nombre_cliente,
                           c.cli_nit as nit_cliente,
                           d.ext_nombrefac as facturado_a,
                           a.veh_vendedor as codigo_vendedor,
                           b.ven_nombre as nombre_vendedor,
                           a.veh_concred1 as forma_pago,
                           CASE
							 WHEN ( select count(*) 
                                                  from qsystems..ventlpos aa, 
                                                       qsystems..mastinvpos ab
                                                  where aa.vel_empresa=a.veh_empresa 
                                                    and aa.vel_tienda=a.veh_tienda 
                                                    and aa.vel_terminal=a.veh_terminal 
                                                    and aa.vel_numero=a.veh_numero 
                                                    and ab.inv_empresa = aa.vel_empresa 
                                                    and ab.inv_inventario=aa.vel_inventario 
                                                    and ab.inv_serieflag='E' 
                                                ) = 0 THEN 'Bien'
				
					           ELSE 'Servicio'
                           END AS clasificacion,
                           convert(decimal(18,2),a.veh_valor) as total_sin_iva,
                           convert(decimal(18,2),a.veh_iva) as total_iva,
                           convert(decimal(18,2),a.veh_total) as total_con_iva,
                           e.vel_linea as numero_linea,
                           e.vel_inventario as codigo_sku,
                           convert(decimal(18,2),e.vel_unidades) as cantidad,
                           convert(decimal(18,2),e.vel_precioun) as precio_unitario,
                           convert(decimal(18,2),e.vel_costotal) as total_por_articulo

                    FROM dbo.venthpos a
                    INNER JOIN dbo.vendedor b
                    ON b.ven_empresa = a.veh_empresa AND b.ven_codigo  = a.veh_vendedor
                    LEFT OUTER JOIN dbo.mastcli c
                    ON c.cli_empresa = a.veh_empresa AND c.cli_codigo  = a.veh_cliente
	                LEFT OUTER JOIN panificadora..qsys_documento ab
		            ON ab.empresa  = a.veh_empresa AND ab.tienda = a.veh_tienda and ab.terminal = a.veh_terminal and ab.factura  = a.veh_factura
	                LEFT OUTER JOIN dbo.clientes_extra d
		            ON d.ext_empresa = a.veh_empresa AND d.ext_cliente = a.veh_cliente
                    INNER JOIN dbo.ventlpos e
                    ON e.vel_empresa  = a.veh_empresa AND e.vel_tienda = a.veh_tienda AND e.vel_terminal = a.veh_terminal AND e.vel_numero = a.veh_numero
                    WHERE a.veh_status <> 'A'
                      AND a.veh_empresa = '" + codigoEmpresa + @"'
                      AND convert(int, a.veh_fecha) between CONVERT(INT,CONVERT(DATETIME,'" + fechaInicio + @"',103)) AND CONVERT(INT,CONVERT(DATETIME,'" + fechaFin + @"',103))
                    ORDER BY a.veh_empresa, a.veh_tienda, a.veh_terminal, a.veh_fecha, e.vel_numero, e.vel_linea";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            FacturaVentasCLS objFacturaVentas;
                            lista = new List<FacturaVentasCLS>();
                            int postFechaFacturaStr = dr.GetOrdinal("fecha_factura_str");
                            int postDiaSemana = dr.GetOrdinal("dia_semana");
                            int postCodigoTienda = dr.GetOrdinal("codigo_tienda");
                            int postCodigoCaja = dr.GetOrdinal("codigo_caja");
                            int postSerieFactura = dr.GetOrdinal("serie_factura");
                            int postNumeroFactura = dr.GetOrdinal("numero_factura");
                            int postSerieFacturaFel = dr.GetOrdinal("serie_factura_fel");
                            int postNumeroFacturaFel = dr.GetOrdinal("numero_factura_fel");
                            int postCodigoCliente = dr.GetOrdinal("codigo_cliente");
                            int postNombreCliente = dr.GetOrdinal("nombre_cliente");
                            int postNitCliente = dr.GetOrdinal("nit_cliente");
                            int postFacturadoA = dr.GetOrdinal("facturado_a");
                            int postCodigoVendedor = dr.GetOrdinal("codigo_vendedor");
                            int postNombreVendedor = dr.GetOrdinal("nombre_vendedor");
                            int postFormaPago = dr.GetOrdinal("forma_pago");
                            int postClasificacion = dr.GetOrdinal("clasificacion");
                            int postTotalSinIVA = dr.GetOrdinal("total_sin_iva");
                            int postTotalIVA = dr.GetOrdinal("total_iva");
                            int postTotalConIVA = dr.GetOrdinal("total_con_iva");
                            int postNumeroLinea = dr.GetOrdinal("numero_linea");
                            int postCodigoSKU = dr.GetOrdinal("codigo_sku");
                            int postCantidad = dr.GetOrdinal("cantidad");
                            int postPrecioUnitario = dr.GetOrdinal("precio_unitario");
                            int postTotalPorArticulo = dr.GetOrdinal("total_por_articulo");

                            while (dr.Read())
                            {
                                objFacturaVentas = new FacturaVentasCLS();
                                objFacturaVentas.FechaFacturaStr = dr.GetString(postFechaFacturaStr);
                                objFacturaVentas.DiaSemana = dr.GetString(postDiaSemana);
                                objFacturaVentas.CodigoTienda = dr.GetString(postCodigoTienda);
                                objFacturaVentas.CodigoCaja = dr.GetInt32(postCodigoCaja);
                                objFacturaVentas.SerieFactura = dr.GetString(postSerieFactura);
                                objFacturaVentas.NumeroFactura = dr.GetInt32(postNumeroFactura);
                                objFacturaVentas.SerieFacturaFEL = dr.IsDBNull(postSerieFacturaFel) ? "" : dr.GetString(postSerieFacturaFel);
                                objFacturaVentas.NumeroFacturaFEL = dr.IsDBNull(postNumeroFacturaFel) ? "" : dr.GetString(postNumeroFacturaFel);
                                objFacturaVentas.CodigoCliente = dr.GetString(postCodigoCliente);
                                objFacturaVentas.NombreCliente = dr.IsDBNull(postNombreCliente) ? "" :  dr.GetString(postNombreCliente);
                                objFacturaVentas.NitCliente = dr.IsDBNull(postNitCliente) ? "" : dr.GetString(postNitCliente);
                                objFacturaVentas.FacturadoA = dr.IsDBNull(postFacturadoA) ? "": dr.GetString(postFacturadoA);
                                objFacturaVentas.CodigoVendedor = dr.GetInt16(postCodigoVendedor);
                                objFacturaVentas.NombreVendedor = dr.GetString(postNombreVendedor);
                                objFacturaVentas.FormaPago = dr.GetString(postFormaPago);
                                objFacturaVentas.Clasificacion = dr.GetString(postClasificacion);
                                objFacturaVentas.TotalSinIVA = dr.GetDecimal(postTotalSinIVA);
                                objFacturaVentas.TotalIVA = dr.GetDecimal(postTotalIVA);
                                objFacturaVentas.TotalConIVA = dr.GetDecimal(postTotalConIVA);
                                objFacturaVentas.NumeroLinea = dr.GetInt32(postNumeroLinea);
                                objFacturaVentas.CodigoSKU = dr.GetString(postCodigoSKU);
                                objFacturaVentas.Cantidad = dr.GetDecimal(postCantidad);
                                objFacturaVentas.PrecioUnitario = dr.GetDecimal(postPrecioUnitario);
                                objFacturaVentas.TotalPorArticulo = dr.GetDecimal(postTotalPorArticulo);

                                lista.Add(objFacturaVentas);
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

        public List<ValeSalidaCLS> GetListaValesDeSalida(string codigoEmpresa, string fechaInicio, string fechaFin)
        {
            List<ValeSalidaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaQSystems))
            {
                try
                {
                    string sql = @"
                    SELECT convert(char,convert(datetime, convert(int,a.dvh_fecha)),103) AS fecha_emision_str,
	                       DATENAME(weekday, convert(datetime, convert(int,dvh_fecha))) AS dia_semana,
                           a.dvh_vendedor AS ruta,
	                       c.ven_nombre AS nombre_vendedor,
	                       a.dvh_numero AS numero_vale,
	                       b.dvl_linea AS linea,
	                       b.dvl_inventario AS codigo_inventario,
	                       d.inv_nombre AS descripcion,
	                       convert(decimal(18, 3), b.dvl_preciou) AS precio_unitario,
	                       convert(int, b.dvl_cantidad) AS cantidad

                    FROM qsystems.dbo.dist_liqvalesl b
                    INNER JOIN qsystems.dbo.dist_liqvalesh a
                    ON b.dvl_empresa = a.dvh_empresa AND b.dvl_numero = a.dvh_numero
                    INNER JOIN qsystems.dbo.vendedor c
                    ON a.dvh_empresa = c.ven_empresa AND a.dvh_vendedor = c.ven_codigo
                    INNER JOIN qsystems.dbo.mastinvpos d
                    ON b.dvl_empresa = d.inv_empresa AND b.dvl_inventario = d.inv_inventario
                    WHERE a.dvh_status <> 'A'
                      AND a.dvh_empresa = '" + codigoEmpresa + @"'
                      AND convert(int, a.dvh_fecha) between CONVERT(INT,CONVERT(DATETIME,'" + fechaInicio + @"',103)) AND CONVERT(INT,CONVERT(DATETIME,'" + fechaFin + @"',103))
                    ORDER BY a.dvh_fecha,a.dvh_vendedor,c.ven_nombre,a.dvh_numero,b.dvl_linea,b.dvl_inventario";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            ValeSalidaCLS objVale;
                            lista = new List<ValeSalidaCLS>();
                            int postFechaEmisionStr = dr.GetOrdinal("fecha_emision_str");
                            int postDiaSemana = dr.GetOrdinal("dia_semana");
                            int postRuta = dr.GetOrdinal("ruta");
                            int postNombreVendedor = dr.GetOrdinal("nombre_vendedor");
                            int postNumeroVale = dr.GetOrdinal("numero_vale");
                            int postLinea = dr.GetOrdinal("linea");
                            int postCodigoInventario = dr.GetOrdinal("codigo_inventario");
                            int postDescripcion = dr.GetOrdinal("descripcion");
                            int postPrecioUnitario = dr.GetOrdinal("precio_unitario");
                            int postCantidad = dr.GetOrdinal("cantidad");

                            while (dr.Read())
                            {
                                objVale = new ValeSalidaCLS();
                                objVale.FechaEmisionStr = dr.GetString(postFechaEmisionStr);
                                objVale.DiaSemana = dr.GetString(postDiaSemana);
                                objVale.Ruta = dr.GetInt32(postRuta);
                                objVale.NombreVendedor = dr.GetString(postNombreVendedor);
                                objVale.NumeroVale = dr.GetInt32(postNumeroVale);
                                objVale.NumeroLinea = dr.GetInt32(postLinea);
                                objVale.CodigoInventario = dr.GetString(postCodigoInventario);
                                objVale.Descripcion = dr.GetString(postDescripcion);
                                objVale.PrecioUnitario = dr.GetDecimal(postPrecioUnitario);
                                objVale.Cantidad = dr.GetInt32(postCantidad);
                                lista.Add(objVale);
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
