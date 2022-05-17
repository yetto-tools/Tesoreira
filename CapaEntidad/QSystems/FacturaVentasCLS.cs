using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.QSystems
{
    public class FacturaVentasCLS
    {
        public string FechaFactura { get; set; }
        public string DiaSemana { get; set; }
        public string CodigoTienda { get; set; }
        public int CodigoCaja { get; set; }
        public string SerieFactura { get; set; }
        public int NumeroFactura { get; set; }
        public string SerieFacturaFEL { get; set; }
        public string NumeroFacturaFEL { get; set; }
        public string CodigoCliente { get; set; }
        public string NombreCliente { get; set; }
        public string NitCliente { get; set; }
        public string FacturadoA { get; set; }

        public short CodigoVendedor { get; set; }

        public string NombreVendedor { get; set; }

        public string FormaPago { get; set; }

        public string Clasificacion { get; set; }

        public decimal TotalSinIVA { get; set; }
        public decimal TotalIVA { get; set; }
        public decimal TotalConIVA { get; set; }
        public int NumeroLinea { get; set; }
        public string CodigoSKU { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        public decimal TotalPorArticulo { get; set; }


    }



}

