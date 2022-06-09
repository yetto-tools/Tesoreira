using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.QSystems
{
    public class ValeSalidaCLS
    {
        public DateTime FechaEmision { get; set; }
        public string FechaEmisionStr { get; set; }
        public string DiaSemana { get; set; }
        public int Ruta { get; set; }
        public string NombreVendedor { get; set; }

        public int NumeroVale { get; set; }

        public int NumeroLinea { get; set; }

        public string CodigoInventario { get; set; }

        public string Descripcion { get; set; }

        public decimal PrecioUnitario { get; set; }

        public int Cantidad { get; set; }

    }
}
