using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using PaginaToros.Shared.Models;

namespace PaginaToros.Client.Helpers
{
    public static class CurrentUserHelper
    {
        public static async Task<User?> GetCurrentUserAsync(AuthenticationStateProvider authenticationStateProvider, HttpClient http)
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            var principal = authState.User;
            var email = principal.Claims.FirstOrDefault(c => c.Type == "userNM")?.Value;

            if (string.IsNullOrWhiteSpace(email))
            {
                return null;
            }

            try
            {
                var raw = await http.GetStringAsync($"api/Account/GetUserByMail/{Uri.EscapeDataString(email)}");
                return JsonSerializer.Deserialize<User>(raw);
            }
            catch
            {
                return null;
            }
        }

        public static async Task<int?> GetCurrentSocioIdAsync(AuthenticationStateProvider authenticationStateProvider, HttpClient http)
        {
            var user = await GetCurrentUserAsync(authenticationStateProvider, http);
            return user?.SocioId;
        }
    }
}
