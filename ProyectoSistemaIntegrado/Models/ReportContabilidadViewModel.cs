using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Models
{
    public class ReportContabilidadViewModel
    {
        public int AnioOperacion { get; set; }
        public int SemanaOperacion { get; set; }
        public int CodigoEmpresa { get; set; }
        public int CodigoCajaChica { get; set; }
        public int CodigoReporte { get; set; }
        public int Arqueo { get; set; }
    }
}
