using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Administracion
{
    public class ParametroImpresionCLS
    {
        public short CodigoConfiguracion { get; set; }
        public string NombreImpresora { get; set; }
        public short NumeroCopias { get; set; }
        public string Ip { get; set; }
        public int Puerto { get; set; }
    }


}
