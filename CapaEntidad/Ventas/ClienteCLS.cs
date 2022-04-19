using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Ventas
{
    public class ClienteCLS
    {
        public string CodigoCliente { get; set; }
        public string NombreCompleto { get; set; }
        public string NombreCorto { get; set; }
        public short CodigoTipoCliente { get; set; }
        public string Descripcion { get; set; }
        public byte Estado { get; set; }
        public string UsuarioIng { get; set; }
        public DateTime FechaIng { get; set; }
    }
}
