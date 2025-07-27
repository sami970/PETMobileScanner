using PETScanner.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;



namespace PETScanner.ViewModel
{
    public class UserDataViewModel : INotifyPropertyChanged
    {
        private string _typeJournalType;
        private string _typeAjustmentType;
        private string _emplacementParDefaut;
        private string _urlParDefaut;
        private string _port;

        private readonly JsonFileService _jsonService = new JsonFileService();

        public string TypeJournalType
        {
            get => _typeJournalType;
            set
            {
                _typeJournalType = value;
                OnPropertyChanged();
            }
        }

        public string TypeAjustmentType
        {
            get => _typeAjustmentType;
            set
            {
                _typeAjustmentType = value;
                OnPropertyChanged();
            }
        }

        public string EmplacementParDefaut
        {
            get => _emplacementParDefaut;
            set
            {
                _emplacementParDefaut = value;
                OnPropertyChanged();
            }
        }

        public string UrlParDefaut
        {
            get => _urlParDefaut;
            set
            {
                _urlParDefaut = value;
                OnPropertyChanged();
            }
        }

        public string Port
        {
            get => _port;
            set
            {
                _port = value;
                OnPropertyChanged();
            }
        }

        public UserDataViewModel()
        {
            _jsonService = new JsonFileService();
        }

        public async Task LoadDataAsync()
        {

            var data = await _jsonService.LoadUserDataAsync();
            if (data != null)
            {
                TypeJournalType = data.TypeJournalType;
                TypeAjustmentType = data.TypeAjustmentType;
                EmplacementParDefaut = data.EmplacementParDefaut;
                UrlParDefaut = data.UrlParDefaut;
                Port = data.Port;

            }



        }

        public async Task SaveDataAsync()
        {
            var data = new UserInputData { TypeJournalType = TypeJournalType, TypeAjustmentType = TypeAjustmentType,
                EmplacementParDefaut = EmplacementParDefaut,
                UrlParDefaut= UrlParDefaut, Port = Port };

            await _jsonService.SaveDataAsync(data);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
