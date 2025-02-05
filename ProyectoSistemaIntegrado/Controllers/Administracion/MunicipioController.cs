﻿using CapaEntidad.Administracion;
using CapaNegocio.Administracion;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Controllers.Administracion
{
    public class MunicipioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public List<MunicipioCLS> GetAllMunicipios(int codigoDepartamento)
        {
            MunicipioBL obj = new MunicipioBL();
            return obj.GetAllMunicipios(codigoDepartamento);
        }

    }
}
