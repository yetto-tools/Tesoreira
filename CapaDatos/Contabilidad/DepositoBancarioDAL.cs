using CapaEntidad.Contabilidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Contabilidad
{
    public class DepositoBancarioDAL: CadenaConexion
    {
        public List<DepositoBancarioCLS> GetDepositos(int anioReporte, int semanaReporte, int codigoReporte)
        {
            List<DepositoBancarioCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaContabilidad))
            {
                try
                {
                    string sql = @" 
                    SELECT m.codigo_banco_deposito,
	                       n.nombre AS nombre_banco,	
	                       m.numero_cuenta,
                           m.numero_boleta,
	                       m.monto,
	                       m.dia_operacion,
	                       CASE
		                     WHEN m.dia_operacion = 1 THEN 'LUNES'
		                     WHEN m.dia_operacion = 2 THEN 'MARTES'
		                     WHEN m.dia_operacion = 3 THEN 'MIERCOLES'
		                     WHEN m.dia_operacion = 4 THEN 'JUEVES'
		                     WHEN m.dia_operacion = 5 THEN 'VIERNES'
		                     WHEN m.dia_operacion = 6 THEN 'SABADO'
		                     WHEN m.dia_operacion = 7 THEN 'DOMINGO'
		                     ELSE 'NO DEFINIDO'
	                       END AS dia,
	                       m.codigo_origen,
	                       m.origen
                    FROM ( SELECT y.codigo_banco_deposito,
			                      y.numero_cuenta,
                                  y.numero_boleta,
			                      y.monto,
			                      y.dia_operacion,			
			                      1 AS codigo_origen,
			                      'TESORERIA' AS origen

	                       FROM db_tesoreria.transaccion y
	                       WHERE y.anio_operacion = @AnioReporte
		                     AND y.semana_operacion = @SemanaReporte
		                     AND y.codigo_reporte = @CodigoReporte
		                     AND y.codigo_estado <> 0
		                     AND y.codigo_operacion = @CodigoOperacion

                           UNION

                           SELECT y.codigo_banco_deposito,
	                              y.numero_cuenta,
	                              y.numero_boleta,
	                              y.monto,
	                              y.dia_operacion,			
	                              2 AS codigo_origen,
	                              'OTROS' AS origen
                           FROM db_contabilidad.deposito_btb y
                           WHERE y.estado = 1
                             AND y.anio_operacion = @AnioReporte
                             AND y.semana_operacion = @SemanaReporte
                             AND y.codigo_reporte = @CodigoReporte
                         ) m
                    INNER JOIN db_admon.banco n
                    ON m.codigo_banco_deposito = n.codigo_banco
                    ORDER by m.codigo_banco_deposito, m.numero_cuenta";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        // CommandType, Gets or sets a value indicating how the CommandText property is to be interpreted.
                        // The Text CommandType.Text is used when the command is raw SQL
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@AnioReporte", anioReporte);
                        cmd.Parameters.AddWithValue("@SemanaReporte", semanaReporte);
                        cmd.Parameters.AddWithValue("@CodigoReporte", codigoReporte);
                        cmd.Parameters.AddWithValue("@CodigoOperacion", Constantes.Operacion.Egreso.DEPOSITOS_BANCARIOS);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            DepositoBancarioCLS objDepositosBancarioCLS;
                            lista = new List<DepositoBancarioCLS>();
                            int postCodigoBancoDeposito = dr.GetOrdinal("codigo_banco_deposito");
                            int postNombreBanco = dr.GetOrdinal("nombre_banco");
                            int postNumeroCuenta = dr.GetOrdinal("numero_cuenta");
                            int postNumeroBoleta = dr.GetOrdinal("numero_boleta");
                            int postMonto = dr.GetOrdinal("monto");
                            int postDiaOperacion = dr.GetOrdinal("dia_operacion");
                            int postDia = dr.GetOrdinal("dia");
                            int postCodigoOrigen = dr.GetOrdinal("codigo_origen");
                            int postOrigen = dr.GetOrdinal("origen");
                            while (dr.Read())
                            {
                                objDepositosBancarioCLS = new DepositoBancarioCLS();
                                objDepositosBancarioCLS.CodigoBancoDeposito = dr.GetInt16(postCodigoBancoDeposito);
                                objDepositosBancarioCLS.NombreBanco = dr.GetString(postNombreBanco);
                                objDepositosBancarioCLS.NumeroCuenta = dr.GetString(postNumeroCuenta);
                                objDepositosBancarioCLS.NumeroBoleta = dr.GetString(postNumeroBoleta);
                                objDepositosBancarioCLS.Monto = dr.GetDecimal(postMonto);
                                objDepositosBancarioCLS.DiaOperacion = dr.GetByte(postDiaOperacion);
                                objDepositosBancarioCLS.NombreDiaOperacion = dr.GetString(postDia);
                                objDepositosBancarioCLS.CodigoOrigenDeposito = (byte)dr.GetInt32(postCodigoOrigen);
                                objDepositosBancarioCLS.OrigenDeposito = dr.GetString(postOrigen);
                                lista.Add(objDepositosBancarioCLS);
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
