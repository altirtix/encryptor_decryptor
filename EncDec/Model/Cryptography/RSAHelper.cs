using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncDec
{
    static class RSAHelper
    {
        readonly static string pubKeyPath = "public.key";
        readonly static string priKeyPath = "private.key";

        public static void MakeKey()
        {
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider(2048);

            RSAParameters privKey = csp.ExportParameters(true);

            RSAParameters pubKey = csp.ExportParameters(false);
            string pubKeyString;
            {
                var sw = new StringWriter();
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                xs.Serialize(sw, pubKey);
                pubKeyString = sw.ToString();
                File.WriteAllText(pubKeyPath, pubKeyString);
            }
            string privKeyString;
            {
                var sw = new StringWriter();
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                xs.Serialize(sw, privKey);
                privKeyString = sw.ToString();
                File.WriteAllText(priKeyPath, privKeyString);
            }
        }
        public static string Encrypt(string plainText)
        {
            string pubKeyString;
            {
                using (StreamReader reader = new StreamReader(pubKeyPath)) { pubKeyString = reader.ReadToEnd(); }
            }
            var sr = new StringReader(pubKeyString);

            var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));

            RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
            csp.ImportParameters((RSAParameters)xs.Deserialize(sr));
            byte[] bytesPlainTextData = Encoding.ASCII.GetBytes(plainText);

            var bytesCipherText = csp.Encrypt(bytesPlainTextData, false);
            string encryptedText = Convert.ToBase64String(bytesCipherText);
            
            return encryptedText;
        }
        public static string Decrypt(string cipherText)
        {
            RSACryptoServiceProvider csp = new RSACryptoServiceProvider();

            string privKeyString;
            {
                privKeyString = File.ReadAllText(priKeyPath);
                var sr = new StringReader(privKeyString);
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                RSAParameters privKey = (RSAParameters)xs.Deserialize(sr);
                csp.ImportParameters(privKey);
            }
            string encryptedText = cipherText;
            byte[] bytesCipherText = Convert.FromBase64String(encryptedText);

            byte[] bytesPlainTextData = csp.Decrypt(bytesCipherText, false);

            string decrytedText;
            decrytedText = Encoding.UTF8.GetString(bytesPlainTextData);
            return decrytedText;
        }
    }
}