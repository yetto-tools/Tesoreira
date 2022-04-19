using CapaDatos.Administracion;
using CapaEntidad.Administracion;
using CapaEntidad.Tesoreria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Administracion
{
    public class UsuarioBL
    {
        public string GuardarUsuario(UsuarioCLS objUsuario)
        {
            UsuarioDAL obj = new UsuarioDAL();
            return obj.GuardarUsuario(objUsuario);
        }

        public string ActualizarContrasenia(UsuarioCLS objUsuario)
        {
            UsuarioDAL obj = new UsuarioDAL();
            return obj.ActualizarContrasenia(objUsuario);
        }

        public List<UsuarioCLS> GetListaUsuarios()
        {
            UsuarioDAL obj = new UsuarioDAL();
            return obj.GetListaUsuarios();
        }

        public UsuarioCLS GetDataUsuario(string idUsuario)
        {
            UsuarioDAL obj = new UsuarioDAL();
            return obj.GetDataUsuario(idUsuario);
        }

        public List<RolCLS> GetPermisoRoles(string idUsuario)
        {
            UsuarioDAL obj = new UsuarioDAL();
            return obj.GetPermisoRoles(idUsuario);
        }

        public List<ConfiguracionCajaChicaCLS> GetPermisoCajasChicas(string idUsuario)
        {
            UsuarioDAL obj = new UsuarioDAL();
            return obj.GetPermisoCajasChicas(idUsuario);
        }

        public List<EmpresaCLS> GetPermisoEmpresas(string idUsuario)
        {
            UsuarioDAL obj = new UsuarioDAL();
            return obj.GetPermisoEmpresas(idUsuario);
        }

        public List<TipoReporteCLS> GetPermisoReportes(string idUsuario)
        {
            UsuarioDAL obj = new UsuarioDAL();
            return obj.GetPermisoReportes(idUsuario);
        }

        public UsuarioCLS Login(string idUsuario, string contrasenia)
        {
            UsuarioDAL obj = new UsuarioDAL();
            return obj.Login(idUsuario, contrasenia);
        }

        public UsuarioCLS LoginSuperAdmin(string idUsuario, string contrasenia)
        {
            UsuarioCLS obj = new UsuarioCLS();
            obj.IdUsuario = "superadmin";
            obj.NombreUsuario = "Henrry Sontay";
            return obj;
        }

        public string GuardarPermisos(List<PermisoCLS> objPermisos, string idUsuario, string usuarioIng)
        {
            UsuarioDAL obj = new UsuarioDAL();
            return obj.GuardarPermisos(objPermisos, idUsuario, usuarioIng);
        }

    }
}
