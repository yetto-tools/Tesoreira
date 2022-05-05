using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Administracion
{
    public class UsuarioCLS
    {
        public string IdUsuario { get; set; }

        public string PrimerNombre { get; set; }

        public string PrimerApellido { get; set; }
        public string NombreUsuario { get; set; }
        public string Contrasenia { get; set; }
        public string Cui { get; set; }
        public byte SuperAdmin { get; set; }
        public string EsSuperAdmin { get; set; }
        public byte CodigoEstado { get; set; }
        public string Estado { get; set; }
        public string UsuarioIng { get; set; }
        public DateTime FechaIng { get; set; }

        public byte SetSemanaAnterior { get; set; }
        public int PermisoEditar { get; set; }
        public int PermisoAnular { get; set; }

    }
}
