using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Tesoreria
{
    public class CajaChicaCLS
    {
        public long CodigoTransaccion { get; set; }

        public long CodigoTransaccionAnt { get; set; }
        public int? CodigoReporte { get; set; }
        public short CodigoCajaChica { get; set; }
        public string NombreCajaChica { get; set; }
        public short CodigoOperacion { get; set; }
        public string Operacion { get; set; }
        public string CodigoTipoTransaccion { get; set; }

        public string NitProveedor { get; set; }

        public string NitProveedorOld { get; set; }

        public string NombreProveedor { get; set; }

        public string SerieFactura { get; set; }

        public string SerieFacturaOld { get; set; }

        public long NumeroDocumento { get; set; }

        public long NumeroDocumentoOld { get; set; }

        public DateTime FechaDocumento { get; set; }
        public string FechaDocumentoStr { get; set; }

        public string Descripcion { get; set; }
        public DateTime FechaOperacion { get; set; }

        public string FechaOperacionStr { get; set; }

        public string FechaStr { get; set; }

        public short AnioOperacion { get; set; }
        public byte SemanaOperacion { get; set; }
        public byte DiaOperacion { get; set; }

        public string NombreDiaOperacion { get; set; }

        public decimal Monto { get; set; }

        public string Observaciones { get; set; }
        public string ObservacionesAnulacion { get; set; }
        public string UsuarioAnulacion { get; set; }

        public DateTime? FechaAnulacion { get; set; }

        public byte ExcluirFactura { get; set; }

        public byte? CodigoMotivoExclusion { get; set; }
        public string ObservacionesExclusion { get; set; }
        public string UsuarioRevision { get; set; }
        public DateTime? FechaRevision { get; set; }

        public short CodigoEstado { get; set; }
        public string Estado { get; set; }
        public string UsuarioIng { get; set; }
        public DateTime FechaIng { get; set; }
        public string FechaIngStr { get; set; }
        public string UsuarioAct { get; set; }
        public DateTime? FechaAct { get; set; }

        public string fechaImpresionStr { get; set; }
        public byte PermisoAnular { get; set; }
        public byte PermisoEditar { get; set; }
        public byte PermisoCorregir { get; set; }
        public byte PermisoAutorizar { get; set; }
        public byte Correccion { get; set; }
        public string ObservacionesSolicitud { get; set; }
        public string ObservacionesAprobacion { get; set; }

        public string EstadoSolicitudCorreccion { get; set; }
        public byte CodigoEstadoSolicitudCorreccion { get; set; }

        public long CodigoTransaccionCorrecta { get; set; }

        public string RespuestaCorreccion { get; set; }
        public byte CodigoEstadoRecepcion { get; set; }
        public string EstadoRecepcion { get; set; }
        public byte CodigoTipoCorreccion { get; set; }
        public string TipoCorreccion { get; set; }

        public string ObservacionesRecepcion { get; set; }

        public byte CodigoTipoDocumento { get; set; }

        public string TipoDocumento { get; set; }

    }



}
