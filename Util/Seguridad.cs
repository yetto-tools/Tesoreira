using System;
using System.Security.Cryptography;
using System.Text;

namespace Util
{
    public static class Seguridad
    {
        public static string GenerarCadena()
        {
            var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var Charsarr = new char[8];
            var random = new Random();

            for (int i = 0; i < Charsarr.Length; i++)
            {
                Charsarr[i] = characters[random.Next(characters.Length)];
            }

            var resultString = new String(Charsarr);

            return resultString;
        }

        public static string CifrarCadena(string cadena)
        {
            SHA256Managed sha = new SHA256Managed();
            byte[] bytecadena = Encoding.Default.GetBytes(cadena);
            byte[] bytecifrado = sha.ComputeHash(bytecadena);
            return BitConverter.ToString(bytecifrado).Replace("-", "");

        }


    }
}
