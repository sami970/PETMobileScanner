using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http.Json;

namespace PETScanner
{
    internal class OnhandDataViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<OnhandData>? onhandDataCollection;
        private List<OnhandData> originalData = new(); // Holds original data from API
        private string _itemID;
        private string _password; //added

        public ObservableCollection<OnhandData>? ÓnhandDataCollection
        {
            get { return onhandDataCollection; }
            set
            {
                onhandDataCollection = value;
                OnPropertyChanged(nameof(ÓnhandDataCollection));
            }
        }

        public string ItemID
        {
            get => _itemID;
            set
            {
                _itemID = value;
                OnPropertyChanged(nameof(ItemID));
            }
        }

        public string password //added
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(password));
            }
        }

        public OnhandDataViewModel()
        {
            onhandDataCollection = new ObservableCollection<OnhandData>();
        }

        public async Task GenerateDataAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return;

            try
            {
                using var httpClient = new HttpClient();
                var result = await httpClient.GetFromJsonAsync<List<OnhandData>>(url);

                if (result != null)
                {
                    originalData = result; // store the unfiltered data
                    ÓnhandDataCollection = new ObservableCollection<OnhandData>(originalData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
            }
        }

        // 🔍 Filtering by LocationId, ItemIndex, InventBatchId, InventSerialId
        public void FilterByFields(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                // Reset to full data
                ÓnhandDataCollection = new ObservableCollection<OnhandData>(originalData);
            }
            else
            {
                var filtered = originalData.Where(x =>
                    (!string.IsNullOrEmpty(x.LocationId) && x.LocationId.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(x.ItemIndex) && x.ItemIndex.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(x.InventBatchId) && x.InventBatchId.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(x.InventSerialId) && x.InventSerialId.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                ).ToList();

                ÓnhandDataCollection = new ObservableCollection<OnhandData>(filtered);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

}
