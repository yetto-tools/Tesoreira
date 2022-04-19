using CapaDatos.Planilla;
using CapaEntidad.Planilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Planilla
{
    public class ConfiguracionDescuentoDevolucionBL
    {
        public List<ConfiguracionPrestamoCLS> GetEmpleadosCuentasPorCobrar(int codigoFrecuenciaPago)
        {
            ConfiguracionDescuentoDevolucionDAL obj = new ConfiguracionDescuentoDevolucionDAL();
            return obj.GetEmpleadosCuentasPorCobrar(codigoFrecuenciaPago);
        }

        public string RegistrarConfiguracionDevolucionBTB(int codigoEmpresa, string codigoEmpleado, decimal montoSalarioDiario, decimal montoBonoDecreto372001, string usuarioAct)
        {
            ConfiguracionDescuentoDevolucionDAL obj = new ConfiguracionDescuentoDevolucionDAL();
            return obj.RegistrarConfiguracionDevolucionBTB(codigoEmpresa, codigoEmpleado, montoSalarioDiario, montoBonoDecreto372001, usuarioAct);
        }

        public string RegistrarConfiguracionPrestamo(int codigoEmpresa, string codigoEmpleado, decimal montoDescuentoPrestamo, string usuarioAct)
        {
            ConfiguracionDescuentoDevolucionDAL obj = new ConfiguracionDescuentoDevolucionDAL();
            return obj.RegistrarConfiguracionPrestamo(codigoEmpresa, codigoEmpleado, montoDescuentoPrestamo, usuarioAct);
        }

        public List<ConfiguracionPrestamoCLS> GetEmpleadosBackToBack()
        {
            ConfiguracionDescuentoDevolucionDAL obj = new ConfiguracionDescuentoDevolucionDAL();
            return obj.GetEmpleadosBackToBack();
        }

    }
}
