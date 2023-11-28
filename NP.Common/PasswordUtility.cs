using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NP.Common
{
    public class PasswordOptions
    {
        public int RequiredLength { get; init; }
        public int RequiredUniqueChars { get; init; }
        public bool RequireNonAlphanumeric { get; init; }
        public bool RequireLowercase { get; init; }
        public bool RequireUppercase { get; init; }
        public bool RequireDigit { get; init; }
    }
    public enum PasswordStrength
    {
        /// <summary>
        /// Blank Password (empty and/or space chars only)
        /// </summary>
        Blank = 0,
        /// <summary>
        /// Either too short (less than 5 chars), one-case letters only or digits only
        /// </summary>
        VeryWeak = 1,
        /// <summary>
        /// At least 5 characters, one strong condition met (>= 8 chars with 1 or more UC letters, LC letters, digits & special chars)
        /// </summary>
        Weak = 2,
        /// <summary>
        /// At least 5 characters, two strong conditions met (>= 8 chars with 1 or more UC letters, LC letters, digits & special chars)
        /// </summary>
        Medium = 3,
        /// <summary>
        /// At least 8 characters, three strong conditions met (>= 8 chars with 1 or more UC letters, LC letters, digits & special chars)
        /// </summary>
        Strong = 4,
        /// <summary>
        /// At least 8 characters, all strong conditions met (>= 8 chars with 1 or more UC letters, LC letters, digits & special chars)
        /// </summary>
        VeryStrong = 5
    }

    public static class PasswordCheck
    {
        /// <summary>
        /// Generic method to retrieve password strength: use this for general purpose scenarios, 
        /// i.e. when you don't have a strict policy to follow.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static PasswordStrength GetPasswordStrength(string password)
        {
            int score = 0;
            if (String.IsNullOrEmpty(password) || String.IsNullOrEmpty(password.Trim())) return PasswordStrength.Blank;
            if (HasMinimumLength(password, 5)) score++;
            if (HasMinimumLength(password, 8)) score++;
            if (HasUpperCaseLetter(password) && HasLowerCaseLetter(password)) score++;
            if (HasDigit(password)) score++;
            if (HasSpecialChar(password)) score++;
            return (PasswordStrength)score;
        }

        /// <summary>
        /// Sample password policy implementation:
        /// - minimum 8 characters
        /// - at lease one UC letter
        /// - at least one LC letter
        /// - at least one non-letter char (digit OR special char)
        /// </summary>
        /// <returns></returns>
        public static bool IsStrongPassword(string password)
        {
            return HasMinimumLength(password, 8)
                && HasUpperCaseLetter(password)
                && HasLowerCaseLetter(password)
                && (HasDigit(password) || HasSpecialChar(password));
        }

        /// <summary>
        /// Sample password policy implementation following the Microsoft.AspNetCore.Identity.PasswordOptions standard.
        /// </summary>
        public static bool IsValidPassword(string password, PasswordOptions opts)
        {
            return IsValidPassword(
                password,
                opts.RequiredLength,
                opts.RequiredUniqueChars,
                opts.RequireNonAlphanumeric,
                opts.RequireLowercase,
                opts.RequireUppercase,
                opts.RequireDigit);
        }


        /// <summary>
        /// Sample password policy implementation following the Microsoft.AspNetCore.Identity.PasswordOptions standard.
        /// </summary>
        public static bool IsValidPassword(
            string password,
            int requiredLength,
            int requiredUniqueChars,
            bool requireNonAlphanumeric,
            bool requireLowercase,
            bool requireUppercase,
            bool requireDigit)
        {
            if (!HasMinimumLength(password, requiredLength)) return false;
            if (!HasMinimumUniqueChars(password, requiredUniqueChars)) return false;
            if (requireNonAlphanumeric && !HasSpecialChar(password)) return false;
            if (requireLowercase && !HasLowerCaseLetter(password)) return false;
            if (requireUppercase && !HasUpperCaseLetter(password)) return false;
            if (requireDigit && !HasDigit(password)) return false;
            return true;
        }

        #region Helper Methods

        private static bool HasMinimumLength(string password, int minLength)
        {
            return password.Length >= minLength;
        }

        private static bool HasMinimumUniqueChars(string password, int minUniqueChars)
        {
            return password.Distinct().Count() >= minUniqueChars;
        }

        /// <summary>
        /// Returns TRUE if the password has at least one digit
        /// </summary>
        private static bool HasDigit(string password)
        {
            return password.Any(c => char.IsDigit(c));
        }

        /// <summary>
        /// Returns TRUE if the password has at least one special character
        /// </summary>
        private static bool HasSpecialChar(string password)
        {
            // return password.Any(c => char.IsPunctuation(c)) || password.Any(c => char.IsSeparator(c)) || password.Any(c => char.IsSymbol(c));
            return password.IndexOfAny("!@#$%^&*?_~-£().,".ToCharArray()) != -1;
        }

        /// <summary>
        /// Returns TRUE if the password has at least one uppercase letter
        /// </summary>
        private static bool HasUpperCaseLetter(string password)
        {
            return password.Any(c => char.IsUpper(c));
        }

        /// <summary>
        /// Returns TRUE if the password has at least one lowercase letter
        /// </summary>
        private static bool HasLowerCaseLetter(string password)
        {
            return password.Any(c => char.IsLower(c));
        }
        #endregion
    }

    /// <summary>
    /// Password Hashing provider
    /// </summary>
    public class PasswordHash
    {
        public const int SaltByteSize = 24;
        public const int HashByteSize = 20; // to match the size of the PBKDF2-HMAC-SHA-1 hash 
        public const int Pbkdf2Iterations = 1000;
        public const int IterationIndex = 0;
        public const int SaltIndex = 1;
        public const int Pbkdf2Index = 2;

        /// <summary>
        /// Create password hash
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string HashPassword(string password)
        {
            return HashPassword(password, Pbkdf2Iterations);
        }


        /// <summary>
        /// Create password hash
        /// </summary>
        /// <param name="password"></param>
        /// <param name="iterations"></param>
        /// <returns></returns>
        public static string HashPassword(string password, int iterations)
        {
            byte[] salt = new byte[SaltByteSize];

            //var cryptoProvider = new RNGCryptoServiceProvider();//Obsolete
            //cryptoProvider.GetBytes(salt);
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var hash = GetPbkdf2Bytes(password, salt, iterations, HashByteSize);
            return iterations + ":" +
                   Convert.ToBase64String(salt) + ":" +
                   Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Validate password by the hash
        /// </summary>
        /// <param name="password"></param>
        /// <param name="correctHash"></param>
        /// <returns></returns>
        public static bool ValidatePassword(string password, string correctHash)
        {
            try
            {
                char[] delimiter = { ':' };
                var split = correctHash.Split(delimiter);
                var iterations = Int32.Parse(split[IterationIndex]);
                var salt = Convert.FromBase64String(split[SaltIndex]);
                var hash = Convert.FromBase64String(split[Pbkdf2Index]);

                var testHash = GetPbkdf2Bytes(password, salt, iterations, hash.Length);
                return SlowEquals(hash, testHash);
            }
            catch
            {
            }

            return false;
        }

        private static bool SlowEquals(byte[] a, byte[] b)
        {
            var diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }
            return diff == 0;
        }

        private static byte[] GetPbkdf2Bytes(string password, byte[] salt, int iterations, int outputBytes)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt)
            {
                IterationCount = iterations
            };
            return pbkdf2.GetBytes(outputBytes);
        }
    }
}
