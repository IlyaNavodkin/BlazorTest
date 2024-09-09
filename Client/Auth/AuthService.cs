using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;

namespace Client.Auth;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;

    public AuthService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
    }

    public async Task LoginAsync(string username, string password)
    {
        var token = "tokenTest";
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", token);
    }

    public async Task LogoutAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.clear");
    }

    public async Task<string> GetTokenAsync()
    {
        return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
    }
}


public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly AuthService _authService;

    public CustomAuthenticationStateProvider(AuthService authService)
    {
        _authService = authService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // Получаем токен из AuthService
        var token = await _authService.GetTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
        {
            // Если токен отсутствует, возвращаем анонимное состояние
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        // Создаем утверждение (claim) на основе токена
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, token) };

        // Создаем ClaimsIdentity с утверждениями
        var identity = new ClaimsIdentity(claims, "apiauth_type");

        // Создаем пользователя на основе ClaimsIdentity
        var user = new ClaimsPrincipal(identity);

        return new AuthenticationState(user);
    }

    public async Task NotifyUserAuthentication()
    {
        // Получаем токен
        var token = await _authService.GetTokenAsync();

        if (!string.IsNullOrWhiteSpace(token))
        {
            // Создаем утверждение (claim) на основе токена
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, token) };

            // Создаем ClaimsIdentity с утверждениями
            var identity = new ClaimsIdentity(claims, "apiauth_type");

            // Создаем пользователя на основе ClaimsIdentity
            var user = new ClaimsPrincipal(identity);

            // Уведомляем об изменении состояния аутентификации
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
    }

    public void NotifyUserLogout()
    {
        // Создаем анонимное состояние
        var identity = new ClaimsIdentity();
        var user = new ClaimsPrincipal(identity);

        // Уведомляем об изменении состояния аутентификации
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }
}