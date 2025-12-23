using System.ComponentModel.DataAnnotations;

namespace BombermanServer.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string? Username { get; set; }
        public string? PasswordHash { get; set; }

        public int Wins { get; set; }
        public int Losses { get; set; }

        // Tema tercihi (0: Çöl, 1: Şehir, 2: Orman)
        public int ThemePreference { get; set; } = 0;
    }
}