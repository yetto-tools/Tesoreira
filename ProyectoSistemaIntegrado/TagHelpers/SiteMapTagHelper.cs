using CapaEntidad.Administracion;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.TagHelpers
{
    public class SiteMapTagHelper : TagHelper
    {
        public List<SiteMapCLS> items { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            StringBuilder html = new StringBuilder();

            int contadorOpcionesSistema = 0;
            int contadorSubopcionesSistema = 0;
            int contadorOpciones = 0;
            int contadorSubOpciones = 0;
            int numeroItems = 0;
            int numeroSubItems = 0;
            foreach (var item in items)
            {
                if (item.Nivel == Constantes.SiteMap.Nivel.MENU)
                {
                    contadorOpcionesSistema = item.CantidadOpciones;
                    if (contadorOpcionesSistema == 0)
                    {
                        html.Append("<li class=\"nav-item\">");
                        html.Append("<a class=\"nav-link\" href=\"#\">" + item.Titulo + "</a>");
                        html.Append("</li>");
                    }
                    else
                    {
                        html.Append("<li class=\"nav-item dropdown\">");
                        html.Append("<a class=\"nav-link dropdown-toggle\" href=\"#\" data-bs-toggle=\"dropdown\">" + item.Titulo + "</a>");
                        html.Append("<ul class='dropdown-menu'>");
                    }
                }

                if (item.Nivel == Constantes.SiteMap.Nivel.OPCION)
                {
                    contadorSubopcionesSistema = item.CantidadSubOpciones;
                    numeroItems = item.CantidadItems;
                    numeroSubItems = item.CantidadSubItems;
                    contadorOpciones++;
                    if (contadorSubopcionesSistema == 0)
                    {
                        html.Append("<li><a class=\"dropdown-item\" href=\"/" + item.NombreController + "/" + item.NombreAction + "\">" + item.Titulo + "</a></li>");
                    }
                    else
                    {
                        html.Append("<li>");
                        html.Append("<a class=\"dropdown-item\" href=\"#\">" + item.Titulo + " &raquo;</a>");
                        html.Append("<ul class=\"submenu dropdown-menu\">");
                    }
                    if (contadorOpciones == numeroItems && item.CantidadSubOpciones == 0)
                    {
                        html.Append("</ul>");
                        html.Append("</li>");
                        contadorOpciones = 0;
                    }
                }

                if (item.Nivel == Constantes.SiteMap.Nivel.SUBOPCION)
                {
                    numeroSubItems = item.CantidadSubItems;
                    contadorSubOpciones++;
                    html.Append("<li><a class=\"dropdown-item\" href=\"/" + item.NombreController + "/" + item.NombreAction + "\">" + item.Titulo + "</a></li>");
                    if (contadorSubOpciones == numeroSubItems)
                    {
                        html.Append("</ul>");
                        html.Append("</li>");
                        contadorSubOpciones = 0;

                        if (contadorOpciones == numeroItems)
                        {
                            html.Append("</ul>");
                            html.Append("</li>");
                            contadorOpciones = 0;
                        }
                    }
                }
            }

            output.TagName = "li";
            output.Attributes.SetAttribute(new TagHelperAttribute("class", "inner-tag-helper"));
            output.PreContent.SetHtmlContent(html.ToString());
        }

        //public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        //{
        //    StringBuilder html = new StringBuilder();

        //    int contadorOpcionesSistema = 0;
        //    int contadorSubopcionesSistema = 0;
        //    int contadorOpciones = 0;
        //    int contadorSubOpciones = 0;
        //    int numeroItems = 0;
        //    int numeroSubItems = 0;
        //    //int valor = 0;
        //    foreach (var item in items)
        //    {
        //        /*if (item.Titulo == "Caja Chica")
        //        {
        //            valor = 1;
        //        }*/
        //        if (item.Nivel == Constantes.SiteMap.Nivel.MENU)
        //        {
        //            contadorOpcionesSistema = item.CantidadOpciones;
        //            if (contadorOpcionesSistema == 0)
        //            {
        //                html.Append("<li class=\"nav-item\">");
        //                html.Append("<a class=\"nav-link\" href=\"#\">" + item.Titulo + "</a>");
        //                html.Append("</li>");
        //            }
        //            else
        //            {
        //                html.Append("<li class=\"nav-item dropdown\">");
        //                html.Append("<a class=\"nav-link dropdown-toggle\" href=\"#\" data-bs-toggle=\"dropdown\">" + item.Titulo + "</a>");
        //                html.Append("<ul class='dropdown-menu'>");

        //            }
        //        }

        //        if (item.Nivel == Constantes.SiteMap.Nivel.OPCION)
        //        {
        //            contadorSubopcionesSistema = item.CantidadSubOpciones;
        //            numeroItems = item.CantidadItems;
        //            numeroSubItems = item.CantidadSubItems;
        //            contadorOpciones++;
        //            if (contadorSubopcionesSistema == 0)
        //            {
        //                html.Append("<li><a class=\"dropdown-item\" href=\"/" + item.NombreController + "/" + item.NombreAction + "\">" + item.Titulo + "</a></li>");
        //            }
        //            else
        //            {
        //                html.Append("<li>");
        //                html.Append("<a class=\"dropdown-item\" href=\"#\">" + item.Titulo + " &raquo;</a>");
        //                html.Append("<ul class=\"submenu dropdown-menu\">");
        //            }
        //            if (contadorOpciones == numeroItems && item.CantidadSubOpciones == 0)
        //            {
        //                html.Append("</ul>");
        //                html.Append("</li>");
        //                contadorOpciones = 0;
        //            }
        //        }

        //        if (item.Nivel == Constantes.SiteMap.Nivel.SUBOPCION)
        //        {
        //            numeroSubItems = item.CantidadSubItems;
        //            contadorSubOpciones++;
        //            html.Append("<li><a class=\"dropdown-item\" href=\"/" + item.NombreController + "/" + item.NombreAction + "\">" + item.Titulo + "</a></li>");
        //            if (contadorSubOpciones == numeroSubItems)
        //            {
        //                html.Append("</ul>");
        //                html.Append("</li>");
        //                contadorSubOpciones = 0;

        //                if (contadorOpciones == numeroItems)
        //                {
        //                    html.Append("</ul>");
        //                    html.Append("</li>");
        //                    contadorOpciones = 0;
        //                }
        //            }
        //        }
        //    }

        //    output.TagName = "li";
        //    output.Attributes.SetAttribute(new TagHelperAttribute("class", "inner-tag-helper"));
        //    output.PreContent.SetHtmlContent(html.ToString());
        //}

    }
}
