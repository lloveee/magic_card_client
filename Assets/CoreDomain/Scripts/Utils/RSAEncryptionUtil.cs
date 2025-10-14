using System;
using System.Security.Cryptography;
using System.Text;

namespace CoreDomain.Scripts.Utils
{
    public static class RSAEncryptionUtil
    {
        public static string EncryptWithPublicKey(string plainText, string publicKey)
        {
            using var rsa = RSA.Create();
            rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(publicKey), out _);
            var data = Encoding.UTF8.GetBytes(plainText);
            var encrypted = rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA256);
            return Convert.ToBase64String(encrypted);
        }
        
        public static bool VerifySignature(string plainText, string signatureBase64, string publicKey)
        {
            using var rsa = RSA.Create();
            rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(publicKey), out _);
            var data = Encoding.UTF8.GetBytes(plainText);
            var signature = Convert.FromBase64String(signatureBase64);
            return rsa.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    }
}