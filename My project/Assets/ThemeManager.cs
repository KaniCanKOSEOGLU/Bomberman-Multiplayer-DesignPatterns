using UnityEngine;

public class ThemeManager : MonoBehaviour
{
    [Header("Baðlantýlar")]
    public MapGenerator mapGenerator;

    [Header("Fabrika Objeleri")]
    public GameObject desertFactoryObj; // 0
    public GameObject cityFactoryObj;   // 1
    public GameObject forestFactoryObj; // 2

    void Start()
    {
        // Hafýzadan son seçilen temayý alýr (Lokal)
        int savedThemeID = PlayerPrefs.GetInt("LocalThemeID", 0);
        SetTheme(savedThemeID, false);
    }

    // Butonlar bu fonksiyonu çaðýrýr
    public void SetThemeFromButton(int themeID)
    {
        SetTheme(themeID, true);
    }

    public void SetTheme(int themeID, bool saveToServer)
    {
        // 1. Fabrikayý Seç
        switch (themeID)
        {
            case 0:
                mapGenerator.factoryObject = desertFactoryObj;
                break;
            case 1:
                mapGenerator.factoryObject = cityFactoryObj;
                break;
            case 2:
                if (forestFactoryObj != null)
                    mapGenerator.factoryObject = forestFactoryObj;
                break;
        }

        // 2. Haritayý Oluþturma (Seed yok, düz oluþtur)
        mapGenerator.GenerateMap();

        // 3. Sadece Cihaza Kaydet (Sunucuya göndermez)
        PlayerPrefs.SetInt("LocalThemeID", themeID);
        PlayerPrefs.Save();

    }
}