
namespace Constantes
{
    public class Correccion
    {
        public static class EstadoSolicitudcorreccion
        {
            public const short SIN_SOLICITUD = 0;
            public const short SOLICITA_APROBACION = 1;
            public const short VISTO_BUENO = 2;
            public const short CORREGIDO = 3;
            public const short DENEGADO = 4;
        }

        public static class ResultadoSolicitudCorreccion
        {
            public const short SIN_RESULTADO = 0;
            public const short APROBADA = 1;
            public const short DENEGADA = 2;
        }

        public static class TipoCorreccion
        {
            public const short MODIFICACION = 1;
            public const short ANULACION = 2;
        }



    }
}
