using CapaDatos.Tesoreria;
using CapaEntidad.Administracion;
using CapaEntidad.Tesoreria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Tesoreria
{
    public class CajaChicaBL
    {
        public List<ProgramacionSemanalCLS> GetAniosTransacciones()
        {
            CajaChicaDAL obj = new CajaChicaDAL();
            return obj.GetAniosTransacciones();
        }

        public List<ConfiguracionCajaChicaCLS> GetCajasChicas()
        {
            CajaChicaDAL obj = new CajaChicaDAL();
            return obj.GetCajasChicas();
        }
        public List<ConfiguracionCajaChicaCLS> GetConfiguracionCajasChicas(string idUsuario, int esSuperAdmin)
        {
            CajaChicaDAL obj = new CajaChicaDAL();
            return obj.GetConfiguracionCajasChicas(idUsuario, esSuperAdmin);
        }

        public List<CajaChicaCLS> GetMovimientosEnConfiguracionCajasChicas()
        {
            CajaChicaDAL obj = new CajaChicaDAL();
            return obj.GetMovimientosEnConfiguracionCajasChicas();
        }

        public List<CajaChicaCLS> GetAllMovimientosEnCajasChicas(int codigoCajaChica, int anioOperacion, int codigoOperacion)
        {
            CajaChicaDAL obj = new CajaChicaDAL();
            return obj.GetAllMovimientosEnCajasChicas(codigoCajaChica, anioOperacion, codigoOperacion);
        }

        public CajaChicaComboCLS fillCombosNewCajaChica(string idUsuario, int esSuperAdmin)
        {
            CajaChicaDAL obj = new CajaChicaDAL();
            return obj.FillCombosNewCajaChica(idUsuario, esSuperAdmin);
        }

        public CajaChicaComboCLS FillCombosEditCajaChica(int anioOperacion, int semanaOperacion, string idUsuario, int esSuperAdmin)
        {
            CajaChicaDAL obj = new CajaChicaDAL();
            return obj.FillCombosEditCajaChica(anioOperacion, semanaOperacion, idUsuario, esSuperAdmin);
        }

        public string GuardarTransaccion(CajaChicaCLS objTransaccion, string usuarioIng)
        {
            CajaChicaDAL obj = new CajaChicaDAL();

            objTransaccion.CodigoTipoTransaccion = "F";
            objTransaccion.FechaOperacion = Util.Conversion.ConvertDateSpanishToEnglish(objTransaccion.FechaStr);
            objTransaccion.FechaDocumento = Util.Conversion.ConvertDateSpanishToEnglish(objTransaccion.FechaDocumentoStr);
            return obj.GuardarTransaccion(objTransaccion, usuarioIng);
        }

        public CajaChicaCLS GetDataTransaccion(long codigoTransaccion)
        {
            CajaChicaDAL obj = new CajaChicaDAL();
            return obj.GetDataTransaccion(codigoTransaccion);
        }

        public string RegistrarSolicitudDeCorreccion(long codigoTransaccion, int codigoTipoCorreccion, string observaciones, string usuarioIng)
        {
            CajaChicaDAL obj = new CajaChicaDAL();
            return obj.RegistrarSolicitudDeCorreccion(codigoTransaccion, codigoTipoCorreccion, observaciones, usuarioIng);
        }

        public List<CajaChicaCLS> GetSolicitudesDeCorreccion(int anioOperacion, int semanaOperacion, int codigoReporte)
        {
            CajaChicaDAL obj = new CajaChicaDAL();
            return obj.GetSolicitudesDeCorreccion(anioOperacion, semanaOperacion, codigoReporte);
        }

        public SolicitudCorreccionCLS GetDataCorreccion(long codigoTransaccion)
        {
            CajaChicaDAL obj = new CajaChicaDAL();
            return obj.GetDataCorreccion(codigoTransaccion);
        }

        public string AutorizarCorreccion(long codigoTransaccion, string observaciones, int codigoResultado, string usuarioAct)
        {
            CajaChicaDAL obj = new CajaChicaDAL();
            return obj.AutorizarCorreccion(codigoTransaccion, observaciones, codigoResultado, usuarioAct);
        }

        public string ActualizarTransaccion(CajaChicaCLS objTransaccion, int codigoTipoActualizacion, string usuarioAct)
        {
            CajaChicaDAL obj = new CajaChicaDAL();

            objTransaccion.CodigoTipoTransaccion = "F";
            objTransaccion.FechaOperacion = Util.Conversion.ConvertDateSpanishToEnglish(objTransaccion.FechaStr);
            objTransaccion.FechaDocumento = Util.Conversion.ConvertDateSpanishToEnglish(objTransaccion.FechaDocumentoStr);
            return obj.ActualizarTransaccion(objTransaccion, codigoTipoActualizacion, usuarioAct);
        }

        public List<CajaChicaCLS> GetTransaccionesCajaChica(int codigoCajaChica, string usuarioIng, int esSuperAdmin, int setSemanaAnterior)
        {
            CajaChicaDAL obj = new CajaChicaDAL();
            return obj.GetTransaccionesCajaChica(codigoCajaChica, usuarioIng, esSuperAdmin, setSemanaAnterior);
        }

        public List<CajaChicaCLS> GetTransaccionesCajaChicaConsulta(int codigoCajaChica, int anioOperacion, int semanaOperacion, string usuarioIng)
        {
            CajaChicaDAL obj = new CajaChicaDAL();
            return obj.GetTransaccionesCajaChicaConsulta(codigoCajaChica, anioOperacion, semanaOperacion, usuarioIng);
        }

        public List<CajaChicaCLS> ListarTransaccionesCajaChica(int codigoReporte)
        {
            CajaChicaDAL obj = new CajaChicaDAL();
            return obj.ListarTransaccionesCajaChica(codigoReporte);
        }

        public Boolean ExisteFactura(string nitProveedor, string serieFactura, long numeroFactura)
        {
            CajaChicaDAL obj = new CajaChicaDAL();
            return obj.ExisteFactura(nitProveedor, serieFactura, numeroFactura);
        }

        public string ExcluirFacturaDeCajaChica(long codigoTransaccion, int excluirFactura, int codigoMotivoExclusion, string usuarioIng, string observaciones)
        {
            CajaChicaDAL obj = new CajaChicaDAL();
            return obj.ExcluirFacturaDeCajaChica(codigoTransaccion, excluirFactura, codigoMotivoExclusion, usuarioIng, observaciones);
        }

        public string AnularTransaccionCajaChica(long codigoTransaccion, string observaciones, string usuarioAct)
        {
            CajaChicaDAL obj = new CajaChicaDAL();
            return obj.AnularTransaccionCajaChica(codigoTransaccion, observaciones, usuarioAct);
        }

        public string AnularMovimientoCajaChica(long codigoTransaccion, string observaciones, string usuarioAct)
        {
            CajaChicaDAL obj = new CajaChicaDAL();
            return obj.AnularMovimientoCajaChica(codigoTransaccion, observaciones, usuarioAct);
        }

        public List<MotivoExclusionCLS> GetMotivosExclusionDeFacturas()
        {
            CajaChicaDAL obj = new CajaChicaDAL();
            return obj.GetMotivosExclusionDeFacturas();
        }

        public string ActualizarTransaccionCajaChica(long codigoTransaccion, int codigoOperacion, string descripcion, string usuarioAct)
        {
            CajaChicaDAL obj = new CajaChicaDAL();
            return obj.ActualizarTransaccionCajaChica(codigoTransaccion, codigoOperacion, descripcion, usuarioAct);
        }

        public string ActualizarMontoDisponibleCajaChica(int codigoCajaChica, int codigoOperacion, int anioOperacion, int semanaOperacion, decimal monto, string observaciones, string usuarioAct)
        {
            CajaChicaDAL obj = new CajaChicaDAL();
            return obj.ActualizarMontoDisponibleCajaChica(codigoCajaChica, codigoOperacion, anioOperacion, semanaOperacion, monto, observaciones, usuarioAct);
        }

        public string ActualizarMontosCajaChicaAdmin(int codigoCajaChica, decimal montoLimite, decimal montoDisponible, string observaciones, string usuarioAct)
        {
            CajaChicaDAL obj = new CajaChicaDAL();
            return obj.ActualizarMontosCajaChicaAdmin(codigoCajaChica, montoLimite, montoDisponible, observaciones, usuarioAct);
        }

        public decimal GetSaldoActual(int codigoCajaChica)
        {
            CajaChicaDAL obj = new CajaChicaDAL();
            return obj.GetSaldoActual(codigoCajaChica);
        }

        public List<CajaChicaCLS> GetTransaccionesPorRecepcionarEnTesoreria(string usuarioIng, int esSuperAdmin)
        {
            CajaChicaDAL obj = new CajaChicaDAL();
            return obj.GetTransaccionesPorRecepcionarEnTesoreria(usuarioIng, esSuperAdmin);

        }

        public string RecepcionarTransaccion(TransaccionCajaChicaCLS objTransaccion, string usuarioAct)
        {
            CajaChicaDAL obj = new CajaChicaDAL();

            if (objTransaccion.OrigenRecepcion == Constantes.CajaChica.OrigenRecepcion.TESORERIA)
            {
                return obj.RecepcionarTransaccion(objTransaccion, usuarioAct, Util.Conversion.DayOfWeek(DateTime.Now), DateTime.Now);
            }
            else
            {
                DateTime fechaOperacion = Util.Conversion.ConvertDateSpanishToEnglish(objTransaccion.FechaOperacionStr);
                return obj.RecepcionarTransaccion(objTransaccion, usuarioAct, Util.Conversion.DayOfWeek(fechaOperacion), fechaOperacion);
            }
            
        }



    }

}
