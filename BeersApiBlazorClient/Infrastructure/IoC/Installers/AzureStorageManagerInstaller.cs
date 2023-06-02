using System.Linq;
using Autofac;
using AzureStorageManager.Core.Images;

namespace BeersApiBlazorClient.Infrastructure.IoC.Installers
{
   public class AzureStorageManagerInstaller : Module
   {
      protected override void Load(ContainerBuilder builder)
      {
         var azureStorageManagerAssembly = typeof(IImagesHandler).Assembly;

         builder
            .RegisterAssemblyTypes(azureStorageManagerAssembly)
            .Where(t => t.GetInterfaces().Any()) // implementing an interface
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

         base.Load(builder);
      }
   }
}
