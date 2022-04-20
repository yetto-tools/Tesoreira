using CapaDatos.Ventas;
using CapaEntidad.Tesoreria;
using CapaEntidad.Ventas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Ventas
{
    public class ClienteBL
    {
        public string GuardarCliente(EntidadGenericaCLS objEntidad, string usuarioIng)
        {
            ClienteDAL obj = new ClienteDAL();
            return obj.GuardarCliente(objEntidad, usuarioIng);
        }

        public List<ClienteCLS> GetListAllClientes()
        {
            ClienteDAL obj = new ClienteDAL();
            return obj.GetListAllClientes();
        }
    }
}
