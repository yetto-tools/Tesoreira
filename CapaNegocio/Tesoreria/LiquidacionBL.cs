using CapaDatos.Tesoreria;
using CapaEntidad.Tesoreria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Tesoreria
{
    public class LiquidacionBL
    {
        public List<TrasladoLiquidacionCLS> GetTrasladosParaLiquidacion()
        {
            LiquidacionDAL obj = new LiquidacionDAL();
            return obj.GetTrasladosParaLiquidacion();
        }

        public List<TrasladoLiquidacionDetalleCLS> GetDetalleTrasladoLiquidacionPorGenerar(int anioOperacion, int semanaOperacion)
        {
            LiquidacionDAL obj = new LiquidacionDAL();
            return obj.GetDetalleTrasladoLiquidacionPorGenerar(anioOperacion, semanaOperacion);
        }

        public List<TrasladoLiquidacionDetalleCLS> GetDetalleTrasladoLiquidacion(long codigoTraslado)
        {
            LiquidacionDAL obj = new LiquidacionDAL();
            return obj.GetDetalleTrasladoLiquidacion(codigoTraslado);
        }

        public List<TrasladoLiquidacionCLS> GetTrasladosLiquidacionConsulta(int anioOperacion)
        {
            LiquidacionDAL obj = new LiquidacionDAL();
            return obj.GetTrasladosLiquidacionConsulta(anioOperacion);
        }

        public string GenerarTraslado(int anioOperacion, int semanaOperacion, string usuarioIng)
        {
            LiquidacionDAL obj = new LiquidacionDAL();
            return obj.GenerarTraslado(anioOperacion, semanaOperacion, usuarioIng);
        }

        public string AnularTraslado(long codigoTraslado, string usuarioAct)
        {
            LiquidacionDAL obj = new LiquidacionDAL();
            return obj.AnularTraslado(codigoTraslado, usuarioAct);
        }

        public string TrasladarParaLiquidacion(long codigoTraslado, string usuarioAct)
        {
            LiquidacionDAL obj = new LiquidacionDAL();
            return obj.TrasladarParaLiquidacion(codigoTraslado, usuarioAct);
        }

        public ReporteTrasladoLiquidacionCLS GetReporteTrasladoLiquidacion(long codigoTraslado, int anioOperacion, int semanaOperacion)
        {
            LiquidacionDAL obj = new LiquidacionDAL();
            return obj.GetReporteTrasladoLiquidacion(codigoTraslado, anioOperacion, semanaOperacion);
        }

    }
}
