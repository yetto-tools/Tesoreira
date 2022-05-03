using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.RRHH
{
    public class EmpleadoCLS
    {
        public short CodigoEmpresa { get; set; }
        public string Empresa { get; set; }
        public string CodigoEmpleado { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string TercerNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string ApellidoCasada { get; set; }
        public string NombreCompleto { get; set; }

        public string FechaNacimientoStr { get; set; }
        public string CodigoGenero { get; set; }
        public string CodigoTipoIdentificacion { get; set; }
        public string Cui { get; set; }
        public string Nit { get; set; }
        public string CorreoElectronico { get; set; }
        public string NumeroAfiliacion { get; set; }
        public byte EmpleadoExterno { get; set; }
        public short CodigoArea { get; set; }
        public string Area { get; set; }
        public short CodigoSeccion { get; set; }
        public string Seccion { get; set; }
        public short CodigoPuesto { get; set; }
        public string Puesto { get; set; }
        public short CodigoTipoCuenta { get; set; }
        public short CodigoUbicacion { get; set; }
        public string Ubicacion { get; set; }
        public string NumeroCuenta { get; set; }
        public decimal MontoDevengado { get; set; }
        public byte CodigoJornada { get; set; }
        public string Jornada { get; set; }
        public byte CodigoFrecuenciaPago { get; set; }
        public string FrecuenciaPago { get; set; }
        public byte Igss { get; set; }
        public byte BonoDeLey { get; set; }
        public byte RetencionIsr { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string FechaIngresoStr { get; set; }
        public DateTime? FechaEgreso { get; set; }
        public string FechaEgresoStr { get; set; }
        public short CodigoMotivoBaja { get; set; }
        public string Observaciones { get; set; }
        public short CodigoEstado { get; set; }
        public string EstadoEmpleado { get; set; }
        public string UsuarioIng { get; set; }
        public DateTime FechaIng { get; set; }
        public string UsuarioAct { get; set; }
        public DateTime? FechaAct { get; set; }
        public Byte BackToBack { get; set; }
        public Byte CodigoTipoBackToBack { get; set; }
        public Byte PermisoAnular { get; set; }
        public Byte PermisoEditar { get; set; }
        public string  Foto { get; set; }
        public decimal SalarioDiario { get; set; }
        public decimal BonoDecreto372001 { get; set; }
        public Byte PagoPendiente { get; set; }
        public Byte SaldoPrestamo { get; set; }
        public string PagoPendienteStr { get; set; }
        public string SaldoPrestamoStr { get; set; }


    }
}
