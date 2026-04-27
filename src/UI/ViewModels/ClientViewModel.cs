using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Core.Models;
using Core.Interfaces;

namespace UI.ViewModels
{
    public class ClientViewModel : INotifyPropertyChanged{
        private readonly IClientRepository _clientService;
        private ObservableCollection<Client> _clients = new ObservableCollection<Client>();
        private Client? _selectedClient;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<Client> Clients{
            get => _clients;
            set{
                _clients = value;
                OnPropertyChanged();
            }
        }

        public Client? SelectedClient
        {
            get => _selectedClient;
            set{
                _selectedClient = value;
                OnPropertyChanged();
            }
        }

        public ClientViewModel(IClientRepository clientService){
            _clientService = clientService;
            // Ne pas charger automatiquement dans le constructeur - laisser la vue gérer cela
        }

        public async Task LoadClients(){
            var data = await _clientService.GetAllClient();
            Clients = new ObservableCollection<Client>(data);
        }

        public async Task SaveClient(Client client)
        {
            if (client.RefClient == 0){
                await _clientService.AddClient(client);
            }
            else{
                await _clientService.UpdateClient(client);
            }
            await LoadClients();
        }

        public async Task RemoveClient(Client client){
            await _clientService.DeleteClient(client);
            Clients.Remove(client);
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null){
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}