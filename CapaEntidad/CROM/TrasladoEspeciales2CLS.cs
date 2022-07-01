using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad.CROM
{
    public class TrasladoEspeciales2CLS
    {
        public int CodigoTraslado { get; set; }
        public DateTime FechaOperacion { get; set; }
        public string FechaOperacionStr { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string FechaIngresoStr { get; set; }
        public DateTime FechaTraslado { get; set; }
        public string FechaTrasladoStr { get; set; }
        public decimal MontoTotal { get; set; }
        public int NumeroPedidos { get; set; }
        public int CodigoEstado { get; set; }
        public string Estado { get; set; }
        public string ObservacionesTraslado { get; set; }
        public string UsuarioIngreso { get; set; }
        public int PermisoAnular { get; set; }
        public int PermisoTraslado { get; set; }
        public int PermisoImprimir { get; set; }
        public int PermisoImportar { get; set; }
        public int PermisoDepurar { get; set; }
        public int PermisoRegistrar { get; set; }
        public int PermisoEditar { get; set; }
        public int PermisoActualizar { get; set; }
    }



}

