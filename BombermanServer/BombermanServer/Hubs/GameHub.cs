using Microsoft.AspNetCore.SignalR;
using BombermanServer.Models;
using BombermanServer.Data.IRepositories;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System;

namespace BombermanServer.Hubs
{
    public class GameHub : Hub
    {
        private readonly IUserRepository _userRepository;

        public GameHub(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // --- 1. KAYIT OL ---
        public async Task Register(string username, string password)
        {
            var existingUser = await _userRepository.GetUserByUsernameAsync(username);
            if (existingUser != null)
            {
                await Clients.Caller.SendAsync("RegisterFailed", "Bu kullanıcı adı zaten alınmış.");
                return;
            }

            var newUser = new User
            {
                Username = username,
                PasswordHash = password,
                Wins = 0,
                Losses = 0,
                ThemePreference = 0
            };

            await _userRepository.AddUserAsync(newUser);
            await _userRepository.SaveChangesAsync();

            await Clients.Caller.SendAsync("RegisterSuccess", "Kayıt başarılı! Giriş yapabilirsiniz.");
        }

        // --- 2. GİRİŞ YAP (Basit Hali) ---
        public async Task Login(string username, string password)
        {
            // Tekil oturum kontrolünü kaldırdık, veritabanı kilitlenmesin diye.
            var user = await _userRepository.GetUserByUsernameAsync(username);

            if (user != null && user.PasswordHash == password)
            {
                // Sadece İsim ve Puan gönderiyoruz (Tema ve Seed yok, kafa karışıklığı olmasın)
                await Clients.Caller.SendAsync("LoginSuccess", user.Username, user.Wins);
            }
            else
            {
                await Clients.Caller.SendAsync("LoginFailed", "Kullanıcı adı veya şifre hatalı.");
            }
        }

        // --- 3. SKOR TABLOSU ---
        public async Task GetLeaderboard()
        {
            var topPlayers = await _userRepository.GetLeaderboardAsync();

            string[] names = topPlayers.Select(p => p.Username).ToArray();
            int[] wins = topPlayers.Select(p => p.Wins).ToArray();
            int[] losses = topPlayers.Select(p => p.Losses).ToArray();

            await Clients.Caller.SendAsync("ReceiveLeaderboard", names, wins, losses);
        }

        // --- 4. OYUN İÇİ HAREKET ---
        public async Task SendMove(float x, float y)
        {
            await Clients.Others.SendAsync("PlayerMoved", x, y);
        }

        // --- 5. BOMBA KOYMA ---
        public async Task SendBombPlaced(float x, float y, float range)
        {
            await Clients.All.SendAsync("BombPlaced", x, y, range);
        }

        // --- 6. İSTATİSTİKLER ---
        public async Task PlayerWon(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user != null)
            {
                user.Wins++;
                await _userRepository.SaveChangesAsync();
            }
        }

        public async Task PlayerLost(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user != null)
            {
                user.Losses++;
                await _userRepository.SaveChangesAsync();
            }
            await Clients.Others.SendAsync("RemotePlayerDied");
        }
    }
}