using Client.Auth;
using Client.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// ����������� ��������
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<HttpClient>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

// ���������� ��������� �������������� �� ������� �������

// ����������� MudBlazor
builder.Services.AddMudServices();

// ��������� �����������
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

var app = builder.Build();

// ��������� ��������� ����������
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// ����� ������������� ����������� Razor
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
