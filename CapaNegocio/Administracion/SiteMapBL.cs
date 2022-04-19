using CapaDatos.Administracion;
using CapaEntidad.Administracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Administracion
{
    public class SiteMapBL
    {
        public List<SiteMapCLS> GetMenus(string idUsuario, int esSuperAdmin)
        {
            SiteMapDAL obj = new SiteMapDAL();
            return obj.GetMenus(idUsuario, esSuperAdmin);
        }

        public List<SiteMapCLS> GetConfiguracion(int codigoSistema, int nivel)
        {
            SiteMapDAL obj = new SiteMapDAL();
            return obj.GetConfiguracion(codigoSistema, nivel);
        }

        public List<SiteMapCLS> GetSiteMapsPadre(int codigoSistema, int nivel)
        {
            SiteMapDAL obj = new SiteMapDAL();
            return obj.GetSiteMapsPadre(codigoSistema, nivel);
        }

        public string GuardarItemMenu(SiteMapCLS objMenu, string usuarioIng)
        {
            SiteMapDAL obj = new SiteMapDAL();
            return obj.GuardarItemMenu(objMenu, usuarioIng);
        }

        public SiteMapCLS GetDataItemMenu(int codigoSiteMap)
        {
            SiteMapDAL obj = new SiteMapDAL();
            return obj.GetDataItemMenu(codigoSiteMap);
        }

        public string ActualizarItemMenu(SiteMapCLS objMenu, string usuarioAct)
        {
            SiteMapDAL obj = new SiteMapDAL();
            return obj.ActualizarItemMenu(objMenu, usuarioAct);
        }

        public string AnularItemMenu(int codigoSitemap, string usuarioAct)
        {
            SiteMapDAL obj = new SiteMapDAL();
            return obj.AnularItemMenu(codigoSitemap, usuarioAct);
        }

    }
}
