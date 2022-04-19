using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Tesoreria
{
    public class ConfiguracionSueldoIndirectoCLS
    {
        public short Anio { get; set; }
        public byte Mes { get; set; }
        public string NombreMes { get; set; }
        public decimal Monto { get; set; }
        public byte CodigoEstado { get; set; }
        public string Estado { get; set; }
        public string UsuarioIng { get; set; }
        public DateTime FechaIng { get; set; }
        public byte PermisoEditar { get; set; }

    }
}
