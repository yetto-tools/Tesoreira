using CapaEntidad.Tesoreria;
using CapaEntidad.Ventas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Tesoreria
{
    public class Especiales2DAL: CadenaConexion
    {
         public List<TrasladoEspeciales2DetalleCLS> GetTrasladosParaDepuracion(int codigoTraslado)
         {
            List<TrasladoEspeciales2DetalleCLS> listaDetalle = new List<TrasladoEspeciales2DetalleCLS>();
            List<ClienteCLS> listaClientesEspeciales2 = new List<ClienteCLS>();
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    int postCodigoCliente = 0;
                    int postNombreCliente = 0;
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_tesoreria.uspGetTrasladoEspeciales2ParaDepuracion", conexion))
                    {
                        // le indico que es un procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CodigoTraslado", codigoTraslado);
                        // Devolvera varias tablas
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr != null)
                        {
                            postCodigoCliente = dr.GetOrdinal("codigo_cliente");
                            postNombreCliente = dr.GetOrdinal("nombre_completo");

                            ClienteCLS objCliente;
                            while (dr.Read())
                            {
                                objCliente = new ClienteCLS();
                                objCliente.CodigoCliente = dr.GetString(postCodigoCliente);
                                objCliente.NombreCompleto = dr.GetString(postNombreCliente);
                                listaClientesEspeciales2.Add(objCliente);
                            }//fin while
                        }// fin if

                        if (dr.NextResult())
                        {
                            int postSerie = dr.GetOrdinal("serie");
                            int postNumeroPedido = dr.GetOrdinal("pedido");
                            int postCodigoEmpresa = dr.GetOrdinal("empresa");
                            postCodigoCliente = dr.GetOrdinal("codigo_cliente");
                            postNombreCliente = dr.GetOrdinal("nombre_cliente");
                            int postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            int postNombreEntidad = dr.GetOrdinal("nombre_entidad");
                            int postMonto = dr.GetOrdinal("monto");
                            int postModificacion = dr.GetOrdinal("modificacion");
                            int postCodigoEstadoDepuracion = dr.GetOrdinal("codigo_estado_depuracion");

                            int ratio = 0;
                            int ratioAnterior = 0;
                            TrasladoEspeciales2DetalleCLS objDetalle;
                            while (dr.Read())
                            {
                                objDetalle = new TrasladoEspeciales2DetalleCLS();
                                objDetalle.Serie = dr.GetString(postSerie);
                                objDetalle.NumeroPedido = dr.GetInt64(postNumeroPedido);
                                objDetalle.CodigoEmpresa = dr.GetString(postCodigoEmpresa);
                                objDetalle.CodigoCliente = dr.GetString(postCodigoCliente);
                                objDetalle.NombreCliente = dr.GetString(postNombreCliente);
                                objDetalle.CodigoEstadoDepuracion = (byte)dr.GetInt32(postCodigoEstadoDepuracion);
                                if (objDetalle.CodigoEstadoDepuracion == Constantes.Especiales2.EstadoDetalleTraslado.NO_DEPURADO)
                                {
                                    objDetalle.NombreCliente = new String(objDetalle.NombreCliente.Where(f => char.IsLetter(f) || f == ' ').ToArray());
                                    string[] textSplit = objDetalle.NombreCliente.Split();
                                    int len = textSplit.Length;
                                    if (len > 1)
                                    {
                                        if (textSplit[1] == "TRAE")
                                        {
                                            objDetalle.NombreCliente = textSplit[0];
                                        }
                                        else {
                                            objDetalle.NombreCliente = textSplit[0] + " " + textSplit[1];
                                        }
                                        if (textSplit[1] == "DE" || textSplit[1] == "LA")
                                        {
                                            if (len > 2)
                                            {
                                                if (textSplit[2] == "LAS")
                                                {
                                                    if (len > 3)
                                                        objDetalle.NombreCliente = objDetalle.NombreCliente + " " + textSplit[2] + " " + textSplit[3];
                                                    else
                                                        objDetalle.NombreCliente = objDetalle.NombreCliente + " " + textSplit[2];
                                                }
                                                else
                                                {
                                                    objDetalle.NombreCliente = objDetalle.NombreCliente + " " + textSplit[2];
                                                }
                                            }
                                        }
                                    }
                                    else {
                                        objDetalle.NombreCliente = textSplit[0];
                                    }

                                    ratioAnterior = 100;
                                    foreach (ClienteCLS clienteEspecial2 in listaClientesEspeciales2)
                                    {
                                        ratio = Util.Manipulacion.Compute(clienteEspecial2.NombreCompleto, objDetalle.NombreCliente);
                                        if (ratio < ratioAnterior && ratio < 2)
                                        {
                                            objDetalle.CodigoEntidad = clienteEspecial2.CodigoCliente;
                                            objDetalle.NombreEntidad = clienteEspecial2.NombreCompleto;
                                            objDetalle.CodigoEstadoDepuracion = Constantes.Especiales2.EstadoDetalleTraslado.EXISTE_COINCIDENCIA;
                                            ratioAnterior = ratio;
                                        }
                                    }
                                }
                                else {
                                    objDetalle.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                    objDetalle.NombreEntidad = dr.GetString(postNombreEntidad);
                                }
                                objDetalle.RatioSimilitud = ratioAnterior;
                                objDetalle.Monto = dr.GetDecimal(postMonto);
                                objDetalle.modificacion = dr.GetByte(postModificacion);
                                

                                listaDetalle.Add(objDetalle);
                            }//fin while
                        }// fin if
                    }// fin using
                    conexion.Close();
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    listaDetalle = null;

                }
                return listaDetalle;
            }
        }

        public string ActualizarDetalleEspeciales2(List<TrasladoEspeciales2DetalleCLS> listDetalle, int codigoTraslado, string usuarioAct)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                conexion.Open();

                SqlCommand cmd = conexion.CreateCommand();
                SqlTransaction transaction;

                // Start a local transaction.
                transaction = conexion.BeginTransaction("SampleTransaction");

                // Must assign both transaction object and connection
                // to Command object for a pending local transaction
                cmd.Connection = conexion;
                cmd.Transaction = transaction;

                try
                {
                    string sentenciaUpdateDetalle = String.Empty;
                    int contador = 0;
                    foreach (TrasladoEspeciales2DetalleCLS objDetalle in listDetalle)
                    {
                        if (contador == 0)
                        {
                            cmd.Parameters.Add("@Serie", SqlDbType.VarChar);
                            cmd.Parameters.Add("@Pedido", SqlDbType.BigInt);
                            cmd.Parameters.Add("@Empresa", SqlDbType.VarChar);
                            cmd.Parameters.Add("@CodigoEntidad", SqlDbType.VarChar);
                            cmd.Parameters.Add("@NombreEntidad", SqlDbType.VarChar);
                            cmd.Parameters.Add("@Modificacion", SqlDbType.TinyInt);
                            cmd.Parameters.Add("@CodigoEstadoDepuracion", SqlDbType.TinyInt);
                        }

                        sentenciaUpdateDetalle = @"
                        UPDATE db_tesoreria.traslado_detalle_especiales2 
                        SET codigo_entidad = @CodigoEntidad,
                            nombre_entidad = @NombreEntidad,
                            codigo_estado_depuracion = @CodigoEstadoDepuracion
                        WHERE serie = @Serie
                          AND pedido = @Pedido
                          AND empresa = @Empresa";

                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sentenciaUpdateDetalle;

                        cmd.Parameters["@Serie"].Value = objDetalle.Serie;
                        cmd.Parameters["@Pedido"].Value = objDetalle.NumeroPedido;
                        cmd.Parameters["@Empresa"].Value = objDetalle.CodigoEmpresa;
                        if (objDetalle.CodigoEstadoDepuracion == Constantes.Especiales2.EstadoDetalleTraslado.NO_DEPURADO)
                        {
                            cmd.Parameters["@CodigoEntidad"].Value = objDetalle.CodigoCliente;
                            cmd.Parameters["@NombreEntidad"].Value = objDetalle.NombreCliente;
                        }
                        else
                        {
                            cmd.Parameters["@CodigoEntidad"].Value = objDetalle.CodigoEntidad;
                            cmd.Parameters["@NombreEntidad"].Value = objDetalle.NombreEntidad;
                        }
                        cmd.Parameters["@Modificacion"].Value = 1;
                        cmd.Parameters["@CodigoEstadoDepuracion"].Value = objDetalle.CodigoEstadoDepuracion;
                        cmd.ExecuteNonQuery();
                        contador++;
                    }

                    string sentenciaUpdateEncabezado = @"
                    UPDATE db_tesoreria.traslado_especiales2 
                    SET codigo_estado = @CodigoEstadoTraslado,
                        usuario_act = @UsuarioAct,
                        fecha_act = @FechaAct
                    WHERE codigo_traslado = @CodigoTraslado";

                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sentenciaUpdateEncabezado;
                    cmd.Parameters.AddWithValue("@CodigoTraslado", codigoTraslado);
                    cmd.Parameters.AddWithValue("@CodigoEstadoTraslado", Constantes.Especiales2.EstadoTraslado.DEPURADO);
                    cmd.Parameters.AddWithValue("@UsuarioAct", usuarioAct);
                    cmd.Parameters.AddWithValue("@FechaAct", DateTime.Now);
                    cmd.ExecuteNonQuery();
                    
                    transaction.Commit();
                    conexion.Close();
                    resultado = "OK";

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    conexion.Close();
                    resultado = "Error [0]: " + ex.Message;
                }

                return resultado;
            }// fin using
        }// fin metodo

        public List<TrasladoEspeciales2DetalleCLS> GetDetalleUnificadoEspeciales2(int codigoTraslado)
        {
            List<TrasladoEspeciales2DetalleCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    SELECT codigo_entidad, 
	                       nombre_entidad, 
	                       codigo_estado_depuracion, 
	                       SUM(monto) AS monto
                    FROM db_tesoreria.traslado_detalle_especiales2
                    WHERE codigo_traslado = @CodigoTraslado
                    GROUP BY codigo_entidad, nombre_entidad, codigo_estado_depuracion";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoTraslado", codigoTraslado);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            TrasladoEspeciales2DetalleCLS objDetalle;
                            lista = new List<TrasladoEspeciales2DetalleCLS>();
                            int postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            int postNombreEntidad = dr.GetOrdinal("nombre_entidad");
                            int postMonto = dr.GetOrdinal("monto");
                            int postCodigoEstadoDepuracion = dr.GetOrdinal("codigo_estado_depuracion");
                            while (dr.Read())
                            {
                                objDetalle = new TrasladoEspeciales2DetalleCLS();
                                objDetalle.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objDetalle.NombreEntidad = dr.GetString(postNombreEntidad);
                                objDetalle.Monto = dr.GetDecimal(postMonto);
                                objDetalle.CodigoEstadoDepuracion = dr.GetByte(postCodigoEstadoDepuracion);
                                lista.Add(objDetalle);
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

        public List<TrasladoEspeciales2DetalleCLS> GetDetalleEspeciales2(int codigoTraslado)
        {
            List<TrasladoEspeciales2DetalleCLS> lista = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sql = @"
                    SELECT x.serie,
		                   x.pedido,
		                   x.empresa,
		                   x.codigo_cliente,
		                   x.nombre_cliente,
		                   CASE
			                 WHEN (x.codigo_entidad = '000001' OR x.codigo_entidad IS NULL OR x.codigo_entidad = '') THEN NULL
			                 ELSE x.codigo_entidad
		                   END AS codigo_entidad,
		                   CASE
			                 WHEN (x.codigo_entidad = '000001' OR x.codigo_entidad IS NULL OR x.codigo_entidad = '') THEN NULL
			                 ELSE x.nombre_entidad
		                   END AS nombre_entidad,
		                   x.monto,
                           FORMAT(x.fecha_grabado, 'dd/MM/yyyy, hh:mm:ss') AS fecha_grabado_str,
		                   x.modificacion,
		                   x.codigo_estado_depuracion,
                           y.nombre AS estado_depuracion 

	                FROM db_tesoreria.traslado_detalle_especiales2 x
                    INNER JOIN db_tesoreria.estado_depuracion_especiales2 y
                    ON x.codigo_estado_depuracion = y.codigo_estado_depuracion
                    WHERE x.codigo_traslado = @CodigoTraslado
	                ORDER BY x.nombre_cliente";

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@CodigoTraslado", codigoTraslado);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr != null)
                        {
                            TrasladoEspeciales2DetalleCLS objDetalle;
                            lista = new List<TrasladoEspeciales2DetalleCLS>();
                            int postSerie = dr.GetOrdinal("serie");
                            int postNumeroPedido = dr.GetOrdinal("pedido");
                            int postCodigoEmpresa = dr.GetOrdinal("empresa");
                            int postCodigoCliente = dr.GetOrdinal("codigo_cliente");
                            int postNombreCliente = dr.GetOrdinal("nombre_cliente");
                            int postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            int postNombreEntidad = dr.GetOrdinal("nombre_entidad");
                            int postMonto = dr.GetOrdinal("monto");
                            int postFechaGrabadoStr = dr.GetOrdinal("fecha_grabado_str");
                            int postCodigoEstadoDepuracion = dr.GetOrdinal("codigo_estado_depuracion");
                            int postEstadoDepuracion = dr.GetOrdinal("estado_depuracion");
                            while (dr.Read())
                            {
                                objDetalle = new TrasladoEspeciales2DetalleCLS();
                                objDetalle.Serie = dr.GetString(postSerie);
                                objDetalle.NumeroPedido = dr.GetInt64(postNumeroPedido);
                                objDetalle.CodigoEmpresa = dr.GetString(postCodigoEmpresa);
                                objDetalle.CodigoCliente = dr.GetString(postCodigoCliente);
                                objDetalle.NombreCliente = dr.GetString(postNombreCliente);
                                objDetalle.CodigoEstadoDepuracion = dr.GetByte(postCodigoEstadoDepuracion);
                                if (objDetalle.CodigoEstadoDepuracion == Constantes.Especiales2.EstadoDetalleTraslado.NO_DEPURADO)
                                {
                                    objDetalle.NombreCliente = new String(objDetalle.NombreCliente.Where(f => char.IsLetter(f) || f == ' ').ToArray());
                                    string[] textSplit = objDetalle.NombreCliente.Split();
                                    int len = textSplit.Length;
                                    if (len > 1)
                                    {
                                        if (textSplit[1] == "TRAE")
                                        {
                                            objDetalle.NombreCliente = textSplit[0];
                                        }
                                        else
                                        {
                                            objDetalle.NombreCliente = textSplit[0] + " " + textSplit[1];
                                        }
                                        if (textSplit[1] == "DE" || textSplit[1] == "LA")
                                        {
                                            if (len > 2)
                                            {
                                                if (textSplit[2] == "LAS")
                                                {
                                                    if (len > 3)
                                                        objDetalle.NombreCliente = objDetalle.NombreCliente + " " + textSplit[2] + " " + textSplit[3];
                                                    else
                                                        objDetalle.NombreCliente = objDetalle.NombreCliente + " " + textSplit[2];
                                                }
                                                else
                                                {
                                                    objDetalle.NombreCliente = objDetalle.NombreCliente + " " + textSplit[2];
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        objDetalle.NombreCliente = textSplit[0];
                                    }
                                }
                                else
                                {
                                    objDetalle.CodigoEntidad = dr.IsDBNull(postCodigoEntidad) ? "" : dr.GetString(postCodigoEntidad);
                                    objDetalle.NombreEntidad = dr.IsDBNull(postNombreEntidad) ? "" : dr.GetString(postNombreEntidad);
                                }

                                objDetalle.Monto = dr.GetDecimal(postMonto);
                                objDetalle.FechaGrabadoStr = dr.GetString(postFechaGrabadoStr);
                                objDetalle.EstadoDepuracion = dr.GetString(postEstadoDepuracion);
                                lista.Add(objDetalle);
                            }
                        }
                    }
                    conexion.Close();
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    lista = null;
                }

                return lista;
            }
        }

        public string RegistrarEspeciales2(int codigoTraslado, string fechaOperacionStr, int semanaOperacion, int anioOperacion, string usuarioIng)
        {
            string resultado = "";
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string[] word = fechaOperacionStr.Split('/');
                    int anio = Convert.ToInt32(word[2]);
                    int mes = Convert.ToInt32(word[1]);
                    int dia = Convert.ToInt32(word[0]);
                    string fechaOperacion = anio.ToString() + "-" + mes.ToString("D2") + "-" + dia.ToString("D2");

                    DateTime fecha = Util.Conversion.ConvertDateSpanishToEnglish(fechaOperacionStr);
                    int diaOperacion = Util.Conversion.DayOfWeek(fecha);

                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_tesoreria.uspRegistrarEspeciales2", conexion))
                    {
                        // le indico que es un procedure
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CodigoTraslado", codigoTraslado);
                        cmd.Parameters.AddWithValue("@FechaOperacion", fechaOperacion);
                        cmd.Parameters.AddWithValue("@DiaOperacion", diaOperacion);
                        cmd.Parameters.AddWithValue("@SemanaOperacion", semanaOperacion);
                        cmd.Parameters.AddWithValue("@AnioOperacion", anioOperacion);
                        cmd.Parameters.AddWithValue("@UsuarioIng", usuarioIng);

                        //Set SqlParameter
                        SqlParameter outParameter = new SqlParameter
                        {
                            ParameterName = "@Resultado", //Parameter name defined in stored procedure
                            //SqlDbType = SqlDbType.Int, //Data Type of Parameter
                            Size = 2000,
                            SqlDbType = SqlDbType.VarChar, //Data Type of Parameter
                            Direction = ParameterDirection.Output //Specify the parameter as ouput
                        };

                        //add the parameter to the SqlCommand object
                        cmd.Parameters.Add(outParameter);
                        cmd.ExecuteReader();
                        resultado = outParameter.Value.ToString();
                    }// fin using
                    conexion.Close();
                }
                catch (Exception ex)
                {
                    conexion.Close();
                    resultado = "Error [0]: " + ex.Message;
                }
                return resultado;
            }
        }

    }
}
