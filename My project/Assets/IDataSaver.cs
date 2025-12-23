// ADAPTER PATTERN - Arayüz
// Uygulamanýn veriyi kaydetmek için bildiði tek yöntem budur.
public interface IDataSaver
{
    void SaveString(string key, string value);
    string GetString(string key);
}
