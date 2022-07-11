using CapaDatos.Ventas;
using CapaEntidad.Ventas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Ventas
{
    public class TrasladoVentasContadoBL
    {
        public string GuardarTraslado(TrasladoVentasContadoCLS objTraslado, string usuarioIng)
        {
            TrasladoVentasContadoDAL obj = new TrasladoVentasContadoDAL();

            objTraslado.FechaOperacion = Util.Conversion.ConvertDateSpanishToEnglish(objTraslado.FechaOperacionStr);
            objTraslado.Monto = objTraslado.MontoCheques + objTraslado.MontoEfectivo + objTraslado.MontoTransferencia;
            return obj.GuardarTraslado(objTraslado, usuarioIng);
        }

        public List<TrasladoVentasContadoCLS> GetTrasladosEnProceso()
        {
            TrasladoVentasContadoDAL obj = new TrasladoVentasContadoDAL();
            return obj.GetTrasladosEnProceso();
        }

        public string AnularTraslado(int codigoTraslado, string usuarioAct)
        {
            TrasladoVentasContadoDAL obj = new TrasladoVentasContadoDAL();
            return obj.AnularTraslado(codigoTraslado, usuarioAct);
        }

        public string AceptarTraslado(int codigoTraslado, string usuarioAct)
        {
            TrasladoVentasContadoDAL obj = new TrasladoVentasContadoDAL();
            return obj.AceptarTraslado(codigoTraslado, usuarioAct);
        }

        public List<TrasladoVentasContadoCLS> GetTrasladosParaRecepcion(int codigoTipoTraslado)
        {
            TrasladoVentasContadoDAL obj = new TrasladoVentasContadoDAL();
            return obj.GetTrasladosParaRecepcion(codigoTipoTraslado);
        }

    }
}
