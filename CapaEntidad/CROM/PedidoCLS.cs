using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.CROM
{
    public class PedidoCLS
    {
        public string CodigoEmpresa { get; set; }
        public string SeriePedido { get; set; }
        public long NumeroPedido { get; set; }

        public decimal Monto { get; set; }
        public string CodigoCliente { get; set; }
        public string NombreCliente { get; set; }

        public string SerieFactura { get; set; }

        public long NumeroFactura { get; set; }

        public long NumeroVale { get; set; }

        public long NumeroPedidoQSystems { get; set; }

        public string Observaciones { get; set; }
        public int PermisoSelect { get; set; }


    }
}
