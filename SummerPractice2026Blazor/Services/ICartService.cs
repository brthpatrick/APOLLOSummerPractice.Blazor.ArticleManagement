using SummerPractice2026Blazor.Models;

namespace SummerPractice2026Blazor.Services
{
    public interface ICartService
    {
        event Action? CartChanged;

        Task<List<CartItem>> GetItemsAsync();

        Task AddToCartAsync(Guid articleId, int quantity = 1);

        Task UpdateQuantityAsync(Guid articleId, int quantity);

        Task RemoveFromCartAsync(Guid articleId);

        Task ClearCartAsync();

        Task<int> GetDistinctItemCountAsync();
    }
}