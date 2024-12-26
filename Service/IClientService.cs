

using ges_commande.Models.entity;

namespace GesDetteWeb.Service; 
public interface IClientService{
   Task<List<Client>> GetClientsAsync();
    Task<Client> GetClientByIdAsync(int id);
        Task<Client> GetClientByPhoneNumberAsync(string phoneNumber);

}