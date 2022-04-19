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
    public class OperacionDAL: CadenaConexion
    {
        public OperacionComboCLS GetListOperaciones(int codigoTipoOperacion)
        {
            OperacionComboCLS objOperacionComboCLS = new OperacionComboCLS();
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    SELECT x.codigo_operacion, 
	                       x.nombre_operacion AS nombre,
                           x.nombre_reporte_caja 
                    FROM db_tesoreria.operacion x
                    INNER JOIN db_tesoreria.tipo_operacion y
                    ON x.codigo_tipo_operacion = y.codigo_tipo_operacion
                    WHERE x.estado = @EstadoRegistro
                      AND x.habilitar_para_caja = @Habilitar  
                      AND y.signo = @CodigoTipoOperacion
                      ORDER BY x.nombre_reporte_caja ASC";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        // CommandType, Gets or sets a value indicating how the CommandText property is to be interpreted.
                        // The Text CommandType.Text is used when the command is raw SQL
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@EstadoRegistro", Constantes.EstadoRegistro.ACTIVO);
                        cmd.Parameters.AddWithValue("@Habilitar", Constantes.EstadoRegistro.ACTIVO);
                        cmd.Parameters.AddWithValue("@CodigoTipoOperacion", codigoTipoOperacion);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            List<OperacionCLS> listaOperaciones = new List<OperacionCLS>();
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postNombre = dr.GetOrdinal("nombre");
                            int postNombreReporteCaja = dr.GetOrdinal("nombre_reporte_caja");

                            OperacionCLS objOperacionCLS;
                            while (dr.Read())
                            {
                                objOperacionCLS = new OperacionCLS();
                                objOperacionCLS.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objOperacionCLS.Nombre = dr.GetString(postNombre);
                                objOperacionCLS.NombreReporteCaja = dr.GetString(postNombreReporteCaja);
                                listaOperaciones.Add(objOperacionCLS);
                            }//fin while
                            objOperacionComboCLS.listaOperaciones = listaOperaciones;
                        }// fin if
                    }// fin using
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                    objOperacionComboCLS = null;

                }
                return objOperacionComboCLS;
            }
        }

        public List<OperacionCLS> GetOperacionesParaAsignacionAEntidadesGenericas()
        {
            List<OperacionCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    SELECT codigo_operacion, 
	                       nombre_operacion AS nombre,
                           nombre_reporte_caja 
                    FROM db_tesoreria.operacion
                    WHERE config_entidad_generica = @CodigoEstadoActivo
                    ORDER BY nombre_operacion";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        // CommandType, Gets or sets a value indicating how the CommandText property is to be interpreted.
                        // The Text CommandType.Text is used when the command is raw SQL
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoEstadoActivo", Constantes.EstadoRegistro.ACTIVO);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            lista = new List<OperacionCLS>();
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postNombre = dr.GetOrdinal("nombre");
                            int postNombreReporteCaja = dr.GetOrdinal("nombre_reporte_caja");

                            OperacionCLS objOperacionCLS;
                            while (dr.Read())
                            {
                                objOperacionCLS = new OperacionCLS();
                                objOperacionCLS.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objOperacionCLS.Nombre = dr.GetString(postNombre);
                                objOperacionCLS.NombreReporteCaja = dr.GetString(postNombreReporteCaja);
                                lista.Add(objOperacionCLS);
                            }//fin while
                        }// fin if
                    }// fin using
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                    lista  = null;

                }
                return lista;
            }
        }

        public OperacionComboCLS GetAllOperacionesConfiguracion()
        {
            OperacionComboCLS objOperacionComboCLS = new OperacionComboCLS();
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    SELECT x.codigo_operacion, 
	                       x.codigo_categoria_operacion,
	                       x.nombre_operacion,
	                       x.nombre_reporte_caja,
	                       x.descripcion,
	                       x.habilitar_para_caja,
	                       CASE 
	                          WHEN x.habilitar_para_caja = 0 THEN 'NO'
		                      WHEN x.habilitar_para_caja = 1 THEN 'SI'
		                      ELSE ''
	                       END AS habilitar_para_caja,
	                       x.estado AS codigo_estado,
	                       CASE 
		                      WHEN x.estado = 0 THEN 'BLOQUEADO'
		                      WHEN x.estado = 1 THEN 'ACTIVO'
		                      ELSE 'NO DEFINIDO'
	                       END AS estado,
	                       x.codigo_concepto,
	                       w.nombre AS concepto,
	                       CASE
		                     WHEN x.aplica_caja_chica = 0 THEN 'NO'
		                     WHEN x.aplica_caja_chica = 1 THEN 'SI'
		                     ELSE ''
	                       END AS aplica_caja_chica,
	                       x.config_entidad_generica,
	                       CASE
	                         WHEN x.config_entidad_generica = 0 THEN 'NO INCLUIR'
		                     WHEN x.config_entidad_generica = 1 THEN 'SI'
		                     ELSE 'NO DEFINIDO'
	                       END AS incluir_en_configuracion_entidad_generica,
	                       x.codigo_tipo_operacion,
	                       q.nombre AS tipo_operacion
                    FROM db_tesoreria.operacion x
                    INNER JOIN db_tesoreria.tipo_operacion y
                    ON x.codigo_tipo_operacion = y.codigo_tipo_operacion
                    INNER JOIN db_tesoreria.categoria_operacion z
                    ON x.codigo_categoria_operacion = z.codigo_categoria_operacion
                    LEFT JOIN db_tesoreria.concepto w
                    ON x.codigo_concepto = w.codigo_concepto
                    ORDER BY x.nombre_operacion ASC";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        // CommandType, Gets or sets a value indicating how the CommandText property is to be interpreted.
                        // The Text CommandType.Text is used when the command is raw SQL
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            List<OperacionCLS> listaOperaciones = new List<OperacionCLS>();
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postNombre = dr.GetOrdinal("nombre");
                            int postNombreReporteCaja = dr.GetOrdinal("nombre_reporte_caja");

                            OperacionCLS objOperacionCLS;
                            while (dr.Read())
                            {
                                objOperacionCLS = new OperacionCLS();
                                objOperacionCLS.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objOperacionCLS.Nombre = dr.GetString(postNombre);
                                objOperacionCLS.NombreReporteCaja = dr.GetString(postNombreReporteCaja);
                                listaOperaciones.Add(objOperacionCLS);
                            }//fin while
                            objOperacionComboCLS.listaOperaciones = listaOperaciones;
                        }// fin if
                    }// fin using
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                    objOperacionComboCLS = null;

                }
                return objOperacionComboCLS;
            }
        }

        public List<OperacionCLS> GetListOperacionesCajaChica()
        {
            List<OperacionCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    SELECT codigo_operacion, 
	                       nombre_operacion
                    FROM db_tesoreria.operacion 
                    WHERE aplica_caja_chica = 1";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        // CommandType, Gets or sets a value indicating how the CommandText property is to be interpreted.
                        // The Text CommandType.Text is used when the command is raw SQL
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            OperacionCLS objOperacionCLS;
                            lista = new List<OperacionCLS>();
                            int postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            int postNombre = dr.GetOrdinal("nombre_operacion");
                            while (dr.Read())
                            {
                                objOperacionCLS = new OperacionCLS();
                                objOperacionCLS.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objOperacionCLS.Nombre = dr.GetString(postNombre);
                                lista.Add(objOperacionCLS);
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
