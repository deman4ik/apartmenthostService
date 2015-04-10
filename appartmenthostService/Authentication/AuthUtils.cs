using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using appartmenthostService.Models;

namespace appartmenthostService.Authentication
{
    public class AuthUtils
    {
        public static string randomString(int size)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        public static byte[] hash(string plaintext, byte[] salt)
        {
            SHA512Cng hashFunc = new SHA512Cng();
            byte[] plainBytes = System.Text.Encoding.ASCII.GetBytes(plaintext);
            byte[] toHash = new byte[plainBytes.Length + salt.Length];
            plainBytes.CopyTo(toHash, 0);
            salt.CopyTo(toHash, plainBytes.Length);
            return hashFunc.ComputeHash(toHash);
        }

        public static byte[] generateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[256];
            rng.GetBytes(salt);
            return salt;
        }

        public static bool slowEquals(byte[] a, byte[] b)
        {
            int diff = a.Length ^ b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= a[i] ^ b[i];
            }
            return diff == 0;
        }

        public static bool IsEmailValid(string email)
        {
            try
            {
                MailAddress m = new MailAddress(email);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static void CreateAccount(string providerName, string providerId, string accountId, string email)
        {
            appartmenthostContext context = new appartmenthostContext();
            Account account =
                        
                            context.Accounts.SingleOrDefault(
                                a => a.Provider == StandartLoginProvider.ProviderName && a.ProviderId == providerId && a.AccountId == accountId);
            if (account == null)
            {
                User user;
                if (providerName == StandartLoginProvider.ProviderName)
                {
                    user =  context.Users.SingleOrDefault(u => u.Email == email);
                }
                else
                {
                    byte[] salt = AuthUtils.generateSalt();
                user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = email,
                    Salt = salt,
                    SaltedAndHashedPassword = AuthUtils.hash(randomString(8), salt)
                };
                context.Users.Add(user);
                }
                account = new Account()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                AccountId = accountId,
                Provider = providerName,
                ProviderId = providerId
            };
                context.Accounts.Add(account);
                context.SaveChanges();

            }
           
            
            
            
        }
    }
}
