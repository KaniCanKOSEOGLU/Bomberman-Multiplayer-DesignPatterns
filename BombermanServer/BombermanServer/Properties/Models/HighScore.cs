using System.ComponentModel.DataAnnotations;

namespace BombermanServer.Models
{
    public class HighScore
    {
        [Key]
        public int Id { get; set; }

        public string? Username { get; set; }
        public int Score { get; set; }
        public DateTime Date { get; set; }
    }
}
