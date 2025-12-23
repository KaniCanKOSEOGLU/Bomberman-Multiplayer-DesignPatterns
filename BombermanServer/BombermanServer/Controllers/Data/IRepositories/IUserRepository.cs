using BombermanServer.Models;

namespace BombermanServer.Data.IRepositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task<IEnumerable<User>> GetLeaderboardAsync();
        Task AddUserAsync(User user);
        Task SaveChangesAsync();
        // UpdateUserStatsAsync gibi metotlara gerek kalmadı,
        // çünkü EF Core değişiklikleri SaveChangesAsync ile otomatik takip ediyor.
    }
}