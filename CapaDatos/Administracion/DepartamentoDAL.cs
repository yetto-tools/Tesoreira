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
    public class DepartamentoDAL: CadenaConexion
    {
        public List<DepartamentoCLS> GetAllDepartamentos()
        {
            List<DepartamentoCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT codigo_departamento, 
	                       nombre 
                    FROM db_admon.departamento 
                    WHERE estado = 1
                    ORDER BY nombre ASC";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            DepartamentoCLS objDepartamento;
                            lista = new List<DepartamentoCLS>();
                            int postCodigoDepartamento = dr.GetOrdinal("codigo_departamento");
                            int postNombreDepartamento = dr.GetOrdinal("nombre");

                            while (dr.Read())
                            {
                                objDepartamento = new DepartamentoCLS();
                                objDepartamento.CodigoDepartamento = dr.GetInt16(postCodigoDepartamento);
                                objDepartamento.Nombre = dr.GetString(postNombreDepartamento);
                                
                                lista.Add(objDepartamento);
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
