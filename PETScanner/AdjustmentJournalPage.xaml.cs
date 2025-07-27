namespace PETScanner;

using Maui.DataGrid;
using PETScanner.Model;
using System.Collections.ObjectModel;
using System.Net;

public partial class AdjustmentJournalPage : ContentPage
{
    string _journalId;
    
    public AdjustmentJournalPage(string journalId)
    {
        InitializeComponent();
        Shell.SetBackButtonBehavior(this, new BackButtonBehavior
        {
            IsEnabled = false
        });
        JournalIdLabel.Text = journalId?.Replace("\"", "").Trim();
        this._journalId = journalId;
        // BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ItemIdEntry.Text = string.Empty;
        ItemIdEntry.Focus();
        dataGrid.SelectedRow = null;  // Clear the selection

    }

    private void ItemIdEntry_Loaded(object sender, EventArgs e)
    {
        ItemIdEntry.Focus();
    }

    private async void OnItemIdEntryCompleted(object sender, EventArgs e)
    {

        string itemId = ItemIdEntry.Text?.Trim();
        OnItemIdCheckClicked(sender, e);


    }

    private async void OnItemIdCheckClicked(object sender, EventArgs e)
    {
        string itemId = ItemIdEntry.Text?.Trim();
        if (string.IsNullOrEmpty(itemId))
        {
            await DisplayAlert("Error", "Veuillez saisir l'article ID", "OK");
            return;
        }

        ApiEndpoints endpoints = await ApiService.LoadEndpointsAsync();
        var onHandDataUrl = endpoints.OnhandDataUrl;

        string baseUrl = SettingsService.DefaultUrl + SettingsService.DefaultPort + onHandDataUrl;
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            await DisplayAlert("Error", "OnhandDataURL is not set in Setup.", "OK");
            return;
        }

        string url = $"{baseUrl.TrimEnd('/')}/{Uri.EscapeDataString(itemId)}";
        if (BindingContext is OnhandDataViewModel vm)
        {
            // OnhandDataViewModel vm = new OnhandDataViewModel();
            await vm.GenerateDataAsync(url);
        }
    }

    private void OnFilterEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        if (BindingContext is OnhandDataViewModel vm)
        {
            vm.FilterByFields(e.NewTextValue);
        }
    }

    private void OnGridSelectionChanged(object sender, Syncfusion.Maui.DataGrid.DataGridSelectionChangedEventArgs e)
    {
        if (e.AddedRows.Count > 0 && e.AddedRows[0] is OnhandData selectedItem)
        {
            var journalId = JournalIdLabel.Text;
            Navigation.PushAsync(new QtyValidationPage(selectedItem, journalId));
        }
    }

    private void OnCellTapped(object sender, Syncfusion.Maui.DataGrid.DataGridCellTappedEventArgs e)
    {
        if (e.RowData is OnhandData selectedItem)
        {
            var journalId = JournalIdLabel.Text;
            Navigation.PushAsync(new QtyValidationPage(selectedItem, journalId));
        }
    }

    private async void OnAdjustmentJournalPostClicked(object sender, EventArgs e)
    {
        var httpClient = new HttpClient();
        _journalId = _journalId.Replace("\"", "");

        ApiEndpoints endpoints = await ApiService.LoadEndpointsAsync();
        var validateAdjustmentJournalUrl = endpoints.ValidateAdjustmentJournalUrl;

        string baseUri = SettingsService.DefaultUrl + SettingsService.DefaultPort + validateAdjustmentJournalUrl;
        string url = $"{baseUri.TrimEnd('/')}/{Uri.EscapeDataString(_journalId)}";


        try
        {
            HttpResponseMessage response = await httpClient.PostAsync(url, null);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                await DisplayAlert("succès", "Journal ajusté et validé", "OK");
                await Navigation.PushAsync(new MainPage());
            }
            else
            {
                await DisplayAlert("Error", $"API Error: {response.StatusCode}", "OK");
            }
        }
        catch (HttpRequestException ex)
        {
            await DisplayAlert("Connection Error", ex.Message, "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Unexpected Error", ex.Message, "OK");
        }
    }

    private async void OnCancelAdjustmentJournalPostClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

}


    