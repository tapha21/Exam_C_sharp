using ges_commande.Models.entity;
using GesCommande.core;
using GesDetteWeb.Service;
using Microsoft.EntityFrameworkCore;

namespace Gesdette.Service{
    public class PersonneService : IPersonneService
    {
         private readonly Context context;

        public Task<List<Personne>> GetPaiementsAsync(int detteid)
        {
            throw new NotImplementedException();
        }
    }
}