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
    public class EntidadProveedorDAL: CadenaConexion
    {
        public List<EntidadProveedorCLS> GetProveedores(int codigoEntidad)
        {
            List<EntidadProveedorCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    SELECT codigo_proveedor, 
	                       nombre 
                    FROM db_tesoreria.entidad_proveedor
                    WHERE codigo_entidad = @CodigoEntidad
                    AND estado = @CodigoEstado
                    ORDER BY nombre ASC";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEntidad", codigoEntidad);
                        cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.EstadoRegistro.ACTIVO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            EntidadProveedorCLS objProveedor;
                            lista = new List<EntidadProveedorCLS>();
                            int postCodigoProveedor = dr.GetOrdinal("codigo_proveedor");
                            int postNombreProveedor = dr.GetOrdinal("nombre");
                            while (dr.Read())
                            {
                                objProveedor = new EntidadProveedorCLS();
                                objProveedor.CodigoProveedor = dr.GetInt32(postCodigoProveedor);
                                objProveedor.NombreProveedor = dr.GetString(postNombreProveedor);
                                lista.Add(objProveedor);
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
