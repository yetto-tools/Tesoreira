using CapaDatos.Tesoreria;
using CapaEntidad.Tesoreria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Tesoreria
{
    public class OperacionBL
    {
        public OperacionComboCLS FillComboOperacion(int codigoTipoOperacion)
        {
            OperacionDAL obj = new OperacionDAL();
            return obj.GetListOperaciones(codigoTipoOperacion);
        }

        public List<OperacionCLS> GetOperacionesParaAsignacionAEntidadesGenericas()
        {
            OperacionDAL obj = new OperacionDAL();
            return obj.GetOperacionesParaAsignacionAEntidadesGenericas();
        }

        public List<OperacionCLS> GetListOperacionesCajaChica()
        {
            OperacionDAL obj = new OperacionDAL();
            return obj.GetListOperacionesCajaChica();
        }

    }
}
