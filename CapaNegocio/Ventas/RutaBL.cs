using CapaDatos.Ventas;
using CapaEntidad.Ventas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Ventas
{
    public class RutaBL
    {
        public string GuardarRuta(RutaCLS objRuta, string usuarioIng)
        {
            RutaDAL obj = new RutaDAL();
            return obj.GuardarRuta(objRuta, usuarioIng);
        }
        public List<RutaCLS> GetListaRutas()
        {
            RutaDAL obj = new RutaDAL();
            return obj.GetListaRutas();
        }

        public List<RutaCLS> GetRutas()
        {
            RutaDAL obj = new RutaDAL();
            return obj.GetRutas();
        }

        public string ActualizarRuta(RutaCLS objRuta, string usuarioAct)
        {
            RutaDAL obj = new RutaDAL();
            return obj.ActualizarRuta(objRuta, usuarioAct);
        }

    }
}
