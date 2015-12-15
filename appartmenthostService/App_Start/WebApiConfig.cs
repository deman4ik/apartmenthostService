using System.Data.Entity;
using System.Web.Http;
using apartmenthostService.Authentication;
using apartmenthostService.Helpers;
using apartmenthostService.Models;
using AutoMapper;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security.Providers;
using Newtonsoft.Json;

namespace apartmenthostService
{
    public static class WebApiConfig
    {
        public static void Register()
        {
            // Use this class to set configuration options for your mobile service
            var options = new ConfigOptions();
            options.LoginProviders.Remove(typeof (AzureActiveDirectoryLoginProvider));
            options.LoginProviders.Add(typeof (AzureActiveDirectoryExtendedLoginProvider));
            options.LoginProviders.Add(typeof (FBLoginProvider));
            options.LoginProviders.Add(typeof (VKLoginProvider));
            // Use this class to set WebAPI configuration options
            var config = ServiceConfig.Initialize(new ConfigBuilder(options));

            // To display errors in the browser during development, uncomment the following
            // line. Comment it out again when you deploy your service for production use.
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            config.Formatters.JsonFormatter.SerializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include;
            config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Include;
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
            config.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling =
                PreserveReferencesHandling.Objects;
            Mapper.Initialize(cfg => { DTOMapper.CreateMapping(cfg); });

            //var migrator = new DbMigrator(new Configuration());
            //migrator.Update();
           // Database.SetInitializer(new appartmenthostInitializer());
        }
    }

    //public class appartmenthostInitializer : ClearDatabaseSchemaAlways<ApartmenthostContext>

    //{
    //    protected override void Seed(ApartmenthostContext context)
    //{

    //    base.Seed(context);
    //    context.Article.Add(new Article
    //    {
    //        Id = SequentialGuid.NewGuid().ToString()


    //    }
    //        );

    //    context.SaveChanges();
    //}
//}
}