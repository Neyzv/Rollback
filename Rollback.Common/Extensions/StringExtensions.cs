using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Rollback.Common.Extensions
{
    public static class StringExtensions
    {
        private static readonly Regex _specialCharsRegex = new("^[a-zA-Z0-9 ]*$", RegexOptions.Compiled);

        public static string RandomString(this Random random, int size)
        {
            var builder = new StringBuilder();
            for (int i = 0; i < size; i++)
                builder.Append((char)Math.Floor(26 * random.NextDouble() + 65));

            return builder.ToString();
        }

        public static string Md5Hash(string input)
        {
            using var md5 = MD5.Create();
            var hashedBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            var sb = new StringBuilder();
            for (var i = 0; i < hashedBytes.Length; i++)
                sb.Append(hashedBytes[i].ToString("x2"));

            return sb.ToString();
        }

        public static string CipherPassword(string hashedPassword, string ticket) =>
            Md5Hash(string.Concat(hashedPassword, ticket));

        public static string CipherSecretAnswer(int characterId, string answer) =>
            Md5Hash(characterId + "~" + answer);

        public static bool ContainsSpecialChars(this string str) =>
            !_specialCharsRegex.IsMatch(str);

        public static string RandomName()
        {
            string str = string.Empty;
            const string vowels = "aeiouy";
            const string consonants = "bcdfghjklmnpqrstvwxz";
            for (int i = 0; i <= Random.Shared.Next(5, 10); i++)
            {
                str += i % 2 == 0
                    ? vowels[Random.Shared.Next(vowels.Length - 1)]
                    : consonants[Random.Shared.Next(consonants.Length - 1)];
            }
            return char.ToUpper(str[0]) + str[1..];
        }
    }
}
