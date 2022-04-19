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
    public class ProgramacionSemanalDAL: CadenaConexion
    {
        public List<ProgramacionSemanalCLS> GetAniosProgramacionSemanal()
        {
            List<ProgramacionSemanalCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    string sql = @"
                    SELECT anio 
                    FROM db_admon.programacion_semanal
                    GROUP BY anio
                    ORDER BY anio DESC";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        // le indico que es un procedure
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            ProgramacionSemanalCLS objProgramacionSemanal;
                            lista = new List<ProgramacionSemanalCLS>();
                            int postAnio = dr.GetOrdinal("anio");
                            while (dr.Read())
                            {
                                objProgramacionSemanal = new ProgramacionSemanalCLS();
                                objProgramacionSemanal.Anio = dr.GetInt16(postAnio);
                                lista.Add(objProgramacionSemanal);
                            }
                        }
                    }
                    conexion.Close();
                }
                catch (Exception)
                {
                    lista = null;
                    conexion.Close();
                }

                return lista;
            }
        }

        public List<ProgramacionSemanalCLS> GetDiasOperacion(int anio, int numeroSemana)
        {
            List<ProgramacionSemanalCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    string sql = @"
                    SELECT anio,
	                       fecha,
	                       convert(varchar, fecha, 103) as fecha_str,
	                       CASE 
			                 WHEN numero_dia = 1 then 'LUNES'
			                 WHEN numero_dia = 2 then 'ARTES'
			                 WHEN numero_dia = 3 then 'MIERCOLES'
			                 WHEN numero_dia = 4 then 'JUEVES'
			                 WHEN numero_dia = 5 then 'VIERNES'
			                 WHEN numero_dia = 6 then 'SABADO'
			                 WHEN numero_dia = 7 then 'DOMINGO'
			                ELSE 'NINGUN DIA'
	                       END AS dia,
		                   numero_semana
                    FROM db_admon.programacion_semanal
                    WHERE anio = @Anio AND 
                          numero_semana = @NumeroSemana";
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        // le indico que es un procedure
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Anio", anio);
                        cmd.Parameters.AddWithValue("@NumeroSemana", numeroSemana);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            ProgramacionSemanalCLS objProgramacionSemanal;
                            lista = new List<ProgramacionSemanalCLS>();
                            int postFechaStr = dr.GetOrdinal("fecha_str");
                            int postDia = dr.GetOrdinal("dia");
                            while (dr.Read())
                            {
                                objProgramacionSemanal = new ProgramacionSemanalCLS();
                                objProgramacionSemanal.FechaStr = dr.GetString(postFechaStr);
                                objProgramacionSemanal.Dia = dr.GetString(postDia);
                                lista.Add(objProgramacionSemanal);
                            }
                        }
                    }
                    conexion.Close();
                }
                catch (Exception)
                {
                    lista = null;
                    conexion.Close();
                }

                return lista;
            }
        }

        public ProgramacionSemanalCLS GetSemanaActual()
        {
            ProgramacionSemanalCLS objSemanaActual = new ProgramacionSemanalCLS();
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_admon.uspGetSemanaOperacion", conexion))
                    {
                        // le indico que es un procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            int postNumeroSemana = dr.GetOrdinal("numero_semana");
                            int postFechaInicioSemana = dr.GetOrdinal("fecha_inicio_semana");
                            int postFechaFinSemana = dr.GetOrdinal("fecha_fin_semana");
                            int postFechaSistema = dr.GetOrdinal("fecha_sistema");
                            int postAnio = dr.GetOrdinal("anio");

                            while (dr.Read())
                            {
                                objSemanaActual = new ProgramacionSemanalCLS();
                                objSemanaActual.FechaSistema = dr.GetString(postFechaSistema);
                                objSemanaActual.NumeroSemana = dr.GetByte(postNumeroSemana);
                                objSemanaActual.Periodo = dr.GetString(postFechaInicioSemana) + "-" + dr.GetString(postFechaFinSemana);
                                objSemanaActual.Anio = dr.GetInt16(postAnio);
                            }
                        }
                    }
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                }

                return objSemanaActual;
            }
        }

        public List<ProgramacionSemanalCLS> GetSemanasAnteriores(int anio, int numeroSemanaActual, int numeroSemanas, int incluirSemanaActual)
        {
            List<ProgramacionSemanalCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_admon.uspGetSemanaAnterior", conexion))
                    {
                        // le indico que es un procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Anio", anio);
                        cmd.Parameters.AddWithValue("@NumeroSemanaActual", numeroSemanaActual);
                        cmd.Parameters.AddWithValue("@NumeroSemanas", numeroSemanas);
                        cmd.Parameters.AddWithValue("@IncluirSemanaActual", incluirSemanaActual);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            ProgramacionSemanalCLS objProgramacionSemanal;
                            lista = new List<ProgramacionSemanalCLS>();
                            int postNumeroSemana = dr.GetOrdinal("numero_semana");
                            int postFechaInicioSemana = dr.GetOrdinal("fecha_inicio_semana");
                            int postFechaFinSemana = dr.GetOrdinal("fecha_fin_semana");
                            int postFechaSistema = dr.GetOrdinal("fecha_sistema");

                            while (dr.Read())
                            {
                                objProgramacionSemanal = new ProgramacionSemanalCLS();
                                objProgramacionSemanal.NumeroSemana = dr.GetByte(postNumeroSemana);
                                objProgramacionSemanal.Periodo = dr.GetString(postFechaInicioSemana) + "-" + dr.GetString(postFechaFinSemana) + "[" + dr.GetByte(postNumeroSemana).ToString() + "]";
                                lista.Add(objProgramacionSemanal);
                            }
                        }
                    }
                    conexion.Close();
                }
                catch (Exception)
                {
                    lista = null;
                    conexion.Close();
                }

                return lista;
            }
        }

        public List<ProgramacionSemanalCLS> GetListaSemanasComision(int anio, int numeroSemana, int ultimaSemanaAnioAnterior)
        {
            List<ProgramacionSemanalCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_admon.uspGetSemanasComision", conexion))
                    {
                        // le indico que es un procedure
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@anio", anio);
                        cmd.Parameters.AddWithValue("@numeroSemana", numeroSemana);
                        cmd.Parameters.AddWithValue("@ultimaSemanaAnioAnterior", ultimaSemanaAnioAnterior);

                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            ProgramacionSemanalCLS objProgramacionSemanal;
                            lista = new List<ProgramacionSemanalCLS>();
                            int postNumeroSemanaComision = dr.GetOrdinal("numero_semana");
                            int postFechaInicioSemana = dr.GetOrdinal("fecha_inicio_semana");
                            int postFechaFinSemana = dr.GetOrdinal("fecha_fin_semana");

                            while (dr.Read())
                            {
                                objProgramacionSemanal = new ProgramacionSemanalCLS();
                                objProgramacionSemanal.SemanaComision = dr.GetByte(postNumeroSemanaComision);
                                objProgramacionSemanal.Periodo = dr.GetString(postFechaInicioSemana) + "-" + dr.GetString(postFechaFinSemana) + "[" + dr.GetByte(postNumeroSemanaComision).ToString() + "]";
                                lista.Add(objProgramacionSemanal);
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

        public List<ProgramacionSemanalCLS> GetListaSemanasPlanilla(int anio, int numeroSemana)
        {
            List<ProgramacionSemanalCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_admon.uspGetSemanasPlanillaPago", conexion))
                    {
                        // le indico que es un procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@anio", anio);
                        cmd.Parameters.AddWithValue("@numeroSemana", numeroSemana);

                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            ProgramacionSemanalCLS objProgramacionSemanal;
                            lista = new List<ProgramacionSemanalCLS>();
                            int postNumeroSemana = dr.GetOrdinal("numero_semana");
                            int postFechaInicioSemana = dr.GetOrdinal("fecha_inicio_semana");
                            int postFechaFinSemana = dr.GetOrdinal("fecha_fin_semana");

                            while (dr.Read())
                            {
                                objProgramacionSemanal = new ProgramacionSemanalCLS();
                                objProgramacionSemanal.NumeroSemana = dr.GetByte(postNumeroSemana);
                                objProgramacionSemanal.Periodo = dr.GetString(postFechaInicioSemana) + "-" + (dr.IsDBNull(postFechaFinSemana) ? "" : dr.GetString(postFechaFinSemana)) + "[" + dr.GetByte(postNumeroSemana).ToString() + "]";
                                lista.Add(objProgramacionSemanal);
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
