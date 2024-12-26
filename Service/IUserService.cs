
using ges_commande.Models.entity;

namespace GesDetteWeb.Service; 
public interface IUserService {
  Task<List<User>> GetRoleByID(int roleID);
  Task<List<User>> GetAllUsers();
    Task<List<User>> GetUsersByRoleAsync(string role);


}