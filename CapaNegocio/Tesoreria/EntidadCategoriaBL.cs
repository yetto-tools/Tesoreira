using CapaDatos.Tesoreria;
using CapaEntidad.Tesoreria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Tesoreria
{
    public class EntidadCategoriaBL
    {
        public List<EntidadCategoriaCLS> GetAllCategoriaEntidades()
        {
            EntidadCategoriaDAL obj = new EntidadCategoriaDAL();
            return obj.GetAllCategoriaEntidades();
        }

        public List<EntidadCategoriaCLS> GetCategoriaParaAsignacionDeOperacion()
        {
            EntidadCategoriaDAL obj = new EntidadCategoriaDAL();
            return obj.GetCategoriaParaAsignacionDeOperacion();
        }

        public List<EntidadCategoriaCLS> GetCategoriasParaRegistrarEntidad()
        {
            EntidadCategoriaDAL obj = new EntidadCategoriaDAL();
            return obj.GetCategoriasParaRegistrarEntidad();
        }

        public List<EntidadCategoriaCLS> filtrarCategoriaEntidades(string nombreCategoria)
        {
            EntidadCategoriaDAL obj = new EntidadCategoriaDAL();
            return obj.filtrarCategoriaEntidades(nombreCategoria);
        }



    }
}
