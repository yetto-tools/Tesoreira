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
    public class EmpresaDAL: CadenaConexion
    {
        public List<EmpresaCLS> GetAllEmpresas()
        {
            List<EmpresaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT codigo_empresa, 
                           nombre_razon_social, 
                           nombre_comercial,
                           codigo_qsystem 
                    FROM db_admon.empresa
                    WHERE estado = @CodigoEstado";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.EstadoRegistro.ACTIVO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            EmpresaCLS objEmpresa;
                            lista = new List<EmpresaCLS>();
                            int postCodigoEmpresa = dr.GetOrdinal("codigo_empresa");
                            int postNombreEmpresa = dr.GetOrdinal("nombre_comercial");

                            while (dr.Read())
                            {
                                objEmpresa = new EmpresaCLS();
                                objEmpresa.CodigoEmpresa = dr.GetInt16(postCodigoEmpresa);
                                objEmpresa.NombreComercial = dr.GetString(postNombreEmpresa);

                                lista.Add(objEmpresa);
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

        public List<EmpresaCLS> GetEmpresasTesoreriaFacturas()
        {
            List<EmpresaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT codigo_empresa, 
                           nombre_razon_social, 
                           nombre_comercial,
                           codigo_qsystem 
                    FROM db_admon.empresa
                    WHERE estado = @CodigoEstado
                      AND tesoreria = @Activo";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.EstadoRegistro.ACTIVO);
                        cmd.Parameters.AddWithValue("@Activo", Constantes.EstadoRegistro.ACTIVO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            EmpresaCLS objEmpresa;
                            lista = new List<EmpresaCLS>();
                            int postCodigoEmpresa = dr.GetOrdinal("codigo_empresa");
                            int postNombreEmpresa = dr.GetOrdinal("nombre_comercial");

                            while (dr.Read())
                            {
                                objEmpresa = new EmpresaCLS();
                                objEmpresa.CodigoEmpresa = dr.GetInt16(postCodigoEmpresa);
                                objEmpresa.NombreComercial = dr.GetString(postNombreEmpresa);

                                lista.Add(objEmpresa);
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
