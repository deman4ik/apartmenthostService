using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Http;
using appartmenthostService.Authentication;
using Microsoft.WindowsAzure.Mobile.Service;
using appartmenthostService.Models;
using appartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service.Security.Providers;

namespace appartmenthostService
{
    public static class WebApiConfig
    {
        public static void Register()
        {
            // Use this class to set configuration options for your mobile service
            ConfigOptions options = new ConfigOptions();
            options.LoginProviders.Remove(typeof(AzureActiveDirectoryLoginProvider));
            options.LoginProviders.Add(typeof(AzureActiveDirectoryExtendedLoginProvider));
            options.LoginProviders.Add(typeof(FBLoginProvider));
            // Use this class to set WebAPI configuration options
            HttpConfiguration config = ServiceConfig.Initialize(new ConfigBuilder(options));

            // To display errors in the browser during development, uncomment the following
            // line. Comment it out again when you deploy your service for production use.
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            
            Database.SetInitializer(new appartmenthostInitializer());
        }
    }

    public class appartmenthostInitializer : DropCreateDatabaseAlways<appartmenthostContext> //ClearDatabaseSchemaIfModelChanges
    { 
        protected override void Seed(appartmenthostContext context)
        {
            byte[] salt = StandartLoginProviderUtils.generateSalt();
            string id1 = Guid.NewGuid().ToString();
            string id2 = Guid.NewGuid().ToString(); 
            List<User> users = new List<User>
            {
                new User { Id = id1, Email = "vasek@example.com", Salt = salt, SaltedAndHashedPassword = StandartLoginProviderUtils.hash("parusina", salt) },
                new User { Id = id2, Email = "parus@parus.ru", Salt = salt, SaltedAndHashedPassword = StandartLoginProviderUtils.hash("parusina", salt) },
            };

            List<Profile> profiles = new List<Profile>
            {
                new Profile {Id = id1, FirstName = "Vasek", LastName = "Pupkin"},
                new Profile {Id = id2, FirstName = "Parus", LastName = "Parusina"},
            };
            foreach (User user in users)
            {
                context.Set<User>().Add(user);
            }

            foreach (Profile profile in profiles)
            {
                context.Set<Profile>().Add(profile);
            }

            //context.SaveChanges();
            base.Seed(context);
        }
    }
}

