using CapaDatos.Ventas;
using CapaEntidad.Ventas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Ventas
{
    public class VendedorRutaBL
    {
        public List<VendedorRutaCLS> GetListaVendedores()
        {
            VendedorRutaDAL obj = new VendedorRutaDAL();
            return obj.GetListaVendedores();
        }
        public List<VendedorRutaCLS> GetListaVendedores(int codigoCanalVenta)
        {
            VendedorRutaDAL obj = new VendedorRutaDAL();
            return obj.GetListaVendedores(codigoCanalVenta);
        }

        public string AnularConfiguracionVendedorRuta(int codigoConfiguracion, string usuarioAct)
        {
            VendedorRutaDAL obj = new VendedorRutaDAL();
            return obj.AnularConfiguracionVendedorRuta(codigoConfiguracion, usuarioAct);
        }

        //public string BloquearVendedorRuta(string codigoVendedor, int codigoCanalVenta, int ruta)
        //{
        //    VendedorRutaDAL obj = new VendedorRutaDAL();
        //    return obj.BloquearVendedorRuta(codigoVendedor, codigoCanalVenta, ruta);
        //}

        //public string DesbloquearVendedorRuta(string codigoVendedor, int codigoCanalVenta, int ruta)
        //{
        //    VendedorRutaDAL obj = new VendedorRutaDAL();
        //    return obj.DesbloquearVendedorRuta(codigoVendedor, codigoCanalVenta, ruta);
        //}

        public string GuardarVendedorRuta(VendedorRutaCLS objVendedorRuta, string usuarioIng)
        {
            VendedorRutaDAL obj = new VendedorRutaDAL();
            return obj.GuardarVendedorRuta(objVendedorRuta, usuarioIng);
        }

        public string ActualizarConfiguracionVendedorRuta(VendedorRutaCLS objVendedorRuta, string usuarioAct)
        {
            VendedorRutaDAL obj = new VendedorRutaDAL();
            return obj.ActualizarConfiguracionVendedorRuta(objVendedorRuta, usuarioAct);
        }

        public List<VendedorRutaCLS> GetRutasDelVendedor(int codigoCategoriaEntidad, string codigoVendedor)
        {
            VendedorRutaDAL obj = new VendedorRutaDAL();
            return obj.GetRutasDelVendedor(codigoCategoriaEntidad, codigoVendedor);
        }

        public int ExisteConfiguracionVendedorRuta(string codigoVendedor, int codigoCanalVenta, int ruta)
        {
            VendedorRutaDAL obj = new VendedorRutaDAL();
            return obj.ExisteConfiguracionVendedorRuta(codigoVendedor, codigoCanalVenta, ruta);
        }


    }
}
