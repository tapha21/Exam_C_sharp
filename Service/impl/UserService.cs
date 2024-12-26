using ges_commande.Models.Enum;
using ges_commande.Models.entity;
using GesCommande.core;
using GesDetteWeb.Service;
using Microsoft.EntityFrameworkCore;

namespace Gesdette.Service{
    public class UserService : IUserService
    {
        private readonly Context context;

        public UserService(Context context) {
            this.context = context;
        }

       public async Task<List<User>> GetAllUsers()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<List<User>> GetRoleByID(int roleID)
        {
            return await context.Users
                .Where(u => u.role.Equals((Role)roleID)) 
                .ToListAsync();
        }

        public async Task<List<User>> GetUsersByRoleAsync(string role)
        {
             return await context.Users
            .Where(u => u.role.ToString() == role) 
            .ToListAsync();
        }

    }
}