using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmes.core.Helpers
{
    internal class Helper
    {
        private static readonly Random random = new();
        internal static string GenerateRandomString(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var result = new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());

            return result;
        }
        internal static string GetStringInitials(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }
            var splitted = value?.Split(' ');
            var initials = $"{splitted[0][0]}{(splitted.Length > 1 ? splitted[splitted.Length - 1][0] : (char?)null)}";
            return initials;
        }
    }
}
