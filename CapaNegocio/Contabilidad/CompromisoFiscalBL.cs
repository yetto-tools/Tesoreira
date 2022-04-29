using CapaDatos.Contabilidad;
using CapaEntidad.Contabilidad;
using CapaEntidad.Tesoreria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Contabilidad
{
    public class CompromisoFiscalBL
    {
        public List<CompromisoFiscalCLS> GetCompromisosFiscales(int codigoEmpresa, int anioOperacion,  int semanaOperacion)
        {
            CompromisoFiscalDAL obj = new CompromisoFiscalDAL();
            return obj.GetCompromisosFiscales(codigoEmpresa, anioOperacion,  semanaOperacion);
        }

        public List<CompromisoFiscalDetalleCLS> GetDetalleCompromisoFiscal(int codigoEmpresa, int anioOperacion,  int semanaOperacion)
        {
            CompromisoFiscalDAL obj = new CompromisoFiscalDAL();
            return obj.GetDetalleCompromisoFiscal(codigoEmpresa, anioOperacion, semanaOperacion);
        }

        public List<CompromisoFiscalCLS> GetReportesCompromisoFiscal(int anioOperacion)
        {
            CompromisoFiscalDAL obj = new CompromisoFiscalDAL();
            return obj.GetReportesCompromisoFiscal(anioOperacion);
        }

        public List<CompromisoFiscalDetalleCLS> GetDetalleReporteCompromisoFiscal(int anioOperacion, int semanaOperacion)
        {
            CompromisoFiscalDAL obj = new CompromisoFiscalDAL();
            return obj.GetDetalleReporteCompromisoFiscal(anioOperacion, semanaOperacion);
        }

        public string CargarCompromisoFiscal(List<TransaccionCLS> listaTransacciones, string usuarioAct)
        {
            CompromisoFiscalDAL obj = new CompromisoFiscalDAL();
            return obj.CargarCompromisoFiscal(listaTransacciones, usuarioAct);
        }

        public CompromisoFiscalCLS GetMontoCompromisosFiscal(int anioOperacion, int semanaOperacion)
        {
            CompromisoFiscalDAL obj = new CompromisoFiscalDAL();
            return obj.GetMontoCompromisosFiscal(anioOperacion, semanaOperacion);
        }


    }
}
