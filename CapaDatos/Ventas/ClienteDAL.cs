using CapaEntidad.Tesoreria;
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
                    INSERT INTO db_ventas.cliente(codigo_cliente, nombre_completo, nombre_corto, codigo_tipo_cliente, descripcion, estado, usuario_ing, fecha_ing, codigo_cliente_origen)
                    VALUES(@CodigoCliente,@NombreCompleto,@NombreCorto,@CodigoTipoCliente,@Descripcion,@CodigoEstado, @UsuarioIng, @FechaIng, @CodigoClienteOrigen)";

                    cmd.CommandText = sentenciaSQL;
                    string codigoCliente = "9" + codigoSecuencia.ToString("D6");

                    cmd.Parameters.AddWithValue("@CodigoCliente", codigoCliente);
                    cmd.Parameters.AddWithValue("@NombreCompleto", objEntidad.NombreEntidad);
                    cmd.Parameters.AddWithValue("@NombreCorto", objEntidad.NombreEntidad);
                    switch (objEntidad.CodigoCategoriaEntidad)
                    {
                        case Constantes.Entidad.Categoria.CLIENTES_ESPECIALES_1:
                            cmd.Parameters.AddWithValue("@CodigoTipoCliente", Constantes.Cliente.Tipo.ESPECIALES_1);
                            cmd.Parameters.AddWithValue("@CodigoClienteOrigen", objEntidad.CodigoEntidad);
                            break;
                        case Constantes.Entidad.Categoria.CLIENTES_ESPECIALES_2:
                            cmd.Parameters.AddWithValue("@CodigoTipoCliente", Constantes.Cliente.Tipo.ESPECIALES_2);
                            cmd.Parameters.AddWithValue("@CodigoClienteOrigen", DBNull.Value);
                            break;
                        default:
                            cmd.Parameters.AddWithValue("@CodigoTipoCliente", 0);
                            cmd.Parameters.AddWithValue("@CodigoClienteOrigen", DBNull.Value);
                            break;
                    }
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

        public List<ClienteCLS> GetListAllClientes()
        {
            List<ClienteCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaQSystems))
            {
                try
                {
                    String sql = @"
                    SELECT cli_codigo AS codigo_cliente, 
	                       cli_nombre AS nombre_cliente
                    FROM MASTCLI 
                    WHERE cli_empresa = 'PSA'
                      AND cli_vendedor NOT IN (0,1,4,332) AND cli_codigo NOT IN ('000001')
                      AND cli_codigo NOT IN(SELECT codigo_cliente_origen FROM SysTesoreria.db_ventas.cliente WHERE codigo_tipo_cliente = 2)";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoTipoCliente", Constantes.Cliente.Tipo.ESPECIALES_1);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            ClienteCLS objCliente;
                            lista = new List<ClienteCLS>();
                            int postCodigoCliente = dr.GetOrdinal("codigo_cliente");
                            int postNombreCliente = dr.GetOrdinal("nombre_cliente");
                            while (dr.Read())
                            {
                                objCliente = new ClienteCLS();
                                objCliente.CodigoCliente = dr.GetString(postCodigoCliente);
                                objCliente.NombreCompleto = dr.GetString(postNombreCliente);
                                lista.Add(objCliente);
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
