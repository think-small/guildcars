using Autofac;
using Autofac.Configuration;
using Autofac.Features.AttributeFilters;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using GuildCars.Services;
using GuildCars.UI.Controllers;
using Microsoft.Extensions.Configuration;
using System.Web.Http;
using System.Web.Mvc;

namespace GuildCars.UI
{
    public class ContainerConfig
    {
        public static void RegisterContainer()
        {
            var config = new ConfigurationBuilder();
            config.AddJsonFile("autofac.json");
            var dataModule = new ConfigurationModule(config.Build());

            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterApiControllers(typeof(MvcApplication).Assembly);
            builder.RegisterModule<ServicesModule>();
            builder.RegisterModule(dataModule);

            var container = builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));            
        }
    }
}