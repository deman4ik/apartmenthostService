using System;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using apartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Authentication
{
    public static class AuthUtils
    {
        public static string randomString(int size)
        {
            var random = new Random((int) DateTime.Now.Ticks);
            var builder = new StringBuilder();
            char ch;
            for (var i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26*random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        public static string randomNumString(int size)
        {
            var random = new Random((int) DateTime.Now.Ticks);
            var builder = new StringBuilder();
            char ch;
            for (var i = 0; i < size; i++)
            {
                var str = random.Next(0, 9).ToString();
                ch = str[0];
                builder.Append(ch);
            }

            return builder.ToString();
        }

        public static byte[] hash(string plaintext, byte[] salt)
        {
            var hashFunc = new SHA512Cng();
            var plainBytes = Encoding.ASCII.GetBytes(plaintext);
            var toHash = new byte[plainBytes.Length + salt.Length];
            plainBytes.CopyTo(toHash, 0);
            salt.CopyTo(toHash, plainBytes.Length);
            return hashFunc.ComputeHash(toHash);
        }

        public static byte[] generateSalt()
        {
            var rng = new RNGCryptoServiceProvider();
            var salt = new byte[256];
            rng.GetBytes(salt);
            return salt;
        }

        public static bool slowEquals(byte[] a, byte[] b)
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

        public static void CreateAccount(string providerName, string providerId, string accountId, string email = null,
            string name = null)
        {
            var context = new apartmenthostContext();
            var account =
                context.Accounts.SingleOrDefault(
                    a =>
                        a.Provider == providerName && a.ProviderId == providerId &&
                        a.AccountId == accountId);
            if (account == null)
            {
                User user = context.Users.SingleOrDefault(u => u.Email == email);
                if (providerName != StandartLoginProvider.ProviderName)
                {
                    if (user == null)
                    {
                        user = new User
                        {
                            Id = Guid.NewGuid().ToString(),
                            Email = email,
                            EmailConfirmed = email != null
                        };
                        context.Users.Add(user);
                        context.SaveChanges();
                    }
                }
                account = new Account
                {
                    Id = Guid.NewGuid().ToString(),
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
                    profile = new Profile();
                    profile.Id = user.Id;
                    if (name != null)
                    {
                        profile.FirstName = name.Split(' ')[0];
                        profile.LastName = name.Split(' ')[1];
                    }

                    context.Profile.Add(profile);
                }
                context.SaveChanges();
            }
        }

        public static Account GetUserAccount(apartmenthostContext context, ServiceUser user)
        {
            return context.Accounts.SingleOrDefault(a => a.AccountId == user.Id);
        }
    }
}