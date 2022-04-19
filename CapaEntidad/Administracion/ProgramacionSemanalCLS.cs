using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Administracion
{
    public class ProgramacionSemanalCLS
    {
        public int CodigoProgramacion { get; set; }
        public short Anio { get; set; }
        public DateTime Fecha { get; set; }

        public string FechaStr { get; set; }

        public byte NumeroSemana { get; set; }

        public byte SemanaComision { get; set; }

        public byte NumeroDia { get; set; }

        public string FechaInicioSemana { get; set; }

        public string FechaFinSemana { get; set; }

        public string FechaSistema { get; set; }
        public string Periodo { get; set; }
        public string Dia { get; set; }
    }
}
