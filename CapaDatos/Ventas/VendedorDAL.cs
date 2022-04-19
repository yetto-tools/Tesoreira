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
    public class VendedorDAL:CadenaConexion
    {
        public List<VendedorCLS> GetVendedores()
        {
            List<VendedorCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaVentas))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT codigo_vendedor, 
                           nombre_completo 
                    FROM db_ventas.vendedor
                    WHERE estado = @CodigoEstadoActivo
                    ORDER BY nombre_completo ASC";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoActivo", Constantes.EstadoRegistro.ACTIVO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            VendedorCLS objVendedor;
                            lista = new List<VendedorCLS>();
                            int postCodigoVendedor = dr.GetOrdinal("codigo_vendedor");
                            int postNombreVendedor = dr.GetOrdinal("nombre_completo");
                            while (dr.Read())
                            {
                                objVendedor = new VendedorCLS();
                                objVendedor.CodigoVendedor = dr.GetString(postCodigoVendedor);
                                objVendedor.NombreVendedor = dr.GetString(postNombreVendedor);
                                lista.Add(objVendedor);
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

        public string GuardarVendedor(VendedorRutaCLS objVendedor, string usuarioIng)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaVentas))
            {
                conexion.Open();

                SqlCommand cmd = conexion.CreateCommand();

                try
                {
                    string sentenciaInsert = @"
                    INSERT INTO db_ventas.vendedor(codigo_vendedor, cui, codigo_empresa, codigo_empleado, nombre_completo, descripcion, estado, usuario_ing, fecha_ing)
                    VALUES(@codigoVendedor,@Cui,@CodigoEmpresa,@CodigoEmpleado,@NombreCompleto,@Descripcion,@CodigoEstado,@UsuarioIng,@FechaIng)";

                    cmd.CommandText = sentenciaInsert;
                    cmd.Parameters.AddWithValue("@codigoVendedor", objVendedor.CodigoVendedor);
                    cmd.Parameters.AddWithValue("@Cui", DBNull.Value);
                    cmd.Parameters.AddWithValue("@CodigoEmpresa", DBNull.Value);
                    cmd.Parameters.AddWithValue("@CodigoEmpleado", DBNull.Value);
                    cmd.Parameters.AddWithValue("@NombreCompleto", objVendedor.NombreVendedor);
                    cmd.Parameters.AddWithValue("@Descripcion", DBNull.Value);
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


    }
}
