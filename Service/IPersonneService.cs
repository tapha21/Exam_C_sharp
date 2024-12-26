
using ges_commande.Models.entity;

namespace GesDetteWeb.Service; 
public interface IPersonneService{
Task <List<Personne>> GetPaiementsAsync (int detteid);
}