using CapaDatos.RRHH;
using CapaEntidad.Administracion;
using CapaEntidad.RRHH;
using CapaEntidad.Tesoreria;
using CapaNegocio.Tesoreria;
using CapaNegocio.Ventas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProyectoSistemaIntegrado.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Controllers.Tesoreria
{
    public class EntidadController : Controller
    {
        [ServiceFilter(typeof(Seguridad))]
        public IActionResult ConfigEntidades() 
        {
            return View();
        }

        public IActionResult AsignacionOperacionContable()
        {
            return View();
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        //public List<EntidadCLS> ListarEntidades()
        //{
        //    EntidadBL obj = new EntidadBL();
        //    return obj.ListarEntidades();
        //}

        public List<EntidadCLS> GetEntidadesGenericasConfiguracion(int codigoCategoriaEntidad)
        {
            EntidadBL obj = new EntidadBL();
            return obj.GetEntidadesGenericasConfiguracion(codigoCategoriaEntidad);
        }

        //public List<EntidadCLS> FiltrarEntidades(string nombreEntidad)
        //{
        //    EntidadBL obj = new EntidadBL();
        //    return obj.filtrarEntidades(nombreEntidad);
        //}

        public List<EntidadGenericaCLS> ListarEntidadesGenericasCxC()
        {
            EntidadBL obj = new EntidadBL();
            return obj.ListarEntidadesGenericasCxC();
        }

        public List<EntidadGenericaCLS> ListarEntidadesGenericasCxCPorPrestamosNoRegistradosEnTesoreria()
        {
            EntidadBL obj = new EntidadBL();
            return obj.ListarEntidadesGenericasCxCPorPrestamosNoRegistradosEnTesoreria();
        }


        public string GuardarEntidad(EntidadGenericaCLS objEntidad, string idUsuario)
        {
            ClienteBL objCliente = new ClienteBL();
            string resultado = "";
            switch (objEntidad.CodigoCategoriaEntidad)
            {
                case Constantes.Entidad.Categoria.CLIENTES_ESPECIALES_2:
                    resultado = objCliente.GuardarCliente(objEntidad, idUsuario);
                    break;
                case Constantes.Entidad.Categoria.CLIENTES_ESPECIALES_1:
                    resultado = objCliente.GuardarCliente(objEntidad, idUsuario);
                    break;
                case Constantes.Entidad.Categoria.EMPLEADO_INDIRECTO:
                    PersonaCLS objPersona = new PersonaCLS();
                    objPersona.PrimerNombre = objEntidad.PrimerNombre;
                    objPersona.SegundoNombre = objEntidad.SegundoNombre;
                    objPersona.PrimerApellido = objEntidad.PrimerApellido;
                    objPersona.SegundoApellido = objEntidad.SegundoApellido;
                    objPersona.CodigoGenero = objEntidad.CodigoGenero;
                    objPersona.NoIncluidoEnPlanilla = 1;
                    objPersona.CodigoArea = Constantes.Area.PRODUCCION;
                    PersonaDAL obj = new PersonaDAL();
                    resultado = obj.GuardarPersonaIndirecta(objPersona, idUsuario);
                    break;
                default:
                    EntidadBL objEntidadGenerica = new EntidadBL();
                    resultado = objEntidadGenerica.GuardarEntidad(objEntidad, idUsuario);
                    break;
            }

            return resultado;
        }

        public string ActualizarOperacionEntidad(int codigoEntidad, int codigoOperacion)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            EntidadBL obj = new EntidadBL();
            return obj.ActualizarOperacionEntidad(codigoEntidad, codigoOperacion, objUsuario.IdUsuario);
        }


    }



}
