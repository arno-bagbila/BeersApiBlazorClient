using System.Linq;
using Autofac;
using BeersApiBlazorClient.Services.Beer;

namespace BeersApiBlazorClient.Infrastructure.IoC.Installers
{
   public class ComponentsInstaller : Module
   {
      protected override void Load(ContainerBuilder builder)
      {
         var servicesAssembly = typeof(IBeerService).Assembly;

         builder
            .RegisterAssemblyTypes(servicesAssembly)
            .Where(t => t.GetInterfaces().Any()) // implementing an interface
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

         base.Load(builder);
      }
   }
}
