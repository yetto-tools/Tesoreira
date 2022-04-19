using CapaEntidad.Tesoreria;
using CapaNegocio.Tesoreria;
using Microsoft.AspNetCore.Mvc;
using ProyectoSistemaIntegrado.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Controllers.Tesoreria
{
    public class EntidadCategoriaController : Controller
    {
        [ServiceFilter(typeof(Seguridad))]
        public IActionResult Index()
        {
            return View();
        }

        public List<EntidadCategoriaCLS> GetAllCategoriaEntidades()
        {
            EntidadCategoriaBL obj = new EntidadCategoriaBL();
            return obj.GetAllCategoriaEntidades();
        }

        public List<EntidadCategoriaCLS> GetCategoriaParaAsignacionDeOperacion()
        {
            EntidadCategoriaBL obj = new EntidadCategoriaBL();
            return obj.GetCategoriaParaAsignacionDeOperacion();
        }

        public List<EntidadCategoriaCLS> GetCategoriasParaRegistrarEntidad()
        {
            EntidadCategoriaBL obj = new EntidadCategoriaBL();
            return obj.GetCategoriasParaRegistrarEntidad();
        }

        public List<EntidadCategoriaCLS> filtrarCategoriaEntidades(string nombreCategoria)
        {
            EntidadCategoriaBL obj = new EntidadCategoriaBL();
            return obj.filtrarCategoriaEntidades(nombreCategoria);
        }


    }
}
