

using System.Net.Http;
using System.Threading.Tasks;
using WalletService.Application.Models.GoogleModel;

namespace WalletService.Application.Contracts.Infrastructure
{
    public interface IGoogleRecaptchaV3Service
    {
        HttpClient _httpClient { get; set; }
        GRequestModel Request { get; set; }
        GResponseModel Response { get; set; }
        void InitializeRequest(GRequestModel request);
        Task<bool> Execute();
    }
}
