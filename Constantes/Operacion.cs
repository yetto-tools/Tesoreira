namespace Constantes
{
    public static class Operacion
    {
        public const short BACK_TO_BACK = 50;

        public static class Neutro
        {
            public const short REGISTRO_FACTURAS_AL_CONTADO = 43;
            public const short SALDO_INICIAL = 46;

            public const short REDUCCION_DE_CAJA_CHICA = 69;
            public const short ABONO_A_CAJA_CHICA = 70;

        }

        public static class Ingreso 
        {
            public const short VENTAS_EN_RUTA = 1;
            public const short VENTAS_ESTABLECIMIENTO = 2;
            public const short OTRAS_VENTAS = 3;
            public const short DEVOLUCION_POR_ANTICIPO_SALARIO = 4;
            public const short ANTICIPO_LIQUIDABLE_REEMBOLSO = 39;
            public const short RUTERO_LOCAL = 52;
            public const short RUTERO_INTERIOR = 53;
            public const short VENDEDOR = 54;
            public const short CAFETERIAS = 55;
            public const short SUPERMERCADOS = 56;
            public const short ESPECIALES_1 = 57;
            public const short ESPECIALES_2 = 58;
            public const short PRESTAMO_ABONO = 42;
            public const short REINTEGRO_CAJA_CHICA = 6;
            public static class TipoReembolso
            {
                public const byte PRESTAMO = 1;
                public const byte BACK_TO_BACK = 2;
            }
        }
        public static class Egreso
        {
            public const short PLANILLA_PAGO = 8;
            public const short AJUSTE_PLANILLA = 10;
            public const short PLANILLA_BONOS_EXTRAS_COMISION = 14;
            public const short PLANILLA_BONOS_EXTRAS_QUINTALAJE = 59;
            public const short PLANILLA_BONOS_EXTRAS_FERIADOS_Y_DOMINGOS = 60;
            public const short PLANILLA_BONOS_EXTRAS_OTROS = 61;
            public const short DEPOSITOS_BANCARIOS = 20;
            public const short GASTOS_INDIRECTOS = 34;
            public const short SUELDOS_INDIRECTOS = 64;
            public const short GASTOS_ADMINISTRATIVOS = 35;
            public const short ANTICIPO_LIQUIDABLE_EGRESO = 38;
            public const short PRESTAMO_EGRESO = 41;
            public const short ANTICIPO_SALARIO = 62;

            public static class TipoPlanilla
            {
                public const byte AJUSTE_PLANILLA = 1;
            }

            public static class TipoGastoIndirecto
            {
                public const byte SUELDO_INDIRECTO = 1;
            }
        }

        public static class IdTipoOperacion
        {
            public const string A = "A";
            public const string D = "D";
            public const string E = "E";
        }


    }
}
