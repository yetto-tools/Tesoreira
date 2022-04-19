using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Tesoreria
{
    public class OperacionCLS
    {
        public short CodigoOperacion { get; set; }
        public short CodigoCategoriaOperacion { get; set; }

        public string CategoriaOperacion { get; set; }
        public short CodigoTipoOperacion { get; set; }
        public string TipoOperacion { get; set; }
        public string Nombre { get; set; }
        public string NombreReporteCaja { get; set; }
        public string Descripcion { get; set; }

        public string HabilitarParaCajaTesoreria { get; set; }
        public byte CodigoEstado { get; set; }
        public string Estado { get; set; }
        public short CodigoConcepto { get; set; }
        public string Concepto { get; set; }

        public string AplicaCajaChica { get; set; }
        public string IncluirEnConfiguracionEntidadGenerica { get; set; }

        public byte Grupo01 { get; set; }
        public byte Grupo02 { get; set; }

        public byte Grupo03 { get; set; }

        public string UsuarioIng { get; set; }
        public DateTime FechaIng { get; set; }
    }
}
