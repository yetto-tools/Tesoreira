using CapaEntidad.Ventas;
using CapaNegocio.Ventas;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Controllers.Ventas
{
    public class CanalesVentasController : Controller
    {
        public List<CanalVentaCLS> GetCanalesDeVentas()
        {
            CanalVentaBL obj = new CanalVentaBL();
            return obj.GetCanalesDeVentas();
        }

        public List<RutaCLS> GetCanalVenta(int ruta)
        {
            CanalVentaBL obj = new CanalVentaBL();
            return obj.GetCanalVenta(ruta);
        }


    }



}
