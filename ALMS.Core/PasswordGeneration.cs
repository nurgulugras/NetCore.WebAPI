using System;
using System.Linq;

namespace ALMS.Core
{
    public enum PasswordCombinationType
    {
        Any,
        UpperLowerChars,
        Alphanumeric
    }
    public static class PasswordGeneration
    {
        private const string _specialCharacters = "@!_<>;#.[]+";

        /// <summary>
        /// Generate Random Password
        /// </summary>
        /// <param name="length">Minimum length of password </param>
        /// <param name="minLowerCaseCharacter">Minimum Lower Case Character</param>
        /// <param name="minUpperCaseCharacter">Minimum Upper Case Character</param>
        /// <param name="MinSpecialCharacter">Minimum Special Character</param>
        /// <param name="minDigit">Minimum Digit</param>
        /// <param name="passwordCombinationType"> ALL= Character and Special Character with digit, UpperLowerChars= Character only , Alphanumeric= Character with digit </param>
        /// <returns>Random Password</returns>
        public static string GeneratePassword(int passwordLength,
            int minLowerCaseCharacter,
            int minUpperCaseCharacter,
            int minSpecialCharacter,
            int minDigit,
            PasswordCombinationType passwordCombinationType)
        {
            var randomPassword = string.Empty;
            if (passwordLength < 1) return randomPassword;

            var random = new Random();
            var minDigitCount = 0;
            var minLowerCaseCharacterCount = 0;
            var minUpperCaseCharacterCount = 0;
            var minSpecialCharacterCount = 0;

            do
            {
                switch (passwordCombinationType)
                {
                    case PasswordCombinationType.Any:
                        RandomDigit(minDigit, ref randomPassword, random, ref minDigitCount);
                        RandomLowerCaseCharacter(minLowerCaseCharacter, ref randomPassword, random, ref minLowerCaseCharacterCount);
                        RandomUpperCaseCharacter(minUpperCaseCharacter, ref randomPassword, random, ref minUpperCaseCharacterCount);
                        RandomSpecialCharacter(minSpecialCharacter, ref randomPassword, random, ref minSpecialCharacterCount);
                        break;
                    case PasswordCombinationType.UpperLowerChars:
                        RandomLowerCaseCharacter(minLowerCaseCharacter, ref randomPassword, random, ref minLowerCaseCharacterCount);
                        RandomUpperCaseCharacter(minUpperCaseCharacter, ref randomPassword, random, ref minUpperCaseCharacterCount);
                        break;
                    case PasswordCombinationType.Alphanumeric:
                        RandomDigit(minDigit, ref randomPassword, random, ref minDigitCount);
                        RandomLowerCaseCharacter(minLowerCaseCharacter, ref randomPassword, random, ref minLowerCaseCharacterCount);
                        RandomUpperCaseCharacter(minUpperCaseCharacter, ref randomPassword, random, ref minUpperCaseCharacterCount);
                        break;
                }
                if (passwordLength != randomPassword.Length)
                {
                    randomPassword += GenerateRandomChar();
                }
            } while (passwordLength != randomPassword.Length);
            return MixChars(randomPassword);
        }
        private static string GenerateRandomChar()
        {
            var random = new Random();
            var randomNumber = default(int);
            while (true)
            {
                randomNumber = random.Next(48, 122);
                if ((randomNumber > 57 && randomNumber < 65) || (randomNumber > 90 && randomNumber < 97))
                    continue;
                break;
            }
            var randomChar = Convert.ToChar(randomNumber).ToString();
            System.Console.WriteLine("Char: " + randomChar + " - NO: " + randomNumber);
            return randomChar;
        }
        private static void RandomLowerCaseCharacter(int minLowerCaseCharacter, ref string randomPassword, Random random, ref int minLowerCaseCharacterCount)
        {
            while (minLowerCaseCharacter != minLowerCaseCharacterCount)
            {
                randomPassword += Convert.ToChar(random.Next(97, 122));  // a-z
                minLowerCaseCharacterCount++;
            }
        }
        private static void RandomDigit(int minDigit, ref string randomPassword, Random random, ref int minDigitCount)
        {
            while (minDigit != minDigitCount)
            {
                randomPassword += Convert.ToChar(random.Next(48, 58));  // 0-9
                minDigitCount++;
            }
        }
        private static void RandomUpperCaseCharacter(int minUpperCaseCharacter, ref string randomPassword, Random random, ref int minUpperCaseCharacterCount)
        {
            while (minUpperCaseCharacter != minUpperCaseCharacterCount)
            {
                randomPassword += Convert.ToChar(random.Next(65, 90));  // A-Z
                minUpperCaseCharacterCount++;
            }
        }
        private static void RandomSpecialCharacter(int MinSpecialCharacter, ref string randomPassword, Random random, ref int minSpecialCharacterCount)
        {
            while (MinSpecialCharacter != minSpecialCharacterCount)
            {
                randomPassword += _specialCharacters[random.Next(0, _specialCharacters.Length - 1)];
                minSpecialCharacterCount++;
            }
        }
        private static string MixChars(string value)
        {
            var num = new Random();
            return new string(value.ToCharArray().OrderBy(s => (num.Next(2) % 2) == 0).ToArray());
        }
    }
}