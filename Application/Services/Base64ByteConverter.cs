using Application.Services.Interfaces;
using Microsoft.AspNetCore.StaticFiles;

namespace Application.Services;

public class Base64ByteConverter : IBase64ByteConverter
{
    public byte[] Base64ToBytes(string base64String)
    {
        var base64Data = base64String.Contains(',') ? base64String.Substring(base64String.IndexOf(",") + 1) : base64String;

        return string.IsNullOrEmpty(base64Data) ? throw new ArgumentNullException(nameof(base64String)) : Convert.FromBase64String(base64Data);
    }

    public string BytesToBase64(byte[] bytes)
    {
        return bytes == null ? throw new ArgumentNullException(nameof(bytes)) : $"data:{GetImageMimeType(bytes)};base64,{Convert.ToBase64String(bytes)}"; ;
    }

    private static string GetImageMimeType(byte[] bytes)
    {
        if (bytes == null || bytes.Length < 4)
            return "application/octet-stream";

        if (bytes[0] == 0x89 && bytes[1] == 0x50 &&
            bytes[2] == 0x4E && bytes[3] == 0x47)
            return "image/png";

        if (bytes[0] == 0xFF && bytes[1] == 0xD8)
            return "image/jpeg";

        if (bytes[0] == 0x47 && bytes[1] == 0x49 &&
            bytes[2] == 0x46 && bytes[3] == 0x38)
            return "image/gif";

        if (bytes[0] == 0x42 && bytes[1] == 0x4D)
            return "image/bmp";

        if ((bytes[0] == 0x49 && bytes[1] == 0x49) ||
            (bytes[0] == 0x4D && bytes[1] == 0x4D))
            return "image/tiff";

        return "application/octet-stream"; 
    }

}