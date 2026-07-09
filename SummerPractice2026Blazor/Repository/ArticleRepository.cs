using Microsoft.EntityFrameworkCore;
using SummerPractice2026Blazor.Repository.Entities;
using SummerPractice2026Blazor.Repository.Interfaces;
using SummerPractice2026Blazor.Services;

namespace SummerPractice2026Blazor.Repository
{
  public class ArticleRepository(
    ApplicationDbContext dbContext,
    ArticleImageService articleImageService) : IArticleRepository
  {
    public async Task<Article> CreateArticleAsync(Article article)
    {
      dbContext.Articles.Add(article);
      await dbContext.SaveChangesAsync();
      return article;
    }

    public async Task<bool> DeleteArticleAsync(Guid id)
    {
      var articleFromDb = await dbContext.Articles.FirstOrDefaultAsync(x => x.Id == id);
      if (articleFromDb is null)
      {
        return false;
      }

      dbContext.Articles.Remove(articleFromDb);

      if (articleFromDb.ImageUrl != null)
      {
        articleImageService.DeleteFile(articleFromDb.ImageUrl);
      }

      await dbContext.SaveChangesAsync();
      return true;
    }

    public async Task<List<Article>> GetAllArticlesAsync()
    {
      return await dbContext.Articles
        .Include(x => x.ArticleCategory)
        .ToListAsync();
    }

    public async Task<Article> GetArticleByIdAsync(Guid id)
    {
      var articleFromDb = await dbContext.Articles
        .Include(x => x.ArticleCategory)
        .FirstOrDefaultAsync(x => x.Id == id);

      return articleFromDb ?? new Article();
    }

    public async Task<Article> UpdateArticleAsync(Article article)
    {
      dbContext.Articles.Update(article);
      await dbContext.SaveChangesAsync();
      return article;
    }
  }
}
