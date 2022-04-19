using System;

namespace Util
{
    public static class Conversion
    {
        public static int DayOfWeek(DateTime fecha) {
            int day = (int)fecha.DayOfWeek;
            int localDay = day;
            switch (day)
            {
                case 0: localDay = 7; break;
                case 1: localDay = 1; break;
                case 2: localDay = 2; break;
                case 3: localDay = 3; break;
                case 4: localDay = 4; break;
                case 5: localDay = 5; break;
                case 6: localDay = 6; break;
                default: localDay = 0; break;
            }

            /*switch (day) {
                case 1: localDay = 7; break;
                case 2: localDay = 1; break;
                case 3: localDay = 2; break;
                case 4: localDay = 3; break;
                case 5: localDay = 4; break;
                case 6: localDay = 5; break;
                case 7: localDay = 6; break;
                default:  localDay = 0; break;
            }*/
            return localDay;
        }

        public static string nombreDia(int numeroDia)
        {
            string nombreDia = String.Empty;
            switch (numeroDia)
            {
                case 1: nombreDia = "LUNES"; break;
                case 2: nombreDia = "MARTES"; break;
                case 3: nombreDia = "MIERCOLES"; break;
                case 4: nombreDia = "JUEVES"; break;
                case 5: nombreDia = "VIERNES"; break;
                case 6: nombreDia = "SABADO"; break;
                case 7: nombreDia = "DOMINGO"; break;
                default: nombreDia = "NO DEFINIDO"; break;
            }
            return nombreDia;
        }

        public static DateTime ConvertDateSpanishToEnglish(string fechaStr)
        {
            DateTime fecha = DateTime.MinValue;
            string[] word = fechaStr.Split('/');
            int anio = Convert.ToInt32(word[2]);
            int mes = Convert.ToInt32(word[1]);
            int dia = Convert.ToInt32(word[0]);

            fecha = new DateTime(anio, mes, dia);
            return fecha;
        }


    }


}
