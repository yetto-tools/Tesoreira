using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.Administracion
{
    public class EmpresaCLS
    {
        public short CodigoEmpresa { get; set; }
        public string NombreRazonSocial { get; set; }
        public string NombreComercial { get; set; }
        public string CodigoQsystem { get; set; }
        public string Direccion { get; set; }
        public string Nit { get; set; }
        public byte Estado { get; set; }
        public string UsuarioIng { get; set; }
        public DateTime FechaIng { get; set; }

        public int Asignado { get; set; }

    }
}
