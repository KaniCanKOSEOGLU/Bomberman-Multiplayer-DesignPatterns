using BombermanServer.Models;
using BombermanServer.Data.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace BombermanServer.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<IEnumerable<User>> GetLeaderboardAsync()
        {
            // En çok kazanan ilk 10 kişiyi çek
            return await _context.Users
                                 .OrderByDescending(u => u.Wins)
                                 .Take(10)
                                 .ToListAsync();
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}