using CapaEntidad.Administracion;
using CapaEntidad.CROM;
using CapaNegocio.CROM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Controllers.CROM
{
    public class Especiales2Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GeneracionTraslados()
        {
            return View();
        }

        public IActionResult ImportEspeciales2()
        {
            return View();
        }

        public string GuardarTraslados([FromBody] List<TrasladoEspeciales2DetalleCLS> listDetalle, int codigoTraslado, string fechaOperacionStr)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            Especiales2BL obj = new Especiales2BL();
            DateTime FechaOperacion = Util.Conversion.ConvertDateSpanishToEnglish(fechaOperacionStr);

            return obj.GuardarTraslados(listDetalle, codigoTraslado, FechaOperacion, objUsuario.IdUsuario);
        }


        public async Task<List<TrasladoEspeciales2CLS>> GetTrasladosEnProceso()
        {
            HttpClient client = new HttpClient();

            // Setting Base address.
            client.BaseAddress = new Uri("http://localhost:5000");

            // Setting content type
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>()
            {
                ["env"] = "DEV"
            };

            var uri = QueryHelpers.AddQueryString("http://localhost:5000/api/especiales2/trasladosgenerados", query);

            List<TrasladoEspeciales2CLS> list = new List<TrasladoEspeciales2CLS>();
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string jsonArrayString = await response.Content.ReadAsStringAsync();
                JArray jsonArray = JArray.Parse(jsonArrayString);
                foreach (var value in jsonArray)
                {
                    var row = new TrasladoEspeciales2CLS
                    {
                        CodigoTraslado = Convert.ToInt32(value["codigo_traslado"].ToString()),
                        FechaOperacionStr = DateTime.Parse(value["fecha_operacion"].ToString()).ToString(),
                        MontoTotal = Convert.ToDecimal(value["monto_total"].ToString()),
                        NumeroPedidos = Convert.ToInt32(value["numero_pedidos"].ToString()),
                        CodigoEstado = Convert.ToInt32(value["codigo_estado"].ToString()),
                        Estado = value["estado"].ToString(),
                        ObservacionesTraslado = value["observaciones_traslado"].ToString(),
                        UsuarioIngreso = value["usuario_ing"].ToString(),
                        FechaIngresoStr = DateTime.Parse(value["fecha_ing"].ToString()).ToString(),
                        FechaTrasladoStr = DateTime.Parse(value["fecha_traslado"].ToString()).ToString(),
                        PermisoImprimir = Convert.ToInt32(value["permiso_imprimir"].ToString())
                    };
                    list.Add(row);
                }
            }
            return list;
        }

        public async Task<List<TrasladoEspeciales2CLS>> GenerarTrasladosDeEspeciales2(string fechaOperacion)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            HttpClient client = new HttpClient();

            // Setting Base address.
            //client.BaseAddress = new Uri("http://localhost:5000/api/");
            client.BaseAddress = new Uri("http://localhost:5000");

            // Setting content type
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("env", "DEV"),
                new KeyValuePair<string, string>("fecha", fechaOperacion),
                new KeyValuePair<string, string>("usuario", objUsuario.IdUsuario)
            });

            //var uri = "http://localhost:5000/api";
            //var uri = "/api/especiales2/traslados";
            //var jsonParameter = JsonConvert.SerializeObject(objTraslado);
            //var data = new StringContent(jsonParameter, Encoding.UTF8, "application/json");

            List<TrasladoEspeciales2CLS> list = new List<TrasladoEspeciales2CLS>();
            HttpResponseMessage response = await client.PostAsync("http://localhost:5000/api/especiales2/generar", content);
            if (response.IsSuccessStatusCode)
            {
                string jsonArrayString = await response.Content.ReadAsStringAsync();
                JArray jsonArray = JArray.Parse(jsonArrayString);
                var json = JObject.Parse(jsonArray[0].ToString());

                string mySqlTime = "00:00:00";
                DateTime time = DateTime.Parse(mySqlTime);

                var isoDateTimeFormat = CultureInfo.InvariantCulture.DateTimeFormat;
                foreach (var value in jsonArray)
                {
                    var row = new TrasladoEspeciales2CLS
                    {
                        CodigoTraslado = Convert.ToInt32(value["codigo_traslado"].ToString()),
                        FechaOperacionStr = DateTime.Parse(value["fecha_operacion"].ToString()).ToString(),
                        MontoTotal = Convert.ToDecimal(value["monto_total"].ToString()),
                        NumeroPedidos = Convert.ToInt32(value["numero_pedidos"].ToString()),
                        CodigoEstado = Convert.ToInt32(value["codigo_estado"].ToString()),
                        Estado = value["estado"].ToString(),
                        ObservacionesTraslado = value["observaciones_traslado"].ToString(),
                        UsuarioIngreso = value["usuario_ing"].ToString(),
                        FechaIngresoStr = DateTime.Parse(value["fecha_ing"].ToString()).ToString(),
                        FechaTrasladoStr = DateTime.Parse(value["fecha_traslado"].ToString()).ToString(),
                        PermisoImprimir = Convert.ToInt32(value["permiso_imprimir"].ToString())
                    };
                    list.Add(row);
                }
            }
            return list;
        }

        public async Task<string> AceptarGeneracionTrasladoEspeciales2(int codigoTraslado)
        {
            string resultado = "";

            HttpClient client = new HttpClient();

            // Setting Base address.
            client.BaseAddress = new Uri("http://localhost:5000");

            // Setting content type
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("env", "DEV"),
                new KeyValuePair<string, string>("codigo", codigoTraslado.ToString())
            });

            List<TrasladoEspeciales2CLS> list = new List<TrasladoEspeciales2CLS>();
            HttpResponseMessage response = await client.PutAsync("http://localhost:5000/api/especiales2/aceptargeneracion", content);
            if (response.IsSuccessStatusCode)
            {
                resultado = response.ReasonPhrase;
            }
            else {
                resultado = response.ReasonPhrase;
            }
            return resultado;
        }

        public async Task<List<TrasladoEspeciales2CLS>> GetTrasladosParaImportacion()
        {
            HttpClient client = new HttpClient();

            // Setting Base address.
            //client.BaseAddress = new Uri("http://localhost:5000/api/");
            client.BaseAddress = new Uri("http://localhost:5000");

            // Setting content type
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>()
            {
                ["env"] = "DEV"
            };

            var uri = QueryHelpers.AddQueryString("http://localhost:5000/api/especiales2/importaciones", query);

            List<TrasladoEspeciales2CLS> list = new List<TrasladoEspeciales2CLS>();
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string jsonArrayString = await response.Content.ReadAsStringAsync();
                JArray jsonArray = JArray.Parse(jsonArrayString);
                var json = JObject.Parse(jsonArray[0].ToString());

                string mySqlTime = "00:00:00";
                DateTime time = DateTime.Parse(mySqlTime);

                var isoDateTimeFormat = CultureInfo.InvariantCulture.DateTimeFormat;
                foreach (var value in jsonArray)
                {
                    var row = new TrasladoEspeciales2CLS
                    {
                        CodigoTraslado = Convert.ToInt32(value["codigo_traslado"].ToString()),
                        FechaOperacionStr = DateTime.Parse(value["fecha_operacion"].ToString()).ToString(),
                        MontoTotal = Convert.ToDecimal(value["monto_total"].ToString()),
                        NumeroPedidos = Convert.ToInt32(value["numero_pedidos"].ToString()),
                        CodigoEstado = Convert.ToInt32(value["codigo_estado"].ToString()),
                        Estado = value["estado"].ToString(),
                        ObservacionesTraslado = value["observaciones_traslado"].ToString(),
                        UsuarioIngreso = value["usuario_ing"].ToString(),
                        FechaIngresoStr = DateTime.Parse(value["fecha_ing"].ToString()).ToString(),
                        FechaTrasladoStr = DateTime.Parse(value["fecha_traslado"].ToString()).ToString(),
                        PermisoImportar = Convert.ToInt32(value["permiso_importar"].ToString()),
                        PermisoDepurar = Convert.ToInt32(value["permiso_depurar"].ToString())
                    };
                    list.Add(row);
                }
            }
            return list;
        }

        public async Task<List<TrasladoEspeciales2DetalleCLS>> GetDetalleTrasladosEspeciales2(int codigoTraslado)
        {
            HttpClient client = new HttpClient();

            // Setting Base address.
            //client.BaseAddress = new Uri("http://localhost:5000/api/");
            client.BaseAddress = new Uri("http://localhost:5000");

            // Setting content type
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>()
            {
                ["env"] = "DEV",
                ["codigo"] = codigoTraslado.ToString()
            };

            var uri = QueryHelpers.AddQueryString("http://localhost:5000/api/especiales2/detalletraslado", query);

            List<TrasladoEspeciales2DetalleCLS> list = new List<TrasladoEspeciales2DetalleCLS>();
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string jsonArrayString = await response.Content.ReadAsStringAsync();
                JArray jsonArray = JArray.Parse(jsonArrayString);
                var json = JObject.Parse(jsonArray[0].ToString());
                try
                {
                    foreach (var value in jsonArray)
                    {
                        var row = new TrasladoEspeciales2DetalleCLS
                        {
                            CodigoCliente = value["codigo_cliente"].ToString(),
                            CodigoTraslado = Convert.ToInt32(value["codigo_traslado"].ToString()),
                            CodigoEmpresa = value["empresa"].ToString(),
                            FechaGrabadoStr = DateTime.Parse(value["fecha_grabado"].ToString()).ToString(),
                            Monto = Convert.ToDecimal(value["monto"].ToString()),
                            NombreCliente = value["nombre_cliente_depurado"].ToString(),
                            NumeroPedido = Convert.ToInt64(value["pedido"].ToString().Split('.')[0]),
                            Serie = value["serie"].ToString()
                        };
                        list.Add(row);
                    }
                }
                catch (Exception ex)
                {
                    list = null;
                }
            }
            return list;
        }

        public async Task<string> AceptarImportacion(int codigoTraslado)
        {
            string resultado = "";

            HttpClient client = new HttpClient();

            // Setting Base address.
            client.BaseAddress = new Uri("http://localhost:5000");

            // Setting content type
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("env", "DEV"),
                new KeyValuePair<string, string>("codigo", codigoTraslado.ToString())
            });

            List<TrasladoEspeciales2CLS> list = new List<TrasladoEspeciales2CLS>();
            HttpResponseMessage response = await client.PutAsync("http://localhost:5000/api/especiales2/importacionrealizada", content);
            if (response.IsSuccessStatusCode)
            {
                resultado = response.ReasonPhrase;
            }
            else
            {
                resultado = response.ReasonPhrase;
            }
            return resultado;
        }



    }
}
