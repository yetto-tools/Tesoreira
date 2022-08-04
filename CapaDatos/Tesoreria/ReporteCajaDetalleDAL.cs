using CapaEntidad.Administracion;
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
    public class ReporteCajaDetalleDAL: CadenaConexion
    {
        public ReporteCajaDetalleListCLS GetDetalleReporte(int codigoReporte)
        {
            int postCodigoDetalleReporte = 0;
            int postCodigoReporte = 0;
            int postCodigoConcepto = 0;
            int postConcepto = 0;
            int postCodigoOperacion = 0;
            int postCodigoCategoriaEntidad = 0;
            int postCodigoEntidad = 0;
            int postNombreEntidad = 0;
            int postCodigoTransaccion = 0;
            int postSaldoAnterior = 0;
            int postMontoLunes = 0;
            int postMontoMartes = 0;
            int postMontoMiercoles = 0;
            int postMontoJueves = 0;
            int postMontoViernes = 0;
            int postMontoSabado = 0;
            int postMontoDomingo = 0;
            int postEstado = 0;
            int postTotalSemana = 0;
            int postDevoluciones = 0;
            int postAcumulado = 0;
            int postObservaciones = 0;
            ReporteCajaDetalleListCLS objReporteCaja = new ReporteCajaDetalleListCLS();
            //ReporteCajaDetalleListCLS objReporteCaja = null;
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_tesoreria.uspGetReporteSemanalCaja", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CodigoReporte", codigoReporte);
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr != null)
                        {// Encabezado de las fechas de la semana indicada
                            int postFechaStr = dr.GetOrdinal("fecha_str");
                            List<ProgramacionSemanalCLS> listaEncabezadoFechas = new List<ProgramacionSemanalCLS>();
                            ProgramacionSemanalCLS objEncabezadoFechas;
                            while (dr.Read())
                            {
                                objEncabezadoFechas = new ProgramacionSemanalCLS();
                                objEncabezadoFechas.FechaStr = dr.GetString(postFechaStr);
                                listaEncabezadoFechas.Add(objEncabezadoFechas);
                            }
                            objReporteCaja.listaEncabezadoFechas = listaEncabezadoFechas;
                        }

                        if (dr.NextResult())
                        {// Concepto: Vendedores
                            postCodigoDetalleReporte = dr.GetOrdinal("codigo_detalle_reporte");
                            postCodigoReporte = dr.GetOrdinal("codigo_reporte");
                            postCodigoConcepto = dr.GetOrdinal("codigo_concepto");
                            postConcepto = dr.GetOrdinal("concepto");
                            postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            postCodigoCategoriaEntidad = dr.GetOrdinal("codigo_categoria_entidad");
                            postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            postNombreEntidad = dr.GetOrdinal("nombre_entidad");
                            postCodigoTransaccion = dr.GetOrdinal("codigo_transaccion");
                            postSaldoAnterior = dr.GetOrdinal("saldo_anterior");
                            postMontoLunes = dr.GetOrdinal("monto_lunes");
                            postMontoMartes = dr.GetOrdinal("monto_martes");
                            postMontoMiercoles = dr.GetOrdinal("monto_miercoles");
                            postMontoJueves = dr.GetOrdinal("monto_jueves");
                            postMontoViernes = dr.GetOrdinal("monto_viernes");
                            postMontoSabado = dr.GetOrdinal("monto_sabado");
                            postMontoDomingo = dr.GetOrdinal("monto_sabado");
                            postEstado = dr.GetOrdinal("estado");
                            postTotalSemana = dr.GetOrdinal("total_semana");
                            postDevoluciones = dr.GetOrdinal("devoluciones");
                            postAcumulado = dr.GetOrdinal("acumulado");
                            postObservaciones = dr.GetOrdinal("observaciones");

                            List<ReporteCajaDetalleCLS> listaVendedores = new List<ReporteCajaDetalleCLS>();
                            ReporteCajaDetalleCLS objReporteCajaDetalle;
                            while (dr.Read())
                            {
                                objReporteCajaDetalle = new ReporteCajaDetalleCLS();
                                objReporteCajaDetalle.CodigoDetalleReporte = dr.GetInt64(postCodigoDetalleReporte);
                                objReporteCajaDetalle.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objReporteCajaDetalle.CodigoConcepto = dr.GetInt16(postCodigoConcepto);
                                objReporteCajaDetalle.Concepto = dr.GetString(postConcepto);
                                objReporteCajaDetalle.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objReporteCajaDetalle.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objReporteCajaDetalle.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objReporteCajaDetalle.NombreEntidad = dr.GetString(postNombreEntidad);
                                objReporteCajaDetalle.CodigoTransaccion = dr.IsDBNull(postCodigoTransaccion) ? null : dr.GetInt64(postCodigoTransaccion);
                                objReporteCajaDetalle.SaldoAnterior = dr.GetDecimal(postSaldoAnterior);
                                objReporteCajaDetalle.MontoLunes = dr.GetDecimal(postMontoLunes);
                                objReporteCajaDetalle.MontoMartes = dr.GetDecimal(postMontoMartes);
                                objReporteCajaDetalle.MontoMiercoles = dr.GetDecimal(postMontoMiercoles);
                                objReporteCajaDetalle.MontoJueves = dr.GetDecimal(postMontoJueves);
                                objReporteCajaDetalle.MontoViernes = dr.GetDecimal(postMontoViernes);
                                objReporteCajaDetalle.MontoSabado = dr.GetDecimal(postMontoSabado);
                                objReporteCajaDetalle.MontoDomingo = dr.GetDecimal(postMontoDomingo);
                                objReporteCajaDetalle.Estado = dr.GetByte(postEstado);
                                objReporteCajaDetalle.TotalSemana = dr.GetDecimal(postTotalSemana);
                                objReporteCajaDetalle.Devoluciones = dr.GetDecimal(postDevoluciones);
                                objReporteCajaDetalle.Acumulado = dr.GetDecimal(postAcumulado);

                                listaVendedores.Add(objReporteCajaDetalle);
                            }// fin while
                            objReporteCaja.listaVendedores = listaVendedores;
                        }

                        if (dr.NextResult())
                        {// Concepto: Especial I
                            List<ReporteCajaDetalleCLS> listaEspeciales1 = new List<ReporteCajaDetalleCLS>();
                            ReporteCajaDetalleCLS objReporteCajaDetalle;
                            while (dr.Read())
                            {
                                objReporteCajaDetalle = new ReporteCajaDetalleCLS();
                                objReporteCajaDetalle.CodigoDetalleReporte = dr.GetInt64(postCodigoDetalleReporte);
                                objReporteCajaDetalle.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objReporteCajaDetalle.CodigoConcepto = dr.GetInt16(postCodigoConcepto);
                                objReporteCajaDetalle.Concepto = dr.GetString(postConcepto);
                                objReporteCajaDetalle.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objReporteCajaDetalle.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objReporteCajaDetalle.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objReporteCajaDetalle.NombreEntidad = dr.GetString(postNombreEntidad);
                                objReporteCajaDetalle.CodigoTransaccion = dr.IsDBNull(postCodigoTransaccion) ? null : dr.GetInt64(postCodigoTransaccion);
                                objReporteCajaDetalle.SaldoAnterior = dr.GetDecimal(postSaldoAnterior);
                                objReporteCajaDetalle.MontoLunes = dr.GetDecimal(postMontoLunes);
                                objReporteCajaDetalle.MontoMartes = dr.GetDecimal(postMontoMartes);
                                objReporteCajaDetalle.MontoMiercoles = dr.GetDecimal(postMontoMiercoles);
                                objReporteCajaDetalle.MontoJueves = dr.GetDecimal(postMontoJueves);
                                objReporteCajaDetalle.MontoViernes = dr.GetDecimal(postMontoViernes);
                                objReporteCajaDetalle.MontoSabado = dr.GetDecimal(postMontoSabado);
                                objReporteCajaDetalle.MontoDomingo = dr.GetDecimal(postMontoDomingo);
                                objReporteCajaDetalle.Estado = dr.GetByte(postEstado);
                                objReporteCajaDetalle.TotalSemana = dr.GetDecimal(postTotalSemana);
                                objReporteCajaDetalle.Devoluciones = dr.GetDecimal(postDevoluciones);
                                objReporteCajaDetalle.Acumulado = dr.GetDecimal(postAcumulado);

                                listaEspeciales1.Add(objReporteCajaDetalle);
                            }// fin while
                            objReporteCaja.listaEspecial1 = listaEspeciales1;

                        }
                        if (dr.NextResult())
                        {// Concepto: Especial II
                            List<ReporteCajaDetalleCLS> listaEspeciales2 = new List<ReporteCajaDetalleCLS>();
                            ReporteCajaDetalleCLS objReporteCajaDetalle;
                            while (dr.Read())
                            {
                                objReporteCajaDetalle = new ReporteCajaDetalleCLS();
                                objReporteCajaDetalle.CodigoDetalleReporte = dr.GetInt64(postCodigoDetalleReporte);
                                objReporteCajaDetalle.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objReporteCajaDetalle.CodigoConcepto = dr.GetInt16(postCodigoConcepto);
                                objReporteCajaDetalle.Concepto = dr.GetString(postConcepto);
                                objReporteCajaDetalle.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objReporteCajaDetalle.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objReporteCajaDetalle.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objReporteCajaDetalle.NombreEntidad = dr.GetString(postNombreEntidad);
                                objReporteCajaDetalle.CodigoTransaccion = dr.IsDBNull(postCodigoTransaccion) ? null : dr.GetInt64(postCodigoTransaccion);
                                objReporteCajaDetalle.SaldoAnterior = dr.GetDecimal(postSaldoAnterior);
                                objReporteCajaDetalle.MontoLunes = dr.GetDecimal(postMontoLunes);
                                objReporteCajaDetalle.MontoMartes = dr.GetDecimal(postMontoMartes);
                                objReporteCajaDetalle.MontoMiercoles = dr.GetDecimal(postMontoMiercoles);
                                objReporteCajaDetalle.MontoJueves = dr.GetDecimal(postMontoJueves);
                                objReporteCajaDetalle.MontoViernes = dr.GetDecimal(postMontoViernes);
                                objReporteCajaDetalle.MontoSabado = dr.GetDecimal(postMontoSabado);
                                objReporteCajaDetalle.MontoDomingo = dr.GetDecimal(postMontoDomingo);
                                objReporteCajaDetalle.Estado = dr.GetByte(postEstado);
                                objReporteCajaDetalle.TotalSemana = dr.GetDecimal(postTotalSemana);
                                objReporteCajaDetalle.Devoluciones = dr.GetDecimal(postDevoluciones);
                                objReporteCajaDetalle.Acumulado = dr.GetDecimal(postAcumulado);

                                listaEspeciales2.Add(objReporteCajaDetalle);
                            }// fin while
                            objReporteCaja.listaEspecial2 = listaEspeciales2;
                        }// fin if
                        if (dr.NextResult())
                        {// Concepto: Cajas
                            List<ReporteCajaDetalleCLS> listaCajas = new List<ReporteCajaDetalleCLS>();
                            ReporteCajaDetalleCLS objReporteCajaDetalle;
                            while (dr.Read())
                            {
                                objReporteCajaDetalle = new ReporteCajaDetalleCLS();
                                objReporteCajaDetalle.CodigoDetalleReporte = dr.GetInt64(postCodigoDetalleReporte);
                                objReporteCajaDetalle.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objReporteCajaDetalle.CodigoConcepto = dr.GetInt16(postCodigoConcepto);
                                objReporteCajaDetalle.Concepto = dr.GetString(postConcepto);
                                objReporteCajaDetalle.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objReporteCajaDetalle.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objReporteCajaDetalle.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objReporteCajaDetalle.NombreEntidad = dr.GetString(postNombreEntidad);
                                objReporteCajaDetalle.CodigoTransaccion = dr.IsDBNull(postCodigoTransaccion) ? null : dr.GetInt64(postCodigoTransaccion);
                                objReporteCajaDetalle.SaldoAnterior = dr.GetDecimal(postSaldoAnterior);
                                objReporteCajaDetalle.MontoLunes = dr.GetDecimal(postMontoLunes);
                                objReporteCajaDetalle.MontoMartes = dr.GetDecimal(postMontoMartes);
                                objReporteCajaDetalle.MontoMiercoles = dr.GetDecimal(postMontoMiercoles);
                                objReporteCajaDetalle.MontoJueves = dr.GetDecimal(postMontoJueves);
                                objReporteCajaDetalle.MontoViernes = dr.GetDecimal(postMontoViernes);
                                objReporteCajaDetalle.MontoSabado = dr.GetDecimal(postMontoSabado);
                                objReporteCajaDetalle.MontoDomingo = dr.GetDecimal(postMontoDomingo);
                                objReporteCajaDetalle.Estado = dr.GetByte(postEstado);
                                objReporteCajaDetalle.TotalSemana = dr.GetDecimal(postTotalSemana);
                                objReporteCajaDetalle.Devoluciones = dr.GetDecimal(postDevoluciones);
                                objReporteCajaDetalle.Acumulado = dr.GetDecimal(postAcumulado);

                                listaCajas.Add(objReporteCajaDetalle);
                            }// fin while
                            objReporteCaja.listaCajas = listaCajas;
                        }// fin if

                        if (dr.NextResult())
                        {// Concepto: Combustible Carros
                            List<ReporteCajaDetalleCLS> listaCombustibleCarros = new List<ReporteCajaDetalleCLS>();
                            ReporteCajaDetalleCLS objReporteCajaDetalle;
                            while (dr.Read())
                            {
                                objReporteCajaDetalle = new ReporteCajaDetalleCLS();
                                objReporteCajaDetalle.CodigoDetalleReporte = dr.GetInt64(postCodigoDetalleReporte);
                                objReporteCajaDetalle.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objReporteCajaDetalle.CodigoConcepto = dr.GetInt16(postCodigoConcepto);
                                objReporteCajaDetalle.Concepto = dr.GetString(postConcepto);
                                objReporteCajaDetalle.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objReporteCajaDetalle.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objReporteCajaDetalle.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objReporteCajaDetalle.NombreEntidad = dr.GetString(postNombreEntidad);
                                objReporteCajaDetalle.CodigoTransaccion = dr.IsDBNull(postCodigoTransaccion) ? null : dr.GetInt64(postCodigoTransaccion);
                                objReporteCajaDetalle.SaldoAnterior = dr.GetDecimal(postSaldoAnterior);
                                objReporteCajaDetalle.MontoLunes = dr.GetDecimal(postMontoLunes);
                                objReporteCajaDetalle.MontoMartes = dr.GetDecimal(postMontoMartes);
                                objReporteCajaDetalle.MontoMiercoles = dr.GetDecimal(postMontoMiercoles);
                                objReporteCajaDetalle.MontoJueves = dr.GetDecimal(postMontoJueves);
                                objReporteCajaDetalle.MontoViernes = dr.GetDecimal(postMontoViernes);
                                objReporteCajaDetalle.MontoSabado = dr.GetDecimal(postMontoSabado);
                                objReporteCajaDetalle.MontoDomingo = dr.GetDecimal(postMontoDomingo);
                                objReporteCajaDetalle.Estado = dr.GetByte(postEstado);
                                objReporteCajaDetalle.TotalSemana = dr.GetDecimal(postTotalSemana);
                                objReporteCajaDetalle.Devoluciones = dr.GetDecimal(postDevoluciones);
                                objReporteCajaDetalle.Acumulado = dr.GetDecimal(postAcumulado);
                                objReporteCajaDetalle.Observaciones = dr.IsDBNull(postObservaciones) ? "" : dr.GetString(postObservaciones);

                                listaCombustibleCarros.Add(objReporteCajaDetalle);
                            }// fin while
                            objReporteCaja.listaCombustibleCarros = listaCombustibleCarros;
                        }// fin if

                        if (dr.NextResult())
                        {// Concepto: Planillas
                            List<ReporteCajaDetalleCLS> listaPlanillas = new List<ReporteCajaDetalleCLS>();
                            ReporteCajaDetalleCLS objReporteCajaDetalle;
                            while (dr.Read())
                            {
                                objReporteCajaDetalle = new ReporteCajaDetalleCLS();
                                objReporteCajaDetalle.CodigoDetalleReporte = dr.GetInt64(postCodigoDetalleReporte);
                                objReporteCajaDetalle.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objReporteCajaDetalle.CodigoConcepto = dr.GetInt16(postCodigoConcepto);
                                objReporteCajaDetalle.Concepto = dr.GetString(postConcepto);
                                objReporteCajaDetalle.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objReporteCajaDetalle.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objReporteCajaDetalle.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objReporteCajaDetalle.NombreEntidad = dr.GetString(postNombreEntidad);
                                objReporteCajaDetalle.CodigoTransaccion = dr.IsDBNull(postCodigoTransaccion) ? null : dr.GetInt64(postCodigoTransaccion);
                                objReporteCajaDetalle.SaldoAnterior = dr.GetDecimal(postSaldoAnterior);
                                objReporteCajaDetalle.MontoLunes = dr.GetDecimal(postMontoLunes);
                                objReporteCajaDetalle.MontoMartes = dr.GetDecimal(postMontoMartes);
                                objReporteCajaDetalle.MontoMiercoles = dr.GetDecimal(postMontoMiercoles);
                                objReporteCajaDetalle.MontoJueves = dr.GetDecimal(postMontoJueves);
                                objReporteCajaDetalle.MontoViernes = dr.GetDecimal(postMontoViernes);
                                objReporteCajaDetalle.MontoSabado = dr.GetDecimal(postMontoSabado);
                                objReporteCajaDetalle.MontoDomingo = dr.GetDecimal(postMontoDomingo);
                                objReporteCajaDetalle.Estado = dr.GetByte(postEstado);
                                objReporteCajaDetalle.TotalSemana = dr.GetDecimal(postTotalSemana);
                                objReporteCajaDetalle.Devoluciones = dr.GetDecimal(postDevoluciones);
                                objReporteCajaDetalle.Acumulado = dr.GetDecimal(postAcumulado);
                                objReporteCajaDetalle.Observaciones = dr.IsDBNull(postObservaciones) ? "" :  dr.GetString(postObservaciones);

                                listaPlanillas.Add(objReporteCajaDetalle);
                            }// fin while
                            objReporteCaja.listaPlanillas = listaPlanillas;
                        }// fin if


                        if (dr.NextResult())
                        {// Concepto: Sueldos Indirectos
                            List<ReporteCajaDetalleCLS> listaSueldosIndirectos = new List<ReporteCajaDetalleCLS>();
                            ReporteCajaDetalleCLS objReporteCajaDetalle;
                            while (dr.Read())
                            {
                                objReporteCajaDetalle = new ReporteCajaDetalleCLS();
                                objReporteCajaDetalle.CodigoDetalleReporte = dr.GetInt64(postCodigoDetalleReporte);
                                objReporteCajaDetalle.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objReporteCajaDetalle.CodigoConcepto = dr.GetInt16(postCodigoConcepto);
                                objReporteCajaDetalle.Concepto = dr.GetString(postConcepto);
                                objReporteCajaDetalle.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objReporteCajaDetalle.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objReporteCajaDetalle.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objReporteCajaDetalle.NombreEntidad = dr.GetString(postNombreEntidad);
                                objReporteCajaDetalle.CodigoTransaccion = dr.IsDBNull(postCodigoTransaccion) ? null : dr.GetInt64(postCodigoTransaccion);
                                objReporteCajaDetalle.SaldoAnterior = dr.GetDecimal(postSaldoAnterior);
                                objReporteCajaDetalle.MontoLunes = dr.GetDecimal(postMontoLunes);
                                objReporteCajaDetalle.MontoMartes = dr.GetDecimal(postMontoMartes);
                                objReporteCajaDetalle.MontoMiercoles = dr.GetDecimal(postMontoMiercoles);
                                objReporteCajaDetalle.MontoJueves = dr.GetDecimal(postMontoJueves);
                                objReporteCajaDetalle.MontoViernes = dr.GetDecimal(postMontoViernes);
                                objReporteCajaDetalle.MontoSabado = dr.GetDecimal(postMontoSabado);
                                objReporteCajaDetalle.MontoDomingo = dr.GetDecimal(postMontoDomingo);
                                objReporteCajaDetalle.Estado = dr.GetByte(postEstado);
                                objReporteCajaDetalle.TotalSemana = dr.GetDecimal(postTotalSemana);
                                objReporteCajaDetalle.Devoluciones = dr.GetDecimal(postDevoluciones);
                                objReporteCajaDetalle.Acumulado = dr.GetDecimal(postAcumulado);
                                objReporteCajaDetalle.Observaciones = dr.IsDBNull(postObservaciones) ? "" : dr.GetString(postObservaciones);

                                listaSueldosIndirectos.Add(objReporteCajaDetalle);
                            }// fin while
                            objReporteCaja.listaSueldosIndirectos = listaSueldosIndirectos;
                        }// fin if

                        if (dr.NextResult())
                        {// Concepto: Liquidaciones
                            List<ReporteCajaDetalleCLS> listaLiquidaciones = new List<ReporteCajaDetalleCLS>();
                            ReporteCajaDetalleCLS objReporteCajaDetalle;
                            while (dr.Read())
                            {
                                objReporteCajaDetalle = new ReporteCajaDetalleCLS();
                                objReporteCajaDetalle.CodigoDetalleReporte = dr.GetInt64(postCodigoDetalleReporte);
                                objReporteCajaDetalle.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objReporteCajaDetalle.CodigoConcepto = dr.GetInt16(postCodigoConcepto);
                                objReporteCajaDetalle.Concepto = dr.GetString(postConcepto);
                                objReporteCajaDetalle.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objReporteCajaDetalle.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objReporteCajaDetalle.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objReporteCajaDetalle.NombreEntidad = dr.GetString(postNombreEntidad);
                                objReporteCajaDetalle.CodigoTransaccion = dr.IsDBNull(postCodigoTransaccion) ? null : dr.GetInt64(postCodigoTransaccion);
                                objReporteCajaDetalle.SaldoAnterior = dr.GetDecimal(postSaldoAnterior);
                                objReporteCajaDetalle.MontoLunes = dr.GetDecimal(postMontoLunes);
                                objReporteCajaDetalle.MontoMartes = dr.GetDecimal(postMontoMartes);
                                objReporteCajaDetalle.MontoMiercoles = dr.GetDecimal(postMontoMiercoles);
                                objReporteCajaDetalle.MontoJueves = dr.GetDecimal(postMontoJueves);
                                objReporteCajaDetalle.MontoViernes = dr.GetDecimal(postMontoViernes);
                                objReporteCajaDetalle.MontoSabado = dr.GetDecimal(postMontoSabado);
                                objReporteCajaDetalle.MontoDomingo = dr.GetDecimal(postMontoDomingo);
                                objReporteCajaDetalle.Estado = dr.GetByte(postEstado);
                                objReporteCajaDetalle.TotalSemana = dr.GetDecimal(postTotalSemana);
                                objReporteCajaDetalle.Devoluciones = dr.GetDecimal(postDevoluciones);
                                objReporteCajaDetalle.Acumulado = dr.GetDecimal(postAcumulado);
                                objReporteCajaDetalle.Observaciones = dr.IsDBNull(postObservaciones) ? "" : dr.GetString(postObservaciones);

                                listaLiquidaciones.Add(objReporteCajaDetalle);
                            }// fin while
                            objReporteCaja.listaLiquidaciones = listaLiquidaciones;
                        }// fin if

                        if (dr.NextResult())
                        {// Concepto: Vacaciones
                            List<ReporteCajaDetalleCLS> listaVacaciones = new List<ReporteCajaDetalleCLS>();
                            ReporteCajaDetalleCLS objReporteCajaDetalle;
                            while (dr.Read())
                            {
                                objReporteCajaDetalle = new ReporteCajaDetalleCLS();
                                objReporteCajaDetalle.CodigoDetalleReporte = dr.GetInt64(postCodigoDetalleReporte);
                                objReporteCajaDetalle.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objReporteCajaDetalle.CodigoConcepto = dr.GetInt16(postCodigoConcepto);
                                objReporteCajaDetalle.Concepto = dr.GetString(postConcepto);
                                objReporteCajaDetalle.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objReporteCajaDetalle.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objReporteCajaDetalle.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objReporteCajaDetalle.NombreEntidad = dr.GetString(postNombreEntidad);
                                objReporteCajaDetalle.CodigoTransaccion = dr.IsDBNull(postCodigoTransaccion) ? null : dr.GetInt64(postCodigoTransaccion);
                                objReporteCajaDetalle.SaldoAnterior = dr.GetDecimal(postSaldoAnterior);
                                objReporteCajaDetalle.MontoLunes = dr.GetDecimal(postMontoLunes);
                                objReporteCajaDetalle.MontoMartes = dr.GetDecimal(postMontoMartes);
                                objReporteCajaDetalle.MontoMiercoles = dr.GetDecimal(postMontoMiercoles);
                                objReporteCajaDetalle.MontoJueves = dr.GetDecimal(postMontoJueves);
                                objReporteCajaDetalle.MontoViernes = dr.GetDecimal(postMontoViernes);
                                objReporteCajaDetalle.MontoSabado = dr.GetDecimal(postMontoSabado);
                                objReporteCajaDetalle.MontoDomingo = dr.GetDecimal(postMontoDomingo);
                                objReporteCajaDetalle.Estado = dr.GetByte(postEstado);
                                objReporteCajaDetalle.TotalSemana = dr.GetDecimal(postTotalSemana);
                                objReporteCajaDetalle.Devoluciones = dr.GetDecimal(postDevoluciones);
                                objReporteCajaDetalle.Acumulado = dr.GetDecimal(postAcumulado);
                                objReporteCajaDetalle.Observaciones = dr.IsDBNull(postObservaciones) ? "" : dr.GetString(postObservaciones);

                                listaVacaciones.Add(objReporteCajaDetalle);
                            }// fin while
                            objReporteCaja.listaVacaciones = listaVacaciones;
                        }// fin if

                        if (dr.NextResult())
                        {// Concepto: Bono de Fin de Mes
                            List<ReporteCajaDetalleCLS> listaBonoFinDeMes = new List<ReporteCajaDetalleCLS>();
                            ReporteCajaDetalleCLS objReporteCajaDetalle;
                            while (dr.Read())
                            {
                                objReporteCajaDetalle = new ReporteCajaDetalleCLS();
                                objReporteCajaDetalle.CodigoDetalleReporte = dr.GetInt64(postCodigoDetalleReporte);
                                objReporteCajaDetalle.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objReporteCajaDetalle.CodigoConcepto = dr.GetInt16(postCodigoConcepto);
                                objReporteCajaDetalle.Concepto = dr.GetString(postConcepto);
                                objReporteCajaDetalle.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objReporteCajaDetalle.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objReporteCajaDetalle.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objReporteCajaDetalle.NombreEntidad = dr.GetString(postNombreEntidad);
                                objReporteCajaDetalle.CodigoTransaccion = dr.IsDBNull(postCodigoTransaccion) ? null : dr.GetInt64(postCodigoTransaccion);
                                objReporteCajaDetalle.SaldoAnterior = dr.GetDecimal(postSaldoAnterior);
                                objReporteCajaDetalle.MontoLunes = dr.GetDecimal(postMontoLunes);
                                objReporteCajaDetalle.MontoMartes = dr.GetDecimal(postMontoMartes);
                                objReporteCajaDetalle.MontoMiercoles = dr.GetDecimal(postMontoMiercoles);
                                objReporteCajaDetalle.MontoJueves = dr.GetDecimal(postMontoJueves);
                                objReporteCajaDetalle.MontoViernes = dr.GetDecimal(postMontoViernes);
                                objReporteCajaDetalle.MontoSabado = dr.GetDecimal(postMontoSabado);
                                objReporteCajaDetalle.MontoDomingo = dr.GetDecimal(postMontoDomingo);
                                objReporteCajaDetalle.Estado = dr.GetByte(postEstado);
                                objReporteCajaDetalle.TotalSemana = dr.GetDecimal(postTotalSemana);
                                objReporteCajaDetalle.Devoluciones = dr.GetDecimal(postDevoluciones);
                                objReporteCajaDetalle.Acumulado = dr.GetDecimal(postAcumulado);
                                objReporteCajaDetalle.Observaciones = dr.IsDBNull(postObservaciones) ? "" : dr.GetString(postObservaciones);

                                listaBonoFinDeMes.Add(objReporteCajaDetalle);
                            }// fin while
                            objReporteCaja.listaBonoFinDeMes = listaBonoFinDeMes;
                        }// fin if

                        if (dr.NextResult())
                        {// Concepto: Bonos Extras (Comisiones)
                            List<ReporteCajaDetalleCLS> listaBonosExtras = new List<ReporteCajaDetalleCLS>();
                            ReporteCajaDetalleCLS objReporteCajaDetalle;
                            while (dr.Read())
                            {
                                objReporteCajaDetalle = new ReporteCajaDetalleCLS();
                                objReporteCajaDetalle.CodigoDetalleReporte = dr.GetInt64(postCodigoDetalleReporte);
                                objReporteCajaDetalle.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objReporteCajaDetalle.CodigoConcepto = dr.GetInt16(postCodigoConcepto);
                                objReporteCajaDetalle.Concepto = dr.GetString(postConcepto);
                                objReporteCajaDetalle.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objReporteCajaDetalle.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objReporteCajaDetalle.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objReporteCajaDetalle.NombreEntidad = dr.GetString(postNombreEntidad);
                                objReporteCajaDetalle.CodigoTransaccion = dr.IsDBNull(postCodigoTransaccion) ? null : dr.GetInt64(postCodigoTransaccion);
                                objReporteCajaDetalle.SaldoAnterior = dr.GetDecimal(postSaldoAnterior);
                                objReporteCajaDetalle.MontoLunes = dr.GetDecimal(postMontoLunes);
                                objReporteCajaDetalle.MontoMartes = dr.GetDecimal(postMontoMartes);
                                objReporteCajaDetalle.MontoMiercoles = dr.GetDecimal(postMontoMiercoles);
                                objReporteCajaDetalle.MontoJueves = dr.GetDecimal(postMontoJueves);
                                objReporteCajaDetalle.MontoViernes = dr.GetDecimal(postMontoViernes);
                                objReporteCajaDetalle.MontoSabado = dr.GetDecimal(postMontoSabado);
                                objReporteCajaDetalle.MontoDomingo = dr.GetDecimal(postMontoDomingo);
                                objReporteCajaDetalle.Estado = dr.GetByte(postEstado);
                                objReporteCajaDetalle.TotalSemana = dr.GetDecimal(postTotalSemana);
                                objReporteCajaDetalle.Devoluciones = dr.GetDecimal(postDevoluciones);
                                objReporteCajaDetalle.Acumulado = dr.GetDecimal(postAcumulado);
                                objReporteCajaDetalle.Observaciones = dr.IsDBNull(postObservaciones) ? "" : dr.GetString(postObservaciones);

                                listaBonosExtras.Add(objReporteCajaDetalle);
                            }// fin while
                            objReporteCaja.listaBonosExtras = listaBonosExtras;
                        }// fin if

                        if (dr.NextResult())
                        {// Concepto: Bono 14
                            List<ReporteCajaDetalleCLS> listaBono14 = new List<ReporteCajaDetalleCLS>();
                            ReporteCajaDetalleCLS objReporteCajaDetalle;
                            while (dr.Read())
                            {
                                objReporteCajaDetalle = new ReporteCajaDetalleCLS();
                                objReporteCajaDetalle.CodigoDetalleReporte = dr.GetInt64(postCodigoDetalleReporte);
                                objReporteCajaDetalle.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objReporteCajaDetalle.CodigoConcepto = dr.GetInt16(postCodigoConcepto);
                                objReporteCajaDetalle.Concepto = dr.GetString(postConcepto);
                                objReporteCajaDetalle.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objReporteCajaDetalle.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objReporteCajaDetalle.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objReporteCajaDetalle.NombreEntidad = dr.GetString(postNombreEntidad);
                                objReporteCajaDetalle.CodigoTransaccion = dr.IsDBNull(postCodigoTransaccion) ? null : dr.GetInt64(postCodigoTransaccion);
                                objReporteCajaDetalle.SaldoAnterior = dr.GetDecimal(postSaldoAnterior);
                                objReporteCajaDetalle.MontoLunes = dr.GetDecimal(postMontoLunes);
                                objReporteCajaDetalle.MontoMartes = dr.GetDecimal(postMontoMartes);
                                objReporteCajaDetalle.MontoMiercoles = dr.GetDecimal(postMontoMiercoles);
                                objReporteCajaDetalle.MontoJueves = dr.GetDecimal(postMontoJueves);
                                objReporteCajaDetalle.MontoViernes = dr.GetDecimal(postMontoViernes);
                                objReporteCajaDetalle.MontoSabado = dr.GetDecimal(postMontoSabado);
                                objReporteCajaDetalle.MontoDomingo = dr.GetDecimal(postMontoDomingo);
                                objReporteCajaDetalle.Estado = dr.GetByte(postEstado);
                                objReporteCajaDetalle.TotalSemana = dr.GetDecimal(postTotalSemana);
                                objReporteCajaDetalle.Devoluciones = dr.GetDecimal(postDevoluciones);
                                objReporteCajaDetalle.Acumulado = dr.GetDecimal(postAcumulado);
                                objReporteCajaDetalle.Observaciones = dr.IsDBNull(postObservaciones) ? "" : dr.GetString(postObservaciones);

                                listaBono14.Add(objReporteCajaDetalle);
                            }// fin while
                            objReporteCaja.listaBono14 = listaBono14;
                        }// fin if

                        if (dr.NextResult())
                        {// Concepto: Aguinaldos
                            List<ReporteCajaDetalleCLS> listaAguinaldos = new List<ReporteCajaDetalleCLS>();
                            ReporteCajaDetalleCLS objReporteCajaDetalle;
                            while (dr.Read())
                            {
                                objReporteCajaDetalle = new ReporteCajaDetalleCLS();
                                objReporteCajaDetalle.CodigoDetalleReporte = dr.GetInt64(postCodigoDetalleReporte);
                                objReporteCajaDetalle.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objReporteCajaDetalle.CodigoConcepto = dr.GetInt16(postCodigoConcepto);
                                objReporteCajaDetalle.Concepto = dr.GetString(postConcepto);
                                objReporteCajaDetalle.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objReporteCajaDetalle.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objReporteCajaDetalle.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objReporteCajaDetalle.NombreEntidad = dr.GetString(postNombreEntidad);
                                objReporteCajaDetalle.CodigoTransaccion = dr.IsDBNull(postCodigoTransaccion) ? null : dr.GetInt64(postCodigoTransaccion);
                                objReporteCajaDetalle.SaldoAnterior = dr.GetDecimal(postSaldoAnterior);
                                objReporteCajaDetalle.MontoLunes = dr.GetDecimal(postMontoLunes);
                                objReporteCajaDetalle.MontoMartes = dr.GetDecimal(postMontoMartes);
                                objReporteCajaDetalle.MontoMiercoles = dr.GetDecimal(postMontoMiercoles);
                                objReporteCajaDetalle.MontoJueves = dr.GetDecimal(postMontoJueves);
                                objReporteCajaDetalle.MontoViernes = dr.GetDecimal(postMontoViernes);
                                objReporteCajaDetalle.MontoSabado = dr.GetDecimal(postMontoSabado);
                                objReporteCajaDetalle.MontoDomingo = dr.GetDecimal(postMontoDomingo);
                                objReporteCajaDetalle.Estado = dr.GetByte(postEstado);
                                objReporteCajaDetalle.TotalSemana = dr.GetDecimal(postTotalSemana);
                                objReporteCajaDetalle.Devoluciones = dr.GetDecimal(postDevoluciones);
                                objReporteCajaDetalle.Acumulado = dr.GetDecimal(postAcumulado);
                                objReporteCajaDetalle.Observaciones = dr.IsDBNull(postObservaciones) ? "" : dr.GetString(postObservaciones);

                                listaAguinaldos.Add(objReporteCajaDetalle);
                            }// fin while
                            objReporteCaja.listaAguinaldos = listaAguinaldos;
                        }// fin if

                        if (dr.NextResult())
                        {// Concepto: Prestamos
                            List<ReporteCajaDetalleCLS> listaPrestamos = new List<ReporteCajaDetalleCLS>();
                            ReporteCajaDetalleCLS objReporteCajaDetalle;
                            while (dr.Read())
                            {
                                objReporteCajaDetalle = new ReporteCajaDetalleCLS();
                                objReporteCajaDetalle.CodigoDetalleReporte = dr.GetInt64(postCodigoDetalleReporte);
                                objReporteCajaDetalle.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objReporteCajaDetalle.CodigoConcepto = dr.GetInt16(postCodigoConcepto);
                                objReporteCajaDetalle.Concepto = dr.GetString(postConcepto);
                                objReporteCajaDetalle.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objReporteCajaDetalle.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objReporteCajaDetalle.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objReporteCajaDetalle.NombreEntidad = dr.GetString(postNombreEntidad);
                                objReporteCajaDetalle.CodigoTransaccion = dr.IsDBNull(postCodigoTransaccion) ? null : dr.GetInt64(postCodigoTransaccion);
                                objReporteCajaDetalle.SaldoAnterior = dr.GetDecimal(postSaldoAnterior);
                                objReporteCajaDetalle.MontoLunes = dr.GetDecimal(postMontoLunes);
                                objReporteCajaDetalle.MontoMartes = dr.GetDecimal(postMontoMartes);
                                objReporteCajaDetalle.MontoMiercoles = dr.GetDecimal(postMontoMiercoles);
                                objReporteCajaDetalle.MontoJueves = dr.GetDecimal(postMontoJueves);
                                objReporteCajaDetalle.MontoViernes = dr.GetDecimal(postMontoViernes);
                                objReporteCajaDetalle.MontoSabado = dr.GetDecimal(postMontoSabado);
                                objReporteCajaDetalle.MontoDomingo = dr.GetDecimal(postMontoDomingo);
                                objReporteCajaDetalle.Estado = dr.GetByte(postEstado);
                                objReporteCajaDetalle.TotalSemana = dr.GetDecimal(postTotalSemana);
                                objReporteCajaDetalle.Devoluciones = dr.GetDecimal(postDevoluciones);
                                objReporteCajaDetalle.Acumulado = dr.GetDecimal(postAcumulado);
                                objReporteCajaDetalle.Observaciones = dr.IsDBNull(postObservaciones) ? "" : dr.GetString(postObservaciones);

                                listaPrestamos.Add(objReporteCajaDetalle);
                            }// fin while
                            objReporteCaja.listaPrestamos = listaPrestamos;
                        }// fin if


                        if (dr.NextResult())
                        {// Concepto: Anticipos
                            List<ReporteCajaDetalleCLS> listaAnticipos = new List<ReporteCajaDetalleCLS>();
                            ReporteCajaDetalleCLS objReporteCajaDetalle;
                            while (dr.Read())
                            {
                                objReporteCajaDetalle = new ReporteCajaDetalleCLS();
                                objReporteCajaDetalle.CodigoDetalleReporte = (long)dr.GetInt32(postCodigoDetalleReporte);
                                objReporteCajaDetalle.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objReporteCajaDetalle.CodigoConcepto = dr.GetInt16(postCodigoConcepto);
                                objReporteCajaDetalle.Concepto = dr.GetString(postConcepto);
                                objReporteCajaDetalle.CodigoOperacion = (short)dr.GetInt32(postCodigoOperacion);
                                objReporteCajaDetalle.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objReporteCajaDetalle.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objReporteCajaDetalle.NombreEntidad = dr.GetString(postNombreEntidad);
                                objReporteCajaDetalle.CodigoTransaccion = dr.IsDBNull(postCodigoTransaccion) ? null : dr.GetInt64(postCodigoTransaccion);
                                objReporteCajaDetalle.SaldoAnterior = dr.GetDecimal(postSaldoAnterior);
                                objReporteCajaDetalle.MontoLunes = dr.GetDecimal(postMontoLunes);
                                objReporteCajaDetalle.MontoMartes = dr.GetDecimal(postMontoMartes);
                                objReporteCajaDetalle.MontoMiercoles = dr.GetDecimal(postMontoMiercoles);
                                objReporteCajaDetalle.MontoJueves = dr.GetDecimal(postMontoJueves);
                                objReporteCajaDetalle.MontoViernes = dr.GetDecimal(postMontoViernes);
                                objReporteCajaDetalle.MontoSabado = dr.GetDecimal(postMontoSabado);
                                objReporteCajaDetalle.MontoDomingo = dr.GetDecimal(postMontoDomingo);
                                objReporteCajaDetalle.Estado = (byte)dr.GetInt32(postEstado);
                                objReporteCajaDetalle.TotalSemana = dr.GetDecimal(postTotalSemana);
                                objReporteCajaDetalle.Devoluciones = dr.GetDecimal(postDevoluciones);
                                objReporteCajaDetalle.Acumulado = dr.GetDecimal(postAcumulado);
                                objReporteCajaDetalle.Observaciones = dr.IsDBNull(postObservaciones) ? "" : dr.GetString(postObservaciones);

                                listaAnticipos.Add(objReporteCajaDetalle);
                            }// fin while
                            objReporteCaja.listaAnticipos = listaAnticipos;

                        }// fin if

                        if (dr.NextResult())
                        {// Concepto: Materia Prima
                            List<ReporteCajaDetalleCLS> listaMateriaPrima = new List<ReporteCajaDetalleCLS>();
                            ReporteCajaDetalleCLS objReporteCajaDetalle;
                            while (dr.Read())
                            {
                                objReporteCajaDetalle = new ReporteCajaDetalleCLS();
                                objReporteCajaDetalle.CodigoDetalleReporte = (long)dr.GetInt32(postCodigoDetalleReporte);
                                objReporteCajaDetalle.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objReporteCajaDetalle.CodigoConcepto = dr.GetInt16(postCodigoConcepto);
                                objReporteCajaDetalle.Concepto = dr.GetString(postConcepto);
                                objReporteCajaDetalle.CodigoOperacion = (short)dr.GetInt32(postCodigoOperacion);
                                objReporteCajaDetalle.CodigoCategoriaEntidad = (short)dr.GetInt32(postCodigoCategoriaEntidad);
                                objReporteCajaDetalle.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objReporteCajaDetalle.NombreEntidad = dr.GetString(postNombreEntidad);
                                objReporteCajaDetalle.CodigoTransaccion = dr.IsDBNull(postCodigoTransaccion) ? null : dr.GetInt64(postCodigoTransaccion);
                                objReporteCajaDetalle.SaldoAnterior = dr.GetDecimal(postSaldoAnterior);
                                objReporteCajaDetalle.MontoLunes = dr.GetDecimal(postMontoLunes);
                                objReporteCajaDetalle.MontoMartes = dr.GetDecimal(postMontoMartes);
                                objReporteCajaDetalle.MontoMiercoles = dr.GetDecimal(postMontoMiercoles);
                                objReporteCajaDetalle.MontoJueves = dr.GetDecimal(postMontoJueves);
                                objReporteCajaDetalle.MontoViernes = dr.GetDecimal(postMontoViernes);
                                objReporteCajaDetalle.MontoSabado = dr.GetDecimal(postMontoSabado);
                                objReporteCajaDetalle.MontoDomingo = dr.GetDecimal(postMontoDomingo);
                                objReporteCajaDetalle.Estado = (byte)dr.GetInt32(postEstado);
                                objReporteCajaDetalle.TotalSemana = dr.GetDecimal(postTotalSemana);
                                objReporteCajaDetalle.Devoluciones = dr.GetDecimal(postDevoluciones);
                                objReporteCajaDetalle.Acumulado = dr.GetDecimal(postAcumulado);
                                objReporteCajaDetalle.Observaciones = dr.IsDBNull(postObservaciones) ? "" : dr.GetString(postObservaciones);

                                listaMateriaPrima.Add(objReporteCajaDetalle);
                            }// fin while
                            objReporteCaja.listaMateriaPrima = listaMateriaPrima;

                        }// fin if

                        if (dr.NextResult())
                        {// Concepto: Gastos Indirectos
                            List<ReporteCajaDetalleCLS> listaGastosIndirectos = new List<ReporteCajaDetalleCLS>();
                            ReporteCajaDetalleCLS objReporteCajaDetalle;
                            while (dr.Read())
                            {
                                objReporteCajaDetalle = new ReporteCajaDetalleCLS();
                                objReporteCajaDetalle.CodigoDetalleReporte = (long)dr.GetInt32(postCodigoDetalleReporte);
                                objReporteCajaDetalle.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objReporteCajaDetalle.CodigoConcepto = dr.GetInt16(postCodigoConcepto);
                                objReporteCajaDetalle.Concepto = dr.GetString(postConcepto);
                                objReporteCajaDetalle.CodigoOperacion = (short)dr.GetInt32(postCodigoOperacion);
                                objReporteCajaDetalle.CodigoCategoriaEntidad = (short)dr.GetInt32(postCodigoCategoriaEntidad);
                                objReporteCajaDetalle.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objReporteCajaDetalle.NombreEntidad = dr.GetString(postNombreEntidad);
                                objReporteCajaDetalle.CodigoTransaccion = dr.IsDBNull(postCodigoTransaccion) ? null : dr.GetInt64(postCodigoTransaccion);
                                objReporteCajaDetalle.SaldoAnterior = dr.GetDecimal(postSaldoAnterior);
                                objReporteCajaDetalle.MontoLunes = dr.GetDecimal(postMontoLunes);
                                objReporteCajaDetalle.MontoMartes = dr.GetDecimal(postMontoMartes);
                                objReporteCajaDetalle.MontoMiercoles = dr.GetDecimal(postMontoMiercoles);
                                objReporteCajaDetalle.MontoJueves = dr.GetDecimal(postMontoJueves);
                                objReporteCajaDetalle.MontoViernes = dr.GetDecimal(postMontoViernes);
                                objReporteCajaDetalle.MontoSabado = dr.GetDecimal(postMontoSabado);
                                objReporteCajaDetalle.MontoDomingo = dr.GetDecimal(postMontoDomingo);
                                objReporteCajaDetalle.Estado = (byte)dr.GetInt32(postEstado);
                                objReporteCajaDetalle.TotalSemana = dr.GetDecimal(postTotalSemana);
                                objReporteCajaDetalle.Devoluciones = dr.GetDecimal(postDevoluciones);
                                objReporteCajaDetalle.Acumulado = dr.GetDecimal(postAcumulado);
                                objReporteCajaDetalle.Observaciones = dr.IsDBNull(postObservaciones) ? "" : dr.GetString(postObservaciones);

                                listaGastosIndirectos.Add(objReporteCajaDetalle);
                            }// fin while
                            objReporteCaja.listaGastosIndirectos = listaGastosIndirectos;

                        }// fin if

                        if (dr.NextResult())
                        {// Concepto: Gastos Administrativos
                            List<ReporteCajaDetalleCLS> listaGastosAdministrativos = new List<ReporteCajaDetalleCLS>();
                            ReporteCajaDetalleCLS objReporteCajaDetalle;
                            while (dr.Read())
                            {
                                objReporteCajaDetalle = new ReporteCajaDetalleCLS();
                                objReporteCajaDetalle.CodigoDetalleReporte = (long)dr.GetInt32(postCodigoDetalleReporte);
                                objReporteCajaDetalle.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objReporteCajaDetalle.CodigoConcepto = dr.GetInt16(postCodigoConcepto);
                                objReporteCajaDetalle.Concepto = dr.GetString(postConcepto);
                                objReporteCajaDetalle.CodigoOperacion = (short)dr.GetInt32(postCodigoOperacion);
                                objReporteCajaDetalle.CodigoCategoriaEntidad = (short)dr.GetInt32(postCodigoCategoriaEntidad);
                                objReporteCajaDetalle.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objReporteCajaDetalle.NombreEntidad = dr.GetString(postNombreEntidad);
                                objReporteCajaDetalle.CodigoTransaccion = dr.IsDBNull(postCodigoTransaccion) ? null : dr.GetInt64(postCodigoTransaccion);
                                objReporteCajaDetalle.SaldoAnterior = dr.GetDecimal(postSaldoAnterior);
                                objReporteCajaDetalle.MontoLunes = dr.GetDecimal(postMontoLunes);
                                objReporteCajaDetalle.MontoMartes = dr.GetDecimal(postMontoMartes);
                                objReporteCajaDetalle.MontoMiercoles = dr.GetDecimal(postMontoMiercoles);
                                objReporteCajaDetalle.MontoJueves = dr.GetDecimal(postMontoJueves);
                                objReporteCajaDetalle.MontoViernes = dr.GetDecimal(postMontoViernes);
                                objReporteCajaDetalle.MontoSabado = dr.GetDecimal(postMontoSabado);
                                objReporteCajaDetalle.MontoDomingo = dr.GetDecimal(postMontoDomingo);
                                objReporteCajaDetalle.Estado = (byte)dr.GetInt32(postEstado);
                                objReporteCajaDetalle.TotalSemana = dr.GetDecimal(postTotalSemana);
                                objReporteCajaDetalle.Devoluciones = dr.GetDecimal(postDevoluciones);
                                objReporteCajaDetalle.Acumulado = dr.GetDecimal(postAcumulado);
                                objReporteCajaDetalle.Observaciones = dr.IsDBNull(postObservaciones) ? "" : dr.GetString(postObservaciones);

                                listaGastosAdministrativos.Add(objReporteCajaDetalle);
                            }// fin while
                            objReporteCaja.listaGastosAdministrativos = listaGastosAdministrativos;

                        }// fin if


                        if (dr.NextResult())
                        {// Concepto: Mantenimiento de Vehiculos
                            List<ReporteCajaDetalleCLS> listaMantenimientoVehiculos = new List<ReporteCajaDetalleCLS>();
                            ReporteCajaDetalleCLS objReporteCajaDetalle;
                            while (dr.Read())
                            {
                                objReporteCajaDetalle = new ReporteCajaDetalleCLS();
                                objReporteCajaDetalle.CodigoDetalleReporte = dr.GetInt64(postCodigoDetalleReporte);
                                objReporteCajaDetalle.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objReporteCajaDetalle.CodigoConcepto = dr.GetInt16(postCodigoConcepto);
                                objReporteCajaDetalle.Concepto = dr.GetString(postConcepto);
                                objReporteCajaDetalle.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objReporteCajaDetalle.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objReporteCajaDetalle.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objReporteCajaDetalle.NombreEntidad = dr.GetString(postNombreEntidad);
                                objReporteCajaDetalle.CodigoTransaccion = dr.IsDBNull(postCodigoTransaccion) ? null : dr.GetInt64(postCodigoTransaccion);
                                objReporteCajaDetalle.SaldoAnterior = dr.GetDecimal(postSaldoAnterior);
                                objReporteCajaDetalle.MontoLunes = dr.GetDecimal(postMontoLunes);
                                objReporteCajaDetalle.MontoMartes = dr.GetDecimal(postMontoMartes);
                                objReporteCajaDetalle.MontoMiercoles = dr.GetDecimal(postMontoMiercoles);
                                objReporteCajaDetalle.MontoJueves = dr.GetDecimal(postMontoJueves);
                                objReporteCajaDetalle.MontoViernes = dr.GetDecimal(postMontoViernes);
                                objReporteCajaDetalle.MontoSabado = dr.GetDecimal(postMontoSabado);
                                objReporteCajaDetalle.MontoDomingo = dr.GetDecimal(postMontoDomingo);
                                objReporteCajaDetalle.Estado = dr.GetByte(postEstado);
                                objReporteCajaDetalle.TotalSemana = dr.GetDecimal(postTotalSemana);
                                objReporteCajaDetalle.Devoluciones = dr.GetDecimal(postDevoluciones);
                                objReporteCajaDetalle.Acumulado = dr.GetDecimal(postAcumulado);
                                objReporteCajaDetalle.Observaciones = dr.IsDBNull(postObservaciones) ? "" : dr.GetString(postObservaciones);

                                listaMantenimientoVehiculos.Add(objReporteCajaDetalle);
                            }// fin while
                            objReporteCaja.listaMantenimientoVehiculos = listaMantenimientoVehiculos;

                        }// fin if

                        if (dr.NextResult())
                        {// Concepto: Depósitos Bancarios
                            List<ReporteCajaDetalleCLS> listaDepositosBancarios = new List<ReporteCajaDetalleCLS>();
                            ReporteCajaDetalleCLS objReporteCajaDetalle;
                            while (dr.Read())
                            {
                                objReporteCajaDetalle = new ReporteCajaDetalleCLS();
                                objReporteCajaDetalle.CodigoDetalleReporte = dr.GetInt64(postCodigoDetalleReporte);
                                objReporteCajaDetalle.CodigoReporte = dr.GetInt32(postCodigoReporte);
                                objReporteCajaDetalle.CodigoConcepto = dr.GetInt16(postCodigoConcepto);
                                objReporteCajaDetalle.Concepto = dr.GetString(postConcepto);
                                objReporteCajaDetalle.CodigoOperacion = dr.GetInt16(postCodigoOperacion);
                                objReporteCajaDetalle.CodigoCategoriaEntidad = dr.GetInt16(postCodigoCategoriaEntidad);
                                objReporteCajaDetalle.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objReporteCajaDetalle.NombreEntidad = dr.GetString(postNombreEntidad);
                                objReporteCajaDetalle.CodigoTransaccion = dr.IsDBNull(postCodigoTransaccion) ? null : dr.GetInt64(postCodigoTransaccion);
                                objReporteCajaDetalle.SaldoAnterior = dr.GetDecimal(postSaldoAnterior);
                                objReporteCajaDetalle.MontoLunes = dr.GetDecimal(postMontoLunes);
                                objReporteCajaDetalle.MontoMartes = dr.GetDecimal(postMontoMartes);
                                objReporteCajaDetalle.MontoMiercoles = dr.GetDecimal(postMontoMiercoles);
                                objReporteCajaDetalle.MontoJueves = dr.GetDecimal(postMontoJueves);
                                objReporteCajaDetalle.MontoViernes = dr.GetDecimal(postMontoViernes);
                                objReporteCajaDetalle.MontoSabado = dr.GetDecimal(postMontoSabado);
                                objReporteCajaDetalle.MontoDomingo = dr.GetDecimal(postMontoDomingo);
                                objReporteCajaDetalle.Estado = dr.GetByte(postEstado);
                                objReporteCajaDetalle.TotalSemana = dr.GetDecimal(postTotalSemana);
                                objReporteCajaDetalle.Devoluciones = dr.GetDecimal(postDevoluciones);
                                objReporteCajaDetalle.Acumulado = dr.GetDecimal(postAcumulado);
                                objReporteCajaDetalle.Observaciones = dr.IsDBNull(postObservaciones) ? "" : dr.GetString(postObservaciones);

                                listaDepositosBancarios.Add(objReporteCajaDetalle);
                            }// fin while
                            objReporteCaja.listaDepositosBancarios = listaDepositosBancarios;

                        }// fin if
                    }
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                    objReporteCaja = null;
                }

                return objReporteCaja;
            }
        }


        public ReporteCajaDetalleListCLS GetDetalleReporteCaja(int anioOperacion, int semanaOperacion, int codigoReporte, int codigoTipoReporte)
        {
            int postCodigoConcepto = 0;
            int postConcepto = 0;
            int postCodigoOperacion = 0;
            int postCodigoCategoriaEntidad = 0;
            int postCodigoEntidad = 0;
            int postNombreEntidad = 0;
            int postSaldoAnterior = 0;
            int postMontoLunes = 0;
            int postMontoMartes = 0;
            int postMontoMiercoles = 0;
            int postMontoJueves = 0;
            int postMontoViernes = 0;
            int postMontoSabado = 0;
            int postMontoDomingo = 0;
            int postDevoluciones = 0;
            int postObservaciones = 0;
            ReporteCajaDetalleListCLS objReporteCaja = new ReporteCajaDetalleListCLS();
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    string sqlSp = string.Empty;
                    if (codigoTipoReporte == 0)
                    {
                        sqlSp = "db_tesoreria.uspGetReporteSemanalDonPepe";
                    }
                    else {
                        sqlSp = "db_tesoreria.uspGetReporteSemanalEnProcesoDonPepe";
                    }
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand(sqlSp, conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Anio", anioOperacion);
                        cmd.Parameters.AddWithValue("@NumeroSemana", semanaOperacion);
                        cmd.Parameters.AddWithValue("@CodigoReporte", codigoReporte);
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr != null)
                        {// Encabezado de las fechas de la semana indicada
                            int postFechaStr = dr.GetOrdinal("fecha_str");
                            List<ProgramacionSemanalCLS> listaEncabezadoFechas = new List<ProgramacionSemanalCLS>();
                            ProgramacionSemanalCLS objEncabezadoFechas;
                            while (dr.Read())
                            {
                                objEncabezadoFechas = new ProgramacionSemanalCLS();
                                objEncabezadoFechas.FechaStr = dr.GetString(postFechaStr);
                                listaEncabezadoFechas.Add(objEncabezadoFechas);
                            }
                            objReporteCaja.listaEncabezadoFechas = listaEncabezadoFechas;
                        }

                        if (dr.NextResult())
                        {// Concepto: Vendedores
                            postCodigoConcepto = dr.GetOrdinal("codigo_concepto");
                            postConcepto = dr.GetOrdinal("concepto");
                            postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            postCodigoCategoriaEntidad = dr.GetOrdinal("codigo_categoria_entidad");
                            postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            postNombreEntidad = dr.GetOrdinal("nombre_entidad");
                            postSaldoAnterior = dr.GetOrdinal("saldo_anterior");
                            postMontoLunes = dr.GetOrdinal("monto_lunes");
                            postMontoMartes = dr.GetOrdinal("monto_martes");
                            postMontoMiercoles = dr.GetOrdinal("monto_miercoles");
                            postMontoJueves = dr.GetOrdinal("monto_jueves");
                            postMontoViernes = dr.GetOrdinal("monto_viernes");
                            postMontoSabado = dr.GetOrdinal("monto_sabado");
                            postMontoDomingo = dr.GetOrdinal("monto_domingo");
                            postDevoluciones = dr.GetOrdinal("monto_devoluciones");
                            postObservaciones = dr.GetOrdinal("observaciones");

                            List<ReporteCajaDetalleCLS> listaVendedores = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaEspeciales1 = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaEspeciales2 = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaCajas = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaCombustibleCarros = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaPlanillas = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaSueldosIndirectos = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaLiquidaciones = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaVacaciones = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaBonoFinDeMes = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaBonosExtras = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaBono14 = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaAguinaldos = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaPrestamos = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaAnticipos = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaMateriaPrima = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaGastosIndirectos = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaGastosAdministrativos = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaMantenimientoVehiculos = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaDepositosBancarios = new List<ReporteCajaDetalleCLS>();

                            ReporteCajaDetalleCLS objReporteCajaDetalle;
                            while (dr.Read())
                            {
                                objReporteCajaDetalle = new ReporteCajaDetalleCLS();
                                objReporteCajaDetalle.CodigoConcepto = (short)dr.GetInt32(postCodigoConcepto);
                                objReporteCajaDetalle.Concepto = dr.GetString(postConcepto);
                                objReporteCajaDetalle.CodigoOperacion = (short) dr.GetInt32(postCodigoOperacion);
                                objReporteCajaDetalle.CodigoCategoriaEntidad = (short)dr.GetInt32(postCodigoCategoriaEntidad);
                                objReporteCajaDetalle.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objReporteCajaDetalle.NombreEntidad = dr.IsDBNull(postNombreEntidad) ? "Sin Nombre" : dr.GetString(postNombreEntidad);
                                objReporteCajaDetalle.SaldoAnterior = dr.GetDecimal(postSaldoAnterior);
                                objReporteCajaDetalle.MontoLunes = dr.GetDecimal(postMontoLunes);
                                objReporteCajaDetalle.MontoMartes = dr.GetDecimal(postMontoMartes);
                                objReporteCajaDetalle.MontoMiercoles = dr.GetDecimal(postMontoMiercoles);
                                objReporteCajaDetalle.MontoJueves = dr.GetDecimal(postMontoJueves);
                                objReporteCajaDetalle.MontoViernes = dr.GetDecimal(postMontoViernes);
                                objReporteCajaDetalle.MontoSabado = dr.GetDecimal(postMontoSabado);
                                objReporteCajaDetalle.MontoDomingo = dr.GetDecimal(postMontoDomingo);
                                objReporteCajaDetalle.TotalSemana = (dr.GetDecimal(postMontoLunes) == 0.01m ? 0 : dr.GetDecimal(postMontoLunes)) +  (dr.GetDecimal(postMontoMartes) == 0.01m ? 0 : dr.GetDecimal(postMontoMartes)) +  (dr.GetDecimal(postMontoMiercoles) == 0.01m ? 0 : dr.GetDecimal(postMontoMiercoles)) + (dr.GetDecimal(postMontoJueves) == 0.01m ? 0 : dr.GetDecimal(postMontoJueves)) + (dr.GetDecimal(postMontoViernes) == 0.01m ? 0: dr.GetDecimal(postMontoViernes)) + (dr.GetDecimal(postMontoSabado) == 0.01m ? 0 : dr.GetDecimal(postMontoSabado)) + (dr.GetDecimal(postMontoDomingo) == 0.01m ? 0 : dr.GetDecimal(postMontoDomingo)) + dr.GetDecimal(postDevoluciones);
                                objReporteCajaDetalle.Devoluciones = dr.GetDecimal(postDevoluciones);
                                objReporteCajaDetalle.Acumulado = dr.GetDecimal(postSaldoAnterior) + (dr.GetDecimal(postMontoLunes) == 0.01m ? 0 : dr.GetDecimal(postMontoLunes)) + (dr.GetDecimal(postMontoMartes) == 0.01m ? 0 : dr.GetDecimal(postMontoMartes)) + (dr.GetDecimal(postMontoMiercoles) == 0.01m ? 0 : dr.GetDecimal(postMontoMiercoles)) + (dr.GetDecimal(postMontoJueves) == 0.01m ? 0 : dr.GetDecimal(postMontoJueves)) + (dr.GetDecimal(postMontoViernes) == 0.01m ? 0 : dr.GetDecimal(postMontoViernes)) + (dr.GetDecimal(postMontoSabado) == 0.01m ? 0 : dr.GetDecimal(postMontoSabado)) + (dr.GetDecimal(postMontoDomingo) == 0.01m ? 0 : dr.GetDecimal(postMontoDomingo));
                                objReporteCajaDetalle.Observaciones = dr.IsDBNull(postObservaciones) ? "" : dr.GetString(postObservaciones);

                                switch (objReporteCajaDetalle.CodigoConcepto) {
                                    case Constantes.Concepto.RUTEROS:
                                        listaVendedores.Add(objReporteCajaDetalle);
                                        break;
                                    case Constantes.Concepto.RUTEROS_INTERIOR:
                                        listaVendedores.Add(objReporteCajaDetalle);
                                        break;
                                    case Constantes.Concepto.VENDEDORES:
                                        listaVendedores.Add(objReporteCajaDetalle);
                                        break;
                                    case Constantes.Concepto.CAFETERIAS:
                                        listaVendedores.Add(objReporteCajaDetalle);
                                        break;
                                    case Constantes.Concepto.SUPERMERCADOS:
                                        listaVendedores.Add(objReporteCajaDetalle);
                                        break;
                                    case Constantes.Concepto.ESPECIALES_1:
                                        listaEspeciales1.Add(objReporteCajaDetalle);
                                        break;
                                    case Constantes.Concepto.ESPECIALES_2:
                                        listaEspeciales2.Add(objReporteCajaDetalle);
                                        break;
                                    case Constantes.Concepto.CAJAS:
                                        listaCajas.Add(objReporteCajaDetalle);
                                        break;
                                    case Constantes.Concepto.COMBUSTIBLES_CARROS:
                                        listaCombustibleCarros.Add(objReporteCajaDetalle);
                                        break;
                                    case Constantes.Concepto.PLANILLAS:
                                        listaPlanillas.Add(objReporteCajaDetalle);
                                        break;
                                    case Constantes.Concepto.SUELDOS_INDIRECTOS:
                                        listaSueldosIndirectos.Add(objReporteCajaDetalle);
                                        break;
                                    case Constantes.Concepto.LIQUIDACIONES:
                                        listaLiquidaciones.Add(objReporteCajaDetalle);
                                        break;
                                    case Constantes.Concepto.VACACIONES:
                                        listaVacaciones.Add(objReporteCajaDetalle);
                                        break;
                                    case Constantes.Concepto.BONO_DE_FIN_DE_MES:
                                        listaBonoFinDeMes.Add(objReporteCajaDetalle);
                                        break;
                                    case Constantes.Concepto.BONOS_EXTRAS:
                                        listaBonosExtras.Add(objReporteCajaDetalle);
                                        break;
                                    case Constantes.Concepto.BONO14:
                                        listaBono14.Add(objReporteCajaDetalle);
                                        break;
                                    case Constantes.Concepto.AGUINALDOS:
                                        listaAguinaldos.Add(objReporteCajaDetalle);
                                        break;
                                    case Constantes.Concepto.PRESTAMOS:
                                        listaPrestamos.Add(objReporteCajaDetalle);
                                        break;
                                    case Constantes.Concepto.ANTICIPIOS:
                                        listaAnticipos.Add(objReporteCajaDetalle);
                                        break;
                                    case Constantes.Concepto.MATERIAS_PRIMAS:
                                        listaMateriaPrima.Add(objReporteCajaDetalle);
                                        break;
                                    case Constantes.Concepto.GASTOS_INDIRECTOS:
                                        listaGastosIndirectos.Add(objReporteCajaDetalle);
                                        break;
                                    case Constantes.Concepto.GASTOS_ADMINISTRATIVOS:
                                        listaGastosAdministrativos.Add(objReporteCajaDetalle);
                                        break;
                                    case Constantes.Concepto.MANTIMIENTO_DE_VEHICULOS:
                                        listaMantenimientoVehiculos.Add(objReporteCajaDetalle);
                                        break;
                                    case Constantes.Concepto.DEPOSITOS_BANCARIOS:
                                        listaDepositosBancarios.Add(objReporteCajaDetalle);
                                        break;

                                    default:
                                        break;

                                }
 
                            }// fin while

                            objReporteCaja.listaVendedores = listaVendedores;
                            objReporteCaja.listaEspecial1 = listaEspeciales1;
                            objReporteCaja.listaEspecial2 = listaEspeciales2;
                            objReporteCaja.listaCajas = listaCajas;
                            objReporteCaja.listaCombustibleCarros = listaCombustibleCarros;
                            objReporteCaja.listaPlanillas = listaPlanillas;
                            objReporteCaja.listaSueldosIndirectos = listaSueldosIndirectos;
                            objReporteCaja.listaLiquidaciones = listaLiquidaciones;
                            objReporteCaja.listaVacaciones = listaVacaciones;
                            objReporteCaja.listaBonoFinDeMes = listaBonoFinDeMes;
                            objReporteCaja.listaBonosExtras = listaBonosExtras;
                            objReporteCaja.listaBono14 = listaBono14;
                            objReporteCaja.listaAguinaldos = listaAguinaldos;
                            objReporteCaja.listaPrestamos = listaPrestamos.OrderBy(x => x.NombreEntidad).ToList();
                            objReporteCaja.listaAnticipos = listaAnticipos.OrderBy(x => x.NombreEntidad).ToList();
                            objReporteCaja.listaMateriaPrima = listaMateriaPrima.OrderBy(x => x.NombreEntidad).ToList();
                            objReporteCaja.listaGastosIndirectos = listaGastosIndirectos.OrderBy(x => x.NombreEntidad).ToList();
                            objReporteCaja.listaGastosAdministrativos = listaGastosAdministrativos.OrderBy(x => x.NombreEntidad).ToList();
                            objReporteCaja.listaMantenimientoVehiculos = listaMantenimientoVehiculos.OrderBy(x => x.NombreEntidad).ToList();
                            objReporteCaja.listaDepositosBancarios = listaDepositosBancarios;
                        }

                    }
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                    objReporteCaja = null;
                }

                return objReporteCaja;
            }
        }

        public ReporteCajaDetalleListCLS GetDetalleResumenVentasPagoPlanilaNF(int anioOperacion, int semanaOperacion, int codigoReporte)
        {
            int postCodigoConcepto = 0;
            int postConcepto = 0;
            int postCodigoOperacion = 0;
            int postCodigoCategoriaEntidad = 0;
            int postCodigoEntidad = 0;
            int postNombreEntidad = 0;
            int postSaldoAnterior = 0;
            int postMontoLunes = 0;
            int postMontoMartes = 0;
            int postMontoMiercoles = 0;
            int postMontoJueves = 0;
            int postMontoViernes = 0;
            int postMontoSabado = 0;
            int postMontoDomingo = 0;
            int postDevoluciones = 0;
            int postObservaciones = 0;
            ReporteCajaDetalleListCLS objReporteCaja = new ReporteCajaDetalleListCLS();
            using (SqlConnection conexion = new SqlConnection(cadenaTesoreria))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand cmd = new SqlCommand("db_tesoreria.uspGetReporteResumenPagoPlanillaNF", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Anio", anioOperacion);
                        cmd.Parameters.AddWithValue("@NumeroSemana", semanaOperacion);
                        cmd.Parameters.AddWithValue("@CodigoReporte", codigoReporte);
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr != null)
                        {// Encabezado de las fechas de la semana indicada
                            int postFechaStr = dr.GetOrdinal("fecha_str");
                            List<ProgramacionSemanalCLS> listaEncabezadoFechas = new List<ProgramacionSemanalCLS>();
                            ProgramacionSemanalCLS objEncabezadoFechas;
                            while (dr.Read())
                            {
                                objEncabezadoFechas = new ProgramacionSemanalCLS();
                                objEncabezadoFechas.FechaStr = dr.GetString(postFechaStr);
                                listaEncabezadoFechas.Add(objEncabezadoFechas);
                            }
                            objReporteCaja.listaEncabezadoFechas = listaEncabezadoFechas;
                        }

                        if (dr.NextResult())
                        {// Concepto: Vendedores
                            postCodigoConcepto = dr.GetOrdinal("codigo_concepto");
                            postConcepto = dr.GetOrdinal("concepto");
                            postCodigoOperacion = dr.GetOrdinal("codigo_operacion");
                            postCodigoCategoriaEntidad = dr.GetOrdinal("codigo_categoria_entidad");
                            postCodigoEntidad = dr.GetOrdinal("codigo_entidad");
                            postNombreEntidad = dr.GetOrdinal("nombre_entidad");
                            postSaldoAnterior = dr.GetOrdinal("saldo_anterior");
                            postMontoLunes = dr.GetOrdinal("monto_lunes");
                            postMontoMartes = dr.GetOrdinal("monto_martes");
                            postMontoMiercoles = dr.GetOrdinal("monto_miercoles");
                            postMontoJueves = dr.GetOrdinal("monto_jueves");
                            postMontoViernes = dr.GetOrdinal("monto_viernes");
                            postMontoSabado = dr.GetOrdinal("monto_sabado");
                            postMontoDomingo = dr.GetOrdinal("monto_domingo");
                            postDevoluciones = dr.GetOrdinal("monto_devoluciones");
                            postObservaciones = dr.GetOrdinal("observaciones");

                            List<ReporteCajaDetalleCLS> listaVendedores = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaEspeciales1 = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaEspeciales2 = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaCajas = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaCombustibleCarros = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaPlanillas = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaSueldosIndirectos = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaLiquidaciones = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaVacaciones = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaBonoFinDeMes = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaBonosExtras = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaBono14 = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaAguinaldos = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaPrestamos = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaAnticipos = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaMateriaPrima = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaGastosIndirectos = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaGastosAdministrativos = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaMantenimientoVehiculos = new List<ReporteCajaDetalleCLS>();
                            List<ReporteCajaDetalleCLS> listaDepositosBancarios = new List<ReporteCajaDetalleCLS>();

                            ReporteCajaDetalleCLS objReporteCajaDetalle;
                            while (dr.Read())
                            {
                                objReporteCajaDetalle = new ReporteCajaDetalleCLS();
                                objReporteCajaDetalle.CodigoConcepto = (short)dr.GetInt32(postCodigoConcepto);
                                objReporteCajaDetalle.Concepto = dr.GetString(postConcepto);
                                objReporteCajaDetalle.CodigoOperacion = (short)dr.GetInt32(postCodigoOperacion);
                                objReporteCajaDetalle.CodigoCategoriaEntidad = (short)dr.GetInt32(postCodigoCategoriaEntidad);
                                objReporteCajaDetalle.CodigoEntidad = dr.GetString(postCodigoEntidad);
                                objReporteCajaDetalle.NombreEntidad = dr.IsDBNull(postNombreEntidad) ? "Sin Nombre" : dr.GetString(postNombreEntidad);
                                objReporteCajaDetalle.SaldoAnterior = dr.GetDecimal(postSaldoAnterior);
                                objReporteCajaDetalle.MontoLunes = dr.GetDecimal(postMontoLunes);
                                objReporteCajaDetalle.MontoMartes = dr.GetDecimal(postMontoMartes);
                                objReporteCajaDetalle.MontoMiercoles = dr.GetDecimal(postMontoMiercoles);
                                objReporteCajaDetalle.MontoJueves = dr.GetDecimal(postMontoJueves);
                                objReporteCajaDetalle.MontoViernes = dr.GetDecimal(postMontoViernes);
                                objReporteCajaDetalle.MontoSabado = dr.GetDecimal(postMontoSabado);
                                objReporteCajaDetalle.MontoDomingo = dr.GetDecimal(postMontoDomingo);
                                objReporteCajaDetalle.TotalSemana = dr.GetDecimal(postMontoLunes) + dr.GetDecimal(postMontoMartes) + dr.GetDecimal(postMontoMiercoles) + dr.GetDecimal(postMontoJueves) + dr.GetDecimal(postMontoViernes) + dr.GetDecimal(postMontoSabado) + dr.GetDecimal(postMontoDomingo) + dr.GetDecimal(postDevoluciones);
                                objReporteCajaDetalle.Devoluciones = dr.GetDecimal(postDevoluciones);
                                objReporteCajaDetalle.Acumulado = dr.GetDecimal(postSaldoAnterior) + dr.GetDecimal(postMontoLunes) + dr.GetDecimal(postMontoMartes) + dr.GetDecimal(postMontoMiercoles) + dr.GetDecimal(postMontoJueves) + dr.GetDecimal(postMontoViernes) + dr.GetDecimal(postMontoSabado) + dr.GetDecimal(postMontoDomingo);
                                objReporteCajaDetalle.Observaciones = dr.IsDBNull(postObservaciones) ? "" : dr.GetString(postObservaciones);

                                switch (objReporteCajaDetalle.CodigoConcepto)
                                {
                                    case Constantes.Concepto.RUTEROS:
                                        listaVendedores.Add(objReporteCajaDetalle);
                                        break;
                                    case Constantes.Concepto.RUTEROS_INTERIOR:
                                        listaVendedores.Add(objReporteCajaDetalle);
                                        break;
                                    case Constantes.Concepto.VENDEDORES:
                                        listaVendedores.Add(objReporteCajaDetalle);
                                        break;
                                    case Constantes.Concepto.CAFETERIAS:
                                        listaVendedores.Add(objReporteCajaDetalle);
                                        break;
                                    case Constantes.Concepto.SUPERMERCADOS:
                                        listaVendedores.Add(objReporteCajaDetalle);
                                        break;
                                    case Constantes.Concepto.ESPECIALES_1:
                                        listaEspeciales1.Add(objReporteCajaDetalle);
                                        break;
                                    case Constantes.Concepto.ESPECIALES_2:
                                        listaEspeciales2.Add(objReporteCajaDetalle);
                                        break;
                                    case Constantes.Concepto.CAJAS:
                                        listaCajas.Add(objReporteCajaDetalle);
                                        break;
                                    default:
                                        break;

                                }

                            }// fin while

                            objReporteCaja.listaVendedores = listaVendedores;
                            objReporteCaja.listaEspecial1 = listaEspeciales1;
                            objReporteCaja.listaEspecial2 = listaEspeciales2;
                            objReporteCaja.listaCajas = listaCajas;
                        }

                    }
                    conexion.Close();
                }
                catch (Exception)
                {
                    conexion.Close();
                    objReporteCaja = null;
                }

                return objReporteCaja;
            }
        }

    }
}
