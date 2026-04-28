using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Core.Models;
using Core.Services;

namespace UI.ViewModels
{
    public class ClientViewModel : INotifyPropertyChanged
    {
        private readonly ClientServices _clientService;
        private ObservableCollection<Client> _clients = new ObservableCollection<Client>();
        private Client? _selectedClient;
        private string _nomClient    = string.Empty;
        private string _prenomClient = string.Empty;
        private string _telephone    = string.Empty;
        private string _errorMessage = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<Client> Clients
        {
            get => _clients;
            set { _clients = value; OnPropertyChanged(); }
        }

        public Client? SelectedClient
        {
            get => _selectedClient;
            set { _selectedClient = value; OnPropertyChanged(); }
        }

        public string NomClient
        {
            get => _nomClient;
            set { _nomClient = value; OnPropertyChanged(); }
        }

        public string PrenomClient
        {
            get => _prenomClient;
            set { _prenomClient = value; OnPropertyChanged(); }
        }

        public string Telephone
        {
            get => _telephone;
            set { _telephone = value; OnPropertyChanged(); }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public ClientViewModel(ClientServices clientService)
        {
            _clientService = clientService;
        }

        public async Task LoadClients()
        {
            var data = await _clientService.GetAllClient();
            Clients  = new ObservableCollection<Client>(data);
        }

        public async Task SaveClient()
        {
            try
            {
                ErrorMessage = string.Empty;
                await _clientService.AddClient(NomClient, PrenomClient, Telephone);
                await LoadClients();
                ResetForm();
            }
            catch (ArgumentException ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        public async Task RemoveClient(Client client)
        {
            try
            {
                await _clientService.DeleteClient(client);
                Clients.Remove(client);
            }
            catch (ArgumentNullException ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        public void ResetForm()
        {
            NomClient    = string.Empty;
            PrenomClient = string.Empty;
            Telephone    = string.Empty;
            ErrorMessage = string.Empty;
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}