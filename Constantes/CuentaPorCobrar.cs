namespace Constantes
{
    public static class CuentaPorCobrar
    {
        public static class Estado
        {
            public const short ANULADO = 0;
            public const short REGISTRADO = 1;
            public const short INCLUIDO_EN_REPORTE = 2;
            public const short TEMPORAL = 3;
            public const short PARA_INCLUIR_EN_REPORTE = 4;
        }

        public static class EstadoReporte
        {
            public const short ANULADO = 0;
            public const short POR_GENERAR = 1;
            public const short GENERADO = 2;
            public const short VALIDO = 3;
        }

        public static class Operacion
        {
            public const short ANTICIPO_LIQUIDABLE = 38;
            public const short DEVOLUCION_ANTICIPO_LIQUIDABLE = 39;

            public const short ANTICIPO_SALARIO = 62;
            public const short DESCUENTO_PLANILLA_POR_ANTICIPO_SALARIO = 4;

            public const short PRESTAMO = 41;
            public const short ABONO_PRESTAMO = 42;

            public const short BACK_TO_BACK = 50;
            public const short BACK_TO_BACK_PAGO_PLANILLA = 65;

            public const short RETIRO_SOCIOS = 26;
            public const short DEVOLUCION_SOCIOS = 17;
        }


        public static class Tipo
        {
            public const short NO_APLICA = 0;
            public const short COBRO_PRODUCTO_MAL_ESTADO = 1;
            public const short OTRO = 2;
        }

        public static class Categoria
        {
            public const short SOCIO = 32;
            public const short EMPLEADO = 36;
            public const short VENDEDORES = 37;
        }

    }
}
