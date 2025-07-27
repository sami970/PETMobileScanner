using PETScanner.Model;

namespace PETScanner
{
    public partial class MainPage : ContentPage
    {
        
        AuthService _authService;

        public MainPage()
        {
            InitializeComponent();
        }
        
     
        private async void OnTransferJournalClicked(object sender, EventArgs e)
        {

            ApiEndpoints endpoints = await ApiService.LoadEndpointsAsync();
            var createJournalUrl = endpoints.CreateJournalUrl;

            string url = SettingsService.DefaultUrl + SettingsService.DefaultPort + createJournalUrl;
            
            string checkpass = SettingsService.Password;
            if (string.IsNullOrWhiteSpace(checkpass))
            {
                await DisplayAlert("Mot de passe manquant", "Veuillez saisir le mot de passe associé au mobile utilisé au page de paramètres", "OK");
                await Navigation.PushAsync(new SetupPage());
                return;
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                await DisplayAlert("Error", "CreateJournalURL is not set in Setup.", "OK");
                await Navigation.PushAsync(new SetupPage());
                return;
            }

            var mobile = new Mobile
            {
                MobileId = SettingsService.MobileId,
                Password = SettingsService.Password
            };

            
            _authService = new AuthService();
            var result = await _authService.ValidateMobileAsync(mobile);
            var requestBody = new
            {
                journalType = SettingsService.TransferJournalType,
                mobileId = SettingsService.MobileId,
                password = SettingsService.Password

            };

            if (result.IsValid)
            {
                //await DisplayAlert("Connexion réussie !", "Bienvenue dans le Journal de transfert", "OK");

                try
                {
                    string journalId = await ApiService.PostJsonAsync(url, requestBody);
                    await Navigation.PushAsync(new TransferJournalPage(journalId));
                }
                catch (Exception ex)
                {
                    await DisplayAlert("API Error", ex.Message, "OK");
                }


            }
            else
            {
                await DisplayAlert("Error", result.Message, "OK");
                await Navigation.PushAsync(new SetupPage());
            }
        }



        private async void OnSetupClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SetupPage());
        }

        private async void MenueAdjustmentJournalClicked(object sender, EventArgs e)
        {
            ApiEndpoints endpoints = await ApiService.LoadEndpointsAsync();
            var createJournalUrl = endpoints.CreateJournalUrl;

            string url = SettingsService.DefaultUrl + SettingsService.DefaultPort + createJournalUrl;


            string checkpass = SettingsService.DefaultUrl + SettingsService.DefaultPort + SettingsService.Password;
            if (string.IsNullOrWhiteSpace(checkpass))
            {
                await DisplayAlert("Mot de passe manquant", "Veuillez saisir le Mot de passe associé au mobile utilisé au page de paramètres", "OK");
                await Navigation.PushAsync(new SetupPage());
                return;
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                await DisplayAlert("Error", "CreateJournalURL is not set in Setup.", "OK");
                await Navigation.PushAsync(new SetupPage());
                return;
            }

            var mobile = new Mobile
            {
                MobileId = SettingsService.MobileId,
                Password = SettingsService.Password
            };

            _authService = new AuthService();
            var result = await _authService.ValidateMobileAsync(mobile);

            var requestBody = new
            {
                journalType = SettingsService.AdjustmentJournalType,
                mobileId = SettingsService.MobileId,
                password = SettingsService.Password
            };

            if (result.IsValid)
            { 
            //await DisplayAlert("Connexion réussie !", "Bienvenue dans l'ajustement du journal", "OK");
                
                try
                {
                    string journalId = await ApiService.PostJsonAsync(url, requestBody);
                    await Navigation.PushAsync(new AdjustmentJournalPage(journalId));
                }
                catch (Exception ex)
                {
                    await DisplayAlert("API Error", ex.Message, "OK");
                }


            }
            else
            { 
                await DisplayAlert("Error", result.Message, "OK");
                await Navigation.PushAsync(new SetupPage());
            }

        }
    }

}
