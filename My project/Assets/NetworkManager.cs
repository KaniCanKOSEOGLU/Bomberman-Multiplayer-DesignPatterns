using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    [Header("Giriþ Ekraný UI")]
    public InputField usernameInput;
    public InputField passwordInput;
    public Text statusText;
    public GameObject loginPanel;
    public GameObject themePanel;

    [Header("Leaderboard UI")]
    public GameObject leaderboardPanel;
    public Text leaderboardListText;

    [Header("Fabrikalar ve Objeler")]
    public BombFactory bombFactory;
    public GameObject remotePlayer;

    private HubConnection connection;
    private IDataSaver dataSaver;

    // Thread Deðiþkenleri
    private string pendingStatusMessage = null;
    private string pendingLeaderboardData = null;
    private bool loginSuccessful = false;

    private Vector2 targetPosition = Vector2.zero;
    private bool shouldUpdateRemote = false;

    private Queue<Vector2> bombSpawnQueue = new Queue<Vector2>();

    private void Awake()
    {
        instance = this;
    }

    async void Start()
    {
        // Temizlik
        if (connection != null)
        {
            await connection.StopAsync();
            await connection.DisposeAsync();
            connection = null;
        }

        System.Net.ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;

        if (statusText) statusText.text = "Sunucuya baðlanýlýyor...";

        dataSaver = new PlayerPrefsAdapter();
        string savedName = dataSaver.GetString("SavedUsername");
        if (!string.IsNullOrEmpty(savedName) && usernameInput != null)
        {
            usernameInput.text = savedName;
        }

        // Baðlantý
        connection = new HubConnectionBuilder()
            .WithUrl("http://127.0.0.1:5049/gameHub")
            .Build();

        // --- DÝNLEYÝCÝLER ---
        connection.On<string>("RegisterSuccess", (msg) => pendingStatusMessage = "Kayýt Baþarýlý! Giriþ yapýn.");
        connection.On<string>("RegisterFailed", (msg) => pendingStatusMessage = "Kayýt Hatasý: " + msg);

        // Basit Login (Sadece Ýsim ve Puan)
        connection.On<string, int>("LoginSuccess", (user, wins) => {
            pendingStatusMessage = $"Hoþgeldin {user}!";
            loginSuccessful = true;
        });

        connection.On<string>("LoginFailed", (msg) => pendingStatusMessage = "Giriþ Hatasý: " + msg);

        connection.On<string[], int[], int[]>("ReceiveLeaderboard", (names, wins, losses) =>
        {
            string finalText = "";
            for (int i = 0; i < names.Length; i++)
            {
                int total = wins[i] + losses[i];
                finalText += $"{i + 1}. {names[i]} : {wins[i]} W - {losses[i]} L (T:{total})\n";
            }
            pendingLeaderboardData = finalText;
        });

        connection.On("RemotePlayerDied", () =>
        {
            GameEventManager.TriggerPlayerDeath("RemotePlayer");
        });

        connection.On<float, float>("PlayerMoved", (x, y) =>
        {
            targetPosition = new Vector2(x, y);
            shouldUpdateRemote = true;
        });

        connection.On<float, float, float>("BombPlaced", (x, y, range) =>
        {
            if (bombFactory != null) bombFactory.nextBombRange = range;
            lock (bombSpawnQueue)
            {
                bombSpawnQueue.Enqueue(new Vector2(x, y));
            }
        });

        try
        {
            await connection.StartAsync();
            if (statusText) statusText.text = "Sunucuya Baðlandý. Giriþ Yapýn.";
        }
        catch (Exception ex)
        {
            if (statusText) statusText.text = "Baðlantý Hatasý!";
            Debug.LogError($"Hata: {ex.Message}");
        }
    }

    private async void OnDestroy()
    {
        if (connection != null)
        {
            try { await connection.StopAsync(); await connection.DisposeAsync(); } catch { }
        }
    }

    void Update()
    {
        if (!string.IsNullOrEmpty(pendingStatusMessage))
        {
            if (statusText) statusText.text = pendingStatusMessage;
            pendingStatusMessage = null;
        }

        if (pendingLeaderboardData != null)
        {
            if (leaderboardListText) leaderboardListText.text = pendingLeaderboardData;
            if (leaderboardPanel) leaderboardPanel.SetActive(true);
            pendingLeaderboardData = null;
        }

        if (loginSuccessful)
        {
            loginSuccessful = false;
            if (loginPanel != null) loginPanel.SetActive(false);
            if (themePanel != null) themePanel.SetActive(false); // Butonlarý da kapat

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                var controller = player.GetComponent<PlayerController>();
                if (controller != null) controller.enabled = true;
            }
        }

        if (shouldUpdateRemote && remotePlayer != null)
        {
            remotePlayer.transform.position = Vector2.Lerp(remotePlayer.transform.position, targetPosition, Time.deltaTime * 10);
        }

        while (bombSpawnQueue.Count > 0)
        {
            Vector2 pos;
            lock (bombSpawnQueue)
            {
                pos = bombSpawnQueue.Dequeue();
            }
            if (bombFactory != null) bombFactory.CreateBomb(pos);
        }
    }

    // --- BUTON FONKSÝYONLARI ---

    public async void OnRegisterClicked()
    {
        if (connection != null && connection.State == HubConnectionState.Connected)
        {
            if (dataSaver != null) dataSaver.SaveString("SavedUsername", usernameInput.text);
            statusText.text = "Kayýt olunuyor...";
            await connection.InvokeAsync("Register", usernameInput.text, passwordInput.text);
        }
    }

    public async void OnLoginClicked()
    {
        if (connection != null && connection.State == HubConnectionState.Connected)
        {
            if (dataSaver != null) dataSaver.SaveString("SavedUsername", usernameInput.text);
            statusText.text = "Giriþ yapýlýyor...";
            await connection.InvokeAsync("Login", usernameInput.text, passwordInput.text);
        }
    }

    public async void OnOpenLeaderboardClicked()
    {
        if (connection != null && connection.State == HubConnectionState.Connected)
        {
            statusText.text = "Liste çekiliyor...";
            await connection.InvokeAsync("GetLeaderboard");
        }
    }

    public void OnCloseLeaderboardClicked()
    {
        if (leaderboardPanel) leaderboardPanel.SetActive(false);
    }

    public void OnExitClicked()
    {
        Application.Quit();
    }

    public async void SendPlayerMove(float x, float y)
    {
        if (connection == null || connection.State != HubConnectionState.Connected) return;
        await connection.InvokeAsync("SendMove", x, y);
    }

    public async void SendBomb(Vector2 pos, float range)
    {
        if (connection != null && connection.State == HubConnectionState.Connected)
            await connection.InvokeAsync("SendBombPlaced", pos.x, pos.y, range);
    }

    public async void SendWin()
    {
        if (connection != null && connection.State == HubConnectionState.Connected)
        {
            string myName = dataSaver.GetString("SavedUsername");
            if (!string.IsNullOrEmpty(myName)) await connection.InvokeAsync("PlayerWon", myName);
        }
    }

    public async void SendLoss()
    {
        if (connection != null && connection.State == HubConnectionState.Connected)
        {
            string myName = dataSaver.GetString("SavedUsername");
            if (!string.IsNullOrEmpty(myName)) await connection.InvokeAsync("PlayerLost", myName);
        }
    }
}