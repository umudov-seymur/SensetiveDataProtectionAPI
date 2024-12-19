using System.Security.Cryptography;
using System.Text;

public interface IEncryptionService
{
    string EncryptData(string plainText);
    string DecryptData(string cipherText);
}

public class EncryptionService : IEncryptionService
{
    private readonly string _key;

    public EncryptionService(IConfiguration config)
    {
        _key = config["EncryptionKey"];
    }

    public string EncryptData(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(_key);
        aes.GenerateIV();

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        {
            using (var writer = new StreamWriter(cs))
            {
                writer.Write(plainText);
            }
        }

        var iv = Convert.ToBase64String(aes.IV);
        var cipherText = Convert.ToBase64String(ms.ToArray());
        return $"{iv}:{cipherText}";
    }

    public string DecryptData(string cipherText)
    {
        var parts = cipherText.Split(':');
        if (parts.Length != 2) throw new ArgumentException("Invalid cipher text format.");

        var iv = Convert.FromBase64String(parts[0]);
        var encryptedText = Convert.FromBase64String(parts[1]);

        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(_key);
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(encryptedText);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var reader = new StreamReader(cs);
        return reader.ReadToEnd();
    }
}