using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Tesoreria
{
    public class ConfiguracionCajaChicaCLS
    {
        public short CodigoCajaChica { get; set; }
        public string NombreCajaChica { get; set; }
        public decimal MontoLimite { get; set; }
        public decimal MontoDisponible { get; set; }
        public string MontoDisponibleStr { get; set; }
        public byte PermisoAnular { get; set; }
        public byte PermisoEditar { get; set; }
        public int Asignado { get; set; }

    }
}
