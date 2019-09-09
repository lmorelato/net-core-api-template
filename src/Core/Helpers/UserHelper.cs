using System;
using System.Collections.Generic;
using System.Linq;

namespace Template.Core.Helpers
{
    public static class UserHelper
    {
        private static readonly Random Rand = new Random();

        public static string GenerateCode(string prefix = null, int size = 8)
        {
            var start = Convert.ToInt32("1".PadRight(size, '0'));
            var end = Convert.ToInt32(string.Empty.PadLeft(size, '9'));
            var code = (prefix ?? string.Empty) + Rand.Next(start, end);
            return code.Substring(0, size);
        }

        // https://github.com/Darkseal/PasswordGenerator/blob/master/PasswordGenerator.cs
        public static string GeneratePassword(
            int requiredLength = 6, 
            int requiredUniqueChars = 4, 
            bool requireDigit = true,
            bool requireUppercase = true,
            bool requireLowercase = false, 
            bool requireNonAlphanumeric = false)
        {
            var randomChars = new[]
            {
                    "ABCDEFGHJKLMNOPQRSTUVWXYZ", // uppercase 
                    "abcdefghijkmnopqrstuvwxyz", // lowercase
                    "0123456789", // digits
                    "!@#$%&+=?" // non-alphanumeric
            };

            var chars = new List<char>();

            if (requireUppercase)
            {
                chars.Insert(Rand.Next(0, chars.Count), randomChars[0][Rand.Next(0, randomChars[0].Length)]);
            }

            if (requireLowercase)
            {
                chars.Insert(Rand.Next(0, chars.Count), randomChars[1][Rand.Next(0, randomChars[1].Length)]);
            }

            if (requireDigit)
            {
                chars.Insert(Rand.Next(0, chars.Count), randomChars[2][Rand.Next(0, randomChars[2].Length)]);
            }

            if (requireNonAlphanumeric)
            {
                chars.Insert(Rand.Next(0, chars.Count), randomChars[3][Rand.Next(0, randomChars[3].Length)]);
            }

            for (var i = chars.Count; i < requiredLength || chars.Distinct().Count() < requiredUniqueChars; i++)
            {
                var rcs = randomChars[Rand.Next(0, randomChars.Length)];
                chars.Insert(Rand.Next(0, chars.Count), rcs[Rand.Next(0, rcs.Length)]);
            }

            var text = chars.OrderBy(x => Rand.Next()).ToArray();
            return new string(text);
        }
    }
}
