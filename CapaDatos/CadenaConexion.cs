using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CadenaConexion
    {
        public string cadenaTesoreria { get; set; }

        public string cadenaRRHH { get; set; }
        public string cadenaQSystems { get; set; }

        public string cadenaAdmon { get; set; }

        public string cadenaVentas { get; set; }

        public string cadenaContabilidad { get; set; }

        public string cadenaCROM { get; set; }

        public CadenaConexion()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();

            string value = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            switch (value)
            {
                case "Development":
                    // Ambiente de Desarrollo
                    builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Development.json"));
                    break;
                case "Production":
                    // Ambiente de Produccion
                    builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
                    break;
                default:
                    // Ambiente Local
                    builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Development.json"));
                    break;
            }
            
            var root = builder.Build();

            cadenaTesoreria = root.GetConnectionString("SqlConnectionTesoreria");
            cadenaRRHH = root.GetConnectionString("SqlConnectionRRHH");
            cadenaAdmon = root.GetConnectionString("SqlConnectionAdmon");
            cadenaVentas = root.GetConnectionString("SqlConnectionVentas");
            cadenaContabilidad = root.GetConnectionString("SqlConnectionContabilidad");

            cadenaQSystems = root.GetConnectionString("SqlConnectionQsystems");
            cadenaCROM = root.GetConnectionString("SqlConnectionCROM");
        }
    }
}
