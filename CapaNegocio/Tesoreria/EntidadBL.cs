using CapaDatos.Tesoreria;
using CapaEntidad.Tesoreria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Tesoreria
{
    public class EntidadBL
    {

        public string GuardarEntidad(EntidadGenericaCLS objEntidad, string usuarioIng)
        {
            EntidadDAL obj = new EntidadDAL();
            return obj.GuardarEntidad(objEntidad, usuarioIng);

        }

        public List<EntidadCLS> GetEntidadesGenericasConfiguracion(int codigoCategoriaEntidad)
        {
            EntidadDAL obj = new EntidadDAL();
            return obj.GetEntidadesGenericasConfiguracion(codigoCategoriaEntidad);
        }

        public List<EntidadGenericaCLS> ListarEntidadesGenericas()
        {
            EntidadDAL obj = new EntidadDAL();
            return obj.ListarEntidadesGenericas();
        }

        public List<EntidadGenericaCLS> ListarEntidadesGenericasCxC()
        {
            EntidadDAL obj = new EntidadDAL();
            return obj.ListarEntidadesGenericasCxC();
        }

        public List<EntidadGenericaCLS> ListarEntidadesGenericasCxCPorPrestamosNoRegistradosEnTesoreria()
        {
            EntidadDAL obj = new EntidadDAL();
            return obj.ListarEntidadesGenericasCxCPorPrestamosNoRegistradosEnTesoreria();
        }

        public string ActualizarOperacionEntidad(int codigoEntidad, int codigoOperacion, string usuarioAct)
        {
            EntidadDAL obj = new EntidadDAL();
            return obj.ActualizarOperacionEntidad(codigoEntidad, codigoOperacion, usuarioAct);
        }

    }
}
