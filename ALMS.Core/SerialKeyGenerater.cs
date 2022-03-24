using System;
using System.Text;

namespace ALMS.Core
{
    public static class SerialKeyGenerater
    {
        public static string GenerateLicenseKey(int groupLength = 5)
        {
            var random = new Random();
            var serialKey = new StringBuilder();
            for (var i = 0; i < groupLength; i++)
            {
                if (serialKey.Length > 0)
                    serialKey.Append("-");
                serialKey.Append(GetSerialGroup(random));
            }
            random = null;
            return serialKey.ToString();
        }

        private static string GetSerialGroup(Random random, int length = 4)
        {
            var serialGroup = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                serialGroup.Append(GenerateCharacter(random));
            }
            return serialGroup.ToString();
        }
        private static string GenerateCharacter(Random random)
        {
            var randomNumber = default(int);
            while (true)
            {
                randomNumber = random.Next(48, 90);
                if (randomNumber > 57 && randomNumber < 65)
                    continue;
                return Convert.ToChar(randomNumber).ToString();
            }
        }
    }
}