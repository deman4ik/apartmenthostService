using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Http;
using appartmenthostService.Authentication;
using Microsoft.WindowsAzure.Mobile.Service;
using appartmenthostService.DataObjects;
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

    public class appartmenthostInitializer : ClearDatabaseSchemaIfModelChanges<appartmenthostContext>
    {
        protected override void Seed(appartmenthostContext context)
        {
            byte[] salt = StandartLoginProviderUtils.generateSalt();
            List<User> users = new List<User>
            {
                new User { Id = Guid.NewGuid().ToString(), Email = "vasek@example.com", Salt = salt, SaltedAndHashedPassword = StandartLoginProviderUtils.hash("parusina", salt) },
                new User { Id = Guid.NewGuid().ToString(), Email = "parus@parus.ru", Salt = salt, SaltedAndHashedPassword = StandartLoginProviderUtils.hash("parusina", salt) },
            };

            foreach (User user in users)
            {
                context.Set<User>().Add(user);
            }
            context.SaveChanges();
            base.Seed(context);
        }
    }
}

