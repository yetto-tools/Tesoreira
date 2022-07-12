using CapaDatos;
using CapaEntidad.Administracion;
using CapaEntidad.CROM;
using CapaNegocio.CROM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProyectoSistemaIntegrado.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Controllers.CROM
{
    public class PedidosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<List<PedidoCLS>> GetPedidosAlCredito(string fechaCreditoStr)
        {
            CadenaConexion conexion = new CadenaConexion();
            string puerto = conexion.puerto;
            HttpClient client = new HttpClient();

            // Setting Base address.
            client.BaseAddress = new Uri("http://10.34.1.43:" + puerto + "/api/");

            var query = new Dictionary<string, string>()
            {
                ["fecha"] = fechaCreditoStr
            };

            // Setting content type
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var uri = QueryHelpers.AddQueryString("pedidos/ventascredito", query);

            List<PedidoCLS> list = new List<PedidoCLS>();
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string jsonArrayString = await response.Content.ReadAsStringAsync();
                JArray jsonArray = JArray.Parse(jsonArrayString);
                long numeroPedido = Convert.ToInt64(jsonArray[0]["pedido"].ToString().Split('.')[0]);
                if (numeroPedido != 0)
                {
                    foreach (var value in jsonArray)
                    {
                        var row = new PedidoCLS
                        {
                            CodigoEmpresa = value["empresa"].ToString(),
                            SeriePedido = value["serie"].ToString(),
                            NumeroPedido = Convert.ToInt64(value["pedido"].ToString().Split('.')[0]),
                            Monto = Convert.ToDecimal(value["monto"].ToString()),
                            CodigoCliente = value["codigo_cliente"].ToString(),
                            NombreCliente = value["nombre_cliente"].ToString(),
                            SerieFactura = value["factura_serie"].ToString(),
                            NumeroFactura = Convert.ToInt64(value["factura"].ToString().Split('.')[0]),
                            NumeroVale = Convert.ToInt64(value["vale"].ToString().Split('.')[0]),
                            NumeroPedidoQSystems = Convert.ToInt64(value["qsys_pedido"].ToString().Split('.')[0]),
                            Observaciones = value["observaciones"].ToString(),
                            PermisoSelect = Convert.ToInt32(value["permiso_select"].ToString())
                        };
                        list.Add(row);
                    }
                }
                else
                {
                    var row2 = new PedidoCLS
                    {
                        NumeroPedido = 0,
                        Observaciones = jsonArray[0]["observaciones"].ToString()
                    };
                    list.Add(row2);
                }
            }
            return list;
        }

    }
}
