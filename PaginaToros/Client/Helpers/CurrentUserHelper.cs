using System.Text.Json;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using PaginaToros.Shared.Models;

namespace PaginaToros.Client.Helpers
{
    public static class CurrentUserHelper
    {
        private static async Task<UserContextDTO?> GetCurrentContextAsync(HttpClient http)
        {
            try
            {
                return await http.GetFromJsonAsync<UserContextDTO>("api/Account/context");
            }
            catch
            {
                return null;
            }
        }

        public static async Task<User?> GetCurrentUserAsync(AuthenticationStateProvider authenticationStateProvider, HttpClient http)
        {
            var context = await GetCurrentContextAsync(http);
            if (context?.UserId > 0)
            {
                try
                {
                    var raw = await http.GetStringAsync($"api/Account/GetUserById/{context.UserId}");
                    var userById = JsonSerializer.Deserialize<User>(raw);
                    if (userById is not null)
                    {
                        return userById;
                    }
                }
                catch
                {
                    // Fallback below: algunos entornos todavía resuelven el usuario por mail.
                }
            }

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
            var context = await GetCurrentContextAsync(http);
            if (context?.ActiveSocioId.HasValue == true)
            {
                return context.ActiveSocioId;
            }

            var user = await GetCurrentUserAsync(authenticationStateProvider, http);
            return user?.SocioId;
        }
    }
}
