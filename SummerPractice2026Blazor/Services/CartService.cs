
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using SummerPractice2026Blazor.Models;
using System.Security.Claims;
using System.Text.Json;

namespace SummerPractice2026Blazor.Services
{
    public class CartService(IJSRuntime jsRuntime, AuthenticationStateProvider authenticationStateProvider) : ICartService
    {
        public event Action? CartChanged;

        public async Task<List<CartItem>> GetItemsAsync()
        {
            var key = await GetStorageKeyAsync();
            var json = await jsRuntime.InvokeAsync<string?>("localStorage.getItem", key);

            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<CartItem>();
            }

            return JsonSerializer.Deserialize<List<CartItem>>(json) ?? new List<CartItem>();
        }

        public async Task AddToCartAsync(Guid articleId, int quantity = 1)
        {
            var items = await GetItemsAsync();
            var existing = items.FirstOrDefault(i => i.ArticleId == articleId);

            if (existing is not null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                items.Add(new CartItem { ArticleId = articleId, Quantity = quantity });
            }

            await SaveItemsAsync(items);
        }

        public async Task UpdateQuantityAsync(Guid articleId, int quantity)
        {
            var items = await GetItemsAsync();
            var existing = items.FirstOrDefault(i => i.ArticleId == articleId);

            if (existing is null)
            {
                return;
            }

            if (quantity <= 0)
            {
                items.Remove(existing);
            }
            else
            {
                existing.Quantity = quantity;
            }

            await SaveItemsAsync(items);
        }

        public async Task RemoveFromCartAsync(Guid articleId)
        {
            var items = await GetItemsAsync();
            items.RemoveAll(i => i.ArticleId == articleId);
            await SaveItemsAsync(items);
        }

        public async Task ClearCartAsync()
        {
            await SaveItemsAsync(new List<CartItem>());
        }

        public async Task<int> GetDistinctItemCountAsync()
        {
            var items = await GetItemsAsync();
            return items.Count;
        }

        private async Task SaveItemsAsync(List<CartItem> items)
        {
            var key = await GetStorageKeyAsync();
            var json = JsonSerializer.Serialize(items);
            await jsRuntime.InvokeVoidAsync("localStorage.setItem", key, json);
            CartChanged?.Invoke();
        }

        private async Task<string> GetStorageKeyAsync()
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            var userId = authState.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "anonymous";
            return $"cart_{userId}";
        }
    }
}