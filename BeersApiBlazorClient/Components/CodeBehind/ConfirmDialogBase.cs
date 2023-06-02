using Microsoft.AspNetCore.Components;
using Radzen;

namespace BeersApiBlazorClient.Components.CodeBehind
{
   public class ConfirmDialogBase : ComponentBase
   {
      [ParameterAttribute]
      public string Message { get; set; }

      [Inject]
      public DialogService DialogService { get; set; }
   }
}
