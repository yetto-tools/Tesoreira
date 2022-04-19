using CapaDatos.Administracion;
using CapaEntidad.Administracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Administracion
{
    public class TipoReporteBL
    {
        public List<TipoReporteCLS> GetAllTiposReportes()
        {
            TipoReporteDAL obj = new TipoReporteDAL();
            return obj.GetAllTiposReportes();
        }

        public List<TipoReporteCLS> GetTiposDeReportesAsignados(string idUsuario, int superAdmin)
        {
            TipoReporteDAL obj = new TipoReporteDAL();
            return obj.GetTiposDeReportesAsignados(idUsuario, superAdmin);
        }

        public string GuardarTipoReporte(TipoReporteCLS objTipoReporte, string usuarioIng)
        {
            TipoReporteDAL obj = new TipoReporteDAL();
            return obj.GuardarTipoReporte(objTipoReporte, usuarioIng);
        }

        public string ActualizarTipoReporte(TipoReporteCLS objTipoReporte, string usuarioAct)
        {
            TipoReporteDAL obj = new TipoReporteDAL();
            return obj.ActualizarTipoReporte(objTipoReporte, usuarioAct);
        }

    }
}
