using System;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using apartmenthostService.Helpers;
using apartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Authentication
{
    public static class AuthUtils
    {
        public static string RandomString(int size)
        {
            var random = new Random((int) DateTime.Now.Ticks);
            var builder = new StringBuilder();
            for (var i = 0; i < size; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26*random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        public static string RandomNumString(int size)
        {
            var random = new Random((int) DateTime.Now.Ticks);
            var builder = new StringBuilder();
            for (var i = 0; i < size; i++)
            {
                var str = random.Next(0, 9).ToString();
                var ch = str[0];
                builder.Append(ch);
            }

            return builder.ToString();
        }

        public static byte[] Hash(string plaintext, byte[] salt)
        {
            var hashFunc = new SHA512Cng();
            var plainBytes = Encoding.ASCII.GetBytes(plaintext);
            var toHash = new byte[plainBytes.Length + salt.Length];
            plainBytes.CopyTo(toHash, 0);
            salt.CopyTo(toHash, plainBytes.Length);
            return hashFunc.ComputeHash(toHash);
        }

        public static byte[] GenerateSalt()
        {
            var rng = new RNGCryptoServiceProvider();
            var salt = new byte[256];
            rng.GetBytes(salt);
            return salt;
        }

        public static bool SlowEquals(byte[] a, byte[] b)
        {
            var diff = a.Length ^ b.Length;
            for (var i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= a[i] ^ b[i];
            }
            return diff == 0;
        }

        public static bool IsEmailValid(string email)
        {
            try
            {
                var m = new MailAddress(email);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static void CreateAccount(IApartmenthostContext context, string providerName, string providerId,
            string accountId, string email = null,
            string name = null)
        {
            var account =
                context.Accounts.SingleOrDefault(
                    a =>
                        a.Provider == providerName && a.ProviderId == providerId &&
                        a.AccountId == accountId);
            if (account == null)
            {
                User user = null;
                if (!string.IsNullOrEmpty(email))
                    user = context.Users.SingleOrDefault(u => u.Email == email);
                if (providerName != StandartLoginProvider.ProviderName)
                {
                    if (user == null)
                    {
                        user = new User
                        {
                            Id = SequentialGuid.NewGuid().ToString(),
                            Email = email,
                            EmailConfirmed = email != null,
                            PhoneStatus = ConstVals.PUnconf,
                            Salt = GenerateSalt()
                        };
                        context.Users.Add(user);
                    }
                    else
                    {
                        user.EmailConfirmed = email != null;
                    }
                    context.SaveChanges();
                }
                account = new Account
                {
                    Id = SequentialGuid.NewGuid().ToString(),
                    UserId = user.Id,
                    AccountId = accountId,
                    Provider = providerName,
                    ProviderId = providerId
                };
                context.Accounts.Add(account);
                context.SaveChanges();


                var profile = context.Profile.SingleOrDefault(p => p.Id == user.Id);
                if (profile == null)
                {
                    profile = new Profile {Id = user.Id};
                    if (name != null)
                    {
                        string[] names = name.Split(' ');
                        if (names.Length == 1)
                            profile.FirstName = name;
                        if (names.Length > 1)
                        {
                            profile.FirstName = names[0];
                            profile.LastName = names[1];
                        }
                    }

                    context.Profile.Add(profile);
                }
                context.SaveChanges();
            }
        }

        public static Account GetUserAccount(IApartmenthostContext context, ServiceUser user)
        {
            return context.Accounts.SingleOrDefault(a => a.AccountId == user.Id);
        }
    }
}