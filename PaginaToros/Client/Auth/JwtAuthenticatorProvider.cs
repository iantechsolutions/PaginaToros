using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using PaginaToros.Client.Helpers;

namespace PaginaToros.Client.Auth
{
    public class JwtAuthenticatorProvider : AuthenticationStateProvider, ILoginServices
    {
        private readonly IJSExtensions js;
        private readonly HttpClient httpClient;
        private readonly NavigationManager navigationManager;
        public static readonly string TOKENKEY = "TOKENKEY";
        private bool sesionVencidaNotificada;
        private AuthenticationState anonimo => new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        public JwtAuthenticatorProvider(IJSRuntime _js, HttpClient httpClient, NavigationManager navigationManager)
        {
            this.httpClient = httpClient;
            this.navigationManager = navigationManager;
            js = new IJSExtensions(_js);
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await js.GetFromLocalStorage(TOKENKEY);

            if (string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = null;
                return anonimo;
            }

            if (TokenIsExpired(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = null;
                await js.RemoveItem(TOKENKEY);
                NotificarSesionVencida();
                return anonimo;
            }

            sesionVencidaNotificada = false;
            return BuildAuthenticationState(token);
        }

        public async Task Login(string token)
        {
            await js.RemoveItem(TOKENKEY);
            await js.SetInLocalStorage(TOKENKEY, token);
            sesionVencidaNotificada = false;
            var authState = BuildAuthenticationState(token);
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public async Task Logout()
        {
            httpClient.DefaultRequestHeaders.Authorization = null;
            await js.RemoveItem(TOKENKEY);
            NotifyAuthenticationStateChanged(Task.FromResult(anonimo));
        }

        // Devolver el estado anonimo no alcanza: si no se avisa el cambio, CascadingAuthenticationState
        // conserva el estado anterior y queda una sesion zombi donde la UI sigue mostrando permisos
        // que el token ya no tiene.
        private void NotificarSesionVencida()
        {
            if (sesionVencidaNotificada)
            {
                return;
            }

            sesionVencidaNotificada = true;
            NotifyAuthenticationStateChanged(Task.FromResult(anonimo));

            if (EstaEnPantallaDeAcceso())
            {
                return;
            }

            // Relativa al <base href> para que siga funcionando si la app se publica en un subpath.
            navigationManager.NavigateTo("Login");
        }

        private bool EstaEnPantallaDeAcceso()
        {
            var ruta = navigationManager
                .ToBaseRelativePath(navigationManager.Uri)
                .Split('?', '#')[0]
                .Trim('/');

            return ruta.Equals("login", StringComparison.OrdinalIgnoreCase)
                || ruta.Equals("logout", StringComparison.OrdinalIgnoreCase);
        }

        private AuthenticationState BuildAuthenticationState(string token)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt")));
        }

        private static bool TokenIsExpired(string token)
        {
            try
            {
                var payload = token.Split('.')[1];
                var jsonBytes = ParseBase64WithoutPadding(payload);
                var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

                if (keyValuePairs == null || !keyValuePairs.TryGetValue("exp", out var expValue) || expValue == null)
                {
                    return false;
                }

                if (!long.TryParse(expValue.ToString(), out var expSeconds))
                {
                    return false;
                }

                var expiration = DateTimeOffset.FromUnixTimeSeconds(expSeconds);
                return expiration <= DateTimeOffset.UtcNow;
            }
            catch
            {
                return true;
            }
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

            if (roles != null)
            {
                if (roles.ToString().Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
