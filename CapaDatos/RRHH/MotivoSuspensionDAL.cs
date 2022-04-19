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
    public class MotivoSuspensionDAL: CadenaConexion
    {
        public List<MotivoSuspensionCLS> GetMotivosDeSuspension()
        {
            List<MotivoSuspensionCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaRRHH))
            {
                try
                {
                    string sql = @"
                    SELECT codigo_motivo_suspension, 
                           nombre
                    FROM db_rrhh.motivo_suspension
                    WHERE estado = 1
                    ORDER BY nombre ASC";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        // CommandType, Gets or sets a value indicating how the CommandText property is to be interpreted.
                        // The Text CommandType.Text is used when the command is raw SQL
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            MotivoSuspensionCLS objMotivoSuspensionCLS;
                            lista = new List<MotivoSuspensionCLS>();
                            int postCodigoMotivoSuspension = dr.GetOrdinal("codigo_motivo_suspension");
                            int postNombre = dr.GetOrdinal("nombre");
                            while (dr.Read())
                            {
                                objMotivoSuspensionCLS = new MotivoSuspensionCLS();
                                objMotivoSuspensionCLS.CodigoMotivoSuspension = dr.GetInt16(postCodigoMotivoSuspension);
                                objMotivoSuspensionCLS.Nombre = dr.GetString(postNombre);
                                lista.Add(objMotivoSuspensionCLS);

                            }//fin while
                        }// fin if
                    }// fin using
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
