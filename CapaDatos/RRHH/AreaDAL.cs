using CapaEntidad.RRHH;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.RRHH
{
    public class AreaDAL: CadenaConexion
    {
        public List<AreaCLS> GetAllAreas()
        {
            List<AreaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaRRHH))
            {
                try
                {
                    string sql = @"
                    SELECT codigo_area,
		                   nombre
                    FROM db_rrhh.area
                    WHERE estado <> @CodigoEstadoAnulado";
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoAnulado", Constantes.EstadoRegistro.BLOQUEADO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            AreaCLS objArea;
                            lista = new List<AreaCLS>();
                            int postCodigoArea = dr.GetOrdinal("codigo_area");
                            int postNombreArea = dr.GetOrdinal("nombre");

                            while (dr.Read())
                            {
                                objArea = new AreaCLS();
                                objArea.CodigoArea = dr.GetInt16(postCodigoArea);
                                objArea.Nombre = dr.GetString(postNombreArea);
                                lista.Add(objArea);
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
