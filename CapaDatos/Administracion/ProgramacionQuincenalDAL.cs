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
    public class ProgramacionQuincenalDAL: CadenaConexion
    {
        public List<ProgramacionQuincenalCLS> GetListaQuincenas(int anio, int numeroMes)
        {
            List<ProgramacionQuincenalCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_admon.uspGetQuincenas", conexion))
                    {
                        // le indico que es un procedure
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Anio", anio);
                        cmd.Parameters.AddWithValue("@NumeroMes", numeroMes);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            ProgramacionQuincenalCLS objProgramacionQuincenal;
                            lista = new List<ProgramacionQuincenalCLS>();
                            int postCodigoQuincena = dr.GetOrdinal("codigo_quincena");
                            int postPeriodo = dr.GetOrdinal("periodo");
                            int postFechaInicioStr = dr.GetOrdinal("inicio");
                            int postFechaFinStr = dr.GetOrdinal("fin");


                            while (dr.Read())
                            {
                                objProgramacionQuincenal = new ProgramacionQuincenalCLS();
                                objProgramacionQuincenal.CodigoQuincenaPlanilla = dr.GetInt32(postCodigoQuincena);
                                objProgramacionQuincenal.Periodo = dr.GetString(postPeriodo);
                                objProgramacionQuincenal.FechaInicioStr = dr.GetString(postFechaInicioStr);
                                objProgramacionQuincenal.FechaFinStr = dr.GetString(postFechaFinStr);

                                lista.Add(objProgramacionQuincenal);
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
