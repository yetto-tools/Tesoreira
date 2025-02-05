﻿namespace Constantes
{
    public static class Empleado
    {
        public static class EstadoEmpleado
        {
            public const short INACTIVO = 0;
            public const short ACTIVO = 1;
            public const short RETIRADO = 2;
            public const short SUSPENDIDO = 3;
            public const short EN_PROCESO_DE_LIQUIDACION = 4;
        }

        public static class MotivoDeBaja
        {
            public const short SUSPENDIDO = 4;
        }

        public static class TipoBackToBack
        {
            public const short NO_APLICA = 0;
            public const short DEPOSITO_COMPLETO = 1;
            public const short EFECTIVO_COMPLETO_POR_TESORERIA = 2;
            public const short EFECTIVO_IGSS_POR_TESORERIA = 3;
        }

    }
}
