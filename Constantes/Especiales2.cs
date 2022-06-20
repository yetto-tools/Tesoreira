namespace Constantes
{
    public static class Especiales2
    {
        public static class EstadoTraslado
        {
            public const int ANULADO = 0;
            public const int RECEPCIONADO = 3;
            public const int EN_PROCESO_DE_DEPURACION = 4;
            public const int CARGA_COMPLETADA = 5;
        }

        public static class EstadoDetalleTraslado
        {
            public const int NO_DEPURADO = 1;
            public const int NO_EXISTE_CLIENTE = 2;
            public const int EXISTE_CLIENTE = 3;
            public const int EXISTE_COINCIDENCIA = 4;
            public const int NO_EXISTE_COINCIDENCIA = 5;
        }

        
    }
}
