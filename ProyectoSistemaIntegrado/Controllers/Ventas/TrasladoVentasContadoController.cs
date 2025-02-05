﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad.Administracion;
using CapaEntidad.Ventas;
using CapaNegocio.Ventas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ProyectoSistemaIntegrado.Controllers.Ventas
{
    public class TrasladoVentasContadoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Generacion()
        {
            return View();
        }

        public IActionResult RecepcionEfectivoVentas()
        {
            return View();
        }

        public string GuardarTraslado(TrasladoVentasContadoCLS objTraslado)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            TrasladoVentasContadoBL obj = new TrasladoVentasContadoBL();
            return obj.GuardarTraslado(objTraslado, objUsuario.IdUsuario);
        }

        public List<TrasladoVentasContadoCLS> GetTrasladosEnProceso()
        {
            TrasladoVentasContadoBL obj = new TrasladoVentasContadoBL();
            return obj.GetTrasladosEnProceso();
        }

        public List<TrasladoVentasContadoCLS> GetTrasladosParaRecepcion(int codigoTipoTraslado)
        {
            TrasladoVentasContadoBL obj = new TrasladoVentasContadoBL();
            return obj.GetTrasladosParaRecepcion(codigoTipoTraslado);
        }

        public string AnularTraslado(int codigoTraslado)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            TrasladoVentasContadoBL obj = new TrasladoVentasContadoBL();
            return obj.AnularTraslado(codigoTraslado, objUsuario.IdUsuario);
        }

        public string AceptarTraslado(int codigoTraslado)
        {
            ViewBag.Message = HttpContext.Session.GetString("usuario");
            UsuarioCLS objUsuario = JsonConvert.DeserializeObject<UsuarioCLS>(ViewBag.Message);

            TrasladoVentasContadoBL obj = new TrasladoVentasContadoBL();
            return obj.AceptarTraslado(codigoTraslado, objUsuario.IdUsuario);
        }

        public IActionResult PrintConstanciaTrasladoVentasAlContado(int codigoTraslado, string fechaOperacionStr, string fechaGeneracionStr, decimal montoEfectivo, decimal montoCheques, decimal montoTransferencia, decimal montoTotal, decimal montoTotalDia)
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

            string lineaMontoEfectivo = "Monto Efectivo: ";
            string lineaMontoCheques = "Monto Cheques: ";
            string lineaMontoTransferencia = "Monto Transferencia: ";
            string lineaMontoTotal = "MONTO TOTAL: ";
            string tituloMontoTotalPorDia = "TOTAL DEL DIA:";

            string linea = new string('-', 40);
            string t = ("----------- CAJA ----------------------- \r\n");
            t = t + ("TRASLADO DE VENTAS AL CONTADO \r\n");
            t = t + ("Fecha Generacion: " + fechaGeneracionStr + "\r\n");
            t = t + ("Fecha Operacion: " + fechaOperacionStr + "\r\n");
            t = t + ("Usuario: " + objUsuario.IdUsuario + "\r\n");
            t = t + ("Traslado No.: " + codigoTraslado.ToString() + "\r\n");
            t = t + (linea + "\r\n");
            t = t + (lineaMontoEfectivo.PadRight(30).Substring(0, 30) + " ");
            t = t + montoEfectivo.ToString("N2").PadLeft(10).Substring(0, 10);
            t = t + "\r\n";
            t = t + (lineaMontoCheques.PadRight(30).Substring(0, 30) + " ");
            t = t + montoCheques.ToString("N2").PadLeft(10).Substring(0, 10);
            t = t + "\r\n";
            t = t + (lineaMontoTransferencia.PadRight(30).Substring(0, 30) + " ");
            t = t + montoTransferencia.ToString("N2").PadLeft(10).Substring(0, 10);
            t = t + "\r\n";
            t = t + (linea + "\r\n");
            t = t + (lineaMontoTotal.PadRight(30).Substring(0, 30) + " ");
            t = t + montoTotal.ToString("N2").PadLeft(10).Substring(0, 10);
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
            t = t + ("Impreso el " + DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss") + "\r\n");
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


    }



}
