using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Administracion
{
    public class ProgramacionQuincenalCLS
    {
        public int CodigoProgramacion { get; set; }

        public short Anio { get; set; }

        public byte NumeroMes { get; set; }

        public byte NumeroQuincena { get; set; }

        public int CodigoQuincenaPlanilla { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public string FechaInicioStr { get; set; }

        public string FechaFinStr { get; set; }

        public string Periodo { get; set; }
        
    }
}
