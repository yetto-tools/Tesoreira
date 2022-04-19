using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Tesoreria
{
    public class TransaccionCLS
    {
        public long CodigoTransaccion { get; set; }

        public long? CodigoTransaccionAnt { get; set; }
        public long? CodigoTransaccionCorrecta { get; set; }
        public string CodigoSeguridad { get; set; }
        public short? CodigoEmpresa { get; set; }
        public string FormaPago { get; set; }
        public short CodigoOperacion { get; set; }
        public short CodigoOperacionCaja { get; set; }
        public string Operacion { get; set; }
        public byte CodigoTipoCuentaPorCobrar { get; set; }
        public byte CodigoTipoGastoIndirecto { get; set; }
      
        public string TipoCuentaPorCobrar { get; set; }
        public long? CodigoCuentaPorCobrar { get; set; }
        public short CodigoArea { get; set; }
        public string Area { get; set; }
        public short CodigoCategoriaEntidad { get; set; }
        public string CategoriaEntidad { get; set; }
        public string CodigoEntidad { get; set; }
        public string NombreEntidad { get; set; }
        public byte CodigoTipoOperacion { get; set; }
        public string CodigoTipoTransaccion { get; set; }

        public byte CodigoTipoDocumento { get; set; }

        public byte Efectivo { get; set; }

        public byte Deposito { get; set; }
        public byte Cheque { get; set; }

        public string NitProveedor { get; set; }

        public string SerieFactura { get; set; }

        public int? NumeroDocumento { get; set; }

        public DateTime? FechaDocumento { get; set; }

        public byte ConcederIva { get; set; }

        public string NitEmpresaConcedeIva { get; set; }

        public short? CodigoBancoDeposito { get; set; }

        public string NumeroCuenta { get; set; }

        public byte CodigoTipoDocumentoDeposito { get; set; }

        public string NumeroBoleta { get; set; }

        public string NumeroVoucher { get; set; }

        public long NumeroRecibo { get; set; }

        public DateTime FechaRecibo { get; set; }

        public string FechaReciboStr { get; set; }

        public string NumeroReciboStr { get; set; }

        public DateTime FechaOperacion { get; set; }
        public String FechaOperacionStr { get; set; }
        public String HoraOperacionStr { get; set; }

        public string FechaStr { get; set; }

        public short AnioOperacion { get; set; }
        public byte SemanaOperacion { get; set; }
        public byte DiaOperacion { get; set; }

        public string NombreDiaOperacion { get; set; }
        public int? CodigoBoletaComision { get; set; }
        public short RutaVendedor { get; set; }
        public short Ruta { get; set; }
        public string CodigoVendedor { get; set; }

        public byte SemanaComision { get; set; }

        public short AnioComision { get; set; }

        public decimal MontoEfectivo { get; set; }
        public decimal MontoCheques { get; set; }
        public decimal Monto { get; set; }

        public byte CodigoFrecuenciaPago { get; set; }
        public byte CodigoTipoPago { get; set; }
        public byte CodigoPlanilla { get; set; }
        public byte CodigoTipoPlanilla { get; set; }
        public Int64 CodigoPagoPlanilla { get; set; }
        public short AnioPlanilla { get; set; }
        public byte MesPlanilla { get; set; }
        public byte SemanaPlanilla { get; set; }
        public int CodigoQuincenaPlanilla { get; set; }

        public byte CodigoBonoExtra { get; set; }
        public byte TipoEspeciales1 { get; set; }
        public string Observaciones { get; set; }
        public DateTime? FechaConfirmacion { get; set; }

        public string MotivoAnulacion { get; set; }
        public string UsuarioAnulacion { get; set; }
        public DateTime? FechaAnulacion { get; set; }

        public short CodigoEstado { get; set; }
        public string Estado { get; set; }
        public string UsuarioIng { get; set; }
        public DateTime FechaIng { get; set; }
        public string FechaIngStr { get; set; }
        public string UsuarioAct { get; set; }
        public DateTime? FechaAct { get; set; }
        public byte PermisoEditar { get; set; }
        public byte PermisoAnular { get; set; }
        public byte PermisoCorregir { get; set; }
        public byte PermisoAutorizar { get; set; }
        public DateTime? FechaPrestamo { get; set; }
        public DateTime? FechaInicioPago { get; set; }

        public short AnioSueldoIndirecto { get; set; }
        public byte MesSueldoIndirecto { get; set; }
        public short Signo { get; set; }
        public string FechaImpresionStr { get; set; }

        public string Recursos { get; set; }

        public byte ComplementoConta { get; set; }

        public string EscomplementoDeInformacion { get; set; }

        public int? CodigoReporte { get; set; }

        public string ObservacionesCorreccion { get; set; }

        public byte CodigoEstadoReporteCaja { get; set; }
        public byte Revisado { get; set; }
        public byte CodigoEstadoSolicitudCorreccion { get; set; }
        public string EstadoSolicitudCorreccion { get; set; }
        public byte Correccion { get; set; }
        public string NombreProveedor { get; set; }

        public int PermisoActualizar { get; set; }

        public byte CodigoTipoCorreccion { get; set; }

        public string TipoCorreccion { get; set; }

        public short CodigoCanalVenta { get; set; }

    }



}
