using CapaDatos.Ventas;
using CapaEntidad.Ventas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Ventas
{
    public class CanalVentaBL
    {
        public List<CanalVentaCLS> GetCanalesDeVentas()
        {
            CanalVentaDAL obj = new CanalVentaDAL();
            return obj.GetCanalesDeVentas();
        }

        public List<RutaCLS> GetCanalVenta(int ruta)
        {
            CanalVentaDAL obj = new CanalVentaDAL();
            return obj.GetCanalVenta(ruta);
        }

    }
}
