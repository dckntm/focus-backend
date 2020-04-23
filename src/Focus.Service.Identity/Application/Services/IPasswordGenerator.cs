using System;

namespace Focus.Service.Identity.Application.Services
{
    public interface IPasswordGenerator
    {
        string Generate(
            bool useLowercase,
            bool useUppercase,
            bool useNumbers,
            bool useSpecial,
            int passwordSize)
        {
            var LOWER_CASE = "abcdefghijklmnopqursuvwxyz";
            var UPPER_CASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var NUMBERS = "123456789";
            var SPECIALS = @"!@£$%^&*()#€";
            var _password = new char[passwordSize];
            var charSet = "";
            var _random = new Random();

            // Build up the character set to choose from
            if (useLowercase) charSet += LOWER_CASE;

            if (useUppercase) charSet += UPPER_CASE;

            if (useNumbers) charSet += NUMBERS;

            if (useSpecial) charSet += SPECIALS;

            for (int i = 0; i < passwordSize; i++)
            {
                _password[i] = charSet[_random.Next(charSet.Length - 1)];
            }

            return string.Join(null, _password);
        }
    }
}