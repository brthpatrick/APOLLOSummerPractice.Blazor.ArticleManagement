using FluentValidation;
using SummerPractice2026Blazor.Repository.Entities;

namespace SummerPractice2026Blazor.Validation;

public class ArticleValidator : AbstractValidator<Article>
{
  public ArticleValidator()
  {
    RuleFor(x => x.Name)
      .NotEmpty()
      .WithMessage("Please enter article name");

    RuleFor(x => x.Price)
      .InclusiveBetween(0.01m, 1000m)
      .WithMessage("Price must be between 0.01 and 1000");

    RuleFor(x => x.ArticleCategoryId)
      .NotEqual(Guid.Empty)
      .WithMessage("Please select an article category");
  }
}
