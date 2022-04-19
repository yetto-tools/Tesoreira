using CapaEntidad.Administracion;
using CapaNegocio.Administracion;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Controllers.Administracion
{
    public class ProgramacionSemanalController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public List<ProgramacionSemanalCLS> GetAniosProgramacionSemanal()
        {
            ProgramacionSemanalBL obj = new ProgramacionSemanalBL();
            return obj.GetAniosProgramacionSemanal();
        }

        public List<ProgramacionSemanalCLS> GetDiasOperacion(int anio, int numeroSemana)
        {
            ProgramacionSemanalBL obj = new ProgramacionSemanalBL();
            return obj.GetDiasOperacion(anio, numeroSemana);
        }

        public List<ProgramacionSemanalCLS> GetListaSemanasComision(int anio, int numeroSemana, int ultimaSemanaAnioAnterior)
        {
            ProgramacionSemanalBL obj = new ProgramacionSemanalBL();
            return obj.GetListaSemanasComision(anio, numeroSemana, ultimaSemanaAnioAnterior);
        }

        public List<ProgramacionSemanalCLS> GetListaSemanasPlanilla(int anio, int numeroSemana)
        {
            ProgramacionSemanalBL obj = new ProgramacionSemanalBL();
            return obj.GetListaSemanasPlanilla(anio, numeroSemana);
        }

        public ProgramacionSemanalCLS GetSemanaActual()
        {
            ProgramacionSemanalBL obj = new ProgramacionSemanalBL();
            return obj.GetSemanaActual();
        }

        public List<ProgramacionSemanalCLS> GetSemanasAnteriores(int anio, int numeroSemanaActual, int numeroSemanas, int incluirSemanaActual)
        {
            ProgramacionSemanalBL obj = new ProgramacionSemanalBL();
            return obj.GetSemanasAnteriores(anio, numeroSemanaActual, numeroSemanas, incluirSemanaActual);
        }

    }
}
