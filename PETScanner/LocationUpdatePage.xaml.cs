namespace PETScanner;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using PETScanner.Model;


public partial class LocationUpdatePage : ContentPage
{
    private OnhandData _selectedItem;
    private string _journalId;
    private string oldLocation;
    private bool ValidatedJournal=false;
    private bool isFirstEdit = true;

    public LocationUpdatePage(OnhandData selectedItem, string journalId)
    {
        InitializeComponent();
        _selectedItem = selectedItem;
        _journalId = journalId;

        ItemIdLabel.Text = selectedItem.ItemId;
        
        LocationEntry.Text = SettingsService.DefaultLocation;
        oldLocation = selectedItem.LocationId;
        
        QtyEntry.Text= selectedItem.QtyPhysical.ToString();
        //WeightLabel.Text = selectedItem.Weight.ToString();
        WeightLabel.Text = (selectedItem.QtyPhysical * selectedItem.Weight).ToString();
    }

    

    private void QtyEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (decimal.TryParse(e.NewTextValue, out decimal qty))
        {

            decimal? Weight_ = _selectedItem.Weight;
            decimal? newWeight = qty;

            WeightLabel.Text = (Weight_ * newWeight).ToString();

        }

        if (isFirstEdit && !string.IsNullOrEmpty(e.NewTextValue))
        {
            var entry = sender as Entry;
            QtyEntry.Text = e.NewTextValue.Length > 0 ? e.NewTextValue[^1].ToString() : string.Empty;
            isFirstEdit = false;
        }
    }

    private void QtyEntry_Focused(object sender, FocusEventArgs e)
    {
        isFirstEdit = true;
    }

    private void QtyEntry_Loaded(object sender, EventArgs e)
    {
        QtyEntry.Focus();
        QtyEntry.Text = _selectedItem.QtyPhysical.ToString(); // Or your desired initial value
        isFirstEdit = true;
    }

    private async void OnQtyEntryCompleted(object sender, EventArgs e)
    {

        string qty = QtyEntry.Text?.Trim();
        OnValidateClicked(sender, e);
    }

    private async void OnValidateClicked(object sender, EventArgs e)
    {
        var newLocation = LocationEntry.Text?.Trim();
        if (string.IsNullOrEmpty(newLocation))
        {
            await DisplayAlert("Validation", "Veuillez saisir le Nouvel emplacement des articles à transférer ", "OK");
            return;
        }

        var newQty = QtyEntry.Text?.Trim();
        if(string.IsNullOrEmpty(newQty))
        {
            
            return;
        }

        if(LocationEntry.Text == oldLocation)
        {
            await DisplayAlert("Validation", "Veuiller saisir un nouvel emplacement different a l'emplacement actuel ", "OK");
            return;
        }

        decimal Qtyvalue = decimal.Parse(newQty); // Converting string to decimal for easier data transmission

        var request = new InventJournalContractcs
        {
          
            itemId = _selectedItem.ItemId,
            journalType = SettingsService.TransferJournalType,
            mobileId = SettingsService.MobileId,
            inventdimId = _selectedItem.InventDimId, // adjust if needed
            journalId = _journalId,
            qty = Qtyvalue,
            wmsLocationId = newLocation

        };

        try
        {
            using var httpClient = new HttpClient();
            var json = JsonSerializer.Serialize(request);
            //var content = new StringContent(json, Encoding.UTF8);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            //  content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");


            ApiEndpoints endpoints = await ApiService.LoadEndpointsAsync();
            var insertJournalLineUrl = endpoints.InsertJournalLineUrl;

            var response = await httpClient.PostAsync(SettingsService.DefaultUrl + SettingsService.DefaultPort + insertJournalLineUrl, content);

            if (response.IsSuccessStatusCode)
            {
               // await DisplayAlert("Success", "Location updated successfully.", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Error", $"Failed to update: {error}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Exception", ex.Message, "OK");
        }
    }
}
