using System.Text.Json;
using WalletAPIService;

namespace WalletService.API.Helper
{
    public static class DeserializeHelper
    {
        public static T DeserializeMethod<T> (string data)
        {
            return JsonSerializer.Deserialize<T>(data);
        }
    }
}
