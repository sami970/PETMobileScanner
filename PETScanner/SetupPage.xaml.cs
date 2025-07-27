
using System.Text;
using System.Text.Json;
using PETScanner.Model;
using PETScanner.ViewModel;

namespace PETScanner;




public partial class SetupPage : ContentPage
{
    private UserDataViewModel viewModel;
    
    public SetupPage()
	{
    InitializeComponent();
    LoadSavedSettings();
    BindingContext = viewModel;

    }


private void LoadSavedSettings()
{
    MobileIdEntry.Text = SettingsService.MobileId;
    passwordEntry.Text = SettingsService.Password;
    TransferJournalTypeEntry.Text = SettingsService.TransferJournalType;
    AdjustmentJournalTypeEntry.Text = SettingsService.AdjustmentJournalType;
    DefaultLocationEntry.Text = SettingsService.DefaultLocation;
    DefaultUrlEntry.Text = SettingsService.DefaultUrl;
    DefaultPortEntry.Text = SettingsService.DefaultPort;

}

private async void OnSaveClicked(object sender, EventArgs e)
{
    SettingsService.MobileId = MobileIdEntry.Text?.Trim() ?? string.Empty;
    SettingsService.Password = passwordEntry.Text?.Trim() ?? string.Empty;
    SettingsService.TransferJournalType = TransferJournalTypeEntry.Text?.Trim() ?? string.Empty;
    SettingsService.AdjustmentJournalType = AdjustmentJournalTypeEntry.Text?.Trim() ?? string.Empty;
    SettingsService.DefaultLocation = DefaultLocationEntry.Text?.Trim() ?? string.Empty;
    SettingsService.DefaultUrl = DefaultUrlEntry.Text?.Trim() ?? string.Empty;
    SettingsService.DefaultPort = DefaultPortEntry.Text?.Trim() ?? string.Empty;

    await DisplayAlert("Enregistré", "Tous les paramètres ont été enregistrés.", "OK");
    await Navigation.PushAsync(new MainPage());

}

private async void SaveDataAsync(object sender, EventArgs e)
    {
        viewModel = new UserDataViewModel();
        viewModel.SaveDataAsync();
    }

private AuthService _authService;
private async void OnValidPassCheckClicked(object sender, EventArgs e) //added
{

    SettingsService.MobileId = MobileIdEntry.Text?.Trim() ?? string.Empty;
    SettingsService.Password = passwordEntry.Text?.Trim() ?? string.Empty;

    var mobile = new Mobile
    {
        MobileId = SettingsService.MobileId,
        Password = SettingsService.Password
    };

    ApiEndpoints endpoints = await ApiService.LoadEndpointsAsync();
   _authService = new AuthService();

    var result = await _authService.ValidateMobileAsync(mobile);

    if (result.IsValid)
    {
        await DisplayAlert("Succès", "Connexion réussie !", "OK");
        await Navigation.PushAsync(new MainPage());
    }
    else
    { 
    await DisplayAlert("Error", result.Message, "OK");
    }
}


}