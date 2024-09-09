using Client.Auth;
using Client.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Регистрация сервисов
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<HttpClient>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

// Добавление поддержки аутентификации на стороне клиента

// Регистрация MudBlazor
builder.Services.AddMudServices();

// Настройка компонентов
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

var app = builder.Build();

// Настройка обработки исключений
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// Карта маршрутизации компонентов Razor
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
