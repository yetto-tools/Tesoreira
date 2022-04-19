using CapaDatos.Tesoreria;
using CapaEntidad.Tesoreria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Tesoreria
{
    public class ConfiguracionSueldoIndirectoBL
    {
        public string GuardarConfiguracion(ConfiguracionSueldoIndirectoCLS objConfiguracion, string usuarioIng)
        {
            ConfiguracionSueldoIndirectoDAL obj = new ConfiguracionSueldoIndirectoDAL();
            return obj.GuardarConfiguracion(objConfiguracion, usuarioIng);
        }

        public string ActualizarConfiguracion(ConfiguracionSueldoIndirectoCLS objConfiguracion, string usuarioAct)
        {
            ConfiguracionSueldoIndirectoDAL obj = new ConfiguracionSueldoIndirectoDAL();
            return obj.ActualizarConfiguracion(objConfiguracion, usuarioAct);
        }

        public List<ConfiguracionSueldoIndirectoCLS> GetConfiguracionSueldoIndirecto(int anio)
        {
            ConfiguracionSueldoIndirectoDAL obj = new ConfiguracionSueldoIndirectoDAL();
            return obj.GetConfiguracionSueldoIndirecto(anio);
        }
    }
}
