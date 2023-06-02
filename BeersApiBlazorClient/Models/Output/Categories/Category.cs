using FluentValidation;

namespace BeersApiBlazorClient.Models.Output.Categories
{
   public class Category
   {
      public string Name { get; set; }

      public string Description { get; set; }
   }

   public class CategoryValidator : AbstractValidator<Category>
   {
      private const int DescriptionMaxLength = 3000;
      private const int NameMaxLength = 50;
      private const int MinimumLength = 3;

      public CategoryValidator()
      {
         RuleFor(c => c.Name)
            .NotEmpty()
            .MinimumLength(MinimumLength)
            .MaximumLength(NameMaxLength);

         RuleFor(c => c.Description)
            .NotEmpty()
            .MinimumLength(MinimumLength)
            .MaximumLength(DescriptionMaxLength);
      }

   }
}
