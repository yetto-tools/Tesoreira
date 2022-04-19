using CapaDatos.Administracion;
using CapaEntidad.Administracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Administracion
{
    public class ProgramacionSemanalBL
    {
        public List<ProgramacionSemanalCLS> GetAniosProgramacionSemanal()
        {
            ProgramacionSemanalDAL obj = new ProgramacionSemanalDAL();
            return obj.GetAniosProgramacionSemanal();
        }

        public List<ProgramacionSemanalCLS> GetDiasOperacion(int anio, int numeroSemana)
        {
            ProgramacionSemanalDAL obj = new ProgramacionSemanalDAL();
            return obj.GetDiasOperacion(anio, numeroSemana);
        }

        public List<ProgramacionSemanalCLS> GetListaSemanasComision(int anio, int numeroSemana, int ultimaSemanaAnioAnterior)
        {
            ProgramacionSemanalDAL obj = new ProgramacionSemanalDAL();
            return obj.GetListaSemanasComision(anio, numeroSemana, ultimaSemanaAnioAnterior);
        }

        public List<ProgramacionSemanalCLS> GetListaSemanasPlanilla(int anio, int numeroSemana)
        {
            ProgramacionSemanalDAL obj = new ProgramacionSemanalDAL();
            return obj.GetListaSemanasPlanilla(anio, numeroSemana);
        }

        public ProgramacionSemanalCLS GetSemanaActual()
        {
            ProgramacionSemanalDAL obj = new ProgramacionSemanalDAL();
            return obj.GetSemanaActual();
        }

        public List<ProgramacionSemanalCLS> GetSemanasAnteriores(int anio, int numeroSemanaActual, int numeroSemanas, int incluirSemanaActual)
        {
            ProgramacionSemanalDAL obj = new ProgramacionSemanalDAL();
            return obj.GetSemanasAnteriores(anio, numeroSemanaActual, numeroSemanas, incluirSemanaActual);
        }

    }


}
