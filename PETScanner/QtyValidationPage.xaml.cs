namespace PETScanner;
using PETScanner.Model;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public partial class QtyValidationPage : ContentPage
{
    private OnhandData selectedItem;
    private string journalId;
    private bool ValidatedJournal = false;

    public QtyValidationPage(OnhandData data, string journalId)
    {
        InitializeComponent();
        
        selectedItem = data;
        this.journalId = journalId;

        // Populate UI
        ItemIdLabel.Text = data.ItemId;
        TheoreticalQtyLabel.Text = data.QtyPhysical.ToString();
       
    }

    

    private void QtyEntry_Loaded(object sender, EventArgs e)
    {
        PhysicalQtyEntry.Focus();
    }

    private async void OnQtyEntryCompleted(object sender, EventArgs e)
    {

        string qty = PhysicalQtyEntry.Text?.Trim();
        OnValidateClicked(sender, e);
    }

    private async void OnValidateClicked(object sender, EventArgs e)
    {
        if (!decimal.TryParse(PhysicalQtyEntry.Text, out decimal physicalQty))
        {
            await DisplayAlert("Error", "Veuillez saisir la quantité physique disponible en stock", "OK");
            return;
        }

        var request = new InventJournalContractcs
        {
            itemId = selectedItem.ItemId,
            journalType = SettingsService.AdjustmentJournalType,
            mobileId = SettingsService.MobileId,
            inventdimId = selectedItem.InventDimId, // adjust if needed
            journalId = journalId,
            qty = physicalQty,
            wmsLocationId = selectedItem.LocationId
        };

        try
        {
            using var httpClient = new HttpClient();
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            ApiEndpoints endpoints = await ApiService.LoadEndpointsAsync();
            var insertJournalLineUrl = endpoints.InsertJournalLineUrl;

            var response = await httpClient.PostAsync(SettingsService.DefaultUrl + SettingsService.DefaultPort + insertJournalLineUrl, content);

            if (response.IsSuccessStatusCode)
            {
               // await DisplayAlert("Success", "Line submitted successfully.", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                await DisplayAlert("API Error", $"Failed to submit: {error}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Exception: {ex.Message}", "OK");
        }
        
    }

}

