using CapaEntidad.Administracion;
using CapaEntidad.Tesoreria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Contabilidad
{
    public class ReporteOperacionesCajaListCLS
    {
        public List<ProgramacionSemanalCLS> listaEncabezado { get; set; }
        public List<ProgramacionSemanalCLS> listaEncabezadoFechas { get; set; }

        public List<ReporteResumenOperacionesCajaCLS> listaIngresos { get; set; }
        public List<ReporteResumenOperacionesCajaCLS> listaEgresos { get; set; }
        public List<ReporteOperacionesCajaCLS> listaTransaccciones { get; set; }

        public List<TipoOperacionCLS> listaMontosTiposDeOperacion { get; set; }

        public List<ReporteOperacionesCajaCLS> listaMontosEntidad { get; set; }

        public ReporteCierreCLS objCierre { get; set; }

        public List<CuentaPorCobrarReporteDetalleCLS> listaDetalleCuentasPorCobrar { get; set; }

        public List<OrigenDepositoCLS> listaMontosPorOrigen { get; set; }

        public decimal MontoAsignado { get; set; }

        public string FechaGeneracionStr { get; set; }

        public decimal MontoReembolso { get; set; }


    }
}
