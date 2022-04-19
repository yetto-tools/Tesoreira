using CapaEntidad.Tesoreria;
using CapaEntidad.Ventas;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Ventas
{
    public class ClienteDAL: CadenaConexion
    {
        public string GuardarCliente(EntidadGenericaCLS objEntidad, string usuarioIng)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaVentas))
            {
                conexion.Open();
                SqlCommand cmd = conexion.CreateCommand();

                try
                {
                    string sqlSequence = "SELECT NEXT VALUE FOR db_ventas.SQ_CLIENTE";
                    cmd.CommandText = sqlSequence;
                    long codigoSecuencia = (long)cmd.ExecuteScalar();

                    string sentenciaSQL = @"
                    INSERT INTO db_ventas.cliente(codigo_cliente, nombre_completo, nombre_corto, codigo_tipo_cliente, descripcion, estado, usuario_ing, fecha_ing)
                    VALUES(@CodigoCliente,@NombreCompleto,@NombreCorto,@CodigoTipoCliente,@Descripcion,@CodigoEstado, @UsuarioIng, @FechaIng)";

                    cmd.CommandText = sentenciaSQL;
                    string codigoCliente = "9" + codigoSecuencia.ToString("D6");

                    cmd.Parameters.AddWithValue("@CodigoCliente", codigoCliente);
                    cmd.Parameters.AddWithValue("@NombreCompleto", objEntidad.NombreEntidad);
                    cmd.Parameters.AddWithValue("@NombreCorto", objEntidad.NombreEntidad);
                    cmd.Parameters.AddWithValue("@CodigoTipoCliente", Constantes.Cliente.Tipo.ESPECIALES_2);
                    cmd.Parameters.AddWithValue("@Descripcion", objEntidad.Descripcion == null ? DBNull.Value : objEntidad.Descripcion);
                    cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.Empleado.EstadoEmpleado.ACTIVO);
                    cmd.Parameters.AddWithValue("@UsuarioIng", usuarioIng);
                    cmd.Parameters.AddWithValue("@FechaIng", DateTime.Now);
                    cmd.ExecuteNonQuery();

                    resultado = codigoCliente.ToString();
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    resultado = "Error [0]: " + ex.Message;
                }
            }

            return resultado;
        }

        //public List<ClienteCLS> buscarCliente()
        //{
        //    List<ClienteCLS> lista = null;
        //    //using (SqlConnection conexion = new SqlConnection(cadenaQSystems))
        //    using (OdbcConnection conexion = new OdbcConnection(cadenaQSystems))
        //    {
        //        try
        //        {
        //            conexion.Open();
        //            String sql = @"
        //            SELECT x.CLI_EMPRESA AS CODIGO_EMPRESA, 
        //                   x.CLI_CODIGO AS CODIGO_CLIENTE,
        //                   x.CLI_NOMBRE AS NOMBRE_CLIENTE,
        //                   x.CLI_NIT AS NIT,
        //                   x.CLI_VENDEDOR AS CODIGO_VENDEDOR
        //            FROM MASTCLI x
        //            LEFT JOIN CLIENTES_EXTRA y
        //            ON x.CLI_EMPRESA = y.EXT_EMPRESA AND x.CLI_CODIGO = y.EXT_CLIENTE
        //            WHERE x.CLI_EMPRESA = 'PAN'  AND x.CLI_CODIGO = '13608'";

        //            OdbcCommand command = new OdbcCommand(sql, conexion);
        //            try
        //            {
        //                conexion.Open();
        //                OdbcDataReader dr = command.ExecuteReader();
        //                //SqlDataReader dr = cmd.ExecuteReader();
        //                if (dr != null)
        //                {
        //                    ClienteCLS objCliente;
        //                    lista = new List<ClienteCLS>();
        //                    int postCodigoEmpresa = dr.GetOrdinal("codigo_empresa");
        //                    int postCodigoCliente = dr.GetOrdinal("codigo_cliente");
        //                    int postNombre = dr.GetOrdinal("nombre_cliente");
        //                    int postNit = dr.GetOrdinal("nit");
        //                    int postCodigoVendedor = dr.GetOrdinal("codigo_vendedor");

        //                    while (dr.Read())
        //                    {
        //                        objCliente = new ClienteCLS();
        //                        objCliente.codigoEmpresa = dr.GetString(postCodigoEmpresa);
        //                        objCliente.codigo = dr.GetString(postCodigoCliente);
        //                        objCliente.nombre = dr.GetString(postNombre);
        //                        objCliente.nit = dr.IsDBNull(postNit) ? "" : dr.GetString(postNit);
        //                        objCliente.codigoVendedor = dr.IsDBNull(postCodigoVendedor) ? 0 : dr.GetInt32(postCodigoVendedor);
        //                        lista.Add(objCliente);
        //                    }
        //                }

        //                command.ExecuteNonQuery();
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex.Message);
        //            }

        //            /*using (SqlCommand cmd = new SqlCommand(sql, conexion))
        //            {
        //                cmd.CommandType = CommandType.Text;
        //                SqlDataReader dr = cmd.ExecuteReader();
        //                if (dr != null)
        //                {
        //                    ClienteCLS objCliente;
        //                    lista = new List<ClienteCLS>();
        //                    int postCodigoEmpresa = dr.GetOrdinal("codigo_empresa");
        //                    int postCodigoCliente = dr.GetOrdinal("codigo_cliente");
        //                    int postNombre = dr.GetOrdinal("nombre_cliente");
        //                    int postNit = dr.GetOrdinal("nit");
        //                    int postCodigoVendedor = dr.GetOrdinal("codigo_vendedor");

        //                    while (dr.Read())
        //                    {
        //                        objCliente = new ClienteCLS();
        //                        objCliente.codigoEmpresa = dr.GetString(postCodigoEmpresa);
        //                        objCliente.codigo = dr.GetString(postCodigoCliente);
        //                        objCliente.nombre = dr.GetString(postNombre);
        //                        objCliente.nit = dr.IsDBNull(postNit) ? "" : dr.GetString(postNit);
        //                        objCliente.codigoVendedor = dr.IsDBNull(postCodigoVendedor) ? 0 : dr.GetInt32(postCodigoVendedor);
        //                        lista.Add(objCliente);
        //                    }
        //                }
        //            }*/
        //        }
        //        catch (Exception ex)
        //        {
        //            conexion.Close();
        //            lista = null;
        //        }

        //        return lista;
        //    }
        //}

        /*public List<ClienteCLS> listaCliente()
        {
            List<ClienteCLS> list = new List<ClienteCLS>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(cadenaCROM))
                {
                    MySqlCommand cmd = new MySqlCommand("select * from cxc_cliente where cliente = 1398", con);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                       
                    }
                }

            }
            catch (Exception ex)
            {
                return null;
            }

            return list;
        }*/


    }
}
