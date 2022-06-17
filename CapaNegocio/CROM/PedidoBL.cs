using CapaDatos.CROM;
using CapaEntidad.CROM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.CROM
{
    public class PedidoBL
    {
        public List<PedidoCLS> GetListaPedidos()
        {
            PedidoDAL obj = new PedidoDAL();
            return obj.GetListaPedidos();
        }
    }
}
