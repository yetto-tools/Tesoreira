using CapaDatos.Tesoreria;
using CapaEntidad.Administracion;
using CapaEntidad.Tesoreria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Tesoreria
{
    public class TransaccionBL
    {

        public List<TransaccionCLS> BuscarTransacciones(string idUsuario, int codigoTipoOperacion, int codigoOperacion, int codigoCategoriaEntidad, int diaOperacion, int esSuperAdmin, int setSemanAnterior)
        {
            TransaccionDAL obj = new TransaccionDAL();
            return obj.BuscarTransacciones(idUsuario, codigoTipoOperacion, codigoOperacion, codigoCategoriaEntidad, diaOperacion, esSuperAdmin, setSemanAnterior);
        }

        /// <summary>
        /// temporal, solo para validar la edicion de las diferentes operaciones
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="codigoOperacion"></param>
        /// <param name="codigoCategoriaEntidad"></param>
        /// <returns></returns>
        //public List<TransaccionCLS> BuscarTransaccionesEdicion(string idUsuario, int codigoOperacion, int codigoCategoriaEntidad)
        //{
        //    TransaccionDAL obj = new TransaccionDAL();
        //    return obj.BuscarTransaccionesEdicion(idUsuario, codigoOperacion, codigoCategoriaEntidad);
        //}

        public List<TransaccionCLS> BuscarTransaccionesConsulta(int anioOperacion, int semanaOperacion, int codigoReporte, int codigoOperacion, int codigoCategoriaEntidad, int diaOperacion)
        {
            TransaccionDAL obj = new TransaccionDAL();
            return obj.BuscarTransaccionesConsulta(anioOperacion, semanaOperacion, codigoReporte, codigoOperacion, codigoCategoriaEntidad, diaOperacion);
        }

        public List<TransaccionCLS> BuscarTransaccionesConsultaContabilidad(int anioOperacion, int semanaOperacion, int codigoTipoOperacion, int codigoOperacion, int codigoCategoriaEntidad, string nombreEntidad, string fechaInicioStr, string fechaFinStr)
        {
            TransaccionDAL obj = new TransaccionDAL();
            return obj.BuscarTransaccionesConsultaContabilidad(anioOperacion, semanaOperacion, codigoTipoOperacion, codigoOperacion, codigoCategoriaEntidad, nombreEntidad, fechaInicioStr, fechaFinStr);
        }

        public List<TransaccionCLS> BuscarTransaccionesParaCorreccion(int anioOperacion, int semanaOperacion, int codigoReporte, int codigoOperacion, int codigoCategoriaEntidad, int esSuperAdmin)
        {
            TransaccionDAL obj = new TransaccionDAL();
            return obj.BuscarTransaccionesParaCorreccion(anioOperacion, semanaOperacion, codigoReporte, codigoOperacion, codigoCategoriaEntidad, esSuperAdmin);
        }
        public List<TransaccionCLS> BuscarTransaccionesDepositosBancarios(int anioOperacion, int semanaOperacion, int codigoReporte, int esSuperAdmin)
        {
            TransaccionDAL obj = new TransaccionDAL();
            return obj.BuscarTransaccionesDepositosBancarios(anioOperacion, semanaOperacion, codigoReporte, esSuperAdmin);
        }

        /// <summary>
        /// Obtener transacciones gasto para completar la empresa asignada al gasto, esto se realizar en contabilidad
        /// </summary>
        /// <param name="anioOperacion"></param>
        /// <param name="semanaOperacion"></param>
        /// <param name="codigoReporte"></param>
        /// <param name="esSuperAdmin"></param>
        /// <returns></returns>
        public List<TransaccionCLS> BuscarTransaccionesGasto(int anioOperacion, int semanaOperacion, int codigoReporte, int esSuperAdmin)
        {
            TransaccionDAL obj = new TransaccionDAL();
            return obj.BuscarTransaccionesGasto(anioOperacion, semanaOperacion, codigoReporte, esSuperAdmin);
        }

        public List<TransaccionCLS> GetSolicitudesDeCorreccion(int anioOperacion, int semanaOperacion, int codigoReporte, int codigoOperacion, int codigoCategoriaEntidad, int esSuperAdmin)
        {
            TransaccionDAL obj = new TransaccionDAL();
            return obj.GetSolicitudesDeCorreccion(anioOperacion, semanaOperacion, codigoReporte, codigoOperacion, codigoCategoriaEntidad, esSuperAdmin);
        }

        public string AutorizarCorreccion(long codigoTransaccion, string observaciones, int codigoResultado, string usuarioAct)
        {
            TransaccionDAL obj = new TransaccionDAL();
            return obj.AutorizarCorreccion(codigoTransaccion, observaciones, codigoResultado, usuarioAct);
        }

        public List<TransaccionCLS> BuscarTransaccionesDesglocePagoPlanillas()
        {
            TransaccionDAL obj = new TransaccionDAL();
            return obj.BuscarTransaccionesDesglocePagoPlanillas();
        }

        public List<TransaccionCLS> BuscarTransaccionesReporteFacturadoAlContado()
        {
            TransaccionDAL obj = new TransaccionDAL();
            return obj.BuscarTransaccionesReporteFacturadoAlContado();
        }

        public TransaccionCLS GetDataTransaccion(long codigoTransaccion)
        {
            TransaccionDAL obj = new TransaccionDAL();
            return obj.GetDataTransaccion(codigoTransaccion);
        }
        public TransaccionComboCLS FillCombosNuevaTransaccion(int codigoTipoOperacion, string idUsuario)
        {
            TransaccionDAL obj = new TransaccionDAL();
            return obj.fillCombosNewTransaccion(codigoTipoOperacion, idUsuario);
        }

        public TransaccionComboCLS FillCombosEditTransaccion(int codigoTipoOperacion, int semanaOperacion, int anioOperacion)
        {
            TransaccionDAL obj = new TransaccionDAL();
            return obj.FillCombosEditTransaccion(codigoTipoOperacion, semanaOperacion, anioOperacion);
        }

        public TransaccionComboCLS FillCombosConsultaTransacciones()
        {
            TransaccionDAL obj = new TransaccionDAL();
            return obj.FillCombosConsultaTransacciones();
        }

        public TransaccionComboCLS FillComboSemana(int habilitarSemanaAnterior)
        {
            TransaccionDAL obj = new TransaccionDAL();
            return obj.fillComboSemana(habilitarSemanaAnterior);
        }

        public string GuardarTransaccion(TransaccionCLS objTransaccion, string usuarioIng, int complemento)
        {
            TransaccionDAL obj = new TransaccionDAL();
            if (objTransaccion.CodigoOperacionCaja == 0)
                objTransaccion.CodigoOperacionCaja = objTransaccion.CodigoOperacion;
            if (objTransaccion.CodigoOperacion == Constantes.Operacion.Ingreso.OTRAS_VENTAS)
                objTransaccion.CodigoOperacionCaja = Constantes.Operacion.Ingreso.ESPECIALES_1;
            if (objTransaccion.NitEmpresaConcedeIva == "1000")
                objTransaccion.NitEmpresaConcedeIva = null;
            if (objTransaccion.CodigoEmpresa == -1)
                objTransaccion.CodigoEmpresa = null;
            if (objTransaccion.CodigoOperacion != Constantes.Operacion.Ingreso.VENTAS_EN_RUTA)
            {
                objTransaccion.CodigoOperacionCaja = objTransaccion.CodigoOperacion;
            }
            else {
                if (objTransaccion.RutaVendedor != -1)
                {
                    objTransaccion.Ruta = objTransaccion.RutaVendedor;
                }
            }

            if (objTransaccion.CodigoOperacion == Constantes.Operacion.Egreso.DEPOSITOS_BANCARIOS)
            {
                switch (objTransaccion.CodigoTipoDocumentoDeposito)
                {
                    case Constantes.TipoDocumentoDeposito.NUMERO_BOLETA:
                        objTransaccion.NumeroBoleta = objTransaccion.NumeroBoleta;
                        objTransaccion.NumeroVoucher = String.Empty;
                        break;
                    case Constantes.TipoDocumentoDeposito.NUMERO_VOUCHER:
                        objTransaccion.NumeroVoucher = objTransaccion.NumeroBoleta;
                        objTransaccion.NumeroBoleta = String.Empty;
                        break;
                    default:
                        break;
                }
            }
            else {
                objTransaccion.NumeroBoleta = String.Empty;
                objTransaccion.NumeroVoucher = String.Empty;
            }

            if (objTransaccion.NumeroCuenta == "-1")
                objTransaccion.NumeroCuenta = null;
            if (objTransaccion.CodigoBancoDeposito == -1)
                objTransaccion.CodigoBancoDeposito = null;

            // Planilla
            if (objTransaccion.AnioPlanilla == -1)
            {
                objTransaccion.AnioPlanilla = 0;
                objTransaccion.CodigoQuincenaPlanilla = 0;
            }

            if (objTransaccion.CodigoQuincenaPlanilla == -1)
            {
                objTransaccion.CodigoQuincenaPlanilla = 0;
            }

            // Bonos Extras
            if (objTransaccion.CodigoOperacion != Constantes.Operacion.Egreso.PLANILLA_BONOS_EXTRAS_COMISION)
                objTransaccion.AnioComision = 0;
            else
            {
                switch (objTransaccion.CodigoBonoExtra)
                {
                    case Constantes.BonoExtra.POR_COMISIONES:
                        break;
                    case Constantes.BonoExtra.POR_QUINTALAJE:
                        objTransaccion.AnioComision = 0;
                        objTransaccion.CodigoOperacionCaja = Constantes.Operacion.Egreso.PLANILLA_BONOS_EXTRAS_QUINTALAJE;
                        break;
                    case Constantes.BonoExtra.FERIADOS_Y_DOMINGOS:
                        objTransaccion.AnioComision = 0;
                        objTransaccion.CodigoOperacionCaja = Constantes.Operacion.Egreso.PLANILLA_BONOS_EXTRAS_FERIADOS_Y_DOMINGOS;
                        break;
                    case Constantes.BonoExtra.OTROS:
                        objTransaccion.AnioComision = 0;
                        objTransaccion.CodigoOperacionCaja = Constantes.Operacion.Egreso.PLANILLA_BONOS_EXTRAS_OTROS;
                        break;
                    default:
                        objTransaccion.AnioComision = 0;
                        break;
                }
            }

            if (objTransaccion.CodigoOperacion == Constantes.Operacion.Egreso.PLANILLA_PAGO)
            {
                if (objTransaccion.CodigoTipoPlanilla == Constantes.Operacion.Egreso.TipoPlanilla.AJUSTE_PLANILLA) {
                    objTransaccion.CodigoOperacionCaja = Constantes.Operacion.Egreso.AJUSTE_PLANILLA;
                }
            }

            if (objTransaccion.CodigoOperacion == Constantes.Operacion.Egreso.GASTOS_INDIRECTOS)
            {
                if (objTransaccion.CodigoTipoGastoIndirecto == Constantes.Operacion.Egreso.TipoGastoIndirecto.SUELDO_INDIRECTO)
                {
                    objTransaccion.CodigoOperacionCaja = Constantes.Operacion.Egreso.SUELDOS_INDIRECTOS;
                }
            }

            objTransaccion.FechaOperacion = Util.Conversion.ConvertDateSpanishToEnglish(objTransaccion.FechaStr);
            if (complemento == 1)
            {// desgloce de planillas
                objTransaccion.CodigoCategoriaEntidad = Constantes.Entidad.Categoria.EMPLEADO;
                objTransaccion.CodigoEntidad = Constantes.Entidad.EMPLEADOS.ToString();
                objTransaccion.Efectivo = 1;
                objTransaccion.ComplementoConta = 1;
                objTransaccion.CodigoFrecuenciaPago = (byte)Constantes.FrecuenciaPago.MENSUAL;
                objTransaccion.CodigoTipoTransaccion = "F";
            }
            else {
                if (complemento == 2)
                {// Facturas al contado (Compromiso fiscal)
                    objTransaccion.CodigoCategoriaEntidad = Constantes.Entidad.Categoria.EMPRESA;
                    objTransaccion.CodigoEntidad = objTransaccion.CodigoEmpresa.ToString();
                    objTransaccion.Efectivo = 1;
                    objTransaccion.ComplementoConta = 1;
                    objTransaccion.CodigoReporte = 0;
                    objTransaccion.CodigoTipoTransaccion = "F";
                }
                else
                {
                    objTransaccion.ComplementoConta = 0;
                    objTransaccion.CodigoReporte = -1;
                }
            }

            return obj.GuardarTransaccion(objTransaccion, usuarioIng);
        }

        public string ActualizarTransaccion(TransaccionCLS objTransaccion, string usuarioAct)
        {
            TransaccionDAL obj = new TransaccionDAL();
            if (objTransaccion.CodigoOperacionCaja == 0)
                objTransaccion.CodigoOperacionCaja = objTransaccion.CodigoOperacion;
            if (objTransaccion.CodigoOperacion == Constantes.Operacion.Ingreso.OTRAS_VENTAS)
                objTransaccion.CodigoOperacionCaja = Constantes.Operacion.Ingreso.ESPECIALES_1;
            if (objTransaccion.NitEmpresaConcedeIva == "1000")
                objTransaccion.NitEmpresaConcedeIva = null;
            if (objTransaccion.CodigoEmpresa == -1)
                objTransaccion.CodigoEmpresa = null;
            if (objTransaccion.CodigoOperacion != Constantes.Operacion.Ingreso.VENTAS_EN_RUTA)
            {
                objTransaccion.CodigoOperacionCaja = objTransaccion.CodigoOperacion;
            }
            else
            {
                if (objTransaccion.RutaVendedor != -1)
                {
                    objTransaccion.Ruta = objTransaccion.RutaVendedor;
                }
            }

            if (objTransaccion.CodigoOperacion == Constantes.Operacion.Egreso.DEPOSITOS_BANCARIOS)
            {
                switch (objTransaccion.CodigoTipoDocumentoDeposito)
                {
                    case Constantes.TipoDocumentoDeposito.NUMERO_BOLETA:
                        objTransaccion.NumeroBoleta = objTransaccion.NumeroBoleta;
                        objTransaccion.NumeroVoucher = String.Empty;
                        break;
                    case Constantes.TipoDocumentoDeposito.NUMERO_VOUCHER:
                        objTransaccion.NumeroVoucher = objTransaccion.NumeroBoleta;
                        objTransaccion.NumeroBoleta = String.Empty;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                objTransaccion.NumeroBoleta = String.Empty;
                objTransaccion.NumeroVoucher = String.Empty;
            }

            if (objTransaccion.NumeroCuenta == "-1")
                objTransaccion.NumeroCuenta = null;
            if (objTransaccion.CodigoBancoDeposito == -1)
                objTransaccion.CodigoBancoDeposito = null;

            // Planilla
            if (objTransaccion.AnioPlanilla == -1)
            {
                objTransaccion.AnioPlanilla = 0;
                objTransaccion.CodigoQuincenaPlanilla = 0;
            }

            if (objTransaccion.CodigoQuincenaPlanilla == -1)
            {
                objTransaccion.CodigoQuincenaPlanilla = 0;
            }

            // Bonos Extras
            if (objTransaccion.CodigoOperacion != Constantes.Operacion.Egreso.PLANILLA_BONOS_EXTRAS_COMISION)
                objTransaccion.AnioComision = 0;
            else
            {
                switch (objTransaccion.CodigoBonoExtra)
                {
                    case Constantes.BonoExtra.POR_COMISIONES:
                        break;
                    case Constantes.BonoExtra.POR_QUINTALAJE:
                        objTransaccion.AnioComision = 0;
                        objTransaccion.CodigoOperacionCaja = Constantes.Operacion.Egreso.PLANILLA_BONOS_EXTRAS_QUINTALAJE;
                        break;
                    case Constantes.BonoExtra.FERIADOS_Y_DOMINGOS:
                        objTransaccion.AnioComision = 0;
                        objTransaccion.CodigoOperacionCaja = Constantes.Operacion.Egreso.PLANILLA_BONOS_EXTRAS_FERIADOS_Y_DOMINGOS;
                        break;
                    case Constantes.BonoExtra.OTROS:
                        objTransaccion.AnioComision = 0;
                        objTransaccion.CodigoOperacionCaja = Constantes.Operacion.Egreso.PLANILLA_BONOS_EXTRAS_OTROS;
                        break;
                    default:
                        objTransaccion.AnioComision = 0;
                        break;
                }
            }

            if (objTransaccion.CodigoOperacion == Constantes.Operacion.Egreso.PLANILLA_PAGO)
            {
                if (objTransaccion.CodigoTipoPlanilla == Constantes.Operacion.Egreso.TipoPlanilla.AJUSTE_PLANILLA)
                {
                    objTransaccion.CodigoOperacionCaja = Constantes.Operacion.Egreso.AJUSTE_PLANILLA;
                }
            }

            if (objTransaccion.CodigoOperacion == Constantes.Operacion.Egreso.GASTOS_INDIRECTOS)
            {
                if (objTransaccion.CodigoTipoGastoIndirecto == Constantes.Operacion.Egreso.TipoGastoIndirecto.SUELDO_INDIRECTO)
                {
                    objTransaccion.CodigoOperacionCaja = Constantes.Operacion.Egreso.SUELDOS_INDIRECTOS;
                }
            }

            objTransaccion.FechaOperacion = Util.Conversion.ConvertDateSpanishToEnglish(objTransaccion.FechaStr);
            objTransaccion.FechaRecibo = Util.Conversion.ConvertDateSpanishToEnglish(objTransaccion.FechaReciboStr);
            objTransaccion.ComplementoConta = 0;

            return obj.ActualizarTransaccion(objTransaccion, usuarioAct);
        }

        public string RegistrarSolicitudAprobacionDeCorreccion(long codigoTransaccion, int codigoTipoCorreccion, string observaciones, string usuarioIng)
        {
            TransaccionDAL obj = new TransaccionDAL();
            return obj.RegistrarSolicitudAprobacionDeCorreccion(codigoTransaccion, codigoTipoCorreccion, observaciones, usuarioIng);
        }

        public string RegistrarCorreccion(TransaccionCLS objTransaccion, string usuarioIng)
        {
            TransaccionDAL obj = new TransaccionDAL();
            if (objTransaccion.CodigoOperacionCaja == 0)
                objTransaccion.CodigoOperacionCaja = objTransaccion.CodigoOperacion;
            if (objTransaccion.CodigoOperacion == Constantes.Operacion.Ingreso.OTRAS_VENTAS)
                objTransaccion.CodigoOperacionCaja = Constantes.Operacion.Ingreso.ESPECIALES_1;
            if (objTransaccion.NitEmpresaConcedeIva == "1000")
                objTransaccion.NitEmpresaConcedeIva = null;
            if (objTransaccion.CodigoEmpresa == -1)
                objTransaccion.CodigoEmpresa = null;
            if (objTransaccion.CodigoOperacion != Constantes.Operacion.Ingreso.VENTAS_EN_RUTA)
            {
                objTransaccion.CodigoOperacionCaja = objTransaccion.CodigoOperacion;
            }
            else
            {
                if (objTransaccion.RutaVendedor != -1)
                {
                    objTransaccion.Ruta = objTransaccion.RutaVendedor;
                }
            }

            if (objTransaccion.NumeroCuenta == "-1")
                objTransaccion.NumeroCuenta = null;
            if (objTransaccion.CodigoBancoDeposito == -1)
                objTransaccion.CodigoBancoDeposito = null;

            // Planilla
            if (objTransaccion.AnioPlanilla == -1)
            {
                objTransaccion.AnioPlanilla = 0;
                objTransaccion.CodigoQuincenaPlanilla = 0;
            }

            if (objTransaccion.CodigoQuincenaPlanilla == -1)
            {
                objTransaccion.CodigoQuincenaPlanilla = 0;
            }

            // Bonos Extras
            if (objTransaccion.CodigoOperacion != Constantes.Operacion.Egreso.PLANILLA_BONOS_EXTRAS_COMISION)
                objTransaccion.AnioComision = 0;
            else
            {
                switch (objTransaccion.CodigoBonoExtra)
                {
                    case Constantes.BonoExtra.POR_COMISIONES:
                        break;
                    case Constantes.BonoExtra.POR_QUINTALAJE:
                        objTransaccion.AnioComision = 0;
                        objTransaccion.CodigoOperacionCaja = Constantes.Operacion.Egreso.PLANILLA_BONOS_EXTRAS_QUINTALAJE;
                        break;
                    case Constantes.BonoExtra.FERIADOS_Y_DOMINGOS:
                        objTransaccion.AnioComision = 0;
                        objTransaccion.CodigoOperacionCaja = Constantes.Operacion.Egreso.PLANILLA_BONOS_EXTRAS_FERIADOS_Y_DOMINGOS;
                        break;
                    case Constantes.BonoExtra.OTROS:
                        objTransaccion.AnioComision = 0;
                        objTransaccion.CodigoOperacionCaja = Constantes.Operacion.Egreso.PLANILLA_BONOS_EXTRAS_OTROS;
                        break;
                    default:
                        objTransaccion.AnioComision = 0;
                        break;
                }
            }

            if (objTransaccion.CodigoOperacion == Constantes.Operacion.Egreso.PLANILLA_PAGO)
            {
                if (objTransaccion.CodigoTipoPlanilla == Constantes.Operacion.Egreso.TipoPlanilla.AJUSTE_PLANILLA)
                {
                    objTransaccion.CodigoOperacionCaja = Constantes.Operacion.Egreso.AJUSTE_PLANILLA;
                }
            }

            if (objTransaccion.CodigoOperacion == Constantes.Operacion.Egreso.GASTOS_INDIRECTOS)
            {
                if (objTransaccion.CodigoTipoGastoIndirecto == Constantes.Operacion.Egreso.TipoGastoIndirecto.SUELDO_INDIRECTO)
                {
                    objTransaccion.CodigoOperacionCaja = Constantes.Operacion.Egreso.SUELDOS_INDIRECTOS;
                }
            }

            objTransaccion.FechaOperacion = Util.Conversion.ConvertDateSpanishToEnglish(objTransaccion.FechaStr);
            objTransaccion.FechaRecibo = Util.Conversion.ConvertDateSpanishToEnglish(objTransaccion.FechaReciboStr);
            objTransaccion.ComplementoConta = 0;
           
            return obj.RegistrarCorreccion(objTransaccion, usuarioIng);
        }

        public List<ContribuyenteCLS> GetEmpresasConcecionIVA()
        {
            TransaccionDAL obj = new TransaccionDAL();
            return obj.GetEmpresasConcecionIVA();
        }

        public string AnularTransaccion(long codigoTransaccion, int codigoOperacion, long codigoCuentaPorCobrar, string observaciones, string usuarioAct)
        {
            TransaccionDAL obj = new TransaccionDAL();
            return obj.AnularTransaccion(codigoTransaccion, codigoOperacion, codigoCuentaPorCobrar, observaciones, usuarioAct);
        }

        public string AnularTransaccionComplementoContabilidad(long codigoTransaccion, string usuarioAct)
        {
            TransaccionDAL obj = new TransaccionDAL();
            return obj.AnularTransaccionComplementoContabilidad(codigoTransaccion, usuarioAct);
        }

        public string AceptarTransaccionComplementoContabilidad(long codigoTransaccion, string usuarioAct)
        {
            TransaccionDAL obj = new TransaccionDAL();
            return obj.AceptarTransaccionComplementoContabilidad(codigoTransaccion, usuarioAct);
        }

        public List<ReporteCajaCLS> GetTransaccionParaCambioDeSemanaOperacion(string usuarioIng)
        {
            TransaccionDAL obj = new TransaccionDAL();
            return obj.GetTransaccionParaCambioDeSemanaOperacion(usuarioIng);
        }

        public string CambiarSemanaOperacionTransacciones(int anioOperacion, int semanaOperacion, string usuarioIng)
        {
            TransaccionDAL obj = new TransaccionDAL();
            return obj.CambiarSemanaOperacionTransacciones(anioOperacion, semanaOperacion, usuarioIng);
        }

        public string AceptarRevision(long codigoTransaccion, int revisado, string usuarioAct)
        {
            TransaccionDAL obj = new TransaccionDAL();
            return obj.AceptarRevision(codigoTransaccion, revisado, usuarioAct);
        }

        public SolicitudCorreccionCLS GetDataCorreccion(long codigoTransaccion)
        {
            TransaccionDAL obj = new TransaccionDAL();
            return obj.GetDataCorreccion(codigoTransaccion);
        }

        public string ActualizarNumeroBoletaDeposito(long codigoTransaccion, string numeroBoletaDeposito, string usuarioAct)
        {
            TransaccionDAL obj = new TransaccionDAL();
            return obj.ActualizarNumeroBoletaDeposito(codigoTransaccion, numeroBoletaDeposito, usuarioAct);
        }

        public decimal GetMontoPlanillaParaDesglosar(int anioOperacion, int semanaOperacion, int codigoReporte)
        {
            TransaccionDAL obj = new TransaccionDAL();
            return obj.GetMontoPlanillaParaDesglosar(anioOperacion, semanaOperacion, codigoReporte);
        }

    }
}
