using CapaDatos.CROM;
using CapaEntidad.CROM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.CROM
{
    public class Especiales2BL
    {
        public string GuardarTraslados(List<TrasladoEspeciales2DetalleCLS> listDetalle, int codigoTraslado, DateTime fechaOperacion, string usuarioIng)
        {
            Especiales2DAL obj = new Especiales2DAL();
            return obj.GuardarTraslados(listDetalle, codigoTraslado, fechaOperacion, usuarioIng);
        }
    }
}
