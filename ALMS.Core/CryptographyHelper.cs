using System;
using Elsa.NNF.Common.Library;

namespace ALMS.Core
{
    public class CryptographyHelper
    {
        private const string _startKey = "##EL0";
        private const string _IV = "g_5bt3.!";
        private const string _KEY = "ft190ha0";
        public static string SymmetricEncrypt(string data)
        {
            if (data.StartsWith(_startKey))
                return data;

            var symmetricParameter = new SymmetricParameter { AlgorithmType = SymmetricAlgorithmType.DESCrypto, IV = _IV, KEY = _KEY };
            var symmetric = Cryptography.CreateAlgorithm(symmetricParameter);
            var encryptedData = symmetric.Encryt(data);
            encryptedData = encryptedData.Replace("0x", _startKey);

            return encryptedData;
        }
        public static string SymmetricDecrypt(string data)
        {
            if (!data.StartsWith(_startKey))
                return data;
            data = data.Replace(_startKey, "0x");
            var symmetricParameter = new SymmetricParameter { AlgorithmType = SymmetricAlgorithmType.DESCrypto, IV = _IV, KEY = _KEY };
            var symmetric = Cryptography.CreateAlgorithm(symmetricParameter);

            return symmetric.Decrypt(data);
        }
        // public static string HashEncrypt (string data) {
        //     // return CryptographyHash.SHA256 (data).Replace ("-", "");
        // }

        public static string GenerateUniqeKey(uint maxLenght = 0)
        {
            var guid = Guid.NewGuid();
            var guidBase64 = Convert.ToBase64String(guid.ToByteArray());
            guidBase64 = guidBase64.Replace("=", "");
            guidBase64 = guidBase64.Replace("+", "");
            if (maxLenght == default(uint) || maxLenght >= guidBase64.Length)
                return guidBase64;
            return guidBase64.Substring(0, (int)maxLenght);
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData, bool ignoreThrowException = false)
        {
            try
            {
                var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
                return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch (System.Exception)
            {
                if (ignoreThrowException) return null;
                throw;
            }

        }

    }
}