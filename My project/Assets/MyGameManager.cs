using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MyGameManager : MonoBehaviour
{
    [Header("UI Bileþenleri")]
    public GameObject gameOverPanel; // Siyah Panel
    public Text resultText;          // Sonuç Yazýsý

    private void OnEnable()
    {
        GameEventManager.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        GameEventManager.OnPlayerDeath -= HandlePlayerDeath;
    }

    // Biri öldüðünde çalýþýr
    private void HandlePlayerDeath(string deadTag)
    {
        // 1. BÝZ ÖLDÜK
        if (deadTag == "Player")
        {
            ShowGameOver("KAYBETTÝN!", Color.red);

            // Ýstatistik: Kaybetme bilgisini sunucuya gönder
            if (NetworkManager.instance != null) NetworkManager.instance.SendLoss();

            if (AudioManager.instance != null) AudioManager.instance.PlaySFX(AudioManager.instance.lose);
        }
        // 2. RAKÝP ÖLDÜ
        else if (deadTag == "RemotePlayer")
        {
            ShowGameOver("KAZANDIN!", Color.green);

            // Ýstatistik: Kazanma bilgisini sunucuya gönder
            if (NetworkManager.instance != null) NetworkManager.instance.SendWin();

            if (AudioManager.instance != null) AudioManager.instance.PlaySFX(AudioManager.instance.win);
        }
        // 3. DÜÞMAN ÖLDÜ
        else if (deadTag == "Enemy")
        {
            Debug.Log("Bir düþman yok edildi.");
        }
    }

    private void ShowGameOver(string message, Color color)
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        if (resultText != null)
        {
            resultText.text = message;
            resultText.color = color;
        }
    }

    // "Tekrar Oyna" Butonu
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}