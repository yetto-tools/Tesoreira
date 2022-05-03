using CapaDatos.RRHH;
using CapaDatos.Tesoreria;
using CapaEntidad.RRHH;
using CapaEntidad.Tesoreria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.RRHH
{
    public class EmpleadoBL
    {
        public List<EmpleadoCLS> GetListaEmpleados(int codigoEmpresa, int codigoArea, int codigoPuesto, int codigoEstado, int btb, int saldoPrestamo)
        {
            EmpleadoDAL obj = new EmpleadoDAL();
            return obj.GetListaEmpleados(codigoEmpresa, codigoArea, codigoPuesto, codigoEstado, btb, saldoPrestamo);
        }

        public List<EmpleadoCLS> GetListaEmpleadosRetirados(int codigoEmpresa, int codigoArea, int codigoPuesto)
        {
            EmpleadoDAL obj = new EmpleadoDAL();
            return obj.GetListaEmpleadosRetirados(codigoEmpresa, codigoArea, codigoPuesto);
        }

        public EmpleadoComboCLS FillCombosNewEmpleado()
        {
            EmpleadoDAL obj = new EmpleadoDAL();
            return obj.GetListFillCombosNewEmpleado();
        }

        public EmpleadoComboCLS GetListFillCombosConsulta()
        {
            EmpleadoDAL obj = new EmpleadoDAL();
            return obj.GetListFillCombosConsulta();
        }

        public string GuardarEmpleado(EmpleadoCLS objEmpleado, string usuarioIng)
        {
            EmpleadoDAL obj = new EmpleadoDAL();
            objEmpleado.FechaIngreso = Util.Conversion.ConvertDateSpanishToEnglish(objEmpleado.FechaIngresoStr);
            return obj.GuardarEmpleado(objEmpleado, usuarioIng);
        }

        public string ActualizarEmpleado(EmpleadoCLS objEmpleado, string usuarioAct)
        {
            EmpleadoDAL obj = new EmpleadoDAL();
            objEmpleado.FechaIngreso = Util.Conversion.ConvertDateSpanishToEnglish(objEmpleado.FechaIngresoStr);
            return obj.ActualizarEmpleado(objEmpleado, usuarioAct);
        }

        public string ActualizarEmpleadoOperacionPendiente(EmpleadoCLS objEmpleado, string usuarioAct)
        {
            EmpleadoDAL obj = new EmpleadoDAL();
            return obj.ActualizarEmpleadoOperacionPendiente(objEmpleado, usuarioAct);
        }

        public string ActualizarEmpleadoPlanilla(EmpleadoCLS objEmpleado, string usuarioAct)
        {
            EmpleadoDAL obj = new EmpleadoDAL();
            objEmpleado.FechaIngreso = Util.Conversion.ConvertDateSpanishToEnglish(objEmpleado.FechaIngresoStr);
            return obj.ActualizarEmpleadoPlanilla(objEmpleado, usuarioAct);
        }

        public EmpleadoCLS GetDataEmpleado(string codigoEmpleado)
        {
            EmpleadoDAL obj = new EmpleadoDAL();
            return obj.GetDataEmpleado(codigoEmpleado);
        }

        public List<EntidadGenericaCLS> ListarEntidadesGenericasByCategoria(int codigoCategoria)
        {
            EmpleadoDAL obj = new EmpleadoDAL();
            return obj.ListarEntidadesGenericasByCategoria(codigoCategoria);
        }

        //public List<EntidadGenericaCLS> filtrarEntidadesGenericasByCategoria(int codigoCategoriaEntidad, string nombreEntidad)
        //{
        //    EmpleadoDAL obj = new EmpleadoDAL();
        //    return obj.FiltrarEntidadesGenericasByCategoria(codigoCategoriaEntidad, nombreEntidad);
        //}

        public string DarDeBajaEmpleadoPorSuspension(SuspensionCLS objSuspension, string usuarioAct)
        {
            EmpleadoDAL obj = new EmpleadoDAL();
            objSuspension.FechaInicioSuspension = Util.Conversion.ConvertDateSpanishToEnglish(objSuspension.FechaInicioSuspensionStr);
            objSuspension.FechaFinSuspension = Util.Conversion.ConvertDateSpanishToEnglish(objSuspension.FechaFinSuspensionStr);
            return obj.DarDeBajaEmpleadoPorSuspension(objSuspension, usuarioAct);
        }

        public string DarDeBajaEmpleado(SuspensionCLS objSuspension, string usuarioAct)
        {
            EmpleadoDAL obj = new EmpleadoDAL();
            objSuspension.FechaEgreso = Util.Conversion.ConvertDateSpanishToEnglish(objSuspension.FechaEgresoStr);
            return obj.DarDeBajaEmpleado(objSuspension, usuarioAct);
        }

    }
}
