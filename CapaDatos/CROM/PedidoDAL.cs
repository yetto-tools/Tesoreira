using CapaEntidad.CROM;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.CROM
{
    public class PedidoDAL: CadenaConexion
    {
        public List<PedidoCLS> GetListaPedidos()
        {
            List<PedidoCLS> lista = null;
            MySqlConnection conn = null;

            try
            {
                var sb = new MySqlConnectionStringBuilder
                {
                    Server = "10.34.1.7",
                    UserID = "root",
                    Password = "pancompany",
                    Port = 3306,
                    Database = "panificadora"
                };

                conn = new MySqlConnection(sb.ConnectionString);
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT codigo_cliente, nombre_cliente FROM INF_TRASLADO_DETALLE_ESPECIALES2";
                var reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (reader.Read())
                {

                    //Console.WriteLine("id={0}, value={1}", reader.GetInt32("id"), reader.GetString("value"));
                }
            }
            catch (MySqlException ex)
            {
                //Console.Write(ex.Message);
                lista = null;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }

            return lista;
        }

    }




}
