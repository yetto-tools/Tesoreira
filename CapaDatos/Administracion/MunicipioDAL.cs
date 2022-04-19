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
    public class MunicipioDAL : CadenaConexion
    {
        public List<MunicipioCLS> GetAllMunicipios(Int32 codigoDepartamento)
        {
            List<MunicipioCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT codigo_municipio, 
	                       nombre 
                    FROM db_admon.municipio
                    WHERE estado = @CodigoEstado
                    AND codigo_departamento = @CodigoDepartamento
                    ORDER BY nombre ASC";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.EstadoRegistro.ACTIVO);
                        cmd.Parameters.AddWithValue("@CodigoDepartamento", codigoDepartamento);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            MunicipioCLS objMunicipio;
                            lista = new List<MunicipioCLS>();
                            int postCodigoMunicipio = dr.GetOrdinal("codigo_municipio");
                            int postNombreMunicipio = dr.GetOrdinal("nombre");

                            while (dr.Read())
                            {
                                objMunicipio = new MunicipioCLS();
                                objMunicipio.CodigoMunicipio = dr.GetInt16(postCodigoMunicipio);
                                objMunicipio.Nombre = dr.GetString(postNombreMunicipio);

                                lista.Add(objMunicipio);
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
