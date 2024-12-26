using ges_commande.Models.entity;
using GesCommande.core;
using GesDetteWeb.Service;
using Microsoft.EntityFrameworkCore;

namespace Gesdette.Service{
    public class ClientService : IClientService{
        private readonly Context context;

        public ClientService(Context context) {
            this.context = context;
        }

        public async Task<Client> GetClientByIdAsync(int id)
        {
            return await context.Clients
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Client> GetClientByPhoneNumberAsync(string telephone)
        {
           return await context.Clients
                .FirstOrDefaultAsync(c => c.Telephone == telephone);
        }

        public async Task<List<Client>> GetClientsAsync()
        {
            return await context.Clients.ToListAsync();
        }
    }
}