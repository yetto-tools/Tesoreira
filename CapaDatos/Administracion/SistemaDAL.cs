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
    public class SistemaDAL: CadenaConexion
    {
        public List<SistemaCLS> GetAllSistemas()
        {
            List<SistemaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT codigo_sistema, 
                           nombre 
                    FROM db_admon.sistema
                    WHERE estado = @CodigoEstado";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.EstadoRegistro.ACTIVO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            SistemaCLS objSistema;
                            lista = new List<SistemaCLS>();
                            int postCodigoSistema = dr.GetOrdinal("codigo_sistema");
                            int postNombreSistema = dr.GetOrdinal("nombre");

                            while (dr.Read())
                            {
                                objSistema = new SistemaCLS();
                                objSistema.CodigoSistema = dr.GetInt16(postCodigoSistema);
                                objSistema.NombreSistema = dr.GetString(postNombreSistema);

                                lista.Add(objSistema);
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
