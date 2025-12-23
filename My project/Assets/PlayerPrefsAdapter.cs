using UnityEngine;

// ADAPTER PATTERN - Adaptör Sýnýfý
// Unity'nin eski "PlayerPrefs" sistemini, bizim modern "IDataSaver" arayüzümüze dönüþtürür.
public class PlayerPrefsAdapter : IDataSaver
{
    public void SaveString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
        PlayerPrefs.Save(); // Diske yazmayý zorla
    }

    public string GetString(string key)
    {
        // Eðer kayýt yoksa boþ döndür
        return PlayerPrefs.GetString(key, "");
    }
}
