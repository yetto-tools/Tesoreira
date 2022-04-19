using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Tesoreria
{
    public class ContribuyenteCLS
    {
        public string Nit { get; set; }
        public string Nombre { get; set; }
        public short CodigoTipoContribuyente { get; set; }
        public string Descripcion { get; set; }
        public byte Estado { get; set; }

    }
}
