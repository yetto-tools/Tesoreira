using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Tesoreria
{
    public class TrasladoEspeciales2DetalleCLS
    {
        public string CodigoEmpresa { get; set; }
        public string Serie { get; set; }
        public long NumeroPedido { get; set; }
        public int CodigoTraslado { get; set; }
        public string CodigoCliente { get; set; }
        public string NombreCliente { get; set; }
        public string CodigoEntidad { get; set; }
        public string NombreEntidad { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaGrabado { get; set; }
        public string FechaGrabadoStr { get; set; }
        public int modificacion { get; set; }
        public byte CodigoEstado { get; set; }
        public string Estado { get; set; }
        public byte CodigoEstadoDepuracion { get; set; }
        public string EstadoDepuracion { get; set; }

        public int RatioSimilitud { get; set; }
    }



}
