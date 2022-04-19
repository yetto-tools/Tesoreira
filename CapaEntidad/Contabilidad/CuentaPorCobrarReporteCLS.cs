using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Contabilidad
{
    public class CuentaPorCobrarReporteCLS
    {
        public int CodigoReporte { get; set; }
        public short AnioOperacion { get; set; }
        public byte SemanaOperacion { get; set; }
        public decimal AnticipoLiquidable { get; set; }
        public decimal DevolucionAnticipoLiquidable { get; set; }
        public decimal AnticipoSalario { get; set; }
        public decimal DescuentoAnticipoSalario { get; set; }
        public decimal Prestamo { get; set; }
        public decimal AbonoPrestamo { get; set; }
        public decimal BackToBack { get; set; }
        public decimal BackToBackPagoPlanilla { get; set; }
        public decimal RetiroSocios { get; set; }
        public decimal DevolucionSocios { get; set; }
        public byte Bloqueado { get; set; }
        public byte CodigoEstado { get; set; }
        public string Estado { get; set; }
        public byte PermisoAnular { get; set; }
        public byte PermisoEditar { get; set; }


    }
}

