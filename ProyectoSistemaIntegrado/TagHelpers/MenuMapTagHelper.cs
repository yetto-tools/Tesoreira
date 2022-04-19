using CapaEntidad.Administracion;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.TagHelpers
{
    public class MenuMapTagHelper : TagHelper
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
            html.Append("<div id='jstree'>");
            html.Append("<ul>");
            foreach (var item in items)
            {
                if (item.Nivel == Constantes.SiteMap.Nivel.MENU)
                {
                    contadorOpcionesSistema = item.CantidadOpciones;
                    if (contadorOpcionesSistema == 0)
                    {
                        html.Append("<li value='" + item.CodigoSitemap.ToString() + "' onclick='clickSelectItemTree()'>");
                        html.Append("[" + item.CodigoSitemap.ToString() + "]" + item.Titulo);
                        html.Append("</li>");
                    }
                    else
                    {
                        html.Append("<li value='" + item.CodigoSitemap.ToString() + "' onclick='clickSelectItemTree()'>");
                        html.Append("[" + item.CodigoSitemap.ToString() + "]" + item.Titulo);
                        html.Append("<ul>");

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
                        html.Append("<li value='" + item.CodigoSitemap.ToString() + "' onclick='clickSelectItemTree()'>" + "[" + item.CodigoSitemap.ToString() + "]" + item.Titulo + "</li>");
                    }
                    else
                    {
                        html.Append("<li value='" + item.CodigoSitemap.ToString() + "' onclick='clickSelectItemTree()'>");
                        html.Append("[" + item.CodigoSitemap.ToString() + "]" + item.Titulo);
                        html.Append("<ul>");
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
                    html.Append("<li value='" + item.CodigoSitemap.ToString() + "' onclick='clickSelectItemTree()'>" + "[" + item.CodigoSitemap.ToString() + "]" + item.Titulo + "</li>");
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
            html.Append("</ul>");
            html.Append("</div>");
            output.PreContent.SetHtmlContent(html.ToString());
        }
    }
}
