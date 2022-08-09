
namespace Constantes
{
    public static class CajaChica
    {
        public static class EstadoTransaccion
        {
            public const int ANULADO = 0;
            public const int REGISTRADO = 1;
            public const int INCLUIDO_EN_REPORTE = 2;
            public const int DESEMBOLSADO = 3;
        }
        public static class EstadoReporte {
            public const int ANULADO = 0;
            public const int GENERAR_REPORTE = 1;
            public const int REPORTE_GENERADO = 2;
            public const int EN_REVISION = 3;
            public const int REVISADO = 4;
            public const int REEMBOLSADO = 5;
        }

        public static class EstadoRecepcion
        {
            public const int ANULADO = 0;
            public const int POR_RECEPCIONAR = 1;
            public const int RECEPCIONADO = 2;
            public const int REGISTRADO = 3;
        }

        public static class OrigenRecepcion
        {
            public const int TESORERIA = 1;
            public const int CONTABILIDAD = 2;
        }


    }

}

