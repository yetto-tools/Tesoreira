using CapaDatos.Contabilidad;
using CapaEntidad.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Contabilidad
{
    public class CuentaPorCobrarBL
    {
        public string CargarSaldosIniciales(int anioOperacion, int semanaOperacion, string usuarioIng)
        {
            CuentaPorCobrarDAL obj = new CuentaPorCobrarDAL();
            return obj.CargarSaldosIniciales(anioOperacion, semanaOperacion, usuarioIng);
        }

        public string CargarCxCTemporal(string usuarioAct)
        {
            CuentaPorCobrarDAL obj = new CuentaPorCobrarDAL();
            return obj.CargarCxCTemporal(usuarioAct);
        }

        public List<TipoCuentaPorCobrarCLS> GetListTiposCuentasPorCobrar()
        {
            CuentaPorCobrarDAL obj = new CuentaPorCobrarDAL();
            return obj.GetListTiposCuentasPorCobrar();
        }

        public string GuardarCuentaPorCobrar(CuentaPorCobrarCLS objCuentaPorCobrar, string usuarioIng, int cargaInicial)
        {
            switch (objCuentaPorCobrar.CodigoCategoriaEntidad)
            {
                case Constantes.Entidad.Categoria.VENDEDOR:
                    objCuentaPorCobrar.CodigoCategoria = Constantes.Entidad.Categoria.VENDEDORES;
                    break;
                case Constantes.Entidad.Categoria.RUTERO_LOCAL:
                    objCuentaPorCobrar.CodigoCategoria = Constantes.Entidad.Categoria.VENDEDORES;
                    break;
                case Constantes.Entidad.Categoria.RUTERO_INTERIOR:
                    objCuentaPorCobrar.CodigoCategoria = Constantes.Entidad.Categoria.VENDEDORES;
                    break;
                case Constantes.Entidad.Categoria.CAFETERIA:
                    objCuentaPorCobrar.CodigoCategoria = Constantes.Entidad.Categoria.VENDEDORES;
                    break;
                case Constantes.Entidad.Categoria.SUPERMERCADO:
                    objCuentaPorCobrar.CodigoCategoria = Constantes.Entidad.Categoria.VENDEDORES;
                    break;
                default:
                    objCuentaPorCobrar.CodigoCategoria = objCuentaPorCobrar.CodigoCategoriaEntidad;
                    break;
            }
            CuentaPorCobrarDAL obj = new CuentaPorCobrarDAL();
            return obj.GuardarCuentaPorCobrar(objCuentaPorCobrar, usuarioIng, cargaInicial);
        }

        public List<CuentaPorCobrarCLS> GetCuentasPorCobrarCargaInicial()
        {
            CuentaPorCobrarDAL obj = new CuentaPorCobrarDAL();
            return obj.GetCuentasPorCobrarCargaInicial();
        }

        public List<CuentaPorCobrarCLS> GetCuentasPorCobrarTemporal()
        {
            CuentaPorCobrarDAL obj = new CuentaPorCobrarDAL();
            return obj.GetCuentasPorCobrarTemporal();
        }

        public string ActualizarCuentaPorCobrarTemporal(CuentaPorCobrarCLS objCuentaPorCobrar, string usuarioAct)
        {
            CuentaPorCobrarDAL obj = new CuentaPorCobrarDAL();
            return obj.ActualizarCuentaPorCobrarTemporal(objCuentaPorCobrar, usuarioAct);
        }

        public string AnularCuentaPorCobrarTemporal(long codigoCuentaPorCobrar, string usuarioAct)
        {
            CuentaPorCobrarDAL obj = new CuentaPorCobrarDAL();
            return obj.AnularCuentaPorCobrarTemporal(codigoCuentaPorCobrar, usuarioAct);
        }

        public decimal GetMontoCuentaPorCobrar(int codigoTipoOperacion, int codigoOperacion, string codigoEntidad, int codigoCategoriaEntidad)
        {
            CuentaPorCobrarDAL obj = new CuentaPorCobrarDAL();
            return obj.GetMontoCuentaPorCobrar(codigoTipoOperacion, codigoOperacion, codigoEntidad, codigoCategoriaEntidad);
        }


    }
}
