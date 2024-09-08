using Blazing.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Client.Components.Pages;

public partial class Login
{
    [Inject]
    public NavigationManager _navigationManager { get; set; }

    [Inject]
    public ISnackbar _snackbar { get; set; }

    public LoginPageViewModel LoginPageViewModel { get; set; }

    protected override async Task OnInitializedAsync()
    {
        LoginPageViewModel = new LoginPageViewModel();

        base.OnInitializedAsync();
    }

    public async Task SendForm()
    {
        try
        {
            if (
                string.IsNullOrEmpty(LoginPageViewModel.Login)
                || string.IsNullOrEmpty(LoginPageViewModel.Password)
            )
            {
                _snackbar.Add("Заполните все поля", Severity.Error);

                return;
            }

            var isValid = true;

            if (isValid)
            {
                _snackbar.Add("Вы успешно зашли в систему", Severity.Success);

                _navigationManager.NavigateTo("/");
            }
            else
            {
                _snackbar.Add("Безуспешно", Severity.Error);
            }
        }
        catch (Exception exception)
        {
            _snackbar.Add(exception.Message, Severity.Error);
        }
    }
}

public partial class LoginPageViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _login = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;
}
