using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Administracion
{
    public class SiteMapCLS
    {
        public int CodigoSitemap { get; set; }
        public short CodigoSistema { get; set; }
        public string NombreSistema { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string NombreController { get; set; }
        public string NombreAction { get; set; }
        public int? CodigoSitemapPadre { get; set; }
        public byte Nivel { get; set; }
        public string NombreNivel { get; set; }
        public int CantidadItems { get; set; }
        public int CantidadSubItems { get; set; }
        public int CantidadOpciones { get; set; }
        public int CantidadSubOpciones { get; set; }
        public int CantidadSistemas { get; set; }
        public byte Estado { get; set; }
        public string TituloPadre { get; set; }
        public string UsuarioIng { get; set; }
        public DateTime FechaIng { get; set; }
        public int PermisoAnular { get; set; }
        public int PermisoEditar { get; set; }
    }
}
