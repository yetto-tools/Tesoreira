using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Contabilidad
{
    public class ReporteOperacionesCajaCLS
    {
        public short CodigoTipoOperacion { get; set; }
        public string TipoOperacion { get; set; }
        public string IdTipoOperacion { get; set; }
        public short Signo { get; set; }
        public short CodigoCategoriaOperacion { get; set; }
        public string CategoriaOperacion { get; set; }
        public short CodigoOperacion { get; set; }
        public string Operacion { get; set; }
        public byte CodigoOrigen { get; set; }
        public string Origen { get; set; }
        public short CodigoCategoriaEntidad { get; set; }
        public string CategoriaEntidad { get; set; }
        public string CodigoEntidad { get; set; }
        public string NombreEntidad { get; set; }
        public string NombreEntidadCompleto { get; set; }

        public short CodigoEmpresa { get; set; }
        public string NombreEmpresa { get; set; }
        public string NumeroCuenta { get; set; }
        public string NumeroBoleta { get; set; }
        public string Descripcion { get; set; }
        public decimal MontoLunes { get; set; }
        public decimal MontoMartes { get; set; }
        public decimal MontoMiercoles { get; set; }
        public decimal MontoJueves { get; set; }
        public decimal MontoViernes { get; set; }
        public decimal MontoSabado { get; set; }
        public decimal MontoDomingo { get; set; }
        public decimal MontoSemana { get; set; }
        public decimal MontoTotalLunes { get; set; }
        public decimal MontoTotalMartes { get; set; }
        public decimal MontoTotalMiercoles { get; set; }
        public decimal MontoTotalJueves { get; set; }
        public decimal MontoTotalViernes { get; set; }
        public decimal MontoTotalSabado { get; set; }
        public decimal MontoTotalDomingo { get; set; }
        public decimal MontoTotalSemana { get; set; }
        public decimal MontoTotalEntidad { get; set; }
        public string FechaOperacionStr { get; set; }
        public string DescripcionLibre { get; set; }

    }
}
