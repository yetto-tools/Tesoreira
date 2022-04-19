using CapaDatos.Ventas;
using CapaEntidad.Ventas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Ventas
{
    public class VendedorBL
    {
        public List<VendedorCLS> GetVendedores()
        {
            VendedorDAL obj = new VendedorDAL();
            return obj.GetVendedores();
        }

        public string GuardarVendedor(VendedorRutaCLS objVendedor, string usuarioIng)
        {
            VendedorDAL obj = new VendedorDAL();
            return obj.GuardarVendedor(objVendedor, usuarioIng);
        }

    }
}
