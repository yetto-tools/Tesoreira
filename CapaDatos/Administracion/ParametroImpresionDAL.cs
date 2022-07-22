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
    public class ParametroImpresionDAL: CadenaConexion
    {
        public List<ParametroImpresionCLS> GetAllConfiguracionesImpresion()
        {
            List<ParametroImpresionCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT codigo_configuracion,
	                       nombre_impresora, 
	                       numero_copias,
                           ip,
                           puerto 
                    FROM db_admon.parametro_impresion
                    WHERE estado = @CodigoEstado";
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.EstadoRegistro.ACTIVO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            ParametroImpresionCLS objParametro;
                            lista = new List<ParametroImpresionCLS>();
                            int postCodigoConfiguracion = dr.GetOrdinal("codigo_configuracion");
                            int postNombreImpresora = dr.GetOrdinal("nombre_impresora");
                            int postNumeroCopias = dr.GetOrdinal("numero_copias");
                            int postIp = dr.GetOrdinal("ip");
                            int postPuerto = dr.GetOrdinal("puerto");
                            while (dr.Read())
                            {
                                objParametro = new ParametroImpresionCLS();
                                objParametro.CodigoConfiguracion = dr.GetInt16(postCodigoConfiguracion);
                                objParametro.NombreImpresora = dr.GetString(postNombreImpresora);
                                objParametro.NumeroCopias = dr.GetInt16(postNumeroCopias);
                                objParametro.Ip = dr.IsDBNull(postIp) ? "" : dr.GetString(postIp);
                                objParametro.Puerto = dr.GetInt32(postPuerto);
                                lista.Add(objParametro);
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
