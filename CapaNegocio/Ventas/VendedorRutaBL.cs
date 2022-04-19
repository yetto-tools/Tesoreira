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
        public List<VendedorRutaCLS> GetListaVendedores(int codigoCanalVenta, bool incluirBloqueados)
        {
            VendedorRutaDAL obj = new VendedorRutaDAL();
            return obj.GetListaVendedores(codigoCanalVenta, incluirBloqueados);
        }

        public string BloquearVendedorRuta(string codigoVendedor, int codigoCanalVenta, int ruta)
        {
            VendedorRutaDAL obj = new VendedorRutaDAL();
            return obj.BloquearVendedorRuta(codigoVendedor, codigoCanalVenta, ruta);
        }

        public string DesbloquearVendedorRuta(string codigoVendedor, int codigoCanalVenta, int ruta)
        {
            VendedorRutaDAL obj = new VendedorRutaDAL();
            return obj.DesbloquearVendedorRuta(codigoVendedor, codigoCanalVenta, ruta);
        }

        public string GuardarVendedorRuta(VendedorRutaCLS objVendedorRuta, string usuarioIng)
        {
            VendedorRutaDAL obj = new VendedorRutaDAL();
            return obj.GuardarVendedorRuta(objVendedorRuta, usuarioIng);
        }

        public List<VendedorRutaCLS> GetRutasDelVendedor(int codigoCategoriaEntidad, string codigoVendedor)
        {
            VendedorRutaDAL obj = new VendedorRutaDAL();
            return obj.GetRutasDelVendedor(codigoCategoriaEntidad, codigoVendedor);
        }
    }
}
