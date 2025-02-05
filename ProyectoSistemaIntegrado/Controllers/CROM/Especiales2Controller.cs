﻿using CapaDatos;
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
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Security;
using System.Security.Permissions;
using System.Text;
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

        public IActionResult GeneracionTrasladosConta()
        {
            return View();
        }

        public IActionResult ImportEspeciales2()
        {
            return View();
        }

        public IActionResult ConsultaTrasladosEspeciales2()
        {
            return View();
        }

        public IActionResult PrintAPI([FromBody] List<TrasladoEspeciales2DetalleCLS> listaDetalle, int codigoTraslado, string fechaOperacionStr, string fechaGeneracionStr, decimal montoTotalDia)
        {
            string ipString = HttpContext.Session.GetString("Ip");
            int puerto = Convert.ToInt32(HttpContext.Session.GetString("Puerto"));

            //string ipString = (TempData["Ip"]).ToString();
            //int puerto = Convert.ToInt32(TempData["Puerto"]);

            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            SocketPermission socketPermission1 = new SocketPermission(PermissionState.Unrestricted);
            // Create a 'SocketPermission' object for two ip addresses.
            SocketPermission socketPermission2 = new SocketPermission(PermissionState.None);
            SecurityElement securityElement1 = socketPermission2.ToXml();
            // 'SocketPermission' object for 'Connect' permission
            SecurityElement securityElement2 = new SecurityElement("ConnectAccess");
            // Second 'SocketPermission' ip-address is '192.168.144.240' for 'All' transport types and
            // for 'All' ports for the ip-address.
            SecurityElement securityElement4 = new SecurityElement("URI", ipString);
            //securityElement2.AddChild(securityElement3);
            securityElement2.AddChild(securityElement4);
            //securityElement1.AddChild(securityElement2);
            // Obtain a 'SocketPermission' object using 'FromXml' method.
            socketPermission2.FromXml(securityElement1);
            // Obtain a 'SocketPermission' object using 'FromXml' method.
            socketPermission2.FromXml(securityElement1);
            Socket clientSock = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp
                );
            //clientSock.NoDelay = true;
            IPAddress ip = IPAddress.Parse(ipString);
            IPEndPoint remoteEP = new IPEndPoint(ip, puerto);
            clientSock.Connect(remoteEP);
            if (!clientSock.Connected)
            {
                return BadRequest("Printer is not connected");
            }
            Encoding enc = Encoding.Latin1;
            string GS = Convert.ToString((char)29);
            string ESC = Convert.ToString((char)27);
            string COMMAND = "";
            COMMAND = ESC + "@";
            COMMAND += GS + "V" + (char)1;
            //byte[] bse = 
            char[] bse = COMMAND.ToCharArray();
            byte[] paperCut = enc.GetBytes(bse);
            // Line feed hexadecimal values
            byte[] bEsc = new byte[4];
            // Sends an ESC/POS command to the printer to cut the paper

            string tituloTotal = "TOTAL";
            string tituloMontoTotalPorDia = "TOTAL DEL DIA:";
            string linea = new string('-', 40);
            string t = ("Fecha y Hora de Generación: " + fechaGeneracionStr + "\r\n");
            t = t + ("Fecha de las Operaciones: " + fechaOperacionStr + "\r\n");
            t = t + ("Traslado No.: " + codigoTraslado.ToString() + "\r\n");
            t = t + ("Usuario:" + objUsuario.IdUsuario + "\r\n");
            t = t + "\r\n";
            t = t + ("Listado Especiales 2 \r\n");
            decimal montoTotal = 0;
            t = t + ("Nombre                             Monto \r\n");
            foreach (TrasladoEspeciales2DetalleCLS dr in listaDetalle)
            {
                t = t + (dr.NombreClienteDepurado.PadRight(30).Substring(0, 30) + " ");
                t = t + dr.Monto.ToString("N2").PadLeft(10).Substring(0, 10);
                t = t + ("\r\n");
                montoTotal = montoTotal + dr.Monto;
            }
            t = t + (linea + "\r\n");
            t = t + (tituloTotal.PadRight(30).Substring(0, 30) + " ");
            t = t + montoTotal.ToString("N2").PadLeft(10).Substring(0, 10);
            t = t + "\r\n";
            t = t + "\r\n";
            t = t + "\r\n";
            t = t + "\r\n";
            
            if (montoTotalDia > 0)
            {
                t = t + (linea + "\r\n");
                t = t + (tituloMontoTotalPorDia.PadRight(30).Substring(0, 30) + " ");
                t = t + montoTotalDia.ToString("N2").PadLeft(10).Substring(0, 10);
                t = t + (linea + "\r\n");
            }
            t = t + "\r\n";
            t = t + "\r\n";
            t = t + "\r\n";
            t = t + "\r\n";
            t = t + "\r\n";

            char[] array = t.ToCharArray();
            byte[] byData = enc.GetBytes(array);
            clientSock.Send(byData);
            clientSock.Send(paperCut);
            //clientSock.DuplicateAndClose(2);
            clientSock.Close();
            return Ok(200);
        }

        public string GuardarTraslados([FromBody] List<TrasladoEspeciales2DetalleCLS> listDetalle, int codigoTraslado, string fechaOperacionStr)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            Especiales2BL obj = new Especiales2BL();
            DateTime FechaOperacion = DateTime.MinValue;
            try
            {
                FechaOperacion = Util.Conversion.ConvertDateSpanishToEnglish(fechaOperacionStr);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return obj.GuardarTraslados(listDetalle, codigoTraslado, FechaOperacion, objUsuario.IdUsuario);
        }

        public async Task<List<TrasladoEspeciales2CLS>> GetConsultaTraslados(string fechaOperacion)
        {
            CadenaConexion conexion = new CadenaConexion();
            string puerto = conexion.puerto;
            HttpClient client = new HttpClient();

            // Setting Base address.
            client.BaseAddress = new Uri("http://10.34.1.43:" + puerto + "/api/");

            var query = new Dictionary<string, string>()
            {
                ["fecha"] = fechaOperacion
            };

            // Setting content type
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var uri = QueryHelpers.AddQueryString("especiales2/consultatrasladospordia", query);

            //var uri = "especiales2/trasladosgenerados";
            List<TrasladoEspeciales2CLS> list = new List<TrasladoEspeciales2CLS>();
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string jsonArrayString = await response.Content.ReadAsStringAsync();
                JArray jsonArray = JArray.Parse(jsonArrayString);
                int codigoTraslado = Convert.ToInt32(jsonArray[0]["codigo_traslado"].ToString());
                if (codigoTraslado != 0)
                {
                    foreach (var value in jsonArray)
                    {
                        var row = new TrasladoEspeciales2CLS
                        {
                            CodigoTraslado = Convert.ToInt32(value["codigo_traslado"].ToString()),
                            FechaOperacionStr = value["fecha_operacion"].ToString(),
                            MontoTotal = Convert.ToDecimal(value["monto_total"].ToString()),
                            NumeroPedidos = Convert.ToInt32(value["numero_pedidos"].ToString()),
                            CodigoEstado = Convert.ToInt32(value["codigo_estado"].ToString()),
                            Estado = value["estado"].ToString(),
                            ObservacionesTraslado = value["observaciones_traslado"].ToString(),
                            UsuarioIngreso = value["usuario_ing"].ToString(),
                            FechaIngresoStr = value["fecha_ing"].ToString(),
                            FechaTrasladoStr = DateTime.Parse(value["fecha_traslado"].ToString()).ToString(),
                            PermisoImprimir = Convert.ToInt32(value["permiso_imprimir"].ToString())
                        };
                        list.Add(row);
                    }
                }
                else {
                    var row2 = new TrasladoEspeciales2CLS
                    {
                        CodigoTraslado = 0,
                        ObservacionesTraslado = jsonArray[0]["observaciones_traslado"].ToString()
                    };
                    list.Add(row2);
                }
            }
            return list;
        }

        public async Task<List<TrasladoEspeciales2CLS>> GetTrasladosEnProceso()
        {
            CadenaConexion conexion = new CadenaConexion();
            string puerto = conexion.puerto;
            HttpClient client = new HttpClient();

            // Setting Base address.
            client.BaseAddress = new Uri("http://10.34.1.43:" + puerto + "/api/");

            // Setting content type
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var uri = "especiales2/trasladosgenerados";
            List<TrasladoEspeciales2CLS> list = new List<TrasladoEspeciales2CLS>();
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string jsonArrayString = await response.Content.ReadAsStringAsync();
                JArray jsonArray = JArray.Parse(jsonArrayString);
                if (jsonArray.Count > 0)
                {
                    int codigoTraslado = Convert.ToInt32(jsonArray[0]["codigo_traslado"].ToString());
                    if (codigoTraslado != 0)
                    {
                        foreach (var value in jsonArray)
                        {
                            var row = new TrasladoEspeciales2CLS
                            {
                                CodigoTraslado = Convert.ToInt32(value["codigo_traslado"].ToString()),
                                FechaOperacionStr = value["fecha_operacion"].ToString(),
                                MontoTotal = Convert.ToDecimal(value["monto_total"].ToString()),
                                NumeroPedidos = Convert.ToInt32(value["numero_pedidos"].ToString()),
                                CodigoEstado = Convert.ToInt32(value["codigo_estado"].ToString()),
                                Estado = value["estado"].ToString(),
                                ObservacionesTraslado = value["observaciones_traslado"].ToString(),
                                UsuarioIngreso = value["usuario_ing"].ToString(),
                                FechaIngresoStr = value["fecha_ing"].ToString(),
                                FechaTrasladoStr = DateTime.Parse(value["fecha_traslado"].ToString()).ToString(),
                                PermisoAnular = Convert.ToInt32(value["permiso_anular"].ToString()),
                                PermisoTraslado = Convert.ToInt32(value["permiso_traslado"].ToString()),
                                PermisoImprimir = Convert.ToInt32(value["permiso_imprimir"].ToString()),
                                PermisoEditar = Convert.ToInt32(value["permiso_editar"].ToString()),
                                PermisoActualizar = Convert.ToInt32(value["permiso_actualizar"].ToString()),
                                MontoTotalDia = Convert.ToDecimal(value["monto_total_dia"].ToString())
                            };
                            list.Add(row);
                        }
                    }
                    else
                    {
                        var row2 = new TrasladoEspeciales2CLS
                        {
                            CodigoTraslado = 0,
                            ObservacionesTraslado = jsonArray[0]["observaciones_traslado"].ToString()
                        };
                        list.Add(row2);
                    }
                }
            }
            return list;
        }

        public async Task<List<TrasladoEspeciales2CLS>> GetTrasladosEnProcesoContabilidad()
        {
            CadenaConexion conexion = new CadenaConexion();
            string puerto = conexion.puerto;
            HttpClient client = new HttpClient();

            // Setting Base address.
            client.BaseAddress = new Uri("http://10.34.1.43:" + puerto + "/api/");

            // Setting content type
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var uri = "especiales2/trasladosgeneradosconta";
            List<TrasladoEspeciales2CLS> list = new List<TrasladoEspeciales2CLS>();
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string jsonArrayString = await response.Content.ReadAsStringAsync();
                JArray jsonArray = JArray.Parse(jsonArrayString);
                if (jsonArray.Count > 0)
                {
                    int codigoTraslado = Convert.ToInt32(jsonArray[0]["codigo_traslado"].ToString());
                    if (codigoTraslado != 0)
                    {
                        foreach (var value in jsonArray)
                        {
                            var row = new TrasladoEspeciales2CLS
                            {
                                CodigoTraslado = Convert.ToInt32(value["codigo_traslado"].ToString()),
                                FechaOperacionStr = value["fecha_operacion"].ToString(),
                                MontoTotal = Convert.ToDecimal(value["monto_total"].ToString()),
                                NumeroPedidos = Convert.ToInt32(value["numero_pedidos"].ToString()),
                                CodigoEstado = Convert.ToInt32(value["codigo_estado"].ToString()),
                                Estado = value["estado"].ToString(),
                                ObservacionesTraslado = value["observaciones_traslado"].ToString(),
                                UsuarioIngreso = value["usuario_ing"].ToString(),
                                FechaIngresoStr = value["fecha_ing"].ToString(),
                                FechaTrasladoStr = DateTime.Parse(value["fecha_traslado"].ToString()).ToString(),
                                PermisoAnular = Convert.ToInt32(value["permiso_anular"].ToString()),
                                PermisoTraslado = Convert.ToInt32(value["permiso_traslado"].ToString()),
                                PermisoImprimir = Convert.ToInt32(value["permiso_imprimir"].ToString()),
                                PermisoEditar = Convert.ToInt32(value["permiso_editar"].ToString()),
                                PermisoActualizar = Convert.ToInt32(value["permiso_actualizar"].ToString())
                            };
                            list.Add(row);
                        }
                    }
                    else
                    {
                        var row2 = new TrasladoEspeciales2CLS
                        {
                            CodigoTraslado = 0,
                            ObservacionesTraslado = jsonArray[0]["observaciones_traslado"].ToString()
                        };
                        list.Add(row2);
                    }
                }
            }
            return list;
        }

        public async Task<string> GenerarTrasladosDeEspeciales2(string fechaOperacion)
        {
            string resultado = "";
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            CadenaConexion conexion = new CadenaConexion();
            string puerto = conexion.puerto;
            HttpClient client = new HttpClient();

            // Setting Base address.
            client.BaseAddress = new Uri("http://10.34.1.43:" + puerto + "/api/");

            // Setting content type
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("fecha", fechaOperacion),
                new KeyValuePair<string, string>("usuario", objUsuario.IdUsuario)
            });

            List<TrasladoEspeciales2CLS> list = new List<TrasladoEspeciales2CLS>();
            HttpResponseMessage response = await client.PostAsync("especiales2/generar", content);
            resultado = await response.Content.ReadAsStringAsync();

            /*if (response.IsSuccessStatusCode)
            {
                resultado = await response.Content.ReadAsStringAsync();
            }
            else {
                resultado = await response.Content.ReadAsStringAsync();
            }*/

            return resultado;
        }

        public async Task<List<TrasladoEspeciales2CLS>> GetTrasladosParaImportacion()
        {
            CadenaConexion conexion = new CadenaConexion();
            string puerto = conexion.puerto;
            HttpClient client = new HttpClient();

            // Setting Base address.
            client.BaseAddress = new Uri("http://10.34.1.43:" + puerto + "/api/");

            // Setting content type
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var uri = "especiales2/importaciones";

            List<TrasladoEspeciales2CLS> list = new List<TrasladoEspeciales2CLS>();
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string jsonArrayString = await response.Content.ReadAsStringAsync();
                JArray jsonArray = JArray.Parse(jsonArrayString);
                if (jsonArray.Count > 0)
                {
                    int codigoTraslado = Convert.ToInt32(jsonArray[0]["codigo_traslado"].ToString());
                    if (codigoTraslado != 0)
                    {
                        foreach (var value in jsonArray)
                        {
                            var row = new TrasladoEspeciales2CLS
                            {
                                CodigoTraslado = Convert.ToInt32(value["codigo_traslado"].ToString()),
                                FechaOperacionStr = value["fecha_operacion"].ToString(),
                                MontoTotal = Convert.ToDecimal(value["monto_total"].ToString()),
                                NumeroPedidos = Convert.ToInt32(value["numero_pedidos"].ToString()),
                                CodigoEstado = Convert.ToInt32(value["codigo_estado"].ToString()),
                                Estado = value["estado"].ToString(),
                                ObservacionesTraslado = value["observaciones_traslado"].ToString(),
                                UsuarioIngreso = value["usuario_ing"].ToString(),
                                FechaIngresoStr = value["fecha_ing"].ToString(),
                                FechaTrasladoStr = DateTime.Parse(value["fecha_traslado"].ToString()).ToString(),
                                PermisoImportar = Convert.ToInt32(value["permiso_importar"].ToString()),
                                PermisoDepurar = Convert.ToInt32(value["permiso_depurar"].ToString()),
                                PermisoRegistrar = Convert.ToInt32(value["permiso_registrar"].ToString()),
                                PermisoEditar = Convert.ToInt32(value["permiso_editar"].ToString()),
                                PermisoInformacion = Convert.ToInt32(value["permiso_informacion"].ToString())
                            };
                            list.Add(row);
                        }
                    }
                    else
                    {
                        var row2 = new TrasladoEspeciales2CLS
                        {
                            CodigoTraslado = 0,
                            ObservacionesTraslado = jsonArray[0]["observaciones_traslado"].ToString()
                        };
                        list.Add(row2);
                    }
                }
            }
            return list;
        }

        public async Task<List<TrasladoEspeciales2CLS>> GetTrasladosParaImportacionPorFecha(string fechaOperacion)
        {
            CadenaConexion conexion = new CadenaConexion();
            string puerto = conexion.puerto;
            HttpClient client = new HttpClient();

            // Setting Base address.
            client.BaseAddress = new Uri("http://10.34.1.43:" + puerto + "/api/");

            var query = new Dictionary<string, string>()
            {
                ["fecha"] = fechaOperacion
            };

            // Setting content type
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var uri = QueryHelpers.AddQueryString("especiales2/importacionesporfecha", query);

            List<TrasladoEspeciales2CLS> list = new List<TrasladoEspeciales2CLS>();
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string jsonArrayString = await response.Content.ReadAsStringAsync();
                JArray jsonArray = JArray.Parse(jsonArrayString);
                if (jsonArray.Count > 0)
                {
                    int codigoTraslado = Convert.ToInt32(jsonArray[0]["codigo_traslado"].ToString());
                    if (codigoTraslado != 0)
                    {
                        foreach (var value in jsonArray)
                        {
                            var row = new TrasladoEspeciales2CLS
                            {
                                CodigoTraslado = Convert.ToInt32(value["codigo_traslado"].ToString()),
                                FechaOperacionStr = value["fecha_operacion"].ToString(),
                                MontoTotal = Convert.ToDecimal(value["monto_total"].ToString()),
                                NumeroPedidos = Convert.ToInt32(value["numero_pedidos"].ToString()),
                                CodigoEstado = Convert.ToInt32(value["codigo_estado"].ToString()),
                                Estado = value["estado"].ToString(),
                                ObservacionesTraslado = value["observaciones_traslado"].ToString(),
                                UsuarioIngreso = value["usuario_ing"].ToString(),
                                FechaIngresoStr = value["fecha_ing"].ToString(),
                                FechaTrasladoStr = DateTime.Parse(value["fecha_traslado"].ToString()).ToString(),
                                PermisoImportar = Convert.ToInt32(value["permiso_importar"].ToString()),
                                PermisoDepurar = Convert.ToInt32(value["permiso_depurar"].ToString()),
                                PermisoRegistrar = Convert.ToInt32(value["permiso_registrar"].ToString()),
                                PermisoEditar = Convert.ToInt32(value["permiso_editar"].ToString()),
                                PermisoInformacion = Convert.ToInt32(value["permiso_informacion"].ToString())
                            };
                            list.Add(row);
                        }
                    }
                    else
                    {
                        var row2 = new TrasladoEspeciales2CLS
                        {
                            CodigoTraslado = 0,
                            ObservacionesTraslado = jsonArray[0]["observaciones_traslado"].ToString()
                        };
                        list.Add(row2);
                    }
                }
            }
            return list;
        }

        public async Task<List<TrasladoEspeciales2DetalleCLS>> GetDetalleTrasladosEspeciales2(int codigoTraslado)
        {
            CadenaConexion conexion = new CadenaConexion();
            string puerto = conexion.puerto;
            HttpClient client = new HttpClient();

            // Setting Base address.
            client.BaseAddress = new Uri("http://10.34.1.43:" + puerto + "/api/");

            // Setting content type
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>()
            {
                ["codigo"] = codigoTraslado.ToString()
            };

            var uri = QueryHelpers.AddQueryString("especiales2/detalletraslado", query);

            List<TrasladoEspeciales2DetalleCLS> list = new List<TrasladoEspeciales2DetalleCLS>();
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string jsonArrayString = await response.Content.ReadAsStringAsync();
                JArray jsonArray = JArray.Parse(jsonArrayString);
                int codigoError = Convert.ToInt32(jsonArray[0]["codigo_traslado"].ToString());
                if (codigoError != 0)
                {
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
                                NombreCliente = value["nombre_cliente"].ToString(),
                                NombreClienteDepurado = value["nombre_cliente_depurado"].ToString(),
                                NumeroPedido = Convert.ToInt64(value["pedido"].ToString().Split('.')[0]),
                                Serie = value["serie"].ToString()
                            };
                            list.Add(row);
                        }
                    }
                    catch (Exception)
                    {
                        list = null;
                    }
                }
                else
                {
                    var row2 = new TrasladoEspeciales2DetalleCLS
                    {
                        CodigoTraslado = 0,
                        NombreClienteDepurado = jsonArray[0]["nombre_cliente_depurado"].ToString()
                    };
                    list.Add(row2);
                }
            }
            return list;
        }

        public async Task<List<TrasladoEspeciales2DetalleCLS>> GetDetalleTrasladosEspeciales2Edicion(int codigoTraslado)
        {
            CadenaConexion conexion = new CadenaConexion();
            string puerto = conexion.puerto;
            HttpClient client = new HttpClient();

            // Setting Base address.
            client.BaseAddress = new Uri("http://10.34.1.43:" + puerto + "/api/");

            // Setting content type
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var query = new Dictionary<string, string>()
            {
                ["codigo"] = codigoTraslado.ToString()
            };

            var uri = QueryHelpers.AddQueryString("especiales2/detalletrasladoedicion", query);

            List<TrasladoEspeciales2DetalleCLS> list = new List<TrasladoEspeciales2DetalleCLS>();
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string jsonArrayString = await response.Content.ReadAsStringAsync();
                JArray jsonArray = JArray.Parse(jsonArrayString);
                int codigoError = Convert.ToInt32(jsonArray[0]["codigo_traslado"].ToString());
                if (codigoError != 0)
                {
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
                                NombreCliente = value["nombre_cliente"].ToString(),
                                NombreClienteDepurado = value["nombre_cliente_depurado"].ToString(),
                                NumeroPedido = Convert.ToInt64(value["pedido"].ToString().Split('.')[0]),
                                Serie = value["serie"].ToString(),
                                PermisoAnular = Convert.ToInt32(value["permiso_anular"].ToString())
                            };
                            list.Add(row);
                        }
                    }
                    catch (Exception ex)
                    {
                        list = null;
                    }
                }
                else
                {
                    var row2 = new TrasladoEspeciales2DetalleCLS
                    {
                        CodigoTraslado = 0,
                        NombreClienteDepurado = jsonArray[0]["nombre_cliente_depurado"].ToString()
                    };
                    list.Add(row2);
                }
            }
            return list;
        }

        public async Task<string> CambiarEstadoTrasladoEspeciales2(int codigoTraslado, int codigoEstado)
        {
            string resultado = "";

            CadenaConexion conexion = new CadenaConexion();
            string puerto = conexion.puerto;
            HttpClient client = new HttpClient();

            // Setting Base address.
            client.BaseAddress = new Uri("http://10.34.1.43:" + puerto + "/api/");

            // Setting content type
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("codigo", codigoTraslado.ToString()),
                new KeyValuePair<string, string>("estado", codigoEstado.ToString())
            });

            List<TrasladoEspeciales2CLS> list = new List<TrasladoEspeciales2CLS>();
            HttpResponseMessage response = await client.PutAsync("especiales2/cambiarestado", content);
            resultado = await response.Content.ReadAsStringAsync();
            /*if (response.IsSuccessStatusCode)
            {
                resultado = response.ReasonPhrase;
            }
            else
            {
                resultado = response.ReasonPhrase;
            }*/
            return resultado;
        }


        public async Task<string> EliminarTrasladoEspeciales2(int codigoTraslado)
        {
            string resultado = "";

            CadenaConexion conexion = new CadenaConexion();
            string puerto = conexion.puerto;
            HttpClient client = new HttpClient();

            // Setting Base address.
            client.BaseAddress = new Uri("http://10.34.1.43:" + puerto + "/api/");

            // Setting content type
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var uri = "especiales2/eliminartraslado/" + codigoTraslado.ToString();

            List<TrasladoEspeciales2CLS> list = new List<TrasladoEspeciales2CLS>();
            HttpResponseMessage response = await client.DeleteAsync(uri);
            resultado = await response.Content.ReadAsStringAsync();
            /*if (response.IsSuccessStatusCode)
            {
                resultado = response.ReasonPhrase;
            }
            else
            {
                resultado = response.ReasonPhrase;
            }*/
            return resultado;
        }

        public async Task<string> EliminarDetalleTrasladoEspeciales2(string codigoEmpresa, string serie, decimal numeroPedido)
        {
            string resultado = "";

            CadenaConexion conexion = new CadenaConexion();
            string puerto = conexion.puerto;
            HttpClient client = new HttpClient();

            // Setting Base address.
            client.BaseAddress = new Uri("http://10.34.1.43:" + puerto + "/api/");

            // Setting content type
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var uri = "especiales2/eliminardetalletraslado/" + codigoEmpresa + "," + serie + "," + numeroPedido.ToString();

            List<TrasladoEspeciales2CLS> list = new List<TrasladoEspeciales2CLS>();
            HttpResponseMessage response = await client.DeleteAsync(uri);
            resultado = await response.Content.ReadAsStringAsync();
            /*if (response.IsSuccessStatusCode)
            {
                resultado = response.ReasonPhrase;
            }
            else
            {
                resultado = response.ReasonPhrase;
            }*/
            return resultado;
        }

        public async Task<string> ActualizarDetallesTrasladados(int codigoTraslado)
        {
            string resultado = "";
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            CadenaConexion conexion = new CadenaConexion();
            string puerto = conexion.puerto;
            HttpClient client = new HttpClient();

            // Setting Base address.
            client.BaseAddress = new Uri("http://10.34.1.43:" + puerto + "/api/");

            // Setting content type
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("codigo", codigoTraslado.ToString()),
                new KeyValuePair<string, string>("usuario", objUsuario.IdUsuario)
            });

            List<TrasladoEspeciales2CLS> list = new List<TrasladoEspeciales2CLS>();
            HttpResponseMessage response = await client.PostAsync("especiales2/modificaciondetalletrasladado", content);
            resultado = await response.Content.ReadAsStringAsync();

            /*if (response.IsSuccessStatusCode)
            {
                resultado = await response.Content.ReadAsStringAsync();
            }
            else {
                resultado = await response.Content.ReadAsStringAsync();
            }*/

            return resultado;
        }


    }
}
