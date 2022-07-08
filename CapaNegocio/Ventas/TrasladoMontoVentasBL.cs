using CapaDatos.Ventas;
using CapaEntidad.Ventas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Ventas
{
    public class TrasladoMontoVentasBL
    {
        public string GuardarTraslado(TrasladoMontoVentasCLS objTraslado, string usuarioIng)
        {
            TrasladoMontoVentasDAL obj = new TrasladoMontoVentasDAL();

            objTraslado.FechaOperacion = Util.Conversion.ConvertDateSpanishToEnglish(objTraslado.FechaOperacionStr);
            objTraslado.Monto = objTraslado.MontoCheques + objTraslado.MontoEfectivo;
            return obj.GuardarTraslado(objTraslado, usuarioIng);
        }

        public List<TrasladoMontoVentasCLS> GetTrasladosEnProceso()
        {
            TrasladoMontoVentasDAL obj = new TrasladoMontoVentasDAL();
            return obj.GetTrasladosEnProceso();
        }

        public string AnularTraslado(int codigoTraslado, string usuarioAct)
        {
            TrasladoMontoVentasDAL obj = new TrasladoMontoVentasDAL();
            return obj.AnularTraslado(codigoTraslado, usuarioAct);
        }

        public string AceptarTraslado(int codigoTraslado, string usuarioAct)
        {
            TrasladoMontoVentasDAL obj = new TrasladoMontoVentasDAL();
            return obj.AceptarTraslado(codigoTraslado, usuarioAct);
        }


    }
}
