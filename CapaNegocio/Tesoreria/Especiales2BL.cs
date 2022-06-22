using CapaDatos.Tesoreria;
using CapaEntidad.Tesoreria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Tesoreria
{
    public class Especiales2BL
    {
        public List<TrasladoEspeciales2DetalleCLS> GetTrasladosParaDepuracion(int codigoTraslado)
        {
            Especiales2DAL obj = new Especiales2DAL();
            return obj.GetTrasladosParaDepuracion(codigoTraslado);
        }

        public string ActualizarDetalleEspeciales2(List<TrasladoEspeciales2DetalleCLS> listDetalle, int codigoTraslado, string usuarioAct)
        {
            Especiales2DAL obj = new Especiales2DAL();
            return obj.ActualizarDetalleEspeciales2(listDetalle, codigoTraslado, usuarioAct);
        }

        public List<TrasladoEspeciales2DetalleCLS> GetDetalleEspeciales2(int codigoTraslado)
        {
            Especiales2DAL obj = new Especiales2DAL();
            return obj.GetDetalleEspeciales2(codigoTraslado);
        }

        public string RegistrarEspeciales2(List<TrasladoEspeciales2DetalleCLS> listDetalle, int codigoTraslado, DateTime fechaOperacion, int semanaOperacion, int anioOperacion, string usuarioIng)
        {
            Especiales2DAL obj = new Especiales2DAL();
            return obj.RegistrarEspeciales2(listDetalle, codigoTraslado, fechaOperacion, semanaOperacion, anioOperacion, usuarioIng);
        }

    }
}
