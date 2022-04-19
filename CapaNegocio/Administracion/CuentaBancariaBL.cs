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
        public List<CuentaBancariaCLS> GetCuentasBancariasTesoreria(int codigoBanco)
        {
            CuentaBancariaDAL obj = new CuentaBancariaDAL();
            return obj.GetCuentasBancariasTesoreria(codigoBanco);
        }
    }
}
