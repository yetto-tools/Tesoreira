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
    public class ContribuyenteDAL: CadenaConexion
    {
        public ContribuyenteCLS GetDataContribuyente(string nit)
        {
            ContribuyenteCLS objContribuyente = new ContribuyenteCLS();
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    string sql = "SELECT nit, nombre FROM db_tesoreria.contribuyente WHERE nit = @Nit";
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Nit", nit);
                        SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleResult);
                        if (dr != null)
                        {
                            int postNit = dr.GetOrdinal("nit");
                            int postNombreContribuyente = dr.GetOrdinal("nombre");

                            while (dr.Read())
                            {
                                objContribuyente = new ContribuyenteCLS();
                                objContribuyente.Nit = dr.GetString(postNit);
                                objContribuyente.Nombre = dr.GetString(postNombreContribuyente);
                            }
                            
                        }
                    }
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                    objContribuyente = null;
                }
            }
            return objContribuyente;
        }

        public string GuardarContribuyente(ContribuyenteCLS objContribuyente, string usuarioIng)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                conexion.Open();
                SqlCommand cmd = conexion.CreateCommand();

                try
                {
                    string sentenciaSQL = @"
                    INSERT INTO db_tesoreria.contribuyente(nit, nombre, codigo_tipo_contribuyente, descripcion, estado, usuario_ing, fecha_ing)
                    VALUES(@Nit, @NombreContribuyente, @CodigoTipoContribuyente, @Descripcion, @CodigoEstado, @UsuarioIng, @FechaIng)";

                    cmd.CommandText = sentenciaSQL;
                    cmd.Parameters.AddWithValue("@Nit", objContribuyente.Nit);
                    cmd.Parameters.AddWithValue("@NombreContribuyente", objContribuyente.Nombre.ToUpper());
                    cmd.Parameters.AddWithValue("@CodigoTipoContribuyente", Constantes.Contribuyente.TipoContribuyente.SOCIEDAD_ANONIMA);
                    cmd.Parameters.AddWithValue("@Descripcion", DBNull.Value);
                    cmd.Parameters.AddWithValue("@CodigoEstado", Constantes.EstadoRegistro.ACTIVO);
                    cmd.Parameters.AddWithValue("@UsuarioIng", usuarioIng);
                    cmd.Parameters.AddWithValue("@FechaIng", DateTime.Now);
                    cmd.ExecuteNonQuery();

                    resultado = "OK";
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    resultado = "Error [0]: " + ex.Message;
                }
            }

            return resultado;
        }

    }

}
