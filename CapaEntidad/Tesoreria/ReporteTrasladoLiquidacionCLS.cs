using CapaEntidad.Administracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Tesoreria
{
    public class ReporteTrasladoLiquidacionCLS
    {
        public List<ProgramacionSemanalCLS> listaEncabezado { get; set; }
        public List<ProgramacionSemanalCLS> listaEncabezadoFechas { get; set; }


        public TrasladoLiquidacionCLS encabezado { get; set; }

        public List<TrasladoLiquidacionDetalleReporteCLS> detalle { get; set; }

    }
}
