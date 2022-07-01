using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.CROM
{
    public class TrasladoEspeciales2DetalleCLS
    {
        public string CodigoCliente { get; set; }
        public int CodigoTraslado { get; set; }
        public string CodigoEmpresa { get; set; }
        public DateTime FechaGrabado { get; set; }
        public string FechaGrabadoStr { get; set; }
        public decimal Monto { get; set; }
        public string NombreCliente { get; set; }
        public string NombreClienteDepurado { get; set; }
        public long NumeroPedido { get; set; }
        public string Serie { get; set; }
        public int PermisoAnular { get; set; }
    }
}
