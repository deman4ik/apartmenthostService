﻿using System.Data.Entity;
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
            ConfigOptions options = new ConfigOptions();
            options.LoginProviders.Remove(typeof(AzureActiveDirectoryLoginProvider));
            options.LoginProviders.Add(typeof(AzureActiveDirectoryExtendedLoginProvider));
            options.LoginProviders.Add(typeof(FBLoginProvider));
            // Use this class to set WebAPI configuration options
            HttpConfiguration config = ServiceConfig.Initialize(new ConfigBuilder(options));

            // To display errors in the browser during development, uncomment the following
            // line. Comment it out again when you deploy your service for production use.
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
            config.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
            Mapper.Initialize(cfg =>
            {
                DTOMapper.CreateMapping(cfg);
            });
            //Database.SetInitializer(new appartmenthostInitializer());
        }
    }

    public class appartmenthostInitializer : ClearDatabaseSchemaIfModelChanges<apartmenthostContext> 

    { 
        protected override void Seed(apartmenthostContext context)
        {
            
            base.Seed(context);
        }
    }
}

