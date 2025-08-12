using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces;

public interface IBase64ByteConverter
{
    string BytesToBase64(byte[] bytes);
    byte[] Base64ToBytes(string base64String);

}
