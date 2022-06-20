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
    }
}
