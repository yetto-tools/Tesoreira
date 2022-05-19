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
    public class CuentaBancariaDAL: CadenaConexion
    {

        public List<CuentaBancariaCLS> GetCuentasBancarias(int codigoBanco)
        {
            List<CuentaBancariaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT numero_cuenta,
                           CONCAT(numero_cuenta,' (',nombre_cuenta,')') AS numero_cuenta_descriptivo
                    FROM db_admon.cuenta_bancaria
                    WHERE codigo_banco = @CodigoBanco
                    ORDER BY numero_cuenta DESC";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoBanco", codigoBanco);
                        SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleResult);
                        if (dr != null)
                        {
                            CuentaBancariaCLS objCuentaBancaria;
                            lista = new List<CuentaBancariaCLS>();
                            int postNumeroCuenta = dr.GetOrdinal("numero_cuenta");
                            int postNumeroCuentaDescriptivo = dr.GetOrdinal("numero_cuenta_descriptivo");
                            while (dr.Read())
                            {
                                objCuentaBancaria = new CuentaBancariaCLS();
                                objCuentaBancaria.NumeroCuenta = dr.GetString(postNumeroCuenta);
                                objCuentaBancaria.NumeroCuentaDescriptivo = dr.GetString(postNumeroCuentaDescriptivo);
                                lista.Add(objCuentaBancaria);
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

        public List<CuentaBancariaCLS> GetCuentasBancariasTesoreria(int codigoBanco)
        {
            List<CuentaBancariaCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    conexion.Open();
                    string sql = @"
                    SELECT numero_cuenta,
                           CONCAT(numero_cuenta,' (',nombre_cuenta,')') AS numero_cuenta_descriptivo
                    FROM db_admon.cuenta_bancaria
                    WHERE codigo_banco = @CodigoBanco
                    AND tesoreria = 1
                    ORDER BY numero_cuenta DESC";

                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoBanco", codigoBanco);
                        SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleResult);
                        if (dr != null)
                        {
                            CuentaBancariaCLS objCuentaBancaria;
                            lista = new List<CuentaBancariaCLS>();
                            int postNumeroCuenta = dr.GetOrdinal("numero_cuenta");
                            int postNumeroCuentaDescriptivo = dr.GetOrdinal("numero_cuenta_descriptivo");
                            while (dr.Read())
                            {
                                objCuentaBancaria = new CuentaBancariaCLS();
                                objCuentaBancaria.NumeroCuenta = dr.GetString(postNumeroCuenta);
                                objCuentaBancaria.NumeroCuentaDescriptivo = dr.GetString(postNumeroCuentaDescriptivo);
                                lista.Add(objCuentaBancaria);
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

        public string GetComboCuentasBancarias(int codigoBanco)
        {
            string resultado  = "";
            using (SqlConnection conexion = new SqlConnection(cadenaAdmon))
            {
                try
                {
                    string sql = "SELECT db_admon.GetComboCuentas(@CodigoBanco)";
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoBanco", codigoBanco);
                        resultado = (string)cmd.ExecuteScalar();

                    }
                    conexion.Close();
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    resultado = "<select name=NumeroCuenta id=uiNumeroCuenta class=select-cuenta-bancaria><option value=-1>-- Error--</option></select>";
                }

                return resultado;
            }
        }


    }
}
