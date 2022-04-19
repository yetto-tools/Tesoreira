using CapaEntidad.Ventas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Ventas
{
    public class CanalVentaDAL: CadenaConexion
    {
        public List<CanalVentaCLS> GetCanalesDeVentas()
        {
            List<CanalVentaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaVentas))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT codigo_canal_venta, 
                           nombre 
                    FROM db_ventas.canal_venta 
                    WHERE estado = @CodigoEstado
                    AND codigo_categoria_entidad <> @CodigoCategoriaEntidad";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.EstadoRegistro.ACTIVO);
                        cmd.Parameters.AddWithValue("@CodigoCategoriaEntidad", 0);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            CanalVentaCLS objCanalVenta;
                            lista = new List<CanalVentaCLS>();
                            int postCodigoCanalVenta = dr.GetOrdinal("codigo_canal_venta");
                            int postCanalVenta = dr.GetOrdinal("nombre");

                            while (dr.Read())
                            {
                                objCanalVenta = new CanalVentaCLS();
                                objCanalVenta.CodigoCanalVenta = dr.GetInt16(postCodigoCanalVenta);
                                objCanalVenta.CanalVenta = dr.GetString(postCanalVenta);
                                lista.Add(objCanalVenta);
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

        public List<RutaCLS> GetCanalVenta(int ruta)
        {
            List<RutaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaVentas))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT x.ruta, 
	                       x.codigo_canal_venta,
	                       y.nombre AS canal_venta
                    FROM db_ventas.ruta x
                    INNER JOIN db_ventas.canal_venta y
                    ON x.codigo_canal_venta = y.codigo_canal_venta
                    WHERE x.estado <> @CodigoEstadoAnulado
                      AND x.ruta = @Ruta
                    ORDER BY x.ruta";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoAnulado", Constantes.EstadoRegistro.BLOQUEADO);
                        cmd.Parameters.AddWithValue("@Ruta", ruta);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            RutaCLS objRuta;
                            lista = new List<RutaCLS>();
                            int postRuta = dr.GetOrdinal("ruta");
                            int postCodigoCanalVenta = dr.GetOrdinal("codigo_canal_venta");
                            int postCanalVenta = dr.GetOrdinal("canal_venta");
                            while (dr.Read())
                            {
                                objRuta = new RutaCLS();
                                objRuta.Ruta = dr.GetInt16(postRuta);
                                objRuta.CodigoCanalVenta = dr.GetInt16(postCodigoCanalVenta);
                                objRuta.CanalVenta = dr.GetString(postCanalVenta);
                                lista.Add(objRuta);
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
