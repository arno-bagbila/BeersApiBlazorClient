using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Blazored.LocalStorage;
using Blazored.Modal;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Radzen;

namespace BeersApiBlazorClient
{
   public class Program
   {
      public static async Task Main(string[] args)
      {
         var builder = WebAssemblyHostBuilder.CreateDefault(args);

         builder.RootComponents.Add<App>("#app");

         const string beerApiUrl = "https://localhost:44346";

         //for call when user is authenticated
         builder.Services.AddHttpClient("identity", sp =>
            {
               sp.BaseAddress = new Uri(beerApiUrl);
            })
            .AddHttpMessageHandler(sp =>
            {
               var handler = sp.GetService<AuthorizationMessageHandler>()
                  .ConfigureHandler(
                     authorizedUrls: new[] { beerApiUrl },
                     scopes: new[] { "beersapi" });

               return handler;
            });

         //for calls when user not authenticated
         builder.Services.AddHttpClient("api", sp =>
         {
            sp.BaseAddress = new Uri(beerApiUrl);
         });

         // we use the api client as default HttpClient - Not anymore
         builder.Services.AddScoped(
            sp => sp.GetService<IHttpClientFactory>().CreateClient());

         builder.Services.AddBlazoredModal();
         builder.Services.AddBlazoredLocalStorage();

         builder.Services.AddOidcAuthentication(options =>
         {
               // Configure your authentication provider options here.
               // For more information, see https://aka.ms/blazor-standalone-auth
               builder.Configuration.Bind("oidc", options.ProviderOptions);
               options.UserOptions.RoleClaim = "role";

         });

         builder.Services.AddScoped<DialogService>();
         builder.Services.AddScoped<NotificationService>();
         builder.Services.AddOptions();
         builder.Services.AddAuthorizationCore();

         builder.ConfigureContainer(new AutofacServiceProviderFactory(Register));

         await builder.Build().RunAsync();
      }

      public static void Register(ContainerBuilder builder)
      {
         // Add any Autofac modules or registrations.
         // This is called AFTER ConfigureServices so things you
         // register here OVERRIDE things registered in ConfigureServices.
         //
         // You must have the call to `UseServiceProviderFactory(new AutofacServiceProviderFactory())`
         // when building the host or this won't be called.
         builder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());
      }

   }
}
