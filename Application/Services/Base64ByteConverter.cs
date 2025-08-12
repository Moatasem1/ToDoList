using Application.Services.Interfaces;

namespace Application.Services;

public class Base64ByteConverter : IBase64ByteConverter
{
    public byte[] Base64ToBytes(string base64String)
    {
        return string.IsNullOrEmpty(base64String) ? throw new ArgumentNullException(nameof(base64String)) : Convert.FromBase64String(base64String);
    }

    public string BytesToBase64(byte[] bytes)
    {
        return bytes == null ? throw new ArgumentNullException(nameof(bytes)) : Convert.ToBase64String(bytes);
    }
}