using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Ses Dosyalarý (Audio Clips)")]
    public AudioClip bgMusic;       // Arka Plan Müziði
    public AudioClip bombPlace;     // Bomba Koyma
    public AudioClip explosion;     // Patlama
    public AudioClip powerUp;       // Ýksir Alma
    public AudioClip win;           // Kazanma
    public AudioClip lose;          // Kaybetme

    private AudioSource musicSource; // Müzik çalar
    private AudioSource sfxSource;   // Efekt çalar

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // AudioSource bileþenlerini kodla ekliyoruz
            musicSource = gameObject.AddComponent<AudioSource>();
            sfxSource = gameObject.AddComponent<AudioSource>();

            // Müzik ayarlarý
            musicSource.loop = true;
            musicSource.volume = 0.5f;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Oyun baþlayýnca müziði çal
        PlayMusic(bgMusic);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip != null)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }

    // Efekt çalma fonksiyonu (Tek seferlik sesler)
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}