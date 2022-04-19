using CapaEntidad.Tesoreria;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Tesoreria
{
    public class OtroIngresoDAL: CadenaConexion
    {
        public List<OtroIngresoCLS> GetListaOtrosIngresos()
        {
            List<OtroIngresoCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    SELECT codigo_otro_ingreso,
		                   nombre
                    FROM db_tesoreria.otros_ingresos
                    WHERE estado = @CodigoEstadoActivo
                      AND codigo_otro_ingreso <> 0
                    ORDER BY nombre ASC";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoActivo", Constantes.EstadoRegistro.ACTIVO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            OtroIngresoCLS objOtroIngreso;
                            lista = new List<OtroIngresoCLS>();
                            int postCodigoOtroIngreso = dr.GetOrdinal("codigo_otro_ingreso");
                            int postNombre = dr.GetOrdinal("nombre");
                            
                            while (dr.Read())
                            {
                                objOtroIngreso = new OtroIngresoCLS();
                                objOtroIngreso.CodigoOtroIngreso = dr.GetInt16(postCodigoOtroIngreso);
                                objOtroIngreso.Nombre = dr.GetString(postNombre);
                                lista.Add(objOtroIngreso);
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
