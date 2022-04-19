using CapaEntidad.Administracion;
using CapaEntidad.Tesoreria;
using CapaNegocio.Tesoreria;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Controllers
{
    public class BaseController : Controller
    {
        public byte[] ExportarExcelDatos<T>(string[] cabeceras, string[] nombrePropiedades, List<T> lista)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage ep = new ExcelPackage())
                {
                    ep.Workbook.Worksheets.Add("Hoja");
                    // Referencia a la Hoja que se ha agregado
                    ExcelWorksheet ew = ep.Workbook.Worksheets[0];
                    // Agregando cabeceras
                    for (int i = 0; i < cabeceras.Length; i++)
                    {
                        ew.Cells[1, i + 1].Value = cabeceras[i];
                        //ew.Column(i + 1).Width = 50;
                    }
                    // Agregar contenido
                    int fila = 2;
                    int columna = 1;
                    foreach (object item in lista)
                    {
                        columna = 1;
                        foreach (string propiedad in nombrePropiedades)
                        {
                            //var valor = item.GetType().GetProperty(propiedad).GetValue(item);
                            //ew.Cells[fila, columna].Style.Numberformat.Format = "0.00";
                            var prop = item.GetType().GetProperty(propiedad);
                            TypeCode tipo = Type.GetTypeCode(prop.PropertyType);
                            switch (tipo)
                            {
                                case TypeCode.String:
                                    ew.Cells[fila, columna].Value = item.GetType().GetProperty(propiedad).GetValue(item).ToString();
                                    break;
                                case TypeCode.DateTime:
                                    ew.Cells[fila, columna].Value = item.GetType().GetProperty(propiedad).GetValue(item).ToString();
                                    ew.Cells[fila, columna].Style.Numberformat.Format = "HH: mm";
                                    break;
                                case TypeCode.Int16:
                                    ew.Cells[fila, columna].Value = item.GetType().GetProperty(propiedad).GetValue(item);
                                    break;
                                case TypeCode.Int32:
                                    ew.Cells[fila, columna].Value = item.GetType().GetProperty(propiedad).GetValue(item);
                                    break;
                                case TypeCode.Int64:
                                    ew.Cells[fila, columna].Value = item.GetType().GetProperty(propiedad).GetValue(item);
                                    break;
                                case TypeCode.Decimal:
                                    ew.Cells[fila, columna].Value = item.GetType().GetProperty(propiedad).GetValue(item);
                                    ew.Cells[fila, columna].Style.Numberformat.Format = "0.00";
                                    break;
                                default:
                                    ew.Cells[fila, columna].Value = item.GetType().GetProperty(propiedad).GetValue(item).ToString();
                                    break;
                            }


                            columna++;
                        }
                        fila++;
                    }

                    ep.SaveAs(ms);
                    byte[] buffer = ms.ToArray();
                    return buffer;
                }
            }
        }


    }
}
