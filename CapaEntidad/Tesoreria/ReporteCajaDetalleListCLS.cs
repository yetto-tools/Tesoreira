using CapaEntidad.Administracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Tesoreria
{
    public class ReporteCajaDetalleListCLS
    {
        public List<ProgramacionSemanalCLS> listaEncabezadoFechas { get; set; }
        public List<ReporteCajaDetalleCLS> listaVendedores { get; set; }
        public List<ReporteCajaDetalleCLS> listaEspecial1 { get; set; }
        public List<ReporteCajaDetalleCLS> listaEspecial2 { get; set; }
        public List<ReporteCajaDetalleCLS> listaCajas { get; set; }
        public List<ReporteCajaDetalleCLS> listaCombustibleCarros { get; set; }
        public List<ReporteCajaDetalleCLS> listaPlanillas { get; set; }
        public List<ReporteCajaDetalleCLS> listaSueldosIndirectos { get; set; }
        public List<ReporteCajaDetalleCLS> listaLiquidaciones { get; set; }
        public List<ReporteCajaDetalleCLS> listaVacaciones { get; set; }
        public List<ReporteCajaDetalleCLS> listaBonoFinDeMes { get; set; }
        public List<ReporteCajaDetalleCLS> listaBonosExtras { get; set; }
        public List<ReporteCajaDetalleCLS> listaBono14 { get; set; }
        public List<ReporteCajaDetalleCLS> listaAguinaldos { get; set; }
        public List<ReporteCajaDetalleCLS> listaPrestamos { get; set; }
        public List<ReporteCajaDetalleCLS> listaAnticipos { get; set; }
        public List<ReporteCajaDetalleCLS> listaMateriaPrima { get; set; }
        public List<ReporteCajaDetalleCLS> listaGastosIndirectos { get; set; }
        public List<ReporteCajaDetalleCLS> listaGastosAdministrativos { get; set; }
        public List<ReporteCajaDetalleCLS> listaMantenimientoVehiculos { get; set; }
        public List<ReporteCajaDetalleCLS> listaDepositosBancarios { get; set; }
        public decimal SaldoAnteriorAcumulado { get; set; }
    }
}
