using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Tesoreria
{
    public class EntidadGenericaCLS
    {
        public string CodigoEntidad { get; set; }
        public int CodigoCategoriaEntidad { get; set; }
        public string NombreEntidad { get; set; }
        public string NombreCategoria { get; set; }

        public string Descripcion { get; set; }

        public short CodigoOperacionCaja { get; set; }

        public short CodigoArea { get; set; }
        public short CodigoOperacionEntidad { get; set; }
        public short CodigoCanalVenta { get; set; }

        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }

        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string CodigoGenero { get; set; }

        public int CodigoTipoBTB { get; set; }

        public string MesPlanillaBTB { get; set; }
        public short AnioPlanillaBTB { get; set; }
        public decimal MontoDevolucionBTB { get; set; }
        public byte ConcedeIva { get; set; }
    }
}
