using CapaDatos.Administracion;
using CapaEntidad.Administracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Administracion
{
    public class CuentaBancariaBL
    {
        public List<CuentaBancariaCLS> GetCuentasBancarias(int codigoBanco)
        {
            CuentaBancariaDAL obj = new CuentaBancariaDAL();
            return obj.GetCuentasBancarias(codigoBanco);
        }

        public List<CuentaBancariaCLS> GetCuentasBancariasTesoreria(int codigoBanco)
        {
            CuentaBancariaDAL obj = new CuentaBancariaDAL();
            return obj.GetCuentasBancariasTesoreria(codigoBanco);
        }

        public string GetComboCuentasBancarias(int codigoBanco)
        {
            CuentaBancariaDAL obj = new CuentaBancariaDAL();
            return obj.GetComboCuentasBancarias(codigoBanco);
        }
    }
}
