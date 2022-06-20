using CapaDatos.Tesoreria;
using CapaEntidad.Administracion;
using CapaEntidad.Tesoreria;
using CapaNegocio.Tesoreria;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Controllers.Tesoreria
{
    public class TrasladosEspeciales2Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public List<TrasladoEspeciales2DetalleCLS> GetTrasladosParaDepuracion(int codigoTraslado)
        {
            Especiales2DAL obj = new Especiales2DAL();
            return obj.GetTrasladosParaDepuracion(codigoTraslado);
        }


    }
}
