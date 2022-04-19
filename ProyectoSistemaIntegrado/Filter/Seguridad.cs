using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSistemaIntegrado.Filter
{
    public class Seguridad : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("Prueba");

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Se valida si hay sesión o no
            string user = context.HttpContext.Session.GetString("usuario");
            if (user == null)
            {
                //context.Result = new RedirectResult("Login");
                context.Result = new RedirectResult("/Login/Index");
            }
        }
    }
}
