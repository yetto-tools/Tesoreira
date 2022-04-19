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
    public class MotivoBajaDAL: CadenaConexion
    {
        public List<MotivoBajaCLS> GetMotivosDeBaja()
        {
            List<MotivoBajaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaRRHH))
            {
                try
                {
                    string sql = @"
                    SELECT codigo_motivo_baja, 
                           nombre
                    FROM db_rrhh.motivo_baja
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
                            MotivoBajaCLS objMotivoBajaCLS;
                            lista = new List<MotivoBajaCLS>();
                            int postCodigoMotivoBaja = dr.GetOrdinal("codigo_motivo_baja");
                            int postNombre = dr.GetOrdinal("nombre");
                            while (dr.Read())
                            {
                                objMotivoBajaCLS = new MotivoBajaCLS();
                                objMotivoBajaCLS.CodigoMotivoBaja = dr.GetInt16(postCodigoMotivoBaja);
                                objMotivoBajaCLS.Nombre = dr.GetString(postNombre);
                                lista.Add(objMotivoBajaCLS);
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
