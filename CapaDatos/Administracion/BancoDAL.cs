using CapaEntidad.Administracion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Administracion
{
    public class BancoDAL: CadenaConexion
    {
        public List<BancoCLS> GetAllBancos()
        {
            List<BancoCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT codigo_banco, 
	                       nombre
                    FROM db_admon.banco
                    WHERE estado = @CodigoEstado
                    ORDER BY nombre";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.EstadoRegistro.ACTIVO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            BancoCLS objBanco;
                            lista = new List<BancoCLS>();
                            int postCodigoBanco = dr.GetOrdinal("codigo_banco");
                            int postNombreBanco = dr.GetOrdinal("nombre");
                            while (dr.Read())
                            {
                                objBanco = new BancoCLS();
                                objBanco.CodigoBanco = dr.GetInt16(postCodigoBanco);
                                objBanco.Nombre = dr.GetString(postNombreBanco);
                                lista.Add(objBanco);
                            }
                        }
                    }
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
