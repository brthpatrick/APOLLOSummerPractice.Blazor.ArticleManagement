using SummerPractice2026Blazor.Repository.Entities;

namespace SummerPractice2026Blazor.Repository.Interfaces
{
  public interface IArticleRepository
  {
    Task<Article> CreateArticleAsync(Article article);

    Task<bool> DeleteArticleAsync(Guid id);

    Task<List<Article>> GetAllArticlesAsync();

    Task<Article> GetArticleByIdAsync(Guid id);

    Task<Article> UpdateArticleAsync(Article article);
  }
}
