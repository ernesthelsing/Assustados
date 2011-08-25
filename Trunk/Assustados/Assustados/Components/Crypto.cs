using System;
using System.Text;

namespace Assustados.Components
{
    public class Cripto
    {
        public string Encrypt(bool input)
        {
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(input.ToString());
            string encrypted = Convert.ToBase64String(bytes);
         
            return encrypted;
        }

        public string Encrypt(int input)
        {
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(input.ToString());
            string encrypted = Convert.ToBase64String(bytes);

            return encrypted;
        }

        public string Decrypt(string input)
        {
            byte[] bytes = Convert.FromBase64String(input);
            string decrypted = ASCIIEncoding.ASCII.GetString(bytes);

            return decrypted;
        }

        public bool DecryptBoolean(string input)
        {
            byte[] bytes = Convert.FromBase64String(input);
            string decrypted = ASCIIEncoding.ASCII.GetString(bytes);

            return Convert.ToBoolean(decrypted);
        }

        public int DecryptInteger(string input)
        {
            byte[] bytes = Convert.FromBase64String(input);
            string decrypted = ASCIIEncoding.ASCII.GetString(bytes);

            return Convert.ToInt32(decrypted);
        }
    }
}